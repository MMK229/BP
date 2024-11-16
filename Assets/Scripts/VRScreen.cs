using UnityEngine;

public class VRScreen : MonoBehaviour
{
    
    public Renderer rend;
    private UnityChatSDK sdk;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sdk = UnityChatSDK.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        rend.material.mainTexture = sdk.GetPeerTexture(0).Texture;
    }
}
