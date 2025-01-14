using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pokedex.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;

namespace pokedex.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IMongoCollection<Pokemon> _pokemonCollection;

        public PokemonService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
            _pokemonCollection = database.GetCollection<Pokemon>(configuration["MongoDbSettings:CollectionName"]);
        }

        public async Task<List<Pokemon>> GetPokemonsAsync()
        {
            return await _pokemonCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Pokemon> GetPokemonByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ObjectId format", nameof(id));
            }

            var pokemon = await _pokemonCollection.Find(p => p.Id == objectId).FirstOrDefaultAsync();
            return pokemon ?? throw new KeyNotFoundException($"Pokemon with ID {id} not found");
        }

        public async Task<Pokemon> GetPokemonByNameAsync(string name)
        {
            var pokemon = await _pokemonCollection.Find(p => p.Name == name).FirstOrDefaultAsync();
            return pokemon ?? throw new KeyNotFoundException($"Pokemon with name {name} not found");
        }

        public async Task<Pokemon> AddPokemonAsync(Pokemon newPokemon)
        {
            await _pokemonCollection.InsertOneAsync(newPokemon);
            return newPokemon;
        }

        public async Task<Pokemon> UpdatePokemonAsync(string id, Pokemon updatedPokemon)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ObjectId format", nameof(id));
            }

            updatedPokemon.Id = objectId; // Ensure correct ID in update
            var result = await _pokemonCollection.ReplaceOneAsync(p => p.Id == objectId, updatedPokemon);

            if (result.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"Pokemon with ID {id} not found for update");
            }

            return updatedPokemon;
        }

        public async Task<bool> DeletePokemonAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ObjectId format", nameof(id));
            }

            var result = await _pokemonCollection.DeleteOneAsync(p => p.Id == objectId);
            if (result.DeletedCount == 0)
            {
                throw new KeyNotFoundException($"Pokemon with ID {id} not found for deletion");
            }

            return true;
        }
    }
}