#if UNITY_STANDALONE_WIN
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class WindowsZoomVideoSDKVideoHelper
{
    private IntPtr videoHelper;

    public WindowsZoomVideoSDKVideoHelper(IntPtr videoHelper)
    {
        this.videoHelper = videoHelper;
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors setVideoQualityPreference_c(IntPtr videoHelper, ZMVideoSDKPreferenceSetting config);
    public void SetVideoQualityPreference(ZMVideoSDKPreferenceSetting config, Action<ZMVideoSDKErrors> callback)
    {
        callback(setVideoQualityPreference_c(videoHelper, config));
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr getCameraList_c(IntPtr videoHelper, IntPtr pCount);
    public List<ZMVideoSDKCameraDevice> GetCameraList()
    {
        IntPtr pElementCount = Marshal.AllocHGlobal(sizeof(Int32));
        IntPtr pCameras = getCameraList_c(videoHelper, pElementCount);
        int elementCount = Marshal.ReadInt32(pElementCount);
        List<ZMVideoSDKCameraDevice> cameraList = new List<ZMVideoSDKCameraDevice>();
        int elementSize = Marshal.SizeOf(typeof(IntPtr));
        for (int i = 0; i < elementCount; i++)
        {
            IntPtr pCamera = Marshal.ReadIntPtr(pCameras, i * elementSize);
            ZMVideoSDKCameraDevice camera;
            camera = (ZMVideoSDKCameraDevice)Marshal.PtrToStructure(pCamera, typeof(ZMVideoSDKCameraDevice));
            cameraList.Add(camera);
            Marshal.FreeHGlobal(pCamera);
        }
        Marshal.FreeHGlobal(pCameras);
        Marshal.FreeHGlobal(pElementCount);
        return cameraList;
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern uint getNumberOfCameras_c(IntPtr videoHelper);
    public uint GetNumberOfCameras()
    {
        return getNumberOfCameras_c(videoHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool rotateMyVideo_c(IntPtr videoHelper, ZMVideoRotation rotation);
    public void RotateMyVideo(ZMVideoRotation rotation, Action<bool> callback)
    {
        callback(rotateMyVideo_c(videoHelper, rotation));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors startVideo_c(IntPtr videoHelper);
    public void StartVideo(Action<ZMVideoSDKErrors> callback)
    {
        callback(startVideo_c(videoHelper));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors stopVideo_c(IntPtr videoHelper);
    public void StopVideo(Action<ZMVideoSDKErrors> callback)
    {
        callback(stopVideo_c(videoHelper));
    }

    public void SwitchCamera(string deviceId)
    {
        throw new NotImplementedException();
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool switchCamera_c(IntPtr videoHelper);
    public void SwitchCamera(Action<bool> callback)
    {
        callback(switchCamera_c(videoHelper));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool mirrorMyVideo_c(IntPtr videoHelper, bool enable);
    public void MirrorMyVideo(bool enable)
    {
        mirrorMyVideo_c(videoHelper, enable);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool isMyVideoMirrored_c(IntPtr videoHelper);
    public bool IsMyVideoMirrored()
    {
        return isMyVideoMirrored_c(videoHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool isOriginalAspectRatioEnabled_c(IntPtr videoHelper);
    public bool IsOriginalAspectRatioEnabled()
    {
        return isOriginalAspectRatioEnabled_c(videoHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool enableOriginalAspectRatio_c(IntPtr videoHelper, bool enable);
    public void EnableOriginalAspectRatio(bool enable, Action<bool> callback)
    {
        callback(enableOriginalAspectRatio_c(videoHelper, enable));
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern CanControlCameraResult canControlCamera_c(IntPtr videoHelper, string deviceID);
    public (ZMVideoSDKErrors, bool) CanControlCamera(string deviceID)
    {
        CanControlCameraResult res = canControlCamera_c(videoHelper, deviceID);
        return (res.result, res.canControl);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern ZMVideoSDKErrors turnCameraLeft_c(IntPtr videoHelper, uint range, string deviceID);
    public ZMVideoSDKErrors TurnCameraLeft(uint range, string deviceID)
    {
        return turnCameraLeft_c(videoHelper, range, deviceID);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern ZMVideoSDKErrors turnCameraRight_c(IntPtr videoHelper, uint range, string deviceID);
    public ZMVideoSDKErrors TurnCameraRight(uint range, string deviceID)
    {
        return turnCameraRight_c(videoHelper, range, deviceID);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern ZMVideoSDKErrors turnCameraUp_c(IntPtr videoHelper, uint range, string deviceID);
    public ZMVideoSDKErrors TurnCameraUp(uint range, string deviceID)
    {
        return turnCameraUp_c(videoHelper, range, deviceID);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern ZMVideoSDKErrors turnCameraDown_c(IntPtr videoHelper, uint range, string deviceID);
    public ZMVideoSDKErrors TurnCameraDown(uint range, string deviceID)
    {
        return turnCameraDown_c(videoHelper, range, deviceID);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern ZMVideoSDKErrors zoomCameraIn_c(IntPtr videoHelper, uint range, string deviceID);
    public ZMVideoSDKErrors ZoomCameraIn(uint range, string deviceID)
    {
        return zoomCameraIn_c(videoHelper, range, deviceID);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern ZMVideoSDKErrors zoomCameraOut_c(IntPtr videoHelper, uint range, string deviceID);
    public ZMVideoSDKErrors ZoomCameraOut(uint range, string deviceID)
    {
        return zoomCameraOut_c(videoHelper, range, deviceID);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern bool enableMultiStreamVideo_c(IntPtr videoHelper, string deviceID, string deviceName);
    public bool EnableMultiStreamVideo(string cameraDeviceID, string customDeviceName)
    {
        return enableMultiStreamVideo_c(videoHelper, cameraDeviceID, customDeviceName);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern bool disableMultiStreamVideo_c(IntPtr videoHelper, string deviceID);
    public bool DisableMultiStreamVideo(string cameraDeviceID)
    {
        return disableMultiStreamVideo_c(videoHelper, cameraDeviceID);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern string getDeviceIDByMyPipe_c(IntPtr videoHelper, IntPtr pipe);
    public string GetDeviceIDByMyPipe(ZMVideoSDKRawDataPipe zmVideoSDKRawDataPipe)
    {
        return getDeviceIDByMyPipe_c(videoHelper, zmVideoSDKRawDataPipe.VideoSDKRawDataPipe.Instance());
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi)]
    private static extern bool selectCamera_c(IntPtr videoHelper, string deviceID);
    public bool SelectCamera(string cameraDeviceID)
    {
        return selectCamera_c(videoHelper, cameraDeviceID);
    }
}
#endif