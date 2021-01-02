using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime Date { get; set; }
            public string City { get; set; }
            public string Venue { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x=>x.Title).NotEmpty();
                RuleFor(x=>x.Description).NotEmpty();
                RuleFor(x=>x.Category).NotEmpty();
                RuleFor(x=>x.Date).NotNull();
                RuleFor(x=>x.City).NotEmpty();
                RuleFor(x=>x.Venue).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = new Activity
                {
                    Category = request.Category,
                    City = request.City,
                    Date = request.Date,
                    Description = request.Description,
                    Id = request.Id,
                    Title = request.Title,
                    Venue = request.Venue
                };

                _context.Activities.Add(activity);

                var user = await _context.Users.SingleOrDefaultAsync(s => s.UserName == _userAccessor.GetCurrentUsername());
                var attendee = new UserActivity
                {
                    AppUser = user,
                    Activity = activity,                    
                    DateJoined = DateTime.UtcNow,
                    IsHost = true
                };

                _context.UserActivities.Add(attendee);
                var success = await _context.SaveChangesAsync() > 0;

                if(success)
                {
                    return Unit.Value;
                }

                throw new Exception($"Problem saving the activity {nameof(activity.Id)}");
            }
        }
    }
}