using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostCodeSorter
{
    class Postcode
    {
        public string addressPostcode { get; set; }

        public List<int> pList { get; set; }

        public int calculatedPosition { get; set; }

        public int sDev { get; set; }

        public Postcode(string pc)
        {
            addressPostcode = pc;
            pList = new List<int>();

            sDev = 0;

        }

        public void setCalcPosition()
        {
            //if high std dev then set the cal pos to the median
            if (getStandardDeviation() > 800)
            {
                int size = pList.Count;

                int mid = size / 2;

                double median = (size % 2 != 0) ? (double)pList[mid] : ((double)pList[mid] + (double)pList[mid - 1]) / 2;

                calculatedPosition = Convert.ToInt32(median);
            }
            // else set the pos to the average
            else
            {
                calculatedPosition = (pList.Sum() / pList.Count());
            }

        }

        public int getStandardDeviation()
        {
            int varianceTotal = 0;
            int stdDeviation = 0;

            foreach (var pos in pList)
            {
                varianceTotal += (pos-calculatedPosition) * (pos - calculatedPosition);
            }

            stdDeviation = Convert.ToInt32(Math.Sqrt(varianceTotal / pList.Count));

            return stdDeviation;

        }
    }
}
