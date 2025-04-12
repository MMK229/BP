using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Agora.Rtc;
using io.agora.rtc.demo;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks.Sources;
using Agora_RTC_Plugin.API_Example;
using Agora_RTC_Plugin.API_Example.Examples.Basic.JoinChannelVideo;
using Mirror.Examples.CharacterSelection;
using TMPro.SpriteAssetUtilities;
using Unity.Collections;
using UnityEditor;
using UnityEngine.Assertions;
using UnityEngine.Networking;


[System.Serializable]
struct Response
{
    public string token;
}

public class CoolJoinChannelVideo : MonoBehaviour
{
    private string token;
    private IRtcEngine RtcEngine = null;
    public GameObject _videoQualityItemPrefab;
    public Camera sceneCamera;
    
    
    public IEnumerator Start()
    {
        var wr = UnityWebRequest.Get("http://127.0.0.1/RTCTokenBuilder2Sample.php");
        wr.SendWebRequest();
        while (!wr.isDone)
        {
            yield return null;
        }
        token = JsonUtility.FromJson<Response>(wr.downloadHandler.text).token;
        print(token);
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        InitEngine();
        StartPreview();
        JoinChannel();
        //StartPublish();
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
        
        RtcEngine.EnableAudio();
        
        RtcEngine.SetExternalVideoSource(
            enabled: true,
            useTexture: false,
            sourceType: EXTERNAL_VIDEO_SOURCE_TYPE.VIDEO_FRAME,
            encodedVideoOption: new SenderOptions()
        );
        RtcEngine.EnableVideo();
        VideoEncoderConfiguration config = new VideoEncoderConfiguration();
        config.dimensions = new VideoDimensions(640, 360);
        config.frameRate = 15;
        config.bitrate = 0;
        RtcEngine.SetVideoEncoderConfiguration(config);
        RtcEngine.SetChannelProfile(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
    }
    
    public void JoinChannel()
    {
        videoId = RtcEngine.CreateCustomVideoTrack();
        Assert.IsTrue(videoId != 0xffffffff);
        var options = new ChannelMediaOptions();
        options.publishMicrophoneTrack.SetValue(true);
        options.publishCameraTrack.SetValue(false);
        options.publishCustomVideoTrack.SetValue(true);
        //options.defaultVideoStreamType.SetValue();
        options.customVideoTrackId.SetValue(videoId);
        // TODO: dynamic channelId
        RtcEngine.JoinChannel(token, "7d72365eb983485397e3e3f9d460bdda", 0, options);
        var node = MakeVideoView(0);
        CreateLocalVideoCallQualityPanel(node);
    }
    
    public void StartPreview()
    {
        
        var node = MakeVideoView(0);
        CreateLocalVideoCallQualityPanel(node);
        RtcEngine.StartPreview();
    }

    internal static void DestroyVideoView(uint uid)
    {
        var go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            Destroy(go);
        }
    }
    
    public void StopPreview()
    {
        DestroyVideoView(0);
        RtcEngine.StopPreview();
    }
    
    internal static GameObject MakeVideoView(uint uid, string channelId = "")
    {
        var go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            return go; // reuse
        }

