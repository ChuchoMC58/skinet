using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IUnitOfWork UoW) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
        [FromQuery] ProductSpecParams specParams)
    {
        var spec = new ProductSpecification(specParams);

        return await CreatePagedResult(UoW.Repository<Product>(), spec, specParams.PageIndex, specParams.PageSize);
    }  

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await UoW.Repository<Product>().GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        UoW.Repository<Product>().Add(product);

        if(await UoW.Complete()){
            return CreatedAtAction("GetProduct", new {id= product.Id}, product);
        }

        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !ProductExists(id))
        {
            return BadRequest("Cannot update a non-existent product");
        }

        UoW.Repository<Product>().Update(product);

        if(await UoW.Complete()){
            return NoContent();
        }

        return BadRequest("Problem updating the product");     
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await UoW.Repository<Product>().GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        UoW.Repository<Product>().Remove(product);

           if(await UoW.Complete()){
            return NoContent();
        }

        return BadRequest("Problem deleting the product"); 
    }
 
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
       var spec = new BrandListSpecification();
        return Ok(await UoW.Repository<Product>().ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        return Ok(await UoW.Repository<Product>().ListAsync(spec));
    }

    private bool ProductExists(int id)
    {
        return UoW.Repository<Product>().Exists(id);
    }
}
