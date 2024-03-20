using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NESTCOOKING_API.Business.DTOs.PaymentDTOs;
using NESTCOOKING_API.Business.DTOs.TransactionDTOs;
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
		private readonly VnPayLibrary _vnPayLibrary;
		public PaymentService(VnPayLibrary vnPayLibrary)
		{
			_vnPayLibrary = vnPayLibrary;
		}

		public string CreatePaymentUrl(TransactionInfor transactionInfor, HttpContext context, string transactionId)
		{
			try
			{
				//var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
				//var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now, timeZoneInfo);
				var timeNow = DateTime.Now;
				var urlCallBack = StaticDetails.FE_URL + "/payment/callback";

				_vnPayLibrary.AddRequestData("vnp_Version", "2.1.0");
				_vnPayLibrary.AddRequestData("vnp_Command", "pay");
				_vnPayLibrary.AddRequestData("vnp_TmnCode", "BI93KVHO");
				_vnPayLibrary.AddRequestData("vnp_Amount", ((int)transactionInfor.Amount * 100).ToString());
				_vnPayLibrary.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
				_vnPayLibrary.AddRequestData("vnp_CurrCode", "VND");
				_vnPayLibrary.AddRequestData("vnp_IpAddr", "123.32.42.53");
				_vnPayLibrary.AddRequestData("vnp_Locale", "vn");
				_vnPayLibrary.AddRequestData("vnp_OrderInfo", $"{transactionInfor.Name} {transactionInfor.OrderDescription} {transactionInfor.Amount}");
				_vnPayLibrary.AddRequestData("vnp_OrderType", transactionInfor.OrderType);
				_vnPayLibrary.AddRequestData("vnp_ReturnUrl", urlCallBack);
				_vnPayLibrary.AddRequestData("vnp_TxnRef", transactionId);

				var paymentUrl = _vnPayLibrary.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", "FJGGVTZYHYLKESPLBSSQLNXYVPUGXFJK");

				return paymentUrl;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}
		public PaymentResponse ProcessPaymentCallback(IQueryCollection collections)
		{
			try
			{
				var response = _vnPayLibrary.GetFullResponseData(collections);

				return response;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}
	}
}
