using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static GW_EditorTool.Form1;

namespace GW_EditorTool
{
    public class XML_Creator
    {
        public XML_Creator(){ }

        public string AboveDataBase(string Station, List<Station_List> Allstations)
        {
            string AboveDB = "";

            foreach (var item in Allstations)
            {
                if (item.StationName == Station)
                {
                    if (item.Channel == "TCP/IP")
                    {
                        AboveDB = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<AccessDNP3_SG>\r\n  " +
                            "<Configuration Version=\"1\">\r\n    <Outstation Version=\"1\">\r\n      <PhysicalLayer Version=\"1\">\r\n        " +
                            "<ChannelType>" + item.Channel + "</ChannelType>\r\n        " +
                            "<ListeningPort>" + item.TPCListeningPort + "</ListeningPort>\r\n        <UdpBroadcastPort>20000</UdpBroadcastPort>\r\n        " +
                            "<NetworkCard>" + item.NetworkCard + "</NetworkCard>\r\n        " +
                            "<SerialPort>COM1</SerialPort>\r\n        " +
                            "<SerialBaudrate>9600</SerialBaudrate>\r\n        " +
                            "<SerialSettings>8,1,1,N</SerialSettings>\r\n      </PhysicalLayer>\r\n      " +
                            "<Datalink Version=\"1\">\r\n        " +
                            "<OutstationDNPAddress>" + item.OutstationAdd + "</OutstationDNPAddress>\r\n        " +
                            "<MasterDNPAddress>" + item.MasterAdd + "</MasterDNPAddress>\r\n        <LinkStatusRequestInterval>0</LinkStatusRequestInterval>\r\n        <DLConfirmations>false</DLConfirmations>\r\n        <DLRetries>0</DLRetries>\r\n        <DLTimeout>2000</DLTimeout>\r\n        " +
                            "<MasterIPAddress>" + item.MasterIP + "</MasterIPAddress>\r\n        <MasterDualEndpointPort>20000</MasterDualEndpointPort>\r\n        <EnableSelfAddress>false</EnableSelfAddress>\r\n        <ValidateMasterAddress>true</ValidateMasterAddress>\r\n        <UseSpecificMasterUdpPort>false</UseSpecificMasterUdpPort>\r\n        <MasterUdpPort>20000</MasterUdpPort>\r\n        <ApplicationLayer Version=\"1\">\r\n          <ALTimeout>3000</ALTimeout>\r\n          <ALConfirmation>false</ALConfirmation>\r\n          <TXSize>2048</TXSize>\r\n          <LocalTime>true</LocalTime>\r\n          <SyncInterval>60</SyncInterval>\r\n          <SBOTimeout>60</SBOTimeout>\r\n          <DeviceName>Outstation</DeviceName>\r\n          <DeviceID>0</DeviceID>\r\n          <ControlVariable Version=\"2\">\r\n            <Name/>\r\n            <Identification/>\r\n            <ControlType>1</ControlType>\r\n          </ControlVariable>\r\n          <DeviceLocation>1</DeviceLocation>\r\n          <FunctionsAllowed Version=\"1\">\r\n            <ColdWarmRestart>false</ColdWarmRestart>\r\n            <FileTransfer>false</FileTransfer>\r\n            <AssignClass>false</AssignClass>\r\n            <TimeSync>false</TimeSync>\r\n            <Controls>true</Controls>\r\n            <CounterFreeze>true</CounterFreeze>\r\n            <Broadcast>false</Broadcast>\r\n          </FunctionsAllowed>\r\n        </ApplicationLayer>\r\n        <EventsUnsolicited Version=\"1\">\r\n          <DeleteOldest>false</DeleteOldest>\r\n          <SortTimeStamp>false</SortTimeStamp>\r\n          <AllowUnsolicited>true</AllowUnsolicited>\r\n          <UnsolicitedConfirmTimeout>5</UnsolicitedConfirmTimeout>\r\n          <UnsolicitedConfirmRetries>3</UnsolicitedConfirmRetries>\r\n          <UnsolicitedConfirmDelay>15</UnsolicitedConfirmDelay>\r\n          <Class1Events>3</Class1Events>\r\n          <Class1Time>5</Class1Time>\r\n          <Class2Events>3</Class2Events>\r\n          <Class2Time>5</Class2Time>\r\n          <Class3Events>3</Class3Events>\r\n          <Class3Time>5</Class3Time>\r\n          <DualEvents>true</DualEvents>\r\n          <ValueStatusOnly>false</ValueStatusOnly>\r\n        </EventsUnsolicited>\r\n        <SecureAuthentication Version=\"1\">\r\n          <AuthVersion>none</AuthVersion>\r\n          <AuthTimeout>2000</AuthTimeout>\r\n          <AggressiveMode>false</AggressiveMode>\r\n          <KeyWrapAlgorithm>AES128</KeyWrapAlgorithm>\r\n          <HMACAlgorithm>SHA256-16</HMACAlgorithm>\r\n          <KeyChangeInterval>900</KeyChangeInterval>\r\n          <KeyChangeASDU>1000</KeyChangeASDU>\r\n          <ErrorCount>2</ErrorCount>\r\n          <ThresholdValue>5</ThresholdValue>\r\n          <AuthUserName>Common</AuthUserName>\r\n          <AuthUserRole>OPERATOR</AuthUserRole>\r\n          <AuthUserUpdateKey />\r\n          <AuthConfirm>false</AuthConfirm>\r\n          <AuthRead>false</AuthRead>\r\n          <AuthImmediateFreeze>false</AuthImmediateFreeze>\r\n          <AuthImmediateFreezeNoAck>false</AuthImmediateFreezeNoAck>\r\n          <AuthImmediateFreezeAndClear>false</AuthImmediateFreezeAndClear>\r\n          <AuthImmediateFreezeAndClearNoAck>false</AuthImmediateFreezeAndClearNoAck>\r\n          <AuthAssignClass>false</AuthAssignClass>\r\n          <AuthDelayMeasurement>false</AuthDelayMeasurement>\r\n          <AuthOpenFile>false</AuthOpenFile>\r\n          <AuthCloseFile>false</AuthCloseFile>\r\n          <AuthDeleteFile>false</AuthDeleteFile>\r\n          <AuthGetFileInformation>false</AuthGetFileInformation>\r\n          <AuthAbortFile>false</AuthAbortFile>\r\n          <AuthResponses>false</AuthResponses>\r\n          <Statistics Version=\"1\" Count=\"18\">\r\n            <Statistic Index=\"0\" Version=\"1\">\r\n              <Unexpected_Messages>3</Unexpected_Messages>\r\n            </Statistic>\r\n            <Statistic Index=\"1\" Version=\"1\">\r\n              <Authorization_Failures>5</Authorization_Failures>\r\n            </Statistic>\r\n            <Statistic Index=\"2\" Version=\"1\">\r\n              <Authentication_Failures>5</Authentication_Failures>\r\n            </Statistic>\r\n            <Statistic Index=\"3\" Version=\"1\">\r\n              <Reply_Timeouts>3</Reply_Timeouts>\r\n            </Statistic>\r\n            <Statistic Index=\"4\" Version=\"1\">\r\n              <Rekeys_Due_to_Authentication_Failure>3</Rekeys_Due_to_Authentication_Failure>\r\n            </Statistic>\r\n            <Statistic Index=\"5\" Version=\"1\">\r\n              <Total_Messages_Sent>100</Total_Messages_Sent>\r\n            </Statistic>\r\n            <Statistic Index=\"6\" Version=\"1\">\r\n              <Total_Messages_Received>100</Total_Messages_Received>\r\n            </Statistic>\r\n            <Statistic Index=\"7\" Version=\"1\">\r\n              <Critical_Messages_Sent>100</Critical_Messages_Sent>\r\n            </Statistic>\r\n            <Statistic Index=\"8\" Version=\"1\">\r\n              <Critical_Messages_Received>100</Critical_Messages_Received>\r\n            </Statistic>\r\n            <Statistic Index=\"9\" Version=\"1\">\r\n              <Discarded_Messages>10</Discarded_Messages>\r\n            </Statistic>\r\n            <Statistic Index=\"10\" Version=\"1\">\r\n              <Error_Messages_Sent>2</Error_Messages_Sent>\r\n            </Statistic>\r\n            <Statistic Index=\"11\" Version=\"1\">\r\n              <Error_Messages_Rxed>10</Error_Messages_Rxed>\r\n            </Statistic>\r\n            <Statistic Index=\"12\" Version=\"1\">\r\n              <Successful_Authentications>100</Successful_Authentications>\r\n            </Statistic>\r\n            <Statistic Index=\"13\" Version=\"1\">\r\n              <Session_Key_Changes>10</Session_Key_Changes>\r\n            </Statistic>\r\n            <Statistic Index=\"14\" Version=\"1\">\r\n              <Failed_Session_Key_Changes>5</Failed_Session_Key_Changes>\r\n            </Statistic>\r\n            <Statistic Index=\"15\" Version=\"1\">\r\n              <Update_Key_Changes>1</Update_Key_Changes>\r\n            </Statistic>\r\n            <Statistic Index=\"16\" Version=\"1\">\r\n              <Failed_Update_Key_Changes>1</Failed_Update_Key_Changes>\r\n            </Statistic>\r\n            <Statistic Index=\"17\" Version=\"1\">\r\n              <Rekeys_Due_to_Restarts>3</Rekeys_Due_to_Restarts>\r\n            </Statistic>\r\n          </Statistics>\r\n        </SecureAuthentication>\r\n        <FileTransfer Version=\"1\">\r\n          <FileHandleTimeout>3000</FileHandleTimeout>\r\n          <AllowDelete>true</AllowDelete>\r\n          <ReceiveDirectory />\r\n          <TransmitDirectory />\r\n        </FileTransfer>\r\n      " +
                            "</Datalink>\r\n      <Database Version=\"2\">\r\n";
                    }

                    if (item.Channel == "Serial")
                    {
                        AboveDB = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<AccessDNP3_SG>\r\n  " +
                            "<Configuration Version=\"1\">\r\n    <Outstation Version=\"1\">\r\n      <PhysicalLayer Version=\"1\">\r\n        " +
                            "<ChannelType>" + item.Channel + "</ChannelType>\r\n        " +
                            "<ListeningPort>20000</ListeningPort>\r\n        <UdpBroadcastPort>20000</UdpBroadcastPort>\r\n        " +
                            "<NetworkCard>Ethernet</NetworkCard>\r\n        " +
                            "<SerialPort>" + item.SerialPort + "</SerialPort>\r\n        " +
                            "<SerialBaudrate>" + item.BaudRate + "</SerialBaudrate>\r\n        " +
                            "<SerialSettings>" + item.PortSetting + "</SerialSettings>\r\n      </PhysicalLayer>\r\n      " +
                            "<Datalink Version=\"1\">\r\n        " +
                            "<OutstationDNPAddress>" + item.OutstationAdd + "</OutstationDNPAddress>\r\n        " +
                            "<MasterDNPAddress>" + item.MasterAdd + "</MasterDNPAddress>\r\n        <LinkStatusRequestInterval>0</LinkStatusRequestInterval>\r\n        <DLConfirmations>false</DLConfirmations>\r\n        <DLRetries>0</DLRetries>\r\n        <DLTimeout>2000</DLTimeout>\r\n        " +
                            "<MasterIPAddress>192.168.1.1</MasterIPAddress>\r\n        <MasterDualEndpointPort>20000</MasterDualEndpointPort>\r\n        <EnableSelfAddress>false</EnableSelfAddress>\r\n        <ValidateMasterAddress>true</ValidateMasterAddress>\r\n        <UseSpecificMasterUdpPort>false</UseSpecificMasterUdpPort>\r\n        <MasterUdpPort>20000</MasterUdpPort>\r\n        <ApplicationLayer Version=\"1\">\r\n          <ALTimeout>3000</ALTimeout>\r\n          <ALConfirmation>false</ALConfirmation>\r\n          <TXSize>2048</TXSize>\r\n          <LocalTime>true</LocalTime>\r\n          <SyncInterval>60</SyncInterval>\r\n          <SBOTimeout>60</SBOTimeout>\r\n          <DeviceName>Outstation</DeviceName>\r\n          <DeviceID>0</DeviceID>\r\n          <ControlVariable Version=\"2\">\r\n            <Name/>\r\n            <Identification/>\r\n            <ControlType>1</ControlType>\r\n          </ControlVariable>\r\n          <DeviceLocation>1</DeviceLocation>\r\n          <FunctionsAllowed Version=\"1\">\r\n            <ColdWarmRestart>false</ColdWarmRestart>\r\n            <FileTransfer>false</FileTransfer>\r\n            <AssignClass>false</AssignClass>\r\n            <TimeSync>true</TimeSync>\r\n            <Controls>true</Controls>\r\n            <CounterFreeze>true</CounterFreeze>\r\n            <Broadcast>false</Broadcast>\r\n          </FunctionsAllowed>\r\n        </ApplicationLayer>\r\n        <EventsUnsolicited Version=\"1\">\r\n          <DeleteOldest>false</DeleteOldest>\r\n          <SortTimeStamp>false</SortTimeStamp>\r\n          <AllowUnsolicited>false</AllowUnsolicited>\r\n          <UnsolicitedConfirmTimeout>5</UnsolicitedConfirmTimeout>\r\n          <UnsolicitedConfirmRetries>3</UnsolicitedConfirmRetries>\r\n          <UnsolicitedConfirmDelay>15</UnsolicitedConfirmDelay>\r\n          <Class1Events>3</Class1Events>\r\n          <Class1Time>5</Class1Time>\r\n          <Class2Events>3</Class2Events>\r\n          <Class2Time>5</Class2Time>\r\n          <Class3Events>3</Class3Events>\r\n          <Class3Time>5</Class3Time>\r\n          <DualEvents>true</DualEvents>\r\n          <ValueStatusOnly>false</ValueStatusOnly>\r\n        </EventsUnsolicited>\r\n        <SecureAuthentication Version=\"1\">\r\n          <AuthVersion>none</AuthVersion>\r\n          <AuthTimeout>2000</AuthTimeout>\r\n          <AggressiveMode>false</AggressiveMode>\r\n          <KeyWrapAlgorithm>AES128</KeyWrapAlgorithm>\r\n          <HMACAlgorithm>SHA256-16</HMACAlgorithm>\r\n          <KeyChangeInterval>900</KeyChangeInterval>\r\n          <KeyChangeASDU>1000</KeyChangeASDU>\r\n          <ErrorCount>2</ErrorCount>\r\n          <ThresholdValue>5</ThresholdValue>\r\n          <AuthUserName>Common</AuthUserName>\r\n          <AuthUserRole>OPERATOR</AuthUserRole>\r\n          <AuthUserUpdateKey />\r\n          <AuthConfirm>false</AuthConfirm>\r\n          <AuthRead>false</AuthRead>\r\n          <AuthImmediateFreeze>false</AuthImmediateFreeze>\r\n          <AuthImmediateFreezeNoAck>false</AuthImmediateFreezeNoAck>\r\n          <AuthImmediateFreezeAndClear>false</AuthImmediateFreezeAndClear>\r\n          <AuthImmediateFreezeAndClearNoAck>false</AuthImmediateFreezeAndClearNoAck>\r\n          <AuthAssignClass>false</AuthAssignClass>\r\n          <AuthDelayMeasurement>false</AuthDelayMeasurement>\r\n          <AuthOpenFile>false</AuthOpenFile>\r\n          <AuthCloseFile>false</AuthCloseFile>\r\n          <AuthDeleteFile>false</AuthDeleteFile>\r\n          <AuthGetFileInformation>false</AuthGetFileInformation>\r\n          <AuthAbortFile>false</AuthAbortFile>\r\n          <AuthResponses>false</AuthResponses>\r\n          <Statistics Version=\"1\" Count=\"18\">\r\n            <Statistic Index=\"0\" Version=\"1\">\r\n              <Unexpected_Messages>3</Unexpected_Messages>\r\n            </Statistic>\r\n            <Statistic Index=\"1\" Version=\"1\">\r\n              <Authorization_Failures>5</Authorization_Failures>\r\n            </Statistic>\r\n            <Statistic Index=\"2\" Version=\"1\">\r\n              <Authentication_Failures>5</Authentication_Failures>\r\n            </Statistic>\r\n            <Statistic Index=\"3\" Version=\"1\">\r\n              <Reply_Timeouts>3</Reply_Timeouts>\r\n            </Statistic>\r\n            <Statistic Index=\"4\" Version=\"1\">\r\n              <Rekeys_Due_to_Authentication_Failure>3</Rekeys_Due_to_Authentication_Failure>\r\n            </Statistic>\r\n            <Statistic Index=\"5\" Version=\"1\">\r\n              <Total_Messages_Sent>100</Total_Messages_Sent>\r\n            </Statistic>\r\n            <Statistic Index=\"6\" Version=\"1\">\r\n              <Total_Messages_Received>100</Total_Messages_Received>\r\n            </Statistic>\r\n            <Statistic Index=\"7\" Version=\"1\">\r\n              <Critical_Messages_Sent>100</Critical_Messages_Sent>\r\n            </Statistic>\r\n            <Statistic Index=\"8\" Version=\"1\">\r\n              <Critical_Messages_Received>100</Critical_Messages_Received>\r\n            </Statistic>\r\n            <Statistic Index=\"9\" Version=\"1\">\r\n              <Discarded_Messages>10</Discarded_Messages>\r\n            </Statistic>\r\n            <Statistic Index=\"10\" Version=\"1\">\r\n              <Error_Messages_Sent>2</Error_Messages_Sent>\r\n            </Statistic>\r\n            <Statistic Index=\"11\" Version=\"1\">\r\n              <Error_Messages_Rxed>10</Error_Messages_Rxed>\r\n            </Statistic>\r\n            <Statistic Index=\"12\" Version=\"1\">\r\n              <Successful_Authentications>100</Successful_Authentications>\r\n            </Statistic>\r\n            <Statistic Index=\"13\" Version=\"1\">\r\n              <Session_Key_Changes>10</Session_Key_Changes>\r\n            </Statistic>\r\n            <Statistic Index=\"14\" Version=\"1\">\r\n              <Failed_Session_Key_Changes>5</Failed_Session_Key_Changes>\r\n            </Statistic>\r\n            <Statistic Index=\"15\" Version=\"1\">\r\n              <Update_Key_Changes>1</Update_Key_Changes>\r\n            </Statistic>\r\n            <Statistic Index=\"16\" Version=\"1\">\r\n              <Failed_Update_Key_Changes>1</Failed_Update_Key_Changes>\r\n            </Statistic>\r\n            <Statistic Index=\"17\" Version=\"1\">\r\n              <Rekeys_Due_to_Restarts>3</Rekeys_Due_to_Restarts>\r\n            </Statistic>\r\n          </Statistics>\r\n        </SecureAuthentication>\r\n        <FileTransfer Version=\"1\">\r\n          <FileHandleTimeout>3000</FileHandleTimeout>\r\n          <AllowDelete>true</AllowDelete>\r\n          <ReceiveDirectory />\r\n          <TransmitDirectory />\r\n        </FileTransfer>\r\n      " +
                            "</Datalink>\r\n      <Database Version=\"2\">\r\n";
                    }



                    //AboveDB = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<AccessDNP3_SG>\r\n " +
                    //    "<Configuration Version=\"1\">\r\n  " +
                    //    "<Outstation Version=\"1\">\r\n   " +
                    //    "<PhysicalLayer Version=\"1\">\r\n    " +
                    //    "<ChannelType>" + item.Channel + "</ChannelType>\r\n    " +
                    //    "<ListeningPort>" + item.TPCListeningPort + "</ListeningPort>\r\n    <UdpBroadcastPort>20000</UdpBroadcastPort>\r\n    " +
                    //    "<NetworkCard>" + item.NetworkCard + "<NetworkCard/>\r\n    " +
                    //    "<SerialPort>" + item.SerialPort + "</SerialPort>\r\n    " +
                    //    "<SerialBaudrate>" + item.BaudRate + "</SerialBaudrate>\r\n    " +
                    //    "<SerialSettings>" + item.PortSetting + "</SerialSettings>\r\n   </PhysicalLayer>\r\n   " +
                    //    "<Datalink Version=\"1\">\r\n    " +
                    //    "<OutstationDNPAddress>" + item.OutstationAdd + "</OutstationDNPAddress>\r\n    " +
                    //    "<MasterDNPAddress>" + item.MasterAdd + "</MasterDNPAddress>\r\n    <LinkStatusRequestInterval>0</LinkStatusRequestInterval>\r\n    <DLConfirmations>false</DLConfirmations>\r\n    <DLRetries>0</DLRetries>\r\n    <DLTimeout>2000</DLTimeout>\r\n    " +
                    //    "<MasterIPAddress>" + item.MasterIP + "</MasterIPAddress>\r\n    <MasterDualEndpointPort>20000</MasterDualEndpointPort>\r\n    <EnableSelfAddress>false</EnableSelfAddress>\r\n    <ValidateMasterAddress>true</ValidateMasterAddress>\r\n    <UseSpecificMasterUdpPort>false</UseSpecificMasterUdpPort>\r\n    <MasterUdpPort>20000</MasterUdpPort>\r\n    <ApplicationLayer Version=\"1\">\r\n     <ALTimeout>3000</ALTimeout>\r\n     <ALConfirmation>false</ALConfirmation>\r\n     <TXSize>2048</TXSize>\r\n     <LocalTime>true</LocalTime>\r\n     <SyncInterval>60</SyncInterval>\r\n     <SBOTimeout>60</SBOTimeout>\r\n     <DeviceName>Outstation</DeviceName>\r\n     <DeviceID>0</DeviceID>\r\n     <ControlVariable Version=\"2\">\r\n      <Name/>\r\n      <Identification/>\r\n      <ControlType>1</ControlType>\r\n     </ControlVariable>\r\n     <DeviceLocation>1</DeviceLocation>\r\n     <FunctionsAllowed Version=\"1\">\r\n      <ColdWarmRestart>false</ColdWarmRestart>\r\n      <FileTransfer>false</FileTransfer>\r\n      <AssignClass>false</AssignClass>\r\n      <TimeSync>false</TimeSync>\r\n      <Controls>true</Controls>\r\n      <CounterFreeze>true</CounterFreeze>\r\n      <Broadcast>false</Broadcast>\r\n     </FunctionsAllowed>\r\n    </ApplicationLayer>\r\n    <EventsUnsolicited Version=\"1\">\r\n     <DeleteOldest>false</DeleteOldest>\r\n     <SortTimeStamp>false</SortTimeStamp>\r\n     <AllowUnsolicited>false</AllowUnsolicited>\r\n     <UnsolicitedConfirmTimeout>5</UnsolicitedConfirmTimeout>\r\n     <UnsolicitedConfirmRetries>3</UnsolicitedConfirmRetries>\r\n     <UnsolicitedConfirmDelay>15</UnsolicitedConfirmDelay>\r\n     <Class1Events>3</Class1Events>\r\n     <Class1Time>5</Class1Time>\r\n     <Class2Events>3</Class2Events>\r\n     <Class2Time>5</Class2Time>\r\n     <Class3Events>3</Class3Events>\r\n     <Class3Time>5</Class3Time>\r\n     <DualEvents>true</DualEvents>\r\n     <ValueStatusOnly>false</ValueStatusOnly>\r\n    </EventsUnsolicited>\r\n    <SecureAuthentication Version=\"1\">\r\n     <AuthVersion>none</AuthVersion>\r\n     <AuthTimeout>2000</AuthTimeout>\r\n     <AggressiveMode>false</AggressiveMode>\r\n     <KeyWrapAlgorithm>AES128</KeyWrapAlgorithm>\r\n     <HMACAlgorithm>SHA256-16</HMACAlgorithm>\r\n     <KeyChangeInterval>900</KeyChangeInterval>\r\n     <KeyChangeASDU>1000</KeyChangeASDU>\r\n     <ErrorCount>2</ErrorCount>\r\n     <ThresholdValue>5</ThresholdValue>\r\n     <AuthUserName>Common</AuthUserName>\r\n     <AuthUserRole>OPERATOR</AuthUserRole>\r\n     <AuthUserUpdateKey/>\r\n     <AuthConfirm>false</AuthConfirm>\r\n     <AuthRead>false</AuthRead>\r\n     <AuthImmediateFreeze>false</AuthImmediateFreeze>\r\n     <AuthImmediateFreezeNoAck>false</AuthImmediateFreezeNoAck>\r\n     <AuthImmediateFreezeAndClear>false</AuthImmediateFreezeAndClear>\r\n     <AuthImmediateFreezeAndClearNoAck>false</AuthImmediateFreezeAndClearNoAck>\r\n     <AuthAssignClass>false</AuthAssignClass>\r\n     <AuthDelayMeasurement>false</AuthDelayMeasurement>\r\n     <AuthOpenFile>false</AuthOpenFile>\r\n     <AuthCloseFile>false</AuthCloseFile>\r\n     <AuthDeleteFile>false</AuthDeleteFile>\r\n     <AuthGetFileInformation>false</AuthGetFileInformation>\r\n     <AuthAbortFile>false</AuthAbortFile>\r\n     <AuthResponses>false</AuthResponses>\r\n     <Statistics Version=\"1\" Count=\"18\">\r\n      <Statistic Index=\"0\" Version=\"1\">\r\n       <Unexpected_Messages>3</Unexpected_Messages>\r\n      </Statistic>\r\n      <Statistic Index=\"1\" Version=\"1\">\r\n       <Authorization_Failures>5</Authorization_Failures>\r\n      </Statistic>\r\n      <Statistic Index=\"2\" Version=\"1\">\r\n       <Authentication_Failures>5</Authentication_Failures>\r\n      </Statistic>\r\n      <Statistic Index=\"3\" Version=\"1\">\r\n       <Reply_Timeouts>3</Reply_Timeouts>\r\n      </Statistic>\r\n      <Statistic Index=\"4\" Version=\"1\">\r\n       <Rekeys_Due_to_Authentication_Failure>3</Rekeys_Due_to_Authentication_Failure>\r\n      </Statistic>\r\n      <Statistic Index=\"5\" Version=\"1\">\r\n       <Total_Messages_Sent>100</Total_Messages_Sent>\r\n      </Statistic>\r\n      <Statistic Index=\"6\" Version=\"1\">\r\n       <Total_Messages_Received>100</Total_Messages_Received>\r\n      </Statistic>\r\n      <Statistic Index=\"7\" Version=\"1\">\r\n       <Critical_Messages_Sent>100</Critical_Messages_Sent>\r\n      </Statistic>\r\n      <Statistic Index=\"8\" Version=\"1\">\r\n       <Critical_Messages_Received>100</Critical_Messages_Received>\r\n      </Statistic>\r\n      <Statistic Index=\"9\" Version=\"1\">\r\n       <Discarded_Messages>10</Discarded_Messages>\r\n      </Statistic>\r\n      <Statistic Index=\"10\" Version=\"1\">\r\n       <Error_Messages_Sent>2</Error_Messages_Sent>\r\n      </Statistic>\r\n      <Statistic Index=\"11\" Version=\"1\">\r\n       <Error_Messages_Rxed>10</Error_Messages_Rxed>\r\n      </Statistic>\r\n      <Statistic Index=\"12\" Version=\"1\">\r\n       <Successful_Authentications>100</Successful_Authentications>\r\n      </Statistic>\r\n      <Statistic Index=\"13\" Version=\"1\">\r\n       <Session_Key_Changes>10</Session_Key_Changes>\r\n      </Statistic>\r\n      <Statistic Index=\"14\" Version=\"1\">\r\n       <Failed_Session_Key_Changes>5</Failed_Session_Key_Changes>\r\n      </Statistic>\r\n      <Statistic Index=\"15\" Version=\"1\">\r\n       <Update_Key_Changes>1</Update_Key_Changes>\r\n      </Statistic>\r\n      <Statistic Index=\"16\" Version=\"1\">\r\n       <Failed_Update_Key_Changes>1</Failed_Update_Key_Changes>\r\n      </Statistic>\r\n      <Statistic Index=\"17\" Version=\"1\">\r\n       <Rekeys_Due_to_Restarts>3</Rekeys_Due_to_Restarts>\r\n      </Statistic>\r\n     </Statistics>\r\n    </SecureAuthentication>\r\n    <FileTransfer Version=\"1\">\r\n     <FileHandleTimeout>3000</FileHandleTimeout>\r\n     <AllowDelete>true</AllowDelete>\r\n     <ReceiveDirectory/>\r\n     <TransmitDirectory/>\r\n    </FileTransfer>\r\n   </Datalink>\r\n" +
                    //    "   <Database Version=\"1\">\r\n";

                    //AboveDB = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<AccessDNP3_SG>\r\n  " +
                    //    "<Configuration Version=\"1\">\r\n    <Outstation Version=\"1\">\r\n      <PhysicalLayer Version=\"1\">\r\n        " +
                    //    "<ChannelType>" + ChannelType + "</ChannelType>\r\n        " +
                    //    "<ListeningPort>" + TCPListening + "</ListeningPort>\r\n        <UdpBroadcastPort>20000</UdpBroadcastPort>\r\n        " +
                    //    "<NetworkCard>" + NetworkCard + "</NetworkCard>\r\n        " +
                    //    "<SerialPort>" + SerialPort + "</SerialPort>\r\n        " +
                    //    "<SerialBaudrate>" + BaudRate + "</SerialBaudrate>\r\n        " +
                    //    "<SerialSettings>" + PortSetting + "</SerialSettings>\r\n      </PhysicalLayer>\r\n      " +
                    //    "<Datalink Version=\"1\">\r\n        " +
                    //    "<OutstationDNPAddress>" + OutAddress + "</OutstationDNPAddress>\r\n        " +
                    //    "<MasterDNPAddress>" + MasAddress + "</MasterDNPAddress>\r\n        <LinkStatusRequestInterval>0</LinkStatusRequestInterval>\r\n        <DLConfirmations>false</DLConfirmations>\r\n        <DLRetries>0</DLRetries>\r\n        <DLTimeout>2000</DLTimeout>\r\n        " +
                    //    "<MasterIPAddress>" + MasterIP + "</MasterIPAddress>\r\n        <MasterDualEndpointPort>20000</MasterDualEndpointPort>\r\n        <EnableSelfAddress>false</EnableSelfAddress>\r\n        <ValidateMasterAddress>true</ValidateMasterAddress>\r\n        <UseSpecificMasterUdpPort>false</UseSpecificMasterUdpPort>\r\n        <MasterUdpPort>20000</MasterUdpPort>\r\n        <ApplicationLayer Version=\"1\">\r\n          <ALTimeout>3000</ALTimeout>\r\n          <ALConfirmation>false</ALConfirmation>\r\n          <TXSize>2048</TXSize>\r\n          <LocalTime>true</LocalTime>\r\n          <SyncInterval>60</SyncInterval>\r\n          <SBOTimeout>60</SBOTimeout>\r\n          <DeviceName>Outstation</DeviceName>\r\n          <DeviceID>0</DeviceID>\r\n          <ControlVariable Version=\"2\">\r\n            <Name/>\r\n            <Identification/>\r\n            <ControlType>1</ControlType>\r\n          </ControlVariable>\r\n          <DeviceLocation>1</DeviceLocation>\r\n          <FunctionsAllowed Version=\"1\">\r\n            <ColdWarmRestart>false</ColdWarmRestart>\r\n            <FileTransfer>false</FileTransfer>\r\n            <AssignClass>false</AssignClass>\r\n            <TimeSync>true</TimeSync>\r\n            <Controls>true</Controls>\r\n            <CounterFreeze>true</CounterFreeze>\r\n            <Broadcast>false</Broadcast>\r\n          </FunctionsAllowed>\r\n        </ApplicationLayer>\r\n        <EventsUnsolicited Version=\"1\">\r\n          <DeleteOldest>false</DeleteOldest>\r\n          <SortTimeStamp>false</SortTimeStamp>\r\n          <AllowUnsolicited>true</AllowUnsolicited>\r\n          <UnsolicitedConfirmTimeout>5</UnsolicitedConfirmTimeout>\r\n          <UnsolicitedConfirmRetries>3</UnsolicitedConfirmRetries>\r\n          <UnsolicitedConfirmDelay>15</UnsolicitedConfirmDelay>\r\n          <Class1Events>3</Class1Events>\r\n          <Class1Time>5</Class1Time>\r\n          <Class2Events>3</Class2Events>\r\n          <Class2Time>5</Class2Time>\r\n          <Class3Events>3</Class3Events>\r\n          <Class3Time>5</Class3Time>\r\n          <DualEvents>true</DualEvents>\r\n          <ValueStatusOnly>false</ValueStatusOnly>\r\n        </EventsUnsolicited>\r\n        <SecureAuthentication Version=\"1\">\r\n          <AuthVersion>none</AuthVersion>\r\n          <AuthTimeout>2000</AuthTimeout>\r\n          <AggressiveMode>false</AggressiveMode>\r\n          <KeyWrapAlgorithm>AES128</KeyWrapAlgorithm>\r\n          <HMACAlgorithm>SHA256-16</HMACAlgorithm>\r\n          <KeyChangeInterval>900</KeyChangeInterval>\r\n          <KeyChangeASDU>1000</KeyChangeASDU>\r\n          <ErrorCount>2</ErrorCount>\r\n          <ThresholdValue>5</ThresholdValue>\r\n          <AuthUserName>Common</AuthUserName>\r\n          <AuthUserRole>OPERATOR</AuthUserRole>\r\n          <AuthUserUpdateKey />\r\n          <AuthConfirm>false</AuthConfirm>\r\n          <AuthRead>false</AuthRead>\r\n          <AuthImmediateFreeze>false</AuthImmediateFreeze>\r\n          <AuthImmediateFreezeNoAck>false</AuthImmediateFreezeNoAck>\r\n          <AuthImmediateFreezeAndClear>false</AuthImmediateFreezeAndClear>\r\n          <AuthImmediateFreezeAndClearNoAck>false</AuthImmediateFreezeAndClearNoAck>\r\n          <AuthAssignClass>false</AuthAssignClass>\r\n          <AuthDelayMeasurement>false</AuthDelayMeasurement>\r\n          <AuthOpenFile>false</AuthOpenFile>\r\n          <AuthCloseFile>false</AuthCloseFile>\r\n          <AuthDeleteFile>false</AuthDeleteFile>\r\n          <AuthGetFileInformation>false</AuthGetFileInformation>\r\n          <AuthAbortFile>false</AuthAbortFile>\r\n          <AuthResponses>false</AuthResponses>\r\n          <Statistics Version=\"1\" Count=\"18\">\r\n            <Statistic Index=\"0\" Version=\"1\">\r\n              <Unexpected_Messages>3</Unexpected_Messages>\r\n            </Statistic>\r\n            <Statistic Index=\"1\" Version=\"1\">\r\n              <Authorization_Failures>5</Authorization_Failures>\r\n            </Statistic>\r\n            <Statistic Index=\"2\" Version=\"1\">\r\n              <Authentication_Failures>5</Authentication_Failures>\r\n            </Statistic>\r\n            <Statistic Index=\"3\" Version=\"1\">\r\n              <Reply_Timeouts>3</Reply_Timeouts>\r\n            </Statistic>\r\n            <Statistic Index=\"4\" Version=\"1\">\r\n              <Rekeys_Due_to_Authentication_Failure>3</Rekeys_Due_to_Authentication_Failure>\r\n            </Statistic>\r\n            <Statistic Index=\"5\" Version=\"1\">\r\n              <Total_Messages_Sent>100</Total_Messages_Sent>\r\n            </Statistic>\r\n            <Statistic Index=\"6\" Version=\"1\">\r\n              <Total_Messages_Received>100</Total_Messages_Received>\r\n            </Statistic>\r\n            <Statistic Index=\"7\" Version=\"1\">\r\n              <Critical_Messages_Sent>100</Critical_Messages_Sent>\r\n            </Statistic>\r\n            <Statistic Index=\"8\" Version=\"1\">\r\n              <Critical_Messages_Received>100</Critical_Messages_Received>\r\n            </Statistic>\r\n            <Statistic Index=\"9\" Version=\"1\">\r\n              <Discarded_Messages>10</Discarded_Messages>\r\n            </Statistic>\r\n            <Statistic Index=\"10\" Version=\"1\">\r\n              <Error_Messages_Sent>2</Error_Messages_Sent>\r\n            </Statistic>\r\n            <Statistic Index=\"11\" Version=\"1\">\r\n              <Error_Messages_Rxed>10</Error_Messages_Rxed>\r\n            </Statistic>\r\n            <Statistic Index=\"12\" Version=\"1\">\r\n              <Successful_Authentications>100</Successful_Authentications>\r\n            </Statistic>\r\n            <Statistic Index=\"13\" Version=\"1\">\r\n              <Session_Key_Changes>10</Session_Key_Changes>\r\n            </Statistic>\r\n            <Statistic Index=\"14\" Version=\"1\">\r\n              <Failed_Session_Key_Changes>5</Failed_Session_Key_Changes>\r\n            </Statistic>\r\n            <Statistic Index=\"15\" Version=\"1\">\r\n              <Update_Key_Changes>1</Update_Key_Changes>\r\n            </Statistic>\r\n            <Statistic Index=\"16\" Version=\"1\">\r\n              <Failed_Update_Key_Changes>1</Failed_Update_Key_Changes>\r\n            </Statistic>\r\n            <Statistic Index=\"17\" Version=\"1\">\r\n              <Rekeys_Due_to_Restarts>3</Rekeys_Due_to_Restarts>\r\n            </Statistic>\r\n          </Statistics>\r\n        </SecureAuthentication>\r\n        <FileTransfer Version=\"1\">\r\n          <FileHandleTimeout>3000</FileHandleTimeout>\r\n          <AllowDelete>true</AllowDelete>\r\n          <ReceiveDirectory />\r\n          <TransmitDirectory />\r\n        </FileTransfer>\r\n      " +
                    //    "</Datalink>\r\n      <Database Version=\"2\">\r\n";

                }
            }
            return AboveDB;
        }

