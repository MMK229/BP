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
    private IRtcEngine RtcEngine = null;
    public GameObject _videoQualityItemPrefab;
    public Camera sceneCamera;
    uint videoId;

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
        //StartPreview();
        JoinChannel();
        // You can call StartPublish() if needed
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

        // For local preview, create a view for the local user (UID 0)
        //GameObject localNode = MakeVideoView(0);
        //CreateLocalVideoCallQualityPanel(localNode);
    }

    public void StartPreview()
    {
        GameObject localNode = MakeVideoView(0);
        CreateLocalVideoCallQualityPanel(localNode);
        RtcEngine.StartPreview();
    }

    public void StopPreview()
    {
        DestroyVideoView(0);
        RtcEngine.StopPreview();
    }

    internal static void DestroyVideoView(uint uid)
    {
        GameObject go = GameObject.Find(uid.ToString());
        if (go != null)
        {
            GameObject.Destroy(go);
        }
    }

    /// <summary>
    /// Create the video view only for the local user (uid == 0) or the target remote user (targetUID)
    /// </summary>
    /// <param name="uid">UID of the user</param>
    /// <param name="channelId">Channel name</param>
    internal static GameObject MakeVideoView(uint uid, string channelId = "")
    {
        // Only allow local (uid == 0) or remote with targetUID to show video.
        if (uid != targetUID && uid != 0)
            return null;

        GameObject go = GameObject.Find(uid.ToString());
        if (go != null)
        {
            return go; // Reuse the existing GameObject
        }

        // Create an image surface to render video
        VideoSurface videoSurface = MakeImageSurface(uid.ToString());
        if (videoSurface == null) 
            return null;

        // Assign for local or remote user
        if (uid == 0)
        {
            videoSurface.SetForUser(uid, channelId);
        }
        else
        {
            videoSurface.SetForUser(uid, channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
        }

        videoSurface.OnTextureSizeModify += (int width, int height) =>
        {
            RectTransform rect = videoSurface.GetComponent<RectTransform>();
            if (rect)
            {
                // If rendered in RawImage, set size accordingly.
                rect.sizeDelta = new Vector2(width / 2, height / 2);
                rect.localScale = Vector3.one;
            }
            else
            {
                // If rendered in MeshRenderer, adjust scale.
                float scale = (float)height / (float)width;
                videoSurface.transform.localScale = new Vector3(-1, 1, scale);
            }
            Debug.Log("OnTextureSizeModify: " + width + " " + height);
        };

        videoSurface.SetEnable(true);
        return videoSurface.gameObject;
    }

    private static VideoSurface MakeImageSurface(string goName)
    {
        GameObject go = new GameObject();
        go.name = goName;

        // Add RawImage component to render the video
        go.AddComponent<RawImage>();
        // Optional: make the object draggable (if needed)
        go.AddComponent<UIElementDrag>();

        // Find the canvas in the scene to attach this video view
        GameObject canvas = GameObject.Find("VideoCanvas");
        if (canvas != null)
        {
            go.transform.SetParent(canvas.transform);
            Debug.Log("Attached video view to canvas");
        }
        else
        {
            Debug.Log("Canvas not found");
        }

        // Set up transform defaults
        go.transform.Rotate(0f, 0f, 180f);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = new Vector3(2f, 3f, 1f);

        // Add VideoSurface component for Agora
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
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
    
            // Handle the primary target UID as before
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
    
            // Handle the primary target UID as before
            if (uid == targetUID)
            {
                MakeVideoView(uid, connection.channelId);
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