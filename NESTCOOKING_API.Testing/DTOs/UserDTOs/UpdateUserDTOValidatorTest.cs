using FluentValidation;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Testing.DTOs.UserDTOs
{
    public class UpdateUserDTOValidatorTest
    {
        private UpdateUserDTOValidator _validator;
        private UpdateUserDTO _updateUserDTO;

        public UpdateUserDTOValidatorTest()
        {
            _validator = new UpdateUserDTOValidator();
            _updateUserDTO = new UpdateUserDTO
            {
                UserName = "test",
                FirstName = "test",
                LastName = "test",
                IsMale = true,
                Address = "test",
                AvatarUrl = "test"
            };
        }

        [Fact]
        public void Should_Return_True_When_Valid_UpdateUserDTO()
        {
            var result = _validator.Validate(_updateUserDTO);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        public void Should_Return_False_When_UserName_Is_Null(string userName)
        {
            _updateUserDTO.UserName = userName;
            var result = _validator.Validate(_updateUserDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        public void Should_Return_False_When_FirstName_Is_Null(string firstName)
        {
            _updateUserDTO.FirstName = firstName;
            var result = _validator.Validate(_updateUserDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        public void Should_Return_False_When_LastName_Is_Null(string lastName)
        {
            _updateUserDTO.LastName = lastName;
            var result = _validator.Validate(_updateUserDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        public void Should_Return_False_When_Address_Is_Null(string address)
        {
            _updateUserDTO.Address = address;
            var result = _validator.Validate(_updateUserDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        public void Should_Return_False_When_AvatarUrl_Is_Null(string avatarUrl)
        {
            _updateUserDTO.AvatarUrl = avatarUrl;
            var result = _validator.Validate(_updateUserDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.")]
        public void Should_Return_False_When_UserName_Length_Is_More_Than_50(string userName)
        {
            _updateUserDTO.UserName = userName;
            var result = _validator.Validate(_updateUserDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("Lorem Ipsum is simply...")]
        public void Should_Return_True_When_UserName_Length_Is_Less_Than_50(string userName)
        {
            _updateUserDTO.UserName = userName;
            var result = _validator.Validate(_updateUserDTO);
            Assert.True(result.IsValid);
        }

    }
}
