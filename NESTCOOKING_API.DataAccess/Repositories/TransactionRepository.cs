using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace NESTCOOKING_API.DataAccess.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {

        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task UpdateAsync(string transactionId)
        {
            try
            {
                var transaction = _context.Transactions.FirstOrDefault(t => t.Id == transactionId);
                transaction.IsSuccess = true;
                _context.SaveChanges();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }
    }
}
