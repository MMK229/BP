#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class IOSZMVideoSDKRawDataPipe
{
    private IntPtr objC_ZMVideoSDKRawDataPipe;
    RawDataPipeDelegates pipeDelegates = null;
    delegate void fn_onRawDataStatusChanged(ZMVideoSDKRawDataStatus rawDataStatus, string userID);
    delegate void fn_onRawDataFrameReceived(IntPtr videoSDKRawData);
    private string userID;

    public IntPtr ObjC_ZMVideoSDKRawDataPipe
    {
        get { return objC_ZMVideoSDKRawDataPipe; }
    }

    public IOSZMVideoSDKRawDataPipe(IntPtr _objC_ZMVideoSDKRawDataPipe, string userId)
    {
        objC_ZMVideoSDKRawDataPipe = _objC_ZMVideoSDKRawDataPipe;
        userID = userId;
    }

    [DllImport ("__Internal")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKRawDataPipe);
    ~IOSZMVideoSDKRawDataPipe()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKRawDataPipe);
    }
    
    [DllImport ("__Internal")]
    private static extern ZMVideoSDKRawDataType getRawdataType(IntPtr zmVideoSDKRawDataPipe);
    public ZMVideoSDKRawDataType GetRawdataType()
    {
        return getRawdataType(objC_ZMVideoSDKRawDataPipe);
    }

    [DllImport ("__Internal")]
    private static extern IntPtr getVideoStatus(IntPtr zmVideoSDKRawDataPipe);
    public ZMVideoSDKVideoStatus GetVideoStatus()
    {
        IntPtr pVideoStatus = getVideoStatus(objC_ZMVideoSDKRawDataPipe);
        ZMVideoSDKVideoStatus videoStatus;
        videoStatus = (ZMVideoSDKVideoStatus)Marshal.PtrToStructure(pVideoStatus, typeof(ZMVideoSDKVideoStatus));
        Marshal.FreeHGlobal(pVideoStatus);

        ZMVideoSDKVideoStatus status = new ZMVideoSDKVideoStatus();
        status.isOn = videoStatus.isOn;
        status.isHasVideoDevice = videoStatus.isHasVideoDevice;
        return status;
    }
    
    public string GetVideoDeviceName()
    {
        throw new NotImplementedException();
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKShareStatus getShareStatus(IntPtr zmVideoSDKRawDataPipe);
    public ZMVideoSDKShareStatus GetShareStatus()
    {
        return getShareStatus(objC_ZMVideoSDKRawDataPipe);
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors subscribeVideoRawData(IntPtr zmVideoSDKRawDataPipe, ZMVideoSDKResolution zmVideoSDKResolution, fn_onRawDataFrameReceived framePointer, fn_onRawDataStatusChanged statusChangedPointer, string userId);
    public ZMVideoSDKErrors Subscribe(ZMVideoSDKResolution zmVideoSDKResolution, IZMVideoSDKRawDataPipeDelegate dataDelegate)
    {
        if (pipeDelegates == null)
        {
            pipeDelegates = new RawDataPipeDelegates();
        }
        pipeDelegates.AddDelagate(dataDelegate, userID);
        return subscribeVideoRawData(objC_ZMVideoSDKRawDataPipe, zmVideoSDKResolution, pipeDelegates.GetFunctionPointerForRawDataFrameReceived(), pipeDelegates.GetFunctionPointerForRawDataStatusChanged(), userID);
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors unSubscribeVideoRawData(IntPtr zmVideoSDKRawDataPipe, string userId);
    public ZMVideoSDKErrors UnSubscribe()
    {
        if (pipeDelegates == null)
        {
            return ZMVideoSDKErrors.ZMVideoSDKErrors_Wrong_Usage;
        }
        pipeDelegates.RemoveDelagate(userID);
        return unSubscribeVideoRawData(objC_ZMVideoSDKRawDataPipe, userID);
    }

    private class RawDataPipeDelegates
    {
        private static Dictionary<string, IZMVideoSDKRawDataPipeDelegate> rawDataPipeDic = new Dictionary<string, IZMVideoSDKRawDataPipeDelegate>();

        public RawDataPipeDelegates()
        {
        }

        public void AddDelagate(IZMVideoSDKRawDataPipeDelegate dataDelegate, string userId)
        {
            rawDataPipeDic[userId] = dataDelegate;
        }

        public void RemoveDelagate(string userId)
        {
            rawDataPipeDic.Remove(userId);
        }

        public fn_onRawDataFrameReceived GetFunctionPointerForRawDataFrameReceived()
        {
            return onRawDataFrameReceived;
        }

        public fn_onRawDataStatusChanged GetFunctionPointerForRawDataStatusChanged()
        {
            return onRawDataStatusChanged;
        }

        public struct ZMVideoSDKYUVRawDataI420_struct
        {
            public int height;
            public int width;
            public int rotation;
            public IntPtr yBytes;
            public IntPtr uBytes;
            public IntPtr vBytes;
            public string userID;
        };

        [AOT.MonoPInvokeCallback(typeof(fn_onRawDataFrameReceived))]
        public static void onRawDataFrameReceived(IntPtr videoSDKRawData)
        {
            ZMVideoSDKYUVRawDataI420_struct rawDataStruct;
            rawDataStruct = (ZMVideoSDKYUVRawDataI420_struct)Marshal.PtrToStructure(videoSDKRawData, typeof(ZMVideoSDKYUVRawDataI420_struct));
            IZMVideoSDKRawDataPipeDelegate rawDataPipeDelegate = rawDataPipeDic[rawDataStruct.userID];
            if (rawDataPipeDelegate == null)
            {
                return;
            }

            int yCapacity = rawDataStruct.width * rawDataStruct.height;
            int uCapacity = yCapacity / 4;
            int vCapacity = uCapacity;
            byte[] yBytes = new byte[yCapacity];
            byte[] uBytes = new byte[uCapacity];
            byte[] vBytes = new byte[vCapacity];

            Marshal.Copy(rawDataStruct.yBytes, yBytes, 0, yCapacity);
            Marshal.Copy(rawDataStruct.uBytes, uBytes, 0, uCapacity);
            Marshal.Copy(rawDataStruct.vBytes, vBytes, 0, vCapacity);

            ZMVideoSDKVideoRawData rawData = new ZMVideoSDKVideoRawData(rawDataStruct.height, rawDataStruct.width, rawDataStruct.rotation, yBytes, uBytes, vBytes);
            Marshal.FreeHGlobal(videoSDKRawData);
            rawDataPipeDelegate.onRawDataFrameReceived(rawData);
        }

        [AOT.MonoPInvokeCallback(typeof(fn_onRawDataStatusChanged))]
        public static void onRawDataStatusChanged(ZMVideoSDKRawDataStatus rawDataStatus, string userID)
        {
            IZMVideoSDKRawDataPipeDelegate rawDataPipeDelegate = rawDataPipeDic[userID];
            if (rawDataPipeDelegate == null)
            {
                return;
            }
            rawDataPipeDelegate.onRawDataStatusChanged(rawDataStatus);
        }
    }
}
#endif
