using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using Tools;

namespace FuStudy_Service.Service;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<User> CreateUser(CreateAccountDTORequest createAccountRequest)
    {
        IEnumerable<User> checkEmail =
            await _unitOfWork.UserRepository.GetByFilterAsync(x => x.Email.Equals(createAccountRequest.Email));
        IEnumerable<User> checkUsername =
            await _unitOfWork.UserRepository.GetByFilterAsync(x => x.Username.Equals(createAccountRequest.Username));
        if (checkEmail.Count() != 0)
        {
            throw new InvalidDataException($"Email is exist");
        }

        if (checkUsername.Count() != 0)
        {
            throw new InvalidDataException($"Username is exist");
        }

        var user = _mapper.Map<User>(createAccountRequest);
        user.Password = EncryptPassword.Encrypt(createAccountRequest.Password);
        user.Status = true;
        user.CreateDate = DateTime.Now;
        user.Avatar = null;
        await _unitOfWork.UserRepository.AddAsync(user);
        return user;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _unitOfWork.UserRepository.GetAsync();
    }

    public async Task<User> GetUserById(long id)
    {
        return await _unitOfWork.UserRepository.GetByIdAsync(id);
    }

    public async Task<User> UpdateUser(long id, UpdateAccountDTORequest updateAccountDTORequest)
    {
        var userToUpdate = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (userToUpdate == null)
        {
            throw new InvalidDataException($"User not found");
        }
        _mapper.Map(updateAccountDTORequest, userToUpdate);
        await _unitOfWork.UserRepository.UpdateAsync(userToUpdate);
       
        return userToUpdate;
    }
}