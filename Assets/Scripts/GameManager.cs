using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private BombSpawner _bombSpawner;

    private void OnEnable() => 
        _cubeSpawner.CubeDestroyed += OnCubeDestroyed;

    private void OnDisable() => 
        _cubeSpawner.CubeDestroyed -= OnCubeDestroyed;

    private void OnCubeDestroyed(Vector3 position) => 
        _bombSpawner.SpawnBomb(position);
}
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    [SerializeField] private CubePool _cubePool;
//    [SerializeField] private BombPool _bombPool;
//    [SerializeField] private CubeSpawner _cubeSpawner;

//    private void Start()
//    {
//        Debug.Log("GameManager started");

//        // ПРИНУДИТЕЛЬНАЯ ПРОВЕРКА ССЫЛОК
//        if (_cubePool == null)
//        {
//            Debug.LogError("❌ CubePool not set in GameManager!");
//            _cubePool = FindObjectOfType<CubePool>();
//            if (_cubePool != null) Debug.Log("✅ CubePool найден через FindObjectOfType");
//        }

//        if (_bombPool == null)
//        {
//            Debug.LogError("❌ BombPool not set in GameManager!");
//            _bombPool = FindObjectOfType<BombPool>();
//            if (_bombPool != null) Debug.Log("✅ BombPool найден через FindObjectOfType");
//        }

//        // ПОДПИСКА НА УЖЕ СУЩЕСТВУЮЩИЕ КУБЫ
//        SubscribeToExistingCubes();

//        if (_cubeSpawner == null)
//            Debug.LogError("CubeSpawner not set in GameManager!");
//    }

//    private void OnEnable()
//    {
//        GameEvents.OnCubeDestroyed += OnCubeDestroyed;

//        if (_cubePool != null)
//        {
//            _cubePool.CubeCreated += OnCubeCreated;
//            Debug.Log("✅ GameManager подписан на CubeCreated событие");
//        }
//        else
//        {
//            Debug.LogError("❌ CubePool is null - не могу подписаться на события");
//        }
//    }

//    private void OnDisable()
//    {
//        GameEvents.OnCubeDestroyed -= OnCubeDestroyed;

//        if (_cubePool != null)
//        {
//            _cubePool.CubeCreated -= OnCubeCreated;
//        }
//    }

//    private void SubscribeToExistingCubes()
//    {
//        Cube[] existingCubes = FindObjectsOfType<Cube>();
//        Debug.Log($"🔍 Найдено существующих кубов: {existingCubes.Length}");

//        foreach (Cube cube in existingCubes)
//        {
//            if (cube.gameObject.activeInHierarchy)
//            {
//                cube.CubeDestroyed += OnCubeDestroyed;
//                Debug.Log($"✅ Подписались на существующий куб");
//            }
//        }
//    }

//    private void OnCubeCreated(Cube cube)
//    {
//        if (cube != null)
//        {
//            Debug.Log($"🎯 GameManager: Получен новый куб через CubeCreated событие");
//            cube.CubeDestroyed += OnCubeDestroyed;
//            Debug.Log($"✅ GameManager подписан на уничтожение куба");
//        }
//    }

//    private void OnCubeDestroyed(Cube cube)
//    {
//        if (cube != null)
//        {
//            Debug.Log($"💥 GameManager: ПОЛУЧЕНО CubeDestroyed событие!");
//            Debug.Log($"💥 GameManager: Позиция куба: {cube.transform.position}");

//            cube.CubeDestroyed -= OnCubeDestroyed;

//            if (_bombPool != null)
//            {
//                Debug.Log($"💣 GameManager: Запрашиваем бомбу из пула...");
//                Bomb bomb = _bombPool.GetPooledObject();

//                if (bomb != null)
//                {
//                    bomb.transform.position = cube.transform.position;
//                    Debug.Log($"✅ GameManager: Бомба создана на позиции: {cube.transform.position}");
//                }
//                else
//                {
//                    Debug.LogError($"❌ GameManager: Не удалось получить бомбу из пула!");
//                }
//            }
//            else
//            {
//                Debug.LogError($"❌ GameManager: BombPool не установлен!");
//            }
//        }
//    }

//    // Метод для отладки в редакторе
//    [ContextMenu("Принудительно создать бомбу")]
//    private void CreateTestBomb()
//    {
//        if (_bombPool != null)
//        {
//            Bomb bomb = _bombPool.GetPooledObject();
//            if (bomb != null)
//            {
//                bomb.transform.position = Vector3.zero;
//                Debug.Log("✅ Тестовая бомба создана");
//            }
//        }
//    }
//}
////using UnityEngine;

////public class GameManager : MonoBehaviour
////{
////    [SerializeField] private CubePool _cubePool;
////    [SerializeField] private BombPool _bombPool;
////    [SerializeField] private CubeSpawner _cubeSpawner;

////    private void OnEnable()
////    {
////        if (_cubePool != null)
////        {
////            _cubePool.CubeCreated += OnCubeCreated;
////        }

////        if (_cubeSpawner != null)
////        {
////            _cubeSpawner.ObjectSpawned += OnCubeSpawned;
////        }
////    }

////    private void OnDisable()
////    {
////        if (_cubePool != null)
////        {
////            _cubePool.CubeCreated -= OnCubeCreated;
////        }

////        if (_cubeSpawner != null)
////        {
////            _cubeSpawner.ObjectSpawned -= OnCubeSpawned;
////        }
////    }

////    private void OnCubeCreated(Cube cube)
////    {
////        if (cube != null)
////        {
////            cube.CubeDestroyed += OnCubeDestroyed;
////        }
////    }

////    private void OnCubeSpawned(Cube cube)
////    {        // Дополнительная логика при спавне куба через спавнер
////        Debug.Log($"Cube spawned at position: {cube.transform.position}");
////    }

////    private void OnCubeDestroyed(Cube cube)
////    {
////        if (cube != null)
////        {
////            cube.CubeDestroyed -= OnCubeDestroyed;

////            // При уничтожении куба создаем бомбу
////            if (_bombPool != null)
////            {
////                Bomb bomb = _bombPool.GetPooledObject();
////                if (bomb != null)
////                    bomb.transform.position = cube.transform.position;
////            }
////        }
////    }

////    public void SetCubeSpawner(CubeSpawner spawner)
////    {
////        if (_cubeSpawner != null)
////        {
////            _cubeSpawner.ObjectSpawned -= OnCubeSpawned;
////        }

////        _cubeSpawner = spawner;

////        if (_cubeSpawner != null)
////        {
////            _cubeSpawner.ObjectSpawned += OnCubeSpawned;
////        }
////    }
////}