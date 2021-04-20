using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Interface
{
   public interface IQuote
    {
        // Adiciona as cotações
        void AddQuoteCsv(QuoteDataCsv quote);

        void AddQuoteService(QuoteData quote);
        
        List<QuoteDataCsv> GetQuoteCsv();

        List<QuoteData> GetQuoteService();
    }
}
