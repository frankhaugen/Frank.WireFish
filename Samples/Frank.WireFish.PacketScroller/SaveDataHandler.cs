using Frank.Mapping;
using Frank.WireFish.PacketScroller.Data;

namespace Frank.WireFish.PacketScroller;

public class SaveDataHandler : IPacketHandler
{
    private readonly IMappingDefinition<DevicePacket, DevicePacketEntity> _mappingDefinition;
    private readonly IRepository<DevicePacketEntity> _repository;

    public SaveDataHandler(IMappingDefinition<DevicePacket, DevicePacketEntity> mappingDefinition, IRepository<DevicePacketEntity> repository)
    {
        _mappingDefinition = mappingDefinition;
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task HandleAsync(DevicePacket packet, CancellationToken cancellationToken)
    {
        var entity = _mappingDefinition.Map(packet);
        await _repository.AddAsync(entity);
        
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public bool CanHandle(DevicePacket packet) => true;
}