# Deployment Instructions for IIS

Follow these steps to deploy the ContractCheck application to IIS (Internet Information Services).

## 1. Prerequisites

- Windows Server with IIS enabled.
- .NET Core Hosting Bundle installed. If not installed, download and install it from the official Microsoft website.

## 2. Publish the Application

1.  Open a terminal or command prompt in the root directory of the project.
2.  Run the following command to publish the application:

    ```sh
    dotnet publish --configuration Release
    ```

    This will create a `publish` folder in `bin\Release\net9.0\publish\`.

## 3. Create a New Website in IIS

1.  Open IIS Manager.
2.  In the **Connections** pane, right-click on the **Sites** node and select **Add Website**.
3.  Enter a **Site name** (e.g., `ContractCheck`).
4.  For the **Physical path**, browse to the `publish` folder created in the previous step.
5.  Choose a **Port** (e.g., 8080) or configure bindings as needed.
6.  Click **OK**.

## 4. Configure the Application Pool

1.  In the **Connections** pane, select **Application Pools**.
2.  Right-click on the application pool for your site (it should have the same name as your site) and select **Basic Settings**.
3.  Set the **.NET CLR version** to **No Managed Code**. The application runs in its own process and is managed by the ASP.NET Core Module.
4.  Click **OK**.

## 5. Configure appsettings.json

1.  Open the `appsettings.json` file in the `publish` folder.
2.  Update the `ConnectionStrings` section with the correct credentials for your SQL Server database.

## 6. Browse to the Website

- Open a web browser and navigate to the URL of your site (e.g., `http://localhost:8080`).
