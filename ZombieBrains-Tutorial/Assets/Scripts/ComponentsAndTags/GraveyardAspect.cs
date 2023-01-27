using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct GraveyardAspect : IAspect
{
    public readonly Entity Entity;

    private readonly TransformAspect _transformAspect;

    private readonly RefRO<GraveyardProperties> _graveyardProperties;
    private readonly RefRW<GraveyardRandom> _graveyardRandom;

    public int NumberTombstonesToSpawn => _graveyardProperties.ValueRO.NumberTombstonesToSpawn;
    public Entity TombstonePrefab => _graveyardProperties.ValueRO.TombstonePrefab;

    public LocalTransform GetRandomTombstoneTransform()
    {
        return new LocalTransform
        {
            Position = GetRandomPosition(),
            Rotation = GetRandomRotation(),
            Scale = GetRandomScale(0.5f),
        };
    }

    private float3 GetRandomPosition()
    {
        float3 randomPosition;

        do
        {
            randomPosition = _graveyardRandom.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
        } while (math.distancesq(_transformAspect.LocalPosition, randomPosition) <= BRAIN_SAFETY_RADIUS_SQ);

        return randomPosition;
    }

    private float3 MinCorner => _transformAspect.LocalPosition - HalfDimensions;

    private float3 MaxCorner => _transformAspect.LocalPosition + HalfDimensions;

    private float3 HalfDimensions => new()
    {
        x = _graveyardProperties.ValueRO.FieldDimensions.x * 0.5f,
        y = 0f,
        z = _graveyardProperties.ValueRO.FieldDimensions.y * 0.5f
    };

    private const float BRAIN_SAFETY_RADIUS_SQ = 100;

    private quaternion GetRandomRotation() => quaternion.RotateY(_graveyardRandom.ValueRW.Value.NextFloat(-0.25f, 0.25f));
    private float GetRandomScale(float min) => _graveyardRandom.ValueRW.Value.NextFloat(min, 1f);
}
