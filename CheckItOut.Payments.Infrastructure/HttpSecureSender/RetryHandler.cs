﻿using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace CheckItOut.Payments.Infrastructure.HttpSecureSender
{
    public class RetryHandler : DelegatingHandler
    {
        private const int MaxRetries = 3;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            for (int i = 0; i < MaxRetries; i++)
            {
                response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
            }

            return response;
        }
    }
}
