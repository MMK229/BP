using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct ZMVideoSDKExtendParams
{
    [MarshalAs(UnmanagedType.LPStr)]
    public string speakerTestFilePath;
}

public class ZMVideoSDKInitParams
{
    public string domain = "https://zoom.us";
    public string logFilePrefix = "ZoomVideoSDK";
    public bool enableLog = false;
    public bool enableFullHD = false;
    public ZMVideoSDKRawDataMemoryMode audioRawDataMemoryMode = ZMVideoSDKRawDataMemoryMode.ZMVideoSDKRawDataMemoryMode_Heap;
    public ZMVideoSDKRawDataMemoryMode videoRawDataMemoryMode = ZMVideoSDKRawDataMemoryMode.ZMVideoSDKRawDataMemoryMode_Heap;
    public ZMVideoSDKRawDataMemoryMode shareRawDataMemoryMode = ZMVideoSDKRawDataMemoryMode.ZMVideoSDKRawDataMemoryMode_Heap;
    public string teamIdentifier;
    public ZMVideoSDKExtendParams extendParams;
#if UNITY_STANDALONE_WIN
    public bool enableIndirectRawdata;
#elif UNITY_IOS
    public string appGroupIdentifier;
#endif
}

[StructLayout(LayoutKind.Sequential)]
public struct ZMVideoSDKVideoOption
{
    [MarshalAs(UnmanagedType.I1)]
    public bool localVideoOn;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZMVideoSDKAudioOption
{
    [MarshalAs(UnmanagedType.I1)]
    public bool connect;

    [MarshalAs(UnmanagedType.I1)]
    public bool mute;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZMVideoSDKSessionContext
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
    public uint sessionIdleTimeoutMins;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct ZMVideoSDKSpeakerDevice
{
    [MarshalAs(UnmanagedType.LPStr)]
    public string deviceID;

    [MarshalAs(UnmanagedType.LPStr)]
    public string deviceName;

    [MarshalAs(UnmanagedType.I1)]
    public bool isSelectedDevice;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct ZMVideoSDKMicDevice
{
    [MarshalAs(UnmanagedType.LPStr)]
    public string deviceID;

    [MarshalAs(UnmanagedType.LPStr)]
    public string deviceName;

    [MarshalAs(UnmanagedType.I1)]
    public bool isSelectedDevice;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ZMVideoSDKVideoStatus
{
    [MarshalAs(UnmanagedType.I1)]
    public bool isHasVideoDevice;

    [MarshalAs(UnmanagedType.I1)]
    public bool isOn;
}

public struct ZMVideoSDKSessionAudioStatisticInfo
{
    public int sendFrequency;
    public int sendLatency;
    public int sendJitter;
    public int recvFrequency;
    public int recvLatency;
    public int recvJitter;
    public float recvPacketLossAvg;
    public float recvPacketLossMax;
    public float sendPacketLossAvg;
    public float sendPacketLossMax;
}

public struct ZMVideoSDKSessionASVStatisticInfo
{
    public int sendFrameWidth;
    public int sendFrameHeight;
    public int sendFps;
    public int sendLatency;
    public int sendJitter;
    public float sendPacketLossAvg;
    public float sendPacketLossMax;
    public int recvFrameWidth;
    public int recvFrameHeight;
    public int recvFps;
    public int recvLatency;
    public int recvJitter;
    public float recvPacketLossAvg;
    public float recvPacketLossMax;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZMVideoSDKAudioStatus
{
    public ZMVideoSDKAudioType audioType;
    [MarshalAs(UnmanagedType.I1)]
    public bool isMuted;
    [MarshalAs(UnmanagedType.I1)]
    public bool isTalking;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct ZMVideoSDKAudioStatus_s
{
    public int audioType;

    [MarshalAs(UnmanagedType.I1)]
    public bool isMuted;

    [MarshalAs(UnmanagedType.I1)]
    public bool isTalking;
}

public struct ZMVideoSDKVideoStatisticInfo
{
    public int width;
    public int height;
    public int fps;
    public int bpf;
    public ZMVideoSDKNetworkStatus videoNetworkStatus;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct ZMVideoSDKCameraDevice
{
    [MarshalAs(UnmanagedType.LPStr)]
    public string deviceID;

    [MarshalAs(UnmanagedType.LPStr)]
    public string deviceName;

    [MarshalAs(UnmanagedType.I1)]
    public bool isSelectedDevice;

    [MarshalAs(UnmanagedType.I1)]
    public bool isSelectedAsMultiCamera;

    [MarshalAs(UnmanagedType.I1)]
    public bool isRunningAsMultiCamera;
}

public struct ZMVideoSDKPreferenceSetting
{
    public ZMVideoSDKVideoPreferenceMode mode;
    public uint minimumFrameRate;
    public uint maximumFrameRate;
}

[SerializeField]
public struct ZMVideoSDKChatMessage
{
    public string msgId;

    public ZMVideoSDKUser senderUser;

    public ZMVideoSDKUser receiverUser;

    public string content;

    public long timeStamp;

    public bool isChatToAll;

    public bool isSelfSend;

}

[StructLayout(LayoutKind.Sequential)]
public struct ZMVideoSDKShareOption
{
    [MarshalAs(UnmanagedType.I1)]
    public bool isWithDeviceAudio;            ///<share option, true: share computer sound when share screen/window, otherwise not.
    
    [MarshalAs(UnmanagedType.I1)]
    public bool isOptimizeForSharedVideo;     ///<share option, true: optimize the frame rate when share screen/window, otherwise not.
}