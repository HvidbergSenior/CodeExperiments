terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
    }
    random = {
      source = "hashicorp/random"
    }
    postgresql = {
      source  = "cyrilgdn/postgresql"
      version = ">=1.21.0"
    }
  }
}
resource "random_string" "database_postfix" {
  length  = 4
  lower   = true
  numeric = true
  upper   = false
  special = false
}

locals {
  #name must be unique across the entire Azure service, not just within the resource group.
  #added "random" postfix since azure doesn't guarantee "instant" deletion of azurerm_postgresql_flexible_server instances.
  postgresql_server_name = "${var.resource_name_prefix}-database-server-${random_string.database_postfix.result}"
  vpn_ip                 = "87.61.102.133"
  firewall_rules         = {
    all   = { start = "0.0.0.0", end = "0.0.0.0" }
    mia-1 = { start = "87.116.45.64", end = "87.116.45.78" }
    mia-2 = { start = "87.116.45.32", end = "87.116.45.62" }
    vpn   = { start = local.vpn_ip, end = local.vpn_ip }
  }
  password_length           = 16
  password_special          = true
  password_override_special = "_%@"
  postgresql_port           = 5432
  postgresql_grant_schema   = "public"
}

resource "random_password" "psqladmin" {
  length           = local.password_length
  special          = local.password_special
  override_special = local.password_override_special
}

resource "random_password" "insight" {
  length           = local.password_length
  special          = local.password_special
  override_special = local.password_override_special
}

resource "azurerm_postgresql_flexible_server" "postgresql" {
  name                = local.postgresql_server_name
  location            = var.resource_group.location
  resource_group_name = var.resource_group.name
  //delegated_subnet_id = var.delegated_subnet_id
  //private_dns_zone_id = var.private_dns_zone_id

  administrator_login    = "psqladmin"
  administrator_password = random_password.psqladmin.result
  sku_name               = "GP_Standard_D2s_v3"
  storage_mb             = 131072
  backup_retention_days  = 7
  version                = "15"
  zone = var.deployment_zone

  tags = var.common_tags
}

provider "postgresql" {
  host      = azurerm_postgresql_flexible_server.postgresql.fqdn
  username  = azurerm_postgresql_flexible_server.postgresql.administrator_login
  password  = azurerm_postgresql_flexible_server.postgresql.administrator_password
  port      = local.postgresql_port
  sslmode   = "require"
  superuser = false

}

resource "azurerm_postgresql_flexible_server_firewall_rule" "postgresql" {
  for_each         = local.firewall_rules
  name             = each.key
  server_id        = azurerm_postgresql_flexible_server.postgresql.id
  start_ip_address = each.value.start
  end_ip_address   = each.value.end
}

// insight_user, insight_db and privileges
resource "postgresql_role" "insight_user" {
  depends_on          = [azurerm_postgresql_flexible_server_firewall_rule.postgresql]
  name                = "insight_user"
  login               = true
  superuser           = false
  create_database     = false
  create_role         = false
  password            = random_password.insight.result
  skip_reassign_owned = true # prevent errors on terraform destroy
  inherit             = true
  replication         = false
}

resource "azurerm_postgresql_flexible_server_database" "insight_db" {
  depends_on = [postgresql_role.insight_user]
  name       = "insight_db"
  server_id  = azurerm_postgresql_flexible_server.postgresql.id
  collation  = "da_DK.utf8"
}

resource "postgresql_grant" "insight_table" {
  database    = azurerm_postgresql_flexible_server_database.insight_db.name
  schema      = local.postgresql_grant_schema
  role        = postgresql_role.insight_user.name
  object_type = "table"
  privileges  = ["SELECT", "INSERT", "UPDATE", "DELETE", "TRUNCATE"]
}

resource "postgresql_grant" "insight_schema" {
  database    = azurerm_postgresql_flexible_server_database.insight_db.name
  schema      = local.postgresql_grant_schema
  role        = postgresql_role.insight_user.name
  object_type = "schema"
  privileges  = ["CREATE"]
}

resource "postgresql_grant" "insight_database" {
  database    = azurerm_postgresql_flexible_server_database.insight_db.name
  schema      = local.postgresql_grant_schema
  role        = postgresql_role.insight_user.name
  object_type = "database"
  privileges  = ["CREATE"]
}

resource "postgresql_grant" "insight_procedure" {
  database    = azurerm_postgresql_flexible_server_database.insight_db.name
  schema      = local.postgresql_grant_schema
  role        = postgresql_role.insight_user.name
  object_type = "procedure"
  privileges  = ["EXECUTE"]
}

// plv8
resource "azurerm_postgresql_flexible_server_configuration" "plv8" {
  depends_on = [azurerm_postgresql_flexible_server_database.insight_db]
  name       = "azure.extensions"
  server_id  = azurerm_postgresql_flexible_server.postgresql.id
  value      = "PLV8"
}

resource "postgresql_extension" "plv8_extension" {
  depends_on = [azurerm_postgresql_flexible_server_configuration.plv8]
  name       = "plv8"
}