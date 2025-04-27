using Agora_RTC_Plugin.API_Example;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;
using io.agora.rtc.demo;

public class SecondMonitor : MonoBehaviour
{
    // The target UID this monitor should display
    public uint targetUID = 3395;
    
    // Control the exact size of the monitor
    public Vector2 fixedSize = new Vector2(320, 240);
    
    // Reference to the video surface component
    [HideInInspector]
    public VideoSurface videoSurface;
    
    private RawImage rawImage;
    private RectTransform rectTransform;
    private AspectRatioFitter aspectFitter;
    
    void Awake()
    {
        // Set up components
        SetupComponents();
        
        // Register this monitor with the main controller
        CoolJoinChannelVideo mainController = FindObjectOfType<CoolJoinChannelVideo>();
        if (mainController != null)
        {
            mainController.RegisterAdditionalMonitor(this);
        }
        else
        {
            Debug.LogError("CoolJoinChannelVideo not found in scene. Make sure it exists before this monitor.");
        }
    }
    
    private void SetupComponents()
    {
        // Get or add a RawImage component
        rawImage = GetComponent<RawImage>();
        if (rawImage == null)
        {
            rawImage = gameObject.AddComponent<RawImage>();
        }
        rawImage.color = Color.clear;
        // Get or add a RectTransform and set its fixed size
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = fixedSize;
        }
        
        // Add AspectRatioFitter to maintain aspect ratio
        aspectFitter = GetComponent<AspectRatioFitter>();
        if (aspectFitter == null)
        {
            aspectFitter = gameObject.AddComponent<AspectRatioFitter>();
            aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            aspectFitter.aspectRatio = fixedSize.x / fixedSize.y;
        }
        
        // Add the VideoSurface component
        videoSurface = gameObject.AddComponent<VideoSurface>();
        
        // Enable the video surface
        videoSurface.SetEnable(true);
        
        // Override the OnTextureSizeModify to enforce our fixed size
        videoSurface.OnTextureSizeModify += (int width, int height) =>
        {
            if (rectTransform != null)
            {
                // Force back to our fixed size
                rectTransform.sizeDelta = fixedSize;
                rectTransform.localScale = Vector3.one;
                
                // Update aspect ratio if needed
                if (aspectFitter != null && width > 0 && height > 0)
                {
                    aspectFitter.aspectRatio = fixedSize.x / fixedSize.y;
                }
                
                Debug.Log($"Monitor for UID {targetUID} - Size modified: {width}x{height}, maintaining fixed size: {fixedSize.x}x{fixedSize.y}");
            }
        };
    }
    
    // Called after every frame update to ensure size remains fixed
    void LateUpdate()
    {
        // Ensure our fixed size is maintained
        if (rectTransform != null)
        {
            if (rectTransform.sizeDelta != fixedSize || rectTransform.localScale != Vector3.one)
            {
                rectTransform.sizeDelta = fixedSize;
                rectTransform.localScale = Vector3.one;
            }
        }
    }
    
    // Method to be called when the user leaves
    public void OnUserLeft()
    {
        if (rawImage != null)
        {
            // Clear the texture by disabling the raw image
            rawImage.enabled = false;
        }
        
        Debug.Log($"User {targetUID} left - cleared monitor display");
    }
    
    // Method to be called when the user joins/returns
    public void OnUserJoined()
    {
        if (rawImage != null)
        {
            rawImage.enabled = true;
        }
        
        // Force size to our fixed size again
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = fixedSize;
            rectTransform.localScale = Vector3.one;
        }
        
        Debug.Log($"User {targetUID} joined - preparing monitor display");
    }
    
    void OnDestroy()
    {
        // Unregister from the main controller
        CoolJoinChannelVideo mainController = FindObjectOfType<CoolJoinChannelVideo>();
        if (mainController != null)
        {
            mainController.UnregisterAdditionalMonitor(this);
        }
    }
}