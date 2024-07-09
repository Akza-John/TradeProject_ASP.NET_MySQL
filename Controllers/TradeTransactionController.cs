using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ProjectTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTrade.Controllers
{
    [Route("api/trans/")]
    [ApiController]
    public class TradeTransactionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TradeTransactionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetTradeTransactions()
        {
            try
            {
                List<TradeTransactions> transactions = FetchTradeTransactionsFromDatabase();
                List<object> response = transactions.Select(t => new
                {
                    id = t.Id,
                    tradeStage = t.TradeStage,
                    processStage = t.ProcessStages.Select(p => new
                    {
                        stageName = p.StageName,
                        status = p.Status,
                        cost = p.Cost
                    }).ToList()
                }).ToList<object>();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private List<TradeTransactions> FetchTradeTransactionsFromDatabase()
        {
            List<TradeTransactions> transactions = new List<TradeTransactions>();

            string connectionString = _configuration.GetConnectionString("DBConn");
            string query = "SELECT transId, tradeStage, StageName, Status, Cost FROM tradetransactions";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string id = reader["transId"].ToString();
                    string tradeStage = reader["tradeStage"].ToString();
                    string stageName = reader["StageName"].ToString();
                    string status = reader["Status"].ToString();
                    string cost = reader["Cost"].ToString();

                    TradeTransactions transaction = transactions.FirstOrDefault(t => t.Id == id);
                    if (transaction == null)
                    {
                        transaction = new TradeTransactions
                        {
                            Id = id,
                            TradeStage = tradeStage,
                            ProcessStages = new List<ProcessStage>()
                        };
                        transactions.Add(transaction);
                    }

                    transaction.ProcessStages.Add(new ProcessStage
                    {
                        StageName = stageName,
                        Status = status,
                        Cost = cost
                    });
                }

                reader.Close();
            }

            return transactions;
        }

        [HttpGet("{id}")]
        public IActionResult GetTradeTransactionsWithId(string id)
        {
            try
            {
                List<TradeTransactions> transactions = FetchTradeTransactionsFromDatabaseId(id);
                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound("No trade transactions found for the specified ID.");
                }

                List<object> response = transactions.Select(t => new
                {
                    id = t.Id,
                    tradeStage = t.TradeStage,
                    processStage = t.ProcessStages.Select(p => new
                    {
                        stageName = p.StageName,
                        status = p.Status
                    }).ToList()
                }).ToList<object>();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private List<TradeTransactions> FetchTradeTransactionsFromDatabaseId(string id)
        {
            List<TradeTransactions> transactions = new List<TradeTransactions>();

            string connectionString = _configuration.GetConnectionString("DBConn");

            string query = "SELECT transId, tradeStage, StageName, Status FROM tradetransactions WHERE transId = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string tradeStage = reader["tradeStage"].ToString();
                    string stageName = reader["StageName"].ToString();
                    string status = reader["Status"].ToString();

                    TradeTransactions transaction = transactions.FirstOrDefault(t => t.Id == id);
                    if (transaction == null)
                    {
                        transaction = new TradeTransactions
                        {
                            Id = id,
                            TradeStage = tradeStage,
                            ProcessStages = new List<ProcessStage>()
                        };
                        transactions.Add(transaction);
                    }

                    transaction.ProcessStages.Add(new ProcessStage
                    {
                        StageName = stageName,
                        Status = status
                    });
                }

                reader.Close();
            }

            return transactions;
        }
    } 
  

public class TradeTransactions
    {
        public string Id { get; set; }
        public string TradeStage { get; set; }
        public List<ProcessStage> ProcessStages { get; set; }
    }

    public class ProcessStage
    {
        public string StageName { get; set; }
        public string Status { get; set; }
        public object Cost { get; internal set; }
    }
}