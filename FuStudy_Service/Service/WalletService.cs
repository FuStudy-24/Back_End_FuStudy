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
using Tools;

namespace FuStudy_Service.Service
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WalletResponse>> GetAllWalletsAsync(QueryObject queryObject)
        {
            // Check if QueryObject search is not null
            Expression<Func<Wallet, bool>> filter = null;
            if (!string.IsNullOrWhiteSpace(queryObject.Search))
            {
                filter = wallet => wallet.UserId.ToString().Contains(queryObject.Search)
                                || wallet.Balance.ToString().Contains(queryObject.Search)
                                || wallet.Status.ToString().Contains(queryObject.Search);
            }

            var wallets = _unitOfWork.WalletRepository.Get(
                filter: filter,
                pageIndex: queryObject.PageIndex,
                pageSize: queryObject.PageSize);

            if (wallets == null || !wallets.Any())
            {
                throw new CustomException.DataNotFoundException("The wallet list is empty!");
            }

            return _mapper.Map<IEnumerable<WalletResponse>>(wallets);
        }

        public async Task<WalletResponse> GetWalletByIdAsync(long id)
        {
            var wallet = _unitOfWork.WalletRepository.GetByID(id);
            return _mapper.Map<WalletResponse>(wallet);
        }

        public async Task<WalletResponse> GetWalletByUserIdAsync(long userId)
        {
            var wallet = _unitOfWork.WalletRepository.Get(w => w.UserId == userId).FirstOrDefault();
            if (wallet == null)
            {
                throw new CustomException.DataNotFoundException($"Wallet for user ID: {userId} not found");
            }
            return _mapper.Map<WalletResponse>(wallet);
        }

        public async Task<WalletResponse> CreateWalletAsync(WalletRequest walletRequest)
        {
            var wallet = _mapper.Map<Wallet>(walletRequest);
            _unitOfWork.WalletRepository.Insert(wallet);
            _unitOfWork.Save();
            return _mapper.Map<WalletResponse>(wallet);
        }

        public async Task<WalletResponse> UpdateWalletAsync(WalletRequest walletRequest, long walletId)
        {
            var wallet = _unitOfWork.WalletRepository.GetByID(walletId);
            if (wallet == null)
            {
                throw new CustomException.DataNotFoundException($"Wallet with ID: {walletId} not found");
            }

            _mapper.Map(walletRequest, wallet);
            _unitOfWork.WalletRepository.Update(wallet);
            _unitOfWork.Save();
            return _mapper.Map<WalletResponse>(wallet);
        }

        public async Task<bool> DeleteWalletAsync(long walletId)
        {
            var wallet = _unitOfWork.WalletRepository.GetByID(walletId);
            if (wallet == null)
            {
                throw new CustomException.DataNotFoundException($"Wallet with ID: {walletId} not found");
            }

            _unitOfWork.WalletRepository.Delete(wallet);
            _unitOfWork.Save();
            return true;
        }
    }
}

