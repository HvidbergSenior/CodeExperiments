using System.ComponentModel.DataAnnotations;
using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Application.GetAvailableCustomersPermissions;

public sealed class GetAvailableCustomersPermissionsResponse
{
    [Required]
    public IEnumerable<CustomerPermissionDto> CustomerNodes { get; set; } = new List<CustomerPermissionDto>();
}

public sealed class CustomerPermissionDto
{
    [Required] 
    public string CustomerId { get; set; } = string.Empty;
    [Required]
    public string CustomerName { get; set; } = string.Empty;
    [Required]
    public string CustomerNumber { get; set; } = string.Empty;
    [Required]
    public string ParentCustomerId { get; set; } = string.Empty;
    [Required] 
    public List<CustomerPermission> Permissions { get; set; } = new();
    [Required]
    public List<CustomerPermissionDto> Children { get; set; } = new List<CustomerPermissionDto>();
}