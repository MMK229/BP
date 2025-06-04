using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKShareStatus
{
	public AndroidZoomVideoSDKShareStatus()
	{
	}

    private static AndroidJavaClass shareStatusClass = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_SHARE_STATUS_PATH);

    private static Dictionary<string, ZMVideoSDKShareStatus> shareStatusDic
        = new Dictionary<string, ZMVideoSDKShareStatus>()
        {
            {shareStatusClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKShareStatus_None").Call<string>("name"), ZMVideoSDKShareStatus.ZMVideoSDKShareStatus_None},
            {shareStatusClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKShareStatus_Start").Call<string>("name"), ZMVideoSDKShareStatus.ZMVideoSDKShareStatus_Start},
            {shareStatusClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKShareStatus_Pause").Call<string>("name"), ZMVideoSDKShareStatus.ZMVideoSDKShareStatus_Pause},
            {shareStatusClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKShareStatus_Resume").Call<string>("name"), ZMVideoSDKShareStatus.ZMVideoSDKShareStatus_Resume},
            {shareStatusClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKShareStatus_Stop").Call<string>("name"), ZMVideoSDKShareStatus.ZMVideoSDKShareStatus_Stop},
        };

    public static ZMVideoSDKShareStatus GetEnum(string status)
    {
        ZMVideoSDKShareStatus shareStatus;
        try
        {
            shareStatus = shareStatusDic[status];
        }
        catch (KeyNotFoundException)
        {
            shareStatus = ZMVideoSDKShareStatus.ZMVideoSDKShareStatus_None;
        }
        return shareStatus;
    }
}

