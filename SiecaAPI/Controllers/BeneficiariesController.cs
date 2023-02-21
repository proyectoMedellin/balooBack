using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Azure;
using SiecaAPI.DTO;
using SiecaAPI.DTO.Data;
using SiecaAPI.DTO.Requests;
using SiecaAPI.DTO.Response;
using SiecaAPI.Models.Services;
using System.Drawing.Printing;
using System.Net;

namespace SiecaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class BeneficiariesController : ControllerBase
    {
        private readonly ILogger<BeneficiariesController> _logger;

        public BeneficiariesController(ILogger<BeneficiariesController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(DtoBeneficiariesCreateReq request)
        {
            DtoRequestResult<DtoBeneficiaries> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                DtoBeneficiaries beneficiary = new()
                {
                    DocumentTypeId = request.DocumentTypeId,
                    DocumentNumber = request.DocumentNumber,
                    FirstName = request.FirstName,
                    OtherNames = request.OtherNames ?? string.Empty,
                    LastName = request.LastName,
                    OtherLastName = request.OtherLastName ?? string.Empty,
                    GenderId = request.GenderId,
                    BirthDate = request.BirthDate,
                    BirthCountryId = request.BirthCountryId,
                    BirthDepartmentId = request.BirthDepartmentId,
                    BirthCityId = request.BirthCityId,
                    RhId = request.RhId,
                    BloodTypeId = request.BloodTypeId,
                    EmergencyPhoneNumber = request.EmergencyPhoneNumber,
                    PhotoUrl = request.PhotoUrl ?? string.Empty,
                    AdressZoneId = request.AdressZoneId,
                    Adress = request.Adress ?? string.Empty,
                    Neighborhood = request.Neighborhood ?? string.Empty,
                    AdressPhoneNumber = request.AdressPhoneNumber ?? string.Empty,
                    AdressObservations = request.AdressObservations ?? string.Empty,
                    Enabled = request.Enabled,
                    CreatedBy = request.CreationUser
                };

                foreach(DtoBeneficiariesCreateReqFamilyMember dcfm in request.Family)
                {
                    beneficiary.FamilyMembers.Add(new DtoBeneficiariesFamily() {
                        DocumentTypeId = dcfm.DocumentTypeId,
                        DocumentNumber = dcfm.DocumentNumber,
                        FirstName = dcfm.FirstName,
                        OtherNames = dcfm.OtherNames ?? string.Empty,
                        LastName = dcfm.LastName,
                        OtherLastName = dcfm.OtherLastName ?? string.Empty,
                        FamilyRelationId = dcfm.FamilyRelation,
                        Attendant = dcfm.Attendant,
                        Enabled = dcfm.Enabled,
                        CreatedBy = request.CreationUser
                    });
                }

                response.Registros.Add(await BeneficiariesServices.CreateAsync(beneficiary)); 
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: Create -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(DtoTrainingCenterUpdateReq request)
        {
            DtoRequestResult<DtoTrainingCenterCreateResp> response = new DtoRequestResult<DtoTrainingCenterCreateResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Code) || 
                    string.IsNullOrEmpty(request.Name))
                {
                    response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                    response.MensajeRespuesta = "Params not found";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                }
                DtoTrainingCenter center = new()
                {
                    Id = request.Id,
                    Code = request.Code,
                    Name = request.Name,
                    IntegrationCode = request.IntegrationCode,
                    Enabled = request.Enabled,
                    ModifiedBy = request.ModifiedBy,
                };
                center = await TrainingCenterServices.UpdateAsync(center);
                if (center.Id != Guid.Empty)
                {
                    response.Registros.Add(new DtoTrainingCenterCreateResp()
                    {
                        Id = center.Id,
                        OrganizationId = center.OrganizationId,
                        Name = center.Name,
                        Code = center.Code,
                        IntegrationCode = center.IntegrationCode,
                        Enabled = center.Enabled
                    });
                }
                else
                {
                    response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                    response.MensajeRespuesta = "The new center was not updated";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: Update -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            DtoRequestResult<bool> response = new DtoRequestResult<bool>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (id == Guid.Empty)
                {
                    response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                    response.MensajeRespuesta = "Id not found";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                }
                response.Registros.Add(await BeneficiariesServices.DeleteAsync(id));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: Delete -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("ViewGrid")]
        public async Task<IActionResult> ViewGrid(int page, int pageSize, string? fCode,
            string? fName, bool? fEnabled)
        {
            DtoRequestResult<DtoTrainingCenterViewGridResp> response = new DtoRequestResult<DtoTrainingCenterViewGridResp>
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                string filterCode = !string.IsNullOrEmpty(fCode) && fCode != "null" ? fCode : string.Empty;
                string filterName = !string.IsNullOrEmpty(fName) && fName != "null" ? fName : string.Empty; 

                List<DtoTrainingCenter> tCenters = await TrainingCenterServices
                    .GetAllAsync(page, pageSize, filterCode, filterName, fEnabled);

                foreach (DtoTrainingCenter dto in tCenters)
                {
                    response.Registros.Add(new DtoTrainingCenterViewGridResp() { 
                        Id = dto.Id,
                        Code = dto.Code,    
                        Name = dto.Name,
                        Enabled = dto.Enabled
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: getAll -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetEnabledBeneficiaries")]
        public async Task<IActionResult> GetEnabledBeneficiaries()
        {
            DtoRequestResult<DtoTrainingCenter> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                response.Registros = await TrainingCenterServices.GetEnabledTrainigCenterList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: getAll -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            DtoRequestResult<DtoBeneficiaries> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (id == Guid.Empty)
                {
                    response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                    response.MensajeRespuesta = "Id not found";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                }

                response.Registros.Add(await BeneficiariesServices.GetById(id));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: getById -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }

        [HttpGet("GetParmaDataByType")]
        public async Task<IActionResult> GetParmaDataByType(string type)
        {
            DtoRequestResult<DtoBeneficiariesParameters> response = new()
            {
                CodigoRespuesta = HttpStatusCode.OK.ToString()
            };

            try
            {
                if (string.IsNullOrEmpty(type) || string.IsNullOrWhiteSpace(type))
                {
                    response.CodigoRespuesta = HttpStatusCode.BadRequest.ToString();
                    response.MensajeRespuesta = "Param type not found";
                    return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.BadRequest };
                }

                response.Registros = await BeneficiariesServices.GetBeneficiaryParameterInfoByType(type);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("BeneficiariesController: GetParmaDataByType -> " + ex.Message);
                response.CodigoRespuesta = HttpStatusCode.InternalServerError.ToString();
                response.MensajeRespuesta = ex.Message;
                return new ObjectResult(response) { StatusCode = (int?)HttpStatusCode.InternalServerError };
            }
        }
    }
}
