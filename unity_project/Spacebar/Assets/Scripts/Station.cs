using UnityEngine;

public abstract class Station : MonoBehaviour, IInteractable
{
    [Header("Station Settings")]
    [SerializeField] protected string stationName = "Station";
    [SerializeField] protected string interactionPrompt = "Press E to interact";
    [SerializeField] protected float interactionDuration = 0f;

    [Header("Visual Feedback")]
    [SerializeField] protected GameObject highlightObject;
    [SerializeField] protected Material highlightMaterial;

    protected bool isBeingLookedAt = false;
    protected bool isBeingInteracted = false;
    protected float interactionTimer = 0f;

    protected virtual void Update()
    {
        if (isBeingInteracted && interactionDuration > 0)
        {
            interactionTimer += Time.deltaTime;

            if (interactionTimer >= interactionDuration)
            {
                OnInteractionComplete();
                interactionTimer = 0f;
                isBeingInteracted = false;
            }
        }
    }

    public virtual void OnLookEnter()
    {
        isBeingLookedAt = true;
        InteractionPromptUI.ShowPrompt(GetInteractionPrompt());

        if (highlightObject != null)
        {
            highlightObject.SetActive(true);
        }
    }

    public virtual void OnLookExit()
    {
        isBeingLookedAt = false;
        InteractionPromptUI.HidePrompt();

        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
    }

    public virtual void OnInteract(GameObject player)
    {
        isBeingInteracted = true;
        interactionTimer = 0f;
        AudioManager.Instance?.PlaySoundOneShot("Interact");
    }

    public virtual void OnInteractEnd(GameObject player)
    {
        isBeingInteracted = false;
        interactionTimer = 0f;
    }

    public virtual string GetInteractionPrompt()
    {
        return interactionPrompt;
    }

    protected abstract void OnInteractionComplete();
}
