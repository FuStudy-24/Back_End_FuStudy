using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Service.Interface
{
    public interface IConversationService
    {
        Task<ConversationResponse> CreateConversation(ConversationRequest request);
        Conversation CreateConversationWithStudent(long userId, long mentorId);
        /*Task<ConversationResponse> UpdateConversationAsync(ConversationRequest request);*/
        Task<List<ConversationResponse>> GetConversation();
    }
}
