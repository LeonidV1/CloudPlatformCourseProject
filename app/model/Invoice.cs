namespace rut_shop.net.model;

public class Invoice
{
    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = "text/plain; charset=utf-8";

    public byte[] Content { get; set; } = [];
}
