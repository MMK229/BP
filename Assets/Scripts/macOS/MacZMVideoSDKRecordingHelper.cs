#if UNITY_STANDALONE_OSX
using System;
using System.Runtime.InteropServices;

public class MacZMVideoSDKRecordingHelper
{
    public MacZMVideoSDKRecordingHelper(IntPtr _objC_ZMVideoSDKRecordingHelper)
    {
        objC_ZMVideoSDKRecordingHelper = _objC_ZMVideoSDKRecordingHelper;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKRecordingHelper);
    ~MacZMVideoSDKRecordingHelper()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKRecordingHelper);
    }

    private IntPtr objC_ZMVideoSDKRecordingHelper;
    public IntPtr ObjC_ZMVideoSDKRecordingHelper
    {
        get { return objC_ZMVideoSDKRecordingHelper; }
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern ZMVideoSDKErrors canStartRecording(IntPtr zmVideoSDKRecordingHelper);
    public ZMVideoSDKErrors CanStartRecording()
    {
        return canStartRecording(objC_ZMVideoSDKRecordingHelper);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern ZMVideoSDKErrors startCloudRecording(IntPtr zmVideoSDKRecordingHelper);
    public void StartCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        callback(startCloudRecording(objC_ZMVideoSDKRecordingHelper));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern ZMVideoSDKErrors stopCloudRecording(IntPtr zmVideoSDKRecordingHelper);
    public void StopCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        callback(stopCloudRecording(objC_ZMVideoSDKRecordingHelper));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern ZMVideoSDKErrors pauseCloudRecording(IntPtr zmVideoSDKRecordingHelper);
    public void PauseCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        callback(pauseCloudRecording(objC_ZMVideoSDKRecordingHelper));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern ZMVideoSDKErrors resumeCloudRecording(IntPtr zmVideoSDKRecordingHelper);
    public void ResumeCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        callback(resumeCloudRecording(objC_ZMVideoSDKRecordingHelper));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern ZMVideoSDKRecordingStatus getCloudRecordingStatus(IntPtr zmVideoSDKRecordingHelper);
    public ZMVideoSDKRecordingStatus GetCloudRecordingStatus()
    {
        return getCloudRecordingStatus(objC_ZMVideoSDKRecordingHelper);
    }
}
#endif
