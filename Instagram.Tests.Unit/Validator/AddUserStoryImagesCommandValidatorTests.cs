﻿using Instagram.Application.Command;
using Instagram.Application.Validation;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Instagram.Tests.Unit.Validator
{
    public class AddUserStoryImagesCommandValidatorTests
    {
        private readonly AddUserStoryImagesCommandValidator _validator;

        public AddUserStoryImagesCommandValidatorTests()
        {
            _validator = new AddUserStoryImagesCommandValidator();
        }

        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddUserStoryImagesCommand
            {
                Images = new List<IFormFile> { Substitute.For<IFormFile>(), Substitute.For<IFormFile>() },
                UserId = 1
            };

            var result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var command = new AddUserStoryImagesCommand
            {
                Images = new List<IFormFile> { Substitute.For<IFormFile>(), Substitute.For<IFormFile>() },
                UserId = 0
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void EmptyImages_DoesNotPassValidation()
        {
            var command = new AddUserStoryImagesCommand
            {
                Images = new List<IFormFile>(),
                UserId = 1
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }
    }
}
