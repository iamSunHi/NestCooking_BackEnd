namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class InstructorDTO
	{
		public int Id { get; set; }
		public string Description { get; set; } = null!;
		public List<string>? ImageUrls { get; set; }
		public int InstructorOrder { get; set; }
	}
}
