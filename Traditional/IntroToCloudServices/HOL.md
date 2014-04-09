<a name="IntroToCloudServices"></a>
#Introduction to Cloud Services#

---
<a name="Overview"></a>
## Overview ##

There are many ways you can run code in Azure.  The choice you make depends on what kind of control you need over the base operating system, and what kind of workloads you need to run.  One of the ways you can run code in Azure is in a **"Cloud Service"** using one or more **"Web Role"** or **"Worker Role"** virtual machine instances.

"Web Role" virtual machines are **Windows Servers (you can pick which version) with IIS installed**, whereas **"Worker Role" virtual machines are Windows Servers _without_ IIS installed**. The great thing about Web Roles is that YOU don't need to maintain the operating systems or virtual machines.  Azure guarantess that the VMs are up to date, and will automatically replace them with fresh versions if they fail.  

A cloud service often consists of one or more web roles and/or worker roles, each with its own application files and configuration. Web roles provide a dedicated Internet Information Services (IIS) web server that can be used for hosting the web front-end of your cloud service. Application code hosted within worker roles can run tasks in the background that are asynchronous, long-running, or perpetual.

The other thing you need to understand about "Web Role" and "Worker Role" virtual machines is that they are **"stateless"**. That means that if the Azure fabric controler (the software that runs the Azure Data Center) needs to **recycle or replace your role VM, the data that was placed there while it was running will be lost**.  BUT THAT IS A GOOD THING.  That means that as long as you **store your data and configuration off the virtual machine** (like in Azure Storage or in an Azure SQL Database) you can access it from any role instance. The other great thing then is that you can run as many copies of your role instances as you like!  

To develop your application, you can develop locally on your workstation using the Compute and Storage emulators that ship with the Azure SDK.  You then create configuration information that tells the Fabric Controller what kind of role instances you need, and which code you have developed goes on which instances.  You bundle all of your compliled code and configuration information into a "Package" and send it up to the Azure Fabric Controller in the data center of your choice.  The fabric controller then reads the configuration, spins up the necessary Web Role and Worker Role instances, deploys your code to them, then monitors them all to make sure they are healthy! By creating a cloud service, you can deploy a multi-tier application in Windows Azure, defining multiple roles to distribute processing and allow flexible scaling of your application.

![webworker](images/0010webworker.jpg?raw=true)

_Web Roles and Worker Roles_

Storage services provide storage in the cloud, which includes Blob services for storing text and binary data, Table services for structured storage that can be queried, and Queue services for reliable and persistent messaging between services.

In this hands-on lab, you will explore the basic elements of an Azure Cloud Service by creating a simple GuestBook application that demonstrates many features of web and worker roles, blob storage, table storage, and queues.

![Creating a new Windows Azure Cloud Service project](Images/0020storage01.jpg?raw=true "Creating a new Windows Azure Cloud Service project")

_Windows Azure Storage_

The images in our application will be stored as blobs.

![Creating a new Windows Azure Cloud Service project](Images/0030blobs.jpg?raw=true "Creating a new Windows Azure Cloud Service project")

_Blobs are collections that live in containers_

In the GuestBook application, a web role provides the front-end that allows users to view the contents of the guest book and submit new entries. Each entry contains a name, a message, and an associated picture. The application also contains a worker role that can generate thumbnails for the images that users submit.

The logic looks like this:

![architecture](images/0040architecture.jpg?raw=true)

When users post a new item, the web role uploads the picture to blob storage and creates an entry in table storage that contains the information entered by the user and a link to the blob with the picture. The web role renders this information to the browser so users can view the contents of the guest book.

After storing the image and creating the entry, the web role posts a work item to a queue to have the image processed. The worker role fetches the work item from the queue, retrieves the image from blob storage, and resizes it to create a thumbnail. Using queues to post work items is a common pattern in cloud applications and enables the separation of compute-bound tasks from the front-end. The advantage of this approach is that front and back ends can be scaled independently.

The finished solution will look something like this:
	
- Configuration Project
- Web Role
- Worker Role
- Shared Data Library (GuestBook_Data)

![0050SolutionOverview](images/0050solutionoverview.png?raw=true "Solution Overview")

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create applications in Azure using web roles and worker roles
- Use Storage services including blobs, queues and tables
- Publish an application to Windows Azure Cloud Services

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab and what I used to run through this lab:

