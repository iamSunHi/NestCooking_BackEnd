namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
    public class CreateInstructorDTO
    {
        public string Description { get; set; }
        public IList<string> ImageUrls { get; set; }
        public int InstructorOrder { get; set; }
    };

}
