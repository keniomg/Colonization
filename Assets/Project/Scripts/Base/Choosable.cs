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
        Highlight(isChosen);
    }

    private void Highlight(bool isPlay)
    {
        if (isPlay)
        {
            _highlight.Play();
        }
        else
        {
            _highlight.Stop();
        }
    }
}