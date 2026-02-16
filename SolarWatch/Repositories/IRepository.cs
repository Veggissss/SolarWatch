namespace SolarWatch.Repositories;

public interface IRepository<T, in TK>
{
    void Create(T entity);

    IEnumerable<T> ReadAll();
    
    T? Read(TK id);
    
    void Update(T entity);
    
    void Delete(TK id);
}