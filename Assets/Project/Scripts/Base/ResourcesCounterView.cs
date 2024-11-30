using TMPro;
using UnityEngine;

public class ResourcesCounterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resources;
    [SerializeField] private string _defaultText;

    private ResourcesStorage _resourcesStorage;

    private void OnDestroy()
    {
        _resourcesStorage.ValueChanged -= OnValueChanged;
    }

    public void Initialize(ResourcesStorage resourcesStorage)
    {
        _resourcesStorage = resourcesStorage;
        _resourcesStorage.ValueChanged += OnValueChanged;
        OnValueChanged();
    }

    private void OnValueChanged()
    {
        _resources.text = $"{_defaultText} {_resourcesStorage.Count}";
    }
}