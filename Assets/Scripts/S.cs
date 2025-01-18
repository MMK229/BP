/*using UnityEngine;
using // Assuming the Unity Plugin namespace is ZMVideoSDK.

public class ZoomSDKInitializer : MonoBehaviour
{
    void Start()
    {
        InitializeZoomSDK();
    }

    void InitializeZoomSDK()
    {
        // Create the initialization parameters
        ZMVideoSDKInitParams initParams = new ZMVideoSDKInitParams
        {
            domain = "https://zoom.us", // Set the domain to Zoom's URL
            enableLog = true           // Enable logging for debugging
        };

        // Initialize the SDK
        ZMVideoSDK.Instance.Initialize(initParams, (result) =>
        {
            if (result == ZMVideoSDKError.SUCCESS)
            {
                Debug.Log("Zoom SDK Initialized Successfully!");
            }
            else
            {
                Debug.LogError($"Zoom SDK Initialization Failed: {result}");
            }
        });
    }
}*/