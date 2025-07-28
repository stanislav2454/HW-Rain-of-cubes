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
    {
        if (_renderer != null && _renderer.material != null)
            _renderer.material.color = Random.ColorHSV();
    }

    private void CacheComponents()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
    }
}