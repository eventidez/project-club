namespace GameSystems.Pool;

public interface IPool<T>
{
    T Request();

    void Return(T member);
}
