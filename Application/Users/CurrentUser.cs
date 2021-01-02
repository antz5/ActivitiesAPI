using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class CurrentUser
    {
        public class Query : IRequest<User> { }
        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            private readonly IJWTGenerator _jWTGenerator;

            public Handler(UserManager<AppUser> userManager, IUserAccessor userAccessor, IJWTGenerator jWTGenerator)
            {
                _userManager = userManager;
                _userAccessor = userAccessor;
                _jWTGenerator = jWTGenerator;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                return new User
                {
                    DisplayName = user.DisplayName,
                    Token = _jWTGenerator.CreateToken(user),
                    Username = user.UserName
                };                                
            }
        }
    }
}
