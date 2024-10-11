using Mono.Addins;

// Declares that this assembly is an add-in
[assembly: Addin("IEC61850_VariableDiagnosis_81", "1.0")]

// Declares that this add-in depends on the scada v1.0 add-in root
[assembly: AddinDependency("::scada", "1.0")]

[assembly: AddinName("IEC61850_VariableDiagnosis_81")]
[assembly: AddinDescription("")]