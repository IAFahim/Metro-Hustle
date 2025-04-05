using _src.Scripts.Movements.Runtime.Datas;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _src.Scripts.Movements.Runtime.Systems
{
    public partial struct TargetTeleportJobEntity : IJobEntity
    {
        public void Execute(ref LocalTransform localTransform, in TargetPositionComponentData targetPositionComponentData)
        {
            localTransform.Position = targetPositionComponentData.Value;
        }
    }
}