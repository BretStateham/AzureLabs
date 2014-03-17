<a name="Title"></a>
# Setting up an Azure Virtual Machine For Developers with Visual Studio 2013 Ultimate and SQL Server 2012 Express #

---
<a name="Overview"></a>
## Overview ##

This hands-on lab walks you step by step through the process of creating a **Windows Azure Virtual Machine** that includes **Visual Studio 2013 Ultimate**, **SQL Server 2012 Express and SQL Server 2012 Management Studio Express**, and the **Windows Azure SDK and Tools for Visual Studio 2013**. You will use the virtual machine you create in this lab as the development environment for other labs. 

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a Windows Server 2012 R2 Virtual Machine from the Windows Azure Management Portal
- Remotely manage Azure Virtual Machine instances
- Install additional software into Virtual Machines

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)  

<a name="Setup"></a>
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Start by logging into the **Windows Azure Portal** (https://manage.windowsazure.com) to ensure that you have a valid Windows Azure subscription that you wish to create the Virtual Machine in.  


---
<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

- [Exercise 1: Creating an Azure Virtual Machine using the Windows Azure Virtual Image Gallery](#Exercise1)
- [Exercise 2: Installing Visual Studio 2013 Ultimate Evaluation](#Exercise2)
- [Exercise 3: Installing SQL Server 2012 Express](#Exercise3)
- [Exercise 4: Installing the Windows Azure SDK and Tools](#Exercise4)
- [Exercise 5: Creating Shortcuts for Common Tools](#Exercise5)
- [Exercise 6: How to Stop or Delete your VM](#Exercise6)

---

<!--  
========================================
Exercise 1 
========================================
-->

<a name="Exercise1"></a>
### Exercise 1: Creating an Azure Virtual Machine using the Windows Azure Virtual Image Gallery ###

1. Sign into the **[Windows Azure Management Portal](https://manage.windowsazure.com)** (https://manage.windowsazure.com/) with the Microsoft Account for the subscription you wish to use.
1. On the left menu, select **Virtual Machines**, the click the **"+ NEW"** button in the lower left corner. 

	![Virtual Machines New Button](images/01virtual-machines-new-button.png?raw=true "Virtual Machines New Button")

	_Virtual Machines page in the Manamagement Portal_

1. Click **+ NEW** | **COMPUTE** | **VIRTUAL MACHINE** | **FROM GALLERY**

	![Using The Gallery](images/02using-the-gallery.png?raw=true "Using The Gallery")

	_Creating a New Virtual Machine from the Gallery_

1. Chose the **“Windows Server 2012 R2 Datacenter”** from the list of gallery images, and **click the "next" (right arrow) button** to continue...

	![Windows Server 2012 R2 Datacenter Gallery Image](images/03windows-server-2012-r2-datacenter-gallery-ima.png?raw=true "Windows Server 2012 R2 Datacenter Gallery Image")

	_Windows Server 2012 R2 Datacenter Gallery Image_

	> **Note:** Gallery images are often updated with newer versions.  If the Windows Server 2012 R2 Datacenter image isn't available, choose an appropriate operating system that is Windows Server 2012 R2 or later. 

1. **Supply values** for the following, then **click the next (right arrow) button to continue**: 

	- **Version Release Date:** Pick the latest date available
	- **Virtual Machine Name:**  This will be the Windows computer name
	- **Size:** A **"Medium (2 cores, 3.5GB memory)"** virtual machine should be powerful enough for these labs.  You can choose a smaller size (to save cores or cost) or a larger size (for better performance) as desired.
	- **Built-In Administrator User Name and Password:** Make a note of the admin credentials if needed.  You will need to remember these creditentials to log into the Virtual Machine later.

	![Virtual Machine Configuration](images/04virtual-machine-configuration.png?raw=true "Virtual Machine Configuration")

	_Virtual Machine Configuration_

	> **Note:** The user name and password set the credentials for the Windows Built-In Administrator account.  

1. **Supply values as follows** for the new Cloud Service, and **click the next (right arrow) button to continue**:

	- **Cloud Service:** Choose **"Create a new cloud service"** 
	- **Cloud Service DNS Name**: Choose a valid host name for the cloud service.  It will default to the same name as the Virtual Machine (from the previous step).  Change it to create a valid, unique name if needed. You will know if the name is valid when the green circle with the checkmark appears: ![Valid Checkmark](images/valid-checkmark.png?raw=true "Valid Checkmark")
	- **Region / Affinity Group / Virtual Network:** Pick a region (data center) close to you.
	- **Storage Account:** Select **"User an automatically generated storage account"**
	- **Availability Set:** Select **"(None)"**  

	![Cloud Service Configuration](images/05cloud-service-configuration.png?raw=true "Cloud Service Configuration")

	_Cloud Service Configuration_

1. **Leave the Endpoints at the default** (Remote Desktop & PowerShell) and **allow the VM agent to be installed**, **Click the ok (checkmark) button to complete the wizard**:

	![Endpoints Configuration](images/06endpoints-configuration.png?raw=true "Endpoints Configuration")

	_Endpoint Configuration_

	> **Note:** Note that the **"Install the VM Agent"** checkbox is checked.  The VM Agent manages the use of "Extensions" on your VMs.  Extensions help by performing tasks in your VM, like setting the background wall paper (BGInfo) and allowing you to reset the built-in Administrator Credentials (VMAccess).  Learn more about the VM Agent, and the extensions here: http://msdn.microsoft.com/en-us/library/dn606311.aspx 

1. **It will take approximately 10 minutes for the new Virtual Machine to be provisioned**.  Now might be a good time to take a quick break if needed.  While the virtual machine is being provisioned, you can monitor it's status on the **"VIRTUAL MACHINES"** page of the [Windows Azure Management Portal](https://manage.windowsazure.com/):

	![Virtual Machine Provisioning Status](images/07virtual-machine-provisioning-status.png?raw=true "Virtual Machine Provisioning Status")

	_Virtual Machine Provisioning Status_

1. Once the new virtual machine's status is **“Running”**, select it in the Management Portal and click the **“CONNECT”** button along the bottom:

	![Connect to the new VM](images/08connect-to-the-new-vm.png?raw=true "Connect to the new VM")

	_CONNECT Button_

1. The management portal will display a notification that an ".rdp" file will be retrieved.  Click "OK" to clear the notification:

	![RDP File Retrieval Notification](images/09rdp-file-retrieval-notification.png?raw=true "RDP File Retrieval Notification")

	_".rdp" File Notification_

1. Internet Explorer will prompt you to open or save the ".rpd" file. Click the "Open" button to download and open the ".rdp" file:

	![Open the RDP File](images/10open-the-rdp-file.png?raw=true "Open the RDP File")

	_Open the ".rdp" file_

1. In the **"Remote Desktop Connection"** publisher verification window click the **"Connect"** button to continue:

	![RDP Publisher Verification](images/11rdp-publisher-verification.png?raw=true "RDP Publisher Verification")

	_".rdp" File Publisher Verfication_

1. Enter the **User Name and Password** you specified for the built-in administrator account earlier to connect to the Virtual Machine:

	![Administrator Credentials](images/12administrator-credentials.png?raw=true "Administrator Credentials")

1. In the **"Remote Desktop Connection"** Identity Verfication window, **click "Yes"** to continue:

	![RDP Identity Verification](images/13rdp-identity-verification.png?raw=true "RDP Identity Verification")

	_Remote Computer Identity Verfication_

1. In the **"Remote Desktop Connection"** window, if you are asked **"Do you want to find PCs, ..."** click **"Yes"**:

	![Enable Network Discovery](images/14enable-network-discovery.png?raw=true "Enable Network Discovery")

	_Enable network discovery_

1. On the remote machine, if **"Server Manager"** is not running, you can launch it from the Server Manager icon on the taskbar:

	![Server Manager Icon](images/15server-manager-icon.png?raw=true "Server Manager Icon")

	_Server Manager icon_


1. In the Remote Desktop window, in the **Server Manager**, click the **“Configure this local server”** link along the top:

	![Configure Local Server](images/16configure-local-server.png?raw=true "Configure Local Server")

	_Configure the local server_

1. Next to **“IE Enhanced Security Configuration”** click the **“On”** link:


	![IE Enhanced Security On](images/17ie-enhanced-security-on.png?raw=true "IE Enhanced Security On")

1. In the **"Internet Explorer Enhanced Security Configuration"** window, change both **“Administrators”** and **“Users”** to **“Off”**. Click **“OK”**, and then close the Server Manager window:

	![Turn IE ESC Off](images/18turn-ie-esc-off.png?raw=true "Turn IE ESC Off")

	_Turn "Internet Explorer Ehnanced Security Configuration" off_

1. The administrative account we created with the VM is the Built-In administrator account.  If our goal is to use this VM to build Windows 8.1 store apps though, we need to create another account to use as our developer account because the Built-in administrator account can't install or "side-load" store apps. This is due to how **"User Acccount Control"** Or **UAC** is used when running store apps.  In the next steps, we'll create another user account who is also a member of the **"Administrators"** group, but just isn't the special **Built-In Administrator**.  We'll then do all of our future work as this new account.

1. In the **"Server Manager"** window, click **"Dashboard"** along the left, then from the menu in the top right corner, select **"Tools"** | "**Computer Management"**

	![19ComputerManagement](images/19computermanagement.png?raw=true "Computer Management Link")

	_Computer Management Link_

1. In the **"Computer Management"** window, expand **"Computer Management (Local)"** | **"System Tools"** | **"Local Users and Group" | **"Users"**.  **Right-click** on the **"Users"** node, and select **"New User..." from the pop-up menu

	![19NewUser](images/19newuser.png?raw=true "New User")

	_New User_
	
1. In the **"New User"** window, give the new user a **user name**, **password**, and set the **password to never expire**, and click **"Create"**:

	![19NewUserDetails](images/19newuserdetails.png?raw=true "New User Details")

	_New User Details_

1. **Right-click** on **the new user account** and select **"Properties" from the pop-up menu:

	![19NewUserProperties](images/19newuserproperties.png?raw=true "New User Properites")

	_New User Properties_

1. In the new user **"Properties"** window, switch to the **"Member Of"** tab, and click the **"Add..."** button

	![19AddGroup](images/19addgroup.png?raw=true "Add Group")

	_Add Group_

1. In the **"Select Groups"** window, under **"Enter the object names to select"** type **Administrators** and click the **OK** button.  This will add the user user to the Built-in Administrators group and give it all the permissions it needs.  

	![19Administrators](images/19administrators.png?raw=true "Administrators")

	_Administrators_

1. Back on the **"Member Of"** tab, confirm that the **"Administrators"** group appears, and click **"OK"** to close the dialog.  

	![19ConfirmAdministrators](images/19confirmadministrators.png?raw=true "Confirm Administrators")
	
	_Confirm Administrators_

1. Close the **"Computer Management"** window when you are done.

	![19CloseComputerManagement](images/19closecomputermanagement.png?raw=true "Close Computer Management")

	_Close Computer Management_

1. Finally, the last thing we need in order to develop Windows 8.1 apps on our Windows Server 2012 R2 image, is the **"Desktop Experience"** feature.  This puts the **"Store"** icon on the start screen and enables the features necessary to run Windows 8.1 store apps on our server.  

1. Back in the **"Server Manager"** window, ensure that you are on the **"Dashboard"** and click the **"2 Add roles and features"** link:

	![19AddRolesAndFeatures](images/19addrolesandfeatures.png?raw=true "Add roles and features")

	_Add Roles and Features_

1. In the **"Add Roles and Features Wizard" accept the default values and click **"Next"** for the first four pages (**"Before You Begin"**, **"Installation Type*"", **"Server Selection"**, and **"Server Roles"**):

	![19AcceptFirstFourPageDefaults](images/19acceptfirstfourpagedefaults.png?raw=true "Accept the defaults")

	_Accept the defaults for the first four pages_

1. On the **"Features"** page of the wizard, scroll down, and expand **"User Interfaces and Infrastructure (2 of 3 installed)**.  Then **turn on** the checkbox for **"Desktop Experience"**

	![19DesktopExperience](images/19desktopexperience.png?raw=true "Desktop Experience")

	_Desktop Experience_

1. When prompted, click the **"Add Features"** button to confirm the addition of the **"Ink and Handwriting Services" as well as **"Media Foundation"**:

	![19AddFeatures](images/19addfeatures.png?raw=true "Add Features")

	_Add Features_

1. Click **"Next"** on the **"Features"** page:

	![19FeaturesNext](images/19featuresnext.png?raw=true "Features")

	_Features_

1. On the **"Confirmation"** page, click **"Install"** and wait for the features to install:

	![19Install](images/19install.png?raw=true "Install")

	_Install_

1.  When the installation is complete, on the Results tab, note that a reboot is required before the new features will be available.  Click the **"Close"** button

	![19Results](images/19results.png?raw=true "Results")

	_Results_

1. Close the remote desktop connection window, and return to the **"VIRTUAL MACHINES"** page in the Management Portal.  With your development Virtual Machine selected (click on the row to the right of the VM name), click the **"RESTART"** button along the bottom, and wait until the **"STATUS" reads **"RUNNING"** again.  

	![19RestartVM](images/19restartvm.png?raw=true "Restart VM")

	_Restart the VM_

1.  Once the VM status has returned to **"Running"** repeat the process we used earlier to connect to the VM using Remote Desktop:

	![19ConnectViaRDP](images/19connectviardp.png?raw=true "Connect via RDP")

	_Connect via Remote Desktop_

1. This time however, connect using the credentials of the new user account you just created in the previous steps:

	![19LoginAsDevUser](images/19loginasdevuser.png?raw=true "Login as new development user")

	_Login as the new development user_

1. At this point we have:

	- Built a **Windows Server 2012 R2 Virtual Machine** in Windows Azure
	- Created an **administrative developer account** that can be used to debug
	- Enabled the **"Desktop Experience"** so we can develop Windows 8.1 apps
Windows 8.1 apps.

In the next exercise we'll install the Visual Studio 2013 Ultimate Evaluation.

---

<!--  
========================================
Exercise 2 
========================================
-->

<a name="Exercise2"></a>
### Exercise 2: Installing Visual Studio 2013 Ultimate Evaluation ###

In this exercise, we'll install a **evaluation** edition of **Visual Studio 2013 Ultimate** into the new Virtual Machine we just created. ***It should be noted that if you have an MSDN subscription that provides Visual Studio 2013 Ultimate to you, then you may want to start the download from there rather than using the trial link provided in these steps.***  

**The steps in this exercise should all be performed in using a Remote Desktop Connection to the Virtual Machine we created in [Exercise 1](#Exercise1).**

1. In the **Remote Desktop** window to the virtual machine we just created, click the **“Start Button”** in the lower left corner of the desktop to return to the **Start Screen**:

	![Start Button](images/19start-button.png?raw=true "Start Button")

1. From the **Start Screen**, click **“Internet Explorer”**

	![Start Internet Explorer](images/20start-internet-explorer.png?raw=true "Start Internet Explorer")
	
	_Open Internet Explorer_

1. If you are prompted to **"Set up Internet Explorer 11"**, select **"Use recommended security, privacy, and compatability settings"** and click **"OK"**:

	![Setup Internet Explorer 11](images/21setup-internet-explorer-11.png?raw=true "Setup Internet Explorer 11")

	_Setup Internet Explorer 11_

1. Go to **<http://aka.ms/vs13trial>**, and click the **"Download"** button: 

	![Download Visual Studio 2013](images/22download-visual-studio-2013.png?raw=true "Download Visual Studio 2013")

	_Downlaod Visual Studio 2013_

1. Select the **“vs_ultimate.exe”** (1.1MB) download and click **“Next”**:

	![Visual Studio 2013 Installer](images/23visual-studio-2013-installer.png?raw=true "Visual Studio 2013 Installer")

	_Visual Studio 2013 Installer_

1. IE will block the pop-up. Click **“Allow once”**:

	![Allow Pop-Up](images/24allow-pop-up.png?raw=true "Allow Pop-Up")

	_Allow the Pop-up"

1. Then, when prompted, click **“Run”** to start the Visual Studio 2013 Installation after the download:

	![Run Installer](images/25run-installer.png?raw=true "Run Installer")

	_Run the installer after the download_

1. Once the **Visual Studio Ultimate 2013** installation begins, check the **"I agree to the License Terms and Privacy Policy"** checkbox, and click **"Next"**:

	![Start VS 2013 Install](images/26start-vs-2013-install.png?raw=true "Start VS 2013 Install")

	_Visual Studio 2013 Installation_

1. In the Visual Studio 2013 Installation, **select all** options **_except_ “Windows Phone 8.0. SDK”**, and click **"INSTALL"**:

	![VS 2013 Options](images/27vs-2013-options.png?raw=true "VS 2013 Options")

	_Visual Studio 2013 Options_

	> **Note:** If you know **you won't be doing Windows Store App development** in this VM, you can **leave the "Tools for Maintaining Store apps for Windows 8" de-selected** to speed up the download and install. 

	> **Note:** The installation will **take approximately 20-30 minutes**.  Now is a good time to stretch your legs, or to read ahead in the lab.  

1. Once Visual Studio 2013 installation has completed, click **“LAUNCH”** to start it.

	![Launch Visual Studio 2013](images/28launch-visual-studio-2013.png?raw=true "Launch Visual Studio 2013")

	_Launch Visual Studio 2013_
 
1. **Sign in with your Microsoft Account** (Live ID) to allow Visual Studio to sync your preferences, **or click “Not now maybe later” and select a theme**. 

	![Sign In](images/29sign-in.png?raw=true "Sign In")

	_Sign in with your Microsoft Account_

1. Once, Visual Studio is running, install any additional updates.  From the Visual Studio menu bar, select **"TOOLS"** | **"Extensions and Updates..."**

	![30ExtensionsAndUpdates](images/30extensionsandupdates.png?raw=true "Extensions and Updates")

	_Extensions and Updates_

1. In the **"Extensions and Updates"** window, expand **"Updates"** | **"Product Updates"** along the left hand side, and install any Visual Studio Updates as necessary. Follow the prompts for each update and install them accordingly.  Once all desired updates have been applied.  You will likely need to close Visual Studio to allow the installation of certain updates:

	![30VSUpdates](images/30vsupdates.png?raw=true "Visual Studio Updates")

	_Visual Studio Product Updates_

1. Finally, if necessary **close Visual Studio 2013**. 

	![Close Visual Studio](images/30close-visual-studio.png?raw=true "Close Visual Studio")

	_Close Visual Studio 2013_

Great, you now have a virtual machine that you can use for general development purposes.  In the next exercise, we'll install SQL Server 2012 Express With Tools, which includes SQL Server Management Studio Express. 

---

<!--  
========================================
Exercise 3
========================================
-->

<a name="Exercise3"></a>
### Exercise 3: Installing SQL Server 2012 Express ###

In a later exercise, we'll be migrating an "on-premise" database into either an Azure SQL Database or another Azure VM with a full version of SQL Server 2012 installed.   Visual Studio 2013's "SQL Server Data Tools" has some basic functionality to support that, but a more complete set of database migration features is supported in the SQL Server Management Studio tool.  In this exercise, we'll install SQL Server 2012 Express (that will later be used to host our "on-premise" database) and SQL Server Management Studio Express (SSMS).  SSMS supports migrating both the database structures _and their data_ to Azure. 

**The steps in this exercise should all be performed in using a Remote Desktop Connection to the Virtual Machine we created in [Exercise 1](#Exercise1).** 

1. In the **Remote Desktop** window, again, open **Internet Explorer**, go to **<http://aka.ms/sqlexp12>** , and click the **"Download"** Button:

	![Download SQL Server 2012 Express](images/31download-sql-server-2012-express.png?raw=true "Download SQL Server 2012 Express")

	_Download SQL Server 2012 Express_

1. Select the **“ENU\x64\SQLEXPRWT_x64_ENU.exe”** (669.9MB) download. This is a 64bit installation of **SQL Server Express WITH TOOLS** (that includes the SQL Server Management Studio Express) and click **“Next”**:

	![Download SQL Express with Tools](images/32download-sql-express-with-tools.png?raw=true "Download SQL Express with Tools")

	_Download 64Bit SQL Server 2012 Express With Tools_

	> **Note:** Make sure to get the file with **"WT"** in the name (“ENU\x64\SQLEXPR**WT**_x64_ENU.exe”).  The **"WT"** means **"With Tools"** and includes **SQL Server Management Studio** Express (**SSMS**) in the installation.  **We'll need SSMS later to migrate databases into Azure**. 

1. IE will block the pop-up. Click **“Allow once”** to continue, then click "Run" to download and start the SQL Server 2012 Express Installation:

	![Install SQL Express with Tools](images/33install-sql-express-with-tools.png?raw=true "Install SQL Express with Tools")

	_Download and run the SQL Server 2012 Express With Tools Installation_

	> **Note:** The installation file is 670MB in size, but it should still come down quickly.  Remember the download is being done within an Azure VM running in a Microsoft Datacenter.  Those data center's have plenty of bandwidth to support fast downloads.

1. When the **"SQL Server Installation Center"** appears, click **“New SQL Server stand-alone installation or add features to an existing installation”**:

	![New Installation](images/34new-installation.png?raw=true "New Installation")

	_Start a new installation_

1. **Accept the license terms** and click **"Next"** to continue:

	![Accept the License Terms](images/35accept-the-license-terms.png?raw=true "Accept the License Terms")

	_Accept the license terms_

1. **If there are any updates** to be installed (there may not be), ensure that **"Include SQL Server product updates"** is **checked** and **click "Next"** to continue:

	![Install SQL Product Updates](images/36install-sql-product-updates.png?raw=true "Install SQL Product Updates")

	_Install any SQL Server product updates_

	> **Note:** The product updates (if any) may take a few minutes to install after you move to the next step.  Be patient and wait for them to install.

1. If you are shown a **"Computer restart required"** notification, **click "OK"** to clear it.  **We will restart the virtual machine at the end of the exercise**. 

	![Computer Restart Required Notification](images/37computer-restart-required-notification.png?raw=true "Computer Restart Required Notification")

	_Computer Restart Required Notification_

1.  On the **"Feature Selection"** page, **ensure that all features are selected** and **click "Next"** to continue:

	![Select All Features](images/38select-all-features.png?raw=true "Select All Features")

	_SQL Server Feature Selection_

1. On the **"Instance Configuration"** page, ensure that the **"Named instance:"** name and **"Instance ID:"** are both **"SQLEXPRESS"** and click **"Next"** to continue:  

	![SQLEXPRESS Instance](images/39sqlexpress-instance.png?raw=true "SQLEXPRESS Instance")

	_**SQLEXPRESS** Instance Configuration_

1. On the **"Server Configuration"**, **"Database Engine Configuration"** and **"Error Reporting"** pages, **accept the defaults** and click **"Next"** to continue:

	![Accept Configuration Defaults](images/40accept-configuration-defaults.png?raw=true "Accept Configuration Defaults")	

	_Accept configuration defaults_

	> **Note:** The installation will begin.  This **installation will take approximately 20-30 minutes** to complete.  

1. Again, if given a **"Computer Restart Required"** notification, **click "OK"** to clear it. **We will restart the vm in a couple of steps**.  

	![Computer Restart Required Notification](images/41computer-restart-required-notification.png?raw=true "Computer Restart Required Notification")

	_Computer Restart Required Notification_

1. Once the SQL Server 2012 Express installation is complete, click "Close" to exit, and close any other windows open in the virtual machine. 

	![Close the Installation](images/42close-the-installation.png?raw=true "Close the Installation")

	_Close the installation and any other windows_

1. **COMPLETE THIS STEP ON YOUR LOCAL MACHINE, _NOT_ IN THE REMOTE DESKTOP CONNECTION FOR THE VIRTUAL MACHINE**: Open the **[Windows Azure Management Portal](https://manage.windowsazure.com/)** (<https://manage.windowsazure.com>) and go to the **"VIRTUAL MACHINES"** page.

1. On the **"VIRTUAL MACHINES"** page, **click to the right of the name of your development workstation VM** (clicking on the name itself opens the dashboard for the VM, and we don't need that right now).  **With the VM selected**, along the bottom, **click the "RESTART" button** to shutdown and restart the virtual machine:

	![Restart the Development Workstation VM](images/43restart-the-development-workstation-vm.png?raw=true "Restart the Development Workstation VM")

	_Restart the Developer Workstation VM_

1. When prompted click "Yes" to confirm the restart:

	![Confirm the VM Restart](images/44confirm-the-vm-restart.png?raw=true "Confirm the VM Restart")

1. **Wait for the status of your Virtual Machine to return to "Running"** before going on to the next exercise:

	![VM Restarted](images/45vm-restarted.png?raw=true "VM Restarted")

	_Wait for the VM status to be "Running" again_

We're getting close to done!  We now have a Windows Server 2012 Virtual Machine with Visual Studio Ultimate 2013, SQL Server 2012 Express & SQL Server Management Studio.  We just need to install the Windows Azure SDK, the Lab Files, setup some shortcuts and we're done.  

---

<!--  
========================================
Exercise 4
========================================
-->

<a name="Exercise4"></a>
### Exercise 4: Installing the Windows Azure SDK and Tools ###

In this exercise, we'll install the Azure SDK and Tools for Visual Studio into our development workstation Virtual Machine.

**All but the first step in this exercise should all be performed in using a Remote Desktop Connection to the Virtual Machine we created in [Exercise 1](#Exercise1).** 


1. In the **Windows Azure Management Portal**, once the Virtual Machine status reads “Running” again (after having been restarted above), select the development Virtual Machine and click the **“CONNECT”** button along the bottom, and again, **follow the prompts** to connect to the VM using Remote Desktop:

	![Reconnect to the VM](images/46reconnect-to-the-vm.png?raw=true "Reconnect to the VM")

	_Reconnect to the Virtual Machine using Remote Desktop_

1. In the **Remote Desktop Window**, open **Internet Explorer**, and go to **<http://aka.ms/azsdks>**, and click the **"VS 2013"** link:

	![Downlaod the Azure SDK for VS2013](images/47downlaod-the-azure-sdk-for-vs2013.png?raw=true "Downlaod the Azure SDK for VS2013")

	_Download the Azure SDK and Tools for VS 2013_

1. Click **“Run”** to start the installation of the Windows Azure SDK:

	![Run the Azure SDK Installer](images/48run-the-azure-sdk-installer.png?raw=true "Run the Azure SDK Installer")

	_Run the Azure SDK Installer_

1. In the **“Web Platform Installer”** window click the **“Install”** button to install the SDK and tools:

	![Install the Azure SDK](images/49install-the-azure-sdk.png?raw=true "Install the Azure SDK")

	_Web Platform Installer_

1. On the **"PREREQUISITES"** page click **"I Accept"**:

	![Accept Prerequisites](images/50accept-prerequisites.png?raw=true "Accept Prerequisites")

	_Accept the Prerequisites_

	> **Note:** **This installation is quick** and should only take **five minutes** at most.  

1. When the installation is complete, if prompted, click "Continue" to continue:

	![Continue Installation](images/51continue-installation.png?raw=true "Continue Installation")

	_Continue the Installation_

1. A browser window with details about the Windows Azure SDK may appear.  If it does, read as much about the SDK as you like, then **close the browser window**:

	![New SDK Details](images/52new-sdk-details.png?raw=true "New SDK Details")

	_Read about the changes in the latest Azure SDK_

1. Back in the **"Web Platform Installer"** window, click **"Finish"**:

	![Finish the Installation](images/53finish-the-installation.png?raw=true "Finish the Installation")

	_Finish the Installation_

1. Close the **"Web Platform Installer"** window by clicking "Exit":

	![Exit Web PI](images/54exit-web-pi.png?raw=true "Exit Web PI")
	
	_Exist the Web Platform Installer_

Great!  Now we can create Azure Cloud Service projects in Visual Studio!  Next, we'll grab the lab files.  

---

<!--  
========================================
Exercise 5
========================================
-->

<a name="Exercise5"></a>
### Exercise 5: Creating Shortcuts for Common Tools ###

In this last exerise, we'll setup some shortcuts for both Visual Studio and SQL Server Management Studio on the Start Screen and taskbar.  This will make it easier to launch those tools in subsuequent labs.  Feel free to add any other shortcuts you like! 

**The steps in this exercise should all be performed in using a Remote Desktop Connection to the Virtual Machine we created in [Exercise 1](#Exercise1).** 

1. In the Remote Desktop window, return to the Start Screen and clic the "down arrow" icon along the bottom to see all apps on the machine:

	![All Apps Button](images/63all-apps-button.png?raw=true "All Apps Button")

	_All Apps Button_

1. Scroll through the list of apps to find **“Visual Studio 2103”**.  **Right-click** the “Visual Studio 2013” icon and select **“Pin to taskbar”**, the **repeat** and select **"Pin to Start"**:

	![Pin Visual Studio ](images/64pin-visual-studio.png?raw=true "Pin Visual Studio")

	_Pin Visual Studio 2013 to the Taskbar and to Start_

1. Repeat the process to pin SQL Server Management Studio to both the taskbar and to Start:

	![Pin SSMS](images/65pin-ssms.png?raw=true "Pin SSMS")

	_Pin SQL Server Management Studio to the Taskbar to Start_
	
1. Once you are done, you should now have convenient shortcuts for both Visual Studio 2013 and SQL Server Management Studio on your Start Screen:

	![Shortcuts on Start Screen](images/66shortcuts-on-start-screen.png?raw=true "Shortcuts on Start Screen")

	_Shotcuts on Start Screen_

1. And on the taskbar:

	![Shortcuts on Taskbar](images/67shortcuts-on-taskbar.png?raw=true "Shortcuts on Taskbar")


All done!  

---

<!--  
========================================
Exercise 6
========================================
-->

<a name="Exercise6"></a>
### Exercise 6: How to Stop or Delete your VM ###

#### When you are done working with your VM and you don't need it to be running, you need to at least stop it, or if you never want to use it again, delete it####

Remember that while your VM is running it is incurring execution costs.  How much the VM costs depends on the Size you picked when you created the VM.  You can help reduce those costs when you are done working with a VM one of two ways:

- Stop the VM. This stops any execution costs, but you will still be paying for the storage of the Virtual Hard Disks (*.vhd files).
- Delete the VM and all attached Disks - This gets rid of the VM and all of its attached disks, but it doesn't affect Cloud Service or any other VMs in the Cloud Service
- Delete the Cloud Service and all its deployments including all VMs (and their VHDs) in the cloud service.  

In this exercise, we'll show you how to stop the VM to prevent execution costs. 

1. With the Development VM selected on the **"VIRTUAL MACHINES" page in the management portal, click the **"SHUT DOWN"** link along the bottom, then click **"YES"** to confirm the release of it's IP addresses and to shut down the machine.  Once the status reads **"Stopped (Deallocated)"** you are no longer being billed execution costs.  Remember however that you are still paying for the storage of the Virtual Hard Drive:

	![68ShutdownVM](images/68shutdownvm.png?raw=true "Shutdown VM")

	_Shutdown VM_

1. The next choice you have is to delete the VM and it's attached disks.  This makes sure that not only are you no longer being charged for the VM execution, but you are also no longer being charged to store it's Virtual Hard Disks.  To do so, again with the VM selected on the management portal's **"VIRTUAL MACHINES"** page, click the **"DELETE"** button along the bottom, then select **"

	![69DeleteVM](images/69deletevm.png?raw=true "Delete VM")

	_Delete the Virtual Machine and Attached Disks_

1. The third choice is to delete the entire Cloud Service, with all Virtual Machines, Web Roles, Worker Roles, VHDs, etc.  This is the most complete way to remove all items associated with a Cloud Service.  To do so, switch to the **"CLOUD SERVICES"** page, and select the cloud service you wish to delete (make sure to pick the right one).  Then click the **"DELETE"** button along the bottom, and select **"Delete the cloud service and its deployments."**:

	![70DeleteCloudService](images/70deletecloudservice.png?raw=true "Delete the Cloud Service")

	_Delete the Cloud Service and all its Deployments_



<!--  
========================================
Summary
========================================
-->

<a name="Summary"></a>
## Summary ##

This hands-on lab walked you step by step through building a Windows Azure Virtual Machine with the following:

 - Windows Server 2012 R2
 - Visual Studio Ultimate 2013
 - SQL Server 2012 Express with Tools
 - Windows Azure SDK and Tools for Visual Studio 2013

Feel free to customize the virtual machine to your liking.  You can use this virtual machine as the "development workstation" for future labs.  When you do that, it will help if you think of the VM as if it were a local workstation or laptop.  

***Lastly, make sure that you shutdown the Virtual Machine when you are not using it.  Windows Azure Virtual Machines that are shutdown have a "Stopped (deallocated)" status and do NOT INCUR HOURLY CPU USAGE COSTS.  If you leave your VM running however, those costs will continue to be charged.***