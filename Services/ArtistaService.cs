using Microsoft.EntityFrameworkCore;
using Teatro.Data;
using Teatro.Models.Dto.Requests;
using Teatro.Models.Dto.Responses;
using Teatro.Models.Entities;

namespace Teatro.Services
{
    public class ArtistaService : ServiceBase
    {
        public ArtistaService(ApplicationDbContext context) : base(context)
        {
        }

        //lista artisti
        public async Task<List<ArtistaResponse>> GetArtistiAsync()
        {
            try
            {
                var artisti = await _context.Artisti
                    .AsNoTracking()
                    .Select(a => new ArtistaResponse
                    {
                        ArtistaId = a.ArtistaId,
                        Nome = a.Nome,
                        Genere = a.Genere,
                        Biografia = a.Biografia
                    })
                    .ToListAsync();

                return artisti;
            }
            catch (Exception)
            {
                return new List<ArtistaResponse>();
            }
        }

        // artista by id
        public async Task<ArtistaResponse?> GetArtistaAsync(Guid id)
        {
            try
            {
                var artista = await _context.Artisti
                    .AsNoTracking()
                    .Where(a => a.ArtistaId == id)
                    .Select(a => new ArtistaResponse
                    {
                        ArtistaId = a.ArtistaId,
                        Nome = a.Nome,
                        Genere = a.Genere,
                        Biografia = a.Biografia
                    })
                    .FirstOrDefaultAsync();

                return artista;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // create artista
        public async Task<ArtistaResponse?> CreateArtistaAsync(ArtistaRequest request)
        {
            try
            {
                var artista = new Artista
                {
                    ArtistaId = Guid.NewGuid(),
                    Nome = request.Nome,
                    Genere = request.Genere,
                    Biografia = request.Biografia
                };

                _context.Artisti.Add(artista);

                bool saved = await SaveAsync();

                if (!saved)
                {
                    return null;
                }

                return new ArtistaResponse
                {
                    ArtistaId = artista.ArtistaId,
                    Nome = artista.Nome,
                    Genere = artista.Genere,
                    Biografia = artista.Biografia
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        // edit artista
        public async Task<ArtistaResponse?> UpdateArtistaAsync(Guid id, ArtistaRequest request)
        {
            try
            {
                var artista = await _context.Artisti.FindAsync(id);

                if (artista is null)
                {
                    return null;
                }

                artista.Nome = request.Nome;
                artista.Genere = request.Genere;
                artista.Biografia = request.Biografia;

                _context.Artisti.Update(artista);

                bool saved = await SaveAsync();

                if (!saved)
                {
                    return null;
                }

                return new ArtistaResponse
                {
                    ArtistaId = artista.ArtistaId,
                    Nome = artista.Nome,
                    Genere = artista.Genere,
                    Biografia = artista.Biografia
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        //elimina artista
        public async Task<bool> DeleteArtistaAsync(Guid id)
        {
            try
            {
                var artista = await _context.Artisti.FindAsync(id);

                if (artista is null)
                {
                    return false;
                }

                _context.Artisti.Remove(artista);

                return await SaveAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}