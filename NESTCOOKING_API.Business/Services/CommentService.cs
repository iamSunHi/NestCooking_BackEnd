using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs.CommentDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services
{
	public class CommentService : ICommentService
	{
		private readonly ICommentRepository _commentRepository;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;
		public CommentService(ICommentRepository commentRepository, UserManager<User> userManager, IMapper mappper)
		{
			_commentRepository = commentRepository;
			_userManager = userManager;
			_mapper = mappper;
		}
		public async Task<RequestCommentDTO> CreateComment(string userId, CreatedCommentDTO createComment)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(userId);
				if (user == null)
				{
					throw new Exception(AppString.SomethingWrongMessage);
				}
				var requestComment = _mapper.Map<Comment>(createComment);
				requestComment.Id = Guid.NewGuid().ToString();
				requestComment.UserId = userId;
				requestComment.CreatedAt = DateTime.Now;
				requestComment.UpdatedAt = DateTime.Now;

				var createdComment = await _commentRepository.CreateComment(requestComment);
				var result = _mapper.Map<RequestCommentDTO>(createdComment);
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(AppString.SomethingWrongMessage);
			}
		}

		public async Task<bool> DeleteComment(string commentId)
		{
			return await _commentRepository.DeleteComment(commentId);
		}

		public async Task<IEnumerable<RequestCommentDTO>> GetAllComments()
		{
			var listComments = await _commentRepository.GetAllComments();
			var result = _mapper.Map<IEnumerable<RequestCommentDTO>>(listComments);
			return result;
		}

		public async Task<RequestCommentDTO> GetCommentById(string commentId)
		{
			var comment = await _commentRepository.GetCommentById(commentId);
			var result = _mapper.Map<RequestCommentDTO>(comment);
			return result;
		}

		public async Task<RequestCommentDTO> UpdateComment(string userId, CreatedCommentDTO updateComment)
		{
			var existtingComment = await _commentRepository.GetCommentById(userId);
			if (existtingComment != null)
			{
				existtingComment.UpdatedAt = DateTime.Now;
				_mapper.Map(updateComment, existtingComment);
				await _commentRepository.UpdateComment(existtingComment);
				var updatedDto = _mapper.Map<RequestCommentDTO>(existtingComment);
				return updatedDto;
			}
			return null;
		}
	}
}
