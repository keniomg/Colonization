using UnityEngine;

[RequireComponent(typeof(ColonizatorCommandController), typeof(UnitColonizer))]

public class Colonizator : Unit
{
    public ColonizatorCommandController ColonizatorCommandController { get; private set; }
    public UnitColonizer Colonizer { get; private set; }

    protected override void GetComponents()
    {
        base.GetComponents();
        ColonizatorCommandController = GetComponent<ColonizatorCommandController>();
        Colonizer = GetComponent<UnitColonizer>();
    }
}