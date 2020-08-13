using System;
using System.IO;
using System.Diagnostics;
using System.Web;

namespace BulkSmsStatTeleperformance
{
    /// <summary>
    /// Description résumée de TraceManager.
    /// </summary>
    public class TraceManager
    {
        private string strLogFileName = "";

        public string _fileName;

        public TraceManager(string fileName)
        {
            _fileName = fileName;

        }

        public void InitializeTraceListener(string strCurrentFileName)
        {
            // Creates the text file that the trace listener will write to.
            FileStream objFileStream = new FileStream(strCurrentFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

            // Creates the new trace listener.
            TextWriterTraceListener objTextWriterTraceListener = new TextWriterTraceListener(objFileStream);

            //add TextWriterTraceListener to listeners collection
            Trace.Listeners.Clear();
            Trace.Listeners.Add(objTextWriterTraceListener);

            strLogFileName = strCurrentFileName;
        }


        public void WriteCSV(string message)
        {
            string currentFileName = _fileName;

            if (currentFileName != strLogFileName)
                InitializeTraceListener(currentFileName);

            //string traceMessage = message;

            System.Text.StringBuilder errorMsg = new System.Text.StringBuilder();

            //byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(traceMessage);

            //errorMsg.Append(System.Text.Encoding.UTF8.GetString(utf8Bytes));

            errorMsg.Append(message);

            //Ecrire dans le fichier log

            Trace.WriteLine(errorMsg.ToString());
            //vider la mémoire 
            Trace.Flush();
        }

    }

}