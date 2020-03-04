/**
 * Author:    Riccardo Bicelli <r.bicelli@gmail.com>>
 * Created:   28.02.2020
 * 
 * (c) Copyright Riccardo Bicelli.
 **/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.ServiceProcess;
using System.Text;
using IniParser;
using IniParser.Model;
using System.Reflection;


namespace LabelSpooler
{
    public partial class LabelSpoolerService : ServiceBase
    {
        private SpoolMonitor _spoolMon;
        public LabelSpoolerService()
        {            
            _spoolMon = new SpoolMonitor();
            Globals.initialize();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Get assembly Version 
            
            
            if (_loadConfig())
            {
                _spoolMon.Run(Globals.pollIntervalms);

                Globals.eventRecord("Service Started (Version " + Globals.appVersion + ")");
            }
            else
                Globals.eventRecord("Unable to Start Service", EventLogEntryType.Error);
        }

        protected override void OnStop()
        {             
            _spoolMon.Stop();
            Globals.eventRecord("Service Stopped (Version" + Globals.appVersion + ")");
        }

        private bool _loadConfig()
        {
                //Load DB Data from INI File                              
                DB.Server =   Globals.iniData["database"]["server"];
                DB.Database = Globals.iniData["database"]["database"];
                DB.Username = Globals.iniData["database"]["username"];
                DB.Password = Globals.iniData["database"]["password"];                

                if (!DB.Connect())
                {                    
                    Globals.eventRecord("Unable to connect to Database",EventLogEntryType.Error);
                    return false;
                }                
                
            return true;
        }
    }
}
