using Job_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Job_Project.Controllers
{
    public class JobSeekersController : Controller
    {
        public List<Jobs> jobsList;
        IConfiguration configuration;
        public JobSeekersController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult viewJobs()
        {
            jobsList = new List<Jobs>();
            try
            {
                string connString = configuration.GetConnectionString("jobsDB");
                SqlConnection conn = new SqlConnection(connString);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from jobs";
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Jobs job = new Jobs();

                    job.Id = (int)reader["Job_id"];
                    job.Name = (string)reader["Job_name"];
                    job.employeerName = (string)reader["job_employeer_name"];
                    job.category = (string)reader["job_category"];
                    job.Description = (string)reader["job_Description"];

                    jobsList.Add(job);

                    
                }
                reader.Close();
               
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            ViewData["jobsList"] = jobsList;
            return View();
        }
    }
}