        public string BelowDataBase()
        {
            string BelowDB = "      </Database>\r\n      " +
                "<RedundancyRemoveEvents>false</RedundancyRemoveEvents>\r\n      " +
                "<SilentOnStandby>false</SilentOnStandby>\r\n      " +
                "<RedundancyPurgeEventBuffer>false</RedundancyPurgeEventBuffer>\r\n    " +
                "</Outstation>\r\n  </Configuration>\r\n</AccessDNP3_SG>\r\n";

            //string BelowDB = "      </Database>\r\n      " +
            //    "<RedundancyRemoveEvents>false</RedundancyRemoveEvents>\r\n      " +
            //    "<SilentOnStandby>false</SilentOnStandby>\r\n      " +
            //    "<RedundancyPurgeEventBuffer>false</RedundancyPurgeEventBuffer>\r\n    " +
            //    "</Outstation>\r\n  </Configuration>\r\n</AccessDNP3_SG>\r\n";
            return BelowDB;
        }

        public string BinaryInputsA(int BI_count, string[] DefaultVariation)
        {
            string BI_title = "        <BinaryInputs Version=\"1\">\r\n";
            string BI_options = "          <Options Version=\"1\">\r\n            " +
                "<EventClass>one</EventClass>\r\n            " +
                "<RetainEvents>all events</RetainEvents>\r\n            " +
                "<MaxEvents>100</MaxEvents>\r\n            " +
                "<StaticVariation>" + DefaultVariation[0] + "</StaticVariation>\r\n            " +
                "<EventVariation>" + DefaultVariation[4] + "</EventVariation>\r\n          </Options>\r\n";
            string BI_pointCnt = "          <Points Version=\"1\" Count=\"" + BI_count.ToString() + "\">\r\n";

            return BI_title + BI_options + BI_pointCnt;
        }

