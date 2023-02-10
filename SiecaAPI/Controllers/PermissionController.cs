using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
   
    public class PermissionController : ControllerBase
    {
        private readonly ILogger<RolController> _logger;

        public PermissionController(ILogger<RolController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetAllPermission")]
        public async Task<IActionResult> GetAllPermission()
        {
            DtoRequestResult<List<DtoPermissionResp>> response = new DtoRequestResult<List<DtoPermissionResp>>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                List<DtoPermission> permissions = await PermissionServices.GetAllPermissionAsync();
                List<DtoPermissionResp> dtoPermissionsResp = new();
                if (permissions.Count == 0) throw new NoDataFoundException("No exiten registros");
                foreach (DtoPermission per in permissions)
                {
                    dtoPermissionsResp.Add(new DtoPermissionResp(
                        per.Id,
                        per.OrganizationId,
                        per.Name,
                        per.Description,
                        per.Type,
                        per.Enabled,
                        per.CreatedBy,
                        per.CreatedOn,
                        per.ModifiedBy,
                        per.ModifiedOn));
                }
                response.Registros.Add(new List<DtoPermissionResp>(dtoPermissionsResp));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("PermissionsController: GetAll -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }

        }

        [HttpGet("GetAllPermissionByUserName")]
        public async Task<IActionResult> GetAllPermissionByUserName(string userName)
        {
            DtoRequestResult<List<DtoPermissionResp>> response = new DtoRequestResult<List<DtoPermissionResp>>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                List<DtoPermission> permissions = await PermissionServices.GetAllPermissionByUserNameAsync(userName);
                List<DtoPermissionResp> dtoPermissionsResp = new();
                if (permissions.Count == 0) throw new NoDataFoundException("No exiten registros");
                foreach (DtoPermission per in permissions)
                {
                    dtoPermissionsResp.Add(new DtoPermissionResp(
                        per.Id,
                        per.OrganizationId,
                        per.Name,
                        per.Description,
                        per.Type,
                        per.Enabled,
                        per.CreatedBy,
                        per.CreatedOn,
                        per.ModifiedBy,
                        per.ModifiedOn));
                }
                response.Registros.Add(new List<DtoPermissionResp>(dtoPermissionsResp));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("PermissionsController: GetAll -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }

        }


    }
}
