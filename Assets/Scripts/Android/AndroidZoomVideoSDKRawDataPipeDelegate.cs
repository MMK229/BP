#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AndroidZoomVideoSDKRawDataPipeDelegate : AndroidJavaProxy
{
    IZMVideoSDKRawDataPipeDelegate sdkRawDataDelegate;
    private IntPtr yPtr = IntPtr.Zero;
    private IntPtr uPtr = IntPtr.Zero;
    private IntPtr vPtr = IntPtr.Zero;
    private byte[] yBytes;
    private byte[] uBytes;
    private byte[] vBytes;

    public AndroidZoomVideoSDKRawDataPipeDelegate(IZMVideoSDKRawDataPipeDelegate sdkRawDataDelegate) : base(Config.ZOOM_VIDEO_SDK_RAW_DATA_PIPE_DELEGATE_PATH)
    {
        this.sdkRawDataDelegate = sdkRawDataDelegate;
    }

    public void onRawDataFrameReceived(AndroidJavaObject rawData)
    {
        using (rawData)
        {
            ZMVideoSDKVideoRawData rawDataObject = ProcessData(rawData);
            sdkRawDataDelegate.onRawDataFrameReceived(rawDataObject);
        }
    }

    public void onRawDataStatusChanged(AndroidJavaObject status)
    {
        if (status != null)
        {
            sdkRawDataDelegate.onRawDataStatusChanged(AndroidRawDataStatus.GetEnum(status.Call<string>("name")));
        }
    }

    private ZMVideoSDKVideoRawData ProcessData(AndroidJavaObject rawData)
    {
        int width = rawData.Call<int>("getStreamWidth");
        int height = rawData.Call<int>("getStreamHeight");

        int rotation = rawData.Call<int>("getRotation");

        int yCapacity = width * height;
        int uCapacity = yCapacity / 4;
        int vCapacity = uCapacity;

        long yHandle = rawData.Call<long>("getNativeYBuffer");
        long uHandle = rawData.Call<long>("getNativeUBuffer");
        long vHandle = rawData.Call<long>("getNativeVBuffer");

        yPtr = (IntPtr)yHandle;
        uPtr = (IntPtr)uHandle;
        vPtr = (IntPtr)vHandle;

        yBytes = new byte[yCapacity];
        uBytes = new byte[uCapacity];
        vBytes = new byte[vCapacity];

        Marshal.Copy(yPtr, yBytes, 0, yCapacity);
        Marshal.Copy(uPtr, uBytes, 0, uCapacity);
        Marshal.Copy(vPtr, vBytes, 0, vCapacity);

        return new ZMVideoSDKVideoRawData(height, width, rotation, yBytes, uBytes, vBytes);
    }

    private void ClearByteArray()
    {
        if (yBytes != null && yBytes.Length > 0)
        {
            Array.Clear(yBytes, 0, yBytes.Length);
        }
        if (uBytes != null && uBytes.Length > 0)
        {
            Array.Clear(uBytes, 0, uBytes.Length);
        }
        if (vBytes != null && vBytes.Length > 0)
        {
            Array.Clear(vBytes, 0, vBytes.Length);
        }
    }

    private void ClearPointer()
    {
        if (yPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(yPtr);
            yPtr = IntPtr.Zero;
        }
        if (uPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(uPtr);
            uPtr = IntPtr.Zero;
        }
        if (vPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(vPtr);
            vPtr = IntPtr.Zero;
        }
    }

    void OnDestroy()
    {
        ClearByteArray();
        ClearPointer();
    }

    internal class AndroidRawDataStatus
    {
        private static AndroidJavaClass rawDataStatusEnum = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_RAW_DATA_STATUS_PATH);

        private static Dictionary<string, ZMVideoSDKRawDataStatus> videoAspectDic
    = new Dictionary<string, ZMVideoSDKRawDataStatus>()
    {
            {rawDataStatusEnum.GetStatic<AndroidJavaObject>("RawData_On").Call<string>("name"), ZMVideoSDKRawDataStatus.ZMVideoSDKRawData_On},
            {rawDataStatusEnum.GetStatic<AndroidJavaObject>("RawData_Off").Call<string>("name"), ZMVideoSDKRawDataStatus.ZMVideoSDKRawData_Off},
    };

        public static ZMVideoSDKRawDataStatus GetEnum(string status)
        {
            ZMVideoSDKRawDataStatus rawDataStatus;
            try
            {
                rawDataStatus = videoAspectDic[status];
            }
            catch (KeyNotFoundException)
            {
                rawDataStatus = ZMVideoSDKRawDataStatus.ZMVideoSDKRawData_Off;
            }
            return rawDataStatus;
        }
    }
}

#endif
