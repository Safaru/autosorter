using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostCodeSorter
{
    class ManifestToSort
    {

        public List<Parcel> newManifest { get; set; }

        public ManifestToSort(string manifestLocation)
        {
            newManifest = new List<Parcel>();

            string[] lines = File.ReadAllLines(manifestLocation, Encoding.UTF8);

            foreach (var line in lines)
            {
                string[] elements = line.Split(',');

                if (elements[0] != "")
                {
                    
                    string csvLine = $"0,0,{elements[8]},{ elements[4] },{ elements[4] }, { elements[0] }, 0:00 ";
                    //Console.WriteLine($"[ { csvLine } ]");

                    Parcel p = new Parcel(csvLine);
                    newManifest.Add(p);
                }

            }
        }

        public void outputSortedManifest(string sortedLocation, List<Postcode> postcodeMasterList)
        {
            List<Postcode> pcmList = postcodeMasterList;

            pcmList = pcmList.OrderBy(x => x.calculatedPosition).ToList();

            //now apply the positions to the new manifest
            foreach (var parcel in newManifest)
            {
                int ind = pcmList.FindIndex(x => x.addressPostcode == parcel.pcAbbriev);

                parcel.sortPosition = ind;

            }

            // sort on applied positions
            newManifest = newManifest.OrderBy(x => x.sortPosition).ToList();


            string json = JsonConvert.SerializeObject(newManifest.ToArray());

            //write to file
            System.IO.File.WriteAllText(sortedLocation, json);
            Console.WriteLine($"sorted manifest (json) { sortedLocation } created ");


    }




    }
}
