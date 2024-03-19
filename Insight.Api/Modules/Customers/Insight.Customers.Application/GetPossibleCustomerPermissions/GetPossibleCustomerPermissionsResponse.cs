using System.ComponentModel.DataAnnotations;
using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Application.GetPossibleCustomerPermissions;

public sealed class GetPossibleCustomerPermissionsResponse
{
    [Required]
    public IEnumerable<GetPossibleCustomerPermissionsCustomerNodeDto> CustomerNodes { get; set; } = new List<GetPossibleCustomerPermissionsCustomerNodeDto>();
}

public sealed class GetPossibleCustomerPermissionsCustomerNodeDto
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
    public List<CustomerPermission> Permissions { get; set; } = new();
    [Required]
    public List<GetPossibleCustomerPermissionsCustomerNodeDto> Children { get; set; } = new List<GetPossibleCustomerPermissionsCustomerNodeDto>();
}