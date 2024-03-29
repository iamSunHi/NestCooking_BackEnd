﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs.CommentDTOs;
using NESTCOOKING_API.Business.DTOs.NotificationDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Business.Services
{
	public class CommentService : ICommentService
	{
		private readonly ICommentRepository _commentRepository;
		private readonly IRecipeRepository _recipeRepository;
		private readonly UserManager<User> _userManager;
		private readonly INotificationService _notificationService;
		private readonly IMapper _mapper;
		public CommentService(ICommentRepository commentRepository, IRecipeRepository recipeRepository, UserManager<User> userManager, INotificationService notificationService, IMapper mappper)
		{
			_commentRepository = commentRepository;
			_recipeRepository = recipeRepository;
			_userManager = userManager;
			_notificationService = notificationService;
			_mapper = mappper;
		}
		public async Task<RequestCommentDTO> CreateComment(string userId, CreatedCommentDTO createComment)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new Exception(AppString.SomethingWrongMessage);
			}
			if (createComment.ParentCommentId != null && createComment.Type.Equals(StaticDetails.CommentType_RECIPE))
			{
				throw new Exception(AppString.CommentFail);
			}
			if (createComment.ParentCommentId == null && createComment.Type.Equals(StaticDetails.CommentType_COMMENTCHILD))
			{
				throw new Exception(AppString.CommentFail);
			}
			var requestComment = _mapper.Map<Comment>(createComment);
			requestComment.CommentId = Guid.NewGuid().ToString();
			requestComment.UserId = userId;
			requestComment.CreatedAt = DateTime.UtcNow.AddHours(7);
			requestComment.UpdatedAt = DateTime.UtcNow.AddHours(7);
			await _commentRepository.CreateAsync(requestComment);

			var notificationCreateDTO = new NotificationCreateDTO()
			{
				SenderId = userId,
				ReceiverId = requestComment.CommentId,
				NotificationType = StaticDetails.NotificationType_COMMENT,
				TargetType = createComment.Type,
			};
			await _notificationService.CreateNotificationAsync(notificationCreateDTO);

			var result = await GetCommentById(requestComment.CommentId);
			return result;
		}
		public async Task DeleteComment(string userId, string commentId)
		{
			try
			{
				var commentFromDb = await _commentRepository.GetAsync(comment => comment.CommentId == commentId);
				if (commentFromDb == null)
				{
					throw new Exception(AppString.RequestCommentNotFound);
				}
				if (commentFromDb.UserId != userId)
				{
					throw new Exception(AppString.DeleteCommentNotOwner);
				}

				var childCommentsFromDb = await _commentRepository.GetAllAsync(comment => comment.ParentCommentId == commentId);
				foreach (var childComment in childCommentsFromDb)
				{
					await _commentRepository.RemoveAsync(childComment);
				}

				await _commentRepository.RemoveAsync(commentFromDb);
			}
			catch (Exception ex)
			{
				if (ex.InnerException == null)
				{
					throw;
				}
				throw new Exception(ex.InnerException.Message);
			}
		}
		public async Task<IEnumerable<RequestCommentDTO>> GetAllComments()
		{
			var result = _mapper.Map<IEnumerable<RequestCommentDTO>>(await _commentRepository.GetAllAsync());
			return result;
		}
		public async Task<IEnumerable<RequestCommentDTO>> GetChildCommentsByParentCommentId(string parentCommentId)
		{
			var result = _mapper.Map<IEnumerable<RequestCommentDTO>>(await _commentRepository.GetChildCommentsByParentCommentId(parentCommentId));
			return result;
		}
		public async Task<IEnumerable<RequestCommentDTO>> GetCommentsOfRecipeByRecipeId(string recipeId)
		{
			var result = _mapper.Map<IEnumerable<RequestCommentDTO>>(await _commentRepository.GetAllCommentsOfRecipeByRecipeId(recipeId));
			return result;
		}
		public async Task<RequestCommentDTO> GetCommentById(string commentId)
		{
			var result = _mapper.Map<RequestCommentDTO>(await _commentRepository.GetAsync(cmt => cmt.CommentId == commentId));
			return result;
		}
		public async Task<RequestCommentDTO> UpdateComment(string commentId, UpdateCommentDTO updateComment)
		{
			var existtingComment = await _commentRepository.GetAsync(cmt => cmt.CommentId == commentId);
			var user = await _userManager.FindByIdAsync(existtingComment.UserId);
			if (existtingComment == null || user == null)
			{
				throw new Exception(AppString.RequestCommentNotFound);
			}
			if (existtingComment != null && existtingComment.UserId.Equals(user.Id))
			{
				existtingComment.UpdatedAt = DateTime.UtcNow.AddHours(7);
				_mapper.Map(updateComment, existtingComment);
				await _commentRepository.UpdateComment(existtingComment);
				var updatedDto = _mapper.Map<RequestCommentDTO>(existtingComment);
				return updatedDto;
			}
			return null;
		}
		public Task<bool> ValidType(string type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type), AppString.TypeCommentNull);
			}
			try
			{
				bool isValidType = string.Equals(type, StaticDetails.CommentType_RECIPE, StringComparison.OrdinalIgnoreCase) ||
								   string.Equals(type, StaticDetails.CommentType_COMMENTCHILD, StringComparison.OrdinalIgnoreCase);
				return Task.FromResult(isValidType);
			}
			catch (Exception ex)
			{
				throw new Exception(AppString.SomethingWrongMessage, ex);
			}
		}


	}
}
