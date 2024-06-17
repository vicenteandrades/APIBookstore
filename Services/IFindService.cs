namespace APIBookstore.Services;

public interface IFindService<T>
{
    Task<IEnumerable<T>> FindNameAsync(string model);
}