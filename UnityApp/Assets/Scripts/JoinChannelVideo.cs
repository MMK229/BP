﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Assertions;
using Unity.Collections;
using Agora.Rtc;
using Agora_RTC_Plugin.API_Example;


// Response, použité pri získavaní tokenu z JSON-u 
[Serializable]
struct Response
{
    public string token;
}

public class JoinChannelVideo : MonoBehaviour
{
    public const uint targetUID = 3394;
    private List<SecondMonitor> additionalMonitors = new List<SecondMonitor>();
    private string token;
    public IRtcEngine RtcEngine;
    public GameObject _videoQualityItemPrefab;
    public Camera sceneCamera;
    uint videoId;
    
    [HideInInspector]
    public VideoSurface primaryVideoSurface;
    public Vector2 fixedSize = new Vector2(320, 240);
    private GameObject primaryVideoView;

    public IEnumerator Start()
    {
        // Zo servera získame token, spracujeme ho, vypíšeme do debug logu a nastavíme ako token
        UnityWebRequest wr = UnityWebRequest.Get("https://tribec.dev/bp/RtcTokenBuilder2Sample.php");
        yield return wr.SendWebRequest();
        token = JsonUtility.FromJson<Response>(wr.downloadHandler.text).token;
        Debug.Log("Token: " + token);
        SharedTokenProvider.SetToken(token);
        
        // Vytvoríme inštanciu Agora RTC
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        // Nastavíme remote event handler a inicializujeme pripojenie
        RtcEngine.InitEventHandler(new MyRtcEngineEventHandler(this));
        InitEngine();
        JoinChannel();
    }

    // Inicializácia Agora enginu
    public void InitEngine()
    {
        // Nastavenie
        AREA_CODE areaCode = AREA_CODE.AREA_CODE_EU;
        RtcEngineContext context = new RtcEngineContext();
        // Toto je ID našej aplikácie, ktorú sme získali z nášho projektu vytvorenom na stránke Agora.io
        context.appId = "42bef5727d2a468190b11cc1d533a7a7";
        // Default nastavenia, špecifikované v rovnomennom kóde JoinChannelVideo.cs od Agora
        context.channelProfile = CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING;
        context.audioScenario = AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT;
        context.areaCode = areaCode;

        var result = RtcEngine.Initialize(context);
        Debug.Log("Initialize result: " + result);

        // Zapneme audio
        RtcEngine.EnableAudio();

        // Nastavíme external video source
        RtcEngine.SetExternalVideoSource(
            enabled: true,
            useTexture: false,
            sourceType: EXTERNAL_VIDEO_SOURCE_TYPE.VIDEO_FRAME,
            encodedVideoOption: new SenderOptions()
        );

        // Zapneme video
        RtcEngine.EnableVideo();
        VideoEncoderConfiguration config = new VideoEncoderConfiguration();
        config.dimensions = new VideoDimensions(640, 360);
        config.frameRate = 15;
        config.bitrate = 0;
        RtcEngine.SetVideoEncoderConfiguration(config);
        RtcEngine.SetChannelProfile(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
    }

    // Pripojenie na kanál
    public void JoinChannel()
    {
        // Vytvoríme vlastný video track
        videoId = RtcEngine.CreateCustomVideoTrack();
        // Overíme platnosť videoID (0xffffffff by znamenala neplatnosť)
        Assert.IsTrue(videoId != 0xffffffff);

        // Options
        // Unity používateľ nebude zdieľať svoju PC webkameru čiže to vypneme, ostatné necháme zapnuté
        ChannelMediaOptions options = new ChannelMediaOptions();
        options.publishMicrophoneTrack.SetValue(true);
        options.publishCameraTrack.SetValue(false);
        options.publishCustomVideoTrack.SetValue(true);
        options.customVideoTrackId.SetValue(videoId);

        // Pripojíme sa na preddefinovaný channel
        // V tomto prototype je channelID hard-codované na hodnotu 7d72365eb983485397e3e3f9d460bdda
        RtcEngine.JoinChannel(token, "7d72365eb983485397e3e3f9d460bdda", 0, options);
    }

    public void StartPreview()
    {
        RtcEngine.StartPreview();
    }

    public void StopPreview()
    {
        DestroyVideoView(0);
        RtcEngine.StopPreview();
    }

    // Vymazávanie videa podľa UID
    internal static void DestroyVideoView(uint uid)
    {
        GameObject go = GameObject.Find(uid.ToString());
        if (go != null)
        {
            GameObject.Destroy(go);
        }
    }

    // Create a video view with styling like SecondMonitor for a specific UID
    internal GameObject CreateVideoViewForUID(uint uid, string channelId = "")
    {
        // Ak už existuje nejaké video pre toto UID tak ho vymažeme
        DestroyVideoView(uid);
        
        GameObject go = new GameObject(uid.ToString());
        RawImage rawImage = go.AddComponent<RawImage>();
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = fixedSize;
        }
        AspectRatioFitter aspectFitter = go.AddComponent<AspectRatioFitter>();
        aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        aspectFitter.aspectRatio = fixedSize.x / fixedSize.y;
        
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        go.AddComponent<UIElementDrag>();

        GameObject canvas = GameObject.Find("VideoCanvas");
        if (canvas != null)
        {
            go.transform.SetParent(canvas.transform);
            Debug.Log($"Attached video view for UID {uid} to canvas");
        }
        
        // Video surface
        if (uid == 0) // Lokálny používateľ
        {
            videoSurface.SetForUser(uid, channelId);
        }
        else // Remote používateľ
        {
            videoSurface.SetForUser(uid, channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
        }
        
        // Override the OnTextureSizeModify to enforce our fixed size
        videoSurface.OnTextureSizeModify += (int width, int height) =>
        {
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = fixedSize;
                rectTransform.localScale = Vector3.one;
                if (aspectFitter != null && width > 0 && height > 0)
                {
                    aspectFitter.aspectRatio = fixedSize.x / fixedSize.y;
                }
                Debug.Log($"Video view for UID {uid} - Size modified: {width}x{height}, maintaining fixed size: {fixedSize.x}x{fixedSize.y}");
            }
        };
        
        // Enable the video surface
        videoSurface.SetEnable(true);
        
        // Store reference if this is the primary video view
        if (uid == targetUID)
        {
            primaryVideoView = go;
            primaryVideoSurface = videoSurface;
        }
        
        return go;
    }
    
