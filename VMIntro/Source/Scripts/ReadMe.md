<a name="anchor-name-here" />
# Scripts #

These scripts are provided FYI only.  

CreateAzureVMLabVMs.ps1 - Creates the SQLVM1, IISVM1 and IISVM2 VMs.

It provisions (but does not format) the additional two data drives on SQLVM1 

It provisions the "webport" load-balanced endpoint on port 80 for IISMV1 and IISVM2.

Usage: CreateAzureVMLabVMs.ps1 -ServiceName "TheNameOfTheCloudServiceToCreate"

CleanAzureCloudService.ps1 - Deletes all deployments for the Cloud Service specified. 

Usage: CleanAzureCloudService.ps1 -ServiceName "TheNameOfTheServiceToDelete"