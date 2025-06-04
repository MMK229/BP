#if UNITY_STANDALONE_OSX
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MacZMVideoSDKSession
{
    private IntPtr objC_ZMVideoSDKSession;

    public MacZMVideoSDKSession(IntPtr _objC_ZMVideoSDKSession)
    {
        objC_ZMVideoSDKSession = _objC_ZMVideoSDKSession;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKSession);
    ~MacZMVideoSDKSession()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKSession);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern ulong getSessionNumber(IntPtr zmVideoSDKSession);
    public ulong GetSessionNumber()
    {
        return getSessionNumber(objC_ZMVideoSDKSession);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern string getSessionName(IntPtr zmVideoSDKSession);
    public string GetSessionName()
    {
        return getSessionName(objC_ZMVideoSDKSession);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern string getSessionPassword(IntPtr zmVideoSDKSession);
    public string GetSessionPassword()
    {
        return getSessionPassword(objC_ZMVideoSDKSession);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern string getSessionPhonePasscode(IntPtr zmVideoSDKSession);
    public string GetSessionPhonePasscode()
    {
        return getSessionPhonePasscode(objC_ZMVideoSDKSession);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern string getSessionID(IntPtr zmVideoSDKSession);
    public string GetSessionID()
    {
        return getSessionID(objC_ZMVideoSDKSession);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern string getSessionHostName(IntPtr zmVideoSDKSession);
    public string GetSessionHostName()
    {
        return getSessionHostName(objC_ZMVideoSDKSession);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern IntPtr  getSessionHost(IntPtr zmVideoSDKSession);
    public ZMVideoSDKUser GetSessionHost()
    {
        IntPtr objC_ZMVideoSDKUser = getSessionHost(objC_ZMVideoSDKSession);
        return new ZMVideoSDKUser(new MacZMVideoSDKUser(objC_ZMVideoSDKUser));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern IntPtr getRemoteUsers(IntPtr zmVideoSDKSession, IntPtr elementCount);
    public List<ZMVideoSDKUser> GetRemoteUsers()
    {
        List<ZMVideoSDKUser> userList = new List<ZMVideoSDKUser>();
        IntPtr pElementCount = Marshal.AllocHGlobal(sizeof(Int32));
        IntPtr pUsers = getRemoteUsers(objC_ZMVideoSDKSession, pElementCount);
        int elementCount = Marshal.ReadInt32(pElementCount);
        int elementSize = Marshal.SizeOf(typeof(IntPtr));
        for (int i = 0; i < elementCount; i++)
        {
            IntPtr objC_ZMVideoSDKUser = Marshal.ReadIntPtr(pUsers, i * elementSize);
            MacZMVideoSDKUser user = new MacZMVideoSDKUser(objC_ZMVideoSDKUser);
            userList.Add(new ZMVideoSDKUser(user));
        }
        Marshal.FreeHGlobal(pElementCount);
        Marshal.FreeHGlobal(pUsers);
        return userList;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern IntPtr getMySelf(IntPtr zmVideoSDKSession);
    public ZMVideoSDKUser GetMySelf()
    {
        IntPtr objC_ZMVideoSDKUser = getMySelf(objC_ZMVideoSDKSession);
        return new ZMVideoSDKUser(new MacZMVideoSDKUser(objC_ZMVideoSDKUser));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern IntPtr getSessionAudioStatisticInfo(IntPtr zmVideoSDKSession);
    public ZMVideoSDKSessionAudioStatisticInfo GetSessionAudioStatisticInfo()
    {
        IntPtr pInfo = getSessionAudioStatisticInfo(objC_ZMVideoSDKSession);
        ZMVideoSDKSessionAudioStatisticInfo info;
        info = (ZMVideoSDKSessionAudioStatisticInfo)Marshal.PtrToStructure(pInfo, typeof(ZMVideoSDKSessionAudioStatisticInfo));
        return info;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern IntPtr getSessionVideoStatisticInfo(IntPtr zmVideoSDKSession);
    public ZMVideoSDKSessionASVStatisticInfo GetSessionVideoStatisticInfo()
    {
        IntPtr pInfo = getSessionVideoStatisticInfo(objC_ZMVideoSDKSession);
        ZMVideoSDKSessionASVStatisticInfo info;
        info = (ZMVideoSDKSessionASVStatisticInfo)Marshal.PtrToStructure(pInfo, typeof(ZMVideoSDKSessionASVStatisticInfo));
        return info;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern IntPtr getSessionShareStatisticInfo(IntPtr zmVideoSDKSession);
    public ZMVideoSDKSessionASVStatisticInfo GetSessionShareStatisticInfo()
    {
        IntPtr pInfo = getSessionShareStatisticInfo(objC_ZMVideoSDKSession);
        ZMVideoSDKSessionASVStatisticInfo info;
        info = (ZMVideoSDKSessionASVStatisticInfo)Marshal.PtrToStructure(pInfo, typeof(ZMVideoSDKSessionASVStatisticInfo));
        return info;
    }
}
#endif
