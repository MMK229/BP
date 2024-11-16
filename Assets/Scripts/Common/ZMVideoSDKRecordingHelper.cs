using System;
public class ZMVideoSDKRecordingHelper
{

#if UNITY_STANDALONE_OSX
    private MacZMVideoSDKRecordingHelper _videoSDKRecordingHelper = null;
    public ZMVideoSDKRecordingHelper(MacZMVideoSDKRecordingHelper recordingHelper)
    {
        _videoSDKRecordingHelper = recordingHelper;
    }
#elif UNITY_ANDROID
    private AndroidZoomVideoSDKRecordingHelper _videoSDKRecordingHelper = null;
    public ZMVideoSDKRecordingHelper(AndroidZoomVideoSDKRecordingHelper videoSDKRecordingHelper)
    {
        _videoSDKRecordingHelper = videoSDKRecordingHelper;
    }
#elif UNITY_STANDALONE_WIN
    private WindowsZoomVideoSDKRecordingHelper _videoSDKRecordingHelper = null;
    public ZMVideoSDKRecordingHelper(WindowsZoomVideoSDKRecordingHelper recordingHelper)
    {
        _videoSDKRecordingHelper = recordingHelper;
    }
#elif UNITY_IOS
    private IOSZMVideoSDKRecordingHelper _videoSDKRecordingHelper = null;
    public ZMVideoSDKRecordingHelper(IOSZMVideoSDKRecordingHelper recordingHelper)
    {
        _videoSDKRecordingHelper = recordingHelper;
    }
#endif

    /**
     * Checks if the current user meets the requirements to start cloud recording.
     * The following are the prerequisites to use the helper class:
     * <ul>
     *     <li>A cloud recording add-on plan</li>
     *     <li>Cloud recording feature enabled on the Web portal</li>
     * </ul>
     *
     * @return <code>Errors_Success</code> if the current user meets the requirements to start cloud recording.
     * Otherwise, the current user does not meeting the requirements to start recording
     * See error codes defined in {@link ZMVideoSDKErrors}.
     */
    public ZMVideoSDKErrors CanStartRecording()
    {
        return _videoSDKRecordingHelper.CanStartRecording();
    }

    /**
     * Start cloud recording.
     *
     * Since cloud recording involves asynchronous operations, a return value of
     * <code>Errors_Success</code> does not guarantee that the recording will start.
     * See {@link ZMVideoSDKDelegate#onCloudRecordingStatus} for information on
     * how to confirm that recording has commenced.
     *
     * @return <code>Errors_Success</code> if the start cloud recording request was successful.
     * Otherwise, the start cloud recording request failed.
     * See error codes defined in {@link ZMVideoSDKErrors}.
     */
    public void StartCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKRecordingHelper.StartCloudRecording(callback);
    }

    /**
     * Stop cloud recording.
     *
     * Since cloud recording involves asynchronous operations, a return value of
     * <code>Errors_Success</code> does not guarantee that the recording will stop.
     * See {@link ZMVideoSDKDelegate#onCloudRecordingStatus} for information on
     * how to confirm that recording has ended.
     *
     * @return <code>Errors_Success</code> if the stop cloud recording request was successful.
     * Otherwise, the stop cloud recording request failed.
     * See error codes defined in {@link ZMVideoSDKErrors}.
     */
    public void StopCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKRecordingHelper.StopCloudRecording(callback);
    }

    /**
     * Pause the ongoing cloud recording.
     *
     * Since cloud recording involves asynchronous operations, a return value of
     * <code>Errors_Success</code> does not guarantee that the recording will pause.
     * See {@link ZMVideoSDKDelegate#onCloudRecordingStatus} for information on
     * how to confirm that recording has paused.
     *
     * @return <code>Errors_Success</code> if the pause cloud recording request was successful.
     * Otherwise, the pause cloud recording request failed.
     * See error codes defined in {@link ZoomVideoSDKErrors}.
     */
    public void PauseCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKRecordingHelper.PauseCloudRecording(callback);
    }

    /**
     * Resume the previously paused cloud recording.
     *
     * Since cloud recording involves asynchronous operations, a return value of
     * <code>Errors_Success</code> does not guarantee that the recording will resume.
     * See {@link ZMVideoSDKDelegate#onCloudRecordingStatus} for information on
     * how to confirm that recording has resumed.
     *
     * @return <code>Errors_Success</code> if the resume cloud recording request was successful.
     * Otherwise, the resume cloud recording request failed.
     * See error codes defined in {@link ZoomVideoSDKErrors}.
     */
    public void ResumeCloudRecording(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKRecordingHelper.ResumeCloudRecording(callback);
    }

    /**
     * Get the current status of cloud recording.
     *
     * @return cloud recording status value defined in {@link ZMVideoSDKRecordingStatus}.
     */
    public ZMVideoSDKRecordingStatus GetCloudRecordingStatus()
    {
        return _videoSDKRecordingHelper.GetCloudRecordingStatus();
    }
}

