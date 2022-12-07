using CatBreeds.Api.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatBreeds.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatController : ControllerBase
    {
        private readonly CatDbContext ctx;
        private readonly ILogger<CatController> _logger;

        public CatController(CatDbContext ctx, ILogger<CatController> logger)
        {
            this.ctx = ctx;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<IEnumerable<CatReturnVm>> GetAll()
        {
            return await ctx.Cats
                    .Select(t => new CatReturnVm
                    {
                        CreatedAt = t.CreatedAt,
                        Name = t.Name,
                        BreedName = t.Breed.Name,
                        BreedId = t.BreedId,
                        LastUpdatedAt = t.LastUpdatedAt
                    }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<CatReturnVm> Get(int id)
        {
            try
            {
                return await ctx.Cats.Include(t => t.Breed)
                    .Where(t => t.Id == id)
                    .Select(t => new CatReturnVm
                    {
                        CreatedAt = t.CreatedAt,
                        Name = t.Name,
                        BreedName = t.Breed.Name,
                        BreedId = t.BreedId,
                        LastUpdatedAt = t.LastUpdatedAt
                    }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(CatPostVm model)
        {
            try
            {
                ctx.Add(new Cat() { Name = model.Name.Trim(), BreedId = model.BreedId });
                var id = await ctx.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured, POST /cat", model);

                return BadRequest(new { Message = "An error occured, kindly check your parameters" });
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CatPostVm model)
        {
            try
            {
                var cat = await ctx.Cats.FindAsync(id);
                cat.Name = model.Name.Trim();
                cat.BreedId = model.BreedId;
                cat.LastUpdatedAt = DateTime.Now;
                await ctx.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured, PUT /cat/{id}", model);

                return BadRequest(new { Message = "An error occured, kindly check your parameters" });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cat = await ctx.Cats.FindAsync(id);
                ctx.Remove(cat);
                await ctx.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured, DEL /cat/{id}");
                return BadRequest("An error occured, kindly check your parameters");
            }
        }
    }
}