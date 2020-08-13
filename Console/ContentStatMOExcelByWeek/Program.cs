using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ContentStatMOExcelByWeek
{
    class Program
    {
        static void Main(string[] args)
        {
            Process process;


            if (args.Count() > 1)
            {

                if (args[0] == "w")
                {
                    if (args.Count() > 2) //w WEEK YEAR
                    {
                        process = new Process(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
                    }
                    else //w WEEK 
                    {
                        process = new Process(Convert.ToInt32(args[1]), DateTime.Now.Year);
                    }
                }
                else if (args[0] == "b")
                {
                    for (int j = Convert.ToInt32(args[1]); j <= Convert.ToInt32(args[2]); j++)
                    {
                        process = new Process(j, Convert.ToInt32(args[3]));
                        Thread.Sleep(120000);
                    }
                }
                
            }
            else
            {
                process = new Process(20, 2018);
            }
        }
    }
}
