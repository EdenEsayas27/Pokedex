using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pokedex.Models;
using pokedex.Services;

namespace pokedex.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        public async Task<List<Pokemon>> Get()
        {
            return await _pokemonService.GetPokemonsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPokemon(string id)
        {
            try
            {
                return Ok(await _pokemonService.GetPokemonByIdAsync(id));
            }
            catch
            {
                return NotFound("Pokemon not found");
            }
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<Pokemon>> GetPokemonByName(string name)
        {
            try
            {
                return Ok(await _pokemonService.GetPokemonByNameAsync(name));
            }
            catch
            {
                return NotFound("Pokemon not found");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Pokemon>> AddPokemon(Pokemon newPokemon)
        {
            await _pokemonService.AddPokemonAsync(newPokemon);
            return CreatedAtAction(nameof(GetPokemon), new { id = newPokemon.Id }, newPokemon);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Pokemon>> UpdatePokemon(string id, Pokemon updatedPokemon)
        {
            try
            {
                return Ok(await _pokemonService.UpdatePokemonAsync(id, updatedPokemon));
            }
            catch
            {
                return NotFound("Pokemon not found");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePokemon(string id)
        {
            try
            {
                await _pokemonService.DeletePokemonAsync(id);
                return NoContent();
            }
            catch
            {
                return NotFound("Pokemon not found");
            }
        }
    }
}
