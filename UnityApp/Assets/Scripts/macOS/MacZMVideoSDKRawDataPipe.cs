#if UNITY_STANDALONE_OSX
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MacZMVideoSDKRawDataPipe
{
    private IntPtr objC_ZMVideoSDKRawDataPipe;
    RawDataPipeDelegates pipeDelegates = null;
    private string userId;

    public IntPtr ObjC_ZMVideoSDKRawDataPipe
    {
        get { return objC_ZMVideoSDKRawDataPipe; }
    }

    public MacZMVideoSDKRawDataPipe(IntPtr _objC_ZMVideoSDKRawDataPipe, string userId)
    {
        objC_ZMVideoSDKRawDataPipe = _objC_ZMVideoSDKRawDataPipe;
        this.userId = userId;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKRawDataPipe);
    ~MacZMVideoSDKRawDataPipe()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKRawDataPipe);
    }
    
    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKRawDataType getRawdataType(IntPtr zmVideoSDKRawDataPipe);
    public ZMVideoSDKRawDataType GetRawdataType()
    {
        return getRawdataType(objC_ZMVideoSDKRawDataPipe);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
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

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern string getVideoDeviceName(IntPtr zmVideoSDKRawDataPipe);
    public string GetVideoDeviceName()
    {
        return getVideoDeviceName(objC_ZMVideoSDKRawDataPipe);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKShareStatus getShareStatus(IntPtr zmVideoSDKRawDataPipe);
    public ZMVideoSDKShareStatus GetShareStatus()
    {
        return getShareStatus(objC_ZMVideoSDKRawDataPipe);
    }

    [DllImport ("ZMMacUnityVideoSDK", CharSet = CharSet.Ansi)]
    private static extern ZMVideoSDKErrors subscribeVideoRawData(IntPtr zmVideoSDKRawDataPipe, ZMVideoSDKResolution zmVideoSDKResolution, IntPtr framePointer, IntPtr statusPointer, string userId);
    public ZMVideoSDKErrors Subscribe(ZMVideoSDKResolution zmVideoSDKResolution, IZMVideoSDKRawDataPipeDelegate dataDelegate)
    {
        if (pipeDelegates == null)
        {
            pipeDelegates = new RawDataPipeDelegates();
        }
        pipeDelegates.AddDelagate(userId, dataDelegate);
        return subscribeVideoRawData(objC_ZMVideoSDKRawDataPipe, zmVideoSDKResolution, pipeDelegates.GetFunctionPointerForRawDataFrameReceived(), pipeDelegates.GetFunctionPointerForRawDataStatusChanged(), userId);
    }

    [DllImport ("ZMMacUnityVideoSDK", CharSet = CharSet.Ansi)]
    private static extern ZMVideoSDKErrors unSubscribeVideoRawData(IntPtr zmVideoSDKRawDataPipe, string userId);
    public ZMVideoSDKErrors UnSubscribe()
    {
        return unSubscribeVideoRawData(objC_ZMVideoSDKRawDataPipe, userId);
    }

    private class RawDataPipeDelegates
    {
        delegate void fn_onRawDataStatusChanged(string userId, ZMVideoSDKRawDataStatus rawDataStatus);
        delegate void fn_onRawDataFrameReceived(string userId, IntPtr videoSDKRawData);

        fn_onRawDataFrameReceived _onRawDataFrameReceived;
        fn_onRawDataStatusChanged _onRawDataStatusChanged;
        private static Dictionary<string, IZMVideoSDKRawDataPipeDelegate> _rawDataPipeDelegates = new Dictionary<string, IZMVideoSDKRawDataPipeDelegate>();

        public RawDataPipeDelegates()
        {
            _onRawDataFrameReceived = onRawDataFrameReceived;
            _onRawDataStatusChanged = onRawDataStatusChanged;
        }

        public void AddDelagate(string userId, IZMVideoSDKRawDataPipeDelegate dataDelegate)
        {
            _rawDataPipeDelegates[userId] = dataDelegate;
        }

        public IntPtr GetFunctionPointerForRawDataFrameReceived()
        {
            return Marshal.GetFunctionPointerForDelegate(_onRawDataFrameReceived);
        }

        public IntPtr GetFunctionPointerForRawDataStatusChanged()
        {
            return Marshal.GetFunctionPointerForDelegate(_onRawDataStatusChanged);
        }

        public struct ZMVideoSDKYUVRawDataI420_struct
        {
            public int height;
            public int width;
            public int rotation;
            public IntPtr yBytes;
            public IntPtr uBytes;
            public IntPtr vBytes;
        };

        public static void onRawDataFrameReceived(string userId, IntPtr videoSDKRawData)
        {
            ZMVideoSDKYUVRawDataI420_struct rawDataStruct;
            rawDataStruct = (ZMVideoSDKYUVRawDataI420_struct)Marshal.PtrToStructure(videoSDKRawData, typeof(ZMVideoSDKYUVRawDataI420_struct));


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
            if (userId != null && _rawDataPipeDelegates.ContainsKey(userId))
            {
                _rawDataPipeDelegates[userId].onRawDataFrameReceived(rawData);
            }
        }

        public static void onRawDataStatusChanged(string userId, ZMVideoSDKRawDataStatus rawDataStatus)
        {
            if (userId != null && _rawDataPipeDelegates.ContainsKey(userId)) {
                _rawDataPipeDelegates[userId].onRawDataStatusChanged(rawDataStatus);
            }
        }
    }
}
#endif