        public string BinaryInputsB() { return "          </Points>\r\n        </BinaryInputs>\r\n"; }

        public string BinaryInputs_point(int DNPindex, string ProjvarName, string pointDes, string staticVariation, string eventVariation, bool invert, bool shift)
        {
            string point_str = "            <Point Index=\"" + DNPindex + "\" Version=\"2\">\r\n";
            string name_str = "              <Name>" + ProjvarName + "</Name>\r\n";
            string ident_str = "              <Identification>" + pointDes + "</Identification>\r\n";
            if (shift) { DNPindex = DNPindex + 1; }
            string pointID_str = "              <PointIndex>" + DNPindex + "</PointIndex>\r\n";
            string eventC_str = "              <EventClass>default</EventClass>\r\n";
            string staticV_str = "              <StaticVariation>" + staticVariation + "</StaticVariation>\r\n";
            string eventV_str = "              <EventVariation>" + eventVariation + "</EventVariation>\r\n";
            string invert_str = "              <InvertBIvalue>" + invert + "</InvertBIvalue>\r\n            </Point>\r\n";

            return point_str + name_str + ident_str + pointID_str + eventC_str + staticV_str + eventV_str + invert_str;
        }

        public string DoubleBitDI()
        {
            string DBI = "        <DoubleBitBinaryInputs Version=\"1\">\r\n          " +
                "<Options Version=\"1\">\r\n            <EventClass>one</EventClass>\r\n            " +
                "<RetainEvents>all events</RetainEvents>\r\n            " +
                "<MaxEvents>100</MaxEvents>\r\n            <StaticVariation>packed format</StaticVariation>\r\n            " +
                "<EventVariation>with absolute time</EventVariation>\r\n          " +
                "</Options>\r\n          <Points Version=\"1\" Count=\"0\">\r\n          " +
                "</Points>\r\n        </DoubleBitBinaryInputs>\r\n";

            return DBI;
        }

