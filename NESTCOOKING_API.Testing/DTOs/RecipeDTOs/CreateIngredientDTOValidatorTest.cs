using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Testing.DTOs.RecipeDTOs
{
    public class CreateIngredientDTOValidatorTest
    {
        private CreateIngredientDTOValidator _validator;
        private CreateIngredientDTO _createIngredientDTO;
        public CreateIngredientDTOValidatorTest()
        {
           _validator = new CreateIngredientDTOValidator();
            _createIngredientDTO = new CreateIngredientDTO
            {
                Amount = "Test",
                Name = "Test",
                IngredientTipId = null
            };
        }
        [Fact]
        public void Should_Return_True_When_Valid_CreateIngredientDTO()
        {
            var result = _validator.Validate(_createIngredientDTO);
            Assert.True(result.IsValid);
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Amount_Is_Null_Or_Empty(string amount)
        {
            _createIngredientDTO.Amount = amount;
            var result = _validator.Validate(_createIngredientDTO);
            Assert.False(result.IsValid);
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Name_Is_Null_Or_Empty(string name)
        {
            _createIngredientDTO.Name = name;
            var result = _validator.Validate(_createIngredientDTO);
            Assert.False(result.IsValid);
        }
    }
}
