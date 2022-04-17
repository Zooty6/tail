using System;
using System.Collections.Generic;
using System.Linq;

namespace Tail.ListRenderer
{
    public class BoxListRenderer<T> : IListRenderer<T>
    {
        private const string TopLeftCorner = "╔";
        private const string TopMiddle = "╦";
        private const string TopRightCorner = "╗";
        private const string BottomLeftCorner = "╚";
        private const string BottomMiddle = "╩";
        private const string BottomRightCorner = "╝";
        private const string Horizontal = "═";
        private const string Vertical = "║";

        private readonly Dictionary<T, int> list;

        public BoxListRenderer(Dictionary<T, int> list)
        {
            this.list = list;
        }

        public string Render()
        {
            var maxKeyLength = MaxLengthOfStrings(list.Keys);
            var maxValueLength = MaxLengthOfInts(list.Values);
            var sortedList = from entry in list orderby entry.Value descending select entry;
            // TOP
            var str = TopLeftCorner + string.Concat(Enumerable.Repeat(Horizontal, maxKeyLength)) + TopMiddle +
                      string.Concat(Enumerable.Repeat(Horizontal, maxValueLength)) + TopRightCorner +
                      Environment.NewLine;
            // CONTENT
            str += sortedList.Aggregate("",
                (current, keyValuePair) =>
                    current + Vertical + " " + keyValuePair.Key +
                    string.Concat(Enumerable.Repeat(" ", maxKeyLength - keyValuePair.Key.ToString()!.Length - 1)) +
                    Vertical +
                    string.Concat(Enumerable.Repeat(" ", maxValueLength - keyValuePair.Value.ToString().Length - 1)) +
                    keyValuePair.Value + " " + Vertical + Environment.NewLine);
            // BOTTOM
            str += BottomLeftCorner + string.Concat(Enumerable.Repeat(Horizontal, maxKeyLength)) + BottomMiddle +
                   string.Concat(Enumerable.Repeat(Horizontal, maxValueLength)) + BottomRightCorner;

            return str;
        }

        public string Render(IEnumerable<T> keys)
        {
            var keysArray = keys as T[] ?? keys.ToArray();
            var maxKeyLength = MaxLengthOfStrings(keysArray);
            var values = keysArray.Select(GetValue);
            var maxValueLength = MaxLengthOfInts(values);
            // TOP
            var str = TopLeftCorner + string.Concat(Enumerable.Repeat(Horizontal, maxKeyLength)) + TopMiddle +
                      string.Concat(Enumerable.Repeat(Horizontal, maxValueLength)) + TopRightCorner +
                      Environment.NewLine;
            // CONTENT
            str += keysArray.Aggregate("",
                (current, key) =>
                    current + Vertical + " " + key +
                    string.Concat(Enumerable.Repeat(" ", maxKeyLength - key.ToString()!.Length - 1)) +
                    Vertical +
                    string.Concat(Enumerable.Repeat(" ", maxValueLength - GetValue(key).ToString().Length - 1)) +
                    GetValue(key) + " " + Vertical + Environment.NewLine);
            // BOTTOM
            str += BottomLeftCorner + string.Concat(Enumerable.Repeat(Horizontal, maxKeyLength)) + BottomMiddle +
                   string.Concat(Enumerable.Repeat(Horizontal, maxValueLength)) + BottomRightCorner;

            return str;
        }

        private int GetValue(T key)
        {
            return list.ContainsKey(key) ? list[key] : 0;
        }

        private static int MaxLengthOfStrings(IEnumerable<T> values)
        {
            return values.Max(t => t.ToString()!.Length + 2);
        }

        private static int MaxLengthOfInts(IEnumerable<int> values)
        {
            return values.Max(i => i.ToString().Length + 2);
        }
    }
}