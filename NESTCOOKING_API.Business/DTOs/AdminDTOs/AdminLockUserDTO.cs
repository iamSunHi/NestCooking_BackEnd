namespace NESTCOOKING_API.Business.DTOs.AdminDTOs
{
	public class AdminLockUserDTO
	{
		public string UserId { get; set; } = null!;
		public int Minute { get; set; }
	}
}
