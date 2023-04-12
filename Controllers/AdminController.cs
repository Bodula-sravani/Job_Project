using Job_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Job_Project.Controllers
{
    public class AdminController : Controller
    {
        public List<Jobs> jobsList;
        IConfiguration configuration;
        public AdminController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult ViewJob()
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


        public IActionResult AddJob()
        {

            return View();

        }

        [HttpPost]
        public ActionResult createJob()
        {
            Console.WriteLine("create job method");
            Jobs job = new Jobs();
            job.Name = (string)Request.Form["Name"];
            job.category = (string)Request.Form["category"];
            job.employeerName = (string)Request.Form["Employeer Name"];
            job.Description = (string)Request.Form["Description"];
            try
            {
                string connString = configuration.GetConnectionString("jobsDB");
                SqlConnection conn = new SqlConnection(connString);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = $"insert into jobs(JOB_NAME,JOB_DESCRIPTION,JOB_CATEGORY,JOB_EMPLOYEER_NAME) values('{job.Name}','{job.Description}','{job.category}','{job.employeerName}')";
                cmd.ExecuteNonQuery();
            }
            catch(SqlException e)
            {
                Console.WriteLine(e.Message);
            }

            return View();
             
        }

    
        public IActionResult UpdateJob(int id)
        {
            Console.WriteLine("update method id: "+id);

            string connString = configuration.GetConnectionString("jobsDB");
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"select * from jobs where job_id = {id}";
            var reader = cmd.ExecuteReader();
            Jobs job = new Jobs();
            while (reader.Read())
            {

                job.Id = (int)reader["Job_id"];
                job.Name = (string)reader["Job_name"];
                job.employeerName = (string)reader["job_employeer_name"];
                job.category = (string)reader["job_category"];
                job.Description = (string)reader["job_Description"];
            }
            ViewData["job"] = job;
            return View();
        }

        [HttpPost]
        public IActionResult UpdateConfirm()
        {

            Jobs job = new Jobs();
            job.Name = (string)Request.Form["Name"];
            job.category = (string)Request.Form["category"];
            job.employeerName = (string)Request.Form["Employeer Name"];
            job.Description = (string)Request.Form["Description"];

            Console.WriteLine(job.Description);
            return View();
        }
        public IActionResult DeleteJob(int id)
        {
            string connString = configuration.GetConnectionString("jobsDB");
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"select * from jobs where job_id = {id}";
            var reader = cmd.ExecuteReader();
            Jobs job = new Jobs();
            while (reader.Read())
            {

                job.Id = (int)reader["Job_id"];
                job.Name = (string)reader["Job_name"];
                job.employeerName = (string)reader["job_employeer_name"];
                job.category = (string)reader["job_category"];
                job.Description = (string)reader["job_Description"];
            }
            ViewData["job"] = job;

            return View();
        }
        }
    }

