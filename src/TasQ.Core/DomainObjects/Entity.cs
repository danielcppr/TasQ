﻿using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace TasQ.Core.DomainObjects;
public abstract class Entity
{
    [Key]
    public Guid Id { get; set; }
    public DateTime? ExcluidoEm { get; private set; }

    protected void ExecutarSoftDelete() => ExcluidoEm = DateTime.Now;

    //private List<Event> _notificacoes;
    //public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();

    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    //public void AdicionarEvento(Event evento)
    //{
    //    _notificacoes = _notificacoes ?? new List<Event>();
    //    _notificacoes.Add(evento);
    //}

    //public void RemoverEvento(Event eventItem)
    //{
    //    _notificacoes?.Remove(eventItem);
    //}

    //public void LimparEventos()
    //{
    //    _notificacoes?.Clear();
    //}

    public override bool Equals(object obj)
    {
        var compareTo = obj as Entity;

        if (ReferenceEquals(this, compareTo)) return true;
        if (ReferenceEquals(null, compareTo)) return false;

        return Id.Equals(compareTo.Id);
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().GetHashCode() * 907) + Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    public virtual ValidationResult Validar(Func<bool> validacao, string mensagemFalha)
    {
        if (validacao()) return new ValidationResult();

        var falhas = new List<ValidationFailure> { new(validacao.Method.Name, mensagemFalha) };
        return new ValidationResult(falhas);
    }

    public virtual bool EhValido()
    {
        throw new NotImplementedException();
    }
}
