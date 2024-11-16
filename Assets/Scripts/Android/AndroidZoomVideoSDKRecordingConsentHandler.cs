using System;
using UnityEngine;

public class AndroidZoomVideoSDKRecordingConsentHandler
{
    private AndroidJavaObject _recordingConsentHandler;

    public AndroidZoomVideoSDKRecordingConsentHandler(AndroidJavaObject recordingConsentHandler)
	{
        _recordingConsentHandler = recordingConsentHandler;
	}

    public bool Accept()
    {
        if (_recordingConsentHandler != null)
        {
            return _recordingConsentHandler.Call<bool>("accept"); 
        }
        throw new NullReferenceException("ZoomVideoSDKRecordingConsentHandler instance is null");
    }

    public bool Decline()
    {
        if (_recordingConsentHandler != null)
        {
            return _recordingConsentHandler.Call<bool>("decline");
        }
        throw new NullReferenceException("ZoomVideoSDKRecordingConsentHandler instance is null");
    }

    public ZMVideoSDKRecordingConsentType GetConsentType()
    {
        if (_recordingConsentHandler != null)
        {
            return AndroidZoomVideoSDKRecordingConsentType.GetEnum(_recordingConsentHandler.Call<AndroidJavaObject>("getConsentType").Call<string>("name"));
        }
        throw new NullReferenceException("ZoomVideoSDKRecordingConsentHandler instance is null");
    }
}

