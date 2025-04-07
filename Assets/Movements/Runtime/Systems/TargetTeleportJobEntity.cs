using Movements.Runtime.Datas;
using Unity.Entities;
using Unity.Transforms;

namespace Movements.Runtime.Systems
{
    public partial struct TargetTeleportJobEntity : IJobEntity
    {
        public void Execute(ref LocalTransform localTransform, in TargetPositionComponentData targetPositionComponentData)
        {
            localTransform.Position = targetPositionComponentData.Value;
        }
    }
}