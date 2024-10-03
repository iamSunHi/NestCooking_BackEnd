using NESTCOOKING_API.Business.DTOs.TransactionDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using System;
using Xunit;

namespace NESTCOOKING_API.Testing.DTOs.TransactionDTOs
{
    public class TransactionDTOValidatorTest
    {
        private readonly TransactionDTOValidator _validator;

        public TransactionDTOValidatorTest()
        {
            _validator = new TransactionDTOValidator();
        }

        [Fact]
        public void Should_Return_True_When_Valid_TransactionDTO()
        {
            var transaction = new TransactionDTO
            {
                Id = "trans123",
                User = new UserShortInfoDTO
                {
                    Id = "user123",
                    UserName = "testuser",
                    FirstName = "John",
                    LastName = "Doe",
                    AvatarUrl = "https://example.com/avatar.jpg"
                },
                Type = "DEPOSIT",
                Amount = 100.00,
                Description = "Test transaction",
                Currency = "USD",
                Payment = "Credit Card",
                IsSuccess = true,
                CreatedAt = DateTime.Now
            };

            var result = _validator.Validate(transaction);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Id_Is_Null_Or_Empty(string id)
        {
            var transaction = new TransactionDTO
            {
                Id = id,
                User = new UserShortInfoDTO { Id = "user123", UserName = "testuser", FirstName = "John", LastName = "Doe" },
                Type = "DEPOSIT",
                Amount = 100.00,
                Description = "Test transaction",
                Currency = "USD",
                Payment = "Credit Card",
                IsSuccess = true,
                CreatedAt = DateTime.Now
            };

            var result = _validator.Validate(transaction);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Should_Return_False_When_UserInfo_Is_Null()
        {
            var transaction = new TransactionDTO
            {
                Id = "trans123",
                User = null,
                Type = "DEPOSIT",
                Amount = 100.00,
                Description = "Test transaction",
                Currency = "USD",
                Payment = "Credit Card",
                IsSuccess = true,
                CreatedAt = DateTime.Now
            };

            var result = _validator.Validate(transaction);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Type_Is_Null_Or_Empty(string type)
        {
            var transaction = new TransactionDTO
            {
                Id = "trans123",
                User = new UserShortInfoDTO { Id = "user123", UserName = "testuser", FirstName = "John", LastName = "Doe" },
                Type = type,
                Amount = 100.00,
                Description = "Test transaction",
                Currency = "USD",
                Payment = "Credit Card",
                IsSuccess = true,
                CreatedAt = DateTime.Now
            };

            var result = _validator.Validate(transaction);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("INVALID_TYPE")]
        public void Should_Return_False_When_Type_Is_Invalid(string type)
        {
            var transaction = new TransactionDTO
            {
                Id = "trans123",
                User = new UserShortInfoDTO { Id = "user123", UserName = "testuser", FirstName = "John", LastName = "Doe" },
                Type = type,
                Amount = 100.00,
                Description = "Test transaction",
                Currency = "USD",
                Payment = "Credit Card",
                IsSuccess = true,
                CreatedAt = DateTime.Now
            };

            var result = _validator.Validate(transaction);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Should_Return_False_When_Amount_Is_Zero()
        {
            var transaction = new TransactionDTO
            {
                Id = "trans123",
                User = new UserShortInfoDTO { Id = "user123", UserName = "testuser", FirstName = "John", LastName = "Doe" },
                Type = "DEPOSIT",
                Amount = 0,
                Description = "Test transaction",
                Currency = "USD",
                Payment = "Credit Card",
                IsSuccess = true,
                CreatedAt = DateTime.Now
            };

            var result = _validator.Validate(transaction);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Description_Is_Null_Or_Empty(string description)
        {
            var transaction = new TransactionDTO
            {
                Id = "trans123",
                User = new UserShortInfoDTO { Id = "user123", UserName = "testuser", FirstName = "John", LastName = "Doe" },
                Type = "DEPOSIT",
                Amount = 100.00,
                Description = description,
                Currency = "USD",
                Payment = "Credit Card",
                IsSuccess = true,
                CreatedAt = DateTime.Now
            };

            var result = _validator.Validate(transaction);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Currency_Is_Null_Or_Empty(string currency)
        {
            var transaction = new TransactionDTO
            {
                Id = "trans123",
                User = new UserShortInfoDTO { Id = "user123", UserName = "testuser", FirstName = "John", LastName = "Doe" },
                Type = "DEPOSIT",
                Amount = 100.00,
                Description = "Test transaction",
                Currency = currency,
                Payment = "Credit Card",
                IsSuccess = true,
                CreatedAt = DateTime.Now
            };

            var result = _validator.Validate(transaction);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_False_When_Payment_Is_Null_Or_Empty(string payment)
        {
            var transaction = new TransactionDTO
            {
                Id = "trans123",
                User = new UserShortInfoDTO { Id = "user123", UserName = "testuser", FirstName = "John", LastName = "Doe" },
                Type = "DEPOSIT",
                Amount = 100.00,
                Description = "Test transaction",
                Currency = "USD",
                Payment = payment,
                IsSuccess = true,
                CreatedAt = DateTime.Now
            };

            var result = _validator.Validate(transaction);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Should_Return_False_When_CreatedAt_Is_Default()
        {
            var transaction = new TransactionDTO
            {
                Id = "trans123",
                User = new UserShortInfoDTO { Id = "user123", UserName = "testuser", FirstName = "John", LastName = "Doe" },
                Type = "DEPOSIT",
                Amount = 100.00,
                Description = "Test transaction",
                Currency = "USD",
                Payment = "Credit Card",
                IsSuccess = true,
                CreatedAt = default
            };

            var result = _validator.Validate(transaction);
            Assert.False(result.IsValid);
        }
    }
}
