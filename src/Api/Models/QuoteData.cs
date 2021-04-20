using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class QuoteData
    {
        public string symbol;
        public double value;
        public DateTime date;
        public bool openValue;
        public double varFirstDay;
        public double varDaily;
    }
}
