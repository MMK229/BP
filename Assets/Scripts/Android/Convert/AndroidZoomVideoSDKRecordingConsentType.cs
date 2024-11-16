using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKRecordingConsentType
{
    public AndroidZoomVideoSDKRecordingConsentType()
    {
    }

    private static AndroidJavaClass recordingConsentTypeClass = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_RECORDING_CONSENT_TYPE);

    private static Dictionary<string, ZMVideoSDKRecordingConsentType> enumDic
        = new Dictionary<string, ZMVideoSDKRecordingConsentType>()
        {
            {recordingConsentTypeClass.GetStatic<AndroidJavaObject>("ConsentType_Invalid").Call<string>("name"), ZMVideoSDKRecordingConsentType.Invalid},
            {recordingConsentTypeClass.GetStatic<AndroidJavaObject>("ConsentType_Traditional").Call<string>("name"), ZMVideoSDKRecordingConsentType.Traditional},
            {recordingConsentTypeClass.GetStatic<AndroidJavaObject>("ConsentType_Individual").Call<string>("name"), ZMVideoSDKRecordingConsentType.Individual},
        };

    public static ZMVideoSDKRecordingConsentType GetEnum(string type)
    {
        ZMVideoSDKRecordingConsentType consentType;
        try
        {
            consentType = enumDic[type];
        }
        catch (KeyNotFoundException)
        {
            consentType = ZMVideoSDKRecordingConsentType.Invalid;
        }
        return consentType;
    }
}

