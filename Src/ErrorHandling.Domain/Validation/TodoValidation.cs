using ErrorHandling.Domain.Entity;
using FluentValidation;

namespace ErrorHandling.Domain.Validation;

public class TodoValidation : AbstractValidator<Todo>
{
    public TodoValidation()
    {
        RuleFor(x => x.Title).Must(IsValid).WithMessage("Title is required");
        RuleFor(x => x.Id).NotEqual(Guid.Empty).NotNull().WithMessage("Id is required");
    }
    
    public bool IsValid(string input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }
}