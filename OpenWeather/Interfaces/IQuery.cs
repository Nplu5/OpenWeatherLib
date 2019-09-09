using System.Collections.Generic;

namespace OpenWeather.Interfaces
{
    // TODO: Create Interface with Types set to expose to outside
    public interface IQuery<T1,T2> : ISpecification<T2>
    {
        IEnumerable<ISpecification<T2>> Specifications { get; }
        IEnumerable<T1> Queries { get; }
    }
}
