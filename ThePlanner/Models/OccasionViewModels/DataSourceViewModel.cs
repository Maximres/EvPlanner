using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ThePlanner.Models.OccasionViewModels
{
    [DataContract]
    public class DataSourceViewModel
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string startDate { get; set; }

        [DataMember]
        public string endDate { get; set; }

        [DataMember]
        public string location { get; set; }

        [DataMember]
        public ICollection<SimpleInputViewModel> inputs { get; set; }
    }
}