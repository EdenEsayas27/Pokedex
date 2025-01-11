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
        public async Task<ActionResult<List<Pokemon>>> Get()
        {
            return Ok(await _pokemonService.GetPokemonsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pokemon>> GetPokemon(string id)
        {
            try
            {
                return Ok(await _pokemonService.GetPokemonByIdAsync(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<Pokemon>> GetPokemonByName(string name)
        {
            try
            {
                return Ok(await _pokemonService.GetPokemonByNameAsync(name));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Pokemon>> AddPokemon(Pokemon newPokemon)
        {
            var createdPokemon = await _pokemonService.AddPokemonAsync(newPokemon);
            return CreatedAtAction(nameof(GetPokemon), new { id = createdPokemon.IdString }, createdPokemon);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Pokemon>> UpdatePokemon(string id, Pokemon updatedPokemon)
        {
            try
            {
                return Ok(await _pokemonService.UpdatePokemonAsync(id, updatedPokemon));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
