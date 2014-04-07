<a name="HOLTop"></a>
# Introduction to Windows Azure Virtual Machines #

---

<a name="Overview"></a>
## Overview ##

Using Windows Azure as your Infrastructure as a Service (IaaS) platform, will enable you to create and manage your infrastructure quickly, provisioning and accessing any host ubiquitously. Grow your business through the cloud-based infrastructure, reducing the costs of licensing, provisioning and backup.

In this hands-on Lab, you will learn how to deploy a simple ASP.NET MVC 4 Web application to a pair of Load Balanced Web Server Virtual Machines hosted in Windows Azure.  The website will consume data hosted in SQL Server instance running on a third Virtual Machine.

![0010LabServiceArchitecture](images/0010labservicearchitecture.png?raw=true "Lab Service Architecture")

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a Web Farm using Windows Azure Management Portal
- Configure Load Balancing Endpoints
- Deploy a Simple MVC4 Application that consumes SQL Server Features
- Create a Virtual Machine with SQL Server Full-Text Search feature to be consumed by the MVC Application

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating the Windows Azure Virtual Machines](#Ex1)
1. [Configure the SQL Server Virtual Machine and Database](#Ex2)
1. [Configure the Web Server Virtual Machines and Web Sites](#Ex3)

Estimated time to complete this lab: **45 minutes**.

<a name="Ex1"></a>
### Exercise 1: Creating the Windows Azure Virtual Machines ###

In this exercise, you will provision the three Windows Azure Virtual Machines used throughout the lab.  Specifically you will

- Create the **SQL Server Virtual Machine** (***sqlvm1***) that will later host the AdventureWorks2012 database used by the website.  

	When you create the SQL Server Virtual Machine, you will also create the **Cloud Service** that will contain all of the Virtual Machines used in this lab.
- Create the **Web Server Virtual Machines** (***iismv1*** and ***iisvm2***) that will host the load balanced web site. You will add those VMs to the same cloud service that ***sqlvm1*** is in.

The highlighted portion of the following diagram shows what you will configure in this exercise.

![1010LabServiceArchitectureEx1](images/1010labservicearchitectureex1.png?raw=true "Architecture after Exercise 1")

<a name="Ex1T1"></a>
#### Task 1 - Creating the SQL VM and the Cloud Service ####

1. Open the **[Windows Azure Management Portal](https://manage.windowsazure.com/ "Windows Azure Management Portal")** (https://manage.windowsazure.com) and login with your credentials.

1. In the menu bar along the bottom of the portal click **+NEW** 

	![1020NewButton](images/1020newbutton.png?raw=true "New Button")

1. Then select **COMPUTE** | **VIRTUAL MACHINE** | **FROM GALLERY** 

	![1030NewVMFromGallery](images/1030newvmfromgallery.png?raw=true "New VM From Gallery")

1. In the **"CREATE A VIRTUAL MACHINE WIZARD"**, on the **"Coose an Image"** page, scroll down to find and select the **"SQL Server 2012 SP1 Enterprise on Windows Server 2008 R2"** image and click the next (arrow) button.

	![1040SQLImage](images/1040sqlimage.png?raw=true "SQL Server 2012 SP1 Enterprise on Windows Server 2008 R2")

1. On the "Virtual machine configuration" page:
	- VERSION RELEASE DATE: ***Choose the latest date***
	- VIRTUAL MACHINE NAME: ***sqlvm1*** (Use this name.  using a different name will cause issues later in the lab).
	- TIER: ***Standard***
	- SIZE: ***A1 (1 core, 1.75 GB memory)*** (You don't need anything bigger for this lab)
	- NEW USER NAME: ***CloudShop***
	- NEW PASSWORD:  ***Azure$123***

	> **Note:** You can use a different user name and password, but it is recommended that you use the values above for consistency.  These will be used as the user name and password for the built-in administrator account in the Windows virtual machine.  Regardless of what you use, it is highly recommended that you use the same credentials for all three Virtual Machines to help eliminate any confusion during the lab. 

	![1050VMConfig](images/1050vmconfig.png?raw=true "Virtual Machine Configuration")

1. On the second **"Virtual machine configuration"** page, enter:
	- CLOUD SERVICE: ***Create a new cloud service***
	- CLOUD SERVICE DNS NAME: ***Enter a unique valid DNS host name.***.  
	The name CloudShopDemo is used as a sample throughout this lab, but you will need to use your own unique name instead.
	- REGION/AFFINITY GROUP/VIRTUAL NETWORK: ***Pick a region close to you***
	- STORAGE ACCOUNT: ***Use an automatically generated storage account***
	- AVAILABILITY SET: ***(None)***
	- ENDPOINTS: ***Leave the default endpoints.***

	![1060VMConfig2](images/1060vmconfig2.png?raw=true "Virtual Machine Configuration")

1. On the third final **"Virtual machine configuration"** page, accept the default values and click the "Check Mark" button in the lower right corner to complete the wizard.

	![1070VMConfig3](images/1070vmconfig3.png?raw=true "Virtual Machine Configuration Page 3")


	> **Note:** The Virtual Machine already has endpoints for Remote Desktop connections as well as for remote PowerShell scripts.  

1. In the portal, you should now see the ***sqlvm1*** virtual machine with a status of **Starting (Provisioning)**.  Wait until the status reads just **Starting** (about 1-2 minutes) before proceeding with the next task. 

	![1080SQVM1Provisioning](images/1080sqvm1provisioning.png?raw=true "SQLVM1 Provisioning")

<a name="Ex1T2"></a>
#### Task 2 - Adding the Web Server Virtual Machines to the Cloud Service ####

In this task, you will create BOTH of the Web Server Virtual Machines, **iisvm1** and **iismv2**.  The steps to create them are identical with the exception of the name.  

1. Wait until the previous virtual machine has been provisioning for a couple of minutes. 

1. Click the +NEW Button along the bottom and select **COMPUTE** | **VIRTUAL MACHINE** | **FROM GALLERY**

	![1090NewVMFromGallery](images/1090newvmfromgallery.png?raw=true "New VM From Gallery")

1. In the **"CREATE A VIRTUAL MACHINE"** wizard, on the **"Choose an Image"** page, select the **"Windows Server 2008 R2 SP1"** image and click the next (arrow) button to continue:
	![1100Win2K8VMImage](images/1100win2k8vmimage.png?raw=true "Windows Server 2008 R2 SP1 Image")

1. On the first **"Virtual machine configuration"** page:
	- VERSION RELEASE DATE: ***Choose the latest date***
	- VIRTUAL MACHINE NAME: ***iisvm1*** for the first vm, ***iisvm2*** for the second
	- TIER: ***Standard***
	- SIZE: ***A1 (1 core, 1.75 GB memory)***
	- NEW USER NAME: ***CloudShop*** (or the same name you used for ***sqlvm1***)
	- NEW PASSWORD:	***Azure$123*** (or the same password you used for ***sqlvm1***)

	![1110VMConfig2](images/1110vmconfig2.png?raw=true "Virtual Machine Configuration Page 2")

1. On the second **"Virtual machine configuration"** page:
	- CLOUD SERVICE: Choose the cloud service you created previously while creating ***sqlvm1***.
	- Leave all the other options at the default

	![1120VMConfig2](images/1120vmconfig2.png?raw=true "Virtual Machine Configuration Page 2")


1. On the third and final **"Virtual machine configuration"** page, leave the default values and click the "Check Mark" button in the lower right corner to complete the wizard.  

	![1130VMConfig3](images/1130vmconfig3.png?raw=true "Virtual Machine Configuration Page 3")

1. After creating **iisvm1** (and waiting 1-2 minutes to continue) then 
**repeat the steps above to create _iisvm2_**

1. Once all three virtual machines have been created, **wait until at least the sqlvm1 status reads "Running"** before proceeding to the next task.  **This could take anywhere from 10-30 minutes**.  Now is a good time to take a break! 

	![1140VMsRunning](images/1140vmsrunning.png?raw=true "Virtual Machines Running")

---

<a name="Ex2"></a>
### Exercise 2: Configure the SQL Server Virtual Machine and Database ###

In this exercise, we will configure the SQL Server Virtual Machine (***sqlvm1***) that we created in the previous exercise.  Specifically, we will 

- Add two additional disks to the SQL Server to store the database data files and logs
- Download the AdventureWorks2012 database and attach it to the SQL Server Instance
- Create a SQL Server Login that will be used by the websites to access the AdventureWorks2012 database
- Setup a Firewall rule to allow TCP connections to the SQL Server service running on port 1433

The highlighted portion of the following diagram shows what you will configure in this exercise.

![2010LabServiceArchitectureEx2](images/2010labservicearchitectureex2.png?raw=true "Architecture after Exercise 2")

<a name="Ex2T1"></a>
#### Task 1 - Adding additional Disks to the SQL Server Virtual Machine ####

1. In the Windows Azure Management Portal, on the "VIRTUAL MACHINES" page, click on the name of the "sqlvm1" virtual machine, then switch to it's DASHBOARD page. Scroll to the bottom to see that there is only a single drive attached to the VM.

	![2013SQLVM1Drive2](images/2013sqlvm1drive2.png?raw=true "SQLVM1 Drives")

1. Click the **"ATTACH"** button along the bottom, and select **"Attach empty disk"**:

	![2014AttachEmptyDisk](images/2014attachemptydisk.png?raw=true "Attach Empty Disk")


1. In the "Attach an empty disk..." window, you don't need to change anything except the size.  Enter **50** for the **SIZE (GB)**:

	![2016VHDSize](images/2016vhdsize.png?raw=true "VHD Size")

1. Once the first drive is successfully attached, **repeat the process to attach a second drive**.  These two drives will be used to store the SQL Server database files, logs and backups.  Once you are done, there should be a total of three drives on the sqlvm1 virtual machine.

	![2018SQLVM1Drives](images/2018sqlvm1drives.png?raw=true "SQLVM1 Drives")

1. While still on the sqlvm1 dashboard page, click the **"CONNECT"** button along the bottom.
	
	![2020ConnectToSQLVM1](images/2020connecttosqlvm1.png?raw=true "Connect to SQLVM1")

1. You will see an informational message from the portal instructing you to open the .rdp file when prompted. Click **OK** to clear the message:

	![2030RDPInfoMsg](images/2030rdpinfomsg.png?raw=true "RDP File Informational Message")

1. Then, when prompted, **open the RDP file**:

	![2040OpenRDPFile](images/2040openrdpfile.png?raw=true "Open the RDP File")

1. You may see a prompt about the publisher not being able to be verified, if you do click **"Connect"**:

	![2050RDPPublisherVerification](images/2050rdppublisherverification.png?raw=true "RDP Publisher Verification")

1. When prompted, enter the credentials you used for the sqlvm1 administrator account (**CloudShop/Azure$123**)

	![2060Credentials](images/2060credentials.png?raw=true "Credentials")

1. You should see a final prompt to verify the identity of the remote computer.  You can turn on the "Don't ask me again..." checkbox to prevent this dialog appearing in the future for this VM.  Click **"Yes"** to connect:

	![2070RDPIdentity](images/2070rdpidentity.png?raw=true "RDP Identity Confirmation")

1. Once you are connected to sqlvm1 via remote desktop, you can close the **"Initial Configuration"** window if it is open.  Then, launch **"Server Manager"** (if it isn't open already) by clicking on the shortcut on the toolbar:

	![2080SeverManagerShortcut](images/2080severmanagershortcut.png?raw=true "Server Manager Shortcut")

1. In the **"Server Manager"** window, on the **"Server Manager (SQLVM1)"** page, click the "Configure IE ESC" link.

	> **Note:** We don't actually need this until we download the AdventureWorks2012 database later in the lab, but we are doing it now since we are here. 

	![2080ConfigureIeEscLink](images/2080configureieesclink.png?raw=true "Configure IE ESC")

1. Then, in the **"Internet Explorer Enhanced Security Configuration"** window, under **"Administrators"** select **"Off"**, then click **"OK"**:

	![2090IeEscOff](images/2090ieescoff.png?raw=true "IE ESC Off for Administrators")
	
	> **Note:** This will allow us to download files from the internet.  We'll need that later so that we can download the AdventureWorks2012 Database.

1. Back in the **"Server Manager"** window, expand the **"Storage"** node, and select **"Disk Management"**.  Then, in the **"Initialize Disk"** window, ensure that the checkmark is on for both disks, and click **"OK"**:

	![2100StorageInitialization](images/2100storageinitialization.png?raw=true "Storage Initialization")

1. **You will repeat the following steps twice**, once for each drive we attached to SQLVM1.  You will assign the drive letter "F" and a volume name of "SQLData" to the first drive, and then you will assign the drive letter "G" and a volume name of "SQLLogs" to the second drive.  

1. **Right click** on an "Unallocated" drive, and select "New Simple Volume..." from the pop-up menu:

	![2110NewSimpleVolume](images/2110newsimplevolume.png?raw=true "New Simple Volume")

1. On the **"Welcome to the New Simple Volume Wizard"** page, click **"Next"**:

	![2120NSVWPage1](images/2120nsvwpage1.png?raw=true "New Simple Volume Wizard Welcome Page")

1. On the "Specify Volume Size" page, leave the default value, and click **"Next"**:

	![2130NSVWPage2](images/2130nsvwpage2.png?raw=true "Simple Volume Size")

1. On the **"Assign Drive Letter or Path"** page, assign the drive letter "F" for the first volume, and assign the drive letter "G" for the second volume.

	![2140AssignDriveLetter](images/2140assigndriveletter.png?raw=true "Assign Drive Letter")

1. On the **"Format Partition"** page, leave all the values at the default except set the **"Volume label"** to **"SQLData"** for the first drive and **"SQLLogs"** for the second drive. 

	![2150FormatPartition](images/2150formatpartition.png?raw=true "Format Partition")

1. On the **"Completing the New Simple Volume Wizard"** page, click **"Finish"** 

	![2160CompleteTheWizard](images/2160completethewizard.png?raw=true "Complete the Wizard")

1. Again, make sure you repeat the steps above to format both new drives.  When you are done you should have two new drive letters (F & G) available.

	![2170NewlyFormattedDrives](images/2170newlyformatteddrives.png?raw=true "Newly Formatted Drives")

1. In the open the new drives in Windows Explorer and ***create the following folders***.  Later, we will configure SQL Server to use these new folders rather than the default folders on the C: Drive:
	
	- **F:\Data**
	- **G:\Logs**
	- **G:\Backups**

	![2180NewFolders](images/2180newfolders.png?raw=true "New Folders")

<a name="Ex2T2"></a>
#### Task 2 - Downloading and Attaching the AdventureWorks2012 Database ####

In this task, we'll download the AdventureWorks2012 Database file from CodePlex into our new **"F:\Data"** folder, configure SQL Server to use our new data, logs, and backups folders, attach the AdventureWorks2012 database to SQL Server, and configure the full text catalog used by the website.  

1. Still working in the SQLVM1 Remote Desktop connection, open Internet Explorer.  If prompted, select **"Use recommended security..."** and click **"OK"**:

	![2190IESettings](images/2190iesettings.png?raw=true "IE Settings")

1. Navigate to the SQL Server Product Samples page on codeplex by opening http://msftdbprodsamples.codeplex.com in Internet Explorer on SQLVM1.  Then click on the "SQL Server 2012 OLTP" link:

	![2200SQLSamples](images/2200sqlsamples.png?raw=true "SQL Samples")

1. On thge "Adventure Works for SQL Server 2012" page, click the **"AdventureWorks2012 Data File Link"**, and when prompted save the file to the **"F:\Data"** folder. Close IE when the download is complete.

	![2210DownloadAdventureWorks](images/2210downloadadventureworks.png?raw=true "Download the AdventureWorks2012 Database")

1. From the SQLVM1 Start Menu, select **"All Programs"** | **"Microsoft SQL Server 2012"** | **"SQL Server Management Studio"**

	![2220StartSSMS](images/2220startssms.png?raw=true "Start SSMS")

1. When prompted, connect to the **SQLVM1** instance:

	![2230ConnectToSQLMV1](images/2230connecttosqlmv1.png?raw=true "Connect to SQLVM1")

1. In the **"Object Explorer"** window, right click the "SQLVM1" server name and select "Properties" from the pop-up menu:

	![2240OpenDatabaseProperties](images/2240opendatabaseproperties.png?raw=true "Open Database Properties")

1. In the **"Server Properties - SQLVM1"** window, switch to the **"Security"** Page, and select **"SQL Server and Windows Authentication Mode"**.  

	![2250MixedAuth](images/2250mixedauth.png?raw=true "Enabled Mixed Authentication")

	> **Note:** We need this because the websites will connect to the server using "SQL Server Authentication" rather than "Windows Authentication"

1. Next, switch to the **"Database Settings"** page, and set the **Data**, **Log** and **Backups** default folders to the F:Data, G:\Logs and G:\Backups folders respectively. Then click **"OK"**:

	![2260DatabaseDefaultFolders](images/2260databasedefaultfolders.png?raw=true "Database Default Folders")

1. You will be notified that the changes won't take effect until we restart the SQL Server instance is restarted.  Click **"OK"** to continue:

	![2270RestartNotice](images/2270restartnotice.png?raw=true "Restart Notice")

1. To restart the SQLVM1 SQL Server, right click on the SQLVM1 server name in the SQL Server Management Studio Object Explorer window, select "Restart" from the pop-up menu, then click **"Yes"** when asked if you are sure you want to restart:

	![2280RestartSQL](images/2280restartsql.png?raw=true "Restart SQL")

1. Once the server has restarted, right click the "Databases" node and select "Attach..."

	![2290AttachDatabase](images/2290attachdatabase.png?raw=true "Attach Database")

1. In the **"Attach Databases"** window, click the **"Add..."** button:

	![2300AddButton](images/2300addbutton.png?raw=true "Add Button")

1. Select the **"F:\Data\AdventureWorks2012_Data.mdf" data file and click **"OK"**:

	![2310AdventureWorksDataFile](images/2310adventureworksdatafile.png?raw=true "AdventureWorks Data File")

1. Back in the **"Attach Databases"** window, select the **"AdventureWorks2012_Log.ldf"** file (listed as Not Found) and click the **"Remove"** button.  

	![2320RemoveLogFile](images/2320removelogfile.png?raw=true "Remove Log File")

	> **Note:** Recall that we ONLY donloaded the **Data** file.  However, since the original datafile was property detached from the source server, it is at a consistent state, and the transaction logs are needed.  We can let SQL Server just create a new one on the fly.  By removing this one, SQL will create a new one when we attach the database. 

1. Click the **"OK"** button to attach the AdventureWorks2012 database:

	![2230AttachAdventureWorks2012](images/2230attachadventureworks2012.png?raw=true "Attach AdventureWorks2012")

1. You should now see the AdventureWorks2012 database in the **"Object Explorer"** window:

	![2240AdventureWorks2012](images/2240adventureworks2012.png?raw=true "AdventureWorks2012")

1. Expand the **"Databases"** | **"AdventureWorks2012"** | **"Storage"**.  Right-click on **"Full Text Catalogs"**, and select **"New Full-Text Catalog..."** from the pop-up menu:

	![2242NewFullTextCatalog](images/2242newfulltextcatalog.png?raw=true "New Full Text Catalog")

1. In the **"New Full-Text Catalog - AdventureWorks2012"** window, enter **"AdventureWorksCatalog*"" for the **Full-text catalog name"** and click **"OK"**:

	![2244AdventureWorksCatalog](images/2244adventureworkscatalog.png?raw=true "AdventureWorksCatalog")

1. Double-click on the new **"AdventureWorksCatalog"** to open it's properties

	![2246OpenAdventureWorksCatalog](images/2246openadventureworkscatalog.png?raw=true "Open AdventureWorksCatalog")

1. In the **"Full-Text Catalog Properites - AdventureWorksCatalog*"" window, switch to the **"Tables/Views"** page, and in the **"All eligible table/view objects in the database"** list, locate the **"Production.Product"** table. Then click the right-arrow button to add it to the tables assigned to the catalog:

	![2247AddProductionProduct](images/2247addproductionproduct.png?raw=true "Add the Product.Product Table")

1. In the **"Eligible Columns" list, turn on the checkbox next to the **"Name"** column, and click **"OK"**:

	![2248IndexName](images/2248indexname.png?raw=true "Index Name")

	> **Note:** These steps created a Full-Text catalog and index that will make searching for words within the Production.Product's Name column much faster than a traditional SQL Server index on the name column.  The website will leverage this index to quickly find products based on their names.  

<a name="Ex2T3"></a>
#### Task 3 - Creating the "CloudShop" SQL Server Login ####

The websites are pre-configured to connect to the SQL Server instance using a SQL Login named "CloudShop" with a password of "Azure$123".  For that to work, we need to create a SQL Server Login with those credentials.  

1. In the **SQLVM1 SQL Server Management Studio**, in the **"Object Explorer"** window, expand **"SQLVM1 ..."** | **"Security"** | **"Logins"**.  **Right-Click** on **"Logins"** and select **"New Login..."** from the pop-up menu.

	![2350NewLogin](images/2350newlogin.png?raw=true "New Login")

1. In the **"Login - New"** Window, on the **"General"** page, enter:

	- Login name:  ***CloudShop*** (Use this exact name because this is what the website's web.config file is pre-configured to use.  Use "CloudShop" here, even if you used a different name for your virtual machine Windows logins). 

	- Password: ***Azure$123*** (Again, use this exact value including case. The website's web.config file has this value pre-configured). 

	- Enforce password policy: ***CLEARED***

	- Default database: ***AdventureWorks2012***

	- Click **"OK"** to create the new login

	![2360CloudShopLogin](images/2360cloudshoplogin.png?raw=true "CloudShop Login")

1. Make sure that you clicked **"OK"** above to create the new **CloudShop** (**Not** the **SQLVM1\CloudShop** login, those are actually different logins) login, then immediately double click on the new login to re-open the Login properties.  Then, switch to the **"User Mapping"** page:

	- **Turn on** the checkbox next to the **AdventureWorks2012** database
	- **Turn on** the checkbox next to the **db_owner** role
	- Click **"OK"**


	![2370MapUser](images/2370mapuser.png?raw=true "Map User")

	> **Note:** There is a glitch in SQL Server Management Studio right now where it can't assign database roles to the user until the login exists.  That is why you had to exit the login properties and re-enter it again to map the login to a user account in the database.

1. You can close SQL Server Management Studio when you are done.

<a name="Ex2T4"></a>
#### Task 4 - Allowing SQL Server Connections through the Firewall ####

The last step we need to tak on the SQL Server is to open 1433 in the Windows Firewall.  The websites running on ***iisvm1*** and ***iisvm2*** will need this port open on the ***sqlvm1*** machine so they can connect to the SQL Server instance. 

1. If needed, on ***sqlvm1*** re-open the **"Server Manger"** by clicking on the icon on the task bar:

	![2080SeverManagerShortcut](images/2080severmanagershortcut.png?raw=true "Server Manager Shortcut")

1. Expand **"Server Manager (SQLVM1)"** | **"Configuration"** | **"Windows Firewall with Advanced Security"**, then right click on **"Inbound Rules"** and select **"New Rule..."** from the pop-up menu:

	![2380NewRule](images/3280newrule.png?raw=true "New Rule")

1. In the **"New Inbound Rule Wizard"**, on the **"Rule Type"** page, select **"Port"** and click **"Next"**:

	![2390RuleType](images/2390ruletype.png?raw=true "Rule Type")

1. On the **"Protocol and Ports"** page, select **"TCP"** and for the **"Specific local ports"** enter **1433**:

	![2400ProtocolAndPorts](images/2400protocolandports.png?raw=true "Protocol and Ports")

1. On the **"Action"** page, select **"Allow the connection"**:

	![2410Action](images/2410action.png?raw=true "Action")

1. On the **"Profile"** page, enable all profiles:

	![2420Profile](images/2420profile.png?raw=true "Profile")

1. On the **"Name"** page, name the rule **"SQLServerRule"**, enter a description, and click **"Finish"**:

	![2430Name](images/2430name.png?raw=true "Name")

1. You should see a new inbound rule:

	![2440SQLServerRule](images/2440sqlserverrule.png?raw=true "SQLServerRule")

1. You can close **"Sever Manager"** when you are done.

1. You can close the Remote Desktop Connection to SQLVM1 when you are done if you wish, but make sure to just disconnect, don't shut down the SQLVM1 server.  

---

<a name="Ex3"></a>
### Exercise 3: Configure the Web Server Virtual Machines and Web Sites ###

In this exercise, you will configure the Web Server Virtual Machines and the Web site.  Specifically you will:

- Add the Load Balanced Endpoint for Port 80
- Configure both Web Server VMs with the Web Server Role and ASP.NET 4.0
- Upload the CloudShop website and configure it in IIS on both Web Server VMs
- Test the Web Site 

The highlighted portion of the following diagram shows what you will configure in this exercise.

![3010LabServiceArchitectureEx3](images/3010labservicearchitectureex3.png?raw=true "Architecture after Exercise 3")

<a name="Ex3T1"></a>
#### Task 1 - Add the Load Balanced Endpoint for Port 80 ####

The following steps will take place back on your development workstation.

1. Login the Windows Azure Management Portal (https://manage.windowsazure.com/), and navigate to the **"VIRTUAL MACHINES"** page.  Click on the name of the **"iisvm1*"", and go to the **"ENDPOINTS"** page, and click the **"ADD"** button along the bottom:

	![3020AddEndpoint](images/3020addendpoint.png?raw=true "AddEndpoint")

1. In the **"ADD ENDPOINT"** wizard, on the **"Add an endpoint to a virtual machine"** page, select **"ADD A STAND-ALONE ENDPOINT"** and click the next (arrow) button.  

	![3030StandaloneEndpoint](images/3030standaloneendpoint.png?raw=true "Standalone Endpoint")

	> **Note:** Event though we selected **ADD A STAND-ALONE ENDPOINT"**, we will actually be making it a load balanced endoint by selecting that option on the next page.  

1. On the **"Specify the details of the endpoint"** page, select **"HTTP"** from the **"NAME"** drop-down list, and **turn on** the **"CREATE A LOAD-BALANCED SET"** checkbox.  Ensure that your setting match the following diagram and click the next (right arrow) button:

	![3040HTTPEndpoint](images/3040httpendpoint.png?raw=true "HTTP Endpoint")

1. On the **"Configure the load-balanced set"** page, enter **"webport"** for the **"LOAD-BALANCED SET NAME"**.  Ensure that your settings match the following diagram and click the finish (check mark) button to complete the wizard:

	![3050LoadBalancedSet](images/3050loadbalancedset.png?raw=true "Load Balanced Set")

1. Wait until the operation completes, and the new load-balanced endpoint is created:

	![3060EndpointCreated](images/3060endpointcreated.png?raw=true "Endpoint Created")

1. Switch the the **"iisvm2"** **"ENDPOINTS"** page, and again, click the **"ADD"** button along the bottom:

	![3070AddIisvm2Endpoint](images/3070addiisvm2endpoint.png?raw=true "Add IISVM2 Endpoint")

1. In the **"ADD ENDPOINT"** wizard, on the **"Add an endpoint to the virtual machine"** page, select **"ADD AN ENDPOINT TO AN EXISTING LOAD-BALANCED SET"**, and choose the **"webport"** endpoint we just created from the drop-down list.  Click the next (right arrow) button to continue:

	![3080AddLbEndpoint](images/3080addlbendpoint.png?raw=true "Add Load Balanced Endpoint")

1. On the **"Specify the details of the endpoint"** page, enter **"webport"** for the **"NAME"**.  Ensure your settings match the following diagram and click the finish (check mark) button to complete the wizard:

	![3090CompleteLBEndpoint](images/3090completelbendpoint.png?raw=true "Complete Load Balanced Endpoint")

1. Again, wait for the new endpoint to be created.  At this point we have configured a load balanced endpoint for port 80 in our cloud service that will be used to load-balance web traffic across our ***iisvm1*** and ***iisvm2*** nodes. 

<a name="Ex3T2"></a>
#### Task 2 - Configure both Web Server VMs with the Web Server Role and ASP.NET 4.0 ####

##### You will complete the following steps for both ***iisvm1*** and ***iisvm2***.  The steps are identical for each server.  #####

1. Select the virtual machine (***iisvm1*** or ***iisvm2***) you are configuring in the Windows Azure Management portal, and click the **"CONNECT"** button along the bottom to connect.  Follow the prompts, and connect using the credentials (**CloudShop/Azure$123**) you specified when you created the VM. If you need a reminder on how to connect review the steps used to connect to ***sqlvm1*** in [Exercise 2](#Ex2), Task 1. 

	![3100ConnectToVm](images/3100connecttovm.png?raw=true "Connect to the VM")

1. Open the **"Server Manager"** (again, remember you can use the shortcut on the task bar).  And just as we did with **sqlvm1**, we'll start by turning off the **"IE Enanced Security Configuration"** for the administrator account.  

1. On the **"Server Manager (IISVMx)"** page, click the **"Configure IE ESC"** link, then on the **"Internet Explorer Enhanced Security Configuration"** window, under the **"Administrators:"** heading, select **"Off"** then click **"OK"**:

	![3110TurnOffIEESC](images/3110turnoffieesc.png?raw=true "Turn Off IE ESC")

1. Next, switch to the **"Roles"** page, and click the **"Add Roles"** link:

	![3120AddRoles](images/3120addroles.png?raw=true "Add Roles")

1. In the **"Add Roles Wizard"**, on the **"Before You Begin"** page, click **"Next"**:

	![3130BeforeYouBegin](images/3130beforeyoubegin.png?raw=true "Before You Begin")

1. **Turn On** the **"Application Server"** Checkbox, and when prompted, click the **"Add Required Features"** button:

	![3140ApplicationServer](images/3140applicationserver.png?raw=true "Application Server")

1. Then Select the **"Web Server (IIS)"** option, and click **"Next"**

	![3150WebServerIIS](images/3150webserveriis.png?raw=true "Web Server (IIS)")

1. On the **"Application Server"** page, click **"Next"**:

	![3160ApplicationServerIntro](images/3160applicationserverintro.png?raw=true "Application Server Intro")

1. On the **"Select Role Services"** Page, **turn on** the **"Web Server (IIS) Support"** checkbox, and when prompted click the **"Add Required Role Services"** button.  Then click **"Next"** to continue:

	![3170RoleServices](images/3170roleservices.png?raw=true "Role Services")

1. On the **"Web Server (IIS)"** Page, click **"Next"** to continue:

	![3180WebServerIISIntro](images/3180webserveriisintro.png?raw=true "Web Server IIS Intro")

1. On the **"Select Role Services" page, leave the default selections, and click **"Next"** to continue:

	![3190WebServerRoleServices](images/3190webserverroleservices.png?raw=true "Web Server Role Services")

1. On the **"Confirmation"** page, click **"Install"** and wait while the services install (3-5 minutes):

	![3200Confirmation](images/3200confirmation.png?raw=true "Confirmation")


1. On the **"Installation Results"** page, click **"Close"**, and then close **"Server Manager"**:

	![3210InstallationResults](images/3210installationresults.png?raw=true "Installation Results")

1. The Windows Server image we used already had the .NET 4.0 framework installed, however, because we added IIS AFTER it the .NET Framework was installed, we need to register the latest version of ASP.NET with IIS.

1. On the web server virtual machine, open a command prompt.  and change to the **"C:\Windows\Microsoft.NET\Framework\v4.0.30319"** directory.  Then run **"aspnet_regiis.exe -i"**.

	![3220RunASPNetRegIIS](images/3220runaspnetregiis.png?raw=true "Run aspnet_regiis")

1. Close the command prompt window when you are done. 

1. Make sure you have completed the steps in this task for both ***iisvm1*** and ***iisvm2*** before continuing.

<a name="Ex3T3"></a>
#### Task 3 - Upload the CloudShop website and configure it in IIS on both Web Server VMs ####

##### You will complete the following steps for both ***iisvm1*** and ***iisvm2***.  The steps are identical for each server.  #####

1. On your local machine locate the CloudShop.zip file in the lab's **"Source\Assets\CloudShop"** folder.  Right-click on the local file and select **"Copy"** from the pop-up menu.

	![3230CopyCloudShopZip](images/3230copycloudshopzip.png?raw=true "Copy CloudShop.zip")

1. In the remote desktop connection window for your web server virtual machine (***iismv1*** or ***iisvm2***), right-click on the desktop and select **"Paste"** from the pop-up menu.  This will copy the CloudShop.zip source file into the VM.  

	![3240PasteCloudShopZip](images/3240pastecloudshopzip.png?raw=true "Paste CloudShop.zip")

1. Right-click on the CloudShop.zip file in the target VM, and select **"Extract All..."** from the pop-up menu, then and extract the files to the **"C:\inetpub\wwwroot\CloudShop"** folder.  You will need to type the path in because the folder doesn't currently exist.  

	![3250ExtractCloudShopZip](images/3250extractcloudshopzip.png?raw=true "Extract CloudShop.zip")

1. When the extraction is complete, the **"C:\inetpub\wwwroot\CloudShop"** folder should be opened, verify that the files extracted successfully, then close the **"C:\inetpub\wwwroot\CloudShop"** folder.

1. From the web server VM's start menu, select **"All Programs"** | **"Administrative Tools"** | **"Internet Information Services (IIS) Manager"**

1. In the **"Internet Information Services (IIS) Manager"** window, expand **"IISVMx"** | **"Sites"** | **"Default Web Site"**. then right click on the **"CloudShop"** folder and select **"Convert to Application"**:

	![3260CloudShopApplication](images/3260cloudshopapplication.png?raw=true "Create CloudShop Application")

1. In the **"Add Application"** window, accept the defaults and click **"OK"**:

	![3270AddApplicationWindow](images/3270addapplicationwindow.png?raw=true "Add Application Window")

1. Next we need to edit the wb.config to point the website to the ***sqlvm1*** server that has the AdventureWorks2012 database on it.  With the **"CloudShop"** application selected in IIS Manager, double click on the **"Connection Strings"** icon to edit the connection strings in the web.config file:

	![3280EditConnectionStrings](images/3280editconnectionstrings.png?raw=true "Edit the Connection Strings")

1. Edit **both** the **"AdventureWorksEntities"** and **"DefaultConnection"** connection strings (The **"LocalSqlServer"** connection isn't used by the web site).  Replace the **"[ENTER YOUR SQL SERVER NAME]"** placeholders (including the square brackets) with ***sqlvm1***  (or the name you gave to your SQL VM if you used something different).  Click **"OK"** When you are done.

	![3290EnterSQLVM1Name](images/3290entersqlvm1name.png?raw=true "Enter SQLVM1 Server Name")

1. Close the **"Internet Information Services (IIS) Manager"** window when you are done. 

1. You can test the website on the VM itself by opening IE on the web server VM and navigating to http://localhost/CloudShop.  It should load successfully.  Close the browser when you are done.

	![3300TestingCloudShopOnWebVM](images/3300testingcloudshoponwebvm.png?raw=true "TestingCloudShoponWebVM")

	> **Note:** This test is testing directly from the Web Server VM itself, and is not going through the load balanced endpoint we created earlier.  You should see the server name (IISVMx) show up next to the word "Products" in the header.  Since you aren't going through the load balancer, it will always show the name of the local server. We will test from an external browswer in the next task.  

1. Make sure to complete the steps in this task for both **iisvm1** and **iisvm2** before continuing.

<a name="Ex3T4"></a>
#### Task 4 - Test the Web Site  ####

1. Back on your personal workstation, login to the Windows Azure Management Portal (https://manage.windowsazure.com), and navigate to the **"Cloud Services"** page. 

1. **Right-Click** the the link in the **"URL"** column for the cloud service you created earlier in Exercise 1, and select **"Copy shortcut"** from the pop-up menu: 

	![3310CopyCloudServiceURL](images/3310copycloudserviceurl.png?raw=true "Copy the Cloud Service URL")

1. In a new browser window or tab, pase in the URL and add the **"CloudShop"** application name to the end of it (e.g. http://<your cloud service name>.cloudapp.net/CloudShop or http://cloudshopdemo.cloudapp.net/CloudShop) and navigate to the URL. Verify that the site loads, and note the name of the server that shows up in the header: 

	![3320CloudShopTest](images/3320cloudshoptest.png?raw=true "Testing Cloud Shop")

1. Try refreshing the browser (you may need to do it multiple times) until the other web server name shows up in the header:

	![3330VerifyLoadBalancing](images/3330verifyloadbalancing.png?raw=true "Verify Load Balancing")

1. Finally, try entering a search term  and clicking on the **"Search"** link to verify that the full-text catalog is being searched correctly:

	![3340VerifyFullTextSearch](images/3340verifyfulltextsearch.png?raw=true "Verify Full Text Search")

<a name="Summary"></a>
### Summary ###

Congratulations!  In this lab you configured a Cloud Service with two Web Server VMs and one SQL Server VM.  You configured the SQL VM to host a database, and allow connections from the Web Server, and you uploaded and configured an MVC4 website onto the Web Servers. 
