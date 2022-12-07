using CatBreeds.Api.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatBreeds.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BreedController : ControllerBase
    {
        private readonly CatDbContext ctx;
        private readonly ILogger<BreedController> _logger;

        public BreedController(CatDbContext ctx, ILogger<BreedController> logger)
        {
            this.ctx = ctx;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<IEnumerable<Breed>> GetAll()
        {
            return await ctx.Breeds.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Breed> Get(int id)
        {
            try
            {
                return await ctx.Breeds.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(Breed model)
        {
            ctx.Add(new Breed() { Name = model.Name.Trim() });
            var id = await ctx.SaveChangesAsync();
            return Ok(model);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Breed model)
        {
            try
            {
                var breed = await ctx.Breeds.FindAsync(id);
                breed.Name = model.Name.Trim();
                await ctx.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured, PUT /breed/{id}");

                return BadRequest(new { Message = "An error occured, kindly check your parameters" });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var breed = await ctx.Breeds.FindAsync(id);
                ctx.Remove(breed);
                await ctx.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured, DEL /breed/{id}");
                return BadRequest("An error occured, kindly check your parameters");
            }
        }
    }
}