using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ExplosiveObject))]
public class Bomb : MonoBehaviour
{
    private const float Alpha = 1f;

    [SerializeField] private float _minExplosionTime = 2f;
    [SerializeField] private float _maxExplosionTime = 5f;

    private ColorChanger _colorChanger;
    private ExplosiveObject _explosiveObject;
    private System.Action<Bomb> _onExploded;
    private Coroutine _explosionCoroutine;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _explosiveObject = GetComponent<ExplosiveObject>();
    }

    public void Initialize(System.Action<Bomb> onExploded)
    {
        _onExploded = onExploded;

        if (_explosionCoroutine != null)
            StopCoroutine(_explosionCoroutine);

        _explosionCoroutine = StartCoroutine(ExplosionCountdown());
    }

    private IEnumerator ExplosionCountdown()
    {
        _colorChanger.SetMaterialTransparent();
        _colorChanger.SetAlpha(Alpha);

        float explosionTime = Random.Range(_minExplosionTime, _maxExplosionTime);
        float timer = 0f;

        while (timer < explosionTime)
        {
            timer += Time.deltaTime;
            _colorChanger.SetAlpha(Alpha - timer / explosionTime);
            yield return null;
        }

        Explode();
    }

    private void Explode()
    {
        _explosiveObject.Explode();
        _onExploded?.Invoke(this);
        _explosionCoroutine = null;
    }

    private void OnDisable()
    {
        if (_explosionCoroutine != null)
        {
            StopCoroutine(_explosionCoroutine);
            _explosionCoroutine = null;
        }
    }
}
//using UnityEngine;
//using System.Collections;

//public class Bomb : MonoBehaviour, IPoolable
//{
//    private const float AlphaMinValue = 0f;
//    private const float AlphaMaxValue = 1f;

//    [SerializeField] private float _minExplosionTime = 2f;
//    [SerializeField] private float _maxExplosionTime = 5f;
//    [SerializeField] private ExplosiveObject _explosiveObject;

//    private ColorChanger _colorChanger;
//    private Coroutine _explosionCoroutine;
//    private bool _isInPool = false;
//    private Pool<Bomb> _pool;

//    public event System.Action<Bomb> BombExploded;

//    private void Awake()
//    {
//        _colorChanger = GetComponent<ColorChanger>();

//        if (TryGetComponent(out _explosiveObject) == false)
//            Debug.LogError($"\"ExplosiveObject\" not set for {GetType().Name} on {gameObject.name}", this);
//    }

//    public void Initialize()
//    {
//        Debug.Log($"💣 Bomb: Инициализация бомбы");
//        _isInPool = false;
//        _colorChanger?.SetMaterialTransparent();
//        Debug.Log($"💣 Bomb: Запуск отсчета до взрыва");
//        StartExplosionCountdown();
//    }

//    public void SetPool<T>(Pool<T> pool) where T : MonoBehaviour, IPoolable
//    {
//        _pool = pool as Pool<Bomb>;
//    }

//    public void ResetObject()
//    {
//        Debug.Log($"Bomb: ResetObject called");
//        ResetBomb();
//    }

//    public void ResetBomb()
//    {
//        Debug.Log($"Bomb: ResetBomb called");
//        _isInPool = false;

//        _colorChanger?.SetAlpha(AlphaMaxValue);
//        _colorChanger?.SetMaterialOpaque();
//        // НЕ устанавливаем opaque материал здесь - это сделает бомбу непрозрачной
//        // Вместо этого только сбрасываем альфа-канал
//        if (_explosionCoroutine != null)
//        {
//            StopCoroutine(_explosionCoroutine);
//            _explosionCoroutine = null;
//        }

//        if (TryGetComponent(out Rigidbody rigidbody))
//        {
//            rigidbody.velocity = Vector3.zero;
//            rigidbody.angularVelocity = Vector3.zero;
//        }
//    }

//    public void ForceExplodeAndDestroy()
//    {
//        if (_isInPool)
//            return;

//        if (_explosionCoroutine != null)
//        {
//            StopCoroutine(_explosionCoroutine);
//            _explosionCoroutine = null;
//        }

//        Explode();
//    }

//    public bool GetIsInPoolState() =>
//        _isInPool;

//    public void SetInPoolState(bool inPool) =>
//        _isInPool = inPool;

//    private void StartExplosionCountdown()
//    {
//        if (_explosionCoroutine != null)
//            StopCoroutine(_explosionCoroutine);

//        float explosionTime = Random.Range(_minExplosionTime, _maxExplosionTime);
//        _explosionCoroutine = StartCoroutine(ExplosionCountdown(explosionTime));
//        Debug.Log($"💣 Bomb: Отсчет запущен - {explosionTime:F1} сек до взрыва");
//    }

