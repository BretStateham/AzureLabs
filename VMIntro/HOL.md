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

<a name="Ex1T2"></a>
#### Task 2 - Adding the Web Server Virtual Machines to the Cloud Service ####

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
