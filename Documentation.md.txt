MicroBee Jobs
=================
MicroBee Jobs is a portable Xamarin Forms mobile application for sharing small jobs and tasks. These include for example garden/household
tasks, aid with children and pets, IT and other jobs of freelance character and many others.

Target platforms
------------------
### Android
* minimal: 4.4 Kit Kat
* target: 8.1 Oreo

### iOS
* target: iOS 8.0

### UWP
* target: Windows 10 Fall Creators Update

User documentation
------------------
In this section, application usage and authentication (and corresponding pages) will be specified. Apart from the login, register and main
page, a side slide menu is accessible. This means navigation is performed by selecting side menu items.

### Main page
This is the starting page for an anonymous user. It provides access to login and register pages. If the user has performed
the login/registration before (on the same device), they are authomatically authenticated and redirected to the Job list page.

### Login page
The page contains a login form.  An error message is displayed if the login does not succeed. After login, the user is redirected
to the Job list page.

### Register
The page contains a register form. The password must contain at least one non-alphanumeric character, at least one number and both
uppercase and lowercase letters. Minimal password length is 6 characters. A valid e-mail must be specified.
Just as after login, user is authenticated and redirected to the Job list page.

### Job list page
Works as the effective main page of the application (provided that user is authenticated). It is refreshable, searchable (by job title) and
allows to filter jobs by category. It shows only items which do not have a worker user assigned. On android & iOS, pull-to-refresh
is enabled, for UWP a refresh button is present instead. The list will load more items as the user scrolls down and also refreshes
authomatically after changing the filter parameters. When a list item is selected, a carousel-like swipeable job detail page is opened.

### Job detail page
Contains all data of the selected job. It works as a carousel page, so it is possible to navigate through jobs by swiping left and right.
When opened from the job list page, it is possible to accept the job (unless the user is the owner of the job). If opened from user created
jobs page, edit is possible. In the edit mode, owner of the job can change the job data and also select a new image from the device gallery.

### Created jobs page
Contains a list of jobs created by the current user. A button for adding a new job is also present. Unlike the jobs page, all items are
loaded immediately. The selection works in the same manner as on the jobs page, with the difference of enabled edit button and
disabled accept button.

### Accepted jobs page
Contains a list of jobs accepted by the current user. The selection works in the same manner as on the jobs page, but both the edit and
accept button is disabled. A message is shown if there are no accepted jobs.

### User profile page
Here users can access their data and job count statistics. It is also possible to log out (the user is then redirected to the main page).

### Add job page
Provides a form for adding a job. The picture does not have to be included and can be changed later on the edit page. The added job will
be immediately added to the job list and can be accepted.

Developer documentation – Xamarin Forms
------------------------
### Configuration
As for now, api paths are hardcoded in startup classes and in the services. Login credentials are stored in a platform-specific secure storage.
As an extension, a dependency injection framework should be used to handle services.

### Page design
Most of the page design is found in the XAML files, with initialization and handler methods in the .xaml.cs files. The MVVM pattern is used
for almost every page (only the shortest pages which shouldn't be much extended lack it). App.xaml contains a few styles to be used globally
in the app.

### Data access
The data is accessed by sending requests to a RESTful API (see ASP.NET Core section of the documentation). There are two services, 
IAccountService and IMicroItemService, providing account and items data respectively. The implementations of the interfaces reference
a helper HttpService.
---
The HttpService facilitates http request creation by providing generic GET, POST, PUT and DELETE methods. There are also some specific
methods for handling byte[] data (for the purpose of image transfer). Login and register data is also handled by the service. After a call of
LoginAsync or RegisterAsync, a bearer jwt token is received and stored in the device (platform specific) secure storage. The token lifetime
is authomatically checked in every subsequent request and the token is refreshed if needed.

The HttpService is a wrapper around a HttpClient instance, which is instantiated only once. DNS change (a singleton HttpClient does not
respect DNS changes) problem is not handled yet.
---
Every data access is asynchronous, as is required by the nature of the HttpClient. Therefore, async void event handler methods are frequently
present.

### Platform specific features
There are some minor platform specific design changes. As for now, there is no major difference.
---
See external libraries - platform specific behavior handled by them includes:
* selecting picture from gallery
* secure storage for password

Developer documentation – ASP.NET Core
------------------------
This project serves as a backend service for the mobile application. It provides a web API, which sends json content (entities or models).
Authentication is handled by jwt bearer tokens.

### Configuration
Configuration of the web api is handled in the Startup class. Some of the settings are stored in the appsettings.json file (namely token
information, local connection string). In Starup.cs, db context, services, repositories and jwt token authentization is configured.
Entity configuration (ApplicationUser and MicroItem) is handled in separated classes (and is applied in the Startup).

### Controllers
There are two controllers - AccountController and ItemsController. The latter also handles image and category requests. In some cases,
entities are returned as models, in other cases custom models are created (e.g. LoginModel).

### Services
There are several services which provide access to the DAL. Apart from CRUD operations, IMicroItemService provides also basic filtering.

### DAL
The Data Access Layer contains entity data, database context and repositories. As specified in the technologies section, the O/RM framework
Entity Framework Core is used in the implementation of the repositories. Database structure is created and updated according to
migrations.

Technologies
--------------
* Xamarin.Forms v3.1.0
* AspNetCore.All v2.1.3

### Xamarin Forms external libraries
List of used NuGet packages:
* Xamarin.Essentials v0.10.0-preview - for SecureStorage
* Xamarin.Forms.Extended.InfiniteScrolling v1.0.0-preview2
* Xam.Plugin.Media v4.0.1.5 - for CrossPlatform media access (picture selection)
* Newtonsoft.Json v11.0.2 - for http content deserialization

### ASP.NET Core
* Entity Framework Core (included in AspNetCore.All)
The project is deployed to Azure and can be accessed at http://microbee-jobs.azurewebsites.net/[route]