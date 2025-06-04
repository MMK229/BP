using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKNetworkStatus
{
	public AndroidZoomVideoSDKNetworkStatus()
	{
	}

    private static AndroidJavaClass networkStatusClass = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_NETWORKS_STATUS_PATH);

    private static Dictionary<string, ZMVideoSDKNetworkStatus> networkStatusDic
        = new Dictionary<string, ZMVideoSDKNetworkStatus>()
        {
            {networkStatusClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKNetwork_None").Call<string>("name"), ZMVideoSDKNetworkStatus.ZMVideoSDKNetworkStatus_None},
            {networkStatusClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKNetwork_Bad").Call<string>("name"), ZMVideoSDKNetworkStatus.ZMVideoSDKNetworkStatus_Bad},
            {networkStatusClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKNetwork_Normal").Call<string>("name"), ZMVideoSDKNetworkStatus.ZMVideoSDKNetworkStatus_Normal},
            {networkStatusClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKNetwork_Good").Call<string>("name"), ZMVideoSDKNetworkStatus.ZMVideoSDKNetworkStatus_Good},
        };

    public static ZMVideoSDKNetworkStatus GetEnum(string status)
    {
        ZMVideoSDKNetworkStatus networkStatus;
        try
        {
            networkStatus = networkStatusDic[status];
        }
        catch (KeyNotFoundException)
        {
            networkStatus = ZMVideoSDKNetworkStatus.ZMVideoSDKNetworkStatus_None;
        }
        return networkStatus;
    }
}

