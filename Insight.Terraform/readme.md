# Biofuel Express Insight platform

## Introduction

This project uses Terraform to provision the infrastructure and deploy the services on Azure.
The following elements will be provisioned on Azure:

* App service
* Multiple Azure functions
* 1 resource group
* 1 General Purpose managed Postgres database

## Setup

You will need to install the following tools

* [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)
* [Terraform](https://developer.hashicorp.com/terraform/tutorials/azure-get-started/install-cli)

Login to the Biofuel tenant using the following command

```
az login --tenant 89efaadc-cfca-4ba1-994e-499e5ed6e241
```

You need a user with `Owner` access to an Azure subscription, and an
Azure Storage client.

You can create a Storage Client with the CLI using the following commands:

```bash
RESOURCE_GROUP_NAME=insight-core
STORAGE_ACCOUNT_NAME=insightinfrastorage
CONTAINER_NAME=tfstate

# Create resource group
az group create --name $RESOURCE_GROUP_NAME --location northeurope

# Create storage account
az storage account create --resource-group $RESOURCE_GROUP_NAME --name $STORAGE_ACCOUNT_NAME --sku Standard_LRS --encryption-services blob

# Get storage account key
ACCOUNT_KEY=$(az storage account keys list --resource-group $RESOURCE_GROUP_NAME --account-name $STORAGE_ACCOUNT_NAME --query [0].value -o tsv)

# Create blob container
az storage container create --name $CONTAINER_NAME --account-name $STORAGE_ACCOUNT_NAME --account-key $ACCOUNT_KEY --allow-blob-public-access 0
```

([Credit](https://medium.com/developingnodes/how-to-manage-terraform-state-in-azure-blob-storage-870a80917450))

### Provisioning the infrastructure

Find the environment you want to work with.
* env/test
* env/prod

Then run the following command:

```bash
terraform apply
```

## CI/CD setup

We need a service account to act on behalf of us on the CI server

``` bash
# Find the subscription id for the desired subscription
az account list --output table

az ad sp create-for-rbac --role="Contributor" --scopes="/subscriptions/SUBSCRIPTION_ID" --name="INSIGHT-Terraformer"
```

## Manual steps

Unfortunately there are some steps in the deployment of the ELVA infrastructure that cannot be managed by terraform (yet)

### SSL certificates for app services and functions

To configure ssl certificates for the api and web app proxies, the following steps needs to be completed:

1. configure a domain in azure dns zones.
   Could be: test.insight.projects.biofuel.dk and api.test.insight.projects.biofuel.dk
2. Setup custom dns in both app services and functions
3. Configure app service managed certificates as descripted in <https://docs.microsoft.com/en-us/azure/app-service/configure-ssl-certificate#create-a-free-certificate-preview>
4. Setup a ssl binding with the newly created certificat

### Azure DevOps/Portal NOTES
DevOps developer must be administrator in Azure DevOps and have Owner permissions on the Azure Subscription inorder to create the needed Service Connections