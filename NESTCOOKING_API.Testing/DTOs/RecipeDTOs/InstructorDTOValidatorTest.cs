using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Testing.DTOs.RecipeDTOs
{
    public class InstructorDTOValidatorTest
    {
        private InstructorDTOValidator _validator;
        private InstructorDTO _instructorDTO;
        public InstructorDTOValidatorTest()
        {
            _validator = new InstructorDTOValidator();
            _instructorDTO = new InstructorDTO
            {
                Id = 1,
                Description = "Test",
                InstructorOrder = 1,
            };
        }
        [Fact]
        public void Should_Return_True_When_Valid_InstructorDTO()
        {
            var result = _validator.Validate(_instructorDTO);
            Assert.True(result.IsValid);
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Description_Is_Null_Or_Empty(string description)
        {
            _instructorDTO.Description = description;
            var result = _validator.Validate(_instructorDTO);
            Assert.False(result.IsValid);
        }
        [Theory]
        [InlineData(null)]
        public void Should_Return_False_When_Id_Is_Null_Or_Empty(int id)
        {
            _instructorDTO.Id = id;
            var result = _validator.Validate(_instructorDTO);
            Assert.False(result.IsValid);
        }
        [Theory]
        [InlineData(-1)]
        public void Should_Return_False_When_InstructorOrder_Is_Less_Than_0(int instructorOrder)
        {
            _instructorDTO.InstructorOrder = instructorOrder;
            var result = _validator.Validate(_instructorDTO);
            Assert.False(result.IsValid);
        }
    }
}
