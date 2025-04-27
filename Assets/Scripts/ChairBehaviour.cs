using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem; // Make sure Input System package is installed
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets; // If using XRI Starter Assets for locomotion

public class ChairBehaviour : MonoBehaviour
{
    public Transform sitTarget; // Assign the SitTarget empty GameObject here in the Inspector
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor leftHandInteractor; // Assign Left Controller Interactor
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor rightHandInteractor; // Assign Right Controller Interactor
    public InputActionProperty standUpAction; // Assign an Input Action (e.g., Jump button) in the Inspector

    private GameObject xrOriginInstance = null;
    private LocomotionSystem locomotionSystem; // Reference to disable/enable movement
    // Or get specific providers if not using LocomotionSystem:
    // private ActionBasedContinuousMoveProvider continuousMoveProvider;
    // private TeleportationProvider teleportationProvider;

    private bool isSeated = false;
    private XRSimpleInteractable simpleInteractable;

    void Start()
    {
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        if (simpleInteractable == null)
        {
            Debug.LogError("SittableChair script requires an XRSimpleInteractable component on the same GameObject.", this);
            enabled = false;
            return;
        }

        // Find the XR Origin and Locomotion System dynamically
        // Note: Finding objects this way can be slow; consider assigning them directly if possible.
        XROrigin origin = FindObjectOfType<XROrigin>();
        if (origin != null)
        {
            xrOriginInstance = origin.gameObject;
            locomotionSystem = xrOriginInstance.GetComponent<LocomotionSystem>();
            // Or find individual providers:
            // continuousMoveProvider = xrOriginInstance.GetComponentInChildren<ActionBasedContinuousMoveProvider>();
            // teleportationProvider = xrOriginInstance.GetComponentInChildren<TeleportationProvider>();
        }
        else
        {
             Debug.LogError("Could not find XR Origin in the scene.", this);
             enabled = false;
             return;
        }


        // Subscribe to the select event
        simpleInteractable.selectEntered.AddListener(SitDown);

         // Ensure stand up action is enabled
        if (standUpAction != null && standUpAction.action != null)
        {
            standUpAction.action.Enable();
            standUpAction.action.performed += CheckStandUp;
        }
    }

     void OnDestroy()
    {
        // Unsubscribe from events to prevent errors
        if (simpleInteractable != null)
        {
            simpleInteractable.selectEntered.RemoveListener(SitDown);
        }
         if (standUpAction != null && standUpAction.action != null)
        {
             standUpAction.action.performed -= CheckStandUp;
        }
    }

    // Called when the chair is selected via the XRSimpleInteractable
    private void SitDown(SelectEnterEventArgs args)
    {
        if (!isSeated && xrOriginInstance != null && sitTarget != null)
        {
            isSeated = true;

            // Disable locomotion
            if (locomotionSystem != null) locomotionSystem.enabled = false;
            // Or disable individual providers:
            // if (continuousMoveProvider != null) continuousMoveProvider.enabled = false;
            // if (teleportationProvider != null) teleportationProvider.enabled = false;

            // Disable interactors temporarily to prevent grabbing while moving
            if(leftHandInteractor) leftHandInteractor.enabled = false;
            if(rightHandInteractor) rightHandInteractor.enabled = false;


            // Move the XR Origin to the sit target position and rotation
            // Match position exactly, but only match Y rotation to allow looking around
            float yRotation = sitTarget.rotation.eulerAngles.y;
            xrOriginInstance.transform.position = sitTarget.position;
            xrOriginInstance.transform.rotation = Quaternion.Euler(0, yRotation, 0);


            // Re-enable interactors after a short delay (optional, adjust as needed)
            Invoke(nameof(EnableInteractors), 0.1f);


            Debug.Log("Player sat down.");
        }
    }

     private void EnableInteractors()
    {
        if(leftHandInteractor) leftHandInteractor.enabled = true;
        if(rightHandInteractor) rightHandInteractor.enabled = true;
    }


    // Checks if the stand up action was performed while seated
    private void CheckStandUp(InputAction.CallbackContext context)
    {
        if (isSeated)
        {
            StandUp();
        }
    }

    private void StandUp()
    {
         if (xrOriginInstance != null)
         {
            isSeated = false;

            // Re-enable locomotion
            if (locomotionSystem != null) locomotionSystem.enabled = true;
            // Or enable individual providers:
            // if (continuousMoveProvider != null) continuousMoveProvider.enabled = true;
            // if (teleportationProvider != null) teleportationProvider.enabled = true;

             // Optional: Move the player slightly up/back to avoid immediate re-sitting
            xrOriginInstance.transform.position += Vector3.up * 0.1f;

            Debug.Log("Player stood up.");
         }
    }
}