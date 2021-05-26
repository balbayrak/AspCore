using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;
using FluentValidation;

namespace AspCoreTest.Business.Validators
{
    public class PersonValidator:AbstractValidator<PersonDto>
    {
        public PersonValidator()
        {
            RuleFor(t => t.Name).NotNull().NotEmpty().WithMessage("Adı boş olamaz.");
        }
    }
}
