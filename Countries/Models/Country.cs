using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;

namespace Countries.Models {
    public class Country {
        public int Id { get; set; }
        public string? Name { set; get; }
        public IList<string>? Capital { set; get; }
        public string? Region { set; get; }
        public IList<string>? Languages { set; get; }    
    }
}
