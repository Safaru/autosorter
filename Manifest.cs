using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostCodeSorter
{
    class Manifest
    {
        public int manifestID { get; set; }

        public List<Parcel> parcels { get; set; }

        public List<string> uniquePostcodes { get; set; }

        public Manifest(int mID)
        {
            manifestID = mID;

            parcels = new List<Parcel>();

            uniquePostcodes = new List<string>();
        }

        public void addParcel(string csv)
        {
            parcels.Add(new Parcel(csv));
        }


        public int numberOfDrops()
        {
            int dropsCount = 0;
            List<string> pcaList = new List<string>();

            foreach (var p in parcels)
            {
                if (!pcaList.Contains(p.identifier))
                {
                    pcaList.Add(p.identifier);
                    dropsCount++;
                }
            }

            return pcaList.Count;

        }
    }
}
