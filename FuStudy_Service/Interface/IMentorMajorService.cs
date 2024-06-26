using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace FuStudy_Service.Interface
{
    public interface IMentorMajorService
    {
        Task<List<MentorMajorResponse>> GetAllMentorMajor(QueryObject queryObject);
        Task<List<MentorMajorResponse>> GetAllMentorMajorByMentorId(long id);
        Task<List<MentorMajorResponse>> GetAllMentorMajorByMajorId(long id);
        Task<List<MentorMajorResponse>> GetMentorMajorById(long id);
        Task<MentorMajorResponse> CreateMentorMajor(MentorMajorRequest mentorMajorRequest);
        Task<bool> DeleteMentorMajor(long id);
    }
}
