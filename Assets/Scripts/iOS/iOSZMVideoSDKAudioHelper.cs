#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class IOSZMVideoSDKAudioHelper
{
    private IntPtr objC_ZMVideoSDKAudioHelper;

    public IOSZMVideoSDKAudioHelper(IntPtr _objC_ZMVideoSDKAudioHelper)
    {
        objC_ZMVideoSDKAudioHelper = _objC_ZMVideoSDKAudioHelper;
    }

    [DllImport ("__Internal")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKAudioHelper);
    ~IOSZMVideoSDKAudioHelper()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKAudioHelper);
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors startAudio(IntPtr zmVideoSDKAudioHelper);
    public void StartAudio(Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(startAudio(objC_ZMVideoSDKAudioHelper));
        });
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors stopAudio(IntPtr zmVideoSDKAudioHelper);
    public void StopAudio(Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(stopAudio(objC_ZMVideoSDKAudioHelper));
        });
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors muteAudio(IntPtr zmVideoSDKAudioHelper, IntPtr zmVideoSDKUser);
    public void MuteAudio(ZMVideoSDKUser zmVideoSDKUser, Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            IOSZMVideoSDKUser macZMVideoSDKUser = zmVideoSDKUser.VideoSDKUser;
            callback(muteAudio(objC_ZMVideoSDKAudioHelper, macZMVideoSDKUser.ObjC_ZMVideoSDKUser));
        });
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors unMuteAudio(IntPtr zmVideoSDKAudioHelper, IntPtr zmVideoSDKUser);
    public void UnMuteAudio(ZMVideoSDKUser zmVideoSDKUser, Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            IOSZMVideoSDKUser macZMVideoSDKUser = zmVideoSDKUser.VideoSDKUser;
            callback(unMuteAudio(objC_ZMVideoSDKAudioHelper, macZMVideoSDKUser.ObjC_ZMVideoSDKUser));
        });
    }

    public List<ZMVideoSDKSpeakerDevice> GetSpeakerList()
    {
        throw new NotImplementedException();
    }

    public List<ZMVideoSDKMicDevice> GetMicList()
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors SelectSpeaker(string deviceID, string deviceName)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors SelectMic(string deviceID, string deviceName)
    {
        throw new NotImplementedException();
    }
 
    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors subscribe(IntPtr zmVideoSDKAudioHelper);
    public void Subscribe(Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(subscribe(objC_ZMVideoSDKAudioHelper));
        });
    }

    [DllImport ("__Internal")]
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
