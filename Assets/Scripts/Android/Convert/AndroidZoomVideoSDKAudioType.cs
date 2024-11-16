using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKAudioType
{
	public AndroidZoomVideoSDKAudioType()
	{
	}

    private static AndroidJavaClass audioTypeClass = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_AUDIO_TYPE_PATH);

    private static Dictionary<string, ZMVideoSDKAudioType> audioTypeDic
        = new Dictionary<string, ZMVideoSDKAudioType>()
        {
            {audioTypeClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKAudioType_VOIP").Call<string>("name"), ZMVideoSDKAudioType.ZMVideoSDKAudioType_VOIP},
            {audioTypeClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKAudioType_TELEPHONY").Call<string>("name"), ZMVideoSDKAudioType.ZMVideoSDKAudioType_TELEPHONY},
            {audioTypeClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKAudioType_None").Call<string>("name"), ZMVideoSDKAudioType.ZMVideoSDKAudioType_None},
        };

    public static ZMVideoSDKAudioType GetEnum(string type)
    {
        ZMVideoSDKAudioType audioType;
        try
        {
            audioType = audioTypeDic[type];
        }
        catch (KeyNotFoundException)
        {
            audioType = ZMVideoSDKAudioType.ZMVideoSDKAudioType_None;
        }
        return audioType;
    }
}

