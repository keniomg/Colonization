using System.Collections.Generic;
using UnityEngine;

public class CollectingResourcesRegister : MonoBehaviour
{
    public Dictionary<int, Resource> CollectingResources { get; private set; }

    public void RegisterCollectingResource(int id, Resource resource)
    {
        if (CollectingResources.ContainsKey(id) == false)
        {
            CollectingResources.Add(id, resource);
        }
    }

    public void UnregisterCollectingResource(int id, Resource resource)
    {
        if (CollectingResources.ContainsKey(id))
        {
            CollectingResources.Remove(id);
        }
    }
}
