using Microsoft.AspNetCore.Http;
using NESTCOOKING_API.Business.DTOs.PaymentDTOs;
using NESTCOOKING_API.Business.DTOs.TransactionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IPaymentService
    {
        public string CreatePaymentUrl(TransactionInfor transactionInfor, HttpContext context,string transactionId);
        public PaymentResponse ProcessPaymentCallback(IQueryCollection collections);
    }
}
