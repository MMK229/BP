#if UNITY_STANDALONE_WIN
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class WindowsZoomVideoSDKUser
{
    private IntPtr userObj;

    public WindowsZoomVideoSDKUser(IntPtr user)
	{
        this.userObj = user;
	}

    public IntPtr Instance()
    {
        return this.userObj;
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern string getUserID_c(IntPtr userObj);
    public string GetUserID()
    {
        return getUserID_c(userObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern string getCustomIdentity_c(IntPtr userObj);
    public string GetCustomIdentity()
    {
        return getCustomIdentity_c(userObj);

    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern string getUserName_c(IntPtr userObj);
    public string GetUserName()
    {
        return getUserName_c(userObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool isHost_c(IntPtr user);
    public bool IsHost()
    {
        return isHost_c(userObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool isManager_c(IntPtr user);
    public bool IsManager()
    {
        return isManager_c(userObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool setUserVolume_c(IntPtr user, float volume, bool isSharingAudio);
    public bool SetUserVolume(float volume, bool isSharingAudio)
    {
        return setUserVolume_c(userObj, volume, isSharingAudio);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern float getUserVolume_c(IntPtr user, bool isSharingAudio);
    public float GetUserVolume(bool isSharingAudio)
    {
        return getUserVolume_c(userObj, isSharingAudio);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool canSetUserVolume_c(IntPtr user, bool isSharingAudio);
    public bool CanSetUserVolume(bool isSharingAudio)
    {
        return canSetUserVolume_c(userObj, isSharingAudio);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool hasIndividualRecordingConsent_c(IntPtr user);
    public bool HasIndividualRecordingConsent()
    {
        return hasIndividualRecordingConsent_c(userObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKAudioStatus getAudioStatus_c(IntPtr user);
    public ZMVideoSDKAudioStatus GetAudioStatus()
    {
        return getAudioStatus_c(userObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKVideoStatisticInfo getVideoStatisticInfo_c(IntPtr user);
    public ZMVideoSDKVideoStatisticInfo GetVideoStatisticInfo()
    {
        return getVideoStatisticInfo_c(userObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern IntPtr getVideoPipe_c(IntPtr user);
    public ZMVideoSDKRawDataPipe GetVideoPipe()
    {
        IntPtr pipe = getVideoPipe_c(userObj);
        if (pipe == IntPtr.Zero)
        {
            return null;
        }
        string userId = getUserID_c(userObj);
        WindowsZoomVideoSDKRawDataPipe winPipe = new WindowsZoomVideoSDKRawDataPipe(pipe, userId);
        return new ZMVideoSDKRawDataPipe(winPipe);
    }

    public static List<ZMVideoSDKUser> GetUserList(IntPtr pUsers, int elementCount)
    {
        List<ZMVideoSDKUser> userList = new List<ZMVideoSDKUser>();
        int elementSize = Marshal.SizeOf(typeof(IntPtr));
        for (int i = 0; i < elementCount; i++)
        {
            IntPtr objC_ZMVideoSDKUser = Marshal.ReadIntPtr(pUsers, i * elementSize);
            WindowsZoomVideoSDKUser user = new WindowsZoomVideoSDKUser(objC_ZMVideoSDKUser);
            userList.Add(new ZMVideoSDKUser(user));
        }
        Marshal.FreeHGlobal(pUsers);
        return userList;
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern IntPtr getSharePipe_c(IntPtr user);
    public ZMVideoSDKRawDataPipe GetSharePipe()
    {
        IntPtr pPipe = getSharePipe_c(userObj);
        if (pPipe == IntPtr.Zero)
        {
            return null;
        }
        WindowsZoomVideoSDKRawDataPipe winPipe = new WindowsZoomVideoSDKRawDataPipe(pPipe, "");
        return new ZMVideoSDKRawDataPipe(winPipe);
    }
}
#endif