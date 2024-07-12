using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Tools;

namespace FuStudy_Service.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TransactionResponse>> GetAllTransactionsAsync(QueryObject queryObject)
        {
            Expression<Func<Transaction, bool>> filter = null;
            if (!string.IsNullOrWhiteSpace(queryObject.Search))
            {
                filter = transaction => transaction.Type.Contains(queryObject.Search) ||
                                        transaction.Description.Contains(queryObject.Search) ||
                                        transaction.WalletId.ToString().Contains(queryObject.Search);
            }




            var transactions = _unitOfWork.TransactionRepository.Get(
                filter: filter,
                pageIndex: queryObject.PageIndex,
                pageSize: queryObject.PageSize);

            if (transactions == null || !transactions.Any())
            {
                throw new CustomException.DataNotFoundException("The transaction list is empty!");
            }

            // Map transactions to response and include user information
            var transactionResponses = transactions.Select(transaction =>
            {
                var wallet = _unitOfWork.WalletRepository.Get(w => w.Id == transaction.WalletId).FirstOrDefault();
                var user = wallet != null ? _unitOfWork.UserRepository.Get(u => u.Id == wallet.UserId).FirstOrDefault() : null;

                return new TransactionResponse
                {
                    Id = transaction.Id,
                    WalletId = transaction.WalletId,
                    Type = transaction.Type,
                    Ammount = transaction.Ammount,
                    CreateTime = transaction.CreateTime,
                    Description = transaction.Description,
                    UserId = user?.Id ?? 0,
                    Username = user?.Username,
                    Email = user?.Email,
                    Fullname = user?.Fullname
                };
            });

            return transactionResponses;
        }

        public async Task<TransactionResponse> GetTransactionByIdAsync(long id)
        {
            var transaction = _unitOfWork.TransactionRepository.GetByID(id);
            return _mapper.Map<TransactionResponse>(transaction);
        }

        public async Task<IEnumerable<TransactionResponse>> GetAllTransactionByWalletIdAsync(long walletId)
        {
            var transactions = _unitOfWork.TransactionRepository.Get(x => x.WalletId == walletId);
            if (transactions.IsNullOrEmpty())
            {
                throw new CustomException.DataNotFoundException("The transaction list is empty!");
            }

            return _mapper.Map<IEnumerable<TransactionResponse>>(transactions);
        }

        public async Task<TransactionResponse> CreateTransactionAsync(TransactionRequest transactionRequest)
        {
            var wallet = _unitOfWork.WalletRepository.GetByID(transactionRequest.WalletId);
            if (wallet == null)
            {
                throw new CustomException.DataNotFoundException($"Wallet with ID: {transactionRequest.WalletId} not found!");
            }

            var transaction = _mapper.Map<Transaction>(transactionRequest);
            transaction.CreateTime = DateTime.Now;
            _unitOfWork.TransactionRepository.Insert(transaction);
            _unitOfWork.Save();

            return _mapper.Map<TransactionResponse>(transaction);
        }

        public async Task<TransactionResponse> UpdateTransactionAsync(TransactionRequest transactionRequest, long transactionId)
        {
            var transaction = _unitOfWork.TransactionRepository.GetByID(transactionId);
            if (transaction == null)
            {
                throw new CustomException.DataNotFoundException($"Transaction with ID: {transactionId} not found");
            }

            _mapper.Map(transactionRequest, transaction);
            transaction.CreateTime = DateTime.Now;
            _unitOfWork.TransactionRepository.Update(transaction);
            _unitOfWork.Save();

            return _mapper.Map<TransactionResponse>(transaction);
        }

        public async Task<bool> DeleteTransactionAsync(long transactionId)
        {
            var transaction = _unitOfWork.TransactionRepository.GetByID(transactionId);
            if (transaction == null)
            {
                throw new CustomException.DataNotFoundException($"Transaction with ID: {transactionId} not found");
            }

            _unitOfWork.TransactionRepository.Delete(transaction);
            _unitOfWork.Save();
            return true;
        }
    }
}
