using System;
using System.Configuration;
using System.Collections.Generic;
using Npgsql;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace ContentStatMOExcelByWeek
{
    public class Process
    {
        private int _week;
        private int _year;

        private string idSmsMoMin = Convert.ToString(ConfigurationSettings.AppSettings["ID_SMS_MO_MIN"]);

        private string requeteSysID = Convert.ToString(ConfigurationSettings.AppSettings["SQL_SYSID"]); 
        private string requeteSmsMO = Convert.ToString(ConfigurationSettings.AppSettings["SQL_MO"]); 

        private string CONNECTION_STRING = Convert.ToString(ConfigurationSettings.AppSettings["CONNECTION_STRING"]);
        private string PARAM_PATH_EXCEL = Convert.ToString(ConfigurationSettings.AppSettings["PARAM_PATH_EXCEL"]);

        private NpgsqlConnection npgsqlConnection;
     

        public Process(int week, int year)
        {
            _week = week;
            _year = year;

            Execute();
        }

        private void Execute()
        {
            try
            {
                List<string> listSysID = new List<string>();
                listSysID = GetSysID();

                for (int i = 0; i < listSysID.Count(); i++)
                {
                    Console.WriteLine("======>Begin System ID" + listSysID[i] + "<========");
                    GenerateExcelMOBySysID(listSysID[i]);
                    Console.WriteLine("======>End System ID" + listSysID[i] + "<========");
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void GenerateExcelMOBySysID(string sysid)
        {
            try
            {
                npgsqlConnection = new NpgsqlConnection(CONNECTION_STRING);
                npgsqlConnection.Open();

                DataTable dataTable = new DataTable();

                string requete = requeteSmsMO.Replace("%WEEK%", _week.ToString()).Replace("%YEAR%", _year.ToString());
                requete = requete.Replace("%id_sms_mo%", idSmsMoMin).Replace("%SYSID%", sysid) ;

                dataTable.Columns.Add("sysid", typeof(String));
                dataTable.Columns.Add("id_sms_mo", typeof(String));
                dataTable.Columns.Add("sms", typeof(String));
                dataTable.Columns.Add("msisdn", typeof(String));
                dataTable.Columns.Add("shortcode", typeof(String));
                dataTable.Columns.Add("codser", typeof(String));
                dataTable.Columns.Add("status", typeof(String));
                dataTable.Columns.Add("entry_date", typeof(String));
                dataTable.Columns.Add("login", typeof(String));
                
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requete, npgsqlConnection);
                NpgsqlDataReader dr = npgsqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    dataTable.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), GetStatutMO(dr[6].ToString()), dr[7].ToString(), dr[8].ToString());
                }

                dr.Close();
                npgsqlConnection.Close();

                Excel.ExcelUtlity obj = new Excel.ExcelUtlity();

                string title = "STAT SYSID " + sysid + " -Y" + _year.ToString() + " - W" + _week.ToString();
                string fileName = "Content_MO_Y" + _year.ToString() + "_W" + GetWeek(_week) + "_S-" + sysid+".xlsx";

                obj.WriteDataTableToExcel(dataTable, title, PARAM_PATH_EXCEL + fileName, "");

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private List<string> GetSysID()
        {
            try
            {
                npgsqlConnection = new NpgsqlConnection(CONNECTION_STRING);
                npgsqlConnection.Open();

                List<string> lstWebSysID = new List<string>();
                string requete = requeteSysID.Replace("%WEEK%", _week.ToString()).Replace("%YEAR%", _year.ToString());
                requete = requete.Replace("%id_sms_mo%", idSmsMoMin);

                
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requete, npgsqlConnection);
                NpgsqlDataReader dr = npgsqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    lstWebSysID.Add(dr[0].ToString());
                }

                dr.Close();

                npgsqlConnection.Close();

                return lstWebSysID;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        private string GetStatutMO(string status)
        {
            if (status == "1")
            {
                return "WORKED";
            }
            else if (status == "2")
            {
                return "ERROR";
            }
            else if (status == "3")
            {
                return "EXCEPTION";
            }
            else if (status == "0")
            {
                return "WAIT";
            }
            else
            {
                return "UNKNOWN";
            }
        }


        private string GetWeek(int week)
        {
            if(week < 10)
            {
                return "0" + week.ToString();
            }
            else
            {
                return week.ToString();
            }
        }
    }
}
