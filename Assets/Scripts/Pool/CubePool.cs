using UnityEngine;

public class CubePool : Pool<Cube>
{
    //private CubeSpawner _spawner;

    protected override void Awake()
    {
        base.Awake();

        //if (TryGetComponent(out _spawner) == false)
        //    _spawner = gameObject.AddComponent<CubeSpawner>();
    }

    [ContextMenu("ContextMenu /ReturnAllCubes ( )")]
    public void ReturnAllCubes()
    {
        var activeCubes = FindObjectsOfType<Cube>();

        foreach (var cube in activeCubes)
            if (cube.gameObject.activeInHierarchy)
                ReturnToPool(cube);
    }
}