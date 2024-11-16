#if UNITY_ANDROID
using System;
using UnityEngine;

public class AndroidZoomVideoSDKUser
{
    private AndroidJavaObject user;

    public AndroidZoomVideoSDKUser(AndroidJavaObject user)
	{
        this.user = user;
	}

    public static AndroidJavaObject GetUserInSession(String userId)
    {
        using(AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using(AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using(AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            AndroidJavaObject mySelf = session.Call<AndroidJavaObject>("getMySelf");
            if (String.Equals(mySelf.Call<string>("getUserID"), userId))
            {
                return mySelf;
            }
            using(AndroidJavaObject userList = session.Call<AndroidJavaObject>("getRemoteUsers"))
            {
                int listSize = userList.Call<int>("size");
                for (int i = 0; i < listSize; i++)
                {
                    AndroidJavaObject user = userList.Call<AndroidJavaObject>("get", i);
                    if (String.Equals(user.Call<string>("getUserID"), userId))
                    {
                        return user;
                    }
                }
            }
        }        
        return null;
    }

    public AndroidJavaObject GetUser()
    {
        return this.user;
    }

    public static AndroidJavaObject GetMyself()
    {
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            AndroidJavaObject mySelf = session.Call<AndroidJavaObject>("getMySelf");
            return mySelf;
        }
    }

    public string GetUserID()
    {
        if (user != null)
        {
            return user.Call<string>("getUserID");
        }
        return "";
    }

    public string GetCustomIdentity()
    {
        if (user != null)
        {
            return user.Call<string>("getCustomIdentity");
        }
        return "";
    }

    public string GetUserName()
    {
        if (user != null)
        {
            return user.Call<string>("getUserName");
        }
        return "";
    }

    public bool IsHost()
    {
        if (user != null)
        {
            return user.Call<bool>("isHost");
        }
        return false;
    }

    public bool IsManager()
    {
        if (user != null)
        {
            return user.Call<bool>("isManager");
        }
        return false;
    }

    public bool SetUserVolume(float volume, bool isSharingAudio)
    {
        if (user != null)
        {
            return user.Call<bool>("setUserVolume", volume, isSharingAudio);
        }
        return false;
    }

    public float GetUserVolume(bool isSharingAudio)
    {
        if (user != null)
        {
            return user.Call<float>("getUserVolume", isSharingAudio);
        }
        return -1.0f;
    }

    public bool CanSetUserVolume(bool isSharingAudio)
    {
        if (user != null)
        {
            return user.Call<bool>("canSetUserVolume", isSharingAudio);
        }
        return false;
    }

    public bool HasIndividualRecordingConsent()
    {
        if (user != null)
        {
            return user.Call<bool>("hasIndividualRecordingConsent");
        }
        return false;
    }

    public ZMVideoSDKAudioStatus GetAudioStatus()
    {
        ZMVideoSDKAudioStatus zmAudioStatus = new ZMVideoSDKAudioStatus();
        if (user != null)
        {
            AndroidJavaObject audioStatusObject = user.Call<AndroidJavaObject>("getAudioStatus");
            zmAudioStatus.isMuted = audioStatusObject.Call<bool>("isMuted");
            zmAudioStatus.isTalking = audioStatusObject.Call<bool>("isTalking");
            using (AndroidJavaObject audioType = audioStatusObject.Call<AndroidJavaObject>("getAudioType"))
            {
                if (audioType != null)
                {
                    zmAudioStatus.audioType = AndroidZoomVideoSDKAudioType.GetEnum(audioType.Call<string>("name"));
                }
            }
        }
        return zmAudioStatus;
    }

    public ZMVideoSDKVideoStatisticInfo GetVideoStatisticInfo()
    {
        ZMVideoSDKVideoStatisticInfo videoStatus = new ZMVideoSDKVideoStatisticInfo();
        if (user != null)
        {
            AndroidJavaObject videoStatusObject = user.Call<AndroidJavaObject>("getVideoStatisticInfo");
            videoStatus.width = videoStatusObject.Call<int>("getWidth");
            videoStatus.height = videoStatusObject.Call<int>("getHeight");
            videoStatus.fps = videoStatusObject.Call<int>("getFps");
            videoStatus.bpf = videoStatusObject.Call<int>("getBpf");
            AndroidJavaObject networkStatusObject = videoStatusObject.Call<AndroidJavaObject>("getVideoNetworkStatus");
            if (networkStatusObject != null)
            {
                videoStatus.videoNetworkStatus = AndroidZoomVideoSDKNetworkStatus.GetEnum(networkStatusObject.Call<string>("name"));
            }
        }
        return videoStatus;
    }

    public ZMVideoSDKRawDataPipe GetVideoPipe()
    {
        if (user != null)
        {
            AndroidJavaObject videoPipeObject = user.Call<AndroidJavaObject>("getVideoPipe");
            AndroidZoomVideoSDKRawDataPipe androidVideoPipe = new AndroidZoomVideoSDKRawDataPipe(videoPipeObject);
            ZMVideoSDKRawDataPipe zmRawDataPipe = new ZMVideoSDKRawDataPipe(androidVideoPipe);
            return zmRawDataPipe;
        }
        return null;
    }

    public ZMVideoSDKRawDataPipe GetSharePipe()
    {
        if (user != null)
        {
            AndroidJavaObject sharePipeObject = user.Call<AndroidJavaObject>("getSharePipe");
            AndroidZoomVideoSDKRawDataPipe androidSharePipe = new AndroidZoomVideoSDKRawDataPipe(sharePipeObject);
            ZMVideoSDKRawDataPipe zmRawDataPipe = new ZMVideoSDKRawDataPipe(androidSharePipe);
            return zmRawDataPipe;
        }
        return null;
    }
}

#endif
