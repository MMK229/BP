#if UNITY_ANDROID
using UnityEngine;

public class AndroidZoomVideoSDKUserHelper
{
    private AndroidJavaObject helper;

    public AndroidZoomVideoSDKUserHelper(AndroidJavaObject helper)
    {
        this.helper = helper;
    }

    public bool ChangeName(string name, ZMVideoSDKUser zmVideoSDKUser)
    {
        return helper.Call<bool>("changeName", name, zmVideoSDKUser.VideoSDKUser.GetUser());
    }

    public bool MakeHost(ZMVideoSDKUser zmVideoSDKUser)
    {
        return helper.Call<bool>("makeHost", zmVideoSDKUser.VideoSDKUser.GetUser());
    }

    public bool MakeManager(ZMVideoSDKUser zmVideoSDKUser)
    {
        return helper.Call<bool>("makeManager", zmVideoSDKUser.VideoSDKUser.GetUser());
    }

    public bool RevokeManager(ZMVideoSDKUser zmVideoSDKUser)
    {
        return helper.Call<bool>("revokeManager", zmVideoSDKUser.VideoSDKUser.GetUser());
    }

    public bool RemoveUser(ZMVideoSDKUser zmVideoSDKUser)
    {
        return helper.Call<bool>("removeUser", zmVideoSDKUser.VideoSDKUser.GetUser());
    }

}
#endif
