#if UNITY_STANDALONE_WIN
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class WindowsZoomVideoSDKSession
{
    private IntPtr sessionObj;
    public WindowsZoomVideoSDKSession(IntPtr sessionObj)
    {
        this.sessionObj = sessionObj;
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ulong getSessionNumber_c(IntPtr sessionObj); 
    public ulong GetSessionNumber()
    {
        return getSessionNumber_c(sessionObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern string getSessionName_c(IntPtr sessionObj);
    public string GetSessionName()
    {
        return getSessionName_c(sessionObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern string getSessionPassword_c(IntPtr sessionObj);
    public string GetSessionPassword()
    {
        return getSessionPassword_c(sessionObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern string getSessionPhonePasscode_c(IntPtr sessionObj);
    public string GetSessionPhonePasscode()
    {
        return getSessionPhonePasscode_c(sessionObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern string getSessionID_c(IntPtr sessionObj);
    public string GetSessionID()
    {
        return getSessionID_c(sessionObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern string getSessionHostName_c(IntPtr sessionObj);
    public string GetSessionHostName()
    {
        return getSessionHostName_c(sessionObj);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern IntPtr getSessionHost_c(IntPtr sessionObj);
    public ZMVideoSDKUser GetSessionHost()
    {
        IntPtr user = getSessionHost_c(sessionObj);
        WindowsZoomVideoSDKUser winUser = new WindowsZoomVideoSDKUser(user);
        return new ZMVideoSDKUser(winUser);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern IntPtr getRemoteUsers_c(IntPtr sessionObj, IntPtr elementCount);
    public List<ZMVideoSDKUser> GetRemoteUsers()
    {
        IntPtr pElementCount = Marshal.AllocHGlobal(sizeof(Int32));
        IntPtr pUsers = getRemoteUsers_c(sessionObj, pElementCount);
        int elementCount = Marshal.ReadInt32(pElementCount);
        return WindowsZoomVideoSDKUser.GetUserList(pUsers, elementCount);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern IntPtr getMyself_c(IntPtr sessionObj);
    public ZMVideoSDKUser GetMySelf()
    {
        IntPtr myselfObj = getMyself_c(sessionObj);
        WindowsZoomVideoSDKUser winUser = new WindowsZoomVideoSDKUser(myselfObj);
        return new ZMVideoSDKUser(winUser);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors getSessionAudioStatisticInfo_c(IntPtr sessionObj, ZMVideoSDKSessionAudioStatisticInfo send, ZMVideoSDKSessionAudioStatisticInfo recv);
    public ZMVideoSDKSessionAudioStatisticInfo GetSessionAudioStatisticInfo()
    {
        ZMVideoSDKSessionAudioStatisticInfo send = new ZMVideoSDKSessionAudioStatisticInfo();
        ZMVideoSDKSessionAudioStatisticInfo recv = new ZMVideoSDKSessionAudioStatisticInfo();
        getSessionAudioStatisticInfo_c(sessionObj, send, recv);
        return send;
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors getSessionVideoStatisticInfo_c(IntPtr sessionObj, ZMVideoSDKSessionASVStatisticInfo send, ZMVideoSDKSessionASVStatisticInfo recv);
    public ZMVideoSDKSessionASVStatisticInfo GetSessionVideoStatisticInfo()
    {
        ZMVideoSDKSessionASVStatisticInfo send = new ZMVideoSDKSessionASVStatisticInfo();
        ZMVideoSDKSessionASVStatisticInfo recv = new ZMVideoSDKSessionASVStatisticInfo();
        getSessionVideoStatisticInfo_c(sessionObj, send, recv) ;
        return send;
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors getSessionShareStatisticInfo_c(IntPtr sessionObj, ZMVideoSDKSessionASVStatisticInfo send, ZMVideoSDKSessionASVStatisticInfo recv);
    public ZMVideoSDKSessionASVStatisticInfo GetSessionShareStatisticInfo()
    {
        ZMVideoSDKSessionASVStatisticInfo send = new ZMVideoSDKSessionASVStatisticInfo();
        ZMVideoSDKSessionASVStatisticInfo recv = new ZMVideoSDKSessionASVStatisticInfo();
        getSessionShareStatisticInfo_c(sessionObj, send, recv);
        return send;
    }
}
#endif