        public string RunningCountersA(int RC_count, string[] DefaultVariation)
        {
            string RC_start = "        <RunningCounters Version=\"1\">\r\n          <CounterOptions Version=\"1\">\r\n            " +
                "<EventClass>zero</EventClass>\r\n            <RetainEvents>most recent</RetainEvents>\r\n            " +
                "<MaxEvents>100</MaxEvents>\r\n            " +
                "<StaticVariation>" + DefaultVariation[3] + "</StaticVariation>\r\n            " +
                "<EventVariation>" + DefaultVariation[7] + "</EventVariation>\r\n            " +
                "<FrozenOptions Version=\"1\">\r\n              " +
                "<EventClass>none</EventClass>\r\n              <RetainEvents>most recent</RetainEvents>\r\n              " +
                "<MaxEvents>100</MaxEvents>\r\n              " +
                "<StaticVariation>" + DefaultVariation[3] + "</StaticVariation>\r\n              " +
                "<EventVariation>" + DefaultVariation[7] + "</EventVariation>\r\n            " +
                "</FrozenOptions>\r\n          </CounterOptions>\r\n";

            //string RC_start = "    <RunningCounters Version=\"1\">\r\n     <CounterOptions Version=\"1\">\r\n      " +
            //    "<EventClass>three</EventClass>\r\n      <RetainEvents>most recent</RetainEvents>\r\n      " +
            //    "<MaxEvents>100</MaxEvents>\r\n      " +
            //    "<StaticVariation>" + DefaultVariation[3] + "</StaticVariation>\r\n      " +
            //    "<EventVariation>" + DefaultVariation[7] + "</EventVariation>\r\n      " +
            //    "<FrozenOptions Version=\"1\">\r\n       " +
            //    "<EventClass>three</EventClass>\r\n       <RetainEvents>most recent</RetainEvents>\r\n       " +
            //    "<MaxEvents>100</MaxEvents>\r\n       " +
            //    "<StaticVariation>" + DefaultVariation[3] + "</StaticVariation>\r\n       " +
            //    "<EventVariation>" + DefaultVariation[7] + "</EventVariation>\r\n      " +
            //    "</FrozenOptions>\r\n     </CounterOptions>\r\n";
            string RC_pointCnt = "          <Points Version=\"1\" Count=\"" + RC_count + "\">\r\n";

            return RC_start + RC_pointCnt;
        }

