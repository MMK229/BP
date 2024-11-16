#if UNITY_STANDALONE_OSX
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MacZMVideoSDKVideoHelper
{
    private IntPtr objC_ZMVideoSDKVideoHelper;

    public MacZMVideoSDKVideoHelper(IntPtr _objC_ZMVideoSDKVideoHelper)
    {
        objC_ZMVideoSDKVideoHelper = _objC_ZMVideoSDKVideoHelper;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKVideoHelper);
    ~MacZMVideoSDKVideoHelper()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKVideoHelper);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors startVideo(IntPtr zmVideoSDKVideoHelper);
    public void StartVideo(Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(startVideo(objC_ZMVideoSDKVideoHelper));
        });
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors stopVideo(IntPtr zmVideoSDKVideoHelper);
    public void StopVideo(Action<ZMVideoSDKErrors> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(stopVideo(objC_ZMVideoSDKVideoHelper));
        });
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool rotateMyVideo(IntPtr zmVideoSDKVideoHelper, ZMVideoRotation rotation);
    public void RotateMyVideo(ZMVideoRotation rotation, Action<bool> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(rotateMyVideo(objC_ZMVideoSDKVideoHelper, rotation));
        });
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool switchCamera(IntPtr zmVideoSDKVideoHelper);
    public void SwitchCamera(Action<bool> callback)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            callback(switchCamera(objC_ZMVideoSDKVideoHelper));
        });
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool selectCamera(IntPtr zmVideoSDKVideoHelper, string cameraDeviceID);
    public bool SelectCamera(string cameraDeviceID)
    {
        return selectCamera(objC_ZMVideoSDKVideoHelper, cameraDeviceID);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern uint getNumberOfCameras(IntPtr zmVideoSDKVideoHelper);
    public uint GetNumberOfCameras()
    {
        return getNumberOfCameras(objC_ZMVideoSDKVideoHelper);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr getCameraList(IntPtr zmVideoSDKVideoHelper, IntPtr elementCount);
    public List<ZMVideoSDKCameraDevice> GetCameraList()
    {
        // todo: fix GetCameraList crash issue
        List<ZMVideoSDKCameraDevice> cameraList = new List<ZMVideoSDKCameraDevice>();
        IntPtr pElementCount = Marshal.AllocHGlobal(sizeof(Int32));
        IntPtr pCameras = getCameraList(objC_ZMVideoSDKVideoHelper, pElementCount);
        int elementCount = Marshal.ReadInt32(pElementCount);
        int elementSize = Marshal.SizeOf(typeof(IntPtr));
        for (int i = 0; i < elementCount; i++)
        {
            IntPtr pCamera = Marshal.ReadIntPtr(pCameras, i * elementSize);
            ZMVideoSDKCameraDevice camera;
            camera = (ZMVideoSDKCameraDevice)Marshal.PtrToStructure(pCamera, typeof(ZMVideoSDKCameraDevice));
            cameraList.Add(camera);
            Marshal.FreeHGlobal(pCamera);
        }
        Marshal.FreeHGlobal(pElementCount);
        Marshal.FreeHGlobal(pCameras);
        return cameraList;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors canControlCamera(IntPtr zmVideoSDKVideoHelper, IntPtr canControl, string deviceID);
    public (ZMVideoSDKErrors, bool) CanControlCamera(string deviceID)
    {
        IntPtr pCanControl = Marshal.AllocHGlobal(sizeof(Int32));
        ZMVideoSDKErrors error = canControlCamera(objC_ZMVideoSDKVideoHelper, pCanControl, deviceID);
        int canControl = Marshal.ReadInt32(pCanControl);
        bool bCanControl = canControl == 1 ? true : false;
        return (error, bCanControl);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors turnCameraLeft(IntPtr zmVideoSDKVideoHelper, uint range, string deviceID);
    public ZMVideoSDKErrors TurnCameraLeft(uint range, string deviceID)
    {
        return turnCameraLeft(objC_ZMVideoSDKVideoHelper, range, deviceID);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors turnCameraRight(IntPtr zmVideoSDKVideoHelper, uint range, string deviceID);
    public ZMVideoSDKErrors TurnCameraRight(uint range, string deviceID)
    {
        return turnCameraRight(objC_ZMVideoSDKVideoHelper, range, deviceID);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors turnCameraUp(IntPtr zmVideoSDKVideoHelper, uint range, string deviceID);
    public ZMVideoSDKErrors TurnCameraUp(uint range, string deviceID)
    {
        return turnCameraUp(objC_ZMVideoSDKVideoHelper, range, deviceID);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors turnCameraDown(IntPtr zmVideoSDKVideoHelper, uint range, string deviceID);
    public ZMVideoSDKErrors TurnCameraDown(uint range, string deviceID)
    {
        return turnCameraDown(objC_ZMVideoSDKVideoHelper, range, deviceID);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors zoomCameraIn(IntPtr zmVideoSDKVideoHelper, uint range, string deviceID);
    public ZMVideoSDKErrors ZoomCameraIn(uint range, string deviceID)
    {
        return zoomCameraIn(objC_ZMVideoSDKVideoHelper, range, deviceID);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors zoomCameraOut(IntPtr zmVideoSDKVideoHelper, uint range, string deviceID);
    public ZMVideoSDKErrors ZoomCameraOut(uint range, string deviceID)
    {
        return zoomCameraOut(objC_ZMVideoSDKVideoHelper, range, deviceID);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors setVideoQualityPreference(IntPtr zmVideoSDKVideoHelper, ZMVideoSDKPreferenceSetting preferenceSetting);
    public ZMVideoSDKErrors SetVideoQualityPreference(ZMVideoSDKPreferenceSetting preferenceSetting, Action<ZMVideoSDKErrors> callback)
    {
        return setVideoQualityPreference(objC_ZMVideoSDKVideoHelper, preferenceSetting);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool enableMultiStreamVideo(IntPtr zmVideoSDKVideoHelper, string cameraDeviceID, string customDeviceName);
    public bool EnableMultiStreamVideo(string cameraDeviceID, string customDeviceName)
    {
        return enableMultiStreamVideo(objC_ZMVideoSDKVideoHelper, cameraDeviceID, customDeviceName);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool disableMultiStreamVideo(IntPtr zmVideoSDKVideoHelper, string cameraDeviceID);
    public bool DisableMultiStreamVideo(string cameraDeviceID)
    {
        return disableMultiStreamVideo(objC_ZMVideoSDKVideoHelper, cameraDeviceID);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern string getDeviceIDByMyPipe(IntPtr zmVideoSDKVideoHelper, IntPtr objC_ZMVideoSDKRawDataPipe);
    public string GetDeviceIDByMyPipe(ZMVideoSDKRawDataPipe zmVideoSDKRawDataPipe)
    {
        throw new NotImplementedException();
    }

    /* todo
    - (ZMVideoSDKErrors)startVideoPreview:(id<ZMVideoSDKRawDataPipeDelegate>)listener deviceID:(NSString*)cameraDeviceID;
    - (ZMVideoSDKErrors)stopVideoPreview:(id<ZMVideoSDKRawDataPipeDelegate>)listener;
    */

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool isOriginalAspectRatioEnabled(IntPtr zmVideoSDKVideoHelper);
    public bool IsOriginalAspectRatioEnabled()
    {
        return isOriginalAspectRatioEnabled(objC_ZMVideoSDKVideoHelper);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
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
