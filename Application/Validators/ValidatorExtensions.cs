using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T,string> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password must be atleast 6 characters")
                .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase character")
                .Matches("[a-z]").WithMessage("Password must have atleast 1 lowercase character")
                .Matches("[0-9]").WithMessage("Password must have atleast a number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain non-alpha numeric character");
            return options;
        }
    }
}
