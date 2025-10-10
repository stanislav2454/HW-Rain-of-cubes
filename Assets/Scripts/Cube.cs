using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private const string PoolName = "Cube";

    [SerializeField] private float _minLifetime = 2f;
    [SerializeField] private float _maxLifetime = 5f;

    private Rigidbody _rigidbody;
    private ColorChanger _colorChanger;
    private bool _hasCollided;
    private Coroutine _lifetimeCoroutine;

    private System.Action<Vector3> Destroyed;//почему private?!

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    public void Initialize(System.Action<Vector3> destroyed)
    {
        _hasCollided = false;
        Destroyed = destroyed;
        _colorChanger.SetColor(Color.white);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        if (_lifetimeCoroutine != null)
            StopCoroutine(_lifetimeCoroutine);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided == false && collision.collider.TryGetComponent<Platform>(out _))
        {
            _hasCollided = true;
            _colorChanger.SetRandomColor();
            _lifetimeCoroutine = StartCoroutine(DestroyAfterLifetime());
        }
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(Random.Range(_minLifetime, _maxLifetime));
        Destroyed?.Invoke(transform.position);

        var pool = GetComponentInParent<SimplePool>();
        pool?.Return(PoolName, gameObject);
    }

    private void OnDisable()
    {
        if (_lifetimeCoroutine != null)
        {
            StopCoroutine(_lifetimeCoroutine);
            _lifetimeCoroutine = null;
        }
    }
}
//using UnityEngine;
//using System.Collections;

//[RequireComponent(typeof(ColorChanger), typeof(Rigidbody))]
//public class Cube : MonoBehaviour, IPoolable
//{
//    [SerializeField] float _minLifetime = 2f;
//    [SerializeField] float _maxLifetime = 5f;

//    private Pool<Cube> _pool;
//    private ColorChanger _colorChanger;
//    private Rigidbody _rigidbody;
//    private bool _hasCollided = false;
//    private Coroutine _lifetimeCoroutine;

//    public event System.Action<Cube> CubeDestroyed;

//    private void Awake()
//    {
//        _colorChanger = GetComponent<ColorChanger>();
//        _rigidbody = GetComponent<Rigidbody>();
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (_hasCollided == false && collision.collider.TryGetComponent<Platform>(out Platform platform))
//        {
//            Debug.Log($"🔼 Cube: Столкновение с платформой '{platform.name}'");
//            _hasCollided = true;
//            _colorChanger.SetRandomColor();
//            float lifetime = Random.Range(_minLifetime, _maxLifetime);
//            Debug.Log($"⏱️ Cube: Запуск таймера {lifetime:F1} сек");
//            _lifetimeCoroutine = StartCoroutine(ReturnAfterLifetime(lifetime));
//        }
//    }

//    public void SetPool<T>(Pool<T> pool) where T : MonoBehaviour, IPoolable
//    {
//        _pool = pool as Pool<Cube>;
//        Debug.Log($"Cube: Pool reference set - {_pool != null}");
//    }

//    public void ResetObject() =>
//        ResetCube();

//    public void ResetCube()
//    {
//        _hasCollided = false;
//        _colorChanger?.SetColor(Color.white);

//        if (_rigidbody != null)
//        {
//            _rigidbody.velocity = Vector3.zero;
//            _rigidbody.angularVelocity = Vector3.zero;
//        }

//        if (_lifetimeCoroutine != null)
//        {
//            StopCoroutine(_lifetimeCoroutine);
//            _lifetimeCoroutine = null;
//        }
//    }

//    private IEnumerator ReturnAfterLifetime(float lifetime)
//    {
//        yield return new WaitForSeconds(lifetime);
//        Debug.Log($"⏰ Cube: Таймер истек, уничтожаем куб");

//        Debug.Log($"📢 Cube: Вызываем CubeDestroyed событие");
//        Debug.Log($"📢 Cube: Количество подписчиков: {CubeDestroyed?.GetInvocationList().Length ?? 0}");

//        // ВМЕСТО: CubeDestroyed?.Invoke(this);
//        // ИСПОЛЬЗУЙТЕ:
//        GameEvents.CubeDestroyed(this);

//        if (_pool != null)
//        {
//            Debug.Log($"🔙 Cube: Возвращаем в пул");
//            _pool.ReturnToPool(this);
//        }
//        else
//        {
//            Debug.LogError($"❌ Cube: Пул не установлен!");
//        }
//    }

//    private void OnDisable()
//    {
//        if (_lifetimeCoroutine != null)
//        {
//            StopCoroutine(_lifetimeCoroutine);
//            _lifetimeCoroutine = null;
//        }
//    }
//}

////[RequireComponent(typeof(ColorChanger), typeof(Rigidbody))]
////public class Cube : MonoBehaviour, IPoolable
////{
////    [SerializeField] float _minLifetime = 2f;
////    [SerializeField] float _maxLifetime = 5f;

////    private Pool<Cube> _pool;
////    private ColorChanger _colorChanger;
////    private Rigidbody _rigidbody;
////    private bool _hasCollided = false;
////    private Coroutine _lifetimeCoroutine;

////    public event System.Action<Cube> CubeDestroyed;

////    private void Awake()
////    {
////        _colorChanger = GetComponent<ColorChanger>();
////        _rigidbody = GetComponent<Rigidbody>();
////    }

////    private void OnCollisionEnter(Collision collision)
////    {
////        if (_hasCollided == false && collision.collider.TryGetComponent<Platform>(out _))
////        {
////            _hasCollided = true;
////            _colorChanger.SetRandomColor();
////            float lifetime = Random.Range(_minLifetime, _maxLifetime);
////            _lifetimeCoroutine = StartCoroutine(ReturnAfterLifetime(lifetime));
////        }
////    }

////    public void SetPool<T>(Pool<T> pool) where T : MonoBehaviour, IPoolable =>
////        _pool = pool as Pool<Cube>;

////    public void SetBombPool(BombPool bombPool)
////    {}

////    public void ResetObject() =>
////        ResetCube();

////    public void ResetCube()
////    {
////        _hasCollided = false;
////        _colorChanger?.SetColor(Color.white);

////        if (_rigidbody != null)
////        {
////            _rigidbody.velocity = Vector3.zero;
////            _rigidbody.angularVelocity = Vector3.zero;
////        }

////        if (_lifetimeCoroutine != null)
////        {
////            StopCoroutine(_lifetimeCoroutine);
////            _lifetimeCoroutine = null;
////        }
////    }

////    private IEnumerator ReturnAfterLifetime(float lifetime)
////    {
////        yield return new WaitForSeconds(lifetime);
////        CubeDestroyed?.Invoke(this);
////        _pool?.ReturnToPool(this);
////    }
////}