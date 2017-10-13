using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostCodeSorter
{
    class Program
    {


        static void Main(string[] args)
        {
            PostcodeMatrix pcNew;

            string preManifestsLocation = "";
            string masterListLocation = "";

            string manToSort = "";
            string jsonManifestLocation = "";

            string[] arguments = args;

            if (arguments.Length == 0)
            {
                Console.WriteLine("no args");
            }
            else
            {
                switch (arguments[0])
                {
                    case "/create":

                        if (arguments.Length <= 2)
                        {
                            Console.WriteLine("not enough parameters");
                            break;
                        }

                        preManifestsLocation = arguments[1].ToString();
                        masterListLocation = arguments[2].ToString();

                        //create postcode matrix
                        pcNew = new PostcodeMatrix(preManifestsLocation);

                        //output json masterlist file
                        pcNew.outputJson(masterListLocation);

                        if (arguments.Length <= 4)
                        {
                            Console.WriteLine("no manifest to sort");
                            break;
                        }

                        manToSort = arguments[3].ToString();
                        jsonManifestLocation = arguments[4].ToString();

                        //load manifest
                        ManifestToSort manifest = new ManifestToSort(manToSort);

                        //output sorted manifest
                        manifest.outputSortedManifest(jsonManifestLocation, pcNew.pcMasterList);

                        break;

                    default:
                        Console.WriteLine("invalid args");
                        break;
                }
            }

            //PostcodeMatrix pcNew = new PostcodeMatrix(@"c:\parcels\manis.csv");


            //Console.ReadLine();

            //string[] lines = File.ReadAllLines(@"c:\parcels\test.csv", Encoding.UTF8);

            //List<Parcel> newManifest = new List<Parcel>();

            //foreach (var line in lines)
            //{
            //    string[] elements = line.Split(',');

            //    if (elements[0] != "")
            //    {

            //        string csvLine = $"0,0,{elements[8]},{ elements[4] },{ elements[4] }, { elements[0] }, 0:00 ";

            //        Parcel p = new Parcel(csvLine);
            //        newManifest.Add(p);
            //    }

            //}

            ////unique addresses
            //var manifestDrops = newManifest.Select(x => x.identifier).Distinct();

            //Console.WriteLine($"***  manifest has { newManifest.Count } parcels");

            //Console.WriteLine($"***  number of drops { manifestDrops.Count() }");

            //Console.WriteLine($"***  average drops per hour { pcNew.averageDropsPerHour() }");

            //double delTimeMinutes = ((double)manifestDrops.Count() / (double)pcNew.averageDropsPerHour()) * 60;
            //TimeSpan span = TimeSpan.FromMinutes(delTimeMinutes);


            //Console.WriteLine($"***  estimated delivery duration { span.ToString(@"hh") } hours and { span.ToString(@"mm") } minutes \n");



            //List<string> pcmList = new List<string>();

            ////create master postcode list
            //pcNew.pcMasterList.OrderBy(x => x.calculatedPosition);
            //foreach (var pc in pcNew.pcMasterList)
            //{
            //    pcmList.Add(pc.addressPostcode);
            //}

            ////now apply the positions to the new manifest
            //foreach (var parcel in newManifest)
            //{
            //    int ind = pcmList.FindIndex(x => x == parcel.pcAbbriev);

            //    parcel.sortPosition = ind;


            //}

            //newManifest = newManifest.OrderBy(x => x.sortPosition).ToList();

            //foreach (var parcel in newManifest)
            //{
            //    int ind = pcNew.pcMasterList.FindIndex(x => x.addressPostcode == parcel.pcAbbriev);

            //    int sDev = 0;
            //    int dataPoints = 0;

            //    if (ind != -1)
            //    {
            //        sDev = pcNew.pcMasterList[ind].getStandardDeviation();
            //    }

            //    if (ind != -1)
            //    {
            //        dataPoints = pcNew.pcMasterList[ind].pList.Count;
            //    }


            //    string statsString = "";

            //    if (sDev > 700)
            //    {
            //        statsString += "[ High Deviation ]";
            //    }

            //    if (dataPoints < 8)
            //    {
            //        statsString += "[ Low Data Points ]";
            //    }

            //    if (statsString == "")
            //    {
            //        statsString = "[ OK ]";
            //    }

            //    //Console.WriteLine($"{ parcel.postcode } - { parcel.addressLine } \t \t \t { statsString }");
            //    Console.WriteLine(String.Format("{0,-50} {1, -20}", $"{ parcel.postcode } - { parcel.addressLine }", statsString));
            //}

        }
    }
}
