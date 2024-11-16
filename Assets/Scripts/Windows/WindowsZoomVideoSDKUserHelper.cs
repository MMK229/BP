#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

public class WindowsZoomVideoSDKUserHelper
{
    private IntPtr helper;

    public WindowsZoomVideoSDKUserHelper(IntPtr helper)
    {
        this.helper = helper;
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern bool changeName_c(IntPtr helper, string name, IntPtr user);
    public bool ChangeName(string name, ZMVideoSDKUser zmVideoSDKUser)
    {
        return changeName_c(helper, name, zmVideoSDKUser.VideoSDKUser.Instance());
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool makeHost_c(IntPtr helper, IntPtr user);
    public bool MakeHost(ZMVideoSDKUser zmVideoSDKUser)
    {
        return makeHost_c(helper, zmVideoSDKUser.VideoSDKUser.Instance());
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool makeManager_c(IntPtr helper, IntPtr user);
    public bool MakeManager(ZMVideoSDKUser zmVideoSDKUser)
    {
        return makeManager_c(helper, zmVideoSDKUser.VideoSDKUser.Instance());
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool revokeManager_c(IntPtr helper, IntPtr user);
    public bool RevokeManager(ZMVideoSDKUser zmVideoSDKUser)
    {
        return revokeManager_c(helper, zmVideoSDKUser.VideoSDKUser.Instance());
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool removeUser_c(IntPtr helper, IntPtr user);
    public bool RemoveUser(ZMVideoSDKUser zmVideoSDKUser)
    {
        return removeUser_c(helper, zmVideoSDKUser.VideoSDKUser.Instance());
    }

}
#endif