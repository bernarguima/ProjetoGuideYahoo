using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Pre
    {
        public string timezone { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public int gmtoffset { get; set; }
    }

    public class Regular
    {
        public string timezone { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public int gmtoffset { get; set; }
    }

    public class Post
    {
        public string timezone { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public int gmtoffset { get; set; }
    }

    public class CurrentTradingPeriod
    {
        public Pre pre { get; set; }
        public Regular regular { get; set; }
        public Post post { get; set; }
    }

    public class Meta
    {
 
        public string symbol { get; set; }

    }

    public class Quote
    {
        [JsonIgnore]
        public List<double?> low { get; set; }
        [JsonIgnore]
        public List<int?> volume { get; set; }
        [JsonIgnore]
        public List<double?> close { get; set; }
        [JsonIgnore]
        public List<double?> high { get; set; }
        public List<double?> open { get; set; }
    }

    public class Indicators
    {
        public List<Quote> quote { get; set; }
    }

    public class Result
    {
        public Meta meta { get; set; }
        public List<int> timestamp { get; set; }
        public Indicators indicators { get; set; }
    }

    public class Chart
    {
        public List<Result> result { get; set; }
        
    }

    public class Finance
    {
        public Chart chart { get; set; }
    }

    public class TimestamOpenValues 
    {
        public List<double?> listOpen { get; set; }
        public List<int> listTimestem { get; set; }
        public string symbol { get; set; }
        public DateTime dtQuote { get; set; }
    }
}
