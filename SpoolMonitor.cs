/**
 * Author:    Riccardo Bicelli <r.bicelli@gmail.com>>
 * Created:   28.02.2020
 * 
 * (c) Copyright Riccardo Bicelli.
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;

namespace LabelSpooler
{
    class SpoolMonitor
    {
        private bool _mStarted;
        private bool _mStop;

        private int _pollInterval;
        
        public void Run(int pollInterval=250)
        {
            _pollInterval = pollInterval;
            Thread thread = new Thread(new ThreadStart(this._spoolMonitor));
            if (this._mStarted)
                return;
            this._mStop = false;
            thread.Start();
        }

        private void _spoolMonitor()
        {           
            this._mStarted = true;
            DataTable spoolJobs;
            DateTime lastHousekeeperRun;
            TimeSpan housekeeperRunDiff; 

            int jobStatusUpdate;

            string printerName;
            string jobName;

            lastHousekeeperRun = DateTime.Now;

            while (!this._mStop)
            {
                //Polling the DB
                spoolJobs = DB.GetJobs();

                foreach (DataRow row in spoolJobs.Rows)
                {
                    //Get Printer name from INI Data
                    printerName = Globals.iniData["printers"][row["printerName"].ToString()];

                    //Set Job Name
                    jobName = Globals.APP_NAME + " - Job " + row["jobID"].ToString();

                    //If the printer isn't mapped in INI file, let's assume a printer name is specified
                    if (printerName==null)
                        printerName = row["printerName"].ToString();
                   
                    //Print Label                    
                    if (RawPrinterHelper.SendStringToPrinter(printerName, row["printData"].ToString(), jobName)) {
                        //Job spooled, OK
                        DB.UpdateJobStatus((long)row["jobID"], 1);
                    } 
                    else
                    {
                        //Job isn't spooled, will be spooled the next iteration unless Max Retries count is reached
                        //TODO: retry print after x amount of seconds, based on timestamp. 
                        if ((int)row["jobRetries"] >= Globals.printMaxRetries - 1) jobStatusUpdate = 2; else jobStatusUpdate = 0; 
                        DB.UpdateJobStatus((long)row["jobID"], jobStatusUpdate, 1);
                    }
                }

                //Check for Housekeeper
                if (Globals.housekeeperRunInterval>0) { 
                    housekeeperRunDiff = DateTime.Now - lastHousekeeperRun;
                
                    if (housekeeperRunDiff.TotalMinutes > Globals.housekeeperRunInterval)
                    {
                        HousekeeperRun();
                        lastHousekeeperRun = DateTime.Now;
                    }
                }
                Thread.Sleep(_pollInterval);
            } 
            this._mStarted = false;
            Globals.eventRecord("Spool Monitor Stopped");
        }

        private void HousekeeperRun()
        {
            int iResult;
            
            iResult = DB.PurgeJobs(Globals.housekeepingMinutes);

            if (iResult>=0)
                Globals.eventRecord("Housekeeper Deleted " + iResult.ToString() + " records");
            else
                Globals.eventRecord("There was an error invoking the Housekeeper", System.Diagnostics.EventLogEntryType.Error);
        }
        
        public void Stop()
        {
            this._mStop = true;
        }
    }
}
