using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace FuStudy_Model.DTO.Request;

public class ImageRequest
{
    public required IFormFile Image { get; set; }
}