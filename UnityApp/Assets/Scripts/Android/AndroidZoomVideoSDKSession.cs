#if UNITY_ANDROID
using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKSession
{

    public AndroidZoomVideoSDKSession()
    {
    }

    public ulong GetSessionNumber()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            return (ulong)session.Call<long>("getSessionNumber");
        }
    }

    public string GetSessionName()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            return session.Call<string>("getSessionHostName");
        }
    }

    public string GetSessionPassword()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            return session.Call<string>("getSessionPassword");
        }
    }

    public string GetSessionPhonePasscode()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            return session.Call<string>("getSessionPhonePasscode");
        }
    }

    public string GetSessionID()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            return session.Call<string>("getSessionID");
        }
    }

    public string GetSessionHostName()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            return session.Call<string>("getSessionHostName");
        }
    }

    public ZMVideoSDKUser GetSessionHost()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            AndroidJavaObject host = session.Call<AndroidJavaObject>("getSessionHost");
            AndroidZoomVideoSDKUser androidHost = new AndroidZoomVideoSDKUser(host);
            ZMVideoSDKUser zmHost = new ZMVideoSDKUser(androidHost);
            return zmHost;
        }
    }

    public List<ZMVideoSDKUser> GetRemoteUsers()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            using (AndroidJavaObject remoteUserList = session.Call<AndroidJavaObject>("getRemoteUsers"))
            {
                List<ZMVideoSDKUser> users = new List<ZMVideoSDKUser>();
                int listSize = remoteUserList.Call<int>("size");
                for (int i = 0; i < listSize; i++)
                {
                    AndroidJavaObject userObject = remoteUserList.Call<AndroidJavaObject>("get", i);
                    AndroidZoomVideoSDKUser androidUserObject = new AndroidZoomVideoSDKUser(userObject);
                    ZMVideoSDKUser user = new ZMVideoSDKUser(androidUserObject);
                    users.Add(user);
                }
                return users;
            }
        }
    }

    public ZMVideoSDKUser GetMySelf()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            AndroidJavaObject myself = session.Call<AndroidJavaObject>("getMySelf");
            AndroidZoomVideoSDKUser myselfObject = new AndroidZoomVideoSDKUser(myself);
            ZMVideoSDKUser zmMyslef = new ZMVideoSDKUser(myselfObject);
            return zmMyslef;
        }
    }

    public ZMVideoSDKSessionAudioStatisticInfo GetSessionAudioStatisticInfo()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            AndroidJavaObject audioStatistic = session.Call<AndroidJavaObject>("getSessionAudioStatisticInfo");
            ZMVideoSDKSessionAudioStatisticInfo audioStatisticInfo = new ZMVideoSDKSessionAudioStatisticInfo();
            audioStatisticInfo.sendFrequency = audioStatistic.Get<int>("sendFrequency");
            audioStatisticInfo.sendLatency = audioStatistic.Get<int>("sendLatency");
            audioStatisticInfo.sendJitter = audioStatistic.Get<int>("sendJitter");
            audioStatisticInfo.recvFrequency = audioStatistic.Get<int>("recvFrequency");
            audioStatisticInfo.recvLatency = audioStatistic.Get<int>("recvLatency");
            audioStatisticInfo.recvJitter = audioStatistic.Get<int>("recvJitter");
            audioStatisticInfo.recvPacketLossAvg = audioStatistic.Get<float>("recvPacketLossAvg");
            audioStatisticInfo.recvPacketLossMax = audioStatistic.Get<float>("recvPacketLossMax");
            audioStatisticInfo.sendPacketLossAvg = audioStatistic.Get<float>("sendPacketLossAvg");
            audioStatisticInfo.sendPacketLossMax = audioStatistic.Get<float>("sendPacketLossMax");
            return audioStatisticInfo;
        }
    }

    public ZMVideoSDKSessionASVStatisticInfo GetSessionVideoStatisticInfo()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            AndroidJavaObject videotatistic = session.Call<AndroidJavaObject>("getSessionVideoStatisticInfo");
            ZMVideoSDKSessionASVStatisticInfo videoStatisticInfo = new ZMVideoSDKSessionASVStatisticInfo();
            videoStatisticInfo.sendFrameWidth = videotatistic.Get<int>("sendFrameWidth");
            videoStatisticInfo.sendFrameHeight = videotatistic.Get<int>("sendFrameHeight");
            videoStatisticInfo.sendFps = videotatistic.Get<int>("sendFps");
            videoStatisticInfo.sendLatency = videotatistic.Get<int>("sendLatency");
            videoStatisticInfo.sendJitter = videotatistic.Get<int>("sendJitter");
            videoStatisticInfo.recvFrameWidth = videotatistic.Get<int>("recvFrameWidth");
            videoStatisticInfo.recvFrameHeight = videotatistic.Get<int>("recvFrameHeight");
            videoStatisticInfo.recvFps = videotatistic.Get<int>("recvFps");
            videoStatisticInfo.recvLatency = videotatistic.Get<int>("recvLatency");
            videoStatisticInfo.recvJitter = videotatistic.Get<int>("recvJitter");
            videoStatisticInfo.sendPacketLossAvg = videotatistic.Get<float>("sendPacketLossAvg");
            videoStatisticInfo.sendPacketLossMax = videotatistic.Get<float>("sendPacketLossMax");
            videoStatisticInfo.recvPacketLossAvg = videotatistic.Get<float>("recvPacketLossAvg");
            videoStatisticInfo.recvPacketLossMax = videotatistic.Get<float>("recvPacketLossMax");
            return videoStatisticInfo;
        }
    }

    public ZMVideoSDKSessionASVStatisticInfo GetSessionShareStatisticInfo()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            AndroidJavaObject shareStatistic = session.Call<AndroidJavaObject>("getSessionShareStatisticInfo");
            ZMVideoSDKSessionASVStatisticInfo shareStatisticInfo = new ZMVideoSDKSessionASVStatisticInfo();
            shareStatisticInfo.sendFrameWidth = shareStatistic.Get<int>("sendFrameWidth");
            shareStatisticInfo.sendFrameHeight = shareStatistic.Get<int>("sendFrameHeight");
            shareStatisticInfo.sendFps = shareStatistic.Get<int>("sendFps");
            shareStatisticInfo.sendLatency = shareStatistic.Get<int>("sendLatency");
            shareStatisticInfo.sendJitter = shareStatistic.Get<int>("sendJitter");
            shareStatisticInfo.recvFrameWidth = shareStatistic.Get<int>("recvFrameWidth");
            shareStatisticInfo.recvFrameHeight = shareStatistic.Get<int>("recvFrameHeight");
            shareStatisticInfo.recvFps = shareStatistic.Get<int>("recvFps");
            shareStatisticInfo.recvLatency = shareStatistic.Get<int>("recvLatency");
            shareStatisticInfo.recvJitter = shareStatistic.Get<int>("recvJitter");
            shareStatisticInfo.sendPacketLossAvg = shareStatistic.Get<float>("sendPacketLossAvg");
            shareStatisticInfo.sendPacketLossMax = shareStatistic.Get<float>("sendPacketLossMax");
            shareStatisticInfo.recvPacketLossAvg = shareStatistic.Get<float>("recvPacketLossAvg");
            shareStatisticInfo.recvPacketLossMax = shareStatistic.Get<float>("recvPacketLossMax");
            return shareStatisticInfo;
        }
    }
}
#endif
