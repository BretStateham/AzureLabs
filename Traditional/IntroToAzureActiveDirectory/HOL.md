<a name="Title" />
# Introduction to Windows Azure Active Directory - Visual Studio 2013#

---
<a name="Overview" />
## Overview ##

The goal of this post is to illustrate authentication for applications posted in Windows Azure websites. This particular post will focus on ASP.NET, not node.js or other open-source platforms, including PHP.

You will learn the basic skills of identity management in the cloud. applications can be as powerful as allowing web-based applications that manage a company's active directory. on a more fundamental level active directory can be used to validate users to allow access to secure information.


<a name="anchor-name-here" />
#### Benefits of Active Directory ####
- It makes the task of network administration simpler by maintaining a central repository of information.
- It provides a single destination to look out for information.
- It provides highly secured access to data through the usage of security policies. 
	-	Thereby it improves the management of security data and workflows.
- Easily scalable. Supports millions of objects in a single domain.
- Unified access to resources by supporting a uniform naming convention.

<a name="anchor-name-here" />
#### Key Facts about Single Sign-On ####


- **In concrete terms** This means that the application will be configured to redirect unauthenticated requests to the IdP, according to some sign-on protocol such as SAML-P, WS-Federation or OpenID Connect. (idP means identity provider)

- **Windows Azure Active Directory** makes it easy to **authenticate** and **authorize users**. 

- You don't need to spend much time worrying about the vast landscape of **changing security standards**. 

- **Windows Azure Active Directory** is a cloud-based service that provides an **easy way of authenticating and authorizing users to gain access to your web applications** and services while allowing the features of **authentication and authorization to be factored out of your code**. 

- Because it is a service, all that security plumbing code is something you DO NOT NEED to worry about. 

- **Windows Azure Active Directory** supports integrated **Single Sign On** and centralized authorization into your web applications 

- Users hate having to log on mutliple times. If you want to retain your users, you need to allow them to just login once. 

- **Windows Azure Active Directory** provides out-of-the-box support for **Active Directory Federation Services (AD FS) 2.0** 

- Great integration story with on-premise corporate users. 
**Windows Azure Active Directory** relies on **Standards-based identity providers** 
Web identities include **Windows Live ID, Google, Yahoo! and Facebook** 

<a name="anchor-name-here" />

#### SAML ####

SAML Tokens, WS-Federation, https

![saml](images/saml.png?raw=true)

**SAML** is an XML-based open standard data format for exchanging authentication and authorization data between parties, in particular, between an identity provider and a service provider. 

- **SAML** is a product of the OASIS Security Services Technical Committee

#### SOAP ####

SOAP on the decline relative to REST-based technologies

![soap](images/soap.png?raw=true)

SOAP is for exchanging structured information in the implementation of Web Services in computer networks. 

It relies on XML Information Set for its message format, and usually relies on other Application Layer protocols, most notably Hypertext Transfer Protocol (HTTP) or Simple Mail Transfer Protocol (SMTP), for message negotiation and transmission.




#### What is WS-Federation?####

WS-Federation is a WS-* specification and OASIS standard 

- Tries to tackle the security challenges in **web applications** and **web services** in a **variety of trust relationships**. 

- WS-Federation supports SOAP clients and web services. 
REST is not supported. 
- **WS-Federation** provides a common model for performing **Federated Identity** operations for both **web services** and **browser-based applications**. 
- **WS-Federation** builds upon the **Security Token Service** model 
- **WS-Security, WS-Trust**, and **WS-SecurityPolicy** provide a basic model for federation between **Identity Providers** and **Relying Parties** 
- **WS-Federation** are **claims-based**. 
- **Claims** are just **dictionaries** inside of a **SAML token** where key value pairs represent the **attributes of a user** 
- Developers can iterate through dictionary to see attributes of a user’s identity to determine what kind of claims we want to authorize 
- These **claims** are also called **assertions**. 
**WS-Federation** strives for richer trust relationships 
- It wants to join together **HTTP** with **STS** and **WS-Trust** to allow resources in **one** **realm** get identities (and related attributes) that are managed and maintained in **another** **realm**. 
- You can imagine scenarios where **WS-Federation** is needed, such as protecting patient records from unauthorized users when these patient records are traveling among multiple health providers. 
- Federation makes sense in a supply chain scenario where a factory needs just-in-time inventory levels from distributors or retailers (and in reverse). 

<a name="anchor-name-here" />
#### What is single sign-on? ####

**Single sign-on** (**SSO**) is a property of access control of multiple related, but independent software systems. 

- With **single sign-on** a user logs in once and gains access to all systems without being prompted to log in again at each of them. 

![Windows Azure AD Architecture Overview](Images/windows-azure-ad-architecture-overview.png?raw=true)

**Prerequisites**

- Visual Studio 2013 

- An account on Windows Azure. You can get a free account here <https://manage.windowsazure.com/>.


<a name="anchor-name-here" />
## Exercise 1 ##

