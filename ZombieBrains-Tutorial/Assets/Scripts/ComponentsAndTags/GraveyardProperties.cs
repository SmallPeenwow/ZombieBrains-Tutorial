using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct GraveyardProperties : IComponentData
{
    public float2 FieldDimensions;
    public int NumberTombstonesToSpawn;
    public Entity TombstonePrefab;
}
