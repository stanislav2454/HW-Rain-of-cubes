using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorChanger), typeof(ExplosiveObject))]
public class Bomb : MonoBehaviour, IPoolable
{
    private const float MaxAlpha = 1f;

    [SerializeField] private float _minExplosionTime = 2f;
    [SerializeField] private float _maxExplosionTime = 5f;

    private ColorChanger _colorChanger;
    private ExplosiveObject _explosiveObject;
    private Coroutine _explosionCoroutine;

    private System.Action<Bomb> Exploded;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _explosiveObject = GetComponent<ExplosiveObject>();
    }

    private void OnDisable()
    {
        if (_explosionCoroutine != null)
        {
            StopCoroutine(_explosionCoroutine);
            _explosionCoroutine = null;
        }
    }

    public void OnSpawn()
    {
        _colorChanger.SetMaterialTransparent();
        _colorChanger.SetAlpha(MaxAlpha);
        _explosionCoroutine = StartCoroutine(ExplosionCountdown());
    }

    public void OnDespawn()
    {
        _colorChanger.SetMaterialOpaque();
        _colorChanger.SetAlpha(MaxAlpha);

        if (_explosionCoroutine != null)
        {
            StopCoroutine(_explosionCoroutine);
            _explosionCoroutine = null;
        }
        Exploded = null;
    }

    public void SetExplodeCallback(System.Action<Bomb> onExploded) =>
        Exploded = onExploded;

    private IEnumerator ExplosionCountdown()
    {
        float explosionTime = Random.Range(_minExplosionTime, _maxExplosionTime);
        float timer = 0f;

        while (timer < explosionTime)
        {
            timer += Time.deltaTime;
            _colorChanger.SetAlpha(MaxAlpha - timer / explosionTime);
            yield return null;
        }

        Explode();
    }

    private void Explode()
    {
        _explosiveObject.Explode();
        Exploded?.Invoke(this);
        _explosionCoroutine = null;
    }
}