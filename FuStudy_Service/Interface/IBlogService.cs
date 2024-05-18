﻿using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Respone;
using FuStudy_Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Service.Interface
{
    public interface IBlogService
    {
        Task<bool> CreateBlog(BlogRequest request);
        Task<bool> DeleteBlog(long id);
        Task<BlogRespone> GetOneBlog(long id);
        Task<bool> UpdateBlog(BlogRequest request);

    }
}
