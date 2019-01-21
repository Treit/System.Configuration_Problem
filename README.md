# System.Configuration issue when consuming .NET Standard libraries

## Problem Overview
This repo contains a trivial HelloWorld project that illustrates a problem with consuming .NET Standard libraries from a .NET 4.7.2 project, related to reading configuration files.

The specific issue arises if the configuration file used by the .NET 4.7.2 project contains certain configuration sections, which are valid for full .NET 4.7.2 applications but are unrecognized by the System.Configuration.ConfigurationManager package utilized by .NET Standard applications.

In the example application, the HelloWorld project's app.config has a system.serviceModel section that causes the configuration system to fail to load at runtime, because the project also has a dependency on a trivial .NET Standard 2.0 class library that attempts to read configuration values.

## Dependency Graph
	HelloWorld.exe
        -> SampleLibrary.dll
               -> System.Configuration.ConfigurationManager 4.5.0 (NuGet package)

## To reproduce
Simply build the included HelloWorld\HelloWorld.csproj project and then attempt to run it.

## Problematic configuration example:
    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
        <startup> 
            <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
        </startup>
      <appSettings>
        <add key="test" value="someValue"/>
      </appSettings>
      <system.serviceModel>
      </system.serviceModel>
    </configuration>

In the above app.config configuration file, the system.serviceModel section causes the following runtime failure:

> Unhandled Exception: System.Configuration.ConfigurationErrorsException: Configuration system failed to initialize ---> System.Configuration.ConfigurationErrorsException: Unrecognized configuration
section system.serviceModel. (E:\src\git\mtreit\System.Configuration_Problem\HelloWorld\bin\debug\HelloWorld.exe.config line 9)
   at System.Configuration.ConfigurationSchemaErrors.ThrowIfErrors(Boolean ignoreLocal)
   at System.Configuration.BaseConfigurationRecord.ThrowIfParseErrors(ConfigurationSchemaErrors schemaErrors)
   at System.Configuration.BaseConfigurationRecord.ThrowIfInitErrors()
   at System.Configuration.ClientConfigurationSystem.EnsureInit(String configKey)

How can we consume .NET Standard 2.0 libraries that need to read configuration values, from applications whose configuration file contains sections unrecognized by the more restricted .NET Standard-compatible configuration manager code? 
