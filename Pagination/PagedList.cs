using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace APIBookstore.Pagination;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set;}
    public int TotalPages { get; private set; }
    public int ItemsPerPage { get; private set; }
    public int TotalCount { get; private set; }
    
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(List<T> list, int pageNumber, int itemsPerPage)
    {
        CurrentPage = pageNumber;
        ItemsPerPage = itemsPerPage;
        TotalPages = (int)Math.Ceiling(list.Count() / (double) itemsPerPage);
        TotalCount = list.Count();
        
        AddRange(list);
    }

    public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int numberItems)
    {
        var list = source.Skip((pageNumber - 1) * numberItems).Take(numberItems).ToList();

        return new PagedList<T>(list, pageNumber, numberItems);
    }

}