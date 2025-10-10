using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Material _opaqueMaterial;
    [SerializeField] private Material _transparentMaterial;

    private Renderer _renderer;
    private Material _currentMaterial;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        SetMaterialOpaque();
    }

    public void SetColor(Color color)
    {
        if (_renderer.material != null)
            _renderer.material.color = color;
    }

    public void SetRandomColor() => 
        SetColor(Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f));

    public void SetAlpha(float alpha)
    {
        if (_renderer.material != null)
        {
            var color = _renderer.material.color;
            color.a = Mathf.Clamp01(alpha);
            _renderer.material.color = color;
        }
    }

    public void SetMaterialTransparent()
    {
        if (_transparentMaterial != null)
            ApplyMaterial(_transparentMaterial);
    }

    public void SetMaterialOpaque()
    {
        if (_opaqueMaterial != null)
            ApplyMaterial(_opaqueMaterial);
    }

    private void ApplyMaterial(Material material)
    {
        if (_currentMaterial != material && _renderer != null)
        {
            _renderer.material = material;
            _currentMaterial = material;
        }
    }
}
//using UnityEngine;

//[RequireComponent(typeof(Renderer))]
//public class ColorChanger : MonoBehaviour
//{
//    [SerializeField] private Material _opaqueMaterial;
//    [SerializeField] private Material _transparentMaterial;

//    private Renderer _renderer;
//    //private Material _currentMaterialInstance;
//    private bool _isTransparentMode = false;

//    private void Awake()
//    {
//        CacheComponents();
//        SetMaterialOpaque(); 
//    }

//    //private void OnDestroy()
//    //{
//    //    if (_currentMaterialInstance != null)
//    //        Destroy(_currentMaterialInstance);
//    //}

//    public void SetColor(Color color)
//    {
//        if (_renderer != null && _renderer.material != null)        
//            _renderer.material.color = color;        
//    }

//    public void SetRandomColor()
//    {
//        if (_renderer != null && _renderer.material != null)
//        {
//            Color randomColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
//            _renderer.material.color = randomColor;
//            //Debug.Log($"Cube color changed to: {randomColor}");
//        }
//    }

//    public void SetAlpha(float alpha)
//    {
//        if (_renderer != null && _renderer.material != null)
//        {
//            Color color = _renderer.material.color;
//            color.a = Mathf.Clamp01(alpha);
//            _renderer.material.color = color;
//        }
//    }

//    public void SetMaterialTransparent()
//    {
//        if (_isTransparentMode || _transparentMaterial == null)
//            return;

//        _renderer.material = _transparentMaterial;
//        _isTransparentMode = true;
//        Debug.Log("Material set to transparent mode");
//    }

//    public void SetMaterialOpaque()
//    {
//        if (!_isTransparentMode || _opaqueMaterial == null)
//            return;

//        _renderer.material = _opaqueMaterial;
//        _isTransparentMode = false;
//        Debug.Log("Material set to opaque mode");
//    }

//    //private void ApplyMaterial(Material material)
//    //{
//    //    if (_currentMaterialInstance != null)
//    //        Destroy(_currentMaterialInstance);

//    //    _currentMaterialInstance = new Material(material);
//    //    _renderer.material = _currentMaterialInstance;
//    //}

//    private void CacheComponents()
//    {
//        if (TryGetComponent(out _renderer) == false)
//        {
//            Debug.LogError("Renderer component not found!", this);
//            return;
//        }
//    }
//}
////[RequireComponent(typeof(Renderer))]
////public class ColorChanger : MonoBehaviour
////{
////    [SerializeField] private Material _material;
////    private Renderer _renderer;
////    private Material _materialInstance;

////    private void Awake()
////    {
////        CacheComponents();
////        CreateMaterialInstance();
////    }

////    private void OnDestroy()
////    {
////        if (_materialInstance != null)
////            Destroy(_materialInstance);
////    }

////    public void SetColor(Color color)
////    {
////        if (_materialInstance != null)
////            _materialInstance.color = color;
////    }

////    public void SetRandomColor()
////    {
////        if (_materialInstance != null)
////            _materialInstance.color = Random.ColorHSV();
////    }

////    public void SetAlpha(float alpha)
////    {
////        if (_materialInstance != null)
////        {
////            Color color = _materialInstance.color;
////            color.a = Mathf.Clamp01(alpha);
////            _materialInstance.color = color;
////        }
////    }

////    public void SetRandomColorWithAlpha(float alpha)
////    {
////        if (_materialInstance != null)
////        {
////            Color color = Random.ColorHSV();
////            color.a = Mathf.Clamp01(alpha);
////            _materialInstance.color = color;
////        }
////    }

////    private void CacheComponents()
////    {
////        if (TryGetComponent(out _renderer) == false)
////        {
////            Debug.LogError("Renderer component not found!", this);
////            return;
////        }
////    }

////    private void CreateMaterialInstance()
////    {
////        if (_material != null)
////        {
////            _materialInstance = new Material(_material);
////            _renderer.material = _materialInstance;
////        }
////    }
////}