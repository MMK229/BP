using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKChatMessageDeleteType
{
	public AndroidZoomVideoSDKChatMessageDeleteType()
	{
	}

    private static AndroidJavaClass javaClass = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_CHAT_MESSAGE_DELETE_TYPE_PATH);

    private static Dictionary<string, ZMVideoSDKChatMessageDeleteType> nameDic
        = new Dictionary<string, ZMVideoSDKChatMessageDeleteType>()
        {
            {javaClass.GetStatic<AndroidJavaObject>("SDK_CHAT_DELETE_BY_NONE").Call<string>("name"), ZMVideoSDKChatMessageDeleteType.ByNone},
            {javaClass.GetStatic<AndroidJavaObject>("SDK_CHAT_DELETE_BY_SELF").Call<string>("name"), ZMVideoSDKChatMessageDeleteType.BySelf},
            {javaClass.GetStatic<AndroidJavaObject>("SDK_CHAT_DELETE_BY_HOST").Call<string>("name"), ZMVideoSDKChatMessageDeleteType.ByHost},
            {javaClass.GetStatic<AndroidJavaObject>("SDK_CHAT_DELETE_BY_DLP").Call<string>("name"), ZMVideoSDKChatMessageDeleteType.ByDLP},
        };

    public static ZMVideoSDKChatMessageDeleteType GetEnum(string javaName)
    {
        ZMVideoSDKChatMessageDeleteType rawDataTypeEnum;
        try
        {
            rawDataTypeEnum = nameDic[javaName];
        }
        catch (KeyNotFoundException)
        {
            rawDataTypeEnum = ZMVideoSDKChatMessageDeleteType.ByNone;
        }
        return rawDataTypeEnum;
    }
}

