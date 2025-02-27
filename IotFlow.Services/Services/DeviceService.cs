using IotFlow.Abstractions.Interfaces.Adapters;
using IotFlow.Abstractions.Interfaces.DA;
using IotFlow.Abstractions.Interfaces.Services;
using IotFlow.Models.DB;
using IotFlow.Models.DTO.Commands;
using IotFlow.Models.DTO.Devices;
using IotFlow.Models.DTO.Devices.Register;
using IotFlow.Models.DTO.Devices.Send;
using IotFlow.Models.DTO.Devices.Update;
using IotFlow.Models.Enum;

namespace IotFlow.Services.Services
{
    public class DeviceService : IDeviceService<DeviceDto, DeviceWithIdDto, RegisterDeviceDto, UpdateDeviceDto, MethodDto, DeviceAliveDto, SendMethodParameterDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDeviceDtoAdapter<DeviceDto, DeviceWithIdDto, Device> _adapter;
        private readonly IIotFlowApiService<CommandDto> _iotFlowApiService;

        public DeviceService(IUnitOfWork unitOfWork, IDeviceDtoAdapter<DeviceDto, DeviceWithIdDto, Device> adapter, IIotFlowApiService<CommandDto> iotFlowApiService)
        {
            _unitOfWork = unitOfWork;
            _adapter = adapter;
            _iotFlowApiService = iotFlowApiService;
        }

        public async Task<DeviceDto> CreateDevice(RegisterDeviceDto registerDevice, CancellationToken cancellationToken = default)
        {
            Device newDevice = new Device
            {
                Name = registerDevice.Name,
                UserId = registerDevice.UserId,
                IsAlive = false
            };

            return await ProcessDeviceAsync(newDevice, cancellationToken: cancellationToken);
        }
        public async Task<DeviceDto> UpdateDevice(UpdateDeviceDto updateDevice, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();

            var foundedDevices = await deviceRepository.ReadEntitiesByPredicate(
                device => device.DeviceGuid == updateDevice.DeviceGuid && device.UserId == updateDevice.UserId);

            Device? existingDevice = foundedDevices.FirstOrDefault();

            if (existingDevice == null)
            {
                throw new ArgumentException("Device not found");
            }

            existingDevice.Name = updateDevice.Name;

            return await ProcessDeviceAsync(existingDevice, cancellationToken);
        }
        public async Task<DeviceDto> SetDeviceAlive(Guid deviceGuid, int userId, DeviceAliveDto deviceAliveDto, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();

            var foundedDevices = await deviceRepository.ReadEntitiesByPredicate(
                device => device.DeviceGuid == deviceGuid && device.UserId == userId);

            Device? existingDevice = foundedDevices.FirstOrDefault();

            if (existingDevice == null)
            {
                throw new ArgumentException("Device not found");
            }

            existingDevice.IsAlive = deviceAliveDto.IsAlive;
            existingDevice.LastSeen = DateTime.UtcNow;

            return await ProcessDeviceAsync(existingDevice, cancellationToken);
        }

        public async Task DeleteDevice(Guid deviceGuid, int userId, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();
            var foundedDevices = await deviceRepository.ReadEntitiesByPredicate(device => device.DeviceGuid == deviceGuid && device.UserId == userId);
            var firstDevice = foundedDevices.FirstOrDefault();
            if (firstDevice == null)
            {
                throw new ArgumentException("Unable to delete device");
            }
            await deviceRepository.DeleteAsync(firstDevice.Id);
        }

        public async Task<DeviceDto> GetDeviceByGuid(Guid deviceGuid, int userId, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();
            var foundedDevices = await deviceRepository.ReadEntitiesByPredicate(device => deviceGuid == device.DeviceGuid && device.UserId == userId);
            Device? firstDevice = foundedDevices.FirstOrDefault();
            if (firstDevice == null)
            {
                throw new ArgumentException("Unable to read device");
            }
            return await _adapter.GetDeviceDtoAsync(firstDevice);
        }
        public async Task<DeviceWithIdDto> GetDeviceWithIdByGuid(Guid deviceGuid, int userId, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();
            var foundedDevices = await deviceRepository.ReadEntitiesByPredicate(device => deviceGuid == device.DeviceGuid && device.UserId == userId);
            Device? firstDevice = foundedDevices.FirstOrDefault();
            if (firstDevice == null)
            {
                throw new ArgumentException("Unable to read device");
            }
            return await _adapter.GetDeviceWidhIdDtoAsync(firstDevice);
        }
        public async Task<ICollection<DeviceDto>> GetDevices(int userId, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();
            var foundedDevices = await deviceRepository.ReadEntitiesByPredicate(device => device.UserId == userId);
            return await _adapter.GetDeviceDtosAsync((ICollection<Device>)foundedDevices);
        }
        public async Task<DeviceDto> ProcessDeviceAsync(Device device, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();

            if (device.Id == 0)
            {
                await deviceRepository.CreateAsync(device, cancellationToken: cancellationToken);
            }
            else
            {
                await deviceRepository.UpdateAsync(device, cancellationToken: cancellationToken);
            }
            await _unitOfWork.CommitAsync();

            return new DeviceDto()
            {
                DeviceGuid = device.DeviceGuid,
                Name = device.Name,
                IsAlive = device.IsAlive,
                Methods = device.Methods,
                LastSeen = device.LastSeen
            };
        }
        private static ParameterType ParseParameterType(string type)
        {
            if (Enum.TryParse<ParameterType>(type, true, out var parsedType))
            {
                return parsedType;
            }
            throw new ArgumentException($"Unknown parameter type: {type}");
        }
        private static MethodType ParseMethodType(string type)
        {
            if (Enum.TryParse<MethodType>(type, true, out var parsedType))
            {
                return parsedType;
            }
            throw new ArgumentException($"Unknown parameter type: {type}");
        }

