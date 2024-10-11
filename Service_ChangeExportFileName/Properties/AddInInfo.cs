using Mono.Addins;

// Declares that this assembly is an add-in
[assembly: Addin("Service_ChangeExportFileName", "1.0")]

// Declares that this add-in depends on the scada v1.0 add-in root
[assembly: AddinDependency("::scada", "1.0")]

[assembly: AddinName("Service_ChangeExportFileName")]
[assembly: AddinDescription("")]