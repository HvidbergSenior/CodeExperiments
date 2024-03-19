terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
    }
  }
}

data "azurerm_subscription" "subscription" {
}

data "azurerm_client_config" "client_config" {
}

/*
TODO Assign as a part of insight-core
resource "azurerm_role_assignment" "key_vault_owner" {
  scope                = data.azurerm_subscription.subscription.id
  role_definition_name = "Key Vault Secrets Officer"
  principal_id         = data.azurerm_client_config.client_config.object_id
}

│ Error: authorization.RoleAssignmentsClient#Create: Failure responding to request: StatusCode=409 -- Original Error: autorest/azure: Service returned an error. Status=409 Code="RoleAssignmentExists" Message="The role assignment already exists."
│
│   with module.environment.module.key_vault.azurerm_role_assignment.key_vault_owner,
│   on ../../modules/key_vault/main.tf line 15, in resource "azurerm_role_assignment" "key_vault_owner":
│   15: resource "azurerm_role_assignment" "key_vault_owner" {

*/

resource "azurerm_key_vault" "key_vault" {
  name                       = "${var.resource_name_prefix}-key-vault"
  location                   = var.resource_group.location
  resource_group_name        = var.resource_group.name
  tenant_id                  = data.azurerm_client_config.client_config.tenant_id
  sku_name                   = "standard"
  soft_delete_retention_days = 7
  enable_rbac_authorization  = true


}