    // Odchod z channelu
    public void LeaveChannel()
    {
        RtcEngine.LeaveChannel();
    }

    public void StartPublish()
    {
        ChannelMediaOptions options = new ChannelMediaOptions();
        options.publishMicrophoneTrack.SetValue(true);
        options.publishCameraTrack.SetValue(false);
        options.publishCustomVideoTrack.SetValue(true);
        options.customVideoTrackId.SetValue(videoId);
        int nRet = RtcEngine.UpdateChannelMediaOptions(options);
    }

    public void StopPublish()
    {
        ChannelMediaOptions options = new ChannelMediaOptions();
        options.publishMicrophoneTrack.SetValue(false);
        options.publishCameraTrack.SetValue(false);
        RtcEngine.DestroyCustomVideoTrack(videoId);
        int nRet = RtcEngine.UpdateChannelMediaOptions(options);
    }

    // Toto je pre timestamp, využité v update() funkcii
    private long i;
    
    // Update funkcia, updatjue view externalVideoFrame
    public void Update()
    {
        byte[] _shareData = Array.Empty<byte>();
        
        // Textúru sceneCamery získavame a ukladáme ako byte array.
        Texture2D texture = new Texture2D(sceneCamera.targetTexture.width, sceneCamera.targetTexture.height, TextureFormat.BGRA32, false);
        RenderTexture.active = sceneCamera.targetTexture;
        texture.ReadPixels(new Rect(0, 0, sceneCamera.targetTexture.width, sceneCamera.targetTexture.height), 0, 0);
        texture.Apply();
        NativeArray<byte> nativeByteArray = texture.GetRawTextureData<byte>();
        if (_shareData.Length != nativeByteArray.Length)
        {
            _shareData = new byte[nativeByteArray.Length];
        }
        nativeByteArray.CopyTo(_shareData);
        RenderTexture.active = null;
        
        // Setup a pushovanie ExternalVideoFrame-ov
        // Na konci premazávame textúru aby sme sa vyhli memory leakom
        ExternalVideoFrame externalVideoFrame = new ExternalVideoFrame();
        externalVideoFrame.type = VIDEO_BUFFER_TYPE.VIDEO_BUFFER_RAW_DATA;
        externalVideoFrame.format = VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_BGRA;
        externalVideoFrame.buffer = _shareData;
        externalVideoFrame.stride = sceneCamera.targetTexture.width;
        externalVideoFrame.height = sceneCamera.targetTexture.height;
        externalVideoFrame.cropLeft = 0;
        externalVideoFrame.cropTop = 0;
        externalVideoFrame.cropRight = 0;
        externalVideoFrame.cropBottom = 0;
        externalVideoFrame.rotation = 180;
        externalVideoFrame.timestamp = i++;
        RtcEngine.PushVideoFrame(externalVideoFrame, videoId);
        Destroy(texture);
    }
    
