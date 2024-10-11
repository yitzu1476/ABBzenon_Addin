# About this Repository
This repository contains several add-in projects for COPA-DATA zenon SCADA system, and can only work with certain developer tool: https://marketplace.visualstudio.com/items?itemName=vs-publisher-1463468.COPA-DATASCADAAdd-InDeveloperToolsforVS22
<br />
<br />
<br />
## Contents

**IEC 61850 FTP Tool**
- IEC 61850 FTP Tool (Manual)
  - This program opens up a page in Runtime for with access of Disturbance Records through IEC 61850 to relays.
  - See [description file](DescriptionFiles/FTP+Tool+-+Manual+FTP+Tool+Explains.pdf) for details.
  - Folder: [IEC61850_FileTransfer](IEC61850_FileTransfer/)<br />
    
- IEC 61850 FTP Tool (Auto)
    - This program collect Disturbance records and download to local pc automatically when a record is created in relay.
    - See description file for details.
    - Folder: [IEC61850_FTOAuto_DRtrig_Editor](IEC61850_FTOAuto_DRtrig_Editor/), [IEC61850_FTPAuto_DRtrig](IEC61850_FTPAuto_DRtrig/)<br /><br />

**IEC 61850 Diagnosis Tool**
- IEC 61850 Variable Diagnosis Tool
  - This program opens a window for variable diagnosis for IEC 61850.
  - See description file for details.
  - Folder: [IEC61850_variableDiagnosis_81](IEC61850_variableDiagnosis_81/), [IEC61850_variableDiagnosis_Container_81](IEC61850_variableDiagnosis_Container_81/), [IEC61850_variableDiagnosis_Editor_81](IEC61850_variableDiagnosis_Editor_81/)<br /><br />

**Gateway Tool**
- Gateway Engineering Tool
  - This program works as engineering tool for IEC61850 to DNP3 gateway.
  - See description file for details.
  - Folder: [Gatewat_EditorTool](Gatewat_EditorTool/), [Gateway_SerialMonitor](Gateway_SerialMonitor/)<br />
  
- OPC Variable Creator
  - This program creates OPC variable text file from SPA info.
  - See description file for details.
  - Folder: [OPC_VariableCreator](OPC_VariableCreator/)<br /><br />

**Report Tool**
- Archive to CSV Report Tool
  - This program create csv file from selected archive data.
  - See description file for details.
  - Folder: [Archive2CSV](Archive2CSV/)<br />
  
- Filename Service Tool
  - This program changes filename automatically after creating report files.
  - See description file for details.
  - Folder: [Service_ChangeExportFileName](Service_ChangeExportFileName/)<br />
  
- Report Tool with equipment model
  - This program create report files with equipment modelling setting and selected content.
  - See description file for details.
  - Folder: [ReportCreator_EquipmentModel](ReportCreator_EquipmentModel), [ReportCreator_EnergyArchiveTool](ReportCreator_EnergyArchiveTool/)<br />
  

