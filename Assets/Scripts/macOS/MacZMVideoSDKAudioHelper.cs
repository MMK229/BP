#if UNITY_STANDALONE_OSX
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MacZMVideoSDKAudioRawData
{
    public string buffer;
    public uint bufferLen;
    public uint sampleRate;
    public uint channelNum;

    private IntPtr objC_ZMVideoSDKAudioRawData;

    public MacZMVideoSDKAudioRawData()
    {  
    }

    public MacZMVideoSDKAudioRawData(string _buffer, uint _bufferLen, uint _sampleRate, uint _channelNum, IntPtr _objC_ZMVideoSDKAudioRawData)
    {  
        buffer = _buffer;
        bufferLen = _bufferLen;
        sampleRate = _sampleRate;
        channelNum = _channelNum;
        objC_ZMVideoSDKAudioRawData = _objC_ZMVideoSDKAudioRawData;
    }

    public MacZMVideoSDKAudioRawData(IntPtr _objC_ZMVideoSDKAudioRawData)
    {
        objC_ZMVideoSDKAudioRawData = _objC_ZMVideoSDKAudioRawData;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool canAddRef(IntPtr zmVideoSDKAudioRawData);
    public bool canAddRef()
    {
        return canAddRef(objC_ZMVideoSDKAudioRawData);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool addRef(IntPtr zmVideoSDKAudioRawData);
    public bool addRef()
    {
        return addRef(objC_ZMVideoSDKAudioRawData);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern int releaseRef(IntPtr zmVideoSDKAudioRawData);
    public int releaseRef()
    {
        return releaseRef(objC_ZMVideoSDKAudioRawData);
    }
}

public class MacZMVideoSDKAudioHelper
{
    private IntPtr objC_ZMVideoSDKAudioHelper;

    public MacZMVideoSDKAudioHelper(IntPtr _objC_ZMVideoSDKAudioHelper)
    {
        objC_ZMVideoSDKAudioHelper = _objC_ZMVideoSDKAudioHelper;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKAudioHelper);
    ~MacZMVideoSDKAudioHelper()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKAudioHelper);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors startAudio(IntPtr zmVideoSDKAudioHelper);
    public void StartAudio(Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(startAudio(objC_ZMVideoSDKAudioHelper));
        });
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors stopAudio(IntPtr zmVideoSDKAudioHelper);
    public void StopAudio(Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(stopAudio(objC_ZMVideoSDKAudioHelper));
        });
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors muteAudio(IntPtr zmVideoSDKAudioHelper, IntPtr zmVideoSDKUser);
    public void MuteAudio(ZMVideoSDKUser zmVideoSDKUser, Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            MacZMVideoSDKUser macZMVideoSDKUser = zmVideoSDKUser.VideoSDKUser;
            callback(muteAudio(objC_ZMVideoSDKAudioHelper, macZMVideoSDKUser.ObjC_ZMVideoSDKUser));
        });
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors unMuteAudio(IntPtr zmVideoSDKAudioHelper, IntPtr zmVideoSDKUser);
    public void UnMuteAudio(ZMVideoSDKUser zmVideoSDKUser, Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            MacZMVideoSDKUser macZMVideoSDKUser = zmVideoSDKUser.VideoSDKUser;
            callback(unMuteAudio(objC_ZMVideoSDKAudioHelper, macZMVideoSDKUser.ObjC_ZMVideoSDKUser));
        });
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr getSpeakerList(IntPtr zmVideoSDKAudioHelper, IntPtr elementCount);
    public List<ZMVideoSDKSpeakerDevice> GetSpeakerList()
    {
        List<ZMVideoSDKSpeakerDevice> speakerList = new List<ZMVideoSDKSpeakerDevice>();
        IntPtr pElementCount = Marshal.AllocHGlobal(sizeof(Int32));
        IntPtr pSpeakers = getSpeakerList(objC_ZMVideoSDKAudioHelper, pElementCount);
        int elementCount = Marshal.ReadInt32(pElementCount);
        int elementSize = Marshal.SizeOf(typeof(IntPtr));
        for (int i = 0; i < elementCount; i++)
        {
            IntPtr pSpeaker = Marshal.ReadIntPtr(pSpeakers, i * elementSize);
            ZMVideoSDKSpeakerDevice speaker;
            speaker = (ZMVideoSDKSpeakerDevice)Marshal.PtrToStructure(pSpeaker, typeof(ZMVideoSDKSpeakerDevice));
            speakerList.Add(speaker);
            Marshal.FreeHGlobal(pSpeaker);
        }
        Marshal.FreeHGlobal(pElementCount);
        Marshal.FreeHGlobal(pSpeakers);
        return speakerList;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr getMicList(IntPtr zmVideoSDKAudioHelper, IntPtr elementCount);
    public List<ZMVideoSDKMicDevice> GetMicList()
    {
        List<ZMVideoSDKMicDevice> micList = new List<ZMVideoSDKMicDevice>();
        IntPtr pElementCount = Marshal.AllocHGlobal(sizeof(Int32));
        IntPtr pMics = getMicList(objC_ZMVideoSDKAudioHelper, pElementCount);
        int elementCount = Marshal.ReadInt32(pElementCount);
        int elementSize = Marshal.SizeOf(typeof(IntPtr));
        for (int i = 0; i < elementCount; i++)
        {
            IntPtr pMic = Marshal.ReadIntPtr(pMics, i * elementSize);
            ZMVideoSDKMicDevice mic;
            mic = (ZMVideoSDKMicDevice)Marshal.PtrToStructure(pMic, typeof(ZMVideoSDKMicDevice));
            micList.Add(mic);
            Marshal.FreeHGlobal(pMic);
        }
        Marshal.FreeHGlobal(pElementCount);
        Marshal.FreeHGlobal(pMics);
        return micList;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors selectSpeaker(IntPtr zmVideoSDKAudioHelper, string deviceID, string deviceName);
    public ZMVideoSDKErrors SelectSpeaker(string deviceID, string deviceName)
    {
        return selectSpeaker(objC_ZMVideoSDKAudioHelper, deviceID, deviceName);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors selectMic(IntPtr zmVideoSDKAudioHelper, string deviceID, string deviceName);
    public ZMVideoSDKErrors SelectMic(string deviceID, string deviceName)
    {
        return selectMic(objC_ZMVideoSDKAudioHelper, deviceID, deviceName);
    }
 
    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors subscribe(IntPtr zmVideoSDKAudioHelper);
    public void Subscribe(Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(subscribe(objC_ZMVideoSDKAudioHelper));
        });
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors unSubscribe(IntPtr zmVideoSDKAudioHelper);
    public void UnSubscribe(Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(unSubscribe(objC_ZMVideoSDKAudioHelper));
        });
    }
}
#endif