    // Toto asi môžem vymazať
    void LateUpdate()
    {
        if (primaryVideoView != null)
        {
            RectTransform rectTransform = primaryVideoView.GetComponent<RectTransform>();
            if (rectTransform != null && rectTransform.sizeDelta != fixedSize)
            {
                rectTransform.sizeDelta = fixedSize;
                rectTransform.localScale = Vector3.one;
            }
        }
    }

    public void OnDisable()
    {
        StopPreview();
        StopPublish();
        LeaveChannel();
    }

    // Používa sa v kóde SecondMonitor.cs pre registrovanie nového monitora
    public void RegisterAdditionalMonitor(SecondMonitor monitor)
    {
        if (!additionalMonitors.Contains(monitor))
        {
            additionalMonitors.Add(monitor);
            Debug.Log($"Registered additional monitor for UID {monitor.targetUID}");
            
            if (RtcEngine != null)
            {
                SetupMonitorForUser(monitor);
            }
        }
    }

    // Ditto, ale pre vymazanie monitoru
    public void UnregisterAdditionalMonitor(SecondMonitor monitor)
    {
        if (additionalMonitors.Contains(monitor))
        {
            additionalMonitors.Remove(monitor);
            Debug.Log($"Unregistered monitor for UID {monitor.targetUID}");
        }
    }

    private void SetupMonitorForUser(SecondMonitor monitor)
    {
        if (monitor.videoSurface != null)
        {
            // Configure for the remote user
            monitor.videoSurface.SetForUser(monitor.targetUID, "7d72365eb983485397e3e3f9d460bdda", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
        
            // Get the RectTransform and force it to the monitor's fixed size
            RectTransform rectTransform = monitor.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = monitor.fixedSize;
                rectTransform.localScale = Vector3.one;
            }
        
            Debug.Log($"Set up video surface for UID {monitor.targetUID} with fixed size: {monitor.fixedSize.x}x{monitor.fixedSize.y}");
        }
        else
        {
            Debug.LogError($"Video surface not found on monitor for UID {monitor.targetUID}");
        }
    }

    // Nested event handler class to process remote user events.
    internal class MyRtcEngineEventHandler : IRtcEngineEventHandler
    {
        private JoinChannelVideo owner;
        
        public MyRtcEngineEventHandler(JoinChannelVideo owner)
        {
            this.owner = owner;
        }

        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            Debug.Log("Remote user went offline, UID: " + uid);
    
            // Handle the primary target UID
            if (uid == targetUID)
            {
                DestroyVideoView(uid);
            }
    
            // Check if this UID matches any of our additional monitors
            foreach (var monitor in owner.additionalMonitors)
            {
                if (uid == monitor.targetUID)
                {
                    // Call the monitor's method to handle user leaving
                    monitor.OnUserLeft();
                }
            }
        }

        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            Debug.Log("Remote user joined with UID: " + uid);
    
            // Handle the primary target UID
            if (uid == targetUID)
            {
                owner.CreateVideoViewForUID(uid, connection.channelId);
            }
    
            // Check if this UID matches any of our additional monitors
            foreach (var monitor in owner.additionalMonitors)
            {
                if (uid == monitor.targetUID)
                {
                    // Oznámime monitoru, že sa naň niekto pripojil, potom nastavíme video surface
                    monitor.OnUserJoined();
                    owner.SetupMonitorForUser(monitor);
                }
            }
        }
    }
}