        public string RunningCountersB() { return "          </Points>\r\n        </RunningCounters>\r\n"; }

        public string RunningCounters_point(int DNPindex, string ProjvarName, string pointDes, string staticVariation, string eventVariation, bool shift)
        {
            string point_str = "            <CounterPoint Index=\"" + DNPindex + "\" Version=\"2\">\r\n";
            string name_str = "              <Name>" + ProjvarName + "</Name>\r\n";
            string ident_str = "              <Identification>" + pointDes + "</Identification>\r\n";
            if (shift) { DNPindex = DNPindex + 1; }
            string pointID_str = "              <PointIndex>" + DNPindex + "</PointIndex>\r\n";
            string eventC_str = "              <EventClass>default</EventClass>\r\n";
            string staticV_str = "              <StaticVariation>" + staticVariation + "</StaticVariation>\r\n";
            string eventV_str = "              <EventVariation>" + eventVariation + "</EventVariation>\r\n";
            string point2_strA = "              <FrozenPoint Version=\"2\">\r\n                <Name />\r\n                <Identification>" + pointDes + "</Identification>\r\n";
            string point2_strB = "                <PointIndex>" + DNPindex + "</PointIndex>\r\n";
            string end_str = "                <EventClass>default</EventClass>\r\n                <StaticVariation>default</StaticVariation>\r\n                " +
                "<EventVariation>default</EventVariation>\r\n              </FrozenPoint>\r\n            </CounterPoint>\r\n";

            return point_str + name_str + ident_str + pointID_str + eventC_str + staticV_str + eventV_str  + point2_strA + point2_strB + end_str;
        }

