using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PFM.Models;
using PFM.Models.Dtos;
using PFM.Services;

namespace PFM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStudentService _studentService;

        public HomeController(ILogger<HomeController> logger, IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                //var studentToCreate = new Student
                //{
                //    Name = "Shruti"
                //};

                //var createdStudent = await _studentService.PostStudent(studentToCreate);

                var students = await _studentService.GetStudents();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
