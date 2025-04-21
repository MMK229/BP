using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Assertions;
using Unity.Collections;
using Agora.Rtc;
using io.agora.rtc.demo;
using Agora_RTC_Plugin.API_Example;
using Agora_RTC_Plugin.API_Example.Examples.Basic.JoinChannelVideo;


[System.Serializable]
struct Response
{
    public string token;
}

public class CoolJoinChannelVideo : MonoBehaviour
{
    // Change this constant to the UID you want to display
    public const uint targetUID = 3394;
    private List<SecondMonitor> additionalMonitors = new List<SecondMonitor>();
    private string token;
    public IRtcEngine RtcEngine = null;
    public GameObject _videoQualityItemPrefab;
    public Camera sceneCamera;
    uint videoId;
    
    // Add fields to match SecondMonitor's visual components
    [HideInInspector]
    public VideoSurface primaryVideoSurface;
    
    // Add fixed size field similar to SecondMonitor
    public Vector2 fixedSize = new Vector2(320, 240);
    
    // GameObject to display the primary target UID's video
    private GameObject primaryVideoView;

    public IEnumerator Start()
    {
        // Get the token from your server
        UnityWebRequest wr = UnityWebRequest.Get("https://tribec.dev/bp/RtcTokenBuilder2Sample.php");
        yield return wr.SendWebRequest();
        token = JsonUtility.FromJson<Response>(wr.downloadHandler.text).token;
        Debug.Log("Token: " + token);
        SharedTokenProvider.SetToken(token);
        
        // Create Agora engine instance and set the event handler
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        // Set the event handler to receive remote user events
        RtcEngine.InitEventHandler(new MyRtcEngineEventHandler(this));

        InitEngine();
        JoinChannel();
    }

    public void InitEngine()
    {
        AREA_CODE areaCode = AREA_CODE.AREA_CODE_EU;
        RtcEngineContext context = new RtcEngineContext();
        context.appId = "42bef5727d2a468190b11cc1d533a7a7";
        context.channelProfile = CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING;
        context.audioScenario = AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT;
        context.areaCode = areaCode;

        var result = RtcEngine.Initialize(context);
        Debug.Log("Initialize result: " + result);

        // Enable audio
        RtcEngine.EnableAudio();

        // Set up external video source
        RtcEngine.SetExternalVideoSource(
            enabled: true,
            useTexture: false,
            sourceType: EXTERNAL_VIDEO_SOURCE_TYPE.VIDEO_FRAME,
            encodedVideoOption: new SenderOptions()
        );

        // Enable video and set encoder configuration
        RtcEngine.EnableVideo();
        VideoEncoderConfiguration config = new VideoEncoderConfiguration();
        config.dimensions = new VideoDimensions(640, 360);
        config.frameRate = 15;
        config.bitrate = 0;
        RtcEngine.SetVideoEncoderConfiguration(config);

        // Set channel and client role; in this sample everyone is a broadcaster.
        RtcEngine.SetChannelProfile(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
    }

    public void JoinChannel()
    {
        // Create the custom video track
        videoId = RtcEngine.CreateCustomVideoTrack();
        Assert.IsTrue(videoId != 0xffffffff);

        ChannelMediaOptions options = new ChannelMediaOptions();
        options.publishMicrophoneTrack.SetValue(true);
        options.publishCameraTrack.SetValue(false);
        options.publishCustomVideoTrack.SetValue(true);
        options.customVideoTrackId.SetValue(videoId);

        // Use your desired channel name; this example uses a hard-coded string.
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

    // Destroy any existing video view for a UID
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
        // Destroy any existing view for this UID
        DestroyVideoView(uid);
        
        // Create a new GameObject for this UID
        GameObject go = new GameObject(uid.ToString());
        
        // Get or add a RawImage component
        RawImage rawImage = go.AddComponent<RawImage>();
        
        // Get or add a RectTransform and set its fixed size
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = fixedSize;
        }
        
        // Add AspectRatioFitter to maintain aspect ratio
        AspectRatioFitter aspectFitter = go.AddComponent<AspectRatioFitter>();
        aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        aspectFitter.aspectRatio = fixedSize.x / fixedSize.y;
        
        // Add the VideoSurface component
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        
        // Add draggable component
        go.AddComponent<UIElementDrag>();
        
        // Find the canvas in the scene to attach this video view
        GameObject canvas = GameObject.Find("VideoCanvas");
        if (canvas != null)
        {
            go.transform.SetParent(canvas.transform);
            Debug.Log($"Attached video view for UID {uid} to canvas");
        }
        
        // Configure the video surface
        if (uid == 0) // Local user
        {
            videoSurface.SetForUser(uid, channelId);
        }
        else // Remote user
        {
            videoSurface.SetForUser(uid, channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
        }
        
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

    // For backward compatibility - redirect to the new method
    internal static GameObject MakeVideoView(uint uid, string channelId = "")
    {
        CoolJoinChannelVideo instance = FindObjectOfType<CoolJoinChannelVideo>();
        if (instance != null)
        {
            return instance.CreateVideoViewForUID(uid, channelId);
        }
        return null;
    }

    public void CreateLocalVideoCallQualityPanel(GameObject parent)
    {
        if (parent == null)
            return;

        if (parent.GetComponentInChildren<LocalVideoCallQualityPanel>() != null)
            return;

        GameObject panel = GameObject.Instantiate(this._videoQualityItemPrefab, parent.transform);
        panel.AddComponent<LocalVideoCallQualityPanel>();
    }

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

    private long i;
    public void Update()
    {
        // Capture current frame data from the camera's render texture.
        byte[] _shareData = Array.Empty<byte>();

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
    
    // Add LateUpdate to ensure sizing is maintained
    void LateUpdate()
    {
        // Ensure primary video view maintains its fixed size
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

    public void RegisterAdditionalMonitor(SecondMonitor monitor)
    {
        if (!additionalMonitors.Contains(monitor))
        {
            additionalMonitors.Add(monitor);
            Debug.Log($"Registered additional monitor for UID {monitor.targetUID}");
        
            // If we're already connected to the channel, set up this monitor
            if (RtcEngine != null)
            {
                SetupMonitorForUser(monitor);
            }
        }
    }

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
        private CoolJoinChannelVideo owner;

        public MyRtcEngineEventHandler(CoolJoinChannelVideo owner)
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
                    // First notify the monitor that its user has joined
                    monitor.OnUserJoined();
                    // Then set up the video surface
                    owner.SetupMonitorForUser(monitor);
                }
            }
        }
    }
}