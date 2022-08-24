using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace penalty__calculator__final__project.Models
{
    public class Country
    {
        public int Country_ID {get; set ;}
        public string Country_Name { get; set; }
        public string Currency { get; set; }
        public float Tax { get; set; }
        public float Penalty_Amount { get; set; }

    }
}