using ErrorHandling.Domain.Entity;
using FluentValidation;

namespace ErrorHandling.Domain.Validation;

public class TodoValidation : AbstractValidator<Todo>
{
    public TodoValidation()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
    }
    
}