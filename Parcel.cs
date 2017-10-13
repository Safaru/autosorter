using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PostCodeSorter
{
    class Parcel
    {

        public string barcode { get; set; }

        public string addressLine { get; set; }

        public string addressLineFormatted { get; set; }

        public string postcode { get; set; }

        public string pcAbbriev { get; set; }

        public DateTime timeAttempted { get; set; }

        public string identifier { get; set; }

        public int sortPosition { get; set; }

        public int pcStdDeviation { get; set; }

        public Parcel(string csvLine)
        {

            string[] tempElements = csvLine.Split(',');

            barcode = tempElements[2];
            addressLine = tempElements[3];
            addressLineFormatted = tempElements[4];
            postcode = tempElements[5];
            timeAttempted = DateTime.Parse(tempElements[6]);

            string ssAddress = Regex.Replace(addressLineFormatted, "[^a-zA-Z0-9]", "").Substring(0, 6);
            string s = postcode + ssAddress;
            identifier = (Regex.Replace(s, "[^a-zA-Z0-9]", "")).ToLower();

            pcAbbriev = Regex.Replace(postcode, "[^a-zA-Z0-9]", "").ToLower();

            sortPosition = 0;
            pcStdDeviation = 0;
        }
    } 
}