using FuStudy_Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Service.Interfaces
{
    public interface IStudentSubcriptionService
    {
        Task<IEnumerable<StudentSubcription>> GetAllStudentSubcription();
    }
}
