using System;
using UnityEngine;
using UnityEngine.UI;

public class Peer : MonoBehaviour
{
    public RawImage rawImage;
    private UnityChatSDK _sdk;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sdk = UnityChatSDK.Instance;
        _sdk.InitVideo(0);
        _sdk.SetAudioEnable(true);
        /*
        if (Microphone.devices.Length > 0)
        {
            Debug.Log("Microphone detected: " + Microphone.devices[0]);
            _sdk.InitMic(0);
        }
        else
        {
            Debug.LogError("No microphone detected.");
        }
        */
        _sdk.SetVideoEnable(true);
        _sdk.SetVideoCaptureType(VideoType.DeviceCamera);
        _sdk.SetVideoQuality(VideoQuality.High);
        _sdk.SetResolution(VideoResolution._720P);
        _sdk.StartCapture();
    }

    // Update is called once per frame
    void Update()
    {
        rawImage.texture = _sdk.GetSelfTexture().Texture;
    }

    private void OnDisable()
    {
        _sdk.StopCapture();
    }
}
