using System.Collections.Generic;
using System.Threading.Tasks;
using PFM.Models.Dtos;

namespace PFM.Services
{
    public interface IStudentService
    {
        Task<IList<Student>> GetStudents();
        Task<Student> PostStudent(Student student);
    }
}