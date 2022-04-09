using System;
using System.Collections.Generic;
using System.Linq;

namespace Tail.ListRenderer
{
    public class PlainListRenderer<T> : IListRenderer<T>
    {
        private readonly Dictionary<T, int> list;

        public PlainListRenderer(Dictionary<T, int> list)
        {
            this.list = list;
        }

        public string Render()
        {
            var sortedList = from entry in list orderby entry.Value descending select entry;

            return sortedList.Aggregate("",
                (current, keyValuePair) =>
                    current + keyValuePair.Key + "\t" + keyValuePair.Value + Environment.NewLine);
        }

        public string Render(IEnumerable<T> keys)
        {
            var str = "";
            foreach (var key in keys)
                if (list.ContainsKey(key))
                    str += key + "\t" + list[key] + Environment.NewLine;
                else
                    str += key + "\t" + "0" + Environment.NewLine;

            return str;
        }
    }
}