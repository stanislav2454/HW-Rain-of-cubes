using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    private const float Delay = 1f;

    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _spawnHeight = 15f;
    [SerializeField] private ObjectPool<Cube> _cubePool;

    public event System.Action<Cube> CubeDestroyed;

    private void Start() => 
        StartCoroutine(SpawnRoutine());

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(Delay);

        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            SpawnCube();
        }
    }

    private void SpawnCube()
    {
        var cube = _cubePool.Spawn(); 

        if (cube != null)
        {
            cube.transform.position = GetRandomPosition();
            cube.SetDestroyCallback(OnCubeDestroyed); 
        }
    }

    private Vector3 GetRandomPosition() =>
        new Vector3(Random.Range(-_spawnRadius, _spawnRadius),
                    _spawnHeight,
                    Random.Range(-_spawnRadius, _spawnRadius));

    private void OnCubeDestroyed(Cube cube) 
    {
        _cubePool.Despawn(cube); 
        CubeDestroyed?.Invoke(cube); 
    }
}