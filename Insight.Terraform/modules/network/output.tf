output "virtual_network_id" {
  value = azurerm_virtual_network.vnet.id
}

output "web_app_subnet_id" {
  value = azurerm_subnet.web_app.id
}

output "browserless_subnet_id" {
  value = azurerm_subnet.browserless.id
}

/*
output "database_subnet_id" {
  value = azurerm_subnet.database.id
}

output "database_private_dns_zone_id" {
  value = azurerm_private_dns_zone.database.id
}*/