using Microsoft.EntityFrameworkCore;
using Teatro.Data;
using Teatro.Models.Dto.Requests;
using Teatro.Models.Dto.Responses;
using Teatro.Models.Entities;

namespace Teatro.Services
{
    public class EventoService : ServiceBase
    {
        public EventoService(ApplicationDbContext context) : base(context)
        {
        }

        //lista eventi
        public async Task<List<EventoResponse>> GetEventiAsync()
        {
            try
            {
                var eventi = await _context.Eventi
                    .AsNoTracking()
                    .Include(e => e.Artista)
                    .Select(e => new EventoResponse
                    {
                        EventoId = e.EventoId,
                        Titolo = e.Titolo,
                        Data = e.Data,
                        Luogo = e.Luogo,
                        ArtistaId = e.ArtistaId,
                        NomeArtista = e.Artista!.Nome
                    })
                    .ToListAsync();

                return eventi;
            }
            catch (Exception)
            {
                return new List<EventoResponse>();
            }
        }

        //evento by id
        public async Task<EventoResponse?> GetEventoAsync(Guid id)
        {
            try
            {
                var evento = await _context.Eventi
                    .AsNoTracking()
                    .Include(e => e.Artista)
                    .Where(e => e.EventoId == id)
                    .Select(e => new EventoResponse
                    {
                        EventoId = e.EventoId,
                        Titolo = e.Titolo,
                        Data = e.Data,
                        Luogo = e.Luogo,
                        ArtistaId = e.ArtistaId,
                        NomeArtista = e.Artista!.Nome
                    })
                    .FirstOrDefaultAsync();

                return evento;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //creare evento
        public async Task<EventoResponse?> CreateEventoAsync(EventoRequest request)
        {
            try
            {
                var artistaExists = await _context.Artisti.AnyAsync(a => a.ArtistaId == request.ArtistaId);

                if (!artistaExists)
                {
                    return null;
                }

                var evento = new Evento
                {
                    EventoId = Guid.NewGuid(),
                    Titolo = request.Titolo,
                    Data = request.Data,
                    Luogo = request.Luogo,
                    ArtistaId = request.ArtistaId
                };

                _context.Eventi.Add(evento);

                bool saved = await SaveAsync();

                if (!saved)
                {
                    return null;
                }

                var artista = await _context.Artisti.AsNoTracking().FirstOrDefaultAsync(a => a.ArtistaId == request.ArtistaId);

                return new EventoResponse
                {
                    EventoId = evento.EventoId,
                    Titolo = evento.Titolo,
                    Data = evento.Data,
                    Luogo = evento.Luogo,
                    ArtistaId = evento.ArtistaId,
                    NomeArtista = artista!.Nome
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        //edit evento
        public async Task<EventoResponse?> UpdateEventoAsync(Guid id, EventoRequest request)
        {
            try
            {
                var evento = await _context.Eventi.FindAsync(id);

                if (evento is null)
                {
                    return null;
                }

                var artistaExists = await _context.Artisti.AnyAsync(a => a.ArtistaId == request.ArtistaId);

                if (!artistaExists)
                {
                    return null;
                }

                evento.Titolo = request.Titolo;
                evento.Data = request.Data;
                evento.Luogo = request.Luogo;
                evento.ArtistaId = request.ArtistaId;

                _context.Eventi.Update(evento);

                bool saved = await SaveAsync();

                if (!saved)
                {
                    return null;
                }

                var artista = await _context.Artisti.AsNoTracking().FirstOrDefaultAsync(a => a.ArtistaId == request.ArtistaId);

                return new EventoResponse
                {
                    EventoId = evento.EventoId,
                    Titolo = evento.Titolo,
                    Data = evento.Data,
                    Luogo = evento.Luogo,
                    ArtistaId = evento.ArtistaId,
                    NomeArtista = artista!.Nome
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        //elimina evento
        public async Task<bool> DeleteEventoAsync(Guid id)
        {
            try
            {
                var evento = await _context.Eventi.FindAsync(id);

                if (evento is null)
                {
                    return false;
                }

                _context.Eventi.Remove(evento);

                return await SaveAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}