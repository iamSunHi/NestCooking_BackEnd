﻿using NESTCOOKING_API.Business.DTOs.CommentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface ICommentService
	{
		Task<IEnumerable<RequestCommentDTO>> GetAllComments();
		Task<RequestCommentDTO> GetCommentById(string commentId);
		Task<RequestCommentDTO> CreateComment(string userId, CreatedCommentDTO createComment);
		Task<RequestCommentDTO> UpdateComment(string commentId, UpdateCommentDTO updateComment);
		Task DeleteComment(string userId, string commentId);
		Task<IEnumerable<RequestCommentDTO>> GetCommentsOfRecipeByRecipeId(string recipeId);
		Task<IEnumerable<RequestCommentDTO>> GetChildCommentsByParentCommentId(string parenCommentId);
		Task <bool> ValidType(string type);
	}
}
