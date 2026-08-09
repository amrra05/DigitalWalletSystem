namespace DigitalWallet.Application.DTOs;

public class PaginationRequest
{
    private const int MaxPageSize = 100;

    private int _page = 1;

    private int _pageSize = 10;

    public int Page
    {
        get => _page;

        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;

        set
        {
            if (value < 1)
            {
                _pageSize = 10;
            }
            else if (value > MaxPageSize)
            {
                _pageSize = MaxPageSize;
            }
            else
            {
                _pageSize = value;
            }
        }
    }

    // Filtering
    public string? Type { get; set; }

    public string? Status { get; set; }

    public string? Search { get; set; }
    public string? Role { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    // Sorting
    public string SortBy { get; set; } = "date";

    public string SortOrder { get; set; } = "desc";
}