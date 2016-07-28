using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RigoFunc.Account.Models;
using RigoFunc.Account.Services;

namespace RigoFunc.Account {
    [Route("api/[controller]")]
    public class WeixinController {
        private readonly IAccountService _service;
        private readonly InvokeErrorDescriber _errorDescriber;

        public WeixinController(IAccountService service, InvokeErrorDescriber describer = null) {
            _service = service;
            _errorDescriber = describer ?? new InvokeErrorDescriber();
        }

        [HttpPost("[action]")]
        public async Task<bool> Bind([FromBody]OpenIdBindingModel model) {
            if (model == null) {
                _errorDescriber.BadArgument().Throw();
            }

            return await _service.BindAsync(model);
        }

        [HttpPost("[action]")]
        public async Task<IResponse> Login([FromBody]OpenIdLoginModel model) {
            if (model == null) {
                _errorDescriber.BadArgument().Throw();
            }

            return await _service.LoginAsync(model);
        }
    }
}
