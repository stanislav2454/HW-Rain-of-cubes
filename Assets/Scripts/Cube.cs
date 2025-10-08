using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorChanger), typeof(Rigidbody))]
public class Cube : MonoBehaviour, IPoolable
{
    [SerializeField] float _minLifetime = 2f;
    [SerializeField] float _maxLifetime = 5f;
    [SerializeField] private Bomb _bombPrefab;

    [SerializeField] private Pool<Cube> _pool;// !!!
    private BombPool _bombPool;
    private ColorChanger _colorChanger;
    private Rigidbody _rigidbody;
    //private Color _defaultColor;

    private bool _hasCollided = false;
    private Coroutine _lifetimeCoroutine;

    public event System.Action<Cube> OnCubeDestroyed;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _rigidbody = GetComponent<Rigidbody>();

        //if (_pool == null)
        //    _pool = FindObjectOfType<CubePool>();// !!!

        // АВТОМАТИЧЕСКИ находим BombPool
        FindBombPool();

        if (_pool == null)
            Debug.LogWarning($"Pool <Cube> not set for {GetType().Name} on {gameObject.name}", this);

        if (_bombPool == null)
            Debug.LogWarning($"BombPool not set for {GetType().Name} on {gameObject.name}", this);


        if (_bombPrefab == null)
            Debug.LogWarning($"Bomb Prefab  not set for {GetType().Name} on {gameObject.name}", this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided == false && collision.collider.GetComponent<Platform>())
        {
            _hasCollided = true;
            _colorChanger.SetRandomColor();
            float lifetime = Random.Range(_minLifetime, _maxLifetime);
            _lifetimeCoroutine = StartCoroutine(ReturnAfterLifetime(lifetime));
        }
    }

    private void FindBombPool()
    {
        if (_bombPool == null)
        {
            _bombPool = FindObjectOfType<BombPool>();
            
            if (_bombPool == null)            
                Debug.LogWarning($"BombPool not found for {name}! Bombs will not spawn.", this);            
        }
    }

    public void SetPool<T>(Pool<T> pool) where T : MonoBehaviour, IPoolable
    {
        _pool = pool as Pool<Cube>;
    }

    // НОВЫЙ МЕТОД: Установка BombPool извне
    public void SetBombPool(BombPool bombPool)
    {
        _bombPool = bombPool;
    }

    public void ResetCube()
    {
        _hasCollided = false;

        _colorChanger?.SetColor(Color.white);

        // СБРАСЫВАЕМ физику
        if (_rigidbody != null)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        if (_lifetimeCoroutine != null)
        {
            StopCoroutine(_lifetimeCoroutine);
            _lifetimeCoroutine = null;
        }
    }

    private IEnumerator ReturnAfterLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        OnCubeDestroyed?.Invoke(this);
        SpawnBomb();
        _pool?.ReturnToPool(this);
    }

    private void SpawnBomb()
    {
        if (_bombPool != null)
        {
            // ИСПОЛЬЗУЕМ ПУЛ вместо Instantiate
            Bomb bomb = _bombPool.GetPooledObject();
            if (bomb != null)
            {
                bomb.transform.position = transform.position;
                // Initialize вызывается в GetPooledObject BombPool
            }
        }
        else if (_bombPrefab != null)
        {
            // Запасной вариант
            Bomb bomb = Instantiate(_bombPrefab, transform.position, Quaternion.identity);
            bomb.Initialize();
        }
        //if (_bombPrefab != null)
        //{
        //    Bomb bomb = Instantiate(_bombPrefab, transform.position, Quaternion.identity);
        //    bomb.Initialize();
        //}
    }

    // ДОПОЛНИТЕЛЬНАЯ ЗАЩИТА: если куб "застрял"
    private void OnBecameInvisible()
    {
        if (_hasCollided && gameObject.activeInHierarchy)
        {
            StartCoroutine(ReturnAfterLifetime(2.5f));
        }
    }
}