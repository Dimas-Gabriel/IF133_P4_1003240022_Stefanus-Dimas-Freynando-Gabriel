using System.ComponentModel.DataAnnotations;

namespace IF133.Tugas1.Api.Entities;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name wajib diisi.")]
    [StringLength(100, ErrorMessage = "Name maksimal 100 karakter.")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Price wajib diisi.")]
    [Range(0, double.MaxValue, ErrorMessage = "Price harus bernilai >= 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Stock wajib diisi.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock harus bernilai >= 0.")]
    public int Stock { get; set; }
}
