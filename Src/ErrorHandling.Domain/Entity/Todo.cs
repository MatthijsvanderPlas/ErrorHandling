using ErrorHandling.Domain.Validation;
using FluentValidation;

namespace ErrorHandling.Domain.Entity;

public class Todo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;

    protected Todo()
    {
    }

    Todo(string title)
    {
        Title = title;
    }
    
    public static Todo Create(string title)
    {
        var todo = new Todo(title);
        var validator = new TodoValidation();
        var result = validator.Validate(todo);
        if (result.IsValid)
        {
            return todo;
        }
        throw new ValidationException(result.Errors);
    }
} 