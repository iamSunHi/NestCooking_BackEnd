using AutoMapper;
using Azure;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<string> CreateTransaction(TransactionInfor transactionInfor, string userId, bool isSuccess, string payMent)
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
                    IsSuccess = isSuccess,
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
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task TransactionSuccessById(string transactionId)
        {
            try
            {
                var transaction = _transactionRepository.UpdateTransactionSuccessAsync(transactionId);
                
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
