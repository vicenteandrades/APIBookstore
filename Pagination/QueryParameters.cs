namespace APIBookstore.Pagination;

public class QueryParameters
{
    public const int MaxItemsPerPage = 50;
    public int ItemsPerPage { get; set; }
    private int PageDefault { get; set; } = 1;

    public int Page
    {
        get => PageDefault;
        set => PageDefault = (value > MaxItemsPerPage) ? MaxItemsPerPage : value;
    }
    
}