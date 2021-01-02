using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Application.Activities.DTOs;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{    
    public class ActivitiesController : BaseController
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<ActivityDto>>> List()
        {
            var activities = await Mediator.Send(new List.Query());
            if(activities == null || activities.Count ==0)
            {
                return NotFound();
            }
            return activities;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> Details(Guid id)
        {
            var activity = await Mediator.Send(new Details.Query{Id = id});
            if(activity == null)
            {
                return NotFound();
            }
            return activity;
         }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create([FromBody]Create.Command command)
        {
            return await Mediator.Send(command);            
        }

        [HttpPut("{id}")]
        [Authorize(Policy ="IsActivityHost")]
        public async Task<ActionResult<Unit>> Edit(Guid id,Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {           
            return await Mediator.Send(new Delete.Command(){Id = id});
        }

        [HttpPost("{id}/attend")]
        public async Task<ActionResult<Unit>> Attend(Guid id)
        {
            return await Mediator.Send(new Attend.Command { Id = id });
        }

        [HttpDelete("{id}/attend")]
        public async Task<ActionResult<Unit>> DeleteAttendee(Guid id)
        {
            return await Mediator.Send(new DeleteAttendee.Command { Id = id });
        }
    }
}