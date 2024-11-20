﻿using TMPro;
using UnityEngine;

public class ResourcesCounterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resources;
    [SerializeField] private string _defaultText;
    [SerializeField] private ResourcesEventInvoker _resourcesEventInvoker;

    private void OnEnable()
    {
        _resourcesEventInvoker.ResourceChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _resourcesEventInvoker.ResourceChanged -= OnValueChanged;
    }

    private void OnValueChanged(int id, int resources)
    {
        if (gameObject.GetInstanceID() == id)
        {
            _resources.text = $"{_defaultText} {resources}";
        }
    }
}