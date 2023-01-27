using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnTombstoneSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GraveyardProperties>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperties>();
        var graveyard = SystemAPI.GetAspectRW<GraveyardAspect>(graveyardEntity);

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        for (int i = 0; i < graveyard.NumberTombstonesToSpawn; i++)
        {
            var newTombstone = ecb.Instantiate(graveyard.TombstonePrefab);
            var newTombstoneTransform = graveyard.GetRandomTombstoneTransform();
            ecb.SetComponent(newTombstone, newTombstoneTransform);
        }

        ecb.Playback(state.EntityManager);
    }
}
