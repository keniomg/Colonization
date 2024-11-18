using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Choosable : MonoBehaviour
{
    [SerializeField] private ParticleSystem _highlight;

    private PlayerInput _playerInput;

    public void ChangeChosenStatus(bool isChosen)
    {
        if (isChosen)
        {
            _playerInput.Enable();
            Highlight();
        }
        else
        {
            _playerInput.Disable();
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