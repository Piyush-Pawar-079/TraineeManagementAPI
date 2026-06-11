using System.ComponentModel.DataAnnotations;
using traineeManagementAPI.DTO.TraineeDTOs;

namespace traineeManagementAPI.DTO.HelperDTOs;



public class FilterDTO
{
    public string? SearchParam { set; get; }

    public Status? StatusFilter { set; get; }

    public string? SortParam { set; get; }

    public bool? Ascending { set; get; }

}