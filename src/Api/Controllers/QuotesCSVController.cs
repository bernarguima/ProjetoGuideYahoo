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
using System.Linq;
using Api.Data;
using System.Net;
using Api.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers
{

    
    [ApiController]
    public class QuotesCSVController : Controller
    {
        private IHostingEnvironment hostingEnv;
        private readonly IQuote _quote;
        public QuotesCSVController(IQuote quoteI, IHostingEnvironment env)
        {
            _quote = quoteI;
            this.hostingEnv = env;
        }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("api/GetQuoteServiceCsv")]
        public async Task<List<QuoteDataCsv>> GetQuoteService()
        {
            List<QuoteDataCsv> list = new List<QuoteDataCsv>();
            try
            {
                list = _quote.GetQuoteCsv();
                list = list.OrderBy(s => s.date).ToList();
                CalcPercent(list);
                
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
        [Route("api/GetQuoteCsvYahoo")]
        public async Task<IActionResult> Index()
        {

            List<QuoteDataCsv> list = _quote.GetQuoteCsv();

                try
                {
                    if(list.Count==0)
                    { 
                    DownloadYahooCsv();
                    }
                return Ok("Processamento Concluído");
                    
                }
           
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString()); ;
                }
           
        }

        List<QuoteDataCsv> CalcPercent(List<QuoteDataCsv> list)
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
        void DownloadYahooCsv() 
        {

            DateTime dataInicial = DateTime.Now.Date.AddDays(-1);

            double parameterStart = ConvertToTimestamp(dataInicial);
            double parameterEnd = ConvertToTimestamp(dataInicial.AddDays(-60));

            string myWebUrlFile = "https://query1.finance.yahoo.com/v7/finance/download/PETR4.SA?period1=" + parameterEnd + "&period2=" + parameterStart + "&interval=1d&events=history&includeAdjustedClose=true";
           
            var FileDic = "CSVFile";

            string FilePath = Path.Combine(hostingEnv.ContentRootPath, FileDic);

            if (!Directory.Exists(FilePath))

                Directory.CreateDirectory(FilePath);

            using (var client = new WebClient())
            {
                FilePath = FilePath + "\\Petr4.csv";
                client.DownloadFile(myWebUrlFile, FilePath);
            }

            CarregaArquivoCsv(FilePath);

        }

        void CarregaArquivoCsv(string myLocalFilePath) 
        {
            int cont = 0;
            using (var reader = new StreamReader(myLocalFilePath))
            {
                string data = null;
                string openValue = null;
                List<string> listB = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (cont > 0)
                    {
                        var values = line.Split(',');
                        if (!string.IsNullOrEmpty(values[0].ToString()) &&
                            values[0].ToString() != "null")
                        {
                            data = values[0].ToString();
                        }
                        if (!string.IsNullOrEmpty(values[1].ToString()) &&
                            values[1].ToString() != "null")
                        {
                            openValue = values[1].ToString().Replace(".", ","); ;
                        }
                        if (!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(openValue))
                        {
                            QuoteDataCsv quote = new QuoteDataCsv();
                            quote.symbol = "PETR4";
                            quote.date = Convert.ToDateTime(data);
                            quote.value = Convert.ToDouble(openValue);

                            _quote.AddQuoteCsv(quote);

                        }
                    }

                    cont++;
                }

            }
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

