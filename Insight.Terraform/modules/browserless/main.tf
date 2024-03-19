terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
    }
  }
}

resource "azurerm_service_plan" "browserless" {
  name                = "${var.resource_name_prefix}-browserless-plan"
  location            = var.resource_group.location
  resource_group_name = var.resource_group.name

  os_type  = "Linux"
  sku_name = var.browserless_sku_name

  tags = var.common_tags

}

# Create an Azure Web App for Containers in that App Service Plan
resource "azurerm_linux_web_app" "browserless" {
  name                = "${var.resource_name_prefix}-browserless-app"
  location            = var.resource_group.location
  resource_group_name = var.resource_group.name
  service_plan_id     = azurerm_service_plan.browserless.id
  https_only          = true

  virtual_network_subnet_id = var.browserless_subnet_id

  # Do not attach Storage by default
  app_settings = {
    WEBSITES_ENABLE_APP_SERVICE_STORAGE = false
  }


  # Configure Docker Image to load on start
  site_config {
    application_stack {
      docker_image_name   = "browserless/chrome:1.60-chrome-stable"
      docker_registry_url = "https://docker.io"
    }
    vnet_route_all_enabled = true
    always_on              = "true"

    // Ignored and supposedly enabled by default
    // https://github.com/MicrosoftDocs/azure-docs/issues/94331
    // Even though the documentation says that it is disabled by default...
    // https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/linux_web_app
    websockets_enabled = true
  }

  identity {
    type = "SystemAssigned"
  }

  tags = var.common_tags
}