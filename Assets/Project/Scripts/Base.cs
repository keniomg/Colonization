using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    
}

public class ResourcesScanner : MonoBehaviour
{
    [SerializeField] private Map _map;

    public Dictionary<int, Resource> Resources { get; private set; }

    private void OnEnable()
    {
        _map.ResourceAppeared += RegisterAvailableResource;
        _map.ResourceDisappeared += UnregisterAvailableResource;
    }

    private void OnDisable()
    {
        _map.ResourceAppeared -= RegisterAvailableResource;
        _map.ResourceDisappeared -= UnregisterAvailableResource;
    }

    private void RegisterAvailableResource(int id, Resource resource)
    {
        if (Resources.ContainsKey(id) == false)
        {
            Resources.Add(id, resource);
        }
    }

    private void UnregisterAvailableResource(int id, Resource resource)
    {
        if (Resources.ContainsKey(id))
        {
            Resources.Remove(id);
        }
    }
}

public class CollectingResourceEventInvoker : MonoBehaviour
{
    public Dictionary<int, Resource> ChoosenResources { get; private set; }

    public void RegisterCollectingResource(int id, Resource resource)
    {
        if (ChoosenResources.ContainsKey(id) == false)
        {
            ChoosenResources.Remove(id);
        }
    }

    public void UnregisterCollectingResource(int id, Resource resource)
    {
        if (ChoosenResources.ContainsKey(id))
        {
            ChoosenResources.Remove(id);
        }
    }
}

public class ResourcesStorage : MonoBehaviour
{

}

public class ResourcesSpawner : MonoBehaviour
{

}

public class UnitTasker : MonoBehaviour
{
    [SerializeField] private float _taskGivingDelay;

    private List<Unit> _units;
    private WaitForSeconds _giveTaskDelay;



    private IEnumerator GiveTask(Unit unit)
    {
        yield return null;
    }
}

public class Unit : MonoBehaviour
{
    public bool IsBusy {get; private set; }
}

public class UnitMover : MonoBehaviour
{

}

public class UnitResourcesHolder : MonoBehaviour
{

}

public class UnitResourcesTaker : MonoBehaviour
{

}

public class UnitResourcesGiver : MonoBehaviour
{

}

public class UnitResourceChooser : MonoBehaviour
{
    private void ChooseResource()
    {

    }
}

public class Map : MonoBehaviour
{
    public event Action<int, Resource> ResourceAppeared;
    public event Action<int, Resource> ResourceDisappeared;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Resource resource))
        {
            ResourceAppeared?.Invoke(resource.gameObject.GetInstanceID(), resource);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Resource resource))
        {
            ResourceDisappeared?.Invoke(resource.gameObject.GetInstanceID(), resource);
        }
    }
}

public class Resource : MonoBehaviour
{

}