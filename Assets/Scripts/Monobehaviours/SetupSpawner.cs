using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SetupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _personPrefab;
    [SerializeField] private int _gridSize;
    [SerializeField] private int _spread;
    [SerializeField] private Vector2 _speedRange = new Vector2(4, 7);
    [SerializeField] private Vector2 _lifetimeRange = new Vector2(10, 60);

    private BlobAssetStore _blob;

    private void Start()
    {
        _blob = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blob);
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(_personPrefab, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        for (int x = 0; x < _gridSize; x++)
        {
            for (int z = 0; z < _gridSize; z++)
            {
                var instance = entityManager.Instantiate(entity);

                float3 position = new float3(x * _spread, 0, z * _spread);
                entityManager.SetComponentData(instance, new Translation { Value = position });
                entityManager.SetComponentData(instance, new Destination { Value = position });
                float lifetime = UnityEngine.Random.Range(_lifetimeRange.x, _lifetimeRange.y);
                entityManager.SetComponentData(instance, new Lifetime { Value = lifetime });
                float speed = UnityEngine.Random.Range(_speedRange.x, _speedRange.y);
                entityManager.SetComponentData(instance,
                    new MovementSpeed { Value = speed });
            }
        }
    }

    private void OnDestroy()
    {
        _blob.Dispose();
    }
}
