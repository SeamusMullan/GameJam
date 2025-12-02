using UnityEngine;

public interface IInteractable
{
    void OnLookEnter();
    void OnLookExit();
    void OnInteract(GameObject player);
    void OnInteractEnd(GameObject player);
    string GetInteractionPrompt();
}
