using _src.Scripts.Easings.Runtime.Datas;
using _src.Scripts.Jumps.Runtime.Datas;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace _src.Scripts.Jumps.Runtime.Settings
{
    public partial struct JumpSystem
    {
        [BurstCompile]
        private partial struct JumpJob : IJobEntity
        {
            public float DeltaTime;

            void Execute(ref JumpComponentData jump, ref LocalTransform transform)
            {
                jump.ElapsedTime += DeltaTime;
                if (HandleRise(ref jump, out var riseDuration)) return;
                if (HandleAir(jump, riseDuration, out var endAirTime)) return;
                if (HandleFall(ref jump, endAirTime)) return;
            }

            [BurstCompile]
            private static bool HandleFall(ref JumpComponentData jump, float endAirTime)
            {
                var fallTime = jump.FallDuration * jump.FallDurationMultiplier;
                var endFallTime = endAirTime + fallTime;
                if (jump.ElapsedTime > endFallTime) return false;
                float t = (endFallTime - jump.ElapsedTime) / fallTime;
                var fall = jump.FallEase.Evaluate(t) * jump.MaxHeight;
                jump.CurrentHeight = fall;
                return true;
            }

            [BurstCompile]
            private static bool HandleAir(JumpComponentData jump, float riseDuration, out float endAirTime)
            {
                endAirTime = riseDuration + jump.AirTime;
                return jump.ElapsedTime < endAirTime;
            }

            [BurstCompile]
            private static bool HandleRise(ref JumpComponentData jump, out float riseDuration)
            {
                riseDuration = jump.RiseDuration * jump.RiseDurationMultiplier;
                if (jump.ElapsedTime > riseDuration) return false;
                float t = jump.ElapsedTime / riseDuration;
                var height = jump.RiseEase.Evaluate(t) * jump.MaxHeight;
                jump.CurrentHeight = height;
                return true;
            }
        }
    }
}