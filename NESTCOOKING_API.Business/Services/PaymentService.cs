using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NESTCOOKING_API.Business.DTOs.PaymentDTOs;
using NESTCOOKING_API.Business.Libraries;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly VnPayLibrary _vnPayLibrary;
        public PaymentService(IConfiguration configuration,VnPayLibrary vnPayLibrary)
        {
            _configuration = configuration;
            _vnPayLibrary = vnPayLibrary;
        }

        public string CreatePaymentUrl(PaymentInfor model, HttpContext context, string transactionId)
        {
            try
            {
                var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);;
                var urlCallBack = StaticDetails.TransactionFe_URL;

                _vnPayLibrary.AddRequestData("vnp_Version", "2.1.0");
                _vnPayLibrary.AddRequestData("vnp_Command", "pay");
                _vnPayLibrary.AddRequestData("vnp_TmnCode", "BI93KVHO");
                _vnPayLibrary.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
                _vnPayLibrary.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
                _vnPayLibrary.AddRequestData("vnp_CurrCode", "VND");
                _vnPayLibrary.AddRequestData("vnp_IpAddr", _vnPayLibrary.GetIpAddress(context));
                _vnPayLibrary.AddRequestData("vnp_Locale", "vn");
                _vnPayLibrary.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
                _vnPayLibrary.AddRequestData("vnp_OrderType", model.OrderType);
                _vnPayLibrary.AddRequestData("vnp_ReturnUrl", urlCallBack);
                _vnPayLibrary.AddRequestData("vnp_TxnRef", transactionId);

                var paymentUrl = _vnPayLibrary.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", "FJGGVTZYHYLKESPLBSSQLNXYVPUGXFJK");

                return paymentUrl;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }
        public PaymentResponse ProcessPaymentCallback(IQueryCollection collections)
        {
            try
            {
                var response = _vnPayLibrary.GetFullResponseData(collections, "FJGGVTZYHYLKESPLBSSQLNXYVPUGXFJK");

                return response;
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
