using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
/*
*             //get min max details
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
}*/
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
            List<string> startAirports = new List<string>();
            List<string> destAirports = new List<string>();
            List<int> takeoffs = new List<int>();

            foreach (string inputLine in inputlines)
            {
                string[] flightDetailsStr = inputLine.Split(',');
                timestamps.Add(Convert.ToInt32(flightDetailsStr[0]));
                latlist.Add(Convert.ToDouble(flightDetailsStr[1]));
                longlist.Add(Convert.ToDouble(flightDetailsStr[2]));
                altlist.Add(Convert.ToDouble(flightDetailsStr[3]));
                startAirports.Add(flightDetailsStr[4]);
                destAirports.Add(flightDetailsStr[5]);
                takeoffs.Add(Convert.ToInt32(flightDetailsStr[6]));
            }

            string path = args[0] + ".out";
            /*Repeated for each pair of airports, A and B, that have at least one flight
            going from A to B.Direction matters
            Sort alphabetically by A and then B*/
            var sortedListStart = startAirports.OrderBy(x => x).ToList();
            var destAirportsCoresp = new List<string>();
            List<KeyValuePair<string, string>> listpairsair = new List<KeyValuePair<string, string>>();
            Dictionary<KeyValuePair<string, string>, int> dict = new Dictionary<KeyValuePair<string, string>, int>();
            foreach (string startApt in sortedListStart)
            {
                int countFlight = 0;

                int index = 0;

                for (int i = 0; i < startAirports.Count; i++)
                {
                    if (startApt.Equals(startAirports[i]))
                    {
                        if (listpairsair.Count != 0)
                        {
                            foreach (var pair in listpairsair)
                            {
                                if (!pair.Key.Equals(startApt))
                                {

                                    index = i; 
                                }
                                else
                                {
                                    continue;
                                }

                            }
                        }
                        else
                        {
                            index = i;
                            break;
                        }
                    }
                }
                
                //if (index == -1) break;
                destAirportsCoresp.Add(destAirports.ElementAt(index));

                KeyValuePair<string, string> newpair = new KeyValuePair<string, string>(startAirports.ElementAt(index), destAirports.ElementAt(index));
                
                listpairsair.Add(newpair);
                foreach (KeyValuePair<string, string> pair in listpairsair)
                {
                    if (pair.Equals(newpair)) countFlight++;
                }

                KeyValuePair<string, string> newpair2 = new KeyValuePair<string, string>(startApt, destAirports.ElementAt(index));

                if (!dict.ContainsKey(newpair2))
                {
                    dict.Add(newpair2, countFlight);
                }
                else
                {
                    dict[newpair2] = countFlight;
                }
            }
           
            var sortedListDest = destAirportsCoresp.OrderBy(x => x).ToList();
            using (StreamWriter w = File.CreateText(path))
            {

                for (int i = 0; i < sortedListStart.Count; i++)
                {
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>(sortedListStart.ElementAt(i),
                        destAirportsCoresp.ElementAt(i));
                    w.WriteLine(sortedListStart.ElementAt(i) + " " + destAirportsCoresp.ElementAt(i)
                        + " "+
                        + dict[pair]);

                }

                w.WriteLine();
            }
        }
    }
}
