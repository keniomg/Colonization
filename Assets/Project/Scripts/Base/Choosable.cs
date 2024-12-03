using System;
using UnityEngine;

public class Choosable : MonoBehaviour
{
    [SerializeField] private ParticleSystem _highlight;

    public event Action Choosed;
    public event Action Unchoosed;

    private void Awake()
    {
        _highlight.Stop();
    }

    public void Choose()
    {
        Choosed?.Invoke();
        Highlight();
    }

    public void Unchoose()
    {
        Unchoosed?.Invoke();
        Unhiglight();
    }

    private void Highlight()
    {
        _highlight.Play();
    }

    private void Unhiglight()
    {
        _highlight.Stop();
    }
}