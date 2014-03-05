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

1. In the "CREATE A VIRTUAL MACHINE WIZARD", on the "Coose an Image" page, scroll down to find and select the "SQL Server 2012 SP1 Enterprise on Windows Server 2008 R2" image and click the next (arrow) button.

	![1040SQLImage](images/1040sqlimage.png?raw=true "SQL Server 2012 SP1 Enterprise on Windows Server 2008 R2")

1. On the "Virtual machine configuration" page:
	- VERSION RELEASE DATE: ***Choose the latest date***
	- VIRTUAL MACHINE NAME: ***sqlvm1*** (Use this name.  using a different name will cause issues later in the lab).
	- SIZE: ***Small (1 core, 1.75 GB memory)*** (You don't need anything bigger for this lab)
	- NEW USER NAME: ***CloudShop***
	- NEW PASSWORD:  ***Azure$123***

	| **Note:** You can use a different user name and password, but it is recommended that you use the values above for consistency.  These will be used as the user name and password for the built-in administrator account in the Windows virtual machine.  Regardless of what you use, it is highly recommended that you use the same credentials for all three Virtual Machines to help eliminate any confusion during the lab. 

	![1050VMConfig](images/1050vmconfig.png?raw=true "Virtual Machine Configuration")

1. On the second **"Virtual machine configuration"** page, enter:
	- CLOUD SERVICE: ***Create a new cloud service***
	- CLOUD SERVICE DNS NAME: ***Enter a unique valid DNS host name.***.  
	The name CloudShopDemo is used as a sample throughout this lab, but you will need to use your own unique name instead.
	- REGION/AFFINITY GROUP/VIRTUAL NETWORK: ***Pick a region close to you***
	- STORAGE ACCOUNT: ***Use an automatically generated storage account***
	- AVAILABILITY SET: ***(None)***

	![1060VMConfig2](images/1060vmconfig2.png?raw=true "Virtual Machine Configuration")

1. On the third final **"Virtual machine configuration"** page, accept the default values and click the "Check Mark" button in the lower right corner to complete the wizard.

	![1070VMConfig3](images/1070vmconfig3.png?raw=true "Virtual Machine Configuration Page 3")

	| **Note:** The Virtual Machine already has endpoints for Remote Desktop connections as well as for remote PowerShell scripts.  

1. In the portal, you should now see the ***sqlvm1*** virtual machine with a status of **Starting (Provisioning)**.  Wait until the status reads just **Starting** (about 1-2 minutes) before proceeding with the next task. 

	![1080SQVM1Provisioning](images/1080sqvm1provisioning.png?raw=true "SQLVM1 Provisioning")

<a name="Ex1T2"></a>
#### Task 2 - Adding the Web Server Virtual Machines to the Cloud Service ####

In this task, you will create BOTH of the Web Server Virtual Machines, **iisvm1** and **iismv2**.  The steps to create them are identical with the exception of the name.  

1. Wait until the previous virtual machine has been provisioning for a couple of minutes. 

1. Click the +NEW Button along the bottom and select **COMPUTE** | **VIRTUAL MACHINE** | **FROM GALLERY**

	![1090NewVMFromGallery](images/1090newvmfromgallery.png?raw=true "New VM From Gallery")

1. In the **"CREATE A VIRTUAL MACHINE"** wizard, on the **"Choose an Image"** page, select the "Windows Server 2008 R2 SP1" image and click the next (arrow) button to continue:

	![1100Win2K8VMImage](images/1100win2k8vmimage.png?raw=true "Windows Server 2008 R2 SP1 Image")

1. On the first **"Virtual machine configuration"** page:
	- VERSION RELEASE DATE: ***Choose the latest date***
	- VIRTUAL MACHINE NAME: ***iisvm1*** for the first vm, ***iisvm2*** for the second
	- SIZE: ***Small 1 core, 1.75 GB memory)***
	- NEW USER NAME: ***CloudShop*** (or the same name you used for ***sqlvm1***)
	- NEW PASSWORD:	***Azure$123*** (or the same password you used for ***sqlvm1***)

	![1110VMConfig2](images/1110vmconfig2.png?raw=true "Virtual Machine Configuration Page 2")

1. On the second **"Virtual machine configuration"** page:
	- CLOUD SERVICE: Choose the cloud service you created previously while creating ***sqlvm1***.
	- Leave all the other options at the default

	![1120VMConfig2](images/1120vmconfig2.png?raw=true "Virtual Machine Configuration Page 2")

1. On the third and final **"Virtual machine configuration"** page, leave the default values and click the "Check Mark" button in the lower right corner to complete the wizard.  

	![1130VMConfig3](images/1130vmconfig3.png?raw=true "Virtual Machine Configuration Page 3")

1. After creating **iisvm1** (and waiting 1-2 minutes to continue) then repeat the steps above to create **iisvm2**

1. Once all three virtual machines have been created, wait until their status reads "Running" before proceeding to the next task.  This could take anywhere from 10-30 minutes.  Now is a good time to take a break! 



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

<a name="Ex2T2"></a>
#### Task 2 - Downloading and Attaching the AdventureWorks2012 Database ####

<a name="Ex2T3"></a>
#### Task 3 - Creating the "CloudShop" SQL Server Login ####

<a name="Ex2T4"></a>
#### Task 4 - Allowing SQL Server Connections through the Firewall ####

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

<a name="Ex3T2"></a>
#### Task 2 - Configure both Web Server VMs with the Web Server Role and ASP.NET 4.0 ####

<a name="Ex3T3"></a>
#### Task 3 - Upload the CloudShop website and configure it in IIS on both Web Server VMs ####

<a name="Ex3T4"></a>
#### Task 4 - Test the Web Site  ####
