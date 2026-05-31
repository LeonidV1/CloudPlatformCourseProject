namespace rut_shop.net.dto.response;

public record ComputingPackageResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int CpuCores { get; init; }
    public int RamGb { get; init; }
    public int StorageGb { get; init; }
    public decimal PricePerMonth { get; init; }
    public bool IsActive { get; init; }
}
