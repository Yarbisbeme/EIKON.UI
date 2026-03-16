namespace EIKON.UI.Interfaces;

public interface IMockService<T> where T : class, new()
{
    Task<List<T>> GetAllAsync(int count = 9);
}