- Windows 8.1
- Visual Studio 2013
- [Visual Studio 2013 Azure SDK and Tooling](http://go.microsoft.com/fwlink/p/?linkid=323510&clcid=0x409)

- IIS Setup on Windows 8.1
	- In the search box, enter "turn Windows features on or off," and then tap or click **Turn Windows features on or off** . 
	- In the list of Windows features, select **Internet Information Services**, and then tap or click **OK**.

	- In the list of Windows features, tap or click the plus sign (+) next to **Internet Information Services**, tap or click the plus sign (+) next to **World Wide Web Services**, tap or click the plus sign (+) next to **Application Development Features**, select the dynamic content features you want to install, and then tap or click **OK**.

<a name="UsingCodeSnippets"/>
### Using the Code Snippets ###

Code snippets are not needed. Everything you need is in this post. Just copy and paste as needed.

---
<a name="Exercises"/>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Building Your First Windows Azure Application](#Exercise1)

1. [Background Processing with Worker Roles and Queues](#Exercise2)

1. [Publishing a Windows Azure Application](#Exercise3)

Estimated time to complete this lab: **120** minutes.

>**Note:** Each exercise is accompanied by a starting solution located in the Begin folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and that they will not necessarily work until you complete the exercise. Inside the source code for an exercise, you will also find an End folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.

<a name="Exercise1"></a>
### Exercise 1: Building Your First Windows Azure Application ###

In this exercise, you create a guest book application and execute it in the local development fabric (emulators). For this purpose, you will use the Windows Azure Tools for Microsoft Visual Studio to create the project using the Cloud Service project template. These tools extend Visual Studio to enable the creation, building and running of Windows Azure services. You will continue to work with this project throughout the remainder of the lab.

<a name="Ex1Task1"></a>
#### Task 1 – Creating the Visual Studio Project ####

In this task, you create a new Cloud Service project in Visual Studio.

1. Open **Microsoft Visual Studio 2013** ***as administrator*** by right clicking (while holding down the **Shift** key if the shortcut is on the taskbar) the **Visual Studio 2013** shortcut and choosing **Run as administrator**.

1. If the **User Account Control** dialog appears, click **Yes**.

1. From the **Visual Studio** menu bar, select **File** | **New** | **Project...**.  

1. In the **New Project dialog**, expand **Visual C#** in the **Installed** list and select **Cloud**. Choose the **Windows Azure Cloud Service** template, set the Name of the project to **GuestBook**, the location to the "Begin" folder for this lab **\Source\Begin**, and ensure that **Create directory for solution** is checked. Click **OK** to create the project.

	![1010NewCloudServiceProject](images/1010newcloudserviceproject.png?raw=true "New GuestBook Cloud Project")

	_Creating a new Windows Azure Cloud Service project_

1. In the **New Windows Azure Cloud Service** dialog, inside the **Roles** panel, expand the **Visual C#** tab. Select **ASP.NET Web Role** from the list of available roles and click the arrow **(>)** to add an instance of this role to the solution. Before closing the dialog, select the new role in the right panel, **click the pencil icon and rename the role** as **GuestBook_WebRole**. Click **OK** to create the cloud service solution.

	![1020GuestBookWebRole](images/1020guestbookwebrole.png?raw=true)
	
	_Assigning roles to a Cloud Service project_

1. Select **Web Forms**, and click **OK**.

	![1030WebForms](images/1030webforms.png?raw=true "Web Forms")

	_Selecting the Web Forms Template_


1. In **Solution Explorer**, review the structure of the created solution.
 
	![1040NewSolution](images/1040newsolution.png?raw=true "New Solution")

	_Solution Explorer showing the GuestBook application_

	> **Note:** The generated solution contains two separate projects. The first project, named **GuestBook**, holds the configuration for the web and worker roles that compose the cloud application. It includes the service definition file, **ServiceDefinition.csdef**, which contains metadata needed by the Windows Azure fabric to understand the requirements of your application, such as which roles are used, their trust level, the endpoints exposed by each role, the local storage requirements and the certificates used by the roles. The service definition also establishes configuration settings specific to the application. The service configuration files (**ServiceConfiguration.Local.cscfg** and **ServiceConfiguration.Cloud.cscfg**) specify the number of instances to run for each role and sets the value of configuration settings defined in the service definition file. This separation between service definition and configuration allows you to update the settings of a running application by uploading a new service configuration file.

    > In Summary, two projects: (1) Configuration Project; (2) Actual Web Project.


	>You can create many configuration files, each one intended for a specific scenario such as production, development, or QA, and select which to use when publishing the application. By default, Visual Studio creates two files **ServiceConfiguration.Local.cscfg** and **ServiceConfiguration.Cloud.cscfg**.

	>The Roles node in the cloud service project enables you to configure which roles the service includes (web, worker or both) as well as which projects to associate with these roles. Adding and configuring roles through the Roles node will update the **ServiceDefinition.csdef** and **ServiceConfiguration.cscfg** files.

	>The second project, named **GuestBook_WebRole**, is a standard ASP.NET Web Application project template modified for the Windows Azure environment. It contains references to the Azure SDK libraries as well as an additional class that provides the entry point for the web role and contains methods to manage the initialization, starting, and stopping of the role.

<a name="Ex1Task2"></a>  
#### Task 2 – Creating a Data Model for Entities in Table Storage ####

The application stores guest book entries in Table storage. The Table service offers semi-structured storage in the form of tables that contain collections of entities. Entities have a primary key and a set of properties, where a property is a name, typed-value pair. 

In addition to the properties required by your model, every entity in Table Storage has three key properties: the **PartitionKey** and the **RowKey**. These properties together form the table's primary key and uniquely identify each entity in the table. Entities also have a **Timestamp** system property, which allows the service to keep track of when an entity was last modified. This field is intended for system use and should not be accessed by the application.
The Table Storage client API provides a **TableServiceEntity** class that defines the necessary properties. Although you can use the **TableServiceEntity** class as the base class for your entities, this is not required.

The Table service API is compliant with the REST API provided by [WCF Data Services](http://msdn.microsoft.com/en-us/library/cc668792.aspx) (formerly ADO.NET Data Services Framework) allowing you to use the [WCF Data Services Client Library](http://msdn.microsoft.com/en-us/library/cc668772.aspx) (formerly .NET Client Library) to work with data in Table Storage using .NET objects.

The Table service does not enforce any schema for tables making it possible for two entities in the same table to have different sets of properties. Nevertheless, the GuestBook application uses a fixed schema to store its data. 

In order to use the WCF Data Services Client Library to access data in table storage, you need to create a context class that derives from **TableServiceContext**, which itself derives from **DataServiceContext** in WCF Data Services. The Table Storage API allows applications to create the tables that they use from these context classes. For this to happen, the context class must expose each required table as a property of type **IQueryable\<SchemaClass\>**, where **SchemaClass** is the class that models the entities stored in the table. 

In this task, you model the schema of the entities stored by the GuestBook application and create a context class to use WCF Data Services to access the information in table storage. To complete the task, you create an object that can be data bound to data controls in ASP.NET and implements the basic data access operations: read, update, and delete.

1. Create a new project for the schema classes. To create the project, right-click the **GuestBook** solution, point to **Add** and then select **New Project**.

	![1050AddNewProject](images/1040addnewproject.png?raw=true "Add New Project")

	_Add New Project_

1. In the **Add New Project** dialog, expand **"Installed"** | **"Visual C#"** and select the **Windows Desktop** category, and then choose the **Class Library** project template. Set the name to ***GuestBook_Data***, leave the proposed location inside the solution folder unchanged, and then click **OK**.

	![1060GuestBook_Data](images/1060guestbookdata.png?raw=true "GuestBook_Data Project")

	_Creating a class library for GuestBook entities_
 
1. Delete the default class file generated by the class library template. To do this, **right-click** **Class1.cs** and choose **Delete**. Click **OK** in the confirmation dialog.

	![1070DeleteClass1](images/1070deleteclass1.png?raw=true "Delete Class1.cs")
	
	_Delete Class1.cs_

1. Next, we'll add some references to the **GuestBook_Data** project. In **Solution Explorer**, expand the **GuestBook_Data** project node, **right-click** the **References** node, and select **Add Reference...**

	![1080AddReference](images/1080addreference.png?raw=true "Add Reference")

	_Add Reference_

1. In the **Reference Manager - GuestBook_Data** window, click the **Extensions** tab, select the **Microsoft.Data.Services.Client** assembly.  You will likely see multiple entries for the **"Microsoft.Data.Services.Client" assembly, hover over each to find the one that is part of the Azure SDK, turn on the checkbox next to that one, and click **"OK"**: 

	![1090DataServicesClient](images/1090dataservicesclient.png?raw=true "Data Services Client Reference")

	_Adding a reference to the System.Data.Service.Client component_

1. Again, right click the **GuestBook_Data** | **References**, node, and select **Add Reference...".  This time, in the **Reference Manager - GuestBook_Data** window, switch to the **Extensions** page, select the following assemblies, and click **OK**:

	- **Microsoft.WindowsAzure.Storage**
	- **Microsoft.WindowsAzure.Configuration**

	![1100AzureReferences](images/1100azurereferences.png?raw=true "Azure Library References")

	_Adding a reference to the Microsoft.WindowsAzure.Storage and Microsoft.WindowsAzure.Configuration component_

	> **Note:** You may see multiple versions of the libraries if more than one version of the Azure SDK has been installed over time.  You can hover over an assembly to verify the path for the assembly points to the latest version.  

1. Before you can store an entity in a table, you must first define its schema. To do this, right-click **GuestBook_Data** in **Solution Explorer**, point to **Add** and select **Class**. In the **Add New Item** dialog, set the name to **GuestBookEntry.cs** and click **Add**.

	![1110GuestBookEntryClass](images/1110guestbookentryclass.png?raw=true "GuestBookEntry Class")

	_Adding the GuestBookEntry class_

1. If not already opened, open the **GuestBookEntry.cs** file and then update the declaration of the **GuestBookEntry** class to make it public and derive from the **TableServiceEntity** class.

	<!-- mark:2 -->
	````C#
	public class GuestBookEntry
		: Microsoft.WindowsAzure.Storage.Table.TableEntity
	{
	}
	````

	>**Note:** **TableServiceEntity** is a class found in the Storage Client API. This class defines the **PartititionKey**, **RowKey** and **TimeStamp** system properties required by every entity stored in a Windows Azure table. 

	>Together, the **PartitionKey** and **RowKey** define the **DataServiceKey** that uniquely identifies every entity within a table.

1. Add a default constructor to the **GuestBookEntry** class that initializes its **PartitionKey** and **RowKey** properties.

	<!-- mark:1-7 -->
	````C#
	public GuestBookEntry()
	{            
  	  PartitionKey = DateTime.UtcNow.ToString("MMddyyyy");

	  // Row key allows sorting, so we make sure the rows come back in time order
  	  RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
	}
	````

	>**Note:** To partition the data, the GuestBook application uses the date of the entry as the **PartitionKey**, which means that there will be a separate partition for each day of guest book entries. In general, you choose the value of the partition key to ensure load balancing of the data across storage nodes.

	>The **RowKey** is a reverse DateTime field with a GUID appended for uniqueness. Tables within partitions are sorted in RowKey order, so this will sort the tables into the correct order to be shown on the home page, with the newest entry shown at the top.

1. To complete the definition of the **GuestBookEntry** class, add properties for **Message**, **GuestName**, **PhotoUrl**, and **ThumbnailUrl** to hold information about the entry.

	<!-- mark:1-7 -->
	```` C#
	public string Message { get; set; }

	public string GuestName { get; set; }

	public string PhotoUrl { get; set; }

	public string ThumbnailUrl { get; set; }
	````

1. Save the **GuestBookEntry.cs** file.

1. Here is the Entire Code Snippet for _GuestBookEntrty.cs_

````C#
using System;

namespace GuestBook_Data
{
  public class GuestBookEntry
      : Microsoft.WindowsAzure.Storage.Table.TableEntity
  {
    public GuestBookEntry()
    {
      PartitionKey = DateTime.UtcNow.ToString("MMddyyyy");

      // Row key allows sorting, so we make sure the rows come back in time order
      RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
    }

    public string Message { get; set; }

    public string GuestName { get; set; }

    public string PhotoUrl { get; set; }

    public string ThumbnailUrl { get; set; }
  }

}
````

1. Next, you need to create the context class required to access the _GuestBook_ table using WCF Data Services. To do this, in **Solution Explorer**, right-click the **GuestBook_Data** project, point to **Add** and select **Class**. In the **Add New Item** dialog, set the name to **GuestBookDataContext.cs** and click **Add**.

1. In the new class file, update the declaration of the new class to make it public and inherit the **TableServiceContext** class.

	<!-- mark:2 -->
	````C#
	public class GuestBookDataContext
		: Microsoft.WindowsAzure.Storage.Table.DataServices.TableServiceContext
	{
	}
	````

1. Now, add a default constructor to initialize the base class with storage account information.

	<!-- mark:4-7 -->
	````C#
	public class GuestBookDataContext 
	  :  Microsoft.WindowsAzure.Storage.Table.DataServices.TableServiceContext
	{
		public GuestBookDataContext(Microsoft.WindowsAzure.Storage.Table.CloudTableClient client) : base(client)
		{
		}        
	}
	````

	>**Note:** You can find the **TableServiceContext** class in the storage client API. This class derives from **DataServiceContext** in WCF Data Services and manages the credentials required to access your storage account as well as providing support for a retry policy for its operations.

1. Add a property to the **GuestBookDataContext** class to expose the **GuestBookEntry** table. To do this, insert the following (highlighted) code into the class. 

	<!-- mark:6-12 -->
	````C#
	public class GuestBookDataContext 
	  :  Microsoft.WindowsAzure.Storage.Table.DataServices.TableServiceContext
	{
		...

		public IQueryable<GuestBookEntry> GuestBookEntry
		{
			get
			{
				return this.CreateQuery<GuestBookEntry>("GuestBookEntry");
			}
		}
	}
	````
	
	>**Note:** You can use the **CreateTablesFromModel** method in the **CloudTableClient** class to create the tables needed by the application. When you supply a **DataServiceContext** (or **TableServiceContext**) derived class to this method, it locates any properties that return an **IQueryable\<T\>**, where the generic parameter **T** identifies the class that models the table schema, and creates a table in storage named after the property.

1. The Entire Code Snippet for _GuestBookDataContext.cs_

````C#
using System.Linq;

namespace GuestBook_Data
{
  public class GuestBookDataContext
    : Microsoft.WindowsAzure.Storage.Table.DataServices.TableServiceContext
  {

    public GuestBookDataContext(Microsoft.WindowsAzure.Storage.Table.CloudTableClient client)
      : base(client)
    {
    }

    public IQueryable<GuestBookEntry> GuestBookEntry
    {
      get
      {
        return this.CreateQuery<GuestBookEntry>("GuestBookEntry");
      }
    }

  }
}
````

1. Finally, you need to implement an object that can be bound to data controls in ASP.NET.  In **Solution Explorer**, right-click **GuestBook_Data**, point to **Add**, and select **Class**. In the **Add New Item** dialog, set the name to **GuestBookDataSource.cs** and click **Add**.

1. In the new class file, add the following namespace declarations to import the types contained in the **Microsoft.WindowsAzure**, **Microsoft.WindowsAzure.Storage** and **Microsoft.WindowsAzure.Storage.Table** namespaces.

	````C#
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.Storage;
	using Microsoft.WindowsAzure.Storage.Table;
	````

1. In the **GuestBookDataSource** class, make the class **public** and define member fields for the data context and the storage account information, as shown below.

	<!-- mark:3-4 -->
	````C#
	public class GuestBookDataSource
	{
		private static CloudStorageAccount storageAccount;
		private GuestBookDataContext context;
	}
	````

1. Now, add a static constructor to the data source class as shown in the following (highlighted) code. This code creates the tables from the **GuestBookDataContext** class.

	<!-- mark:5-12 -->
	````C#
	public class GuestBookDataSource
	{
		...

		static GuestBookDataSource()
		{
			storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

			CloudTableClient cloudTableClient = storageAccount.CreateCloudTableClient();
			CloudTable table = cloudTableClient.GetTableReference("GuestBookEntry");
			table.CreateIfNotExists();
		}
	}
	````
		
	>**Note:**  The static  constructor initializes the storage account by reading its settings from the configuration and then uses the **CreateTablesFromModel** method in the **CloudTableClient** class to create the tables used by the application from the model defined by the **GuestBookDataContext** class. By using the static constructor, you ensure that this initialization task is executed only once.

	>The **"DataConnectionString"** setting will need to be created in the *.cscfg configuration files for the web roles and worker roles.  In that setting we'll need to provide a valid storage services connection string that either points to our local development storage emulator, or to a real Azure Storage account in the cloud.  We'll setup the **"DataConnectionString"** setting in a later step.

1. Add a default constructor to the **GuestBookDataSource** class to initialize the data context class used to access table storage.

	<!-- mark:5-8 -->
	```` C#
	public class GuestBookDataSource
	{
		...

		public GuestBookDataSource()
		{
			this.context = new GuestBookDataContext(storageAccount.CreateCloudTableClient());
		}
	}
	````

1. Next, insert the following method to return the contents of the **GuestBookEntry** table. 

	<!-- mark:5-13 -->
	````C#
	public class GuestBookDataSource
	{
		...

		public IEnumerable<GuestBookEntry> GetGuestBookEntries()
		{
			CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
			CloudTable table = tableClient.GetTableReference("GuestBookEntry");

			TableQuery<GuestBookEntry> query = new TableQuery<GuestBookEntry>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, DateTime.UtcNow.ToString("MMddyyyy")));

			return table.ExecuteQuery(query);
		}
	}
	````

	>**Note:** The **GetGuestBookEntries** method retrieves today's guest book entries by creating a TableQuery<t> operation that filters the retrieved information using the current date as the partition key value. The web role uses this method to bind to a data grid and display the guest book. 

1. Now, add the following method to insert new entries into the **GuestBookEntry** table.

	<!-- mark:5-10 -->
	````C#
	public class GuestBookDataSource
	{
		...

		public void AddGuestBookEntry(GuestBookEntry newItem)
		{
			TableOperation operation = TableOperation.Insert(newItem);
			CloudTable table = context.ServiceClient.GetTableReference("GuestBookEntry");
			table.Execute(operation);
		}
	}
	````

	>**Note:** This method creates a **TableOperation** to insert the new guest book to storage.

1. Finally, add a method to the data source class to update the **Thumbnail URL** property for an entry.

	(Code Snippet – _Introduction to Cloud Services - Ex1 GuestBookDataSource UpdateImageThumbnail_ – CS)

	<!-- mark:5-20 -->
	````C#
	public class GuestBookDataSource
	{
		...

		public void UpdateImageThumbnail(string partitionKey, string rowKey, string thumbUrl)
		{
			CloudTable table = context.ServiceClient.GetTableReference("GuestBookEntry");
			TableOperation retrieveOperation = TableOperation.Retrieve<GuestBookEntry>(partitionKey, rowKey);

			TableResult retrievedResult = table.Execute(retrieveOperation);
			GuestBookEntry updateEntity = (GuestBookEntry)retrievedResult.Result;

			if (updateEntity != null)
			{
				updateEntity.ThumbnailUrl = thumbUrl;

				TableOperation replaceOperation = TableOperation.Replace(updateEntity);
				table.Execute(replaceOperation);
			}
		} 
	}
	````

	>**Note:** The **UpdateImageThumbnail** method creates a table operation to retrieve an entry using its partition key and row key; it updates the thumbnail URL, creates another table operation to replace the existing guest book with the new thumbnail URL.

1. Save the **GuestBookDataSource.cs** file.

<a name="anchor-name-here" />
(The Entire Code Snippet - _GuestBookDataSource.cs_)


````C#
using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace GuestBook_Data
{
  public class GuestBookDataSource
  {
    private static CloudStorageAccount storageAccount;
    private GuestBookDataContext context;

    static GuestBookDataSource()
    {
      storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

      CloudTableClient cloudTableClient = storageAccount.CreateCloudTableClient();
      CloudTable table = cloudTableClient.GetTableReference("GuestBookEntry");
      table.CreateIfNotExists();
    }

    public GuestBookDataSource()
    {
      this.context = new GuestBookDataContext(storageAccount.CreateCloudTableClient());
    }

    public IEnumerable<GuestBookEntry> GetGuestBookEntries()
    {
      CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
      CloudTable table = tableClient.GetTableReference("GuestBookEntry");

      TableQuery<GuestBookEntry> query = new TableQuery<GuestBookEntry>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, DateTime.UtcNow.ToString("MMddyyyy")));

      return table.ExecuteQuery(query);
    }

    public void AddGuestBookEntry(GuestBookEntry newItem)
    {
      TableOperation operation = TableOperation.Insert(newItem);
      CloudTable table = context.ServiceClient.GetTableReference("GuestBookEntry");
      table.Execute(operation);
    }

    public void UpdateImageThumbnail(string partitionKey, string rowKey, string thumbUrl)
    {
      CloudTable table = context.ServiceClient.GetTableReference("GuestBookEntry");
      TableOperation retrieveOperation = TableOperation.Retrieve<GuestBookEntry>(partitionKey, rowKey);

      TableResult retrievedResult = table.Execute(retrieveOperation);
      GuestBookEntry updateEntity = (GuestBookEntry)retrievedResult.Result;

      if (updateEntity != null)
      {
        updateEntity.ThumbnailUrl = thumbUrl;

        TableOperation replaceOperation = TableOperation.Replace(updateEntity);
        table.Execute(replaceOperation);
      }
    } 

  }
}
````

