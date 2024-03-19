using System.ComponentModel.DataAnnotations;
using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Application.GetPossibleCustomerPermissionsForGivenUser;

public sealed class GetPossibleCustomerPermissionsForGivenUserResponse
{
    [Required]
    public IEnumerable<GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto> CustomerNodes { get; set; } =
        new List<GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto>();
}

public sealed class GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto
{
    [Required]
    public string CustomerId { get; set; } = "";
    [Required]
    public string CustomerName { get; set; } = "";
    [Required]
    public string CustomerNumber { get; set; } = "";
    [Required]
    public string ParentCustomerId { get; set; } = "";

    [Required]
    public List<CustomerPermission> PermissionsGiven { get; set; } = new();
    [Required]
    public List<CustomerPermission> PermissionsAvailable { get; set; } = new();
    [Required]
    public List<GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto> Children { get; set; } =
        new List<GetPossibleCustomerPermissionsForGivenUserCustomerNodeDto>();
}