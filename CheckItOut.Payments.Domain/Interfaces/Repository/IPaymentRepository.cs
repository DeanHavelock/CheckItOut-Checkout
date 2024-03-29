﻿using System;
using System.Threading.Tasks;

namespace CheckItOut.Payments.Domain.Interfaces.Repository
{
    public interface IPaymentRepository
    {
        Task Add(Payment payment);
        Task Save();
        Task<Payment> GetById(string paymentId);
        Task<Payment> GetByInvoiceId(string invoiceId);
    }
}