<a name="Ex1Task3"></a>
#### Task 3 – Creating a Web Role to Display the Guest Book and Process User Input ####

In this task, you update the web role project that you generated in Task 1, when you created the Windows Azure Cloud Service solution. This involves updating the UI to render the list of guest book entries. For this purpose, you will find a page that has the necessary elements in the **Assets** folder of this exercise, which you will add to the project. Next, you implement the code necessary to store submitted entries in table storage and images in blob storage. To complete this task, you configure the storage account used by the Web role.

1. Add a reference in the web role to the **GuestBook** project. In **Solution Explorer**, right-click the **GuestBook_WebRole** project node and select **Add Reference**, switch to the **Solution** tab, select the **GuestBook_Data** project, and then click **OK**.

	![1120GuestBookDataReference](images/1120guestbookdatareference.png?raw=true "GuestBook_Data Reference")

	_Add a reference to the GuestBook_Data Project_

1. The web role template generates a default page. You will replace it with another page that contains the UI of the guest book application. To delete the page, in **Solution Explorer**, **right-click** **Default.aspx** in the **GuestBook_WebRole** project and select **Delete**. Click **OK** in the confirmation dialog.

	![1130DeleteDefaultAspx](images/1130deletedefaultaspx.png?raw=true "Delete Default.aspx")

	_Delete the original Default.aspx page_

1. Add the main page and its associated assets to the web role. To do this, right-click **GuestBook_WebRole** in **Solution Explorer**, point to **Add** and select **Existing Item**.

	![1140AddExistingItems](images/1140addexistingitems.png?raw=true "Add Existing Items")

	_Add Existing Items_

1. In the **Add Existing Item** dialog, browse to the **Assets** folder in **\Source\Assets**, ***select every file in this folder*** and click **Add**.

	![1150AddAssets](images/1150addassets.png?raw=true "Add Assets Folder Contents")
	
	_Add the "Assets" folder contents_

	>**Note:** The **Assets** folder contains five files that you need to add to the project, a Default.aspx file with its code-behind and designer files, a CSS file, and an image file.

1. From the **GuestBook_WebnRole"** project, open the **Default.aspx.cs"** code-behind file for the main page. To do this, right-click the **Default.aspx** file in **Solution Explorer** and select **View Code**.

1. In the code-behind file, insert the following namespace declarations.

	<!-- mark:1-8 -->
	```` C#
	using System.IO;
	using System.Net;
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.ServiceRuntime;
	using Microsoft.WindowsAzure.Storage;
	using Microsoft.WindowsAzure.Storage.Blob;
	using Microsoft.WindowsAzure.Storage.Queue;
	using GuestBook_Data;
	````

1. Declare the following member fields in the **_Default** class.

	<!-- mark:3-5 -->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
		private static bool storageInitialized = false;
		private static object gate = new object();
		private static CloudBlobClient blobStorage;

		...
	}
	````

