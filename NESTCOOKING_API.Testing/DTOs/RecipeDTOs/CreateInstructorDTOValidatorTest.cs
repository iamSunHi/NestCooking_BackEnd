using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Testing.DTOs.RecipeDTOs
{
    public class CreateInstructorDTOValidatorTest
    {
        private CreateInstructorDTOValidator _validator;
        private CreateInstructorDTO _createInstructorDTO;
        public CreateInstructorDTOValidatorTest() 
        {
            _validator = new CreateInstructorDTOValidator();
            _createInstructorDTO = new CreateInstructorDTO
            {
                Description = "Test",
                InstructorOrder = 1,
            };
        }
        [Fact]
        public void Should_Return_True_When_Valid_CreateInstructorDTO()
        {
            var result = _validator.Validate(_createInstructorDTO);
            Assert.True(result.IsValid);
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Description_Is_Null_Or_Empty(string description)
        {
            _createInstructorDTO.Description = description;
            var result = _validator.Validate(_createInstructorDTO);
            Assert.False(result.IsValid);
        }
        [Theory]
        [InlineData(-1)]
        public void Should_Return_False_When_InstructorOrder_Is_Less_Than_0(int instructorOrder)
        {
            _createInstructorDTO.InstructorOrder = instructorOrder;
            var result = _validator.Validate(_createInstructorDTO);
            Assert.False(result.IsValid);
        }
    }
}
