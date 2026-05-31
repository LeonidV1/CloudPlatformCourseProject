namespace rut_shop.net.dto.request;

public record UpdateComputingPackageRequest
{
    public string Name { get; init; } = string.Empty;
    public int CpuCores { get; init; }
    public int RamGb { get; init; }
    public int StorageGb { get; init; }
    public decimal PricePerMonth { get; init; }
    public bool IsActive { get; init; } = true;
}