        public async Task AddMethods(Guid deviceGuid, int userId, List<MethodDto> newMethods, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();
            var devices = await deviceRepository.ReadEntitiesByPredicate(d => d.DeviceGuid == deviceGuid && d.UserId == userId);
            Device? device = devices.FirstOrDefault();

            if (device == null)
            {
                throw new ArgumentException("Device not found");
            }

            var existingMethodsDict = device.Methods.ToDictionary(m => m.MethodName, StringComparer.OrdinalIgnoreCase);

            foreach (var newMethod in newMethods)
            {
                if (existingMethodsDict.TryGetValue(newMethod.MethodName, out var existingMethod))
                {
                    if (existingMethod.Description != newMethod.Description)
                    {
                        var parsedMethodType = ParseMethodType(newMethod.Type);
                        existingMethod.Description = newMethod.Description;
                        existingMethod.MethodType = parsedMethodType;
                    }

                    var existingParamsDict = existingMethod.Parameters.ToDictionary(p => p.ParameterName, StringComparer.OrdinalIgnoreCase);

                    foreach (var newParam in newMethod.Parameters)
                    {
                        var newParamType = ParseParameterType(newParam.Type);

                        if (existingParamsDict.TryGetValue(newParam.Name, out var existingParam))
                        {
                            if (existingParam.Description != newParam.Description)
                            {
                                existingParam.Description = newParam.Description;
                            }
                            if (existingParam.ParameterType != newParamType)
                            {
                                existingParam.ParameterType = newParamType;
                            }
                        }
                        else
                        {
                            var param = new DeviceMethodParameter
                            {
                                ParameterName = newParam.Name,
                                Description = newParam.Description,
                                ParameterType = newParamType,
                                Order = existingMethod.Parameters.Count + 1
                            };
                            existingMethod.Parameters.Add(param);
                        }
                    }

                    var newParamNames = newMethod.Parameters.Select(p => p.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
                    var paramsToRemove = existingMethod.Parameters.Where(p => !newParamNames.Contains(p.ParameterName)).ToList();
                    foreach (var param in paramsToRemove)
                    {
                        existingMethod.Parameters.Remove(param);
                    }
                }
                else
                {
                    var parsedMethodType = ParseMethodType(newMethod.Type);
                    var deviceMethod = new DeviceMethod
                    {
                        MethodName = newMethod.MethodName,
                        Description = newMethod.Description,
                        MethodType = parsedMethodType,
                        DeviceId = device.Id,
                        Parameters = new List<DeviceMethodParameter>()
                    };

                    int order = 1;
                    foreach (var newParam in newMethod.Parameters)
                    {
                        var parsedParameterType = ParseParameterType(newParam.Type);
                        var param = new DeviceMethodParameter
                        {
                            ParameterName = newParam.Name,
                            Description = newParam.Description,
                            ParameterType = parsedParameterType,
                            Order = order++
                        };
                        deviceMethod.Parameters.Add(param);
                    }

                    device.Methods.Add(deviceMethod);
                }
            }

            var newMethodNames = newMethods.Select(m => m.MethodName).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var methodsToRemove = device.Methods.Where(m => !newMethodNames.Contains(m.MethodName)).ToList();
            foreach (var method in methodsToRemove)
            {
                device.Methods.Remove(method);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task SendCommandAsync(Guid deviceGuid, int userId, string command, ICollection<SendMethodParameterDto> parameters, Guid correlationId, CancellationToken cancellationToken = default)
        {
            var commandDto = new CommandDto()
            {
                Command = command,
                DeviceGuid = deviceGuid.ToString(),
                Parameters = parameters,
                CorrelationId = correlationId,
            };
            await _iotFlowApiService.SendCommandAsync(commandDto, cancellationToken);
        }
    }
}
