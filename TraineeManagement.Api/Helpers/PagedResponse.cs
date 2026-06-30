namespace TraineeManagement.Api.Helpers;

public class PagedResponse<T>(List<T> data, int pageNumber, int pageSize, int totalRecords)
{
    public List<T> Data { get; set; } = data;
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
    public int TotalRecords { get; set; } = totalRecords;
}