using UnityEngine;
using Mirror;

public class Network : NetworkBehaviour
{
    public Behaviour[] arrayComponents;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (Behaviour c in arrayComponents)
            {
                c.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
