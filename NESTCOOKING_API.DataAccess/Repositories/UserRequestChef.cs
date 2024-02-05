using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class UserRequestChef : IUserRequestChef
	{
		private readonly ApplicationDbContext _context;
	
		public UserRequestChef(ApplicationDbContext context)
		{
			_context = context;
		
		}
		public async Task<RequestToBecomeChef> CreateRequestToBecomeChef(RequestToBecomeChef requestToBecomeChef)
		{
			_context.RequestToBecomeChefs.Add(requestToBecomeChef);
			await _context.SaveChangesAsync();
			return requestToBecomeChef;
		}
		public async Task<bool> DeleteRequestToBecomeChef(string requestId)
		{
			var requestToDelete = await GetRequestById(requestId);

			if (requestToDelete != null)
			{
				_context.RequestToBecomeChefs.Remove(requestToDelete);
				await _context.SaveChangesAsync();
				return true;
			}

			return false; 
		}
		public async Task<IEnumerable<RequestToBecomeChef>> GetAllRequests()
		{
			var requets = await _context.RequestToBecomeChefs.ToListAsync();
			return requets;
		}
		public async Task<RequestToBecomeChef> GetRequestById(string requestId)
		{
			var requestBecomeChef = await _context.RequestToBecomeChefs.FindAsync(requestId);
			return requestBecomeChef;
		}
		public async Task<RequestToBecomeChef> UpdateRequestToBecomeChef(RequestToBecomeChef updatedRequest)
		{
			_context.Update(updatedRequest);
			await _context.SaveChangesAsync();
			return updatedRequest;
		}
	}
}
