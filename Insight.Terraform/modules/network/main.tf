terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
    }
  }
}


resource "azurerm_virtual_network" "vnet" {
  name                = "${var.resource_name_prefix}-vnet"
  location            = var.resource_group.location
  resource_group_name = var.resource_group.name
  address_space       = ["10.1.0.0/16"]

  tags = var.common_tags
}

// Web App
resource "azurerm_subnet" "web_app" {
  name                 = "${var.resource_name_prefix}-web-app-subnet"
  resource_group_name  = var.resource_group.name
  address_prefixes     = ["10.1.1.0/24"]
  virtual_network_name = azurerm_virtual_network.vnet.name
  service_endpoints    = ["Microsoft.Sql", "Microsoft.Storage"]

  delegation {
    name = "web-app-delegation"

    service_delegation {
      name    = "Microsoft.Web/serverFarms"
      actions = ["Microsoft.Network/virtualNetworks/subnets/action"]
    }
  }
}

// browserless
resource "azurerm_subnet" "browserless" {
  name                 = "${var.resource_name_prefix}-browserless-subnet"
  resource_group_name  = var.resource_group.name
  address_prefixes     = ["10.1.2.0/24"]
  virtual_network_name = azurerm_virtual_network.vnet.name
  service_endpoints    = ["Microsoft.Web"] # can be removed when/if we don't use IP restrictions in the environment

  delegation {
    name = "browserless-delegation"

    service_delegation {
      name    = "Microsoft.Web/serverFarms"
      actions = ["Microsoft.Network/virtualNetworks/subnets/action"]
    }
  }
}


/*
TODO : postgresql terraform provider and private dns zone

resource "azurerm_network_security_group" "nsg" {
  name                = "${var.resource_name_prefix}-nsg"
  location            = var.resource_group.location
  resource_group_name = var.resource_group.name

  security_rule {
    name                       = "allow"
    priority                   = 100
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "Tcp"
    source_port_range          = "*"
    destination_port_range     = "*"
    source_address_prefix      = "*"
    destination_address_prefix = "*"
  }
}

Error: Error connecting to PostgreSQL server devops-insight-database-server-w1x6.postgres.database.azure.com (scheme: postgres): dial tcp: lookup devops-insight-database-server-w1x6.postgres.database.azure.com on 172.20.192.1:53: no such host
│
│   with module.environment.module.database.postgresql_role.insight,
│   on ../../modules/database/main.tf line 97, in resource "postgresql_role" "insight":
│   97: resource "postgresql_role" "insight" {
// Database : https://learn.microsoft.com/en-us/azure/developer/terraform/deploy-postgresql-flexible-server-database?tabs=azure-cli
resource "azurerm_subnet" "database" {
  name                 = "${var.resource_name_prefix}-database-subnet"
  resource_group_name  = var.resource_group.name
  address_prefixes     = ["10.1.3.0/24"]
  virtual_network_name = azurerm_virtual_network.vnet.name
  service_endpoints    = ["Microsoft.Storage"]

  delegation {
    name = "database-delegation"

    service_delegation {
      name    = "Microsoft.DBforPostgreSQL/flexibleServers"
      actions = ["Microsoft.Network/virtualNetworks/subnets/join/action"]
    }
  }
}

resource "azurerm_subnet_network_security_group_association" "database_nsg" {
  subnet_id                 = azurerm_subnet.database.id
  network_security_group_id = azurerm_network_security_group.nsg.id
}

resource "azurerm_private_dns_zone" "database" {
  depends_on = [azurerm_subnet_network_security_group_association.database_nsg]
  name                = "${var.resource_name_prefix}-pdz.postgres.database.azure.com"
  resource_group_name = var.resource_group.name
}

resource "azurerm_private_dns_zone_virtual_network_link" "database" {
  name                  = "${var.resource_name_prefix}-pdzvnetlink.com"
  private_dns_zone_name = azurerm_private_dns_zone.database.name
  virtual_network_id    = azurerm_virtual_network.vnet.id
  resource_group_name = var.resource_group.name
}
*/