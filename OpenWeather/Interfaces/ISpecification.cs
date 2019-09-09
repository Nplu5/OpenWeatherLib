namespace OpenWeather.Interfaces
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T element);
    }
}
