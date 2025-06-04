using Agora_RTC_Plugin.API_Example;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;
using io.agora.rtc.demo;

public class SecondMonitor : MonoBehaviour
{
    // UID ktoré bude spojené s týmto monitorom (nastaviť v Unity pre každý monitor object)
    public uint targetUID = 3395;
    
    public Vector2 fixedSize = new Vector2(320, 240);
    
    [HideInInspector]
    public VideoSurface videoSurface;
    
    private RawImage rawImage;
    private RectTransform rectTransform;
    private AspectRatioFitter aspectFitter;
    
    void Awake()
    {
        SetupComponents();
        
        // Registrujeme tento monitor v JoinChannelVideo.cs
        JoinChannelVideo mainController = FindObjectOfType<JoinChannelVideo>();
        if (mainController != null)
        {
            mainController.RegisterAdditionalMonitor(this);
        }
        else
        {
            Debug.LogError("JoinChannelVideo not found in scene. Please check its existence.");
        }
    }
    
    private void SetupComponents()
    {
        rawImage = GetComponent<RawImage>();
        if (rawImage == null)
        {
            rawImage = gameObject.AddComponent<RawImage>();
        }
        
        // Keď nikto nie je pripojený, tak tento objekt nie je vidieť
        rawImage.color = Color.clear;
        if (rawImage.texture == null)
        {
            rawImage.texture = new Texture2D(1, 1);
        }
        
        // Nastavenie fixnej veľkosti
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = fixedSize;
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        aspectFitter = GetComponent<AspectRatioFitter>();
        if (aspectFitter == null)
        {
            aspectFitter = gameObject.AddComponent<AspectRatioFitter>();
            aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            aspectFitter.aspectRatio = fixedSize.x / fixedSize.y;
        }
        
        // Pridávame VideoSurface a nastavujeme aby nemenil veľkosť nášho monitoru
        videoSurface = GetComponent<VideoSurface>();
        if (videoSurface == null)
        {
            videoSurface = gameObject.AddComponent<VideoSurface>();
        }
        videoSurface.SetEnable(true);
        videoSurface.OnTextureSizeModify += (width, height) =>
        {
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = fixedSize;
                rectTransform.localScale = Vector3.one;
                if (aspectFitter != null && width > 0 && height > 0)
                {
                    aspectFitter.aspectRatio = (float)width / height;
                }
                Debug.Log($"Monitor for UID {targetUID} - Size modified: {width}x{height}, maintaining fixed size: {fixedSize.x}x{fixedSize.y}");
            }
        };
    }
    
    // Aby veľkosť zostala rovnaká
    void LateUpdate()
    {
        if (rectTransform != null)
        {
            if (rectTransform.sizeDelta != fixedSize || rectTransform.localScale != Vector3.one)
            {
                rectTransform.sizeDelta = fixedSize;
                rectTransform.localScale = Vector3.one;
            }
        }
    }
    
    // Keď user odíde
    public void OnUserLeft()
    {
        if (rawImage != null)
        {
            rawImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        }
        
        Debug.Log($"User {targetUID} left - cleared monitor display");
    }
    
    // Keď sa user pripojí
    public void OnUserJoined()
    {
        // Nastavíme rawImage farbu na white, aby bola textúra viditeľná po pripojení, a len vtedy, keď je niekto
        // pripojený na tento konkrétny monitor.
        if (rawImage != null)
        {
            rawImage.color = Color.white;
        }
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = fixedSize;
            rectTransform.localScale = Vector3.one;
        }
        
        Debug.Log($"User {targetUID} joined - preparing monitor display");
    }
    
    void OnDestroy()
    {
        JoinChannelVideo mainController = FindObjectOfType<JoinChannelVideo>();
        if (mainController != null)
        {
            mainController.UnregisterAdditionalMonitor(this);
        }
    }
}