In this section you will log into the portal and choose to create a new active directory service. In the left menu pane choose **Active Directory**.

<a name="anchor-name-here" />
### Task 1: Entering basic directory information ###

In the next few screens you'll enter information about the newly created directory. You will need to provide a login name and domain name to be used.

1. You will add a new active directory. Select active directory from the left menu and click new.  **WARNING - You can't currently delete an Azure Active Directory.  Make sure to use a name you are comfortable keeping in your subscription.** 

	![ex1_task1_a.png](images/ex1_task1_a.png?raw=true)

	_Creating a new active directory_

1. Enter in the name, the domain name, and choose the country or region.

	![ex1_task1_b.png](images/ex1_task1_b.png?raw=true)

	_Adding a new directory_

1. Changing details about the directory (brunodirectory). Click the arrow to drill into a specific directory.

	![ex1_task1_c.png](images/ex1_task1_c.png?raw=true)

	_Changing directory details_

1. You will add users. Click the user menu.

	![ex1_task1_d.png](images/ex1_task1_d.png?raw=true)

	_Adding users_

1. From the left menu bar choose add user.

	![ex1_task1_d2.png](images/ex1_task1_d2.png?raw=true)

	_Adding a user_

1. You will enter a username for the provider directory.

	![ex1_task1_e.png](images/ex1_task1_e.png?raw=true)

	_Adding a user_

1. You will enter user profile details. Enter first, last, display name. Also select the role for the user. Choose global administrator. Also indicate an email address.

	![ex1_task1_f.png](images/ex1_task1_f.png?raw=true)

	_Entering user profile details_

1. Creating a temporary password. Click the create button

	![ex1_task1_g.png](images/ex1_task1_g.png?raw=true)

	_Creating a temporary password_

1. You will be given a new password. You will need to select an email address to send a confirming email to.

	![ex1_task1_h.png](images/ex1_task1_h.png?raw=true)

	_Confirming your temporary password_

1. Validating that the user was added. For the details section of your directory, notice the display name should match the one entered, in addition to the user name and domain.

	![ex1_task1_i.png](images/ex1_task1_i.png?raw=true)

	_Validating user profile details_

<a name="anchor-name-here" />
## Exercise 2 ##
Exercise two is about creating an MVC application that can leverage Azure active directory, or directory services. you will learn the built-in tooling inside of Visual Studio is that makes this possible.

<a name="anchor-name-here" />
### Task 1: Creating the web application ###

In this task you will create the ASP.net web application.




1. Start Visual Studio 2013 as administrator. From the start page click new project.

	![ex2_task1_a.png](images/ex2_task1_a.png?raw=true)

	_Creating a new MVC project_

1. From the templates choose Visual C#. from the middle pane select ASP.NET Web Application. Then provide a name, location and click ok.

	![ex2_task1_b.png](images/ex2_task1_b.png?raw=true)

	_Creating a new ASP.net web application_

1. From the template pane select MVC. Then click change authentication.

	![ex2_task1_c.png](images/ex2_task1_c.png?raw=true)

	_Specifying an MVC project with authentication_

1. Login with the credentials from the previous exercise. These are the credentials that you specified at the Azure portal.

	![ex2_task1_d.png](images/ex2_task1_d.png?raw=true)

	_Logging in to your domain_

1. You will need to create a new password. You will need your old one to do so from the previous exercise.

	![ex2_task1_e.png](images/ex2_task1_e.png?raw=true)

	_Creating a new password_

1. There are some important selections here. First choose organizational accounts from the options on the left. On the right pane indicate cloud - single organization, a domain of brunodirectory.onmicrosoft.com and an access level of single sign-on, read and write directory privileges. This will enable the logged in user (azureuser) to modify directory entries, not just read them.

	![ex2_task1_f.png](images/ex2_task1_f.png?raw=true)

	_Changing authentication for the MVC application_

<a name="anchor-name-here" />
### Task 2: Testing the entire process ###

This task is about testing the authentication mechanism to make sure it works. we may do a follow-up lab after this one to see exactly how the user can modify the directory graph. As you recall, the user we created had read/write privileges.


1. You will not test the login process. From the debug menu of Visual Studio click on Internet Explorer.

	![ex2_task1_g.png](images/ex2_task1_g.png?raw=true)

	_Testing your MVC authentication mechanism._

1. As you might expect you are asked to provide some security credentials to continue.

	![ex2_task1_h.png](images/ex2_task1_h.png?raw=true)

	_Continuing to the website to enter security credentials_

1. Notice that the user is logged in. Your MVC application has been authenticated using active directory. The currently logged in user can even modify the directory graph for this domain ( brunodirectory@onmicrosoft.com ).

	![ex2_task1_i.png](images/ex2_task1_i.png?raw=true)

	_Validating login_


<a name="anchor-name-here" />
## Summary ##

In this post we learned how to leverage directory services in Azure. we created an MVC web application that was able to login a user created at the Azure portal. In the next post we may discover that we can actually modify directory information assuming users were added having global administrative privileges.

