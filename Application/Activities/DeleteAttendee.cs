using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class DeleteAttendee
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
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
                var activity = await _context.Activities.FindAsync(request.Id);
                if(activity == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.NotFound, new { Activity = $"Not Found" });
                }

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                var removeAttendee = await _context.UserActivities.SingleOrDefaultAsync(s => s.AppUserId == user.Id && s.ActivityId == activity.Id);
                if(removeAttendee == null)
                {
                    return Unit.Value;
                }

                if(removeAttendee.IsHost)
                {
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { Attendence = "Cannot remove yourself as host" });
                }

                _context.UserActivities.Remove(removeAttendee);

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    return Unit.Value;
                }

                throw new Exception("Error saving unattend data");
            }            
        }
    }
}