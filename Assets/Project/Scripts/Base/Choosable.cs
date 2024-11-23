using System;
using UnityEngine;

public class Choosable : MonoBehaviour
{
    [SerializeField] private ParticleSystem _highlight;

    public event Action<bool> Choosed;

    private void Awake()
    {
        _highlight.Stop();
    }

    public void ChangeChosenStatus(bool isChosen)
    {
        Choosed?.Invoke(isChosen);

        if (isChosen)
        {
            Highlight();
        }
        else
        {
            Unhighlight();
        }
    }

    private void Highlight()
    {
        _highlight.Play();
    }

    private void Unhighlight()
    {
        _highlight.Stop();
    }
}