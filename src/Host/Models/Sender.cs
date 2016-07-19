using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Love.Net.Services;

namespace Host.Models {
    public class Sender : IEmailSender, ISmsSender {
        public Task SendEmailAsync(string email, string subject, string message) {
            return Task.FromResult("");
        }

        public Task<SendSmsResult> SendSmsAsync(string phoneNumber, string message) {
            return Task.FromResult(new SendSmsResult() { IsSuccessSend = true });
        }

        public Task<SendSmsResult> SendSmsAsync(string template, string phoneNumber, params Tuple<string, string>[] parameters) {
            return Task.FromResult(new SendSmsResult() { IsSuccessSend = true });
        }
    }
}
