namespace rut_shop.net.model;

public class ComputingPackage
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CpuCores { get; set; }

    public int RamGb { get; set; }

    public int StorageGb { get; set; }

    public decimal PricePerMonth { get; set; }

    public bool IsActive { get; set; } = true;
}
