#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;
public class WindowsZoomVideoSDKRecordingHelper
{
    private IntPtr recordingHelper;

    public WindowsZoomVideoSDKRecordingHelper(IntPtr recordingHelper)
    {
        this.recordingHelper = recordingHelper;
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors canStartRecording_c(IntPtr helper);
    public ZMVideoSDKErrors CanStartRecording()
    {
        return canStartRecording_c(recordingHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors startCloudRecording_c(IntPtr helper);
    public void StartCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        callback(startCloudRecording_c(recordingHelper));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors stopCloudRecording_c(IntPtr helper);
    public void StopCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        callback(stopCloudRecording_c(recordingHelper));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors pauseCloudRecording_c(IntPtr helper);
    public void PauseCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        callback(pauseCloudRecording_c(recordingHelper));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors resumeCloudRecording_c(IntPtr helper);
    public void ResumeCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        callback(resumeCloudRecording_c(recordingHelper));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKRecordingStatus getCloudRecordingStatus_c(IntPtr helper);
    public ZMVideoSDKRecordingStatus GetCloudRecordingStatus()
    {
        return getCloudRecordingStatus_c(recordingHelper);
    }
}
#endif