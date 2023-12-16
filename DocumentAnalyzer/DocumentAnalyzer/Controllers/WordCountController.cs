using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DocumentAnalyzer.Models;
using DocumentAnalyzer.Classes;

namespace DocumentAnalyzer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordCountController : ControllerBase
    {
        [HttpPost]
        [Route("/")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(WordCountUploadResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountWords([FromForm] WordCountUploadRequest request)
        {
            try
            {

                var result = await WordCountFacade.AnalyzeFileAsync(request.UploadFile) ?? throw new Exception("Error processing file");

                var response = new WordCountUploadResponse(result);

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Error! " + ex.Message);
            }
        }

    }
}
