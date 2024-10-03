using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Testing.DTOs.RecipeDTOs
{
    public class CreateRecipeDTOValidatorTest
    {
        private CreateRecipeDTOValidator _validator;
        private CreateRecipeDTO _createRecipeDTO;

        public CreateRecipeDTOValidatorTest()
        {
            _validator = new CreateRecipeDTOValidator();
            _createRecipeDTO = new CreateRecipeDTO
            {
                Title = "test",
                Description = "test",
                ThumbnailUrl = "test",
                Difficult = 2,
                CookingTime = 100,
                Portion = 10,
                Categories = new List<int> { 1, 2 },
                Ingredients = new List<CreateIngredientDTO> { new CreateIngredientDTO { Name = "test", Amount = "test" } },
                Instructors = new List<CreateInstructorDTO> { new CreateInstructorDTO { Description = "test", ImageUrls = new List<string> { "test" }, InstructorOrder = 1 } },
                IsAvailableForBooking = true,
                BookingPrice = 100,
                IsPrivate = true,
                RecipePrice = 100,
            };
        }

        [Fact]
        public void Should_Return_True_When_Valid_CreateRecipeDTO()
        {
            var result = _validator.Validate(_createRecipeDTO);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Title_Is_Null_Or_Empty(string title)
        {
            _createRecipeDTO.Title = title;
            var result = _validator.Validate(_createRecipeDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Description_Is_Null_Or_Empty(string description)
        {
            _createRecipeDTO.Description = description;
            var result = _validator.Validate(_createRecipeDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(-1.5)]
        [InlineData(-1)]
        public void Should_Return_False_When_Difficult_Is_Less_Than_1(double difficult)
        {
            _createRecipeDTO.Difficult = difficult;
            var result = _validator.Validate(_createRecipeDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Return_False_When_CookingTime_Is_Less_Than_1(int cookingTime)
        {
            _createRecipeDTO.CookingTime = cookingTime;
            var result = _validator.Validate(_createRecipeDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Return_False_When_Portion_Is_Less_Than_1(int portion)
        {
            _createRecipeDTO.Portion = portion;
            var result = _validator.Validate(_createRecipeDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        public void Should_Return_False_When_Categories_Is_Null(IEnumerable<int> categories)
        {
            _createRecipeDTO.Categories = categories;
            var result = _validator.Validate(_createRecipeDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        public void Should_Return_False_When_Ingredients_Is_Null(IEnumerable<CreateIngredientDTO> ingredients)
        {
            _createRecipeDTO.Ingredients = ingredients;
            var result = _validator.Validate(_createRecipeDTO);
            Assert.False(result.IsValid);
        }
    }
}
