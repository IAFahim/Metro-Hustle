using Unity.Entities;
using Unity.Transforms;

namespace _src.Scripts.Movements
{
    public partial struct MoveJobEntity : IJobEntity
    {
        private void Execute(ref LocalTransform localTransform, in SpeedComponentData speedComponentData)
        {
            localTransform.Position.z += speedComponentData.Speed;
        }
    }
}