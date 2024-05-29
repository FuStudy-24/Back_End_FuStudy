using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
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
        /*Task<ConversationResponse> UpdateConversationAsync(ConversationRequest request);*/
        Task<List<ConversationResponse>> GetConversation();
    }
}
