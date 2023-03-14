﻿using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SiecaAPI.DTO;
using SiecaAPI.DTO.Data;
using SiecaAPI.Models.Services;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SiecaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class FilesController : Controller
    {
        private readonly ILogger<FilesController> _logger;

        public FilesController(ILogger<FilesController> logger)
        {
            _logger = logger;
        }

        [HttpPost("UploadPhoto")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile()
        {
            DtoRequestResult<string> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                var formCollection = await Request.ReadFormAsync();
                if (!formCollection.Files.IsNullOrEmpty() && formCollection.Files.Count > 0 
                    && formCollection.ContainsKey("beneficiaryId") 
                    && Guid.TryParse(formCollection["beneficiaryId"], out Guid beneficiaryId))
                {
                    
                    DtoBeneficiaries ben = await BeneficiariesServices.GetById(beneficiaryId);
                    if(ben == null)
                    {
                        response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                        response.MensajeRespuesta = "Beneficiary to update not found";
                        return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                    }

                    var file = formCollection.Files[0];
                    var fContentDisp = file.ContentDisposition;
                    if (fContentDisp != null
                        && ContentDispositionHeaderValue.TryParse(fContentDisp, out var cdHeaderValue)
                        && cdHeaderValue != null
                        && !string.IsNullOrEmpty(cdHeaderValue.FileName))
                    {
                        string fileName = ben.Id + ".jpg";
                        var folderName = Path.Combine("Resources", "Images");

                        if (!Directory.Exists(folderName))
                            Directory.CreateDirectory(folderName);


                        var networkPath = @"\\192.168.2.3\images";
                        /*var pathToSave = Path.Combine(networkPath, folderName);
                        var fullPath = Path.Combine(pathToSave, fileName);*/

                        var dbPath = Path.Combine(networkPath, fileName);
                        using (var stream = new FileStream(dbPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        string photoPath = $"{Request.Scheme}://{Request.Host.Value}/Resources/Images/{fileName}";
                        await BeneficiariesServices.UploadPhotoAsync(ben.Id, photoPath);

                        response.Registros.Add(photoPath);
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
