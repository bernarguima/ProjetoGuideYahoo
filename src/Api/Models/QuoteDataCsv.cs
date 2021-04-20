using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class QuoteDataCsv
    {
        public string symbol;
        public double value;
        public DateTime date;
        public double varFirstDay;
        public double varDaily;
    }
}
