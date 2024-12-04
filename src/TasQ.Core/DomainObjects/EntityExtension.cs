using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TasQ.Core.DomainObjects
{
    public static class EntityExtension
    {
        public static bool EhNullOuRemovido(this Entity? entity) => entity is null || entity.ExcluidoEm.HasValue;
    }
}
