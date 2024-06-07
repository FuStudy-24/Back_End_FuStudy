using FuStudy_Model.DTO.Response;

namespace FuStudy_Service.Interface;

public interface IEmailConfig
{
    public ResponseDTO SendEmail(EmailDTO emailDTO);
}