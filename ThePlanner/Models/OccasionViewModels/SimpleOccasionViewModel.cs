using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThePlanner.Models.OccasionViewModels
{
    public class SimpleOccasionViewModel
    {
        public string Topic { get; set; }
        public string Location { get; set; }
        public int Count { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
    }
}