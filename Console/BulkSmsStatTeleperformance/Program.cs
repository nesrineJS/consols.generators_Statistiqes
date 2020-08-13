using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BulkSmsStatTeleperformance
{
    class Program
    {
        static void Main(string[] args)
        {
            Process process;
            
              //  Console.WriteLine("ok!");
            /*
                        if (args.Count() == 4)
                        {
                            process = new Process(Convert.ToString(args[0]), "",   Convert.ToDateTime(args[1]), Convert.ToDateTime(args[2]), Convert.ToString(args[3]));             
                        }               
                        else if (args.Count() == 5)
                        {
                            process = new Process(Convert.ToString(args[0]),  Convert.ToString(args[1]), Convert.ToDateTime(args[2]), Convert.ToDateTime(args[3]), Convert.ToString(args[4]));
                        }
                        else
                        {
                            Console.WriteLine("PARAM 1: id_reseller date_begin date_end name_base_postgres");
                            Console.WriteLine("PARAM 2: id_reseller id_client date_begin date_end name_base_postgres");
                            process = new Process("204", "", Convert.ToDateTime("2017-03-23 13:56:40.710"), Convert.ToDateTime("2017-03-28 13:56:40.710"), "dbnddd");
                        }
                        */
            if (args.Count() == 0)
            {
                process = new Process();
            }
         
            else
            {
                Console.WriteLine("SORRY!");
               
            }
        }
    
    }
}
