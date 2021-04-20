using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http.Headers;
using Api.Interface;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers
{
    
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IQuote _quote;
        public QuoteController(IQuote quoteI)
        {
            _quote = quoteI;
        }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("api/GetQuoteService")]
        public async Task<List<QuoteData>> GetQuoteService()
        {
            List<QuoteData> list = new List<QuoteData>();
            try
            {
                list = _quote.GetQuoteService();
                list = list.OrderBy(s => s.date).ToList();
                CalcPercent(list);
                
            }
            catch (Exception ex)
            {
                
            }

            return list;
        }

        List<QuoteData> CalcPercent(List<QuoteData> list) 
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


        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("api/GetQuoteYahoo")]
        public async Task<IActionResult> GetQuoteList()
        {
            // Get Quotes of the https://query2.finance.yahoo.com/v8/finance/chart/PETR4.SA


            List<QuoteData> list = _quote.GetQuoteService();

            try
            {

                if (list.Count == 0)
                {
                    DateTime dataInicial = DateTime.Now.Date.AddDays(-1);

                    double parameterStart = ConvertToTimestamp(dataInicial);
                    double parameterEnd = ConvertToTimestamp(dataInicial.AddDays(-60));


                    //List<QuoteData> list = _quote.GetQuoteService();

               
                    using (var client = new HttpClient())
                    {

                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage resposta = await client.GetAsync("https://query2.finance.yahoo.com/v8/finance/chart/PETR4.SA?period1="+ parameterEnd +"&period2="+ parameterStart +"&interval=1d&events=history");

                        var json = await resposta.Content.ReadAsStringAsync();

                        var respostaResult = JsonConvert.DeserializeObject<Finance>(json.ToString());

                        var respotaTicker = respostaResult.chart.result.Select(o => o.meta.symbol).ToList();

                        var periodo = JsonConvert.DeserializeObject<Post>(json.ToString());

                        var respostaTimestemp = respostaResult.chart.result.Select(x => x.timestamp);

                        var respostaQuote = respostaResult.chart.result.Select(x => x.indicators.quote);

                        GetValuesTimestemp(respostaTimestemp);

                        GetValuesQuote(respostaQuote);

                        TimestamOpenValues finalResult = new TimestamOpenValues();
                        finalResult.listOpen = GetValuesQuote(respostaQuote);
                        finalResult.listTimestem = GetValuesTimestemp(respostaTimestemp);


                        DateTime aux = DateTime.Now;
                        for (int i = 0; i <= finalResult.listTimestem.Count - 1; i++)
                        {
                            double? valueOpen = finalResult.listOpen[i];

                            int valueTime = finalResult.listTimestem[i];

                            DateTime date = TimestamToDate(valueTime);
                            try
                            {
                                QuoteData quotedata = new QuoteData();
                                quotedata.date = Convert.ToDateTime(date);
                                quotedata.value = Convert.ToDouble(valueOpen);
                                quotedata.symbol = respotaTicker[0].ToString();
                                quotedata.openValue = true;

                                _quote.AddQuoteService(quotedata);


                            }
                            catch (Exception ex)
                            {

                            }

                        }
                        }
                    }
                
                


                return Ok("Processamento Concluído");
            }

            catch (Exception ex) 
            {
                return BadRequest(ex.ToString());
            }
        }

        List<double?> GetValuesQuote(IEnumerable<List<Quote>> list) 
        {
            List<double?> lista = new List<double?>();
            
            foreach (List<Quote> objQoute in list)
            {
                lista = objQoute[0].open;
            }
            return lista;
        }

        List<int> GetValuesTimestemp(IEnumerable<List<int>> list)
        {
            List<int> lista = new List<int>();

            foreach (List<int> objQoute in list)
            {
                lista = objQoute;
            }
            return lista;
        }

        DateTime TimestamToDate(int ValueTime )
            {

            DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

            dateTime = dateTime.AddSeconds(ValueTime).AddHours(-3);

            return dateTime;

        }

        Double ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (double)span.TotalSeconds;
        }



    }
}

