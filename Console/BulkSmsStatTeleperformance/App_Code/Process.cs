using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Npgsql;



namespace BulkSmsStatTeleperformance
{
    public class Process
    {
        private TraceManager traceManager;


        private string requeteSmsBulkPostrgres = Convert.ToString(ConfigurationSettings.AppSettings["SQL_Bulk_STAT_POSTGRES"]);


        private string CONNECTION_STRING_POSTGRESQL = Convert.ToString(ConfigurationSettings.AppSettings["CONNECTION_STRING_POSTGRESQL"]);

        private string PARAM_PATH_EXCEL = Convert.ToString(ConfigurationSettings.AppSettings["PARAM_PATH_EXCEL"]);

        public Process()
        {
            try
            {
                GenerateExcelBulkPostgres();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
      
        private void GenerateExcelBulkPostgres()
        {
            try
            {
                string dateCreation = DateTime.Now.ToString("yyyy-MM-dd");
                traceManager = new TraceManager(PARAM_PATH_EXCEL + "STAT_TP_SRV_BULK" + dateCreation + ".csv");
               
                int tentative = 0;

                NpgsqlConnection npgsqlConnection = new NpgsqlConnection(CONNECTION_STRING_POSTGRESQL);
                npgsqlConnection.Open();

                while (npgsqlConnection.State != ConnectionState.Open && tentative < 3)
                {
                    System.Threading.Thread.Sleep(2000);
                    npgsqlConnection = new NpgsqlConnection(CONNECTION_STRING_POSTGRESQL);
                    npgsqlConnection.Open();
                }

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSmsBulkPostrgres, npgsqlConnection);
                NpgsqlDataReader dr = npgsqlCommand.ExecuteReader();

                Console.WriteLine("Client;Id_Client_Reseller;MSISDN;ID_SMS;Sender;Sms;Nbre_SMS;Date_SAI;Date_Envoi;SYS_ID;Opérateur;ACCUSE_TYPE;ACCUSE_DATE;STATUT;TAILLE SMS;");

                traceManager.WriteCSV("Client;Id_Client_Reseller;MSISDN;ID_SMS;Sender;Sms;Nbre_SMS;Date_SAI;Date_Envoi;SYS_ID;Opérateur;ACCUSE_TYPE;ACCUSE_DATE;STATUT;TAILLE SMS;");

                while (dr.Read())
                {


                    Console.WriteLine(dr[0].ToString() + ";" + dr[1].ToString() + ";" + dr[2].ToString() + ";" + dr[3].ToString() + ";" + dr[4].ToString() + ";" + dr[5].ToString() + ";" +
                        dr[6].ToString() + ";" + dr[7].ToString() + ";" + dr[8].ToString() + ";" + dr[9].ToString() + ";" + dr[10].ToString() + ";" + dr[11].ToString() + ";" + dr[12].ToString() + ";" + GetStatut(dr[13].ToString()) + ";" + dr[14].ToString() + ";");

                    traceManager.WriteCSV(dr[0].ToString() + ";" + dr[1].ToString() + ";" + dr[2].ToString() + ";" + dr[3].ToString() + ";" + dr[4].ToString() + ";" + dr[5].ToString() + ";" +
                        dr[6].ToString() + ";" + dr[7].ToString() + ";" + dr[8].ToString() + ";" + dr[9].ToString() + ";" + dr[10].ToString() + ";" + dr[11].ToString() + ";" + dr[12].ToString() + ";" + GetStatut(dr[13].ToString()) + ";" + dr[14].ToString() + ";");
                }
                
                dr.Close();

                npgsqlConnection.Close();
                npgsqlConnection.Dispose();

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
