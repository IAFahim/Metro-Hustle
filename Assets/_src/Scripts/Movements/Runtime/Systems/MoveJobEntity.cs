using _src.Scripts.Movements.Runtime.Datas;
using Unity.Entities;
using Unity.Transforms;

namespace _src.Scripts.Movements.Runtime.Systems
{
    public partial struct MoveJobEntity : IJobEntity
    {
        private void Execute(ref LocalTransform localTransform, in SpeedComponentData speedComponentData)
        {
            localTransform.Position.z += speedComponentData.Speed;
        }
    }
}