using System.Collections;
using System.Collections.Generic;

namespace OpenWeather.Helper
{
    // Implemented after: http://codinghelmet.com/articles/avoid-returning-null 
    internal class Maybe<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _values;

        internal string ErrorMessage { get; set; }

        internal Maybe()
        {
            _values = new List<T>();
            ErrorMessage = string.Empty;
        }

        internal Maybe(T value)
        {
            _values = new List<T>() { value };
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
