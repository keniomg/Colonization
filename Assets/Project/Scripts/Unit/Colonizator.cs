using System;
using UnityEngine;

public class Colonizator : Unit 
{
    private Base _basePrefab;
    private Base _base;

    public event Action<Colonizator> BaseBuilded;

    public void Build(Vector3 buildPlace)
    {
        _base = Instantiate(_basePrefab);
        _base.transform.position = buildPlace;
        BaseBuilded?.Invoke(this);
    }
}