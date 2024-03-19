terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
    }
  }
}
locals {
  retention_in_days = 30
}

// https://stackoverflow.com/questions/73857873/use-terraform-to-add-a-vm-to-the-new-azure-monitoring-without-oms-agent
resource "azurerm_log_analytics_workspace" "insight" {
  name                = "${var.resource_name_prefix}-log-analytics-workspace"
  location            = var.resource_group.location
  resource_group_name = var.resource_group.name
  sku                 = "PerGB2018"
  retention_in_days   = local.retention_in_days
}

resource "azurerm_monitor_data_collection_rule" "insight" {
  name                = "${var.resource_name_prefix}-monitor_data_collection_rule"
  resource_group_name = var.resource_group.name
  location            = var.resource_group.location

  destinations {
    log_analytics {
      workspace_resource_id = azurerm_log_analytics_workspace.insight.id
      name                  = "${var.resource_name_prefix}-destination-log"
    }

    azure_monitor_metrics {
      name = "${var.resource_name_prefix}-destination-metrics"
    }
  }

  data_flow {
    streams      = ["Microsoft-Syslog", "Microsoft-Perf"]
    destinations = ["${var.resource_name_prefix}-destination-log"]
  }

  data_flow {
    streams      = ["Microsoft-InsightsMetrics"]
    destinations = ["${var.resource_name_prefix}-destination-metrics"]
  }

  data_sources {
    // TODO FIGURE THIS OUT
    /*performance_counter {
      streams                       = ["Microsoft-InsightsMetrics"]
      sampling_frequency_in_seconds = 60
      counter_specifiers            = ["\\VmInsights\\DetailedMetrics"]
      name                          = "VMInsightsPerfCounters"
    }*/
  }
}

resource "azurerm_log_analytics_solution" "insight-app" {
  solution_name         = "ApplicationInsights"
  location              = var.resource_group.location
  resource_group_name   = var.resource_group.name
  workspace_resource_id = azurerm_log_analytics_workspace.insight.id
  workspace_name        = azurerm_log_analytics_workspace.insight.name
  plan {
    publisher = "Microsoft"
    product   = "OMSGallery/ApplicationInsights"
  }
}

/*
TODO:  WHAT THEN  ???
Error: checking for the presence of an existing Scoped Data Collection Rule Association (Resource Uri: "/subscriptions/6963cabb-d678-4e48-be5f-a3dc901b7b73/resourceGroups/devops-insight-resources/providers/Microsoft.Web/sites/devops-insight-app"
│ Data Collection Rule Association Name: "devops-insight-web-app-monitor-data-collection-rule-association"): datacollectionruleassociations.DataCollectionRuleAssociationsClient#Get: Failure responding to request: StatusCode=400 -- Original Error: autorest/azure: Service returned an error. Status=400 Code="UnsupportedResourceType" Message="Association cannot be created for resource of type 'Microsoft.Web/sites'. Supported types are: Microsoft.AzureStackHCI/virtualmachines,Microsoft.AzureStackHCI/clusters,Microsoft.Compute/virtualMachineScaleSets,Microsoft.Cache/redis,Microsoft.Compute/virtualMachines,Microsoft.ConnectedVMwarevSphere/VirtualMachines,Microsoft.ContainerService/managedClusters,Microsoft.Devices/IotHubs,Microsoft.EventHub/namespaces/eventhubs,Microsoft.HybridCompute/machines,Microsoft.HybridContainerService/ProvisionedClusters,Microsoft.Insights/monitoredObjects,Microsoft.KeyVault/vaults,Microsoft.Kubernetes/connectedClusters,Microsoft.Storage/storageAccounts,Microsoft.Storage/storageAccounts/blobServices,Microsoft.Storage/storageAccounts/fileServices,Microsoft.Storage/storageAccounts/queueServices,Microsoft.Storage/storageAccounts/tableServices"
│
│   with module.environment.module.application_insights.azurerm_monitor_data_collection_rule_association.monitor_data_collection_rule_association,
│   on ../../modules/application_insights/main.tf line 58, in resource "azurerm_monitor_data_collection_rule_association" "monitor_data_collection_rule_association":
│   58: resource "azurerm_monitor_data_collection_rule_association" "monitor_data_collection_rule_association" {
│
│ checking for the presence of an existing Scoped Data Collection Rule Association (Resource Uri:
│ "/subscriptions/6963cabb-d678-4e48-be5f-a3dc901b7b73/resourceGroups/devops-insight-resources/providers/Microsoft.Web/sites/devops-insight-app"
│ Data Collection Rule Association Name: "devops-insight-web-app-monitor-data-collection-rule-association"):
│ datacollectionruleassociations.DataCollectionRuleAssociationsClient#Get: Failure responding to request: StatusCode=400 -- Original Error: autorest/azure: Service returned an
│ error. Status=400 Code="UnsupportedResourceType" Message="Association cannot be created for resource of type 'Microsoft.Web/sites'. Supported types are:
│ Microsoft.AzureStackHCI/virtualmachines,Microsoft.AzureStackHCI/clusters,Microsoft.Compute/virtualMachineScaleSets,Microsoft.Cache/redis,Microsoft.Compute/virtualMachines,Microsoft.ConnectedVMwarevSphere/VirtualMachines,Microsoft.ContainerService/managedClusters,Microsoft.Devices/IotHubs,Microsoft.EventHub/namespaces/eventhubs,Microsoft.HybridCompute/machines,Microsoft.HybridContainerService/ProvisionedClusters,Microsoft.Insights/monitoredObjects,Microsoft.KeyVault/vaults,Microsoft.Kubernetes/connectedClusters,Microsoft.Storage/storageAccounts,Microsoft.Storage/storageAccounts/blobServices,Microsoft.Storage/storageAccounts/fileServices,Microsoft.Storage/storageAccounts/queueServices,Microsoft.Storage/storageAccounts/tableServices"
resource "azurerm_monitor_data_collection_rule_association" "monitor_data_collection_rule_association" {
  name                    = "${var.resource_name_prefix}-web-app-monitor-data-collection-rule-association"
  target_resource_id      = var.web_app_target_resource_id
  data_collection_rule_id = azurerm_monitor_data_collection_rule.monitor_data_collection_rule.id
  description             = "Web App Rule Association"
}
*/


resource "azurerm_application_insights" "insight" {
  name                = "${var.resource_name_prefix}-application-insights"
  location            = var.resource_group.location
  resource_group_name = var.resource_group.name
  application_type    = "web"
  retention_in_days   = local.retention_in_days
  # This is just a work-around that extends legacy application insights from expiring 02-2024 to 08-2024 TODO Use azurerm_monitor_data_collection_rule
  workspace_id        = azurerm_log_analytics_workspace.insight.id
}

// This will be auto-created by Azure, and terraform will fail...
// TODO: missing the two default actions...
resource "azurerm_monitor_action_group" "insight" {
  name                = "Application Insights Smart Detection"
  resource_group_name = var.resource_group.name
  short_name          = "SmartDetect"
}

resource "azurerm_monitor_smart_detector_alert_rule" "insight" {
  name                = "Failure Anomalies - ${azurerm_application_insights.insight.name}"
  description         = "Failure Anomalies notifies you of an unusual rise in the rate of failed HTTP requests or dependency calls."
  resource_group_name = var.resource_group.name
  detector_type       = "FailureAnomaliesDetector"
  scope_resource_ids  = [azurerm_application_insights.insight.id]
  severity            = "Sev3"
  frequency           = "PT1M"
  action_group {
    ids = [azurerm_monitor_action_group.insight.id]
  }
}