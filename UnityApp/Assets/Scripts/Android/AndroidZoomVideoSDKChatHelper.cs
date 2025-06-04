#if UNITY_ANDROID
using System;
using UnityEngine;

public class AndroidZoomVideoSDKChatHelper
{
    private AndroidJavaObject _chatHelper;

    public AndroidZoomVideoSDKChatHelper(AndroidJavaObject chatHelper)
	{
        _chatHelper = chatHelper;
    }

    private AndroidJavaObject GetChatHelper()
    {
        if (_chatHelper != null)
        {
            return _chatHelper;
        }

        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        using (AndroidJavaObject zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance"))
        {
            _chatHelper = zoomSDK.Call<AndroidJavaObject>("getChatHelper");
        }
        if (_chatHelper == null)
        {
            throw new MissingMethodException("ZoomVideoSdkChatHelper:: No chat helper found.");
        }

        return _chatHelper;
    }

    public bool IsChatDisabled()
    {
        return GetChatHelper().Call<bool>("isChatDisabled");
    }

    public bool IsPrivateChatDisabled()
    {
        return GetChatHelper().Call<bool>("isPrivateChatDisabled");
    }

    public void SendChatToUser(ZMVideoSDKUser user, string msgContent, Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                callback(AndroidZoomVideoSDKErrors.GetEnum(GetChatHelper().Call<int>("sendChatToUser", user.VideoSDKUser, msgContent)));
            }
            ));
        }
    }

    public void SendChatToAll(string msgContent, Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                callback(AndroidZoomVideoSDKErrors.GetEnum(GetChatHelper().Call<int>("sendChatToAll", msgContent)));
            }
            ));
        }
    }

    public void DeleteChatMessage(string msgId, Action<ZMVideoSDKErrors> callback)
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                callback(AndroidZoomVideoSDKErrors.GetEnum(GetChatHelper().Call<int>("deleteChatMessage", msgId)));
            }
            ));
        }
    }

    public bool CanChatMessageBeDeleted(string msgId)
    {
        return GetChatHelper().Call<bool>("canChatMessageBeDeleted", msgId);
    }

    public ZMVideoSDKErrors ChangeChatPrivilege(ZMVideoSDKChatPrivilegeType privilege)
    {
        return AndroidZoomVideoSDKErrors.GetEnum(GetChatHelper().Call<int>("changeChatPrivilege", AndroidZoomVideoSDKChatPrivilegeType.GetJavaObject(privilege)));
    }

    public ZMVideoSDKChatPrivilegeType GetChatPrivilege()
    {
        return AndroidZoomVideoSDKChatPrivilegeType.GetEnum(GetChatHelper().Call<AndroidJavaObject>("getChatPrivilege").Call<string>("name"));
    }
}

#endif