1. Locate the **SignButton_Click** event handler in the code-behind file and insert the following code.

	<!-- mark:7-28 -->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
		...

		protected void SignButton_Click(object sender, EventArgs e)
		{
			if (this.FileUpload1.HasFile)
			{
				this.InitializeStorage();

				// upload the image to blob storage
				string uniqueBlobName = string.Format("guestbookpics/image_{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(this.FileUpload1.FileName));
				CloudBlockBlob blob = blobStorage.GetContainerReference("guestbookpics").GetBlockBlobReference(uniqueBlobName);
				blob.Properties.ContentType = this.FileUpload1.PostedFile.ContentType;
				blob.UploadFromStream(this.FileUpload1.FileContent);
				System.Diagnostics.Trace.TraceInformation("Uploaded image '{0}' to blob storage as '{1}'", this.FileUpload1.FileName, uniqueBlobName);

				// create a new entry in table storage
				GuestBookEntry entry = new GuestBookEntry() { GuestName = this.NameTextBox.Text, Message = this.MessageTextBox.Text, PhotoUrl = blob.Uri.ToString(), ThumbnailUrl = blob.Uri.ToString() };
				GuestBookDataSource ds = new GuestBookDataSource();
				ds.AddGuestBookEntry(entry);
				System.Diagnostics.Trace.TraceInformation("Added entry {0}-{1} in table storage for guest '{2}'", entry.PartitionKey, entry.RowKey, entry.GuestName);
			}

			this.NameTextBox.Text = string.Empty;
			this.MessageTextBox.Text = string.Empty;

			this.DataList1.DataBind();
		}
	}
  
	````

	>**Note:** To process a new guest book entry after the user submits the page, the handler first calls the **InitializeStorage** method to ensure that the blob container used to store images exists and allows public access. You will implement this method shortly. 

	>It then obtains a reference to the blob container, generates a unique name and creates a new blob, and then uploads the image submitted by the user into this blob. Notice that the method initializes the **ContentType** property of the blob from the content type of the file submitted by the user. When the guest book page reads the blob back from storage, the response returns this content type, allowing a page to display the image contained in the blob simply by referring to its URL.

	>After that, it creates a new **GuestBookEntry** entity, which is the entity you defined in the previous task, initializes it with the information submitted by the user, and then uses the **GuestBookDataSource** class to save the entry to table storage using the .NET Client Library for WCF Data Services.
	
	>Finally, it data binds the guest book entries list to refresh its contents.

1. Update the body of the **Timer1_Tick** method with the code shown below. 

	<!-- mark:7 -->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
		...

		protected void Timer1_Tick(object sender, EventArgs e)
		{
			this.DataList1.DataBind();
		}

		...
	}
	````

	>**Note:** The timer periodically forces the page to refresh the contents of the guest book entries list.

1. Locate the **Page_Load** event handler and update its body with the following code to enable the page refresh timer.
	
	<!-- mark:7-10 -->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
		...

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				this.Timer1.Enabled = true;
			}
		}

		...
	}
	````

1. Implement the **InitializeStorage** method by replacing its body with the following (highlighted) code.

	````C#
	public partial class _Default : Page
	{

		...

		 private void InitializeStorage()
		 {
			if (storageInitialized)
			{
			  return;
			}

			lock (gate)
			{
			  if (storageInitialized)
			  {
				 return;
			  }

			  try
			  {
				 // read account configuration settings
				 var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

				 // create blob container for images
				 blobStorage = storageAccount.CreateCloudBlobClient();
				 CloudBlobContainer container = blobStorage.GetContainerReference("guestbookpics");
				 container.CreateIfNotExists();

				 // configure container for public access
				 var permissions = container.GetPermissions();
				 permissions.PublicAccess = BlobContainerPublicAccessType.Container;
				 container.SetPermissions(permissions);
			  }
			  catch (WebException)
			  {
				 throw new WebException("Storage services initialization failure. "
						 + "Check your storage account configuration settings. If running locally, "
						 + "ensure that the Development Storage service is running.");
			  }

			  storageInitialized = true;
			}
		 }

		...

	}
	````

	>**Note:** The **InitializeStorage** method first ensures that it executes only once. It reads the storage account settings from the Web role configuration, creates a blob container for the images uploaded with each guest book entry and configures it for public access.

1. Here is the entire code for the Default.aspx.cs file:

	````C#
	using System;
	using System.IO;
	using System.Net;
	using System.Web.UI;
	using GuestBook_Data;
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.Storage;
	using Microsoft.WindowsAzure.Storage.Blob;
	using Microsoft.WindowsAzure.Storage.Queue;


	namespace GuestBook_WebRole
	{
	  public partial class _Default : System.Web.UI.Page
	  {
		 private static bool storageInitialized = false;
		 private static object gate = new object();
		 private static CloudBlobClient blobStorage;


		 protected void Page_Load(object sender, EventArgs e)
		 {
			if (!Page.IsPostBack)
			{
			  this.Timer1.Enabled = true;
			}
		 }

		 protected void SignButton_Click(object sender, EventArgs e)
		 {
			if (this.FileUpload1.HasFile)
			{
			  this.InitializeStorage();

			  // upload the image to blob storage
			  string uniqueBlobName = string.Format("guestbookpics/image_{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(this.FileUpload1.FileName));
			  CloudBlockBlob blob = blobStorage.GetContainerReference("guestbookpics").GetBlockBlobReference(uniqueBlobName);
			  blob.Properties.ContentType = this.FileUpload1.PostedFile.ContentType;
			  blob.UploadFromStream(this.FileUpload1.FileContent);
			  System.Diagnostics.Trace.TraceInformation("Uploaded image '{0}' to blob storage as '{1}'", this.FileUpload1.FileName, uniqueBlobName);

			  // create a new entry in table storage
			  GuestBookEntry entry = new GuestBookEntry() { GuestName = this.NameTextBox.Text, Message = this.MessageTextBox.Text, PhotoUrl = blob.Uri.ToString(), ThumbnailUrl = blob.Uri.ToString() };
			  GuestBookDataSource ds = new GuestBookDataSource();
			  ds.AddGuestBookEntry(entry);
			  System.Diagnostics.Trace.TraceInformation("Added entry {0}-{1} in table storage for guest '{2}'", entry.PartitionKey, entry.RowKey, entry.GuestName);
			}

			this.NameTextBox.Text = string.Empty;
			this.MessageTextBox.Text = string.Empty;

			this.DataList1.DataBind();
		 }

		 protected void Timer1_Tick(object sender, EventArgs e)
		 {
			this.DataList1.DataBind();
		 }

		 private void InitializeStorage()
		 {
			if (storageInitialized)
			{
			  return;
			}

			lock (gate)
			{
			  if (storageInitialized)
			  {
				 return;
			  }

			  try
			  {
				 // read account configuration settings
				 var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

				 // create blob container for images
				 blobStorage = storageAccount.CreateCloudBlobClient();
				 CloudBlobContainer container = blobStorage.GetContainerReference("guestbookpics");
				 container.CreateIfNotExists();

				 // configure container for public access
				 var permissions = container.GetPermissions();
				 permissions.PublicAccess = BlobContainerPublicAccessType.Container;
				 container.SetPermissions(permissions);
			  }
			  catch (WebException)
			  {
				 throw new WebException("Storage services initialization failure. "
						 + "Check your storage account configuration settings. If running locally, "
						 + "ensure that the Development Storage service is running.");
			  }

			  storageInitialized = true;
			}
		 }
	  }
	}
	````


1. The Azure Storage libraries need to be pointed to the storage services they will use.  To do that', we'll provide the storage libraries with a "connection string" that it can parse to determine where the REST endpoints for the various storage services are, and to give it the account key needed to access those endpoints. In our code above we told the storage libraries to look for a **"DataConnectionString"" setting in the Cloud Service Configuration (*.cscfg) files.  Now, we need to create that setting and give it a value.  

1. To create a new setting, in **Solution Explorer**, expand the **Roles** node in the **GuestBook** cloud project, double-click **GuestBook_WebRole** to open the properties for this role and select the **Settings** tab. Click **Add Setting**, type _"DataConnectionString"_ in the **Name** column, change the **Type** to _Connection String_, and then click the button labeled with an ellipsis ("...")

	![1160NewSetting](images/1160newsetting.png?raw=true "NewSetting")

	_Configuring the storage account settings_

1. If prompted, sign in to your Microsoft Account:

	![1170MicrosoftAccountSignIn](images/1170microsoftaccountsignin.png?raw=true "Microsoft Account Sign In")

	_Microsoft Account Sign In_

	> **Note:** This isn't actually required right now, because we will just be pointing at the Development Storage emulator for the time being, but later when we want to point to a real Azure Storage account, signing in to your Microsoft Account helps Visual Studio show you the storage accounts you have in the cloud for easier configuration.  

1. In the **Create Storage Connection String** dialog, choose the option labeled **Windows Azure storage emulator** and then click **OK**. 

	![1180UseDevelopmentStorage](images/1180usedevelopmentstorage.png?raw=true "Use the Windows Azure storage emulator")

	_Creating a connection string for the storage emulator_ 

	>**Note:** A storage account is a unique endpoint for the Windows Azure Blob, Queue, and Table services. You must create a storage account in the Management Portal to use these services. In this exercise, you use Storage emulator, which is included in the Windows Azure SDK development environment to simulate the Blob, Queue, and Table services available in the cloud. If you are building a cloud service that employs storage services or writing any external application that calls storage services, you can test locally against the Storage emulator.

	>To use the storage emulator, you set the value of the **UseDevelopmentStorage** keyword in the connection string for the storage account to true. When you publish your application to Windows Azure, you need to update the connection string to specify storage account settings including your account name and shared key. For example,
	
	>\<Setting name="DataConnectionString" value="DefaultEndpointsProtocol=https;AccountName=YourAccountName;AccountKey=YourAccountKey" /\>
	
	>where _YourAccountName_ is the name of your Storage account and YourAccountKey is your access key.

1. Press **CTRL + S** to save changes to the role configuration.

<a name="Ex1Task4"></a>
#### Task 4 – Queuing Work Items for Background Processing ####

In preparation for the next exercise, you now update the front-end web role to dispatch work items to an Azure queue for background processing. These work items will remain in the queue until you add a worker role that picks items from the queue and generates thumbnails for each uploaded image.

1. Open the Default.aspx.cs code-behind file from the web role project. To do this, right-click the **Default.aspx** file in **Solution Explorer** and select **View Code**.

1. Declare a queue client member by inserting the following (highlighted) declaration into the **Default** class.

	<!-- mark:6 -->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
		private static bool storageInitialized = false;
		private static object gate = new object();
		private static CloudBlobClient blobStorage;
		private static CloudQueueClient queueStorage;

		...
	}
	````

