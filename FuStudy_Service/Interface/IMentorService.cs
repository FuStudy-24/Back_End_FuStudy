

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
    public interface IMentorService
    {
        Task<IEnumerable<MentorResponse>> GetAllMentorVerify(QueryObject queryObject);
        Task<IEnumerable<MentorResponse>> GetAllMentorWaiting(QueryObject queryObject);
        Task<List<MentorResponse>> GetMentorById(long id);
        Task<List<MentorResponse>> GetMentorByUserId(long id);
        Task<MentorResponse> UpdateMentor(long id, MentorRequest mentorRequest);
        Task<MentorResponse> UpdateMentorLoggingIn(MentorRequest mentorRequest);
        Task<MentorResponse> DeleteMentor(long id);
        Task<UpdateMentorOnlineStatusResponse> UpdateOnlineStatus(long id, UpdateMentorOnlineStatusResquest updateMentorOnlineStatusResquest);
        Task<MentorResponse> VerifyMentor(long id);
    }
}
