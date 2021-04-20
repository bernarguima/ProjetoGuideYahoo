using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Util
{
    public class Utilitario {

        public List<QuoteData> CalcPercent(List<QuoteData> list)
        {
            List<QuoteData> listReturn = new List<QuoteData>();

            double firstDay = list[0].value;

            int cont = 0;
            double auxVar = 0;
            foreach (QuoteData a in list)
            {
                if (cont == 0)
                {
                    listReturn.Add(a);
                }
                else
                {
                    a.varFirstDay = (((a.value * 100) / firstDay) - 100);
                    a.varDaily = (((a.value * 100) / auxVar) - 100);

                    a.varFirstDay = Convert.ToDouble(decimal.Round(Convert.ToDecimal(a.varFirstDay), 2));
                    a.varDaily = Convert.ToDouble(decimal.Round(Convert.ToDecimal(a.varDaily), 2));

                    listReturn.Add(a);
                }

                auxVar = a.value;
                cont++;
            }


            return listReturn;
        }

        public List<QuoteDataCsv> CalcPercentCsv(List<QuoteDataCsv> list)
        {
            List<QuoteDataCsv> listReturn = new List<QuoteDataCsv>();

            double firstDay = list[0].value;

            int cont = 0;
            double auxVar = 0;
            foreach (QuoteDataCsv a in list)
            {
                if (cont == 0)
                {
                    listReturn.Add(a);
                }
                else
                {
                    a.varFirstDay = (((a.value * 100) / firstDay) - 100);
                    a.varDaily = (((a.value * 100) / auxVar) - 100);

                    a.varFirstDay = Convert.ToDouble(decimal.Round(Convert.ToDecimal(a.varFirstDay), 2));
                    a.varDaily = Convert.ToDouble(decimal.Round(Convert.ToDecimal(a.varDaily), 2));

                    listReturn.Add(a);
                }

                auxVar = a.value;
                cont++;
            }


            return listReturn;
        }
        public List<double?> GetValuesQuote(IEnumerable<List<Quote>> list)
        {
            List<double?> lista = new List<double?>();

            foreach (List<Quote> objQoute in list)
            {
                lista = objQoute[0].open;
            }
            return lista;
        }

        public List<int> GetValuesTimestemp(IEnumerable<List<int>> list)
        {
            List<int> lista = new List<int>();

            foreach (List<int> objQoute in list)
            {
                lista = objQoute;
            }
            return lista;
        }

        public DateTime TimestamToDate(int ValueTime)
        {

            DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

            dateTime = dateTime.AddSeconds(ValueTime).AddHours(-3);

            return dateTime;

        }

        public Double ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (double)span.TotalSeconds;
        }


    }
}