1. Now, update the storage initialization code to create the queue, if it does not exist, and then initialize the queue reference created in the previous step. To do this, locate the **InitializeStorage** method and insert the following (highlighted) code into this method immediately after the code that configures the blob container for public access.

	<!-- mark:13-16 -->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
		...

		private void InitializeStorage()
		{
			...

			try
			{
				...

				// create queue to communicate with worker role
				queueStorage = storageAccount.CreateCloudQueueClient();
				CloudQueue queue = queueStorage.GetQueueReference("guestthumbs");
				queue.CreateIfNotExists();
			}
			catch (WebException)
			{
				...
			}

			...
		}
	}
	````

	>**Note:** The updated code creates a queue that the web role uses to submit new jobs to the worker role.

1. Finally, add code to post a work item to the queue. To do this, locate the **SignButton_Click** event handler and insert the following (highlighted) code immediately after the lines that create a new entry in table storage.

	<!-- mark:7-11 -->
	````C#
	protected void SignButton_Click(object sender, EventArgs e)
	{
		if (this.FileUpload1.HasFile)
		{
			...

			// queue a message to process the image
			var queue = queueStorage.GetQueueReference("guestthumbs");
			var message = new CloudQueueMessage(string.Format("{0},{1},{2}", uniqueBlobName, entry.PartitionKey, entry.RowKey));
			queue.AddMessage(message);
			System.Diagnostics.Trace.TraceInformation("Queued message to process blob '{0}'", uniqueBlobName);
		}

		this.NameTextBox.Text = string.Empty;
		this.MessageTextBox.Text = string.Empty;

		this.DataList1.DataBind();
	}
	````

	>**Note:** The updated code obtains a reference to the _“guestthumbs”_ queue. It constructs a new message that consists of a comma-separated string with the name of the blob that contains the image, the partition key, and the row key of the entity that was added. The worker role can easily parse messages with this format. The method then submits the message to the queue.

<a name="Ex1Verification"></a>
#### Verification ####


The Windows Azure compute emulator, formerly Development Fabric or devfabric, is a simulated environment for developing and testing Windows Azure applications in your machine. In this task, you launch the GuestBook application in the emulator and create one or more guest book entries.

Among the features available in the Windows Azure Tools for Microsoft Visual Studio is a Storage browser that allows you to connect to a storage account and browse the blobs and tables it contains. If you are using this version of Visual Studio, you will use it during this task to examine the storage resources created by the application.

