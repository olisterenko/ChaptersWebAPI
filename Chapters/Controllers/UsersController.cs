using Chapters.Dto.Requests;
using Chapters.Filters;
using Chapters.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Chapters.Controllers;

[ApiController]
[UserExceptionFilter]
[ValidationExceptionFilter]
[Route("/api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserRequest> _validator;

    public UsersController(IUserService userService, IValidator<CreateUserRequest> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    [HttpPost]
    public async Task SignUp(CreateUserRequest request)
    {
        await _validator.ValidateAndThrowAsync(request);

        await _userService.CreateUser(request);
    }
}