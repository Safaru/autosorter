using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PostCodeSorter
{
    class PostcodeMatrix
    {
        public List<Postcode> pcMasterList { get; set; }

        public List<Manifest> _manifests { get; set; }

        public PostcodeMatrix(string csvLocation)
        {

            _manifests = new List<Manifest>();
            pcMasterList = new List<Postcode>();

            string[] lines = File.ReadAllLines(csvLocation, Encoding.UTF8);


            string[] element = lines[0].Split(',');
            //read first manifest ID
            int manifestID = Int32.Parse(element[1]);

            Manifest manifest = new Manifest(manifestID);

            foreach (var line in lines)
            {
                string[] elements = line.Split(',');

                //if manifest changes then create new manifest
                if (Int32.Parse(elements[1]) != manifest.manifestID )
                {
                    //add the existing manifest to the manifests list
                    _manifests.Add(manifest);
                    //Console.WriteLine($" { manifest.manifestID } added to manifest list with { manifest.parcels.Count } parcels ");
                    //create new
                    manifest = new Manifest(Int32.Parse(elements[1]));

                }

                manifest.addParcel(line);

            }

            //add last manifest
            _manifests.Add(manifest);
            //Console.WriteLine($" { manifest.manifestID } added to manifest list with { manifest.parcels.Count } parcels \n\n");

            //now create the postcode master list with each individual postcode
            foreach (var m in _manifests)
            {
                foreach (var p in m.parcels)
                {
                    //if not already in list then add new postcode
                    if (!pcMasterList.Any(x => x.addressPostcode == p.pcAbbriev))
                    {
                        pcMasterList.Add(new Postcode(p.pcAbbriev));
                    }
                }
            }

            Console.WriteLine($"postcode masterlist has { pcMasterList.Count } unique postcodes \n \n");

            //create the matrix 
            createPostcodeMatrix();

            //outputReport();

        }

        public void outputJson (string jsonLocation)
        {
            string json = JsonConvert.SerializeObject(pcMasterList.ToArray());

            //write string to file
            System.IO.File.WriteAllText(jsonLocation, json);
            Console.WriteLine($"postcode master list (json) { jsonLocation } created ");
        }

        public int averageDropsPerHour()
        {
            List<double> averageDrops = new List<double>();

            foreach (var mani in _manifests)
            {
                mani.parcels = mani.parcels.OrderBy(x => x.timeAttempted).ToList();
                var lastDropTime = mani.parcels.Max(x => x.timeAttempted);
                var firstDropTime = mani.parcels.Min(x => x.timeAttempted);

                var drops = mani.numberOfDrops();

                var duration = (lastDropTime - firstDropTime).TotalMinutes;

                averageDrops.Add(60/(duration/drops));

            }

            return Convert.ToInt32(averageDrops.Average());
        }

        private void outputReport()
        {
            pcMasterList = pcMasterList.OrderBy(x => x.calculatedPosition).ToList();

            foreach (var mP in pcMasterList)
            {
                Console.WriteLine($" { mP.addressPostcode  } has { mP.pList.Count } unique positions - add a value of { mP.calculatedPosition }");
            }
        }

        private void createPostcodeMatrix()
        {
            foreach (var pc in pcMasterList)
            {
                foreach (var m in _manifests)
                {
                    List<int> mPositions = new List<int>();

                    //order list by time attempted
                    m.parcels = m.parcels.OrderBy(x => x.timeAttempted).ToList();

                    int parcelCount = 0;
                    foreach (var p in m.parcels)
                    {
                        //get index of master postcode if exists
                        if (p.pcAbbriev == pc.addressPostcode)
                        {
                            mPositions.Add(parcelCount);
                        }
                        parcelCount++;

                    }

                    // assign value 
                    // take average postion, expand list to 1000, position in manifest = (averagepos/parcel.count)*1000
                    if (mPositions.Count > 0)
                    {
                        double total = mPositions.Sum();
                        int count = mPositions.Count();
                        double averagePos = mPositions.Average();
                        decimal positionValue = (decimal)(averagePos / m.parcels.Count);

                        //add to postiion list for the master postcode
                        pc.pList.Add(Convert.ToInt32(positionValue*5000));
                    }

                }
                pc.setCalcPosition();
            }
            //sort list by calculated position
            pcMasterList.OrderBy(x => x.calculatedPosition);
        }

        //public 
    }
}
