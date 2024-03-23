using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs.ResponseDTOs
{
	public class AdminResponseDTO
    {
        public UserDetailInfoDTO User { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
