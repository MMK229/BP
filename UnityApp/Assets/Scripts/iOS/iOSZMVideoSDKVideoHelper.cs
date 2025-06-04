#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class IOSZMVideoSDKVideoHelper
{
    private IntPtr objC_ZMVideoSDKVideoHelper;

    public IOSZMVideoSDKVideoHelper(IntPtr _objC_ZMVideoSDKVideoHelper)
    {
        objC_ZMVideoSDKVideoHelper = _objC_ZMVideoSDKVideoHelper;
    }

    [DllImport ("__Internal")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKVideoHelper);
    ~IOSZMVideoSDKVideoHelper()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKVideoHelper);
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors startVideo(IntPtr zmVideoSDKVideoHelper);
    public void StartVideo(Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(startVideo(objC_ZMVideoSDKVideoHelper));
        });
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors stopVideo(IntPtr zmVideoSDKVideoHelper);
    public void StopVideo(Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(stopVideo(objC_ZMVideoSDKVideoHelper));
        });
    }

    [DllImport ("__Internal")]
    private static extern bool rotateMyVideo(IntPtr zmVideoSDKVideoHelper, ZMVideoRotation rotation);
    public void RotateMyVideo(ZMVideoRotation rotation, Action<bool> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(rotateMyVideo(objC_ZMVideoSDKVideoHelper, rotation));
        });
    }

    [DllImport ("__Internal")]
    private static extern bool switchCamera(IntPtr zmVideoSDKVideoHelper);
    public void SwitchCamera(Action<bool> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(switchCamera(objC_ZMVideoSDKVideoHelper));
        });
    }

    public bool SelectCamera(string cameraDeviceID)
    {
        throw new NotImplementedException();
    }

    public uint GetNumberOfCameras()
    {
        throw new NotImplementedException();
    }

    public List<ZMVideoSDKCameraDevice> GetCameraList()
    {
        throw new NotImplementedException();
    }

    public (ZMVideoSDKErrors, bool) CanControlCamera(string deviceID)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors TurnCameraLeft(uint range, string deviceID)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors TurnCameraRight(uint range, string deviceID)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors TurnCameraUp(uint range, string deviceID)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors TurnCameraDown(uint range, string deviceID)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors ZoomCameraIn(uint range, string deviceID)
    {
        throw new NotImplementedException();
    }

    public ZMVideoSDKErrors ZoomCameraOut(uint range, string deviceID)
    {
        throw new NotImplementedException();
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors setVideoQualityPreference(IntPtr zmVideoSDKVideoHelper, ZMVideoSDKPreferenceSetting preferenceSetting);
    public ZMVideoSDKErrors SetVideoQualityPreference(ZMVideoSDKPreferenceSetting preferenceSetting, Action<ZMVideoSDKErrors> callback)
    {
        return setVideoQualityPreference(objC_ZMVideoSDKVideoHelper, preferenceSetting);
    }

    public bool EnableMultiStreamVideo(string cameraDeviceID, string customDeviceName)
    {
        throw new NotImplementedException();
    }

    public bool DisableMultiStreamVideo(string cameraDeviceID)
    {
        throw new NotImplementedException();
    }

    public string GetDeviceIDByMyPipe(ZMVideoSDKRawDataPipe zmVideoSDKRawDataPipe)
    {
        throw new NotImplementedException();
    }

    /* todo
    - (ZMVideoSDKErrors)startVideoPreview:(id<ZMVideoSDKRawDataPipeDelegate>)listener deviceID:(NSString*)cameraDeviceID;
    - (ZMVideoSDKErrors)stopVideoPreview:(id<ZMVideoSDKRawDataPipeDelegate>)listener;
    */

    [DllImport ("__Internal")]
    private static extern bool isOriginalAspectRatioEnabled(IntPtr zmVideoSDKVideoHelper);
    public bool IsOriginalAspectRatioEnabled()
    {
        return isOriginalAspectRatioEnabled(objC_ZMVideoSDKVideoHelper);
    }

    [DllImport ("__Internal")]
    private static extern bool enableOriginalAspectRatio(IntPtr zmVideoSDKVideoHelper, bool bEnabled);
    public void EnableOriginalAspectRatio(bool bEnabled, Action<bool> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(enableOriginalAspectRatio(objC_ZMVideoSDKVideoHelper, bEnabled));
        });
    }

     public bool IsMyVideoMirrored()
    {
        throw new NotImplementedException();
    }
}
#endif
