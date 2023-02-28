using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SiecaAPI.DTO;
using SiecaAPI.Models.Services;
using System.Net;
using System.Net.Http.Headers;

namespace SiecaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class FilesController : Controller
    {
        private readonly ILogger<FilesController> _logger;

        public FilesController(ILogger<FilesController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Configure")]
        public async Task<IActionResult> UploadFile()
        {
            DtoRequestResult<string> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                var formCollection = await Request.ReadFormAsync();
                if (!formCollection.Files.IsNullOrEmpty() && formCollection.Files.Count > 0)
                {
                    var file = formCollection.Files[0];
                    var fContentDisp = file.ContentDisposition;
                    if(fContentDisp != null 
                        && ContentDispositionHeaderValue.TryParse(fContentDisp, out var cdHeaderValue)
                        && cdHeaderValue != null
                        && !string.IsNullOrEmpty(cdHeaderValue.FileName))
                    {
                        string fileName =  cdHeaderValue.FileName.Trim('"');
                        var folderName = Path.Combine("Resources", "Images");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        response.Registros.Add(dbPath);
                        return Ok(response);
                    }
                }

                response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                response.MensajeRespuesta = "File to upload not found";
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
            }
            catch (Exception ex)
            {
                _logger.LogError("FilesController: Upload -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
    }
}
