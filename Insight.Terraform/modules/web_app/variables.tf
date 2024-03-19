variable "environment_name" {}

variable "resource_name_prefix" {}

variable "resource_group" {}

variable "common_tags" {}

variable "postgresql_connection_string" {
  description = "TODO Replace with Key Vault Secret"
}

variable "application_insights_instrumentation_key" {
  description = "TODO Replace with Key Vault Secret"
}

variable "application_insights_connection_string" {
  description = "TODO Replace with Key Vault Secret"
}

variable "web_app_sku_name" {}

variable "web_app_use_32_bit_worker" {
  type = bool
}

variable "web_app_dotnet_version" {}

variable "web_app_always_on" {
  type = bool
}

variable "web_app_subnet_id" {}

variable "browserless_connection_string" {}