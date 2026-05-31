namespace rut_shop.net.dto.request;

public record CreateComputingPackageRequest
{
    public Guid? Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int CpuCores { get; init; }
    public int RamGb { get; init; }
    public int StorageGb { get; init; }
    public decimal PricePerMonth { get; init; }
}
