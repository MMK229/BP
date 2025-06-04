using UnityEngine;
using Mirror;
using System.IO;

public class Network : NetworkBehaviour
{
    private string logPath;

    void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (Component c in GetComponentsInChildren<Component>(true))
            {
                if (c is Renderer || 
                    c is PlayerIK || 
                    c is NetworkIdentity || 
                    c is NetworkTransformReliable)
                    continue;

                if (c is Camera cam)
                {
                    cam.enabled = false;
                    var listener = cam.GetComponent<AudioListener>();
                    if (listener != null)
                        listener.enabled = false;
                    continue;
                }

                if (c is Behaviour b && ShouldDisable(c))
                {
                    b.enabled = false;
                }
            }
        }
    }

    bool ShouldDisable(Component c)
    {
        return 
            c is CharacterController ||
            c is Animator ||
            c.GetType().Name.Contains("Locomotion") ||
            c.GetType().Name.Contains("Teleport") ||
            c.GetType().Name.Contains("Climb") ||
            c.GetType().Name.Contains("DynamicMoveProvider");
    }

    void Update()
    {
        
    }
}