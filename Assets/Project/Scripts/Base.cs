using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    
}

public class ResourcesScanner : MonoBehaviour
{

}

public class ResourcesStorage : MonoBehaviour
{

}

public class Unit : MonoBehaviour
{

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

public class Map : MonoBehaviour
{
    public Dictionary<int, Resource> Resources {get; private set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Resource resource))
        {
            RegisterAvailableResource(resource.gameObject.GetInstanceID(), resource);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Resource resource))
        {
            UnregisterAvailableResource(resource.gameObject.GetInstanceID(), resource);
        }
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

public class Resource : MonoBehaviour
{

}