using PFM.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFM.Services
{
    public class StudentService : IStudentService
    {
        private readonly IHttpClientHelper _httpClient;

        public StudentService(HttpClientHelper httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IList<Student>> GetStudents()
        {
            var uri = "api/students";
            return await _httpClient.Get<List<Student>>(uri);
        }

        public async Task<Student> PostStudent(Student student)
        {
            var uri = "api/students";
            return await _httpClient.Post<Student, Student>(uri, student);
        }

    }
}
