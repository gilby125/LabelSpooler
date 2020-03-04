/**
 * Author:    Riccardo Bicelli <r.bicelli@gmail.com>>
 * Created:   28.02.2020
 * 
 * (c) Copyright Riccardo Bicelli.
 **/

using System;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using IniParser;
using IniParser.Model;

namespace LabelSpooler
{
    public static class Globals
    {
        public static string appPath = "";
        public static string appVersion;
        public const string EVENTLOG_SOURCE= "LabelSpoolerService";
        public const string EVENTLOG_LOG = "LabelSpoolerLog";
        public const string APP_NAME = "Label Spooler";
        
        public static string INIFilename;
        public static IniData iniData;

        public static int printMaxRetries;
        public static int printRetrySeconds;
        public static int housekeepingMinutes;

        public static int housekeeperRunInterval;
        public static int pollIntervalms;

        public static System.Diagnostics.EventLog _eventLogger;

        public static void initialize()
        {
            initEventLog();

            appPath = AppDomain.CurrentDomain.BaseDirectory;
            INIFilename = appPath + "config.ini";
            
            appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            if (File.Exists(INIFilename))
            {
                iniData = new FileIniDataParser().ReadFile(INIFilename);
                
                if (!Int32.TryParse(iniData["print_settings"]["max_retries"], out printMaxRetries))
                    printMaxRetries = 3;

                if (!Int32.TryParse(iniData["print_settings"]["retry_interval"], out printRetrySeconds))
                    printRetrySeconds = 5;

                if (!Int32.TryParse(iniData["spooler"]["housekeeping_interval"], out housekeepingMinutes))
                    housekeepingMinutes = 1440;
                
                if (!Int32.TryParse(iniData["spooler"]["housekeeper_run_interval"], out housekeeperRunInterval))
                    housekeeperRunInterval = 60;

                if (!Int32.TryParse(iniData["spooler"]["poll_interval"], out pollIntervalms))
                    pollIntervalms = 250;
            }
            else
                eventRecord("Unable to find Configuration File " + INIFilename);
        }

        private static bool initEventLog()
        {
            //Initialize Event Logger            
            _eventLogger = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(Globals.EVENTLOG_SOURCE))
            {
                System.Diagnostics.EventLog.CreateEventSource(EVENTLOG_SOURCE, EVENTLOG_LOG);
            }

           _eventLogger.Source = Globals.EVENTLOG_SOURCE;
           _eventLogger.Log = Globals.EVENTLOG_LOG;

            return true;
        }

        public static void eventRecord(string sEventMessage, EventLogEntryType eeType = EventLogEntryType.Information)
        {
            if (!System.Diagnostics.EventLog.SourceExists(Globals.EVENTLOG_SOURCE)) initEventLog();
            
            _eventLogger.WriteEntry(sEventMessage, eeType);
            
        }
        
    }
}
