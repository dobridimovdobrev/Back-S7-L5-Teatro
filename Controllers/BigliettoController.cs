using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Teatro.Helpers;
using Teatro.Models.Dto.Requests;
using Teatro.Services;

namespace Teatro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BigliettoController : ControllerBase
    {
        private readonly BigliettoService _bigliettoService;

        public BigliettoController(BigliettoService bigliettoService)
        {
            _bigliettoService = bigliettoService;
        }

        // tutti biglietti
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBiglietti()
        {
            try
            {
                var biglietti = await _bigliettoService.GetBigliettiAsync();
                return Ok(biglietti);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        //miei biglietti
        [HttpGet("biglietti")]
        public async Task<IActionResult> GetMyBiglietti()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Utente non autenticato" });
                }

                var biglietti = await _bigliettoService.GetBigliettiByUserAsync(userId);
                return Ok(biglietti);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        // acquista biglietto
        [HttpPost("acquista")]
        public async Task<IActionResult> AcquistaBiglietto([FromBody] BigliettoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Utente non autenticato" });
                }

                var biglietti = await _bigliettoService.AcquistaBigliettoAsync(userId, request);

                if (biglietti is null)
                {
                    return BadRequest(new { message = "Errore durante l'acquisto del biglietto o evento non esistente" });
                }

                return Ok(biglietti);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}