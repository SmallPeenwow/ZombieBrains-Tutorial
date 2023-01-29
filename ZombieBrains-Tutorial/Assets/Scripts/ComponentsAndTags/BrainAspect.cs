using Unity.Entities;
using Unity.Transforms;

public readonly partial struct BrainAspect : IAspect
{
    public readonly Entity Entity;

    private readonly TransformAspect _transform;
    private readonly RefRW<BrainHealth> _brainHealth;
    private readonly DynamicBuffer<BrainDamageBufferElement> _brainDamageBuffer;

    public void DamageBrain()
    {
        foreach (var brainDamageBufferElement in _brainDamageBuffer)
        {
            _brainHealth.ValueRW.Value -= brainDamageBufferElement.Value;
        }

        _brainDamageBuffer.Clear();

        var ltw = _transform.LocalTransform;
        ltw.Scale = _brainHealth.ValueRO.Value / _brainHealth.ValueRO.Max;
        _transform.LocalTransform = ltw;
    }
}
