terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
    }
    random = {
      source = "hashicorp/random"
    }
  }
}

locals {
  current_stack          = "dotnet"
  health_check_path      = "/health"
  https_only             = true
  vnet_route_all_enabled = true
}


resource "azurerm_service_plan" "plan" {
  name                = "${var.resource_name_prefix}-app-plan"
  resource_group_name = var.resource_group.name
  location            = var.resource_group.location

  os_type  = "Windows"
  sku_name = var.web_app_sku_name

  tags = var.common_tags
}

resource "azurerm_windows_web_app" "web_app" {
  name                = "${var.resource_name_prefix}-app"
  resource_group_name = var.resource_group.name
  location            = var.resource_group.location
  service_plan_id     = azurerm_service_plan.plan.id
  https_only          = local.https_only

  virtual_network_subnet_id = var.web_app_subnet_id

  site_config {
    application_stack {
      current_stack  = local.current_stack
      dotnet_version = var.web_app_dotnet_version
    }
    use_32_bit_worker      = var.web_app_use_32_bit_worker
    health_check_path      = local.health_check_path
    vnet_route_all_enabled = local.vnet_route_all_enabled
  }

  app_settings = {
    ApplicationInsights__ConnectionString = var.application_insights_connection_string
    Marten__ConnectionString              = var.postgresql_connection_string
    Marten__PopulateWithDemoData          = "false"
    Marten__SchemaName                    = "public"
    Marten__ShouldRecreateDatabase        = "false"
    PdfGenerator__BrowserWsEndpoint       = var.browserless_connection_string

    // Enable Legacy Applications Insights: EOL 08/2024
    APPINSIGHTS_INSTRUMENTATIONKEY        = var.application_insights_instrumentation_key
    APPLICATIONINSIGHTS_CONNECTION_STRING = var.application_insights_connection_string
  }

  tags = var.common_tags
}

resource "azurerm_windows_web_app_slot" "slot" {
  name           = "pre-${var.environment_name}"
  app_service_id = azurerm_windows_web_app.web_app.id
  https_only     = local.https_only

  virtual_network_subnet_id = azurerm_windows_web_app.web_app.virtual_network_subnet_id

  site_config {
    application_stack {
      current_stack  = local.current_stack
      dotnet_version = var.web_app_dotnet_version
    }
    use_32_bit_worker      = var.web_app_use_32_bit_worker
    health_check_path      = local.health_check_path
    vnet_route_all_enabled = local.vnet_route_all_enabled
  }

  app_settings = merge(azurerm_windows_web_app.web_app.app_settings, {})

  tags = var.common_tags
}