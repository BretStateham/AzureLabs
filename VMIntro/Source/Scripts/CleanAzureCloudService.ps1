param (
  #Cloud Service Name to Create
  [Parameter(Mandatory = $true)]
  [String]$ServiceName
)

# Check if Windows Azure Powershell is avaiable
if ((Get-Module -ListAvailable Azure) -eq $null)
{
    throw "Windows Azure Powershell not found! Please make sure to install them from http://www.windowsazure.com/en-us/downloads/#cmd-line-tools"
}

$subscription = Get-AzureSubscription -Current

""
"Using subscription   : {0}" -f $subscription.SubscriptionName
"Deleting     Service : {0}" -f $ServiceName


Remove-AzureService -ServiceName $ServiceName -DeleteAll -Force