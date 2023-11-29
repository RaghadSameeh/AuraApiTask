using BussienesLayer.DTO;
using DataAccessLayer.Models;
using DataAccessLayer.Reposatories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AuraTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IGenericReposatory<Product> _product;
        public ProductController(IGenericReposatory<Product> product)
        {
            _product = product;
        }


        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<DTOResult>> GetAllProducts()
        {
            DTOResult result = new DTOResult();
            List<Product> products =await _product.GetAllAsync();
            result.IsPass = products.Count != 0 ? true : false;
            result.Data = products;
            return result;
        }


        [HttpPost("NewProduct")]
        public async Task<ActionResult<DTOResult>> NewProduct(Product product)
        {
            DTOResult result = new DTOResult();
            if (ModelState.IsValid)
            {

                try
                {
                    _product.insert(product);
                    await _product.saveAsync();

                    result.IsPass = true;
                    result.Data = $"Product Created Successfuly with ID {product.Id}";
                }
                catch (Exception ex)
                {
                    result.IsPass = false;
                    result.Data = $"An error{ex.Message} occurred while creating the Product.";
                }
            }
            else
            {
                result.IsPass = false;
                result.Data = ModelState.Values.SelectMany(e=> e.Errors)
                    .Select(e => e.ErrorMessage).ToList();
            }

            return result;

        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<ActionResult<DTOResult>> SoftDelete (int id)
        {
            DTOResult result=new DTOResult();

            try
            {
                result.Data = await _product.SoftDelete(id);
                result.IsPass = true;
                 await _product.saveAsync();
            }
            catch (Exception ex)
            {
                result.IsPass = false;
                result.Data = ex.Message;
            }
            return result;

        }




    }
}
