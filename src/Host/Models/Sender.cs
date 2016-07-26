using System;
using System.Threading.Tasks;
using Love.Net.Core;

namespace Host.Models {
    public class Sender : IEmailSender, ISmsSender {
        public string Code { get; set; }
        public Task<InvokeResult> SendEmailAsync(string email, string subject, string message) {
            return Task.FromResult(InvokeResult.Success);
        }

        public Task<InvokeResult> SendSmsAsync(string phoneNumber, string message) {
            Code = message;
            return Task.FromResult(InvokeResult.Success);
        }

        public Task<InvokeResult> SendSmsAsync(string template, string phoneNumber, params Tuple<string, string>[] parameters) {
            Code = parameters[0].Item2;
            return Task.FromResult(InvokeResult.Success);
        }
    }
}