Optionally, you can use the Azure Management Studio product from Cerebrata, which includes not only a storage explorer, but also diagnostics management, deployment tools and more.  There is a 30 day free trial and you can install the application from the [Cerebrata website](http://www.cerebrata.com/products/azure-management-studio/introduction). 

1. In **Visual Studio**, Press **F5** to execute the service. The service builds and then launches the local Windows Azure compute and storage emulators. To show the Compute Emulator UI, right-click its icon located in the system tray and select **Show Compute Emulator UI**.

	![1190ComputeEmulator](images/1190computeemulator.png?raw=true "Compute Emulator")

	_Showing the Compute Emulator UI_

	>**Note:** If it is the first time you run the **Windows Azure Emulator**, the System will show a **Windows Security Alert** dialog indicating the Firewall has blocked some features. Click **Allow Access** to continue.

	>![Unblocking the Firewall](Images/1200warning-unblocking-firewall.png?raw=true "Unblocking the Firewall")

	>**Note:** When you use the storage emulator for the first time, it needs to execute a one-time initialization procedure to create the necessary database and tables. If this is the case, wait for the procedure to complete and examine the **Development Storage Initialization** dialog to ensure that it completes successfully.

	>![Storage emulator initialization process](Images/1210initialization-process.png?raw=true "Storage emulator initialization process")

	_Storage emulator initialization process_

1. Switch to **Internet Explorer** to view the **GuestBook** application.

1. Add a new entry to the guest book. To do this, type your name and a message, choose an image to upload from the your computer (You may find some samples in the  **Pictures\Sample Pictures** library, or download some if you don't have any handy), and then click the pencil icon to submit the entry.

	![1220NewGuestBookEntry](images/1220newguestbookentry.png?raw=true "New Guest Book Entry")

	_Windows Azure GuestBook home page_

	>**Note:** It is a good idea to choose a large hi-resolution image because, once the application is complete, the guestbook service will resize uploaded images.

1. Once you submit an entry, the web role creates a new entity in the guest book table and uploads the photo to blob storage. The page contains a timer that triggers a page refresh every 5 seconds, so the new entry should appear on the page after a brief interval. 
	Initially, the new entry contains a link to the blob that contains the uploaded image so it will appear with the same size as the original image. 

	![1230LargeImageInEntry](images/1230largeimageinentry.png?raw=true "Large image in entry")

	_GuestBook application showing an uploaded image in its original size_

1. To verify that the data has been saved to storage, return to Visual Studio, and open the **"Server Explorer"** Window (from the **Visual Studio** menu, select **"View"** | **"Server Explorer"** or use the **Ctrl+W, L** keyboard combination) 

1. Expand the **"Windows Azure"** | **"Storage"** | **"(Development)"** node. To ensure that you are seeing the latest info, **right-click** on the **"(Development)"** node, and select **"Refresh"**, then click on each of the **"Blobs"**, **"Queues"**, and **"Tables"** nodes to expand them and view their contents.  

	![1240DevelopmentStorageInServerExplorer](images/1240developmentstorageinserverexplorer.png?raw=true "Development Storage in Server Explorer")

	_Development Storage in Server Explorer_

1. Next double click on the **"Blobs"** | **"guestbookpics"** container to view it's contents, then double click on the first entry in the container window to view the image you uploaded:

	![1250BlobImage](images/1250blobimage.png?raw=true "Blob Image")

	_Image in Blob Storage_

1. Next, try double clicking on the **"Queues"** | **"guestthumbs"** queue, then double clicking on the first entry to view the message (the path to the blob that needs to have a thumbnail created):

	![1260QueueMessage](images/1260queuemessage.png?raw=true "QueueMessage")

	_Queue Message_

1. Finally, double click on the **"Tables"** | **"GuestBookEntry"** table to view it's contents, and double click on the first entity to view it:

	![1270GuestBookEntryEntity](images/1270guestbookentryentity.png?raw=true "GuestBookEntry Entity")

1. Switch to Visual Studio and Press **SHIFT + F5** to stop the debugger and shut down the deployment in the development fabric.

---

<a name="Exercise2"></a>
### Exercise 2: Background Processing with Worker Roles and Queues ###

A worker role runs in the background to provide services or execute time related tasks like a service process. 

In this exercise, you create a worker role to read work items posted to a queue by the web role front-end. To process the work item, the worker role extracts information about a guest book entry from the message and then retrieves the corresponding entity from table storage. It then fetches the associated image from blob storage and creates its thumbnail, which it also stores as a blob. Finally, to complete the processing, it updates the URL of the generated thumbnail blob in the guest book entry.

![architecture](images/0040architecture.jpg?raw=true)

- **Workflow**
	- The **web role** takes in photos
		- It puts an entry into the table
		- It puts a message into the queue
		- It records the whole image as a blob
    - The **worker role** is cnstantly scanning the queue to see if there is work to do
	 - If it sees an entry, it will take the location of the blob (the photo) and make a thumbnail out of it
		- It will then update the table to reflect the cration of the thumbnail photo

<a name="Ex2Task1"></a>
#### Task 1 – Creating a Worker Role to Process Images in the Background ####

In this task, you add a worker role project to the solution and update it so that it reads items posted by the front-end from the queue and processes them.

1. If not already open, launch **Microsoft Visual Studio 2013** as administrator. 

1. You can continue working with the project you built in Exercise 1, or if needed you can open a working version of the project from the **Source\End\GuestBook-AfterExericse1" folder.  

1. In **Solution Explorer**, right-click the Roles node in the **GuestBook** project and then select **Add** | **New Worker Role Project**.

	![2020AddWorkerRole](images/2020addworkerrole.png?raw=true "Add Worker Role")

	_Add a worker role_

1. In the **Add New Role Project** dialog, select the **Worker Role** category and choose the **Worker Role** template. Set the name of the worker role to **GuestBook_WorkerRole** and click **Add**.

	![2030GuestBookWorkerRole](images/2030guestbookworkerrole.png?raw=true "GuestBook_WorkerRole Project")

	_Adding a worker role project to the solution_

1. In the new worker role project, add a reference to the data model project. In **Solution Explorer**, right-click the **GuestBook_WorkerRole** project and select **Add Reference**, switch to the **Solution** tab, select **GuestBook_Data** and then click **OK**.

	![2040GuestBookDataReference](images/2040guestbookdatareference.png?raw=true "Add a reference to GuestBook_Data")

	_GuestBook_Data Reference_

	> **Note**  Now both the **web role** and the **worker role** have access to **GuestBook_Data**.

1. Next, add a reference to the **System.Drawing** assembly, only this time, in the Add Reference dialog, switch to the **Assemblies** tab instead, select the **System.Drawing** component and then click **OK**.

	![2050SystemDrawingReference](images/2050systemdrawingreference.png?raw=true "System.Drawing Reference")

1. Now, open the **WorkerRole.cs** file of the **GuestBook_WorkerRole** project and insert the followings namespace declarations.

	````C#
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.Drawing.Imaging;
	using System.IO;
	using GuestBook_Data;
	using Microsoft.WindowsAzure.Storage.Queue;
	using Microsoft.WindowsAzure.Storage.Blob;
	````

1. Add member fields to the **WorkerRole** class for the blob container and the queue, as shown below.

	<!-- mark:3-4 -->
	````C#
	public class WorkerRole : RoleEntryPoint
	{
		private CloudQueue queue;
		private CloudBlobContainer container;

		...
	}
	````

1. Insert the following code into the body of the **OnStart** method immediately after the line that subscribes the **RoleEnvironmentChanging** event and before the call to the **OnStart** method in the base class.

	<!-- mark:10-58 -->
	````C#
	public class WorkerRole : RoleEntryPoint
	{
	  ...
	
	  public override bool OnStart()
	  {
	    // Set the maximum number of concurrent connections 
	    ServicePointManager.DefaultConnectionLimit = 12;
	
	    // read storage account configuration settings
		var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

		// initialize blob storage
		CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
		this.container = blobStorage.GetContainerReference("guestbookpics");

		// initialize queue storage 
		CloudQueueClient queueStorage = storageAccount.CreateCloudQueueClient();
		this.queue = queueStorage.GetQueueReference("guestthumbs");

		Trace.TraceInformation("Creating container and queue...");

		bool storageInitialized = false;
		while (!storageInitialized)
		{
			 try
			 {
				  // create the blob container and allow public access
				  this.container.CreateIfNotExists();
				  var permissions = this.container.GetPermissions();
				  permissions.PublicAccess = BlobContainerPublicAccessType.Container;
				  this.container.SetPermissions(permissions);

				  // create the message queue(s)
				  this.queue.CreateIfNotExists();

				  storageInitialized = true;
			 }
			 catch (StorageException e)
			 {
				  var requestInformation = e.RequestInformation;
				  var errorCode = requestInformation.ExtendedErrorInformation.ErrorCode;//errorCode = ContainerAlreadyExists
				  var statusCode = (System.Net.HttpStatusCode)requestInformation.HttpStatusCode;//requestInformation.HttpStatusCode = 409, statusCode = Conflict
				  if (statusCode == HttpStatusCode.NotFound)
				  {
						Trace.TraceError(
						  "Storage services initialization failure. "
						  + "Check your storage account configuration settings. If running locally, "
						  + "ensure that the Development Storage service is running. Message: '{0}'",
						  e.Message);
						System.Threading.Thread.Sleep(5000);
				  }
				  else
				  {
						throw;
				  }
			 }
		}
	
	    return base.OnStart();
	  }
	}
	````

1. Replace the body of the **Run** method with the code shown below.

	<!-- mark:7-58 -->
	````C#
	public class WorkerRole : RoleEntryPoint
	{
	  ...
	
	  public override void Run()
	  {
	    Trace.TraceInformation("Listening for queue messages...");

		while (true)
		{
			 try
			 {
				  // retrieve a new message from the queue
				  CloudQueueMessage msg = this.queue.GetMessage();
				  if (msg != null)
				  {
						// parse message retrieved from queue
						var messageParts = msg.AsString.Split(new char[] { ',' });
						var imageBlobName = messageParts[0];
						var partitionKey = messageParts[1];
						var rowkey = messageParts[2];
						Trace.TraceInformation("Processing image in blob '{0}'.", imageBlobName);

						string thumbnailName = System.Text.RegularExpressions.Regex.Replace(imageBlobName, "([^\\.]+)(\\.[^\\.]+)?$", "$1-thumb$2");

						CloudBlockBlob inputBlob = this.container.GetBlockBlobReference(imageBlobName);
						CloudBlockBlob outputBlob = this.container.GetBlockBlobReference(thumbnailName);

						using (Stream input = inputBlob.OpenRead())
						using (Stream output = outputBlob.OpenWrite())
						{
							 this.ProcessImage(input, output);

							 // commit the blob and set its properties
							 outputBlob.Properties.ContentType = "image/jpeg";
							 string thumbnailBlobUri = outputBlob.Uri.ToString();

							 // update the entry in table storage to point to the thumbnail
							 GuestBookDataSource ds = new GuestBookDataSource();
							 ds.UpdateImageThumbnail(partitionKey, rowkey, thumbnailBlobUri);

							 // remove message from queue
							 this.queue.DeleteMessage(msg);

							 Trace.TraceInformation("Generated thumbnail in blob '{0}'.", thumbnailBlobUri);
						}
				  }
				  else
				  {
						System.Threading.Thread.Sleep(1000);
				  }
			 }
			 catch (StorageException e)
			 {
				  Trace.TraceError("Exception when processing queue item. Message: '{0}'", e.Message);
				  System.Threading.Thread.Sleep(5000);
			 }
	    }
	  }
	
	  ...
	}
	````
	
1. Finally, add the following method to the **WorkerRole** class to create thumbnails from a given image.

	<!-- mark:5-45 -->
	````C#
	public class WorkerRole : RoleEntryPoint
	{
	  ...
	
	  public void ProcessImage(Stream input, Stream output)
	  {
	    int width;
	    int height;
	    var originalImage = new Bitmap(input);
	
	    if (originalImage.Width > originalImage.Height)
	    {
	      width = 128;
	      height = 128 * originalImage.Height / originalImage.Width;
	    }
	    else
	    {
	      height = 128;
	      width = 128 * originalImage.Width / originalImage.Height;
	    }
	
	    Bitmap thumbnailImage = null;
	
	    try
	    {
	      thumbnailImage = new Bitmap(width, height);
	      
	      using (Graphics graphics = Graphics.FromImage(thumbnailImage))
	      {
	        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
	        graphics.SmoothingMode = SmoothingMode.AntiAlias;
	        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
	        graphics.DrawImage(originalImage, 0, 0, width, height);
	      }
	
	      thumbnailImage.Save(output, ImageFormat.Jpeg);
	    }
	    finally
	    {
	      if (thumbnailImage != null)
	      {
	          thumbnailImage.Dispose();
	      }
	    }
	  }
	}
	````
		
	>**Note:** Even though the code shown above uses classes in the System.Drawing namespace for simplicity, you should be aware that the classes in this namespace were designed for use with Windows Forms. They are not supported for use within a Windows or ASP.NET service. You should conduct exhaustive testing if you intend to use these classes in your own Windows Azure applications.

1. The entire code for _WorkerRole.cs_

````C#
using GuestBook_Data;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace GuestBook_WorkerRole
{
  public class WorkerRole : RoleEntryPoint
  {
    private CloudQueue queue;
    private CloudBlobContainer container;

    public override void Run()
    {
      Trace.TraceInformation("Listening for queue messages...");

      while (true)
      {
        try
        {
          // retrieve a new message from the queue
          CloudQueueMessage msg = this.queue.GetMessage();
          if (msg != null)
          {
            // parse message retrieved from queue
            var messageParts = msg.AsString.Split(new char[] { ',' });
            var imageBlobName = messageParts[0];
            var partitionKey = messageParts[1];
            var rowkey = messageParts[2];
            Trace.TraceInformation("Processing image in blob '{0}'.", imageBlobName);

            string thumbnailName = System.Text.RegularExpressions.Regex.Replace(imageBlobName, "([^\\.]+)(\\.[^\\.]+)?$", "$1-thumb$2");

            CloudBlockBlob inputBlob = this.container.GetBlockBlobReference(imageBlobName);
            CloudBlockBlob outputBlob = this.container.GetBlockBlobReference(thumbnailName);

            using (Stream input = inputBlob.OpenRead())
            using (Stream output = outputBlob.OpenWrite())
            {
              this.ProcessImage(input, output);

              // commit the blob and set its properties
              outputBlob.Properties.ContentType = "image/jpeg";
              string thumbnailBlobUri = outputBlob.Uri.ToString();

              // update the entry in table storage to point to the thumbnail
              GuestBookDataSource ds = new GuestBookDataSource();
              ds.UpdateImageThumbnail(partitionKey, rowkey, thumbnailBlobUri);

              // remove message from queue
              this.queue.DeleteMessage(msg);

              Trace.TraceInformation("Generated thumbnail in blob '{0}'.", thumbnailBlobUri);
            }
          }
          else
          {
            System.Threading.Thread.Sleep(1000);
          }
        }
        catch (StorageException e)
        {
          Trace.TraceError("Exception when processing queue item. Message: '{0}'", e.Message);
          System.Threading.Thread.Sleep(5000);
        }
      }
    }

    public override bool OnStart()
    {
      // Set the maximum number of concurrent connections 
      ServicePointManager.DefaultConnectionLimit = 12;

      // read storage account configuration settings
      var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

      // initialize blob storage
      CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
      this.container = blobStorage.GetContainerReference("guestbookpics");

      // initialize queue storage 
      CloudQueueClient queueStorage = storageAccount.CreateCloudQueueClient();
      this.queue = queueStorage.GetQueueReference("guestthumbs");

      Trace.TraceInformation("Creating container and queue...");

      bool storageInitialized = false;
      while (!storageInitialized)
      {
        try
        {
          // create the blob container and allow public access
          this.container.CreateIfNotExists();
          var permissions = this.container.GetPermissions();
          permissions.PublicAccess = BlobContainerPublicAccessType.Container;
          this.container.SetPermissions(permissions);

          // create the message queue(s)
          this.queue.CreateIfNotExists();

          storageInitialized = true;
        }
        catch (StorageException e)
        {
          var requestInformation = e.RequestInformation;
          var errorCode = requestInformation.ExtendedErrorInformation.ErrorCode;//errorCode = ContainerAlreadyExists
          var statusCode = (System.Net.HttpStatusCode)requestInformation.HttpStatusCode;//requestInformation.HttpStatusCode = 409, statusCode = Conflict
          if (statusCode == HttpStatusCode.NotFound)
          {
            Trace.TraceError(
              "Storage services initialization failure. "
              + "Check your storage account configuration settings. If running locally, "
              + "ensure that the Development Storage service is running. Message: '{0}'",
              e.Message);
            System.Threading.Thread.Sleep(5000);
          }
          else
          {
            throw;
          }
        }
      }

      return base.OnStart();
    }

    public void ProcessImage(Stream input, Stream output)
    {
      int width;
      int height;
      var originalImage = new Bitmap(input);

      if (originalImage.Width > originalImage.Height)
      {
        width = 128;
        height = 128 * originalImage.Height / originalImage.Width;
      }
      else
      {
        height = 128;
        width = 128 * originalImage.Width / originalImage.Height;
      }

      Bitmap thumbnailImage = null;

      try
      {
        thumbnailImage = new Bitmap(width, height);

        using (Graphics graphics = Graphics.FromImage(thumbnailImage))
        {
          graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
          graphics.SmoothingMode = SmoothingMode.AntiAlias;
          graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
          graphics.DrawImage(originalImage, 0, 0, width, height);
        }

        thumbnailImage.Save(output, ImageFormat.Jpeg);
      }
      finally
      {
        if (thumbnailImage != null)
        {
          thumbnailImage.Dispose();
        }
      }
    }

  }
}
````

1. The worker role also uses Storage services and you need to configure your storage account settings, just as you did in the case of the web role. To create the storage account setting, in **Solution Explorer**, expand the **Roles** node of the **GuestBook** project, double-click **GuestBook_WorkerRole** to open the properties for this role and select the **Settings** tab. Click **Add Setting**, type _"DataConnectionString"_ in the **Name** column, change the **Type** to _Connection String_, and then click the button labeled with an ellipsis. In the **Create Storage Connection String** dialog, choose the option labeled **Windows Azure storage emulator** and click **OK**. Press **CTRL + S** to save your changes.

	![2060DataConnectionString](images/2060dataconnectionstring.png?raw=true "DataConnectionString Setting")

	_Worker Role DataConnectionString Setting_

<a name="Ex2Verification"></a>
#### Verification ####

You now launch the updated application in the Windows Azure compute emulator to verify that the worker role can retrieve queued work items and generate the corresponding thumbnails.

1. Press **F5** to launch the service in the local compute emulator. 

1. Switch to Internet Explorer to view the application. If you completed the verification section of the previous exercise successfully, you will see the guest book entry that you entered, including the uploaded image displayed in its original size. If you recall, during the last task of that exercise, you updated the web role code to post a work item to a queue for each new entry submitted. These messages remain in the queue even though the web role was subsequently recycled. 

1. Wait a few seconds until the worker role picks up the queued message and processes the image that you are viewing. Once that occurs, it generates a thumbnail for this image and updates the corresponding URL property for the entry in table storage. Eventually, because the page refreshes every few seconds, it will show the thumbnail image instead.

	![2070Thumbnail](images/2070thumbnail.png?raw=true "Thumbnail created")

	_Home page showing the thumbnail generated by the worker role_

1. Back in the **Visual Studio** **"Server Explorer"** window, once again double click on the **"guestbookpics" Blob container and verify that the new thumbnail image is present. 

	![2080ThumbnailInBlobStorage](images/2080thumbnailinblobstorage.png?raw=true "Thumbnail in Blob Storage")

	_Blob container showing the blob for the generated thumbnail_

1. Likewise, because the Queue Message has been processed, you should no longer see the message in the **"questthumbs"** queue

	![2090NoQueueMessages](images/2090noqueuemessages.png?raw=true "No Queue Messages")

	_No queue messages_

1. And finally, you should see that the GuestBookEntry entity has been updated with the URL to the thumbnail image:

	![2100UpdatedGuestBookEntry](images/2100updatedguestbookentry.png?raw=true "Updated GuestBookEntry Entity")

	_GuestBookEntry has thumbnail url_

1. Add some more guest book entries. Notice that the images update after a few seconds once the worker role processes the thumbnails.

1. Return to Visual Studio and press **SHIFT + F5** to stop the debugger and shut down the deployment in the compute emulator.

---

<a name="Exercise3"></a>
### Exercise 3: Publishing a Windows Azure Application ###

In this exercise, you publish the application created in the previous exercise to Windows Azure using the Management Portal. First, you provision the required service components, upload the application package to the staging area and configure it. You then execute the application in the staging area to verify its operation. Finally, you promote the application to production.

>**Note:** In order to complete this exercise, you need a valid Windows Azure subscription.  If you don't have one, you can sign up for a [Free Trial](http://aka.ms/azft)

<a name="Ex3Task1"></a>
#### Task 1 – Creating a Storage Account and a Cloud Service Component ####

The application you publish in this exercise requires both compute and storage services. In this task, you create a new Windows Azure affinity group where your services will reside. In addition, you create  a new storage account to allow the application to persist its data and a cloud service component to execute application code.

> **Note** _Affinity groups_ allow you to group your Windows Azure services to optimize performance. All services within an affinity group will be located in the same data center. An affinity group is required in order to create a virtual network. You don't have to do this, but by creating a Virtual Network and Affinity group before you create your storage account and cloud service, you ensure that they will be in the same data centers, and that the Azure Fabric Controller is aware of their association with each other.  

1. Navigate to [https://manage.windowsazure.com](https://manage.windowsazure.com) using a web browser and sign in using the Microsoft Account associated with your Windows Azure account.

1. First, you create a new virtual network and an affinity group where your services will be deployed. In the Windows Azure menu, click **NEW**.

	![3010NewButton](images/3010newbutton.png?raw=true "New Button")

	_New button_

1. In the Networks menu, click **Virtual Network | Custom Create**.

	![3020CustomCreate](images/3020customcreate.png?raw=true "Custom Create")

	_Create Virtual Network

1. In the Create a Virtual Network dialog box complete the **Name** and in **Affinity group** select _Create a new affinity group_. Select the desired region and complete the **Affinity Group Name**. Click the arrow at the bottom of the dialog box.

	![3030VirtualNetworkDetails](images/3030virtualnetworkdetails.png?raw=true "Virtual Network Details")

	_Create Virtual Network dialog box_

1. In the DNS Servers and VPN Connectivity, leave the DNS Servers empty and click **Next** at the bottom of the dialog box.

	![3040DNSServers](images/3040dnsservers.png?raw=true "DNS Servers and VPN Connectivity")

	_DNS Servers and VPN Connectivity_

1. In the Virtual Network Address Spaces window, leave the default values and finish the wizard.

	![3050AddressSpaces](images/3050addressspaces.png?raw=true "Address Spaces")

	_Virtual Network Address Spaces_

1. In the **Networks** Tab, wait until the network status changes to _Created_.

	![3060NetworkCreated](images/3060networkcreated.png?raw=true "Network Created")

	_Virtual Network Created_


1. Now you will create the cloud service were the application will be deployed. To do so, click the **NEW** button at the bottom of the screen.

	![3010NewButton](images/3010newbutton.png?raw=true "New Button")

	_New button_

1. Go to **Compute | Cloud Service | Quick Create**. In the textbox labeled **URL**, enter the name for your cloud service, for example, **\<yourname\>guestbook**, where _\<yourname\>_ is a unique name. Windows Azure uses this value to generate the endpoint URLs for the storage account services. Then, select the drop down list labeled **Region/Affinity group** and pick the affinity group you created in the previous step. Click **Create cloud service** to start creating it.

	![3070NewCloudService](images/3070newcloudservice.png?raw=true "New Cloud Service")

	_New Cloud Service_

	>**Note:** The portal ensures that the name is valid by verifying that the name complies with the naming rules and is currently available. A validation error will be shown if you enter a name that does not satisfy the rules.
	>
	>![3080NameVerification](images/3080nameverification.png?raw=true "Name Verification")
	> 
	> Additionally, the reason that you can choose an affinity group is to deploy both the cloud service and storage account to the same location, thus ensuring high bandwidth and low latency between the application and the data it depends on.

1. Next, you will create the **storage account** where the application will store the data. You will click the **NEW** button at the bottom of the screen.

	![3010NewButton](images/3010newbutton.png?raw=true "New Button")

	_New button_

1.  Go to **Data Services | Storage | Quick Create**. In the textbox labeled **URL**, enter the name for your storage account, for example, **\<yourname\>guestbook**, where _\<yourname\>_ is a unique name. Then, select the drop down list labeled **Region/Affinity group**, pick the affinity group you created in the previous step and click **Create Storage Account**. Wait until the provisioning process completes and updates the **Storage** list view. 

	![3090NewStorageAccount](images/3090newstorageaccount.png?raw=true "New Storage Account")

1. In the Azure portal, switch to the **"Storage"** page, and click on the line next to the name of your new storage account.  Then from the bottom, click the **"Manage Keys"** button. 

	![3100ManageKeys](images/3100managekeys.png?raw=true "Manage Keys Button")

1. At the **Manage Access Keys** dialog, click the **"copy"** button to the right of the **"PRIMARY ACCESS KEY"** value. If prompted to allow access to the clipboard, click **"Allow"**", the click the **"Check Mark"** button to close the **"Manage Access Keys"** window:

	![3110ManageAccessKeys](images/3110manageaccesskeys.png?raw=true "Manage Access Keys")

	_Primary and Secondary Access Keys_
	
	>**Note:** The **Primary Access Key** and **Secondary Access Key** both provide a shared secret that you can use to access storage. The secondary key gives the same access as the primary key and is used for backup purposes. You can regenerate each key independently in case either one is compromised.  **You might want to paste the primary access key you just copied into a notepad window so you have it handy for a future step**. 


1. Do not close the browser window. You will use the portal for the next task.

<a name="Ex3Task2"></a>
#### Task 2 – Publishing the Application to the Windows Azure Management Portal ####

There are several alternatives for publishing applications to Windows Azure. The Windows Azure Tools for Visual Studio allow you to both create and publish the service package to the Windows Azure environment directly from Visual Studio. Another deployment option is the [Windows Azure Service Management PowerShell Cmdlets](http://msdn.microsoft.com/en-us/library/windowsazure/jj156055) that enable a scripted deployment of your application. Lastly, the Windows Azure Management Portal provides the means to publish and manage your service using only your browser. For more information about publishing applications, see the **Windows Azure Deployment** lab in this training kit.  You can also deploy your already packaged solutions through Azure Management Studio.

In this task, you publish the application to the staging environment using the Management Portal but first, you generate the service package using Visual Studio.

1. If it is not already open, launch **Microsoft Visual Studio 2013** as administrator . 

1. You can either continue working with the project from the previous exercises, or if need be, you can open the solution in the **"Source\End\GuestBook-AfterExercise2"** folder.  

1. Expand the **Roles** folder in the **GuestBook** cloud project. Right-click the **GuestBook_WebRole** and select **Properties**.

1. Switch to the **Settings** tab and locate the _DataConnectionString_ settings.

1. Click in the **Ellipsis** located under the **Value** column for the _DataConnectionString_.

	![3110Settings](images/3110settings.png?raw=true "Settings")

	_DataConnectionString settings_

	> **Note:** If you are prompted to sign in with your Microsoft Account credentials, you can do so, and then simply select your storage account from your subscription. However, you don't need to sign in to complete the following steps. 

1. In the **Create Storage Connection String** dialog box, select **Manually entered credentials** and complete the **Account name** and **Account key** of the storage account you've created in the previous task. Then click **OK** to create the storage account.

	![3120ConnectionStringDetails](images/3120connectionstringdetails.png?raw=true "ConnectionString Details")

	_Create Storage Connection String_

	>**Note:** Make sure to your YOUR storage account name and primary access key, not the values shown in the screenshot.  

1. Repeat the steps above to configure the connection string for the **Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString** in the **Web Role** and configure the same connection strings for the **Worker Role**.

	> **Note** Don't forget to do this twice, once for the **Web** role and again for the **Worker** role.

1. You can actually run the application locally on your development workstation, however the application should now be using Azure Storage in the cloud, not the local storage emulator.  Go ahead and give it a try, then after you add an entry, in your **Visual Studio** **"Server Explorer"**, connect to your Azure account, and view the contents in storage:

	![3130AzureStorageInServerExplorer](images/3130azurestorageinserverexplorer.png?raw=true "Azure Storage in Server Explorer")

	> **Note:** You should also notice that the sample data you had entered previously is no longer there.  That is because the sample data was on your local Development Storage Emulator, not in the cloud.  Now the app is using storage fromt the cloud, and that storage was initially empty. 

1. Ok, we are almost ready to push this app completely into the cloud.  The next step is to generate the package (.zip file) that has all of our compiled code, service definition, and service configuration information in it.  

1. Let's generate the package to publish to the cloud. To do this, right-click the **GuestBook** cloud project and select **Package**. 

	![3140Package](images/3140package.png?raw=true "Package")

1. In the **Package Windows Azure Application** dialog, select the **Service Configuration** and the **Build Configuration** you will use from the dropdowns and then click **Package**. 

	![3150CreatePackage](images/3150createpackage.png?raw=true "Create Package")

	_Creating a service package in Visual Studio_

	After Visual Studio builds the project and generates the service package, Windows Explorer opens with the current folder set to the location where the generated package is stored.  Copy the path to the output folder to your clipboard. We'll need that in the next step when we upload the package:

	![3160PackageFolder](images/3160packagefolder.png?raw=true "Package Folder")

	_Package Folder_

	>**Note:** Although the procedure is not shown here, you can use the Publish Cloud Service feature in the Windows Azure Tools to publish your service package directly from Visual Studio. To use this feature, you need to configure a set of credentials that you use to authenticate access to the management service using a self-issued certificate that you upload to the Management Portal.

1. Now, switch back to the browser window with the Management Portal opened.

1. In the cloud services list, click the name of the cloud service that you created before.

	![3170CloudService](images/3170cloudservice.png?raw=true "Cloud Service")

	_Cloud Service_

1. In the **Dashboard** page, click **Staging** and then **Upload a New Staging Environment**. 

	![3180UploadToStaging](images/3180uploadtostaging.png?raw=true "UploadToStaging")

	_Deploy to staging environment_

	>**Note:** A cloud service is a service that runs your code in the Windows Azure environment. It has two separate deployment slots: staging and production. The staging deployment slot allows you to test your service in the Windows Azure environment before you deploy it to production.

1. In the **Upload a Package** dialog, enter a label to identify the deployment; for example, use **FirstVersion**.

	>**Note:** The portal displays the label in its user interface for staging and production, allowing you to identify the version currently deployed in each environment.

1. To select the **Package** from the file system, click **From Local**, navigate to the folder where Visual Studio generated the package then select **GuestBook.cspkg**. 

1. Now, to choose the **Configuration** file, click **From Local** and select **ServiceConfiguration.cscfg** in the same folder that you used in the previous step.

	>**Note:** The _.cscfg_ file contains configuration settings for the application, including the instance count that you will update later in the exercise.

1. Finally, check the option labeled **Deploy even if one or more roles contain a single instance**. Click the **Tick** to start the deployment. 

	>**Note1:** This checkbox is important. Don't forget.


	>**Note2:** In this particular case, only a single instance is being deployed for at least one of the roles. This is not recommended because it does not guarantee the service’s availability. In the next task, you will increase the number of instances to overcome this issue.

	![3190UploadAPackage](images/3190uploadapackage.png?raw=true "Upload a Package")

	_Upload package to staging_

1. Notice that the package begins to upload and that the portal shows the status of the deployment to indicate its progress. 

	![3200Status](images/3200status.png?raw=true "Status")

	_Uploading cloud service_

1. Wait until the deployment process finishes, which may take several minutes. Once the **"STATUS"** reads **"Running"**, Notice the **Site Url** assigned to your deployment under the **quick glance** section.
	
	![3210StagedService](images/3210stagedservice.png?raw=true "Staged Service")

	_Staged Service_

	>**Note** You can use the dashboard to verify progress.
	
	>During deployment, Windows Azure analyzes the configuration file and copies the service to the correct number of machines, and starts all the instances. Load balancers, network devices and monitoring are also configured during this time.

	>Your new cloud service has a **DNS name** asigned, an URL that points to your web role home page.

<a name="Ex3Task3"></a>
#### Task 3 – Configuring the Application to Increase the Number of Instances ####

Once your application is deployed you can make certain changes, like channging the number of instances of each role, while the service is live.  

1. In **Cloud Services**, click on your **GuestBook** service and click **Scale** on the ribbon.

	![3330ScalePage](images/3330scalepage.png?raw=true "Scale Page")

	_Scaling instances_

1. On the **Scale** page, view the number of instances for the GuestBook_WebRole:

	![3340GuestBookWebRoleInstances](images/3340guestbookwebroleinstances.png?raw=true "GuestBook_WebRole Instances")

	_GuestBook_WebRole Instances_

	> **Note:** Alternatively, you can change the instance count entering the new number into the text boxes at right.

	> This setting controls the number of roles that Windows Azure starts and is used to scale the service. For a token-based subscription—currently only available in countries that are not provisioned for billing—this number is limited to a maximum of two instances. However, in the commercial offering, you can change it to any number that you are willing to pay for.

1. Drag the **INSTANCE COUNT** slider to 2 and click the **"SAVE"** button along the bottom:

	![3350ChangeInstanceCount](images/3350changeinstancecount.png?raw=true "Change Instance Count")

	_Saving the instance count_

1. Wait for the Scale operation to complete before continuing:

	![3360ScaleStatus](images/3360scalestatus.png?raw=true "Scale Status")

<a name="Ex3Task4"></a>
#### Task 4 – Testing the Application in the Staging Environment ####

In this task, you run the application in the staging environment and access its public endpoint to test that it operates correctly.

1. Go to your cloud service's **dashboard** and then click the **Site Url** link under the **quick glance** section.

	![3370SiteUrl](images/3370siteurl.png?raw=true "Site URL")

	_Site URL_

	>**Note:** The link shown for Site URL name has the form _\<guid\>.cloudapp.net_, where _<guid>_ is some random identifier. This is different from the address where the application will run once it is in production. Although the application executes in a staging area that is separate from the production environment, there is no actual physical difference between staging and production – it is simply a matter of where the load balancer is connected. 

1. If you wish, you may test the application by signing the guest book and uploading an image.

	![3380StagingSite](images/3380stagingsite.png?raw=true "Staging Site")

	_Application running in the staging environment_

<a name="Ex3Task5"></a>
#### Task 5 – Promoting the Application to Production ####

Now that you have verified that the service is working correctly in the staging environment, you are ready to promote it to final production. When you deploy the application to production, Windows Azure reconfigures its load balancers so that the application is available at its production URL.

1. In **Cloud Services**, click on your service and then click **Swap** at the bottom menu.

	![3390VIPSwap](images/3390vipswap.png?raw=true "VIP Swap")

	_Swap slots_

1. On the **VIP Swap** dialog, click **Yes** to swap the deployments between staging and production.

	![3400ConfirmSwap](images/3400confirmswap.png?raw=true "Confirm VIP Swap")

	_Promoting the application to the production slot_

1. Wait for the promotion process to complete.

	![3410SwapStatus](images/3410swapstatus.png?raw=true "Swap Status")

	_Swapping deployments_

1. When the promotion process is complete, you will no longer have a **"Staging"** deployment:

	![3420NoStaging](images/3420nostaging.png?raw=true "No Staging Deployment")

	_No Staging Deployment_

1. Switch to the **"Production"** page and verify that there is an active deployment.  Scroll down and click the **Site URL** link to open the production site in a browser window and notice the URL in the address bar.

	![3430ProductionDeployment](images/3430productiondeployment.png?raw=true "ProductionDeployment")

	_Production Deployment_

1. Verify that the site loads correctly from the Production URL: 

	![3440ProductionSite](images/3440productionsite.png?raw=true "Production Site")

	>**Note:** If you visit the production site shortly after its promotion, the DNS name might not be ready. If you encounter a DNS error (404), wait a few minutes and try again. Keep in mind that Windows Azure creates DNS name entries dynamically and that the changes might take few minutes to propagate.

1. Even when a deployment is in a suspended state, Windows Azure still needs to allocate a virtual machine for each instance and charge you for it. Once you have completed testing the application, you need to remove the deployment from Windows Azure to avoid an unnecessary expense. To remove a running deployment, go to **Cloud Services**, select the deployment slot where the service is currently hosted, staging or production, then click **Stop** on the bottom pane and accept the confirmation prompt. Once the service has stopped, click **Delete** on the bottom pane and click **Delete the production deployment for cloud service yourusernameguestbook** to remove it.

	![4010DeleteServiceAndDeployments](images/4010deleteserviceanddeployments.png?raw=true "Delete the Cloud Service and it's Deployments")

	_Delete the cloud service and all its deployments to prevent being charged_

---

<a name="summary"></a>
## Summary ##

By completing this hands-on lab, you have reviewed the basic elements of Windows Azure applications. You have seen that services consist of one or more web roles and worker roles. You have learned about Storage services and in particular, Blob, Table and Queue services. Finally, you have explored a basic architectural pattern for cloud applications that allows front-end processes to communicate with back-end processes using queues.
