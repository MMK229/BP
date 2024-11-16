#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class IOSZMVideoSDK
{
    private static readonly IOSZMVideoSDK instance = new IOSZMVideoSDK();
    private IZMVideoSDKDelegate callBackObject = null;  
    private ZMVideoSDKSession currentSession = null;
    static IOSZMVideoSDK()    
    {    
    }    
    private IOSZMVideoSDK()    
    {
        initializeDelegates();
        setCallbacks = false;
    }    
    public static IOSZMVideoSDK Instance
    {    
        get    
        {    
            return instance;    
        }    
    }  

    [StructLayout(LayoutKind.Sequential,CharSet = CharSet.Ansi)]
    public struct ZMVideoSDKInitParams_StructIOS
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string domain;
        [MarshalAs(UnmanagedType.LPStr)]
        public string logFilePrefix;
        [MarshalAs(UnmanagedType.LPStr)]
        public string appGroupIdentifier;
        [MarshalAs(UnmanagedType.LPStr)]
        public string extParamsSpeakerTestFilePath;
        [MarshalAs(UnmanagedType.I1)]
        public bool enableLog;
        public int audioRawDataMemoryMode;
        public int videoRawDataMemoryMode;
        public int shareRawDataMemoryMode;
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors initialize_c(IntPtr pIOSZMVideoSDKInitParams);
    public void Initialize(ZMVideoSDKInitParams zmVideoSDKInitParams, Action<ZMVideoSDKErrors> callback)
    {
        ZMVideoSDKErrors error;
        ZMVideoSDKInitParams_StructIOS zmVideoSDKInitParamsStruct = new ZMVideoSDKInitParams_StructIOS();
        zmVideoSDKInitParamsStruct.domain = zmVideoSDKInitParams.domain;
        zmVideoSDKInitParamsStruct.logFilePrefix = zmVideoSDKInitParams.logFilePrefix;
        zmVideoSDKInitParamsStruct.enableLog = zmVideoSDKInitParams.enableLog;
        zmVideoSDKInitParamsStruct.appGroupIdentifier = zmVideoSDKInitParams.appGroupIdentifier;
        zmVideoSDKInitParamsStruct.extParamsSpeakerTestFilePath = zmVideoSDKInitParams.extendParams.speakerTestFilePath;
        zmVideoSDKInitParamsStruct.audioRawDataMemoryMode = (int)zmVideoSDKInitParams.audioRawDataMemoryMode;
        zmVideoSDKInitParamsStruct.videoRawDataMemoryMode = (int)zmVideoSDKInitParams.videoRawDataMemoryMode;
        zmVideoSDKInitParamsStruct.shareRawDataMemoryMode = (int)zmVideoSDKInitParams.shareRawDataMemoryMode;
        IntPtr pCStructure = Marshal.AllocHGlobal(Marshal.SizeOf(zmVideoSDKInitParamsStruct));
        try
        {
            Marshal.StructureToPtr(zmVideoSDKInitParamsStruct, pCStructure, false);
            error = initialize_c(pCStructure);
        }
        finally
        {
            Marshal.FreeHGlobal(pCStructure);
        }
        callback(error);
    }

    [DllImport ("__Internal")]
    private static extern void cleanUp_c();
    public void Cleanup(Action<ZMVideoSDKErrors> callback)
    {
        cleanUp_c();
    }

    [DllImport ("__Internal")]
    private static extern IntPtr joinSession_c(IntPtr pIOSZMVideoSDKSessionContext);
    public ZMVideoSDKSession JoinSession(ZMVideoSDKSessionContext zmVideoSDKSessionContext, Action<ZMVideoSDKErrors> callback)
    {
        IntPtr pCStructure = Marshal.AllocHGlobal(Marshal.SizeOf(zmVideoSDKSessionContext));
        try
        {
            Marshal.StructureToPtr(zmVideoSDKSessionContext, pCStructure, false);
            IntPtr pObjCSession = joinSession_c(pCStructure);
            if (pObjCSession != IntPtr.Zero)
            {
                currentSession = new ZMVideoSDKSession(new IOSZMVideoSDKSession(pObjCSession));
                callback(ZMVideoSDKErrors.ZMVideoSDKErrors_Success);
            }
            else
            {
                callback(ZMVideoSDKErrors.ZMVideoSDKErrors_Internal_Error);
            }
        }
        finally
        {
            Marshal.FreeHGlobal(pCStructure);
        }
        return currentSession;
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors leaveSession_c(bool end);
    public void LeaveSession(bool end, Action<ZMVideoSDKErrors> callback)
    {
        ZMVideoSDKErrors error = leaveSession_c(end);
        callback(error);
    }

    [DllImport ("__Internal")]
    private static extern IntPtr getSessionInfo_c();
    public ZMVideoSDKSession GetSessionInfo()
    {
        IntPtr pObjCSession = getSessionInfo_c();
        if (pObjCSession == null)
        {
            return null;
        }
        return new ZMVideoSDKSession(new IOSZMVideoSDKSession(pObjCSession));
    }

    [DllImport ("__Internal")]
    private static extern bool isInSession_c();
    public bool IsInSession()
    {
        return isInSession_c();
    }

    [DllImport ("__Internal")]
    private static extern string getSDKVersion_c();
    public string GetSdkVersion()
    {
        return getSDKVersion_c();
    }

    [DllImport ("__Internal")]
    private static extern IntPtr getAudioHelper_c();
    public ZMVideoSDKAudioHelper GetAudioHelper()
    {
        IntPtr objC_ZMVideoSDKAudioHelper = getAudioHelper_c();
        return new ZMVideoSDKAudioHelper(new IOSZMVideoSDKAudioHelper(objC_ZMVideoSDKAudioHelper));
    }

    [DllImport ("__Internal")]
    private static extern IntPtr getVideoHelper_c();
    public ZMVideoSDKVideoHelper GetVideoHelper()
    {
        IntPtr objC_ZMVideoSDKVideoHelper = getVideoHelper_c();
        return new ZMVideoSDKVideoHelper(new IOSZMVideoSDKVideoHelper(objC_ZMVideoSDKVideoHelper));
    }

    [DllImport ("__Internal")]
    private static extern IntPtr getUserHelper_c();
    public ZMVideoSDKUserHelper GetUserHelper()
    {
        IntPtr objC_ZMVideoSDKUserHelper = getUserHelper_c();
        return new ZMVideoSDKUserHelper(new IOSZMVideoSDKUserHelper(objC_ZMVideoSDKUserHelper));
    }

    [DllImport ("__Internal")]
    private static extern IntPtr getChatHelper_c();
    public ZMVideoSDKChatHelper GetChatHelper()
    {
        IntPtr objC_ZMVideoSDKChatHelper = getChatHelper_c();
        return new ZMVideoSDKChatHelper(new IOSZMVideoSDKChatHelper(objC_ZMVideoSDKChatHelper));
    }

    [DllImport ("__Internal")]
    private static extern IntPtr getRecordingHelper_c();
    public ZMVideoSDKRecordingHelper GetRecordingHelper()
    {
        IntPtr objC_ZMVideoSDKRecordingHelper = getRecordingHelper_c();
        return new ZMVideoSDKRecordingHelper(new IOSZMVideoSDKRecordingHelper(objC_ZMVideoSDKRecordingHelper));
    }

    [DllImport ("__Internal")]
    private static extern IntPtr getShareHelper_c();
    public ZMVideoSDKShareHelper GetShareHelper()
    {
        IntPtr objC_ZMVideoSDKShareHelper = getShareHelper_c();
        return new ZMVideoSDKShareHelper(new IOSZMVideoSDKShareHelper(objC_ZMVideoSDKShareHelper));
    }

    [DllImport ("__Internal")]
    private static extern void onSessionJoin_c(fn_NoParams func);
        
    [DllImport ("__Internal")]
    private static extern void onSessionLeave_c(fn_NoParams func);

    [DllImport ("__Internal")]
    private static extern void onError_c(fn_Error_ZMVideoSDKErrors_int func);

    [DllImport ("__Internal")]
    private static extern void onUserJoin_c(fn_2ObjCWrappers_int func);

    [DllImport ("__Internal")]
    private static extern void onUserLeave_c(fn_2ObjCWrappers_int func);

    [DllImport ("__Internal")]
    private static extern void onUserVideoStatusChanged_c(fn_2ObjCWrappers_int func);

    [DllImport ("__Internal")]
    private static extern void onUserAudioStatusChanged_c(fn_2ObjCWrappers_int func);

    [DllImport ("__Internal")]
    private static extern void onUserHostChanged_c(fn_2ObjCWrappers func);

    [DllImport ("__Internal")]
    private static extern void onUserActiveAudioChanged_c(fn_2ObjCWrappers_int func);

    [DllImport ("__Internal")]
    private static extern void onUserManagerChanged_c(fn_ObjCWrapper func);

    [DllImport ("__Internal")]
    private static extern void onUserNameChanged_c(fn_ObjCWrapper func);

    [DllImport ("__Internal")]
    private static extern void onHostAskUnmute_c(fn_NoParams func);

    [DllImport ("__Internal")]
    private static extern void onMicSpeakerVolumeChanged_c(fn_uint_uint func);

    [DllImport ("__Internal")]
    private static extern void onSelectedAudioDeviceChanged_c(fn_NoParams func);

    [DllImport ("__Internal")]
    private static extern void onMixedAudioRawDataReceived_c(fn_CPointer func);

    [DllImport ("__Internal")]
    private static extern void onOneWayAudioRawDataReceived_c(fn_CPointer_ObjCWrapper func);

    [DllImport ("__Internal")]
    private static extern void onUserShareStatusChanged_c(fn_onUserShareStatusChanged func);

    [DllImport ("__Internal")]
    private static extern void onChatPrivilegeChanged_c(fn_onChatPrivilegeChanged func);

    [DllImport ("__Internal")]
    private static extern void onChatNewMessageNotify_c(fn_onChatNewMessageNotify func);

    [DllImport ("__Internal")]
    private static extern void onChatMsgDeleteNotification_c(fn_onChatMsgDeleteNotification func);

    [DllImport ("__Internal")]
    private static extern void onCloudRecordingStatusChanged_c(fn_onCloudRecordingStatusChanged func);

    [DllImport ("__Internal")]
    private static extern void onUserRecordAgreementNotification_c(fn_ObjCWrapper func);

    delegate void fn_NoParams();
    delegate void fn_Error_ZMVideoSDKErrors_int(ZMVideoSDKErrors errorType, int details);
    delegate void fn_uint_uint(uint micVolume, uint speakerVolume);
    delegate void fn_ObjCWrapper(IntPtr obj1);
    delegate void fn_2ObjCWrappers(IntPtr obj1, IntPtr obj2);
    delegate void fn_2ObjCWrappers_int(IntPtr obj1, IntPtr arrayObj2, int arrayElementCount);
    delegate void fn_CPointer(IntPtr voidPointer);
    delegate void fn_CPointer_ObjCWrapper(IntPtr voidPointer, IntPtr obj1);
    delegate void fn_onUserShareStatusChanged(IntPtr shareHelper, IntPtr user, ZMVideoSDKShareStatus status);
    delegate void fn_onChatPrivilegeChanged(IntPtr chatHelper, ZMVideoSDKChatPrivilegeType currentPrivilege);
    delegate void fn_onChatNewMessageNotify(IntPtr chatHelper, IntPtr voidPointer);
    delegate void fn_onChatMsgDeleteNotification(IntPtr chatHelper, string messageID, ZMVideoSDKChatMessageDeleteType type);
    delegate void fn_onCloudRecordingStatusChanged(ZMVideoSDKRecordingStatus status, IntPtr recordingConsentHandler);
    bool setCallbacks;

    private void initializeDelegates()
    { 
        onSessionJoin_c(onSessionJoin);
        onSessionLeave_c(onSessionLeave);
        onHostAskUnmute_c(onHostAskUnmute);
        onSelectedAudioDeviceChanged_c(onSelectedAudioDeviceChanged);
        onError_c(onError);
        onMicSpeakerVolumeChanged_c(onMicSpeakerVolumeChanged);
        onUserManagerChanged_c(onUserManagerChanged);
        onUserNameChanged_c(onUserNameChanged);
        onUserJoin_c(onUserJoin);
        onUserLeave_c(onUserLeave);
        onUserVideoStatusChanged_c(onUserVideoStatusChanged);
        onUserAudioStatusChanged_c(onUserAudioStatusChanged);
        onUserHostChanged_c(onUserHostChanged);
        onUserActiveAudioChanged_c(onUserActiveAudioChanged);
        onMixedAudioRawDataReceived_c(onMixedAudioRawDataReceived);
        onOneWayAudioRawDataReceived_c(onOneWayAudioRawDataReceived);
        onUserShareStatusChanged_c(onUserShareStatusChanged);
        onChatPrivilegeChanged_c(onChatPrivilegeChanged);
        onChatNewMessageNotify_c(onChatNewMessageNotify);
        onChatMsgDeleteNotification_c(onChatMsgDeleteNotification);
        onCloudRecordingStatusChanged_c(onCloudRecordingStatusChanged);
        onUserRecordAgreementNotification_c(onUserRecordAgreementNotification);
    }

    [DllImport ("__Internal")]
    private static extern void addListener_c();
    public void AddListener(IZMVideoSDKDelegate listener)
    {
        callBackObject = listener;
        if (!setCallbacks)
        {
            addListener_c();
            setCallbacks = true;
        }
    }

    public void RemoveListener(IZMVideoSDKDelegate listener)
    {
        callBackObject = null;
    }

    [AOT.MonoPInvokeCallback(typeof(fn_NoParams))]
    static void onSessionJoin()
    {
        if (IOSZMVideoSDK.Instance.callBackObject != null)
        {
            IOSZMVideoSDK.Instance.callBackObject.onSessionJoin(IOSZMVideoSDK.Instance.currentSession.GetMySelf());
        }
    }

    [AOT.MonoPInvokeCallback(typeof(fn_NoParams))]
    static void onSessionLeave()
    {
        if (IOSZMVideoSDK.Instance.callBackObject != null)
        {
            IOSZMVideoSDK.Instance.currentSession = null;
            IOSZMVideoSDK.Instance.callBackObject.onSessionLeave();
        }
    }

    [AOT.MonoPInvokeCallback(typeof(fn_Error_ZMVideoSDKErrors_int))]
    static void onError(ZMVideoSDKErrors errorType, int details)
    {
        if (IOSZMVideoSDK.Instance.callBackObject != null)
        {
            IOSZMVideoSDK.Instance.callBackObject.onError(errorType, details);
        }
    }
    
    static List<ZMVideoSDKUser> GetUserList(IntPtr pUsers, int elementCount)
    {
        List<ZMVideoSDKUser> userList = new List<ZMVideoSDKUser>();
        int elementSize = Marshal.SizeOf(typeof(IntPtr));
        for (int i = 0; i < elementCount; i++)
        {
            IntPtr objC_ZMVideoSDKUser = Marshal.ReadIntPtr(pUsers, i * elementSize);
            IOSZMVideoSDKUser user = new IOSZMVideoSDKUser(objC_ZMVideoSDKUser);
            userList.Add(new ZMVideoSDKUser(user));
        }
        Marshal.FreeHGlobal(pUsers);
        return userList;
    }

    [AOT.MonoPInvokeCallback(typeof(fn_2ObjCWrappers_int))]
    static void onUserJoin(IntPtr userHelper, IntPtr pUsers, int elementCount)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        
        List<ZMVideoSDKUser> userList = GetUserList(pUsers, elementCount);
        ZMVideoSDKUserHelper helper = new ZMVideoSDKUserHelper(new IOSZMVideoSDKUserHelper(userHelper));
        IOSZMVideoSDK.Instance.callBackObject.onUserJoin(helper, userList);
    }
    
    [AOT.MonoPInvokeCallback(typeof(fn_2ObjCWrappers_int))]
    static void onUserLeave(IntPtr userHelper, IntPtr pUsers, int elementCount)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        ZMVideoSDKUserHelper helper = new ZMVideoSDKUserHelper(new IOSZMVideoSDKUserHelper(userHelper));
        List<ZMVideoSDKUser> userList = GetUserList(pUsers, elementCount);
        IOSZMVideoSDK.Instance.callBackObject.onUserLeave(helper, userList);
    }

    [AOT.MonoPInvokeCallback(typeof(fn_2ObjCWrappers_int))]
    static void onUserVideoStatusChanged(IntPtr videoHelper, IntPtr pUsers, int elementCount)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        ZMVideoSDKVideoHelper helper = new ZMVideoSDKVideoHelper(new IOSZMVideoSDKVideoHelper(videoHelper));
        List<ZMVideoSDKUser> userList = GetUserList(pUsers, elementCount);
        IOSZMVideoSDK.Instance.callBackObject.onUserVideoStatusChanged(helper, userList);
    }
    
    [AOT.MonoPInvokeCallback(typeof(fn_2ObjCWrappers_int))]
    static void onUserAudioStatusChanged(IntPtr audioHelper, IntPtr pUsers, int elementCount)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        ZMVideoSDKAudioHelper helper = new ZMVideoSDKAudioHelper(new IOSZMVideoSDKAudioHelper(audioHelper));
        List<ZMVideoSDKUser> userList = GetUserList(pUsers, elementCount);
        IOSZMVideoSDK.Instance.callBackObject.onUserAudioStatusChanged(helper, userList);
    }
    
    [AOT.MonoPInvokeCallback(typeof(fn_2ObjCWrappers))]
    static void onUserHostChanged(IntPtr userHelper, IntPtr user)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        IOSZMVideoSDK.Instance.callBackObject.onUserHostChanged(new ZMVideoSDKUserHelper(new IOSZMVideoSDKUserHelper(userHelper)), new ZMVideoSDKUser(new IOSZMVideoSDKUser(user)));
    }
    
    [AOT.MonoPInvokeCallback(typeof(fn_2ObjCWrappers_int))]
    static void onUserActiveAudioChanged(IntPtr audioHelper, IntPtr pUsers, int elementCount)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        ZMVideoSDKAudioHelper helper = new ZMVideoSDKAudioHelper(new IOSZMVideoSDKAudioHelper(audioHelper));
        List<ZMVideoSDKUser> userList = GetUserList(pUsers, elementCount);
        IOSZMVideoSDK.Instance.callBackObject.onUserActiveAudioChanged(helper, userList);
    }
    
    [AOT.MonoPInvokeCallback(typeof(fn_ObjCWrapper))]
    static void onUserManagerChanged(IntPtr user)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        IOSZMVideoSDK.Instance.callBackObject.onUserManagerChanged(new ZMVideoSDKUser(new IOSZMVideoSDKUser(user)));
    }
    
    [AOT.MonoPInvokeCallback(typeof(fn_ObjCWrapper))]
    static void onUserNameChanged(IntPtr user)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        IOSZMVideoSDK.Instance.callBackObject.onUserNameChanged(new ZMVideoSDKUser(new IOSZMVideoSDKUser(user)));
    }

    [AOT.MonoPInvokeCallback(typeof(fn_NoParams))]
    static void onHostAskUnmute()
    {
        if (IOSZMVideoSDK.Instance.callBackObject != null)
        {
            IOSZMVideoSDK.Instance.callBackObject.onHostAskUnmute();
        }
    }

    [AOT.MonoPInvokeCallback(typeof(fn_uint_uint))]
    static void onMicSpeakerVolumeChanged(uint micVolume, uint speakerVolume)
    {
        if (IOSZMVideoSDK.Instance.callBackObject != null)
        {
            IOSZMVideoSDK.Instance.callBackObject.onMicSpeakerVolumeChanged(micVolume, speakerVolume);
        }
    }

    [AOT.MonoPInvokeCallback(typeof(fn_NoParams))]
    static void onSelectedAudioDeviceChanged()
    {
        if (IOSZMVideoSDK.Instance.callBackObject != null)
        {
            IOSZMVideoSDK.Instance.callBackObject.onSelectedAudioDeviceChanged();
        }
    }

    public struct ZMVideoSDKAudioRawData_struct
    {
        public IntPtr buffer;
        public int bufferLen;
        public int sampleRate;
        public int channelNum;
    };

    [AOT.MonoPInvokeCallback(typeof(fn_CPointer))]
    static void onMixedAudioRawDataReceived(IntPtr audioRawData)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        ZMVideoSDKAudioRawData_struct rawDataStruct;
        rawDataStruct = (ZMVideoSDKAudioRawData_struct)Marshal.PtrToStructure(audioRawData, typeof(ZMVideoSDKAudioRawData_struct));
        ZMVideoSDKAudioRawData videoSDKAudioRawData = new ZMVideoSDKAudioRawData(rawDataStruct.buffer, rawDataStruct.bufferLen, rawDataStruct.sampleRate, rawDataStruct.channelNum);
        Marshal.FreeHGlobal(audioRawData);
        IOSZMVideoSDK.Instance.callBackObject.onMixedAudioRawDataReceived(videoSDKAudioRawData);
    }

    [AOT.MonoPInvokeCallback(typeof(fn_CPointer_ObjCWrapper))]
    static void onOneWayAudioRawDataReceived(IntPtr audioRawData, IntPtr user)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        ZMVideoSDKAudioRawData_struct rawDataStruct;
        rawDataStruct = (ZMVideoSDKAudioRawData_struct)Marshal.PtrToStructure(audioRawData, typeof(ZMVideoSDKAudioRawData_struct));
        ZMVideoSDKAudioRawData videoSDKAudioRawData = new ZMVideoSDKAudioRawData(rawDataStruct.buffer, rawDataStruct.bufferLen, rawDataStruct.sampleRate, rawDataStruct.channelNum);
        Marshal.FreeHGlobal(audioRawData);
        IOSZMVideoSDK.Instance.callBackObject.onOneWayAudioRawDataReceived(videoSDKAudioRawData, new ZMVideoSDKUser(new IOSZMVideoSDKUser(user)));
    }

    [AOT.MonoPInvokeCallback(typeof(fn_onUserShareStatusChanged))]
    static void onUserShareStatusChanged(IntPtr shareHelper, IntPtr user, ZMVideoSDKShareStatus status)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        ZMVideoSDKShareHelper helper = new ZMVideoSDKShareHelper(new IOSZMVideoSDKShareHelper(shareHelper));
        ZMVideoSDKUser iOSUser = new ZMVideoSDKUser(new IOSZMVideoSDKUser(user));
        IOSZMVideoSDK.Instance.callBackObject.onUserShareStatusChanged(helper, iOSUser, status);
    }

    [AOT.MonoPInvokeCallback(typeof(fn_onChatPrivilegeChanged))]
    static void onChatPrivilegeChanged(IntPtr chatHelper, ZMVideoSDKChatPrivilegeType privilege)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        ZMVideoSDKChatHelper helper = new ZMVideoSDKChatHelper(new IOSZMVideoSDKChatHelper(chatHelper));
        // not implemented yet in application layer
        //IOSZMVideoSDK.Instance.callBackObject.onChatPrivilegeChanged(helper, privilege);
    }

    private struct ZoomVideoSDKChatMessage_struct
    {
        public string messageID;
        public IntPtr senderUser;
        public IntPtr receiverUser;
        public string content;
        public long timeStamp;
        public bool isChatToAll;
        public bool isSelfSend;
    };

    [AOT.MonoPInvokeCallback(typeof(fn_onChatNewMessageNotify))]
    static void onChatNewMessageNotify(IntPtr chatHelper, IntPtr marshalledChatMessage)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        ZMVideoSDKChatHelper helper = new ZMVideoSDKChatHelper(new IOSZMVideoSDKChatHelper(chatHelper));
        ZoomVideoSDKChatMessage_struct unMarshalledChatMessage;
        unMarshalledChatMessage = (ZoomVideoSDKChatMessage_struct)Marshal.PtrToStructure(marshalledChatMessage, typeof(ZoomVideoSDKChatMessage_struct));
        ZMVideoSDKChatMessage chatMessage = new ZMVideoSDKChatMessage();
        chatMessage.msgId = unMarshalledChatMessage.messageID;
        chatMessage.content = unMarshalledChatMessage.content;
        chatMessage.timeStamp = unMarshalledChatMessage.timeStamp;
        chatMessage.isChatToAll = unMarshalledChatMessage.isChatToAll;
        chatMessage.isSelfSend = unMarshalledChatMessage.isSelfSend;
        chatMessage.senderUser = new ZMVideoSDKUser(new IOSZMVideoSDKUser(unMarshalledChatMessage.senderUser));
        chatMessage.receiverUser = new ZMVideoSDKUser(new IOSZMVideoSDKUser(unMarshalledChatMessage.receiverUser));
        Marshal.FreeHGlobal(marshalledChatMessage);
        IOSZMVideoSDK.Instance.callBackObject.onChatNewMessageNotify(helper, chatMessage);
    }

    [AOT.MonoPInvokeCallback(typeof(fn_onChatMsgDeleteNotification))]
    static void onChatMsgDeleteNotification(IntPtr chatHelper, string messageID, ZMVideoSDKChatMessageDeleteType deleteType)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        ZMVideoSDKChatHelper helper = new ZMVideoSDKChatHelper(new IOSZMVideoSDKChatHelper(chatHelper));
        IOSZMVideoSDK.Instance.callBackObject.onChatDeleteMessageNotify(helper, messageID, deleteType);
    }

    [AOT.MonoPInvokeCallback(typeof(fn_onCloudRecordingStatusChanged))]
    static void onCloudRecordingStatusChanged(ZMVideoSDKRecordingStatus recordingStatus, IntPtr recordingConsentHandler)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        ZMVideoSDKRecordingConsentHandler consentHandler = new ZMVideoSDKRecordingConsentHandler(new IOSZMVideoSDKRecordingConsentHandler(recordingConsentHandler));
        IOSZMVideoSDK.Instance.callBackObject.onCloudRecordingStatusChanged(recordingStatus, consentHandler);
    }

    [AOT.MonoPInvokeCallback(typeof(fn_ObjCWrapper))]
    static void onUserRecordAgreementNotification(IntPtr user)
    {
        if (IOSZMVideoSDK.Instance.callBackObject == null)
        {
            return;
        }
        ZMVideoSDKUser iOSUser = new ZMVideoSDKUser(new IOSZMVideoSDKUser(user));
        IOSZMVideoSDK.Instance.callBackObject.onUserRecordingConsentChanged(iOSUser);
    }
}
#endif
