using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InteractionDetector))]
public class InteractionInputHandler : MonoBehaviour
{
    private KentInputAction inputActions;
    private InteractionDetector interactionDetector;

    private void Awake()
    {
        inputActions = new KentInputAction();
        interactionDetector = GetComponent<InteractionDetector>();

        if (interactionDetector == null)
        {
            Debug.LogError("InteractionDetector component is missing on " + gameObject.name);
        }
    }

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new KentInputAction();
        }

        inputActions.Kent.Interact.performed += OnInteractPerformed;
        inputActions.Kent.Enable();
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Kent.Interact.performed -= OnInteractPerformed;
            inputActions.Kent.Disable();
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (interactionDetector == null)
        {
            Debug.LogError("InteractionDetector is null when trying to interact");
            return;
        }

        interactionDetector.OnInteract(context);
    }
}