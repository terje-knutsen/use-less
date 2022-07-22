using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Useless.Framework.Extensions
{
    public static class CollectionExtensions
    {
        public static void ReplaceItem<T>(this Collection<T> col, Func<T, bool> match, T newItem)
        {
            var oldItem = col.FirstOrDefault(i => match(i));
            var oldIndex = col.IndexOf(oldItem);
            col[oldIndex] = newItem;
        }
    }
}
