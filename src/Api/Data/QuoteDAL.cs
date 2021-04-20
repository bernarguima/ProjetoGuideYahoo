using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Api.Interface;
using Api.Models;
using TesteGuide;

namespace Api.Data
{
    public class QuoteDAL : IQuote
    {
        public static string GetConnectionString()
        {
            return Startup.ConnectionString;
        }
        string connectionString = GetConnectionString();

        public void AddQuoteCsv(QuoteDataCsv values) 
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SPQuoteAddCsv", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CDQuote", values.symbol);
                    cmd.Parameters.AddWithValue("@VLQuote", values.value);
                    cmd.Parameters.AddWithValue("@DTQuote", values.date);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex) { }
         }

        public void AddQuoteService(QuoteData values)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SPQuoteAddService", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CDQuote", values.symbol);
                    cmd.Parameters.AddWithValue("@VLQuote", values.value);
                    cmd.Parameters.AddWithValue("@DTQuote", values.date);
                    cmd.Parameters.AddWithValue("@STOpen", values.openValue);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex) { }
        }
        
        public List<QuoteData> GetQuoteService() 
        {
            List<QuoteData> listReponse = new List<QuoteData>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SPQuoteGetService", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                       
                        con.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                QuoteData quote = new QuoteData();
                                quote.date = Convert.ToDateTime(reader["DTQuote"]);
                                quote.symbol = reader["CDQuote"].ToString();
                                quote.value = Convert.ToDouble(reader["VLQuote"]);
                                quote.openValue = Convert.ToBoolean(reader["STOpen"]);
                                listReponse.Add(quote);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return listReponse;
        }

        public List<QuoteDataCsv> GetQuoteCsv()
        {
            List<QuoteDataCsv> listReponse = new List<QuoteDataCsv>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SPQuoteGetCsv", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        con.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                QuoteDataCsv quote = new QuoteDataCsv();
                                quote.date = Convert.ToDateTime(reader["DTQuote"]);
                                quote.symbol = reader["CDQuote"].ToString();
                                quote.value = Convert.ToDouble(reader["VLQuote"]);
                                listReponse.Add(quote);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return listReponse;
        }

    }

}
