﻿using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace FuStudy_Service.Service
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<RoleResponse>> GetAllRole(QueryObject queryObject)
        {
            var roles = _unitOfWork.RoleRepository.Get(
                filter: p => p.RoleName.Contains(queryObject.SearchText))
                .ToList();
            if (!roles.Any())
            {
                throw new CustomException.DataNotFoundException("No Role in Database");
            }

            var roleResponses = _mapper.Map<List<RoleResponse>>(roles);

            return roleResponses;
        }

        public async Task<RoleResponse> GetRoledById(long id)
        {
            var role = _unitOfWork.RoleRepository.GetByID(id);

            if (role == null)
            {
                throw new CustomException.DataNotFoundException($"Role not found with ID: {id}");
            }

            var roleResponses = _mapper.Map<RoleResponse>(role);
            return roleResponses;
        }

        public async Task<RoleResponse> CreateRole(RoleRequest roleRequest)
        {
            var existingRole = _unitOfWork.RoleRepository.Get().FirstOrDefault(p => p.RoleName.ToLower() == roleRequest.RoleName.ToLower());

            if (existingRole != null)
            {
                throw new CustomException.DataExistException($"Kind with ColorName '{roleRequest.RoleName}' already exists.");
            }
            var roleResponse = _mapper.Map<RoleResponse>(existingRole);
            var newRole = _mapper.Map<Role>(roleRequest);

            _unitOfWork.RoleRepository.Insert(newRole);
            _unitOfWork.Save();

            _mapper.Map(newRole, roleResponse);
            return roleResponse;
        }

        public async Task<RoleResponse> UpdateRole(long id, RoleRequest roleRequest)
        {
            var existingRole = _unitOfWork.RoleRepository.GetByID(id);

            if (existingRole == null)
            {
                throw new CustomException.DataNotFoundException($"Role with ID {id} not found.");
            }

            var duplicateExists = _unitOfWork.RoleRepository.ExistsAsync(p =>
                p.Id != id &&
                p.RoleName.ToLower() == roleRequest.RoleName.ToLower()
            );

            if (duplicateExists.Equals(true))
            {
                throw new CustomException.DataExistException($"Role with name '{roleRequest.RoleName}' already exists in Data.");
            }

            _mapper.Map(roleRequest, existingRole);
            _unitOfWork.Save();

            var roleResponse = _mapper.Map<RoleResponse>(existingRole);
            return roleResponse;

        }
        public async Task<bool> DeleteRole(long id)
        {
            try
            {
                var role = _unitOfWork.RoleRepository.GetByID(id);
                if (role == null)
                {
                    throw new CustomException.DataNotFoundException("Role not found.");
                }

                _unitOfWork.RoleRepository.Delete(role);
                _unitOfWork.Save();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