        public string AnalogInputsA(int AI_count, string[] DefaultVariation)
        {
            string AI_start = "        <AnalogInputs Version=\"1\">\r\n          <Options Version=\"1\">\r\n            " +
                "<EventClass>two</EventClass>\r\n            <RetainEvents>all events</RetainEvents>\r\n            " +
                "<MaxEvents>100</MaxEvents>\r\n            " +
                "<StaticVariation>" + DefaultVariation[2] + "</StaticVariation>\r\n            " +
                "<EventVariation>" + DefaultVariation[6] + "</EventVariation>\r\n          </Options>\r\n";

            //string AI_start = "    <AnalogInputs Version=\"1\">\r\n     <Options Version=\"1\">\r\n      " +
            //    "<EventClass>three</EventClass>\r\n      <RetainEvents>most recent</RetainEvents>\r\n      " +
            //    "<MaxEvents>100</MaxEvents>\r\n      " +
            //    "<StaticVariation>" + DefaultVariation[2] + "</StaticVariation>\r\n      " +
            //    "<EventVariation>" + DefaultVariation[6] + "</EventVariation>\r\n     " +
            //    "</Options>\r\n";
            string AT_pointCnt = "          <Points Version=\"1\" Count=\"" + AI_count + "\">\r\n";

            return AI_start + AT_pointCnt;
        }

