using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Material _material;
    private Renderer _renderer;

    private void Awake()
    {
        CacheComponents();
    }

    private void OnDestroy()
    {
        // Очищаем материал если он был создан динамически
        if (_material != null && _material.name.Contains("Instance"))
        {
            DestroyImmediate(_material);
        }
    }

    public void SetColor(Color color)
    {
        if (_material != null)
            _material.color = color;
    }

    public void SetRandomColor()
    {// Обычно объекты возвращают в пул через событие.
     //if (_renderer != null && _renderer.material != null)// - такие проверки приводят к скрытым багам.
     //   _renderer.material.color = Random.ColorHSV();
        if (_material != null)
            _material.color = Random.ColorHSV();
    }

    private void CacheComponents()
    {
        if (TryGetComponent(out _renderer) == false)
        {
            Debug.LogError("Renderer component not found!", this);
            return;
        }

        _material = _renderer.material;
    }
}