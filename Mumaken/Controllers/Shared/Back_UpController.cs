using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mumaken.Controllers.Shared
{
    public class Back_UpController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment HostingEnvironment;
        public Back_UpController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }
        //public void OnGet()
        //{
        //    ViewData["config"] = 
        //}


        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateBackUp()
        {
            // set fields
            string _folderPath = Path.Combine(HostingEnvironment.WebRootPath, $"BUp");

            string _connectionString = _configuration["ConnectionStrings:DefaultConnection"]; //get the configuration key value.

            var sqlConStrBuilder = new SqlConnectionStringBuilder(_connectionString);

            string fileName = $"{sqlConStrBuilder.InitialCatalog}({DateTime.Now.ToString().Replace(":", "-").Replace("/", "-").Replace(" ", "_")}).bak";

            var backupFileName = Path.Combine(HostingEnvironment.WebRootPath, $"BUp/{fileName}");

            // create BackUp
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
            using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
            {
                // query to BackUp
                var query = $"BACKUP DATABASE [{sqlConStrBuilder.InitialCatalog}] TO DISK='{backupFileName}'";
                using (var command = new SqlCommand(query, connection))
                {
                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();


                        // return to download file
                        return DownloadFile(backupFileName, fileName);
                    }
                    catch (Exception ex)
                    {
                        return NotFound();
                    }
                }
            }
        }

        // Method
        /// [HttpGet]
        public IActionResult DownloadFile(string path, string name)
        {
            // Afterwards file is converted into a stream
            var fs = new FileStream(path, FileMode.Open);

            // Return the file. A byte array can also be used instead of a stream
            return File(fs, "application/octet-stream", name);
        }



    }
}
