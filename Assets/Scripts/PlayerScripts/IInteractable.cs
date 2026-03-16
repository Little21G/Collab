using UnityEngine;

// Notice this says "interface" instead of "class", and drops the MonoBehaviour!
public interface IInteractable
{
    // Any script using this interface MUST have this method
    void Interact();
}