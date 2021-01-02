using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Activities.DTOs
{
    public class AttendeeDto
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool IsHost { get; set; }
    }
}