        public string AnalogInputsB() { return "          </Points>\r\n        </AnalogInputs>\r\n"; }

        public string AnalogInputs_point(int DNPindex, string ProjvarName, string pointDes, string staticVariation, string eventVariation, float scalingF, bool shift)
        {
            string point_str = "            <Point Index=\"" + DNPindex + "\" Version=\"2\">\r\n";
            string name_str = "              <Name>" + ProjvarName + "</Name>\r\n";
            string ident_str = "              <Identification>" + pointDes + "</Identification>\r\n";
            if (shift) { DNPindex = DNPindex + 1; }
            string pointID_str = "              <PointIndex>" + DNPindex + "</PointIndex>\r\n";
            string eventC_str = "              <EventClass>default</EventClass>\r\n";
            string staticV_str = "              <StaticVariation>" + staticVariation + "</StaticVariation>\r\n";
            string eventV_str = "              <EventVariation>" + eventVariation + "</EventVariation>\r\n";
            string scaling_str = "              <AI_ScalingFactor>" + scalingF + "</AI_ScalingFactor>\r\n            </Point>\r\n";

            return point_str + name_str + ident_str + pointID_str + eventC_str + staticV_str + eventV_str + scaling_str;
        }

        public string BinaryOutputsA(int BO_count, string[] DefaultVariation)
        {
            string BO_start = "        <BinaryOutputs Version=\"1\">\r\n          <Options Version=\"1\">\r\n            " +
                "<EventClass>none</EventClass>\r\n            <RetainEvents>all events</RetainEvents>\r\n            " +
                "<MaxEvents>100</MaxEvents>\r\n            " +
                "<StaticVariation>" + DefaultVariation[1] + "</StaticVariation>\r\n            " +
                "<EventVariation>" + DefaultVariation[5] + "</EventVariation>\r\n            " +
                "<ControlModel>complementary latch</ControlModel>\r\n            " +
                "<GenerateBinOutEvents>false</GenerateBinOutEvents>\r\n            <DefPulseTime>1000</DefPulseTime>\r\n          </Options>\r\n";

            //string BO_start = "    <BinaryOutputs Version=\"1\">\r\n     <Options Version=\"1\">\r\n      " +
            //    "<EventClass>two</EventClass>\r\n      <RetainEvents>all events</RetainEvents>\r\n      " +
            //    "<MaxEvents>100</MaxEvents>\r\n      " +
            //    "<StaticVariation>" + DefaultVariation[1] + "</StaticVariation>\r\n      " +
            //    "<EventVariation>" + DefaultVariation[5] + "</EventVariation>\r\n      " +
            //    "<ControlModel>complementary latch</ControlModel>\r\n      " +
            //    "<GenerateBinOutEvents>true</GenerateBinOutEvents>\r\n      <DefPulseTime>1000</DefPulseTime>\r\n     </Options>\r\n";
            string BO_pointCnt = "          <Points Version=\"1\" Count=\"" + BO_count + "\">\r\n";

            return BO_start + BO_pointCnt;
        }

