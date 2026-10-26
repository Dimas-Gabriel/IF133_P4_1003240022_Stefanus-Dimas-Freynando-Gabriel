using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IF133.Tugas1.Api.Data;
using IF133.Tugas1.Api.Entities;

namespace IF133.Tugas1.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/products (Mendapatkan seluruh daftar produk)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }

    // GET: api/products/{id} (Mendapatkan produk berdasarkan ID)
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(new { message = $"Produk dengan ID {id} tidak ditemukan." });
        }

        return Ok(product);
    }

    // POST: api/products (Menambahkan produk baru)
    [HttpPost]
    public async Task<ActionResult<Product>> Create([FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT: api/products/{id} (Memperbarui produk berdasarkan ID)
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        if (id != product.Id)
        {
            return BadRequest(new { message = "ID di URL dan request body tidak cocok." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
        {
            return NotFound(new { message = $"Produk dengan ID {id} tidak ditemukan." });
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;

        await _context.SaveChangesAsync();

        return Ok(existingProduct);
    }

    // DELETE: api/products/{id} (Menghapus produk berdasarkan ID)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(new { message = $"Produk dengan ID {id} tidak ditemukan." });
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
