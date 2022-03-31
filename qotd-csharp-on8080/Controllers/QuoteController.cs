using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace qotd_csharp.Controllers
{
    [ApiController]
    [Route("/")]
    public class QuoteController : ControllerBase
    {

        private readonly ILogger<QuoteController> _logger;


        public QuoteController(ILogger<QuoteController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Index() {
            return "qotd microservice. Check out the code at: https://github.com/donschenck/qotd-csharp";
        }

        [HttpGet("quotes")]
        public List<Quote> AllQuotes() {
            // Get all quotes from database

            return GetQuotesAsync().Result;
        }
                

        [HttpGet("quotes/{quoteId}")]
        public Quote OneQuote(int quoteId) {
            return GetQuoteByIdAsync(quoteId).Result;
        }

        [HttpGet("quotes/random")]
        public Quote Random() {
            return GetRandomQuoteAsync().Result;
        }

        [HttpGet("version")]
        public string Version() {
                 //return "5.0.0";
                    return "OSDescription-"+System.Runtime.InteropServices.RuntimeInformation.OSDescription+ " OSArchitecture-" + System.Runtime.InteropServices.RuntimeInformation.OSArchitecture + " FrameworkDescription-"+ System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        }

        [HttpGet("writtenin")]
        public string WrittenIn() {
            return "C#";
        }

        private async Task<List<Quote>> GetQuotesAsync() {
            // Get all quotes from database
            string sqlStatement = "SELECT id, quotation, author FROM quotes ORDER BY id";
            string MySqlConnectionString = @"Server=tcp:sqldbservices07.database.windows.net,1433;Initial Catalog=Quote;Persist Security Info=False;User ID=tutul;Password=Welcome@2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using var connection = new SqlConnection(MySqlConnectionString);
            await connection.OpenAsync();

            var quoteList = new List<Quote>();

            using var command = new SqlCommand(sqlStatement, connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var q = new Quote();
                q.Quotation = reader["quotation"].ToString();
                q.Author = reader["author"].ToString();
                q.Id = Convert.ToInt32(reader["Id"]);

                quoteList.Add(q);           
             }
            return quoteList;
        }

        private async Task<Quote> GetQuoteByIdAsync(int theId) {
            // Get one quote from database
            string sqlStatement = String.Format("SELECT id, quotation, author FROM quotes WHERE id = {0}",theId);
            string MySqlConnectionString = @"Server=tcp:sqldbservices07.database.windows.net,1433;Initial Catalog=Quote;Persist Security Info=False;User ID=tutul;Password=Welcome@2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using var connection = new SqlConnection(MySqlConnectionString);
            await connection.OpenAsync();

            var quoteList = new List<Quote>();

            using var command = new SqlCommand(sqlStatement, connection);
            using var reader = await command.ExecuteReaderAsync();
            var q = new Quote();
            while (await reader.ReadAsync())
            {
                q.Quotation = reader["quotation"].ToString();
                q.Author = reader["author"].ToString();
                q.Id = Convert.ToInt32(reader["Id"]);
             }
            return q;
        }

        private async Task<Quote> GetRandomQuoteAsync() {
            // Get one RANDOM quote from database
            string sqlStatement = String.Format("SELECT id, quotation, author FROM quotes ORDER BY RAND() LIMIT 1");
            string MySqlConnectionString = @"Server=tcp:sqldbservices07.database.windows.net,1433;Initial Catalog=Quote;Persist Security Info=False;User ID=tutul;Password=Welcome@2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using var connection = new SqlConnection(MySqlConnectionString);
            await connection.OpenAsync();

            var quoteList = new List<Quote>();

            using var command = new SqlCommand(sqlStatement, connection);
            using var reader = await command.ExecuteReaderAsync();
            var q = new Quote();
            while (await reader.ReadAsync())
            {
                q.Quotation = reader["quotation"].ToString();
                q.Author = reader["author"].ToString();
                q.Id = Convert.ToInt32(reader["Id"]);
             }
            return q;
        }
    }
}
