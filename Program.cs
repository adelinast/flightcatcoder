using System;
using System.Collections.Generic;
using System.IO;

namespace CatCoder
{
    class Program
    {
        static void getMinMaxI(List<int> listI, ref int minI, ref int maxI)
        {
            foreach (int item in listI)
            {
                if (minI > item)
                {
                    minI = item;
                }
                if (maxI < item)
                {
                    maxI = item;
                }
            }
        }
        static void getMinMaxD(List<double> listD, ref double minD, ref double maxD)
        {
            foreach (double item in listD)
            {
                if (minD > item)
                {
                    minD = item;
                }
                if (maxD < item)
                {
                    maxD = item;
                }
            }
        }
        static void Main(string[] args)
        {
            string inputLine1 = "";
            List<string> inputlines = new List<string>();
            int count = 0;
            if (args.Length != 1)
            {
                Console.WriteLine("<arg0> is the input filename");
                return;
            }

            //format
            //N
            //timestamp, lat,long, altitude (repeats N times)
            using (StreamReader sr = new StreamReader(args[0]))
            {
                while (sr.Peek() >= 0)
                {
                    if (count == 0)
                    {
                        inputLine1 = sr.ReadLine();
                    }
                    else
                    {
                        string inputLine;
                        inputLine = sr.ReadLine();
                        inputlines.Add(inputLine);
                    }
                    count++;
                }
            }


            //flight entries
            int N = Convert.ToInt32(inputLine1);

            List<int> timestamps = new List<int>();
            List<double> latlist = new List<double>();
            List<double> longlist = new List<double>();
            List<double> altlist = new List<double>();

            foreach (string inputLine in inputlines)
            {
                string[] flightDetailsStr = inputLine.Split(',');
                timestamps.Add(Convert.ToInt32(flightDetailsStr[0]));
                latlist.Add(Convert.ToDouble(flightDetailsStr[1]));
                longlist.Add(Convert.ToDouble(flightDetailsStr[2]));
                altlist.Add(Convert.ToDouble(flightDetailsStr[3]));
            }

            //get min max details
            int minTimestamp = Int32.MaxValue;
            int maxTimestamp = Int32.MinValue;
            double minLat = Double.MaxValue;
            double maxLat = Double.MinValue;
            double minLong = Double.MaxValue;
            double maxLong = Double.MinValue;
            double maxAltitude = Double.MinValue;

            getMinMaxI(timestamps, ref minTimestamp, ref maxTimestamp);

            getMinMaxD(latlist, ref minLat, ref maxLat);

            getMinMaxD(longlist, ref minLong, ref maxLong);
            
            foreach (double alt in altlist)
            {
                if (maxAltitude < alt)
                {
                    maxAltitude = alt;
                }
            }

            string path = args[0] + ".out";
           
            using (StreamWriter w = File.CreateText(path))
            {
                w.WriteLine(minTimestamp+" "+ maxTimestamp);
                w.WriteLine(minLat + " " + maxLat);
                w.WriteLine(minLong + " " + maxLong);
                w.WriteLine(String.Format("{0:0.0#}", maxAltitude));

                w.WriteLine();
            }
        }
    }
}
