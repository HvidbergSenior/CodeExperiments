terraform {
  required_version = ">= 1.6"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">=3.81.0"
    }
    random = {
      source  = "hashicorp/random"
      version = ">=3.5.1"
    }
  }
  backend "azurerm" {
    resource_group_name  = "insight-core"
    storage_account_name = "insightinfrastorage"
    container_name       = "tfstate"
    subscription_id      = "6963cabb-d678-4e48-be5f-a3dc901b7b73"
    key                  = "prod.terraform.tfstate"
  }
}

locals {
  subscription_id  = "6963cabb-d678-4e48-be5f-a3dc901b7b73"
  tenant_id        = "89efaadc-cfca-4ba1-994e-499e5ed6e241"
  azure_location   = "northeurope"
  project_name     = "insight"
  environment_name = "prod"
}

provider "azurerm" {
  subscription_id = local.subscription_id
  tenant_id       = local.tenant_id
  features {
    key_vault {
      purge_soft_deleted_secrets_on_destroy = true
      recover_soft_deleted_secrets          = true
    }
  }
}

provider "random" {}

module "environment" {
  providers = {
    azurerm = azurerm
    random  = random
  }
  source = "../../modules/environment"

  subscription_id  = local.subscription_id
  tenant_id        = local.tenant_id
  azure_location   = local.azure_location
  project_name     = local.project_name
  environment_name = local.environment_name
  common_tags      = { Project = local.project_name }

  deployment_zone = 2

  web_app_sku_name          = "S2"
  web_app_use_32_bit_worker = false
  web_app_dotnet_version    = "v7.0"
  web_app_always_on         = true

  browserless_sku_name = "S1"
}

