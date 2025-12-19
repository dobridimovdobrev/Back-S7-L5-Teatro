using Microsoft.EntityFrameworkCore;
using Teatro.Data;
using Teatro.Models.Dto.Requests;
using Teatro.Models.Dto.Responses;
using Teatro.Models.Entities;

namespace Teatro.Services
{
    public class BigliettoService : ServiceBase
    {
        public BigliettoService(ApplicationDbContext context) : base(context)
        {
        }

        //lista tutti biglietti
        public async Task<List<BigliettoResponse>> GetBigliettiAsync()
        {
            try
            {
                var biglietti = await _context.Biglietti
                    .AsNoTracking()
                    .Include(b => b.Evento)
                    .Include(b => b.User)
                    .Select(b => new BigliettoResponse
                    {
                        BigliettoId = b.BigliettoId,
                        DataAcquisto = b.DataAcquisto,
                        EventoId = b.EventoId,
                        TitoloEvento = b.Evento!.Titolo,
                        DataEvento = b.Evento.Data,
                        LuogoEvento = b.Evento.Luogo,
                        UserId = b.UserId,
                        NomeUtente = b.User!.Nome + " " + b.User.Cognome
                    })
                    .ToListAsync();

                return biglietti;
            }
            catch (Exception)
            {
                return new List<BigliettoResponse>();
            }
        }

        //biglietti by user
        public async Task<List<BigliettoResponse>> GetBigliettiByUserAsync(string userId)
        {
            try
            {
                var biglietti = await _context.Biglietti
                    .AsNoTracking()
                    .Include(b => b.Evento)
                    .Include(b => b.User)
                    .Where(b => b.UserId == userId)
                    .Select(b => new BigliettoResponse
                    {
                        BigliettoId = b.BigliettoId,
                        DataAcquisto = b.DataAcquisto,
                        EventoId = b.EventoId,
                        TitoloEvento = b.Evento!.Titolo,
                        DataEvento = b.Evento.Data,
                        LuogoEvento = b.Evento.Luogo,
                        UserId = b.UserId,
                        NomeUtente = b.User!.Nome + " " + b.User.Cognome
                    })
                    .ToListAsync();

                return biglietti;
            }
            catch (Exception)
            {
                return new List<BigliettoResponse>();
            }
        }

        //acquista biglietto
        public async Task<List<BigliettoResponse>?> AcquistaBigliettoAsync(string userId, BigliettoRequest request)
        {
            try
            {
                var eventoExists = await _context.Eventi.AnyAsync(e => e.EventoId == request.EventoId);

                if (!eventoExists)
                {
                    return null;
                }

                var biglietti = new List<Biglietto>();

                for (int i = 0; i < request.Quantita; i++)
                {
                    var biglietto = new Biglietto
                    {
                        BigliettoId = Guid.NewGuid(),
                        EventoId = request.EventoId,
                        UserId = userId,
                        DataAcquisto = DateTime.UtcNow
                    };

                    biglietti.Add(biglietto);
                    _context.Biglietti.Add(biglietto);
                }

                bool saved = await SaveAsync();

                if (!saved)
                {
                    return null;
                }

                var evento = await _context.Eventi.AsNoTracking().FirstOrDefaultAsync(e => e.EventoId == request.EventoId);
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

                return biglietti.Select(b => new BigliettoResponse
                {
                    BigliettoId = b.BigliettoId,
                    DataAcquisto = b.DataAcquisto,
                    EventoId = b.EventoId,
                    TitoloEvento = evento!.Titolo,
                    DataEvento = evento.Data,
                    LuogoEvento = evento.Luogo,
                    UserId = b.UserId,
                    NomeUtente = user!.Nome + " " + user.Cognome
                }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}