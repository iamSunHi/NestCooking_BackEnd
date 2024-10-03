using NESTCOOKING_API.Business.DTOs.ReportDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Testing.DTOs.ReportDTOs
{
    public class CreateReportDTOValidatorTest
    {
        private readonly CreateReportDTOValidator _validator;
        private readonly CreateReportDTO _createReportDTO;

        public CreateReportDTOValidatorTest()
        {
            _validator = new CreateReportDTOValidator();
            _createReportDTO = new CreateReportDTO
            {
                TargetId = "123",
                Title = "Test Report",
                Type = "Comment",
                Content = "This is a bug report.",
                ImageUrls = new List<string> { "https://example.com/image1.jpg" }
            };
        }

        [Fact]
        public void Should_Return_True_When_Valid_CreateReportDTO()
        {
            var result = _validator.Validate(_createReportDTO);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Title_Is_Null_Or_Empty(string title)
        {
            _createReportDTO.Title = title;
            var result = _validator.Validate(_createReportDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Type_Is_Null_Or_Empty(string type)
        {
            _createReportDTO.Type = type;
            var result = _validator.Validate(_createReportDTO);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Content_Is_Null_Or_Empty(string content)
        {
            _createReportDTO.Content = content;
            var result = _validator.Validate(_createReportDTO);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Should_Return_True_When_ImageUrls_Is_Null()
        {
            _createReportDTO.ImageUrls = null;
            var result = _validator.Validate(_createReportDTO);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Return_False_When_ImageUrls_Is_Empty()
        {
            _createReportDTO.ImageUrls = new List<string>();
            var result = _validator.Validate(_createReportDTO);
            Assert.False(result.IsValid);
        }
    }
}