        public string BinaryOutputsB() { return "          </Points>\r\n        </BinaryOutputs>\r\n"; }

        public string BinaryOutputs_point(int DNPindex, string ProjvarName, string pointDes, string staticVariation, string eventVariation, bool shift)
        {
            string point_str = "            <Point Index=\"" + DNPindex + "\" Version=\"2\">\r\n";
            string name_str = "              <Name>" + ProjvarName + "</Name>\r\n";
            string ident_str = "              <Identification>" + pointDes + "</Identification>\r\n";
            if (shift) { DNPindex = DNPindex + 1; }
            string pointID_str = "              <PointIndex>" + DNPindex + "</PointIndex>\r\n";
            string eventC_str = "              <EventClass>default</EventClass>\r\n";
            string staticV_str = "              <StaticVariation>" + staticVariation + "</StaticVariation>\r\n";
            string eventV_str = "              <EventVariation>" + eventVariation + "</EventVariation>\r\n";
            string end_str = "              <CommandRouting>disabled</CommandRouting>\r\n              <ControlModel>default</ControlModel>\r\n            </Point>\r\n";

            return point_str + name_str + ident_str + pointID_str + eventC_str + staticV_str + eventV_str + end_str;
        }

        public string RemainingDB()
        {
            string Remaining = "        <AnalogOutputs Version=\"1\">\r\n          <Options Version=\"1\">\r\n            " +
                "<EventClass>three</EventClass>\r\n            <RetainEvents>most recent</RetainEvents>\r\n            " +
                "<MaxEvents>100</MaxEvents>\r\n            <StaticVariation>32 Bit with flag</StaticVariation>\r\n            " +
                "<EventVariation>32 Bit with time</EventVariation>\r\n            <EventGeneration>true</EventGeneration>\r\n          " +
                "</Options>\r\n          <Points Version=\"1\" Count=\"0\">\r\n          </Points>\r\n        " +
                "</AnalogOutputs>\r\n        <OctetStrings Version=\"1\">\r\n          <Options Version=\"1\">\r\n            " +
                "<EventClass>none</EventClass>\r\n            <RetainEvents>most recent</RetainEvents>\r\n            " +
                "<MaxEvents>10</MaxEvents>\r\n          </Options>\r\n          <Points Version=\"1\" Count=\"0\">\r\n          " +
                "</Points>\r\n        </OctetStrings>\r\n";
            return Remaining;
        }

        public string INIcontent()
        {
            string INIfile = "[GENERAL]\r\nDLL=AccessDNP3_SG.dll\r\n[DNP3_SG]\r\nTLS_ACTIVE=0\r\n" +
                "TLS_MIN_VERSION=0\r\nTLS_MAX_VERSION=12\r\nTLS_CERTIFICATE_STORE_PATH=\r\n" +
                "TLS_CERTIFICATE_FILE=\r\nTLS_CIPHER_LIST=\r\nTLS_CIPHER_SUITES=\r\n" +
                "TLS_PEER_CERTIFICATE_SUBJECT=\r\nTLS_RENEGOTIATION_TIMEOUT=86400\r\n" +
                "TLS_RENEGOTIATION_MAX_BYTES=10485760\r\nTLS_RESUMPTION_TIMEOUT=43200\r\n" +
                "TLS_RESUMPTION_MAX_BYTES=1048576\r\nTLS_CRL_CHECK_INTERVAL=21600\r\n";
            return INIfile;
        }


    }
}
