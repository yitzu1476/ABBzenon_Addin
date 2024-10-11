using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace Gateway_SerialMonitor
{
    /// <summary>
    /// Description of Project Service Extension.
    /// </summary>
    [AddInExtension("Gateway Serial Port Monitor", "Add-in Service for gateway monitoring")]
    public class ProjectServiceExtension : IProjectServiceExtension
    {
        #region IProjectServiceExtension implementation
        //SerialPort mySerialPort = new SerialPort("COM12");
        SerialPort mySerialPort_1;
        SerialPort mySerialPort_2;
        SerialPort mySerialPort_3;
        SerialPort mySerialPort_4;
        SerialPort mySerialPort_5;
        SerialPort mySerialPort_6;
        SerialPort mySerialPort_7;
        SerialPort mySerialPort_8;
        SerialPort mySerialPort_9;
        SerialPort mySerialPort_10;
        int Port_cnt = 0;


        IProject thisProject;
        //IVariable Serial_test;
        //IVariable Serial_status_v;

        IOnlineVariableContainer onlineContainer;
        List<Station_Content> stations = new List<Station_Content>();

        public void Start(IProject context, IBehavior behavior)
        {
            thisProject = context;

            // Collect Station info
            var csvF = new StreamReader("C:\\ProgramData\\ABB\\System\\SerialPort_Setting.csv");

            int cnt = 0;
            while (!csvF.EndOfStream)
            {
                var line = csvF.ReadLine();
                string[] Station_content = line.Split(',');

                cnt = cnt + 1;
                if (cnt == 1) { continue; }

                StopBits temp_stopBits = StopBits.One;
                if (Station_content[6] == "1") { temp_stopBits = StopBits.One; }
                if (Station_content[6] == "2") { temp_stopBits = StopBits.Two; }

                Parity temp_parity = Parity.None;
                if (Station_content[7] == "N") { temp_parity = Parity.None; }
                if (Station_content[7] == "E") { temp_parity = Parity.Even; }

                stations.Add(new Station_Content
                {
                    StationNum = cnt - 1,
                    StationName = Station_content[0],
                    serialPort = Station_content[1],
                    MonitorPort = Station_content[2],
                    BaudRate = int.Parse(Station_content[3]),
                    DataBits = int.Parse(Station_content[4]),
                    stopBits = temp_stopBits,
                    parity = temp_parity,
                });
            }

            // Create SerialPort
            Port_cnt = stations.Count;
            if (Port_cnt > 0)
            {
                foreach (var thisStation in stations)
                {
                    if (thisStation.StationNum == 1)
                    {
                        mySerialPort_1 = new SerialPort(thisStation.MonitorPort);

                        mySerialPort_1.BaudRate = thisStation.BaudRate;
                        mySerialPort_1.Parity = thisStation.parity;
                        mySerialPort_1.StopBits = thisStation.stopBits;
                        mySerialPort_1.DataBits = thisStation.DataBits;
                        mySerialPort_1.Handshake = Handshake.None;

                        mySerialPort_1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                        mySerialPort_1.Open();
                    }
                }

                if (Port_cnt > 1)
                {
                    foreach (var thisStation in stations)
                    {
                        if (thisStation.StationNum == 2)
                        {
                            mySerialPort_2 = new SerialPort(thisStation.MonitorPort);

                            mySerialPort_2.BaudRate = thisStation.BaudRate;
                            mySerialPort_2.Parity = thisStation.parity;
                            mySerialPort_2.StopBits = thisStation.stopBits;
                            mySerialPort_2.DataBits = thisStation.DataBits;
                            mySerialPort_2.Handshake = Handshake.None;

                            mySerialPort_2.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                            mySerialPort_2.Open();
                        }
                    }

                    if (Port_cnt > 2)
                    {
                        foreach (var thisStation in stations)
                        {
                            if (thisStation.StationNum == 3)
                            {
                                mySerialPort_3 = new SerialPort(thisStation.MonitorPort);

                                mySerialPort_3.BaudRate = thisStation.BaudRate;
                                mySerialPort_3.Parity = thisStation.parity;
                                mySerialPort_3.StopBits = thisStation.stopBits;
                                mySerialPort_3.DataBits = thisStation.DataBits;
                                mySerialPort_3.Handshake = Handshake.None;

                                mySerialPort_3.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                                mySerialPort_3.Open();
                            }
                        }

                        if (Port_cnt > 3)
                        {
                            foreach (var thisStation in stations)
                            {
                                if (thisStation.StationNum == 4)
                                {
                                    mySerialPort_4 = new SerialPort(thisStation.MonitorPort);

                                    mySerialPort_4.BaudRate = thisStation.BaudRate;
                                    mySerialPort_4.Parity = thisStation.parity;
                                    mySerialPort_4.StopBits = thisStation.stopBits;
                                    mySerialPort_4.DataBits = thisStation.DataBits;
                                    mySerialPort_4.Handshake = Handshake.None;

                                    mySerialPort_4.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                                    mySerialPort_4.Open();
                                }
                            }

                            if (Port_cnt > 4)
                            {
                                foreach (var thisStation in stations)
                                {
                                    if (thisStation.StationNum == 5)
                                    {
                                        mySerialPort_5 = new SerialPort(thisStation.MonitorPort);

                                        mySerialPort_5.BaudRate = thisStation.BaudRate;
                                        mySerialPort_5.Parity = thisStation.parity;
                                        mySerialPort_5.StopBits = thisStation.stopBits;
                                        mySerialPort_5.DataBits = thisStation.DataBits;
                                        mySerialPort_5.Handshake = Handshake.None;

                                        mySerialPort_5.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                                        mySerialPort_5.Open();
                                    }
                                }

                                if (Port_cnt > 5)
                                {
                                    foreach (var thisStation in stations)
                                    {
                                        if (thisStation.StationNum == 6)
                                        {
                                            mySerialPort_6 = new SerialPort(thisStation.MonitorPort);

                                            mySerialPort_6.BaudRate = thisStation.BaudRate;
                                            mySerialPort_6.Parity = thisStation.parity;
                                            mySerialPort_6.StopBits = thisStation.stopBits;
                                            mySerialPort_6.DataBits = thisStation.DataBits;
                                            mySerialPort_6.Handshake = Handshake.None;

                                            mySerialPort_6.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                                            mySerialPort_6.Open();
                                        }
                                    }

                                    if (Port_cnt > 6)
                                    {
                                        foreach (var thisStation in stations)
                                        {
                                            if (thisStation.StationNum == 7)
                                            {
                                                mySerialPort_7 = new SerialPort(thisStation.MonitorPort);

                                                mySerialPort_7.BaudRate = thisStation.BaudRate;
                                                mySerialPort_7.Parity = thisStation.parity;
                                                mySerialPort_7.StopBits = thisStation.stopBits;
                                                mySerialPort_7.DataBits = thisStation.DataBits;
                                                mySerialPort_7.Handshake = Handshake.None;

                                                mySerialPort_7.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                                                mySerialPort_7.Open();
                                            }
                                        }

                                        if (Port_cnt > 7)
                                        {
                                            foreach (var thisStation in stations)
                                            {
                                                if (thisStation.StationNum == 8)
                                                {
                                                    mySerialPort_8 = new SerialPort(thisStation.MonitorPort);

                                                    mySerialPort_8.BaudRate = thisStation.BaudRate;
                                                    mySerialPort_8.Parity = thisStation.parity;
                                                    mySerialPort_8.StopBits = thisStation.stopBits;
                                                    mySerialPort_8.DataBits = thisStation.DataBits;
                                                    mySerialPort_8.Handshake = Handshake.None;

                                                    mySerialPort_8.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                                                    mySerialPort_8.Open();
                                                }
                                            }

                                            if (Port_cnt > 8)
                                            {
                                                foreach (var thisStation in stations)
                                                {
                                                    if (thisStation.StationNum == 9)
                                                    {
                                                        mySerialPort_9 = new SerialPort(thisStation.MonitorPort);

                                                        mySerialPort_9.BaudRate = thisStation.BaudRate;
                                                        mySerialPort_9.Parity = thisStation.parity;
                                                        mySerialPort_9.StopBits = thisStation.stopBits;
                                                        mySerialPort_9.DataBits = thisStation.DataBits;
                                                        mySerialPort_9.Handshake = Handshake.None;

                                                        mySerialPort_9.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                                                        mySerialPort_9.Open();
                                                    }
                                                }

                                                if (Port_cnt > 9)
                                                {
                                                    foreach (var thisStation in stations)
                                                    {
                                                        if (thisStation.StationNum == 10)
                                                        {
                                                            mySerialPort_10 = new SerialPort(thisStation.MonitorPort);

                                                            mySerialPort_10.BaudRate = thisStation.BaudRate;
                                                            mySerialPort_10.Parity = thisStation.parity;
                                                            mySerialPort_10.StopBits = thisStation.stopBits;
                                                            mySerialPort_10.DataBits = thisStation.DataBits;
                                                            mySerialPort_10.Handshake = Handshake.None;

                                                            mySerialPort_10.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                                                            mySerialPort_10.Open();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            // GW_Timer
            onlineContainer = context.OnlineVariableContainerCollection.Create("GW_Timer");
            onlineContainer.AddVariable("GW_Timer");

            onlineContainer.ActivateBulkMode();
            onlineContainer.Activate();
            onlineContainer.BulkChanged += Serial_Status;

        }

        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string portName = sp.PortName;

            foreach (var thisStation in stations)
            {
                if (thisStation.MonitorPort == portName)
                {
                    string Update_varName = "GW_" + thisStation.StationName + "_Serial_Update";
                    IVariable thisUpdateVar = thisProject.VariableCollection[Update_varName];

                    DateTime nowTime = DateTime.Now;
                    thisUpdateVar.SetValue(0, nowTime.ToString());
                }
            }
        }

        public void Serial_Status(object sender, BulkChangedEventArgs e)
        {
            foreach (IVariable var in e.Variables)
            {
                if (var.GetValue(0).ToString() == "1")
                {
                    foreach (var thisStation in stations)
                    {
                        string Update_varName = "GW_" + thisStation.StationName + "_Serial_Update";
                        string Status_varName = "GW_" + thisStation.StationName + "_Serial_Status";
                        IVariable thisUpdateVar = thisProject.VariableCollection[Update_varName];
                        IVariable thisStatusVar = thisProject.VariableCollection[Status_varName];

                        double DT_compare = (DateTime.Now - thisUpdateVar.LastUpdateTime).TotalSeconds;
                        if (DT_compare > 5) { thisStatusVar.SetValue(0, false); }
                        else { thisStatusVar.SetValue(0, true); }
                    }
                }
            }


        }

        public void Stop()
        {
            if (Port_cnt > 0)
            {
                mySerialPort_1.Close();
                if (Port_cnt > 1)
                {
                    mySerialPort_2.Close();
                    if (Port_cnt > 2)
                    {
                        mySerialPort_3.Close();
                        if (Port_cnt > 3)
                        {
                            mySerialPort_4.Close();
                            if (Port_cnt > 4)
                            {
                                mySerialPort_5.Close();
                                if (Port_cnt > 5)
                                {
                                    mySerialPort_6.Close();
                                    if (Port_cnt > 6)
                                    {
                                        mySerialPort_7.Close();
                                        if (Port_cnt > 7)
                                        {
                                            mySerialPort_8.Close();
                                            if (Port_cnt > 8)
                                            {
                                                mySerialPort_9.Close();
                                                if (Port_cnt > 9)
                                                {
                                                    mySerialPort_10.Close();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            onlineContainer.BulkChanged -= Serial_Status;
            onlineContainer.Deactivate();
            thisProject.OnlineVariableContainerCollection.Delete(onlineContainer.Name);

        }

        public class Station_Content
        {
            public int StationNum { get; set; }
            public string StationName { get; set; }
            public string serialPort { get; set; }
            public string MonitorPort { get; set; }
            public int BaudRate { get; set; }
            public int DataBits { get; set; }
            public StopBits stopBits { get; set; }
            public Parity parity { get; set; }
        }

        #endregion
    }
}