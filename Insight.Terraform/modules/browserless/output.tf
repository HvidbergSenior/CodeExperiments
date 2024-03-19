output "browserless_connection_string" {
  value = "ws://${azurerm_linux_web_app.browserless.name}.azurewebsites.net/"

}