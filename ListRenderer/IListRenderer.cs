using System.Collections.Generic;

namespace Tail.ListRenderer
{
    public interface IListRenderer<in T>
    {
        public string Render();
        public string Render(IEnumerable<T> keys);
    }
}