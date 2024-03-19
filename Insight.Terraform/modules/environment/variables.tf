variable "environment_name" {}

variable "project_name" {}

variable "azure_location" {}

variable "common_tags" {
  description = "Custom tags to apply to all resources"
}

variable "subscription_id" {}

variable "tenant_id" {}

variable "deployment_zone" {}

variable "web_app_sku_name" {}

variable "web_app_use_32_bit_worker" {
  type = bool
}

variable "web_app_dotnet_version" {}

variable "web_app_always_on" {
  type = bool
}

variable "browserless_sku_name" {}

