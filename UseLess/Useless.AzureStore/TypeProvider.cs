using Eveneum.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using UseLess.Messages;

namespace Useless.AzureStore
{
    public sealed class TypeProvider : ITypeProvider
    {
        public string GetIdentifierForType(Type type)
        => nameof(Events.Event.Id);

        private static Type BudgetCreated = typeof(Events.BudgetCreated);
        public Type GetTypeForIdentifier(string identifier)
        {
            if (identifier == BudgetCreated.AssemblyQualifiedName) return typeof(Events.BudgetCreated);
            return typeof(string);
        }
    }
}
