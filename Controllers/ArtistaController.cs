using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teatro.Helpers;
using Teatro.Models.Dto.Requests;
using Teatro.Services;

namespace Teatro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistaController : ControllerBase
    {
        private readonly ArtistaService _artistaService;

        public ArtistaController(ArtistaService artistaService)
        {
            _artistaService = artistaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtisti()
        {
            try
            {
                var artisti = await _artistaService.GetArtistiAsync();
                return Ok(artisti);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtista(Guid id)
        {
            try
            {
                var artista = await _artistaService.GetArtistaAsync(id);

                if (artista is null)
                {
                    return NotFound(new { message = "Artista non trovato" });
                }

                return Ok(artista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        // crea artista
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateArtista([FromBody] ArtistaRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var artista = await _artistaService.CreateArtistaAsync(request);

                if (artista is null)
                {
                    return BadRequest(new { message = "Errore durante la creazione dell'artista" });
                }

                return CreatedAtAction(nameof(GetArtista), new { id = artista.ArtistaId }, artista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // aggiora artista
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtista(Guid id, [FromBody] ArtistaRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var artista = await _artistaService.UpdateArtistaAsync(id, request);

                if (artista is null)
                {
                    return NotFound(new { message = "Artista non trovato" });
                }

                return Ok(artista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        //elimina artista
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtista(Guid id)
        {
            try
            {
                var result = await _artistaService.DeleteArtistaAsync(id);

                if (!result)
                {
                    return NotFound(new { message = "Artista non trovato" });
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