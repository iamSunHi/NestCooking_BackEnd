using AutoMapper;
using Azure;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NESTCOOKING_API.Business.DTOs.AdminDTOs;
using NESTCOOKING_API.Business.DTOs.TransactionDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        public async Task<string> CreateTransaction(TransactionInfor transactionInfor, string userId, string payMent)
        {
            try
            {
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Type = transactionInfor.OrderType,
                    Amount = transactionInfor.Amount,
                    Description = transactionInfor.OrderDescription,
                    Currency = StaticDetails.Currency_VND,
                    Payment = payMent,
                    IsSuccess = false,
                    CreatedAt = DateTime.Now
                };
                await _transactionRepository.CreateAsync(transaction);
                return transaction.Id;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TransactionDTO>> GetTransactionsByUserId(string userId)
        {
            try
            {
                var transactionList = await _transactionRepository.GetAllAsync(r => r.UserId == userId, includeProperties: "User");

                if (transactionList == null)
                {
                    return new List<TransactionDTO>();
                }

                return _mapper.Map<List<TransactionDTO>>(transactionList);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<TransactionDTO>> GetAllTransactions()
        {
            try
            {
                var transactionList = await _transactionRepository.GetAllAsync(includeProperties: "User");
                return _mapper.Map<List<TransactionDTO>>(transactionList);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task TransactionSuccessById(string transactionId, bool isSuccess)
        {
            try
            {
                var trannsactioncheck = await _transactionRepository.GetAsync(t => t.Id == transactionId);
                if (trannsactioncheck.IsSuccess == true)
                {
                    throw new Exception("Duplicated transaction");
                }
                else
                {
                    var transaction = _transactionRepository.UpdateTransactionSuccessAsync(transactionId, isSuccess);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetTransactionTypeByIdAsync(string transactionId)
        {
            try
            {
                var transaction = await _transactionRepository.GetAsync(r => r.Id == transactionId);
                if (transaction == null)
                {
                    throw new Exception("Not found Transaction");
                }
                return transaction.Type;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