        // create a GameObject and assign to this new user
        var videoSurface = MakeImageSurface(uid.ToString());
        if (ReferenceEquals(videoSurface, null)) return null;
        // configure videoSurface
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
            var transform = videoSurface.GetComponent<RectTransform>();
            if (transform)
            {
                //If render in RawImage. just set rawImage size.
                transform.sizeDelta = new Vector2(width / 2, height / 2);
                transform.localScale = Vector3.one;
            }
            else
            {
                //If render in MeshRenderer, just set localSize with MeshRenderer
                float scale = (float)height / (float)width;
                videoSurface.transform.localScale = new Vector3(-1, 1, scale);
            }
            Debug.Log("OnTextureSizeModify: " + width + "  " + height);
        };

        videoSurface.SetEnable(true);
        return videoSurface.gameObject;
    }
    
    private static VideoSurface MakeImageSurface(string goName)
    {
        GameObject go = new GameObject();

        if (go == null)
        {
            return null;
        }

        go.name = goName;
        // to be renderered onto
        go.AddComponent<RawImage>();
        // make the object draggable
        go.AddComponent<UIElementDrag>();
        var canvas = GameObject.Find("VideoCanvas");
        if (canvas != null)
        {
            go.transform.parent = canvas.transform;
            Debug.Log("add video view");
        }
        else
        {
            Debug.Log("Canvas is null video view");
        }

        // set up transform
        go.transform.Rotate(0f, 0.0f, 180.0f);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = new Vector3(2f, 3f, 1f);

        // configure videoSurface
        var videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }
    
    public void CreateLocalVideoCallQualityPanel(GameObject parent)
    {
        if (parent.GetComponentInChildren<LocalVideoCallQualityPanel>() != null)
            return;

        var panel = GameObject.Instantiate(this._videoQualityItemPrefab, parent.transform);
        panel.AddComponent<LocalVideoCallQualityPanel>();

    }
    
    public void LeaveChannel()
    {
        RtcEngine.LeaveChannel();
    }
    
    uint videoId;
    
    public void StartPublish()
    {
        var options = new ChannelMediaOptions();
        options.publishMicrophoneTrack.SetValue(true);
        options.publishCameraTrack.SetValue(false);
        options.publishCustomVideoTrack.SetValue(true);
        //options.defaultVideoStreamType.SetValue();
        options.customVideoTrackId.SetValue(videoId);
        var nRet = RtcEngine.UpdateChannelMediaOptions(options);
    }
    
    public void StopPublish()
    {
        var options = new ChannelMediaOptions();
        options.publishMicrophoneTrack.SetValue(false);
        options.publishCameraTrack.SetValue(false);
        RtcEngine.DestroyCustomVideoTrack(videoId);
        var nRet = RtcEngine.UpdateChannelMediaOptions(options);
    }

    private long i;
    public void Update()
    {
        byte[] _shareData = Array.Empty<byte>();
        
        Texture2D texture = new Texture2D(sceneCamera.targetTexture.width, sceneCamera.targetTexture.height, TextureFormat.BGRA32, false);
        RenderTexture.active = sceneCamera.targetTexture;
        texture.ReadPixels(new Rect(0, 0, sceneCamera.targetTexture.width, sceneCamera.targetTexture.height), 0, 0);
        texture.Apply();
        NativeArray<byte> nativeByteArray = texture.GetRawTextureData<byte>();
        if (_shareData?.Length != nativeByteArray.Length)
        { 
            _shareData = new byte[nativeByteArray.Length];
        }
        nativeByteArray.CopyTo(_shareData);
        RenderTexture.active = null;
        
        ExternalVideoFrame externalVideoFrame = new ExternalVideoFrame();
        // Set the buffer type of the video frame
        externalVideoFrame.type = VIDEO_BUFFER_TYPE.VIDEO_BUFFER_RAW_DATA;
        // Set the video pixel format
        externalVideoFrame.format = VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_BGRA;
        // apply raw data you are pulling from the rectangle you created earlier to the video frame
        externalVideoFrame.buffer = _shareData;
        //Set the width of the video frame (in pixels)
        externalVideoFrame.stride = sceneCamera.targetTexture.width;
        //Set the height of the video frame
        externalVideoFrame.height = sceneCamera.targetTexture.height;
        //Remove pixels from the sides of the frame
        externalVideoFrame.cropLeft = 0;
        externalVideoFrame.cropTop = 0;
        externalVideoFrame.cropRight = 0;
        externalVideoFrame.cropBottom = 0;
        // Rotate the video frame (0, 90, 180, or 270)
        externalVideoFrame.rotation = 180;
        // Increment i with the video timestamp
        externalVideoFrame.timestamp = i++;
        // Push the external video frame with the frame we just created
        RtcEngine.PushVideoFrame(externalVideoFrame, videoId);
    
    }
    
    public void OnDisable()
    {
        StopPreview();
        StopPublish();
        LeaveChannel();
    }
}