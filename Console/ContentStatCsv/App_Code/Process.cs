using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Npgsql;



namespace ContentStatCsv
{
    public class Process
    {
        
 

        private TraceManager traceManager;

        private string requeteSmsContent_MO_Postrgres = Convert.ToString(ConfigurationSettings.AppSettings["SQL_STAT_MO_POSTGRES"]);
        private string requeteSmsContent_MT_Postrgres = Convert.ToString(ConfigurationSettings.AppSettings["SQL_STAT_MT__POSTGRES"]);



        private string CONNECTION_STRING_POSTGRESQL = Convert.ToString(ConfigurationSettings.AppSettings["CONNECTION_STRING_POSTGRESQL"]);

        private string PARAM_PATH_EXCEL = Convert.ToString(ConfigurationSettings.AppSettings["PARAM_PATH_EXCEL"]);

      
        public Process()
        {
            try
            {
                GenerateCsvMO();
                Console.WriteLine("********************* GENERATION MT BILING **************************");
                System.Threading.Thread.Sleep(10000);
                GenerateCsvMTBiling();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        
        private void GenerateCsvMO()
        {
            try
            {


                string STAT_MO_BILING = DateTime.Now.ToString("yyyy-MM-dd");


                traceManager = new TraceManager(PARAM_PATH_EXCEL + "STAT_CONTENT_MO_" + STAT_MO_BILING + ".txt");

                string chaineConnexion = CONNECTION_STRING_POSTGRESQL;

                NpgsqlConnection npgsqlConnection = new NpgsqlConnection(chaineConnexion);
                npgsqlConnection.Open();

                DataTable dataTable = new DataTable();

                string requete = requeteSmsContent_MO_Postrgres;

                /*dataTable.Columns.Add("id_sms_mo", typeof(String));
                dataTable.Columns.Add("sms", typeof(String));
                dataTable.Columns.Add("msisdn", typeof(String));
                dataTable.Columns.Add("shortcode", typeof(String));
                dataTable.Columns.Add("codser", typeof(String));
                dataTable.Columns.Add("status", typeof(String));
                dataTable.Columns.Add("entry_date", typeof(String));
                dataTable.Columns.Add("login", typeof(String));
                dataTable.Columns.Add("sysid", typeof(String));
                dataTable.Columns.Add("length", typeof(String));
                */

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requete, npgsqlConnection);
                NpgsqlDataReader dr = npgsqlCommand.ExecuteReader();

                Console.WriteLine("CORPORATE;Code Service;MSISDN;MESSAGE;Date réception;Short Code;Status;SYS_ID;Reference");

                traceManager.WriteCSV("CORPORATE;Code Service;MSISDN;MESSAGE;Date réception;Short Code;Status;SYS_ID;Reference");

                while (dr.Read())
                {


                    Console.WriteLine(dr[0].ToString() + ";" + dr[1].ToString() + ";" + dr[2].ToString() + ";" + dr[3].ToString() + ";" + dr[4].ToString() + ";" + dr[5].ToString() + ";" +
                        GetStatut(dr[6].ToString()) + ";" + dr[7].ToString() + ";" + dr[8].ToString() + ";" );

                    traceManager.WriteCSV(dr[0].ToString() + ";" + dr[1].ToString() + ";" + dr[2].ToString() + ";" + dr[3].ToString() + ";" + dr[4].ToString() + ";" + dr[5].ToString() + ";" +
                      GetStatut(dr[6].ToString()) + ";" + dr[7].ToString() + ";" + dr[8].ToString() + ";" );
                }
                
                dr.Close();
                npgsqlConnection.Close();
               


               // Excel.ExcelUtlity obj = new Excel.ExcelUtlity();

                // string title = "STAT RESELLER_POSTGRESQL";
                //  string fileName = "Bulk_POSTGRESQL_R" + ".csv";


                //   obj.WriteDataTableToExcel(dataTable, title, PARAM_PATH_EXCEL + fileName, "");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GenerateCsvMTBiling()
        {
            try
            {

                string STAT_MT_BILING = DateTime.Now.ToString("yyyy-MM-dd");
               

                traceManager = new TraceManager(PARAM_PATH_EXCEL+ "STAT_CONTENT_MT_BILING_" + STAT_MT_BILING+".txt");

                string chaineConnexion = CONNECTION_STRING_POSTGRESQL;

                int day = 0;

                NpgsqlConnection npgsqlConnection;

                npgsqlConnection = new NpgsqlConnection(chaineConnexion);
                npgsqlConnection.Open();

                while (day < 32)
                {

                    Console.WriteLine("********** DAY : " + day.ToString());

                    int tentative = 0;

                    while (npgsqlConnection.State != ConnectionState.Open && tentative < 3)
                    {
                        System.Threading.Thread.Sleep(2000);
                        npgsqlConnection = new NpgsqlConnection(chaineConnexion);
                        npgsqlConnection.Open();
                    }


                    DataTable dataTable = new DataTable();

                    string requete = requeteSmsContent_MT_Postrgres.Replace("[[NBR_DAY]]", day.ToString());


                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requete, npgsqlConnection);
                    NpgsqlDataReader dr = npgsqlCommand.ExecuteReader();

                    Console.WriteLine("CORPORATE;Code Service;sysid;sms;msisdn;sender;Status;entry_date;ack_message_id;ack_type;ack_entry_date;Reference");

                    traceManager.WriteCSV("CORPORATE;Code Service;sysid;sms;msisdn;sender;Status;entry_date;ack_message_id;ack_type;ack_entry_date;Reference;");

                    while (dr.Read())
                    {



                        Console.WriteLine(dr[0].ToString() + ";" + dr[1].ToString() + ";" + dr[2].ToString() + ";" + dr[3].ToString() + ";" + dr[4].ToString() + ";" + dr[5].ToString() + ";" +
                             GetStatut(dr[6].ToString()) + ";" + dr[7].ToString() + ";" + dr[8].ToString() + ";" + dr[9].ToString() + ";" + dr[10].ToString() + ";" + dr[11].ToString() + ";");

                        traceManager.WriteCSV(dr[0].ToString() + ";" + dr[1].ToString() + ";" + dr[2].ToString() + ";" + dr[3].ToString() + ";" + dr[4].ToString() + ";" + dr[5].ToString() + ";" +
                          GetStatut(dr[6].ToString()) + ";" + dr[7].ToString() + ";" + dr[8].ToString() + ";" + dr[9].ToString() + ";" + dr[10].ToString() + ";" + dr[11].ToString() + ";");
                        System.Threading.Thread.Sleep(20);
                    }

                    dr.Close();
                    

                    day++;

                    System.Threading.Thread.Sleep(2000);

                }

                npgsqlConnection.Close();
                npgsqlConnection.Dispose();


                // Excel.ExcelUtlity obj = new Excel.ExcelUtlity();

                // string title = "STAT RESELLER_POSTGRESQL";
                //  string fileName = "Bulk_POSTGRESQL_R" + ".csv";


                //   obj.WriteDataTableToExcel(dataTable, title, PARAM_PATH_EXCEL + fileName, "");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private string GetStatut(string statut)
        {
            if (statut == "1")
            {
                return "ENVOYER";
            }
            else if (statut == "0")
            {
                return "non envoyer";
            }
            else if (statut == "4")
            {
                return "FILTRER";
            }
            else if (statut == "5")
            {
                return "BLACK_LISTE";
            }
            else if (statut == "6")
            {
                return "SMS STOP";
            }
            else if (statut == "3")
            {
                return "EXCEPTION";
            }
            else
            {
                return "UNKNOWN";
            }
        }

      
    }
}
