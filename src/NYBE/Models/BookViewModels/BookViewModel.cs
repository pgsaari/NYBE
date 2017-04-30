using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NYBE.Models.BookViewModel
{
    public class BookViewModel
    {
        public Book book { get; set; }
        public List<BookListing> forSaleListings { get; set; }
        public List<BookListing> toBuyListings { get; set; }

        //DataContract for Serializing Data - required to serve in JSON format
        [DataContract]
        public class RangeDataPoint
        {
            public RangeDataPoint(string x, double[] y)
            {
                this.X = x;
                this.Y = y;
            }

            //Delimeter |
            public RangeDataPoint(string x)
            {
                this.X = x;
            }

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "x")]
            public string X = null;

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public double[] Y = null;
        }

        //DataContract for Serializing Data - required to serve in JSON format
        [DataContract]
        public class VolumeDataPoint
        {
            public VolumeDataPoint(string x, double y)
            {
                this.X = x;
                this.Y = y;
            }

            //Delimeter |
            public VolumeDataPoint(string x)
            {
                this.X = x;
            }

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "x")]
            public string X = null;

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public Nullable<double> Y = null;
        }
    }
}
