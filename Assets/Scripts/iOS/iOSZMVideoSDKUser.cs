#if UNITY_IOS
using System;
using System.Runtime.InteropServices;

public class IOSZMVideoSDKUser
{
    public IOSZMVideoSDKUser(IntPtr _objC_ZMVideoSDKUser)
    {
        objC_ZMVideoSDKUser = _objC_ZMVideoSDKUser;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ZMVideoSDKAudioStatus_iOS
    {
        public int audioType;

        [MarshalAs(UnmanagedType.I1)]
        public bool isMuted;

        [MarshalAs(UnmanagedType.I1)]
        public bool isTalking;
    }

    [DllImport ("__Internal")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKUser);
    ~IOSZMVideoSDKUser()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKUser);
    }

    private IntPtr objC_ZMVideoSDKUser;
    public IntPtr ObjC_ZMVideoSDKUser
    {
        get { return objC_ZMVideoSDKUser; }
    }

    private string userID = null;

    [DllImport ("__Internal")]
    private static extern string getUserID(IntPtr zmVideoSDKUser);
    public string GetUserID()
    {
        if (userID == null)
        {
            userID = getUserID(objC_ZMVideoSDKUser);
        }
        return userID;
    }

    [DllImport ("__Internal")]
    private static extern string getCustomIdentity(IntPtr zmVideoSDKUser);
    public string GetCustomIdentity()
    {
        return getCustomIdentity(objC_ZMVideoSDKUser);
    }

    [DllImport ("__Internal")]
    private static extern string getUserName(IntPtr zmVideoSDKUser);
    public string GetUserName()
    {
        return getUserName(objC_ZMVideoSDKUser);
    }

    [DllImport ("__Internal")]
    private static extern IntPtr getAudioStatus(IntPtr zmVideoSDKUser);
    public ZMVideoSDKAudioStatus GetAudioStatus()
    {
        IntPtr pAudioStatus = getAudioStatus(objC_ZMVideoSDKUser);
        ZMVideoSDKAudioStatus_iOS audioStatus;
        audioStatus = (ZMVideoSDKAudioStatus_iOS)Marshal.PtrToStructure(pAudioStatus, typeof(ZMVideoSDKAudioStatus_iOS));
        Marshal.FreeHGlobal(pAudioStatus);

        ZMVideoSDKAudioStatus status = new ZMVideoSDKAudioStatus();
        status.audioType = (ZMVideoSDKAudioType)audioStatus.audioType;
        status.isMuted = audioStatus.isMuted;
        status.isTalking = audioStatus.isTalking;
        return status;
    }

    [DllImport ("__Internal")]
    private static extern bool isHost(IntPtr zmVideoSDKUser);
    public bool IsHost()
    {
        return isHost(objC_ZMVideoSDKUser);
    }

    [DllImport ("__Internal")]
    private static extern bool isManager(IntPtr zmVideoSDKUser);
    public bool IsManager()
    {
        return isManager(objC_ZMVideoSDKUser);
    }

    [DllImport ("__Internal")]
    private static extern IntPtr getVideoStatisticInfo(IntPtr zmVideoSDKUser);
    public ZMVideoSDKVideoStatisticInfo GetVideoStatisticInfo()
    {
        IntPtr pVideoStatisticInfo = getVideoStatisticInfo(objC_ZMVideoSDKUser);
        ZMVideoSDKVideoStatisticInfo videoStatisticInfo;
        videoStatisticInfo = (ZMVideoSDKVideoStatisticInfo)Marshal.PtrToStructure(pVideoStatisticInfo, typeof(ZMVideoSDKVideoStatisticInfo));
        return videoStatisticInfo;
    }

    [DllImport ("__Internal")]
    private static extern IntPtr getVideoPipe(IntPtr zmVideoSDKUser);
    public ZMVideoSDKRawDataPipe GetVideoPipe()
    {
        IntPtr objC_ZMVideoSDKRawDataPipe = getVideoPipe(objC_ZMVideoSDKUser);
        if (objC_ZMVideoSDKRawDataPipe != IntPtr.Zero)
        {
            return new ZMVideoSDKRawDataPipe(new IOSZMVideoSDKRawDataPipe(objC_ZMVideoSDKRawDataPipe, GetUserID()));
        }
        return null;
    }

    [DllImport ("__Internal")]
    private static extern bool setUserVolume(IntPtr zmVideoSDKUser, float volume, bool isSharingAudio);
    public bool SetUserVolume(float volume, bool isSharingAudio)
    {
        return setUserVolume(objC_ZMVideoSDKUser, volume, isSharingAudio);
    }

    [DllImport ("__Internal")]
    private static extern float getUserVolume(IntPtr zmVideoSDKUser, bool isSharingAudio);
    public float GetUserVolume(bool isSharingAudio)
    {
        return getUserVolume(objC_ZMVideoSDKUser, isSharingAudio);
    }

    [DllImport ("__Internal")]
    private static extern bool canSetUserVolume(IntPtr zmVideoSDKUser, bool isShareAudio);
    public bool CanSetUserVolume(bool isShareAudio)
    {
        return canSetUserVolume(objC_ZMVideoSDKUser, isShareAudio);
    }

    [DllImport ("__Internal")]
    private static extern bool hasIndividualRecordingConsent(IntPtr zmVideoSDKUser);
    public bool HasIndividualRecordingConsent()
    {
        return hasIndividualRecordingConsent(objC_ZMVideoSDKUser);
    }

    [DllImport ("__Internal")]
    private static extern IntPtr getSharePipe(IntPtr zmVideoSDKUser);
    public ZMVideoSDKRawDataPipe GetSharePipe()
    {
        IntPtr objC_ZMVideoSDKRawDataPipe = getSharePipe(objC_ZMVideoSDKUser);
        if (objC_ZMVideoSDKRawDataPipe != IntPtr.Zero)
        {
            return new ZMVideoSDKRawDataPipe(new IOSZMVideoSDKRawDataPipe(objC_ZMVideoSDKRawDataPipe, GetUserID()));
        }
        return null;
    }
}
#endif
