using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null; //Closest Interactable
    [SerializeField] private GameObject interactionIcon;

    void Start()
    {
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
            if (interactableInRange != null && !interactableInRange.CanInteract())
            {
                if (interactionIcon != null)
                {
                    interactionIcon.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
            if (interactionIcon != null)
            {
                interactionIcon.SetActive(false);
            }
        }
    }
}