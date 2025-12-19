using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teatro.Helpers;
using Teatro.Models.Dto.Requests;
using Teatro.Services;

namespace Teatro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly EventoService _eventoService;

        public EventoController(EventoService eventoService)
        {
            _eventoService = eventoService;
        }
        // tutti eventi
        [HttpGet]
        public async Task<IActionResult> GetEventi()
        {
            try
            {
                var eventi = await _eventoService.GetEventiAsync();
                return Ok(eventi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        //evento by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvento(Guid id)
        {
            try
            {
                var evento = await _eventoService.GetEventoAsync(id);

                if (evento is null)
                {
                    return NotFound(new { message = "Evento non trovato" });
                }

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // creare evento
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateEvento([FromBody] EventoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var evento = await _eventoService.CreateEventoAsync(request);

                if (evento is null)
                {
                    return BadRequest(new { message = "Errore durante la creazione dell'evento o artista non esistente" });
                }

                return CreatedAtAction(nameof(GetEvento), new { id = evento.EventoId }, evento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // aggiornare evento
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvento(Guid id, [FromBody] EventoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var evento = await _eventoService.UpdateEventoAsync(id, request);

                if (evento is null)
                {
                    return NotFound(new { message = "Evento non trovato o artista non esistente" });
                }

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // elimina evento
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(Guid id)
        {
            try
            {
                var result = await _eventoService.DeleteEventoAsync(id);

                if (!result)
                {
                    return NotFound(new { message = "Evento non trovato" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}