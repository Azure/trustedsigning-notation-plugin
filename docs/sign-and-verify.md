# Sign and verify an artifact with a trusted signing certficate profile

> [!NOTE]
> Signing with trusted signing requires a trusted signing account and certificate profile. For more information, see [Set up trusted signing](https://learn.microsoft.com/en-us/azure/trusted-signing/quickstart).

## Requirements

To sign an artifact with trusted signing an active  signing account and certificate profile are needed. The endpoint uri corresponding to the profile's region is also required, to corroborate which endpoint to use, see the [following table](https://learn.microsoft.com/en-us/azure/trusted-signing/quickstart?tabs=registerrp-portal%2Caccount-portal%2Corgvalidation%2Ccertificateprofile-portal%2Cdeleteresources-portal#azure-regions-that-support-trusted-signing)

## Setting up the plugin signing key

From a terminal, run the following command to add a key with the trusted signing plugin:

```sh
notation key add "<<keyname>>" --plugin azure-trustedsigning --plugin-config accountName="<<trustedSigningAccount>>" --plugin-config certProfile="<<certificateProfile>>" --plugin-config baseUrl="<<endpointUri>>" --id "<<keyid>>"
```

> They key name is an user friendly name to refer to during sign operations from the CLI. The id is a unique identifier only known to the plugin. For more information, see [Plugin usage](https://github.com/notaryproject/specifications/blob/v1.0.0/specs/plugin-extensibility.md#using-a-plugin-for-signing)


## Signing an artifact

1. [Install the Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli)
2. Log in to Azure with Azure CLI:
   ```sh
   az login
   az account set --subscription $subscriptionID
   ```
> [!NOTE]
> Ensure the account used for signing has the necessary rbac permissions to sign with the trusted signing account.
> For more information, see [Supported roles for Trusted Signing](https://learn.microsoft.com/en-us/azure/trusted-signing/tutorial-assign-roles#supported-roles-for-trusted-signing)
3. [Create an Azure Container Registry](https://learn.microsoft.com/azure/container-registry/container-registry-get-started-portal?tabs=azure-cli). The remaining steps use the example login server `<registry-name>.azurecr.io`, but you must substitute your own login server value.
4. Log into the container registry and push an image for signing:
   ```sh
   registryName="<registry-name>"
   server="${registryName}.azurecr.io"

   az acr login --name $registryName
   # notation login $server  # if you don't use Azure Container Registry
   ```
   Push a hello-world image for signing
   ```sh
   docker pull hello-world:latest
   docker tag hello-world:latest $server/hello-world:v1
   docker push $server/hello-world:v1
   ```
5. Sign the image with the trusted signing key:
   ```sh
   notation sign $server/hello-world:v1 --key "<<keyname>>"
   ```

## Verify an artifact

To verify the signature of an artifact, run the following command:

```sh
notation verify $server/hello-world:v1
```
