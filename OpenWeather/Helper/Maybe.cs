using System.Collections;
using System.Collections.Generic;

namespace OpenWeather.Helper
{
    // Implemented after: http://codinghelmet.com/articles/avoid-returning-null 
    internal class Maybe<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _values;

        public string ErrorMessage { get; internal set; }

        internal Maybe()
        {
            _values = new T[0];
        }

        internal Maybe(T value)
        {
            _values = new T[] { value };
            ErrorMessage = string.Empty;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
