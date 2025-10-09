using UnityEngine;

public class CubePool : Pool<Cube>
{
#if UNITY_EDITOR
    [ContextMenu("ContextMenu /ReturnAllCubes ( )")]
    public void ReturnAllCubes()
    {
        var activeCubes = FindObjectsOfType<Cube>();
        int returnedCount = 0;

        foreach (var cube in activeCubes)
            if (cube.gameObject.activeInHierarchy)
            {
                ReturnToPool(cube);
                returnedCount++;
            }
    }
#endif
}