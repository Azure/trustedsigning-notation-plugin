# Trusted Signing Notation Plugin

Trusted Signing Provider for the [Notation CLI](https://github.com/notaryproject/notation)

This Notation plugin allows you to use Microsoft's **Trusted Signing** service to sign and verify container images and other OCI artifacts. Signatures enable users to verify that these artifacts are from a trusted source and have not been tampered with since their release.

Please refer to [MS Learn Trusted Signing Documentation](https://learn.microsoft.com/en-us/azure/trusted-signing/) for more information on the service.

# Installation of the trusted signing plugin

1. Navigate to the [Releases](https://github.com/Azure/trustedsigning-notation-plugin/releases) page and choose a release of `notation-azure-trustedsigning`.
2. Download, verify, and then install the specified version of the plugin.

   **Automatic installation**:

   For Notation >= v1.1.0, please use [notation plugin install](https://github.com/notaryproject/notation/blob/v1.1.0/specs/commandline/plugin.md#notation-plugin-install) command to automatically install azure-trustedsigning plugin.

   For Linux amd64:

   ```
   notation plugin install --url https://github.com/Azure/trustedsigning-notation-plugin/releases/download/v1.0.0-beta.3/notation-azure-trustedsigning_1.0.0-beta.3_linux_amd64.tar.gz --sha256sum 0d33115709f6d71914cf834ae4ba827fc5178f13f4192a534eb65c72aa14b8cb
   ```

   For Linux arm64:

   ```
   notation plugin install --url https://github.com/Azure/trustedsigning-notation-plugin/releases/download/v1.0.0-beta.3/notation-azure-trustedsigning_1.0.0-beta.3_linux_arm64.tar.gz --sha256sum 534e88ca73471fda9e6c6d468691aa8960915d07be8dc1493a64f5b7e92f9617
   ```

   For Windows amd64:

   ```
   notation plugin install --url https://github.com/Azure/trustedsigning-notation-plugin/releases/download/v1.0.0-beta.3/notation-azure-trustedsigning_1.0.0-beta.3_windows_amd64.zip --sha256sum fe66537e0b6b85395238d732ab58c1c85907cfb850d2205e4aa297ce2afb2c32
   ```

   For Windows arm64:

   ```
   notation plugin install --url https://github.com/Azure/trustedsigning-notation-plugin/releases/download/v1.0.0-beta.3/notation-azure-trustedsigning_1.0.0-beta.3_windows_arm64.zip --sha256sum 0186cda100bbcc958bbf0212b90e37dd5a5e95d7b27685c72683c26c265bb1d8
   ```

   For macOS amd64:

   ```
   notation plugin install --url https://github.com/Azure/trustedsigning-notation-plugin/releases/download/v1.0.0-beta.3/notation-azure-trustedsigning_1.0.0-beta.3_darwin_amd64.tar.gz --sha256sum a279650dc467f9f98ebd0bd5eb909bc23e158edf5cef8a1ce861a998d7015d81
   ```

   For macOS arm64:

   ```
   notation plugin install --url https://github.com/Azure/trustedsigning-notation-plugin/releases/download/v1.0.0-beta.3/notation-azure-trustedsigning_1.0.0-beta.3_darwin_arm64.tar.gz --sha256sum b9b5a763c628a7d3b3452be69999c32dbead01bc40416a77025cdd407fd7a27d
   ```
    **Manual installation**:

   For Linux Bash:

   ```bash
   version=1.0.0-beta.3
   arch=amd64
   install_path="${HOME}/.config/notation/plugins/azure-trustedsigning"

   # download tarball and checksum
   checksum_file="notation-azure-trustedsigning_${version}_checksums.txt"
   tar_file="notation-azure-trustedsigning_${version}_linux_${arch}.tar.gz"
   curl -Lo ${checksum_file} "https://github.com/Azure/trustedsigning-notation-plugin/releases/download/v${version}/${checksum_file}"
   curl -Lo ${tar_file} "https://github.com/Azure/trustedsigning-notation-plugin/releases/download/v${version}/${tar_file}"

   # validate checksum
   grep ${tar_file} ${checksum_file} | sha256sum -c

   # install the plugin
   mkdir -p ${install_path}
   tar xvzf ${tar_file} -C ${install_path} notation-azure-trustedsigning
   ```

   For macOS Zsh:

   ```zsh
   version=1.0.0-beta.3
   arch=amd64
   install_path="${HOME}/Library/Application Support/notation/plugins/azure-trustedsigning"

   # download tarball and checksum
   checksum_file="notation-azure-trustedsigning_${version}_checksums.txt"
   tar_file="notation-azure-trustedsigning_${version}_darwin_${arch}.tar.gz"
   curl -Lo ${checksum_file} "https://github.com/Azure/trustedsigning-notation-plugin/releases/download/v${version}/${checksum_file}"
   curl -Lo ${tar_file} "https://github.com/Azure/trustedsigning-notation-plugin/releases/download/v${version}/${tar_file}"

   # validate checksum
   grep ${tar_file} ${checksum_file} | shasum -a 256 -c

   # install the plugin
   mkdir -p ${install_path}
   tar xvzf ${tar_file} -C ${install_path} notation-azure-trustedsigning
   ```

   For Windows Powershell:

   ```powershell
   $version = "1.0.0-beta.3"
   $arch = "amd64"
   $install_path = "${env:AppData}\notation\plugins\azure-trustedsigning"

   # download zip file and checksum
   $checksum_file = "notation-azure-trustedsigning_${version}_checksums.txt"
   $zip_file = "notation-azure-trustedsigning_${version}_windows_${arch}.zip"
   Invoke-WebRequest -OutFile ${checksum_file} "https://github.com/Azure/trustedsigning-notation-plugin/releases/download/v${version}/${checksum_file}"
   Invoke-WebRequest -OutFile ${zip_file} "https://github.com/Azure/trustedsigning-notation-plugin/releases/download/v${version}/${zip_file}"

   # validate checksum
   $checksum = (Get-Content ${checksum_file} | Select-String -List ${zip_file}).Line.Split() | Where-Object {$_}
   If ($checksum[0] -ne (Get-FileHash -Algorithm SHA256 $checksum[1]).Hash) {
      throw "$($checksum[1]): Failed"
   }

   # install the plugin
   mkdir ${install_path}
   Expand-Archive -Path ${zip_file} -DestinationPath ${install_path}
   ```

3. Run `notation plugin list` and confirm the `azure-trustedsigning` plugin is installed.

## Getting started

1. [Sign and verify an artifact with with a trusted signing certficate profile](docs/sign-and-verify.md)


## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft
trademarks or logos is subject to and must follow
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
