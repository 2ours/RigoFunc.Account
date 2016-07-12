﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RigoFunc.ApiCore.Services;

namespace RigoFunc.Account.UnitTests.Supports {
    public class Sender : IEmailSender, ISmsSender {
        public Task SendEmailAsync(string email, string subject, string message) {
            return null;
        }

        public Task<SendSmsResult> SendSmsAsnyc(string phoneNumber, string message) {
            return null;
        }

        public Task<SendSmsResult> SendSmsAsync(string template, string phoneNumber, params Tuple<string, string>[] parameters) {
            return null;
        }
    }
}