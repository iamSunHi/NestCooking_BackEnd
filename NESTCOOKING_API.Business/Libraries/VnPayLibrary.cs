using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NESTCOOKING_API.Business.DTOs.PaymentDTOs;
using NESTCOOKING_API.Utility;
using Microsoft.Extensions.Configuration;

namespace NESTCOOKING_API.Business.Libraries
{
    public class VnPayLibrary
    {
        private readonly IConfiguration _configuration;
        private readonly SortedList<string, string> _requestData;
        private readonly SortedList<string, string> _responseData;
        private readonly string hashSecret;

        public VnPayLibrary(IConfiguration configuration)
        {
            _configuration = configuration;
            hashSecret = _configuration["VnPay:HashSecret"];
            _requestData = new SortedList<string, string>(new VnPayCompare());
            _responseData = new SortedList<string, string>(new VnPayCompare());
        }

        public PaymentResponse GetFullResponseData(IQueryCollection collection)
        {

            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    AddResponseData(key, value);
                }
            }
            var orderId = GetResponseData("vnp_TxnRef");
            var amount = Convert.ToDouble(GetResponseData("vnp_Amount"));
            var vnPayTranId = Convert.ToInt64(GetResponseData("vnp_TransactionNo"));
            var vnpResponseCode = GetResponseData("vnp_ResponseCode");
            var vnpSecureHash = collection.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value; //hash của dữ liệu trả về
            var orderInfo = GetResponseData("vnp_OrderInfo");
            var vnpTransactionStatus = GetResponseData("vnp_TransactionStatus");
            var checkSignature = ValidateSignature(vnpSecureHash, hashSecret); //check Signature

            if (checkSignature)
            {
                if (vnpResponseCode == "00" && vnpTransactionStatus == "00")
                {
                    return new PaymentResponse()
                    {
                        Amount = amount,
                        Success = true,
                        PaymentMethod = "VnPay",
                        OrderDescription = orderInfo,
                        OrderId = orderId.ToString(),
                        PaymentId = vnPayTranId.ToString(),
                        TransactionId = vnPayTranId.ToString(),
                        Token = vnpSecureHash,
                        VnPayResponseCode = vnpResponseCode
                    };
                }
                else
                {
                    throw new Exception("Payment of errors");
                }
            }
            else
            {
                throw new Exception("Invalid signature");
            }
        }
        public string GetIpAddress(HttpContext context)
        {
            var ipAddress = string.Empty;
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;

                if (remoteIpAddress != null)
                {
                    if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                            .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                    }

                    if (remoteIpAddress != null) ipAddress = remoteIpAddress.ToString();

                    return ipAddress;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return StaticDetails.IpAddress;
        }
        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
        {
            var data = new StringBuilder();

            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            var querystring = data.ToString();

            baseUrl += "?" + querystring;
            var signData = querystring;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }

            var vnpSecureHash = HmacSha512(vnpHashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnpSecureHash;

            return baseUrl;
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var rspRaw = GetResponseData();
            var myChecksum = HmacSha512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string HmacSha512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        private string GetResponseData()
        {
            var data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }

            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }

            foreach (var (key, value) in _responseData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }

            return data.ToString();
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
