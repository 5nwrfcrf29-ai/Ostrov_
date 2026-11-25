using UnityEngine;
using TMPro; // Include if you use TextMeshPro for UI prompts

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public Camera playerCamera;

    [Header("UI (Optional)")]
    public TextMeshProUGUI promptText;

    [Header("References")]
    public PlayerMove playerMove;
    private IInteractable currentInteractable;



    void Update()
    {
        CheckForInteractables();
        // When 'E' is pressed and we are looking at an interactable item
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact(playerMove);
        }
    }

    void CheckForInteractables()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                currentInteractable = interactable;
                if (promptText != null)
                {
                    promptText.text = interactable.GetInteractionPrompt();
                    promptText.gameObject.SetActive(true);
                }
                return;
            }
        }

        // Clear prompt if nothing is hit
        currentInteractable = null;
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }

}