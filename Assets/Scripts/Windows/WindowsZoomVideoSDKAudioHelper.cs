#if UNITY_STANDALONE_WIN
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class WindowsZoomVideoSDKAudioHelper
{
    private IntPtr audioHelper;

    public WindowsZoomVideoSDKAudioHelper(IntPtr audioHelper)
    {
        this.audioHelper = audioHelper;
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors muteAudio_c(IntPtr audioHelper, IntPtr pUser);
    public void MuteAudio(ZMVideoSDKUser user, Action<ZMVideoSDKErrors> callback)
    {
        callback(muteAudio_c(audioHelper, user.VideoSDKUser.Instance()));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors unMuteAudio_c(IntPtr audioHelper, IntPtr pUser);
    public void UnMuteAudio(ZMVideoSDKUser user, Action<ZMVideoSDKErrors> callback)
    {
        callback(unMuteAudio_c(audioHelper, user.VideoSDKUser.Instance()));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors startAudio_c(IntPtr audioHelper);
    public void StartAudio(Action<ZMVideoSDKErrors> callback)
    {
        callback(startAudio_c(audioHelper));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors stopAudio_c(IntPtr audioHelper);
    public void StopAudio(Action<ZMVideoSDKErrors> callback)
    {
        callback(stopAudio_c(audioHelper));

    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors subscribeAudio_c(IntPtr audioHelper);
    public void Subscribe(Action<ZMVideoSDKErrors> callback)
    {
        callback(subscribeAudio_c(audioHelper));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors unSubscribeAudio_c(IntPtr audioHelper);
    public void UnSubscribe(Action<ZMVideoSDKErrors> callback)
    {
        callback(unSubscribeAudio_c(audioHelper));
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr getSpeakerList_c(IntPtr audioHelper, IntPtr pCount);
    public List<ZMVideoSDKSpeakerDevice> GetSpeakerList()
    {
        IntPtr pElementCount = Marshal.AllocHGlobal(sizeof(Int32));
        IntPtr pSpeakers = getSpeakerList_c(audioHelper, pElementCount);
        int elementCount = Marshal.ReadInt32(pElementCount);
        List<ZMVideoSDKSpeakerDevice> speakerList = new List<ZMVideoSDKSpeakerDevice>();
        int elementSize = Marshal.SizeOf(typeof(IntPtr));
        for (int i = 0; i < elementCount; i++)
        {
            IntPtr pSpeaker = Marshal.ReadIntPtr(pSpeakers, i * elementSize);
            ZMVideoSDKSpeakerDevice speaker;
            speaker = (ZMVideoSDKSpeakerDevice)Marshal.PtrToStructure(pSpeaker, typeof(ZMVideoSDKSpeakerDevice));
            speakerList.Add(speaker);
            Marshal.FreeHGlobal(pSpeaker);
        }
        Marshal.FreeHGlobal(pSpeakers);
        Marshal.FreeHGlobal(pElementCount);
        return speakerList;
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr getMicList_c(IntPtr audioHelper, IntPtr pCount); 
    public List<ZMVideoSDKMicDevice> GetMicList()
    {
        IntPtr pElementCount = Marshal.AllocHGlobal(sizeof(Int32));
        IntPtr pMics = getMicList_c(audioHelper, pElementCount);
        int elementCount = Marshal.ReadInt32(pElementCount);
        List<ZMVideoSDKMicDevice> micList = new List<ZMVideoSDKMicDevice>();
        int elementSize = Marshal.SizeOf(typeof(IntPtr));
        for (int i = 0; i < elementCount; i++)
        {
            IntPtr pSpeaker = Marshal.ReadIntPtr(pMics, i * elementSize);
            ZMVideoSDKMicDevice speaker;
            speaker = (ZMVideoSDKMicDevice)Marshal.PtrToStructure(pSpeaker, typeof(ZMVideoSDKMicDevice));
            micList.Add(speaker);
            Marshal.FreeHGlobal(pSpeaker);
        }
        Marshal.FreeHGlobal(pMics);
        Marshal.FreeHGlobal(pElementCount);
        return micList;
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors selectSpeaker_c(IntPtr audioHelper, string deviceId, string deviceName);
    public ZMVideoSDKErrors SelectSpeaker(string deviceID, string deviceName)
    {
        return selectSpeaker_c(audioHelper, deviceID, deviceName);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors selectMic_c(IntPtr audioHelper, string deviceId, string deviceName);
    public ZMVideoSDKErrors SelectMic(string deviceID, string deviceName)
    {
        return selectMic_c(audioHelper, deviceID, deviceName);
    }
}

#endif