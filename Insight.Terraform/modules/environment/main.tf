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
  resource_name_prefix = "${var.environment_name}-${var.project_name}"

  postgres_admin_user_connection_string_key       = "Postgres-Admin-Connection-String"
  postgres_insight_connection_string_key          = "Postgres-Insight-Connection-String"
  application_insights_user_connection_string_key = "Application-Insights-Connection-String"
}


resource "azurerm_resource_group" "base" {
  name     = "${local.resource_name_prefix}-resources"
  location = var.azure_location
  tags     = var.common_tags
}

module "network" {
  providers = {
    azurerm = azurerm
  }
  source = "../network"

  resource_name_prefix = local.resource_name_prefix
  resource_group       = azurerm_resource_group.base
  common_tags          = var.common_tags
}

module "database" {
  providers = {
    azurerm = azurerm
    random  = random
  }
  source = "../database"

  resource_name_prefix = local.resource_name_prefix
  resource_group       = azurerm_resource_group.base
  common_tags          = var.common_tags
  deployment_zone      = var.deployment_zone
  //delegated_subnet_id  = module.network.database_subnet_id
  //private_dns_zone_id  = module.network.database_private_dns_zone_id
}

module "browserless" {
  providers = {
    azurerm = azurerm
  }
  source = "../browserless"

  browserless_sku_name = var.browserless_sku_name

  resource_name_prefix  = local.resource_name_prefix
  resource_group        = azurerm_resource_group.base
  common_tags           = var.common_tags
  browserless_subnet_id = module.network.browserless_subnet_id
}

module "web_app" {
  providers = {
    azurerm = azurerm
    random  = random
  }
  source = "../web_app"

  web_app_sku_name          = var.web_app_sku_name
  web_app_use_32_bit_worker = var.web_app_use_32_bit_worker
  web_app_dotnet_version    = var.web_app_dotnet_version
  web_app_always_on         = var.web_app_always_on

  environment_name     = var.environment_name
  resource_name_prefix = local.resource_name_prefix
  resource_group       = azurerm_resource_group.base
  common_tags          = var.common_tags
  web_app_subnet_id    = module.network.web_app_subnet_id

  # TODO Replace with Key Vault Secrets
  application_insights_instrumentation_key = module.application_insights.instrumentation_key
  application_insights_connection_string   = module.application_insights.connection_string
  browserless_connection_string            = module.browserless.browserless_connection_string
  postgresql_connection_string             = module.database.insight_connection_string

}


module "application_insights" {
  providers = {
    azurerm = azurerm
  }
  source = "../application_insights"

  resource_name_prefix       = local.resource_name_prefix
  resource_group             = azurerm_resource_group.base
  common_tags                = var.common_tags
  web_app_target_resource_id = module.web_app.web_app_id
}

/*

TODO use key vault instead of web app configuration

module "key_vault" {
  providers = {
    azurerm = azurerm
  }
  source = "../key_vault"

resource_name_prefix = var.resource_name_prefix
  resource_group   = azurerm_resource_group.base
  common_tags      = var.common_tags
}

│ Error: A resource with the ID "https://devops-insight-key-vault.vault.azure.net/secrets/Postgres-Admin-Connection-String/e75e63478cf7485989118859ebd461c6" already exists - to be managed via Terraform this resource needs to be imported into the State. Please see the resource documentation for "azurerm_key_vault_secret" for more information.
│
│   with module.environment.azurerm_key_vault_secret.postgres_admin_connection_string,
│   on ../../modules/environment/main.tf line 56, in resource "azurerm_key_vault_secret" "postgres_admin_connection_string":
│   56: resource "azurerm_key_vault_secret" "postgres_admin_connection_string" {
resource "azurerm_key_vault_secret" "postgres_admin_connection_string" {
  name         = local.postgres_admin_user_connection_string_key
  value        = module.database.admin_user_connection_string
  key_vault_id = module.key_vault.key_vault_id
}

resource "azurerm_key_vault_secret" "postgres_insight_connection_string" {
  name         = local.postgres_insight_connection_string_key
  value        = module.database.insight_connection_string
  key_vault_id = module.key_vault.key_vault_id
}

resource "azurerm_key_vault_secret" "application_insights_connection_string" {
  name         = local.application_insights_user_connection_string_key
  value        = module.application_insights.connection_string
  key_vault_id = module.key_vault.key_vault_id
}


resource "azurerm_key_vault_access_policy" "access_policy_web_app" {
  key_vault_id = module.key_vault.key_vault_id
  tenant_id    = var.tenant_id
  object_id    = module.web_application

  secret_permissions = [
    "Get", "List",
  ]
}

│ Error: expected "object_id" to be a valid UUID, got /subscriptions/6963cabb-d678-4e48-be5f-a3dc901b7b73/resourceGroups/devops-insight-resources/providers/Microsoft.Web/sites/devops-insight-app
│
│   with module.environment.azurerm_key_vault_access_policy.access_policy_web_app,
│   on ../../modules/environment/main.tf line 109, in resource "azurerm_key_vault_access_policy" "access_policy_web_app":
│  109:   object_id    = module.web_application.web_app_id
*/
