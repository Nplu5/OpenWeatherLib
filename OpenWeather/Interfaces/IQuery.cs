using System.Collections.Generic;

namespace OpenWeather.Interfaces
{
    public interface IQuery<T1,T2> : ISpecification<T2>
    {
        IEnumerable<ISpecification<T2>> Specifications { get; }
        IEnumerable<T1> Queries { get; }
    }
}
