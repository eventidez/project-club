namespace GameSystems.Factory;

public interface IFactory<T>
{
    T Create();
}
