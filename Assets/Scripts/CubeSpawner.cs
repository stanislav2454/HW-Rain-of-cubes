using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    private const string PoolName = "Cube";

    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _spawnHeight = 15f;
    [SerializeField] private SimplePool _cubePool;

    public event System.Action<Vector3> CubeDestroyed;

    private void Start() => StartCoroutine(SpawnRoutine());

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            SpawnCube();
        }
    }

    private void SpawnCube()
    {
        var cube = _cubePool.Get<Cube>(PoolName);

        if (cube != null)
        {
            cube.transform.position = GetRandomPosition();
            cube.Initialize(OnCubeDestroyed);
        }
    }

    private Vector3 GetRandomPosition() =>
        new Vector3(Random.Range(-_spawnRadius, _spawnRadius),
                    _spawnHeight,
                    Random.Range(-_spawnRadius, _spawnRadius));

    private void OnCubeDestroyed(Vector3 position) => 
        CubeDestroyed?.Invoke(position);
}