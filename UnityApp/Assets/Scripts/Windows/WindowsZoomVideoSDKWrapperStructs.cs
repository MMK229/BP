#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
struct ZoomVideoSDKExtendParams_w
{
    [MarshalAs(UnmanagedType.LPStr)]
    public string speakerTestFilePath;
    public int wrapperType;
};

[StructLayout(LayoutKind.Sequential)]
struct ZoomVideoSDKInitParams_w
{
    [MarshalAs(UnmanagedType.LPStr)]
    public string domain;
    [MarshalAs(UnmanagedType.LPStr)]
    public string logFilePrefix;
    [MarshalAs(UnmanagedType.I1)]
    public bool enableLog;
    [MarshalAs(UnmanagedType.I1)]
    public bool enableIndirectRawdata;
    public ZMVideoSDKRawDataMemoryMode audioRawDataMemoryMode;
    public ZMVideoSDKRawDataMemoryMode videoRawDataMemoryMode;
    public ZMVideoSDKRawDataMemoryMode shareRawDataMemoryMode;
    public ZMVideoSDKExtendParams extendParam;
};

[StructLayout(LayoutKind.Sequential)]
public struct ZoomVideoSDKSessionContext_w
{
    [MarshalAs(UnmanagedType.LPStr)]
    public string sessionName;
    [MarshalAs(UnmanagedType.LPStr)]
    public string sessionPassword;
    [MarshalAs(UnmanagedType.LPStr)]
    public string userName;
    [MarshalAs(UnmanagedType.LPStr)]
    public string token;
    public ZMVideoSDKVideoOption videoOption;
    public ZMVideoSDKAudioOption audioOption;
    public int sessionIdleTimeoutMins;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZMVideoSDKYUVRawDataI420_w
{
    public uint height;
    public uint width;
    public uint rotation;
    public IntPtr yBytes;
    public IntPtr uBytes;
    public IntPtr vBytes;
};

public struct ZMVideoSDKAudioRawData_w
{
    public int bufferLen;
    public int sampleRate;
    public int channelNum;
    public IntPtr buffer;
};

public struct CanControlCameraResult
{
    public ZMVideoSDKErrors result;
    public bool canControl;
};

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct ZMVideoSDKChatMessage_w
{
    [MarshalAs(UnmanagedType.LPStr)]
    public string msgId;

    public IntPtr senderUser;

    public IntPtr receiverUser;

    [MarshalAs(UnmanagedType.LPStr)]
    public string content;

    public long timeStamp;

    [MarshalAs(UnmanagedType.I1)]
    public bool isChatToAll;

    [MarshalAs(UnmanagedType.I1)]
    public bool isSelfSend;

}
#endif