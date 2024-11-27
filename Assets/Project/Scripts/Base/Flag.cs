using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Flag : MonoBehaviour 
{
    public event Action CollidedWithPreview;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BuildingPreview buildingPreview))
        {
            CollidedWithPreview?.Invoke();
        }       
    }
}