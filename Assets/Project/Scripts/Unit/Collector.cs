using UnityEngine;

[RequireComponent(typeof(CollectorCommandController), typeof(UnitResourcesHolder))]
public class Collector : Unit
{
    public CollectorCommandController CollectorCommandController { get; private set; }
    public UnitResourcesHolder ResourcesHolder { get; private set; }

    protected override void GetComponents()
    {
        base.GetComponents();
        ResourcesHolder = GetComponent<UnitResourcesHolder>();
        CollectorCommandController = GetComponent<CollectorCommandController>();
    }
}