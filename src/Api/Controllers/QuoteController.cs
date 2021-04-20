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
using Api.Util;

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
                list = new Utilitario().CalcPercent(list);
                
            }
            catch (Exception ex)
            {
                
            }

            return list;
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
                    DateTime dataInicial = DateTime.Now.Date;

                    double parameterStart = new Utilitario().ConvertToTimestamp(dataInicial);
                    double parameterEnd = new Utilitario().ConvertToTimestamp(dataInicial.AddDays(-60));


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

                        new Utilitario().GetValuesTimestemp(respostaTimestemp);

                        new Utilitario().GetValuesQuote(respostaQuote);

                        TimestamOpenValues finalResult = new TimestamOpenValues();
                        finalResult.listOpen = new Utilitario().GetValuesQuote(respostaQuote);
                        finalResult.listTimestem = new Utilitario().GetValuesTimestemp(respostaTimestemp);


                        DateTime aux = DateTime.Now;
                        for (int i = 0; i <= finalResult.listTimestem.Count - 1; i++)
                        {
                            double? valueOpen = finalResult.listOpen[i];

                            int valueTime = finalResult.listTimestem[i];

                            DateTime date = new Utilitario().TimestamToDate(valueTime);
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

      



    }
}

