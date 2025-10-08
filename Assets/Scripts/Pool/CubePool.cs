using UnityEngine;

public class CubePool : Pool<Cube>
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override Cube CreatePooledObject()
    {
        Cube cube = base.CreatePooledObject();

        // ПОДПИСЫВАЕМСЯ на событие уничтожения куба
        cube.OnCubeDestroyed += OnCubeDestroyed;

        return cube;
    }

    protected override void OnDestroyPooledObject(Cube cube)
    {
        // ОТПИСЫВАЕМСЯ от события
        if (cube != null)
        {
            cube.OnCubeDestroyed -= OnCubeDestroyed;
        }

        base.OnDestroyPooledObject(cube);
    }

    private void OnCubeDestroyed(Cube cube)
    {
        Debug.Log($"Cube destroyed event received for {cube.name}");
        // Можно добавить дополнительную логику
    }

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

    // ДОБАВЛЕНО: метод для получения расширенной статистики кубов
    public string GetCubeExtendedStatistics()
    {
        return $"=== CUBES EXTENDED STATS ===\n" +
               $"{GetFullStatistics()}\n" +
               $"Pool Capacity: {_defaultCapacity}/{_maxSize}";
    }
}