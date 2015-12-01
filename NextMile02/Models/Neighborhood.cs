using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    public class Neighborhood
    {
        public String neighborhood { get; set; }

        public Neighborhood(string neighborhood)
        {
            this.neighborhood = neighborhood;
        }
    }
}