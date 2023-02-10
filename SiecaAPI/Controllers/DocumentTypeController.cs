using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLayerDotNetAPI.Data.SQLImpl.Entities;
using SecurityLayerDotNetAPI.DTO;
using SecurityLayerDotNetAPI.DTO.Data;
using SecurityLayerDotNetAPI.DTO.Requests;
using SecurityLayerDotNetAPI.DTO.Response;
using SecurityLayerDotNetAPI.Errors;
using SecurityLayerDotNetAPI.Models;
using SecurityLayerDotNetAPI.Services;
using System.Net;

namespace SecurityLayerDotNetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
   
    public class DocumentTypeController : ControllerBase
    {
        private readonly ILogger<DocumentTypeController> _logger;

        public DocumentTypeController(ILogger<DocumentTypeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetAllDocumentType")]
        public async Task<IActionResult> GetAllDocumentType()
        {

            DtoRequestResult<List<DTODocumentTypeResp>> response = new DtoRequestResult<List<DTODocumentTypeResp>>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                List<DTODocumentType> documentTypes = await DocumentTypeServices.GetAllDocumentTypeAsync();
                List<DTODocumentTypeResp> dtodocumentTypeResp = new();
                if (documentTypes.Count == 0) throw new NoDataFoundException("No exiten registros");
                foreach (DTODocumentType doc in documentTypes)
                {
                    dtodocumentTypeResp.Add(new DTODocumentTypeResp(
                        doc.Id,
                        doc.OrganizationId,
                        doc.Code,
                        doc.Name,
                        doc.Enabled,
                        doc.CreatedBy,
                        doc.CreatedOn,
                        doc.ModifiedBy,
                        doc.ModifiedOn));
                }
                response.Registros.Add(new List<DTODocumentTypeResp>(dtodocumentTypeResp));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DocumentTypeController: Login -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
    }
}
