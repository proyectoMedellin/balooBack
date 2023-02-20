using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiecaAPI.DTO;
using SiecaAPI.DTO.Data;
using SiecaAPI.DTO.Requests;
using SiecaAPI.DTO.Response;
using SiecaAPI.Errors;
using SiecaAPI.Models;
using SiecaAPI.Services;
using System.Net;

namespace SiecaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }
        [HttpPost("CreateAccessUser")]
        public async Task<DtoRequestResult<DtoAccessUserResp>> CreateAccessUser(DtoAccessUserReq request)
        {
            DtoRequestResult<DtoAccessUserResp> response = new DtoRequestResult<DtoAccessUserResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(request.UserName) && !string.IsNullOrEmpty(request.Email) && !string.IsNullOrEmpty(request.CreatedBy)
                    && !string.IsNullOrEmpty(request.FirstName) && !string.IsNullOrEmpty(request.LastName))
                {
                    AccessUser user = await UsersServices.CreateAccessUserAsync(request.UserName, request.Email, request.FirstName, request.OtherNames,
                        request.LastName, request.OtherLastName, true, request.CreatedBy, request.Phone, request.DocumentTypeId.Value, request.DocumentNo, request.TrainingCenterId);
                    response.Registros.Add(new DtoAccessUserResp(user.Id, user.OrganizationId, user.UserName,
                        user.Email, user.FirstName, user.OtherLastName,
                        user.LastName, user.OtherLastName, user.DocumentTypeId, user.DocumentNo));
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.PreconditionFailed.ToString();
                    response.MensajeRespuesta = "Parámetros obligatorios incompletos";
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateAccessUser: CreateAccessUser -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return response;
            }
        }

        [HttpGet("GetAccessUserByName")]
        public async Task<IActionResult> GetAccessUserByName(string userName)
        {
            DtoRequestResult<DtoAccessUserResp> response = new DtoRequestResult<DtoAccessUserResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    var user = await UsersServices.GetUserInfo(userName);
                    if (user.Id == Guid.Empty) throw new NoDataFoundException("No exite el usuario");

                    response.Registros.Add(new DtoAccessUserResp(user.Id.Value, user.OrganizationId, 
                        user.UserName, user.Email, user.FirstName, user.OtherNames, user.LastName,
                        user.OtherLastName, user.DocumentTypeId, user.DocumentNo));
                }
                else
                {
                    throw new MissingArgumentsException("el parametro userName no puede ser vacio");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateAccessUser: CreateAccessUser -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
        [HttpPost("UpdateAccessUser")]
        public async Task<IActionResult> UpdateAccessUser(DtoAccessUserReq request)
        
        {
            DtoRequestResult<DtoAccessUserResp> response = new DtoRequestResult<DtoAccessUserResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (!string.IsNullOrEmpty(request.UserName) && !string.IsNullOrEmpty(request.Email) && !string.IsNullOrEmpty(request.CreatedBy)
                    && !string.IsNullOrEmpty(request.FirstName) && !string.IsNullOrEmpty(request.LastName))
                {
                    var user = await UsersServices.GetUserInfo(request.oldUserName);
                    if (user.Id == Guid.Empty) throw new NoDataFoundException("No exite el usuario");
                    
                    bool userResponse = await UsersServices.UpdateAccessUserAsync(request.oldUserName,request.UserName, request.Email, request.FirstName, request.OtherNames,
                        request.LastName, request.OtherLastName, true, request.CreatedBy, request.Phone, request.DocumentTypeId.Value, request.DocumentNo);
                    response.Registros.Add(new DtoAccessUserResp(user.Id.Value, user.OrganizationId,
                        request.UserName, request.Email, request.FirstName, request.OtherNames,
                        request.LastName, request.OtherLastName, request.DocumentTypeId.Value, request.DocumentNo));
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.PreconditionFailed.ToString();
                    response.MensajeRespuesta = "Parámetros obligatorios incompletos";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AuthenticationController: Login -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            DtoRequestResult<List<DtoAccessUserResp>> response = new DtoRequestResult<List<DtoAccessUserResp>>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                List<DtoAccessUser> accessUsers = await UsersServices.GetAllUsersByOrganization();
                List<DtoAccessUserResp> dtoAccessUserResp = new();
                if(accessUsers.Count == 0) throw new NoDataFoundException("No exiten registros");
                foreach (DtoAccessUser user in accessUsers)
                {
                    dtoAccessUserResp.Add(new DtoAccessUserResp(user.OrganizationId,
                        user.UserName, user.Email, user.FirstName, user.OtherNames, user.LastName,
                        user.OtherLastName, user.DocumentTypeId, user.DocumentNo));
                }
                response.Registros.Add(new List<DtoAccessUserResp>(dtoAccessUserResp));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("AuthenticationController: Login -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }

        }
    }
}