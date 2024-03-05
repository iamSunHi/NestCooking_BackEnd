using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs.TransactionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public  interface ITransactionService
    {
        public Task<string> CreateTransaction(TransactionInfor transactionInfor, string userId,bool isSuccess,string payMent);
        public Task<List<TransactionDTO>> GetTransactionsByUserId(string userId);
        public Task<List<TransactionDTO>> GetAllTransactions();
        public Task TransactionSuccessById(string transactionId,bool isSuccess);
        public Task<string> GetTransactionTypeByIdAsync(string transactionId);
    }
}

