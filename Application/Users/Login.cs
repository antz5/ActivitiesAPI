using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.Users
{
    public class Login
    {
        public class Query : IRequest<User>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJWTGenerator _jWTGenerator;

            public Handler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJWTGenerator jWTGenerator)
            {
                _signInManager = signInManager;
                _userManager = userManager;
                _jWTGenerator = jWTGenerator;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if(user == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.Unauthorized);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if(result.Succeeded)
                {
                    var token = _jWTGenerator.CreateToken(user);
                    return new User
                    {
                        DisplayName = user.DisplayName,
                        Token = token,
                        Username = user.UserName,
                        Image = null
                    };
                }

                throw new RestException(HttpStatusCode.Unauthorized);
            }
        }
    }
}