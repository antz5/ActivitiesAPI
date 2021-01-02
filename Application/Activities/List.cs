using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities.DTOs;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query :IRequest<List<ActivityDto>>{}
        public class Handler : IRequestHandler<Query, List<ActivityDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                 var activities = await _context.Activities.ToListAsync();
                var activitiesToReturn = _mapper.Map<List<Activity>, List<ActivityDto>>(activities);
                return activitiesToReturn;
            }
        }

    }
}