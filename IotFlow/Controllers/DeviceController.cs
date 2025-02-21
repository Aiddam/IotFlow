using IotFlow.Abstractions.Interfaces.Services;
using IotFlow.Models.DB;
using IotFlow.Models.DTO.Devices;
using IotFlow.Models.DTO.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace IotFlow.Controllers
{
    [Route("api/[controller]s")]
    public class DeviceController : BaseController
    {
        private readonly IDeviceService<DeviceDto, RegisterDeviceDto, UpdateDeviceDto> _deviceService;
        public DeviceController(IDeviceService<DeviceDto, RegisterDeviceDto, UpdateDeviceDto> deviceService)
        {
            _deviceService = deviceService;
        }
        [Authorize, HttpGet("{deviceGuid}")]
        public async Task<IActionResult> GetDevice(Guid deviceGuid, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            await Console.Out.WriteLineAsync(userId.ToString());
            try
            {
                var device = await _deviceService.GetDeviceByGuid(deviceGuid, userId, cancellationToken);
                return Ok(device);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize, HttpGet]
        public async Task<IActionResult> GetDevices(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            try
            {
                var devices = await _deviceService.GetDevices(userId, cancellationToken);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize, HttpPost("create")]
        public async Task<ActionResult<DeviceDto>> CreateDevice([FromBody] RegisterDevice registerDevice, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            RegisterDeviceDto registerDeviceDto = new RegisterDeviceDto()
            {
                UserId = userId,
                Name = registerDevice.Name
            };
            DeviceDto device = await _deviceService.CreateDevice(registerDeviceDto, cancellationToken);

            return Ok(device);
        }
        [Authorize, HttpPut("{deviceGuid}")]
        public async Task<ActionResult<UpdateDeviceDto>> UpdateDevice(Guid deviceGuid, [FromBody] UpdateDevice updateDevice, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            UpdateDeviceDto updateDeviceDto = new UpdateDeviceDto
            {
                DeviceGuid = deviceGuid,
                UserId = userId,
                Name = updateDevice.Name
            };

            try
            {
                DeviceDto device = await _deviceService.UpdateDevice(updateDeviceDto, cancellationToken);
                return Ok(device);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize, HttpDelete("{deviceGuid}")]
        public async Task<IActionResult> DeleteDevice(Guid deviceGuid, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            try
            {
                await _deviceService.DeleteDevice(deviceGuid, userId, cancellationToken);
                return Ok("Device deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new Exception("User identifier is missing in the token.");
            }

            return int.Parse(userIdClaim);
        }
    }
}
