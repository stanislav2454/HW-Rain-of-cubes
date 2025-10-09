using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorChanger), typeof(Rigidbody))]
public class Cube : MonoBehaviour, IPoolable
{
    [SerializeField] float _minLifetime = 2f;
    [SerializeField] float _maxLifetime = 5f;
    [SerializeField] private Bomb _bombPrefab;

    private Pool<Cube> _pool;
    private BombPool _bombPool;
    private ColorChanger _colorChanger;
    private Rigidbody _rigidbody;
    private bool _hasCollided = false;
    private Coroutine _lifetimeCoroutine;

    public static event System.Action<Cube> OnCubeCreated;
    public event System.Action<Cube> OnCubeDestroyed;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        OnCubeCreated?.Invoke(this);
        ValidateReferences();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided == false && collision.collider.TryGetComponent<Platform>(out _))
        {
            _hasCollided = true;
            _colorChanger.SetRandomColor();
            float lifetime = Random.Range(_minLifetime, _maxLifetime);
            _lifetimeCoroutine = StartCoroutine(ReturnAfterLifetime(lifetime));
        }
    }

    public void SetPool<T>(Pool<T> pool) where T : MonoBehaviour, IPoolable =>
        _pool = pool as Pool<Cube>;

    public void SetBombPool(BombPool bombPool)
    {
        _bombPool = bombPool;

        if (_bombPool == null)
            Debug.LogWarning($"BombPool not set for {GetType().Name} on {gameObject.name}", this);
    }

    public void ResetCube()
    {
        _hasCollided = false;
        _colorChanger?.SetColor(Color.white);

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

    private void ValidateReferences()
    {
        if (_pool == null)
            Debug.LogWarning($"Pool <Cube> not set for {GetType().Name} on {gameObject.name}", this);

        if (_bombPool == null)
            Debug.LogWarning($"BombPool not set for {GetType().Name} on {gameObject.name}", this);

        if (_bombPrefab == null)
            Debug.LogWarning($"Bomb Prefab  not set for {GetType().Name} on {gameObject.name}", this);
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
            Bomb bomb = _bombPool.GetPooledObject();

            if (bomb != null)
                bomb.transform.position = transform.position;
        }
        else if (_bombPrefab != null)
        {
            Bomb bomb = Instantiate(_bombPrefab, transform.position, Quaternion.identity);
            bomb.Initialize();
        }
    }
}