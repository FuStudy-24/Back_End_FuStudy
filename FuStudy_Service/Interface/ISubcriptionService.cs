﻿using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Service.Interface
{
    public interface ISubcriptionService
    {
        Task<SubcriptionResponse> CreateSubcription(CreateSubcriptionRequest subcriptionRequest);
        Task<SubcriptionResponse> DeleteSubcription(long id);
        Task<IEnumerable<Subcription>> GetAllSubcriptions();
        Task<Subcription> GetSubCriptionById(long id);
        Task<SubcriptionResponse> UpdateSubcription(long id, UpdateSubcriptionRequest updateSubcriptionRequest);
    }
}
