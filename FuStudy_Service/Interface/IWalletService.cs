using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace FuStudy_Service.Interface
{
    public interface IWalletService
    {
        Task<IEnumerable<WalletResponse>> GetAllWalletsAsync(QueryObject queryObject);
        Task<WalletResponse> GetWalletByIdAsync(long id);
        Task<WalletResponse> GetWalletByUserIdAsync(long userId);
        Task<WalletResponse> CreateWalletAsync(WalletRequest walletRequest);
        Task<WalletResponse> UpdateWalletAsync(WalletRequest walletRequest, long walletId);
        Task<bool> DeleteWalletAsync(long walletId);
    }
}