using AutoMapper;
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
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TransactionResponse>> GetAllTransactionsAsync()
        {
            var transactions = _unitOfWork.TransactionRepository.Get();
            return _mapper.Map<IEnumerable<TransactionResponse>>(transactions);
        }

        public async Task<TransactionResponse> GetTransactionByIdAsync(long id)
        {
            var transaction = _unitOfWork.TransactionRepository.GetByID(id);
            return _mapper.Map<TransactionResponse>(transaction);
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
