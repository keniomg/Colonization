using TMPro;
using UnityEngine;

public class ResourcesCounterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resources;
    [SerializeField] private string _defaultText;
    [SerializeField] private ResourcesStorage _resourcesStorage;

    private void OnEnable()
    {
        _resourcesStorage.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _resourcesStorage.ValueChanged -= OnValueChanged;
    }

    private void OnValueChanged()
    {
        _resources.text = $"{_defaultText} {_resourcesStorage.Count}";
    }
}