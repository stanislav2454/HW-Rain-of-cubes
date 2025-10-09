using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour, IPoolable
{
    private const float AlphaMinValue = 0f;
    private const float AlphaMaxValue = 1f;
    private const int TransparentRenderQueue = 3000;

    [SerializeField] private float _minExplosionTime = 2f;
    [SerializeField] private float _maxExplosionTime = 5f;
    [SerializeField] private Material _bombMaterial;
    [SerializeField] private ExplosiveObject _explosiveObject;

    private Renderer _renderer;
    private Material _materialInstance;
    private Pool<Bomb> _pool;
    private Coroutine _explosionCoroutine;

    public event System.Action<Bomb> OnBombExploded;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        SetupMaterial();

        if (TryGetComponent<ExplosiveObject>(out _explosiveObject) == false)
            Debug.LogError($"\"ExplosiveObject\" not set for {GetType().Name} on {gameObject.name}", this);
    }

    public void Initialize() =>
        StartExplosionCountdown();

    public void SetPool<T>(Pool<T> pool) where T : MonoBehaviour, IPoolable =>
        _pool = pool as Pool<Bomb>;

    public void SetBombPool(BombPool bombPool) { }

    private void SetupMaterial()
    {
        if (_bombMaterial != null)
        {
            _materialInstance = new Material(_bombMaterial);
            _materialInstance.renderQueue = TransparentRenderQueue;
            _renderer.material = _materialInstance;
        }
    }

    private void StartExplosionCountdown()
    {
        if (_explosionCoroutine != null)
            StopCoroutine(_explosionCoroutine);

        float explosionTime = Random.Range(_minExplosionTime, _maxExplosionTime);
        _explosionCoroutine = StartCoroutine(ExplosionCountdown(explosionTime));
    }

    private IEnumerator ExplosionCountdown(float explosionTime)
    {
        float timer = 0f;
        Color startColor = _materialInstance.color;

        while (timer < explosionTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(AlphaMaxValue, AlphaMinValue, timer / explosionTime);

            Color newColor = startColor;
            newColor.a = alpha;
            _materialInstance.color = newColor;

            yield return null;
        }

        Explode();
    }

    private void Explode()
    {
        if (_explosiveObject != null)
            _explosiveObject.Explode(transform.position);
        else
            Debug.LogWarning("ExplosiveObject not found on Bomb!", this);

        OnBombExploded?.Invoke(this);

        if (_pool != null)
            _pool.ReturnToPool(this);
        else
            Destroy(gameObject);
    }

    public void ResetBomb()
    {
        if (_materialInstance != null)
        {
            Color resetColor = _materialInstance.color;
            resetColor.a = AlphaMaxValue;
            _materialInstance.color = resetColor;
        }

        if (_explosionCoroutine != null)
        {
            StopCoroutine(_explosionCoroutine);
            _explosionCoroutine = null;
        }

        if (TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        else
            Debug.LogWarning("rigidbody not found on Bomb!", this);
    }
}