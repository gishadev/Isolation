using UnityEngine;

public class InteractionTarget : MonoBehaviour
{
    [Header("Interaction Target")]
    public float interactableRadius;

    public virtual void Interact() { }

    public virtual bool IsReadyForInteraction() { return false; }
}
