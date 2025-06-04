#if UNITY_STANDALONE_WIN
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class WindowsZoomVideoSDKDelegate
{

    private static IZMVideoSDKDelegate _videoSDKDelegate;
    public WindowsZoomVideoSDKDelegate(IZMVideoSDKDelegate videoSDKDelegate)
    {
        _videoSDKDelegate = videoSDKDelegate;

        fn_no_params _onSessionJoin = new fn_no_params(WindowsZoomVideoSDKDelegate.onSessionJoin);
        fn_no_params _onSessionLeave = new fn_no_params(WindowsZoomVideoSDKDelegate.onSessionLeave);
        fn_no_params _onHostAskUnmute = new fn_no_params(WindowsZoomVideoSDKDelegate.onHostAskUnmute);
        fn_no_params _onSelectedAudioDeviceChanged = new fn_no_params(WindowsZoomVideoSDKDelegate.onSelectedAudioDeviceChanged);
        fn_error_int _onError = new fn_error_int(WindowsZoomVideoSDKDelegate.onError);
        fn_uint_uint _onMicSpeakerVolumeChanged = new fn_uint_uint(WindowsZoomVideoSDKDelegate.onMicSpeakerVolumeChanged);
        fn_obj _onUserManagerChanged = new fn_obj(WindowsZoomVideoSDKDelegate.onUserManagerChanged);
        fn_obj _onUserNameChanged = new fn_obj(WindowsZoomVideoSDKDelegate.onUserNameChanged);
        fn_obj_obj_int _onUserJoin = new fn_obj_obj_int(WindowsZoomVideoSDKDelegate.onUserJoin);
        fn_obj_obj_int _onUserLeave = new fn_obj_obj_int(WindowsZoomVideoSDKDelegate.onUserLeave);
        fn_obj_obj_int _onUserVideoStatusChanged = new fn_obj_obj_int(WindowsZoomVideoSDKDelegate.onUserVideoStatusChanged);
        fn_obj_obj_int _onUserAudioStatusChanged = new fn_obj_obj_int(WindowsZoomVideoSDKDelegate.onUserAudioStatusChanged);
        fn_obj_obj _onUserHostChanged = new fn_obj_obj(WindowsZoomVideoSDKDelegate.onUserHostChanged);
        fn_obj_obj_int _onUserActiveAudioChanged = new fn_obj_obj_int(WindowsZoomVideoSDKDelegate.onUserActiveAudioChanged);
        fn_obj_obj _onMixedAudioRawDataReceived = new fn_obj_obj(WindowsZoomVideoSDKDelegate.onMixedAudioRawDataReceived);
        fn_obj_obj_obj _onOneWayAudioRawDataReceived = new fn_obj_obj_obj(WindowsZoomVideoSDKDelegate.onOneWayAudioRawDataReceived);
        fn_obj_chat _onChatNewMessageNotify = new fn_obj_chat(WindowsZoomVideoSDKDelegate.onChatNewMessageNotify);
        fn_obj_str_dType _onChatDeleteMessageNotify = new fn_obj_str_dType(WindowsZoomVideoSDKDelegate.onChatDeleteMessageNotify);
        fn_onCloudRecordingStatusChanged _onCloudRecordingStatusChanged = new fn_onCloudRecordingStatusChanged(WindowsZoomVideoSDKDelegate.onCloudRecordingStatusChanged);
        fn_onUserShareStatusChanged _onUserShareStatusChanged = new fn_onUserShareStatusChanged(WindowsZoomVideoSDKDelegate.onUserShareStatusChanged);

        onSessionJoin_c(_onSessionJoin);
        onSessionLeave_c(_onSessionLeave);
        onHostAskUnmute_c(_onHostAskUnmute);
        onSelectedAudioDeviceChanged_c(_onSelectedAudioDeviceChanged);
        onError_c(_onError);
        onMicSpeakerVolumeChanged_c(_onMicSpeakerVolumeChanged);
        onUserManagerChanged_c(_onUserManagerChanged);
        onUserNameChanged_c(_onUserNameChanged);
        onUserJoin_c(_onUserJoin);
        onUserLeave_c(_onUserLeave);
        onUserVideoStatusChanged_c(_onUserVideoStatusChanged);
        onUserAudioStatusChanged_c(_onUserAudioStatusChanged);
        onUserHostChanged_c(_onUserHostChanged);
        onUserActiveAudioChanged_c(_onUserActiveAudioChanged);
        onMixedAudioRawDataReceived_c(_onMixedAudioRawDataReceived);
        onOneWayAudioRawDataReceived_c(_onOneWayAudioRawDataReceived);
        onChatNewMessageNotify_c(_onChatNewMessageNotify);
        onChatMsgDeleteNotification_c(_onChatDeleteMessageNotify);
        onCloudRecordingStatus_c(_onCloudRecordingStatusChanged);
        onUserShareStatusChanged_c(_onUserShareStatusChanged);
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void fn_no_params();

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void fn_error_int(ZMVideoSDKErrors errorType, int details);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void fn_uint_uint(uint value1, uint value2);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void fn_obj(IntPtr obj1);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void fn_obj_obj(IntPtr obj1, IntPtr obj2);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void fn_obj_obj_int(IntPtr obj1, IntPtr arrayObj2, int arrayElementCount);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void fn_obj_obj_obj(IntPtr obj1, IntPtr obj2, IntPtr obj3);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void fn_obj_chat(IntPtr obj1, ZMVideoSDKChatMessage_w message);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void fn_obj_str_dType(IntPtr obj1, string msgID, ZMVideoSDKChatMessageDeleteType deleteType);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void fn_onCloudRecordingStatusChanged(ZMVideoSDKRecordingStatus status, IntPtr obj1);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void fn_onUserShareStatusChanged(IntPtr obj1, IntPtr obj2, ZMVideoSDKShareStatus status, ZMVideoSDKShareType type);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onSessionJoin_c(fn_no_params func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onSessionLeave_c(fn_no_params func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onError_c(fn_error_int func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onUserJoin_c(fn_obj_obj_int func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onUserLeave_c(fn_obj_obj_int func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onUserVideoStatusChanged_c(fn_obj_obj_int func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onUserAudioStatusChanged_c(fn_obj_obj_int func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onUserHostChanged_c(fn_obj_obj func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onUserActiveAudioChanged_c(fn_obj_obj_int func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onUserManagerChanged_c(fn_obj func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onUserNameChanged_c(fn_obj func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onHostAskUnmute_c(fn_no_params func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onMicSpeakerVolumeChanged_c(fn_uint_uint func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onSelectedAudioDeviceChanged_c(fn_no_params func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onMixedAudioRawDataReceived_c(fn_obj_obj func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onOneWayAudioRawDataReceived_c(fn_obj_obj_obj func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onChatNewMessageNotify_c(fn_obj_chat func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    private static extern void onChatMsgDeleteNotification_c(fn_obj_str_dType func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onCloudRecordingStatus_c(fn_onCloudRecordingStatusChanged func);

    [DllImport("ZMWinUnityVideoSDK.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern void onUserShareStatusChanged_c(fn_onUserShareStatusChanged func);

    public static void onSessionJoin()
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        ZMVideoSDKSession session = ZMVideoSDK.Instance.GetSessionInfo();
        ZMVideoSDKUser myself = session.GetMySelf();
        _videoSDKDelegate.onSessionJoin(myself);
    }


    public static void onSessionLeave()
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        _videoSDKDelegate.onSessionLeave();
    }

    public static void onError(ZMVideoSDKErrors errorType, int details)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        _videoSDKDelegate.onError(errorType, details);
    }

    public static void onUserJoin(IntPtr userHelper, IntPtr pUsers, int elementCount)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        List<ZMVideoSDKUser> userList = WindowsZoomVideoSDKUser.GetUserList(pUsers, elementCount);
        ZMVideoSDKUserHelper helper = new ZMVideoSDKUserHelper(new WindowsZoomVideoSDKUserHelper(userHelper));
        _videoSDKDelegate.onUserJoin(helper, userList);
    }

    public static void onUserLeave(IntPtr userHelper, IntPtr pUsers, int elementCount)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        ZMVideoSDKUserHelper helper = new ZMVideoSDKUserHelper(new WindowsZoomVideoSDKUserHelper(userHelper));
        List<ZMVideoSDKUser> userList = WindowsZoomVideoSDKUser.GetUserList(pUsers, elementCount);
        _videoSDKDelegate.onUserLeave(helper, userList);
    }

    public static void onUserVideoStatusChanged(IntPtr videoHelper, IntPtr pUsers, int elementCount)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        ZMVideoSDKVideoHelper helper = new ZMVideoSDKVideoHelper(new WindowsZoomVideoSDKVideoHelper(videoHelper));
        List<ZMVideoSDKUser> userList = WindowsZoomVideoSDKUser.GetUserList(pUsers, elementCount);
        _videoSDKDelegate.onUserVideoStatusChanged(helper, userList);
    }

    public static void onUserAudioStatusChanged(IntPtr audioHelper, IntPtr pUsers, int elementCount)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        ZMVideoSDKAudioHelper helper = new ZMVideoSDKAudioHelper(new WindowsZoomVideoSDKAudioHelper(audioHelper));
        List<ZMVideoSDKUser> userList = WindowsZoomVideoSDKUser.GetUserList(pUsers, elementCount);
        _videoSDKDelegate.onUserAudioStatusChanged(helper, userList);
    }

    public static void onUserHostChanged(IntPtr userHelper, IntPtr user)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        _videoSDKDelegate.onUserHostChanged(new ZMVideoSDKUserHelper(new WindowsZoomVideoSDKUserHelper(userHelper)), new ZMVideoSDKUser(new WindowsZoomVideoSDKUser(user)));
    }

    public static void onUserActiveAudioChanged(IntPtr audioHelper, IntPtr pUsers, int elementCount)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        ZMVideoSDKAudioHelper helper = new ZMVideoSDKAudioHelper(new WindowsZoomVideoSDKAudioHelper(audioHelper));
        List<ZMVideoSDKUser> userList = WindowsZoomVideoSDKUser.GetUserList(pUsers, elementCount);
        _videoSDKDelegate.onUserActiveAudioChanged(helper, userList);
    }

    public static void onUserManagerChanged(IntPtr user)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        _videoSDKDelegate.onUserManagerChanged(new ZMVideoSDKUser(new WindowsZoomVideoSDKUser(user)));
    }

    public static void onUserNameChanged(IntPtr user)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        _videoSDKDelegate.onUserNameChanged(new ZMVideoSDKUser(new WindowsZoomVideoSDKUser(user)));
    }

    public static void onHostAskUnmute()
    {
        if (_videoSDKDelegate != null)
        {
            _videoSDKDelegate.onHostAskUnmute();
        }
    }

    public static void onMicSpeakerVolumeChanged(uint micVolume, uint speakerVolume)
    {
        if (_videoSDKDelegate != null)
        {
            _videoSDKDelegate.onMicSpeakerVolumeChanged(micVolume, speakerVolume);
        }
    }

    public static void onSelectedAudioDeviceChanged()
    {
        if (_videoSDKDelegate != null)
        {
            _videoSDKDelegate.onSelectedAudioDeviceChanged();
        }
    }

    public static void onMixedAudioRawDataReceived(IntPtr audioRawData, IntPtr pRawData)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        ZMVideoSDKAudioRawData_w rawDataStruct;
        rawDataStruct = (ZMVideoSDKAudioRawData_w)Marshal.PtrToStructure(audioRawData, typeof(ZMVideoSDKAudioRawData_w));
        ZMVideoSDKAudioRawData videoSDKAudioRawData = new ZMVideoSDKAudioRawData(rawDataStruct.buffer, rawDataStruct.bufferLen, rawDataStruct.sampleRate, rawDataStruct.channelNum);
        Marshal.FreeHGlobal(audioRawData);
        _videoSDKDelegate.onMixedAudioRawDataReceived(videoSDKAudioRawData);
    }

    public static void onOneWayAudioRawDataReceived(IntPtr audioRawData, IntPtr user, IntPtr pRawData)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        ZMVideoSDKAudioRawData_w rawDataStruct;
        rawDataStruct = (ZMVideoSDKAudioRawData_w)Marshal.PtrToStructure(audioRawData, typeof(ZMVideoSDKAudioRawData_w));
        ZMVideoSDKAudioRawData videoSDKAudioRawData = new ZMVideoSDKAudioRawData(rawDataStruct.buffer, rawDataStruct.bufferLen, rawDataStruct.sampleRate, rawDataStruct.channelNum);
        Marshal.FreeHGlobal(audioRawData);
        _videoSDKDelegate.onOneWayAudioRawDataReceived(videoSDKAudioRawData, new ZMVideoSDKUser(new WindowsZoomVideoSDKUser(user)));
    }

    public static void onChatNewMessageNotify(IntPtr pChatHelper, ZMVideoSDKChatMessage_w messageStruct)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        WindowsZoomVideoSDKChatHelper winChatHelper = new WindowsZoomVideoSDKChatHelper(pChatHelper);
        ZMVideoSDKChatHelper chatHelper = new ZMVideoSDKChatHelper(winChatHelper);
        ZMVideoSDKChatMessage message = new ZMVideoSDKChatMessage();
        message.isChatToAll = messageStruct.isChatToAll;
        message.isSelfSend = messageStruct.isSelfSend;
        message.msgId = messageStruct.msgId;
        message.content = messageStruct.content;
        message.timeStamp = messageStruct.timeStamp;
        message.senderUser = new ZMVideoSDKUser(new WindowsZoomVideoSDKUser(messageStruct.senderUser));
        message.receiverUser = new ZMVideoSDKUser(new WindowsZoomVideoSDKUser(messageStruct.receiverUser));
        _videoSDKDelegate.onChatNewMessageNotify(chatHelper, message);
    }

    public static void onChatDeleteMessageNotify(IntPtr pChatHelper, string msgID, ZMVideoSDKChatMessageDeleteType deleteBy)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        WindowsZoomVideoSDKChatHelper winChatHelper = new WindowsZoomVideoSDKChatHelper(pChatHelper);
        ZMVideoSDKChatHelper chatHelper = new ZMVideoSDKChatHelper(winChatHelper);
        _videoSDKDelegate.onChatDeleteMessageNotify(chatHelper, msgID, deleteBy);
    }

    public static void onCloudRecordingStatusChanged(ZMVideoSDKRecordingStatus status, IntPtr pHandler)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        WindowsZoomVideoSDKRecordingConsentHandler winHandler = new WindowsZoomVideoSDKRecordingConsentHandler(pHandler);
        ZMVideoSDKRecordingConsentHandler handler = new ZMVideoSDKRecordingConsentHandler(winHandler);
        _videoSDKDelegate.onCloudRecordingStatusChanged(status, handler);
    }

    public static void onUserShareStatusChanged(IntPtr pShareHelper, IntPtr pUser, ZMVideoSDKShareStatus status, ZMVideoSDKShareType type)
    {
        if (_videoSDKDelegate == null)
        {
            return;
        }
        WindowsZoomVideoSDKShareHelper winHelper = new WindowsZoomVideoSDKShareHelper(pShareHelper);
        ZMVideoSDKShareHelper shareHelper = new ZMVideoSDKShareHelper(winHelper);

        WindowsZoomVideoSDKUser winUser = new WindowsZoomVideoSDKUser(pUser);
        ZMVideoSDKUser user = new ZMVideoSDKUser(winUser);

        _videoSDKDelegate.onUserShareStatusChanged(shareHelper, user, status);
    }
    
}

#endif