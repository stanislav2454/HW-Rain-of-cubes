using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorChanger : MonoBehaviour
{
    private Renderer _renderer;
    private Material _material;

    private void Awake()
    {
        CacheComponents();
    }

    public void SetRandomColor()
    {// Обычно объекты возвращают в пул через событие.
        if (_renderer != null && _renderer.material != null)// - такие проверки приводят к скрытым багам.
            _renderer.material.color = Random.ColorHSV();
    }

    private void CacheComponents()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
    }
}