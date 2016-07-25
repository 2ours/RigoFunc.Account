using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RigoFunc.Account.Services {
    public interface IAccessTokenProvider {
        Task<IResponse> RequestTokenAsync(string userName, string password);
    }
}