//    private IEnumerator ExplosionCountdown(float explosionTime)
//    {
//        Debug.Log($"💣 Bomb: Начало анимации прозрачности");
//        float timer = 0f;

//        while (timer < explosionTime)
//        {
//            if (_isInPool)
//            {
//                Debug.Log($"💣 Bomb: Отсчет прерван - бомба в пуле");
//                yield break;
//            }

//            timer += Time.deltaTime;
//            float alpha = Mathf.Lerp(AlphaMaxValue, AlphaMinValue, timer / explosionTime);

//            _colorChanger.SetAlpha(alpha);

//            yield return null;
//        }

//        Debug.Log($"💣 Bomb: Время вышло, взрываемся!");
//        Explode();
//    }

//    private void Explode()
//    {
//        if (_isInPool)
//        {
//            Debug.Log($"💣 Bomb: Не могу взорваться - уже в пуле");
//            return;
//        }

//        Debug.Log($"💣 Bomb: ВЗРЫВ! Позиция: {transform.position}");

//        if (_explosiveObject != null)
//            _explosiveObject.Explode(transform.position);

//        Debug.Log($"💣 Bomb: Вызываем BombExploded событие");
//        Debug.Log($"💣 Bomb: Подписчиков: {BombExploded?.GetInvocationList().Length ?? 0}");
//        BombExploded?.Invoke(this);
//        _isInPool = true;
//        Debug.Log($"💣 Bomb: Взрыв завершен, статус в пуле: {_isInPool}");
//    }

//    private void OnDisable()
//    {
//        if (_explosionCoroutine != null)
//        {
//            StopCoroutine(_explosionCoroutine);
//            _explosionCoroutine = null;
//        }
//    }
//}

////public class Bomb : MonoBehaviour, IPoolable
////{
////    private const float AlphaMinValue = 0f;
////    private const float AlphaMaxValue = 1f;

////    [SerializeField] private float _minExplosionTime = 2f;
////    [SerializeField] private float _maxExplosionTime = 5f;
////    [SerializeField] private ExplosiveObject _explosiveObject;

////    private ColorChanger _colorChanger;
////    private Coroutine _explosionCoroutine;
////    private bool _isInPool = false;

////    public event System.Action<Bomb> BombExploded;

////    private void Awake()
////    {
////        _colorChanger = GetComponent<ColorChanger>();

////        if (TryGetComponent(out _explosiveObject) == false)
////            Debug.LogError($"\"ExplosiveObject\" not set for {GetType().Name} on {gameObject.name}", this);
////    }

////    public void Initialize() =>
////        StartExplosionCountdown();

////    public void SetPool<T>(Pool<T> pool) where T : MonoBehaviour, IPoolable
////    {     }

////    public void SetBombPool(BombPool bombPool) { }

////    public void ResetObject() =>
////        ResetBomb();

////    public void ResetBomb()
////    {
////        _isInPool = false;
////        _colorChanger?.SetAlpha(AlphaMaxValue);

////        if (_explosionCoroutine != null)
////        {
////            StopCoroutine(_explosionCoroutine);
////            _explosionCoroutine = null;
////        }

////        if (TryGetComponent(out Rigidbody rigidbody))
////        {
////            rigidbody.velocity = Vector3.zero;
////            rigidbody.angularVelocity = Vector3.zero;
////        }
////    }

////    public void ForceExplodeAndDestroy()
////    {
////        if (_isInPool)
////            return;

////        if (_explosionCoroutine != null)
////        {
////            StopCoroutine(_explosionCoroutine);
////            _explosionCoroutine = null;
////        }

////        Explode();
////    }

////    public bool GetIsInPoolState() =>
////        _isInPool;

////    public void SetInPoolState(bool inPool) =>
////        _isInPool = inPool;

////    private void StartExplosionCountdown()
////    {
////        if (_explosionCoroutine != null)
////            StopCoroutine(_explosionCoroutine);

////        float explosionTime = Random.Range(_minExplosionTime, _maxExplosionTime);
////        _explosionCoroutine = StartCoroutine(ExplosionCountdown(explosionTime));
////    }

////    private IEnumerator ExplosionCountdown(float explosionTime)
////    {
////        float timer = 0f;

////        while (timer < explosionTime)
////        {
////            if (_isInPool) yield break; 

////            timer += Time.deltaTime;
////            float alpha = Mathf.Lerp(AlphaMaxValue, AlphaMinValue, timer / explosionTime);
////            _colorChanger?.SetAlpha(alpha);

////            yield return null;
////        }

////        Explode();
////    }

////    private void Explode()
////    {
////        if (_isInPool)
////            return;

////        if (_explosiveObject != null)
////            _explosiveObject.Explode(transform.position);

////        BombExploded?.Invoke(this);
////    }
////}