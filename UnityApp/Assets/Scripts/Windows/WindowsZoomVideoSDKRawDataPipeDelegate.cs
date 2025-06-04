#if UNITY_STANDALONE_WIN
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class WindowsZoomVideoSDKRawDataPipeDelegate
{
    private static Dictionary<string, IZMVideoSDKRawDataPipeDelegate> delegateInstances = new Dictionary<string, IZMVideoSDKRawDataPipeDelegate>();

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    delegate void fn_onRawDataFrameReceived(ZMVideoSDKYUVRawDataI420_w rawDataStruct, IntPtr pRawData, string userId);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    delegate void fn_onRawDataStatusChanged(ZMVideoSDKRawDataStatus status, string userId);

    public WindowsZoomVideoSDKRawDataPipeDelegate()
    {
    }

    public void AddDelegate(string userId, IZMVideoSDKRawDataPipeDelegate delegateObj)
    {
        delegateInstances.TryAdd(userId, delegateObj);

        fn_onRawDataFrameReceived _onRawDataFrameReceived = new fn_onRawDataFrameReceived(onRawDataFrameReceived);
        fn_onRawDataStatusChanged _onRawDataStatusChanged = new fn_onRawDataStatusChanged(onRawDataStatusChanged);

        onRawDataFrameReceived_c(_onRawDataFrameReceived);
        onRawDataStatusChanged_c(_onRawDataStatusChanged);
    }

    public void RemoveDelegate(string userId)
    {
        delegateInstances.Remove(userId);
    }

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    private static extern void onRawDataFrameReceived_c(fn_onRawDataFrameReceived func);

    [DllImport("ZMWinUnityVideoSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    private static extern void onRawDataStatusChanged_c(fn_onRawDataStatusChanged func);

    public static void onRawDataFrameReceived(ZMVideoSDKYUVRawDataI420_w rawDataStruct, IntPtr pRawData, string userId)
    {
        if (!delegateInstances.ContainsKey(userId))
        {
            return;
        }
        WindowsZoomVideoSDKVideoRawData winRawData = new WindowsZoomVideoSDKVideoRawData(pRawData);
        winRawData.AddRef();
        int yCapacity = Convert.ToInt32(rawDataStruct.width * rawDataStruct.height);
        int uCapacity = yCapacity / 4;
        int vCapacity = uCapacity;

        byte[] yBytes = new byte[yCapacity];
        byte[] uBytes = new byte[uCapacity];
        byte[] vBytes = new byte[vCapacity];

        int height = Convert.ToInt32(rawDataStruct.height);
        int width = Convert.ToInt32(rawDataStruct.width);
        int rotation = Convert.ToInt32(rawDataStruct.rotation);

        Marshal.Copy(rawDataStruct.yBytes, yBytes, 0, yCapacity);
        Marshal.Copy(rawDataStruct.uBytes, uBytes, 0, uCapacity);
        Marshal.Copy(rawDataStruct.vBytes, vBytes, 0, vCapacity);

        ZMVideoSDKVideoRawData rawData = new ZMVideoSDKVideoRawData(height, width, rotation, yBytes, uBytes, vBytes);

        winRawData.ReleaseRef();
        delegateInstances[userId].onRawDataFrameReceived(rawData);
    }
    public static void onRawDataStatusChanged(ZMVideoSDKRawDataStatus status, string userId)
    {
        if (delegateInstances.ContainsKey(userId))
        {
            delegateInstances[userId].onRawDataStatusChanged(status);
        }
    }
}

#endif