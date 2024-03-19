output "admin_user_connection_string" {
  value = "Host=${azurerm_postgresql_flexible_server.postgresql.fqdn};Port=${local.postgresql_port};Database=${azurerm_postgresql_flexible_server_database.insight_db.name};Username=${azurerm_postgresql_flexible_server.postgresql.administrator_login};Password=${azurerm_postgresql_flexible_server.postgresql.administrator_password}"
}

output "insight_connection_string" {
  value = "Host=${azurerm_postgresql_flexible_server.postgresql.fqdn};Port=${local.postgresql_port};Database=${azurerm_postgresql_flexible_server_database.insight_db.name};Username=${postgresql_role.insight_user.name};Password=${postgresql_role.insight_user.password}"
}
