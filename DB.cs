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
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace LabelSpooler
{
    public static class DB
    {
        private static SqlConnection conn = new SqlConnection();
        private static string _dbServer;
        private static string _dbUsername;
        private static string _dbDatabase;
        private static string _dbPassword;

        /* Properties */
        public static string Server
        {
            get
            {
                return DB._dbServer;
            }
            set
            {
                DB._dbServer = value;
            }
        }

        public static string Database
        {
            get
            {
                return DB._dbDatabase;
            }
            set
            {
                DB._dbDatabase = value;
            }
        }

        public static string Username
        {
            get
            {
                return DB._dbUsername;
            }
            set
            {
                DB._dbUsername = value;
            }
        }

        public static string Password
        {
            get
            {
                return DB._dbPassword;
            }
            set
            {
                DB._dbPassword = value;
            }
        }

        /*End Properties*/

        public static bool Connect()
        {
            DB.conn.ConnectionString = "Data Source=" + DB._dbServer + ";Initial Catalog=" + DB._dbDatabase + ";User id=" + DB._dbUsername + ";Password=" + DB._dbPassword;
            try
            {
                DB.conn.Open();
                DB.conn.Close();
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static DataTable GetJobs(int jobStatus = 0)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = DB.conn;
            DataTable dataTable = new DataTable();

            sqlCommand.Parameters.AddWithValue("@pJobStatus", (object)jobStatus);
            if (jobStatus==0)
            {
                sqlCommand.Parameters.AddWithValue("@pPrintRetrySeconds", (object)Globals.printRetrySeconds);
                sqlCommand.CommandText = "SELECT * FROM LabelSpooler WHERE (JobStatus=@pJobStatus AND jobRetries=0) OR  (JobStatus = @pJobStatus AND DATEDIFF(SECOND, jobLastSent, GETDATE()) >= @pPrintRetrySeconds) ORDER BY JobID";
            }
            else
                sqlCommand.CommandText = "SELECT * FROM LabelSpooler WHERE JobStatus=@pJobStatus ORDER BY JobID";

            try
            {
                DB.conn.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                dataTable.Load((IDataReader)sqlDataReader);
                DB.conn.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                Globals.eventRecord("Error getting Jobs: " + ex.Message, EventLogEntryType.Error);
                return dataTable;
            }
        }

        public static bool UpdateJobStatus(long jobID, int jobStatus, int jobRetriesAdd=0)
        {
            string sCommand;

            //if (jobRetriesAdd>0)
                sCommand= "UPDATE LabelSpooler SET jobStatus=@jobStatus,jobRetries=jobRetries+@jobRetriesAdd,jobLastSent=GETDATE() WHERE jobID=@jobID";
            //else
            //    sCommand = "UPDATE LabelSpooler SET jobStatus=@jobStatus,jobRetries=jobRetries+@jobRetriesAdd WHERE jobID=@jobID";

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = DB.conn;
                sqlCommand.CommandText = sCommand;
                sqlCommand.Parameters.AddWithValue("@jobID", (object)jobID);
                sqlCommand.Parameters.AddWithValue("@jobStatus", (object)jobStatus);
                sqlCommand.Parameters.AddWithValue("@jobRetriesAdd", (object)jobRetriesAdd);
                try
                {
                    DB.conn.Open();
                    sqlCommand.ExecuteNonQuery();
                    DB.conn.Close();
                    return true;
                }
                catch (SqlException ex)
                {
                    if (DB.conn.State == ConnectionState.Open) DB.conn.Close();
                    Globals.eventRecord("Error Updating Job Status: (" + ex.Number + ") " + ex.Message, EventLogEntryType.Error);
                    return false;
                }
            }
        }

        public static int PurgeJobs(int minutesLimit)
        {
            //Delete 
            string sCommand;
            int retRows;

            sCommand = "DELETE FROM LabelSpooler WHERE JobStatus=1 AND DATEDIFF(MINUTE,JobCreated,GETDATE())>=@minutesLimit";
            
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = DB.conn;
                sqlCommand.CommandText = sCommand;
                sqlCommand.Parameters.AddWithValue("@minutesLimit", (object)minutesLimit);

                try
                {
                    DB.conn.Open();
                    retRows = sqlCommand.ExecuteNonQuery();
                    DB.conn.Close();
                    return retRows;
                }
                catch (SqlException ex)
                {
                    if (DB.conn.State == ConnectionState.Open) DB.conn.Close();
                    Globals.eventRecord("Error Purging Jobs Status: (" + ex.Number + ") " + ex.Message, EventLogEntryType.Error);
                    return -1;
                }
            }
        }
    }
}



