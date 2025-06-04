using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKRecordingStatus
{
	public AndroidZoomVideoSDKRecordingStatus()
	{
	}

    private static AndroidJavaClass recordingStatusClass = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_RECORDING_STATUS_PATH);

    private static Dictionary<string, ZMVideoSDKRecordingStatus> enumDic
        = new Dictionary<string, ZMVideoSDKRecordingStatus>()
        {
            {recordingStatusClass.GetStatic<AndroidJavaObject>("Recording_Start").Call<string>("name"), ZMVideoSDKRecordingStatus.Start},
            {recordingStatusClass.GetStatic<AndroidJavaObject>("Recording_Stop").Call<string>("name"), ZMVideoSDKRecordingStatus.Stop},
            {recordingStatusClass.GetStatic<AndroidJavaObject>("Recording_DiskFull").Call<string>("name"), ZMVideoSDKRecordingStatus.DiskFull},
            {recordingStatusClass.GetStatic<AndroidJavaObject>("Recording_Pause").Call<string>("name"), ZMVideoSDKRecordingStatus.Pause},
        };

    public static ZMVideoSDKRecordingStatus GetEnum(string status)
    {
        ZMVideoSDKRecordingStatus recordingStatus;
        try
        {
            recordingStatus = enumDic[status];
        }
        catch (KeyNotFoundException)
        {
            recordingStatus = ZMVideoSDKRecordingStatus.Stop;
        }
        return recordingStatus;
    }
}

