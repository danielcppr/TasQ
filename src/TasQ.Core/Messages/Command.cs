using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using MediatR;

namespace TasQ.Core.Messages;

public abstract class Command<TResponse> : Message, IRequest<TResponse>
{
    [System.Text.Json.Serialization.JsonIgnore]
    public DateTime Timestamp { get; private set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public ValidationResult? ValidationResult { get; set; }

    protected Command()
    {
        Timestamp = DateTime.UtcNow;
    }

    public abstract bool EhValido();
}