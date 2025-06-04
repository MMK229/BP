#if UNITY_STANDALONE_OSX
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MacZMVideoSDK
{
    private static readonly MacZMVideoSDK instance = new MacZMVideoSDK();
    private static IZMVideoSDKDelegate callBackObject = null;  
    private static ZMVideoSDKSession currentSession = null;
    static MacZMVideoSDK()    
    {    
    }    
    private MacZMVideoSDK()    
    {
        initializeDelegates();
        setCallbacks = false;
    }    
    public static MacZMVideoSDK Instance    
    {    
        get    
        {    
            return instance;    
        }    
    }

    [StructLayout(LayoutKind.Sequential,CharSet = CharSet.Ansi)]
    public struct ZMVideoSDKInitParams_Struct
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string domain;
        [MarshalAs(UnmanagedType.LPStr)]
        public string logFilePrefix;
        [MarshalAs(UnmanagedType.LPStr)]
        public string teamIdentifier;
        [MarshalAs(UnmanagedType.LPStr)]
        public string speakerTestFilePath;
        [MarshalAs(UnmanagedType.I1)]
        public bool enableLog;
        public int audioRawDataMemoryMode;
        public int videoRawDataMemoryMode;
        public int shareRawDataMemoryMode;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors initialize_c(IntPtr pMacZMVideoSDKInitParams);
    public void Initialize(ZMVideoSDKInitParams zmVideoSDKInitParams, Action<ZMVideoSDKErrors> callback)
    {
        ZMVideoSDKErrors error;
        ZMVideoSDKInitParams_Struct zmVideoSDKInitParamsStruct = new ZMVideoSDKInitParams_Struct();
        zmVideoSDKInitParamsStruct.domain = zmVideoSDKInitParams.domain;
        zmVideoSDKInitParamsStruct.logFilePrefix = zmVideoSDKInitParams.logFilePrefix;
        zmVideoSDKInitParamsStruct.enableLog = zmVideoSDKInitParams.enableLog;
        zmVideoSDKInitParamsStruct.teamIdentifier = zmVideoSDKInitParams.teamIdentifier;
        zmVideoSDKInitParamsStruct.speakerTestFilePath = zmVideoSDKInitParams.extendParams.speakerTestFilePath;
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

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void cleanUp_c();
    public void Cleanup(Action<ZMVideoSDKErrors> callback)
    {
        cleanUp_c();
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr joinSession_c(IntPtr pMacZMVideoSDKSessionContext);
    public ZMVideoSDKSession JoinSession(ZMVideoSDKSessionContext zmVideoSDKSessionContext, Action<ZMVideoSDKErrors> callback)
    {
        IntPtr pCStructure = Marshal.AllocHGlobal(Marshal.SizeOf(zmVideoSDKSessionContext));
        try
        {
            Marshal.StructureToPtr(zmVideoSDKSessionContext, pCStructure, false);
            IntPtr pObjCSession = joinSession_c(pCStructure);
            if (pObjCSession != IntPtr.Zero)
            {
                currentSession = new ZMVideoSDKSession(new MacZMVideoSDKSession(pObjCSession));
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

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors leaveSession_c(bool end);
    public ZMVideoSDKErrors LeaveSession(bool end, Action<ZMVideoSDKErrors> callback)
    {
        return leaveSession_c(end);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr getSessionInfo_c();
    public ZMVideoSDKSession GetSessionInfo()
    {
        IntPtr pObjCSession = getSessionInfo_c();
        if (pObjCSession == null)
        {
            return null;
        }
        return new ZMVideoSDKSession(new MacZMVideoSDKSession(pObjCSession));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool isInSession_c();
    public bool IsInSession()
    {
        return isInSession_c();
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern string getSDKVersion_c();
    public string GetSdkVersion()
    {
        return getSDKVersion_c();
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr getAudioHelper_c();
    public ZMVideoSDKAudioHelper GetAudioHelper()
    {
        IntPtr objC_ZMVideoSDKAudioHelper = getAudioHelper_c();
        return new ZMVideoSDKAudioHelper(new MacZMVideoSDKAudioHelper(objC_ZMVideoSDKAudioHelper));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr getVideoHelper_c();
    public ZMVideoSDKVideoHelper GetVideoHelper()
    {
        IntPtr objC_ZMVideoSDKVideoHelper = getVideoHelper_c();
        return new ZMVideoSDKVideoHelper(new MacZMVideoSDKVideoHelper(objC_ZMVideoSDKVideoHelper));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr getUserHelper_c();
    public ZMVideoSDKUserHelper GetUserHelper()
    {
        IntPtr objC_ZMVideoSDKUserHelper = getUserHelper_c();
        return new ZMVideoSDKUserHelper(new MacZMVideoSDKUserHelper(objC_ZMVideoSDKUserHelper));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr getChatHelper_c();
    public ZMVideoSDKChatHelper GetChatHelper(){
        IntPtr objC_ZMVideoSDKChatHelper = getChatHelper_c();
        return new ZMVideoSDKChatHelper(new MacZMVideoSDKChatHelper(objC_ZMVideoSDKChatHelper));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr getRecordingHelper_c();
    public ZMVideoSDKRecordingHelper GetRecordingHelper(){
        IntPtr objC_ZMVideoSDKRecordingHelper = getRecordingHelper_c();
        return new ZMVideoSDKRecordingHelper(new MacZMVideoSDKRecordingHelper(objC_ZMVideoSDKRecordingHelper));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr getShareHelper_c();
    public ZMVideoSDKShareHelper GetShareHelper(){
        IntPtr objC_ZMVideoSDKShareHelper = getShareHelper_c();
        return new ZMVideoSDKShareHelper(new MacZMVideoSDKShareHelper(objC_ZMVideoSDKShareHelper));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr getAudioSettingHelper();
    // todo

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern IntPtr getVideoSettingHelper();
    // todo

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onSessionJoin_c(IntPtr func);
        
    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onSessionLeave_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onError_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onUserJoin_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onUserLeave_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onUserVideoStatusChanged_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onUserAudioStatusChanged_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onUserHostChanged_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onUserActiveAudioChanged_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onUserManagerChanged_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onUserNameChanged_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onHostAskUnmute_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onMicSpeakerVolumeChanged_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onSelectedAudioDeviceChanged_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onMixedAudioRawDataReceived_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onOneWayAudioRawDataReceived_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onUserShareStatusChanged_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onChatPrivilegeChanged_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onChatNewMessageNotify_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onChatMsgDeleteNotification_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onCloudRecordingStatusChanged_c(IntPtr func);

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern void onUserRecordAgreementNotification_c(IntPtr func);

    delegate void fn_NoParams();
    delegate void fn_Error_int(ZMVideoSDKErrors errorType, int details);
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

    fn_NoParams _onSessionJoin;
    fn_NoParams _onSessionLeave;
    fn_NoParams _onHostAskUnmute;
    fn_NoParams _onSelectedAudioDeviceChanged;
    fn_Error_int _onError;
    fn_uint_uint _onMicSpeakerVolumeChanged;
    fn_ObjCWrapper _onUserManagerChanged;
    fn_ObjCWrapper _onUserNameChanged;
    fn_2ObjCWrappers_int _onUserJoin;
    fn_2ObjCWrappers_int _onUserLeave;
    fn_2ObjCWrappers_int _onUserVideoStatusChanged;
    fn_2ObjCWrappers_int _onUserAudioStatusChanged;
    fn_2ObjCWrappers _onUserHostChanged;
    fn_2ObjCWrappers_int _onUserActiveAudioChanged;
    fn_CPointer _onMixedAudioRawDataReceived;
    fn_CPointer_ObjCWrapper _onOneWayAudioRawDataReceived;
    fn_onUserShareStatusChanged _onUserShareStatusChanged;
    fn_onChatPrivilegeChanged _onChatPrivilegeChanged;
    fn_onChatNewMessageNotify _onChatNewMessageNotify;
    fn_onChatMsgDeleteNotification _onChatMsgDeleteNotification;
    fn_onCloudRecordingStatusChanged _onCloudRecordingStatusChanged;
    fn_ObjCWrapper _onUserRecordAgreementNotification;
    bool setCallbacks;

    private void initializeDelegates()
    {
        _onSessionJoin = onSessionJoin;
        _onSessionLeave = onSessionLeave;
        _onHostAskUnmute = onHostAskUnmute;
        _onSelectedAudioDeviceChanged = onSelectedAudioDeviceChanged;
        _onError = onError;
        _onMicSpeakerVolumeChanged = onMicSpeakerVolumeChanged;
        _onUserManagerChanged = onUserManagerChanged;
        _onUserNameChanged = onUserNameChanged;
        _onUserJoin = onUserJoin;
        _onUserLeave = onUserLeave;
        _onUserVideoStatusChanged = onUserVideoStatusChanged;
        _onUserAudioStatusChanged = onUserAudioStatusChanged;
        _onUserHostChanged = onUserHostChanged;
        _onUserActiveAudioChanged = onUserActiveAudioChanged;
        _onMixedAudioRawDataReceived = onMixedAudioRawDataReceived;
        _onOneWayAudioRawDataReceived = onOneWayAudioRawDataReceived;
        _onUserShareStatusChanged = onUserShareStatusChanged;
        _onChatPrivilegeChanged = onChatPrivilegeChanged;
        _onChatNewMessageNotify = onChatNewMessageNotify;
        _onChatMsgDeleteNotification = onChatMsgDeleteNotification;
        _onCloudRecordingStatusChanged = onCloudRecordingStatusChanged;
        _onUserRecordAgreementNotification = onUserRecordAgreementNotification;
        onSessionJoin_c(Marshal.GetFunctionPointerForDelegate(_onSessionJoin));
        onSessionLeave_c(Marshal.GetFunctionPointerForDelegate(_onSessionLeave));
        onHostAskUnmute_c(Marshal.GetFunctionPointerForDelegate(_onHostAskUnmute));
        onSelectedAudioDeviceChanged_c(Marshal.GetFunctionPointerForDelegate(_onSelectedAudioDeviceChanged));
        onError_c(Marshal.GetFunctionPointerForDelegate(_onError));
        onMicSpeakerVolumeChanged_c(Marshal.GetFunctionPointerForDelegate(_onMicSpeakerVolumeChanged));
        onUserManagerChanged_c(Marshal.GetFunctionPointerForDelegate(_onUserManagerChanged));
        onUserNameChanged_c(Marshal.GetFunctionPointerForDelegate(_onUserNameChanged));
        onUserJoin_c(Marshal.GetFunctionPointerForDelegate(_onUserJoin));
        onUserLeave_c(Marshal.GetFunctionPointerForDelegate(_onUserLeave));
        onUserVideoStatusChanged_c(Marshal.GetFunctionPointerForDelegate(_onUserVideoStatusChanged));
        onUserAudioStatusChanged_c(Marshal.GetFunctionPointerForDelegate(_onUserAudioStatusChanged));
        onUserHostChanged_c(Marshal.GetFunctionPointerForDelegate(_onUserHostChanged));
        onUserActiveAudioChanged_c(Marshal.GetFunctionPointerForDelegate(_onUserActiveAudioChanged));
        onMixedAudioRawDataReceived_c(Marshal.GetFunctionPointerForDelegate(_onMixedAudioRawDataReceived));
        onOneWayAudioRawDataReceived_c(Marshal.GetFunctionPointerForDelegate(_onOneWayAudioRawDataReceived));
        onUserShareStatusChanged_c(Marshal.GetFunctionPointerForDelegate(_onUserShareStatusChanged));
        onChatPrivilegeChanged_c(Marshal.GetFunctionPointerForDelegate(_onChatPrivilegeChanged));
        onChatNewMessageNotify_c(Marshal.GetFunctionPointerForDelegate(_onChatNewMessageNotify));
        onChatMsgDeleteNotification_c(Marshal.GetFunctionPointerForDelegate(_onChatMsgDeleteNotification));
        onCloudRecordingStatusChanged_c(Marshal.GetFunctionPointerForDelegate(_onCloudRecordingStatusChanged));
        onUserRecordAgreementNotification_c(Marshal.GetFunctionPointerForDelegate(_onUserRecordAgreementNotification));
    }

    [DllImport ("ZMMacUnityVideoSDK")]
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

    public static void onSessionJoin()
    {
        if (callBackObject != null)
        {
            callBackObject.onSessionJoin(currentSession.GetMySelf());
        }
    }

    public static void onSessionLeave()
    {
        if (callBackObject != null)
        {
            currentSession = null;
            callBackObject.onSessionLeave();
        }
    }

    public static void onError(ZMVideoSDKErrors errorType, int details)
    {
        if (callBackObject != null)
        {
            callBackObject.onError(errorType, details);
        }
    }
    
    public static List<ZMVideoSDKUser> GetUserList(IntPtr pUsers, int elementCount)
    {
        List<ZMVideoSDKUser> userList = new List<ZMVideoSDKUser>();
        int elementSize = Marshal.SizeOf(typeof(IntPtr));
        for (int i = 0; i < elementCount; i++)
        {
            IntPtr objC_ZMVideoSDKUser = Marshal.ReadIntPtr(pUsers, i * elementSize);
            MacZMVideoSDKUser user = new MacZMVideoSDKUser(objC_ZMVideoSDKUser);
            userList.Add(new ZMVideoSDKUser(user));
        }
        Marshal.FreeHGlobal(pUsers);
        return userList;
    }
    public static void onUserJoin(IntPtr userHelper, IntPtr pUsers, int elementCount)
    {
        if (callBackObject == null)
        {
            return;
        }
        
        List<ZMVideoSDKUser> userList = GetUserList(pUsers, elementCount);
        ZMVideoSDKUserHelper helper = new ZMVideoSDKUserHelper(new MacZMVideoSDKUserHelper(userHelper));
        callBackObject.onUserJoin(helper, userList);
    }

    public static void onUserLeave(IntPtr userHelper, IntPtr pUsers, int elementCount)
    {
        if (callBackObject == null)
        {
            return;
        }
        ZMVideoSDKUserHelper helper = new ZMVideoSDKUserHelper(new MacZMVideoSDKUserHelper(userHelper));
        List<ZMVideoSDKUser> userList = GetUserList(pUsers, elementCount);
        callBackObject.onUserLeave(helper, userList);
    }

    public static void onUserVideoStatusChanged(IntPtr videoHelper, IntPtr pUsers, int elementCount)
    {
        if (callBackObject == null)
        {
            return;
        }
        ZMVideoSDKVideoHelper helper = new ZMVideoSDKVideoHelper(new MacZMVideoSDKVideoHelper(videoHelper));
        List<ZMVideoSDKUser> userList = GetUserList(pUsers, elementCount);
        callBackObject.onUserVideoStatusChanged(helper, userList);
    }

    public static void onUserAudioStatusChanged(IntPtr audioHelper, IntPtr pUsers, int elementCount)
    {
        if (callBackObject == null)
        {
            return;
        }
        ZMVideoSDKAudioHelper helper = new ZMVideoSDKAudioHelper(new MacZMVideoSDKAudioHelper(audioHelper));
        List<ZMVideoSDKUser> userList = GetUserList(pUsers, elementCount);
        callBackObject.onUserAudioStatusChanged(helper, userList);
    }

    public static void onUserHostChanged(IntPtr userHelper, IntPtr user)
    {
        if (callBackObject == null)
        {
            return;
        }
        callBackObject.onUserHostChanged(new ZMVideoSDKUserHelper(new MacZMVideoSDKUserHelper(userHelper)), new ZMVideoSDKUser(new MacZMVideoSDKUser(user)));
    }

    public static void onUserActiveAudioChanged(IntPtr audioHelper, IntPtr pUsers, int elementCount)
    {
        if (callBackObject == null)
        {
            return;
        }
        ZMVideoSDKAudioHelper helper = new ZMVideoSDKAudioHelper(new MacZMVideoSDKAudioHelper(audioHelper));
        List<ZMVideoSDKUser> userList = GetUserList(pUsers, elementCount);
        callBackObject.onUserActiveAudioChanged(helper, userList);
    }

    public static void onUserManagerChanged(IntPtr user)
    {
        if (callBackObject == null)
        {
            return;
        }
        callBackObject.onUserManagerChanged(new ZMVideoSDKUser(new MacZMVideoSDKUser(user)));
    }

    public static void onUserNameChanged(IntPtr user)
    {
        if (callBackObject == null)
        {
            return;
        }
        callBackObject.onUserNameChanged(new ZMVideoSDKUser(new MacZMVideoSDKUser(user)));
    }

    public static void onHostAskUnmute()
    {
        if (callBackObject != null)
        {
            callBackObject.onHostAskUnmute();
        }
    }

    public static void onMicSpeakerVolumeChanged(uint micVolume, uint speakerVolume)
    {
        if (callBackObject != null)
        {
            callBackObject.onMicSpeakerVolumeChanged(micVolume, speakerVolume);
        }
    }

    public static void onSelectedAudioDeviceChanged()
    {
        if (callBackObject != null)
        {
            callBackObject.onSelectedAudioDeviceChanged();
        }
    }

    public struct ZMVideoSDKAudioRawData_struct
    {
        public IntPtr buffer;
        public int bufferLen;
        public int sampleRate;
        public int channelNum;
    };

    public static void onMixedAudioRawDataReceived(IntPtr audioRawData)
    {
        if (callBackObject == null)
        {
            return;
        }
        ZMVideoSDKAudioRawData_struct rawDataStruct;
        rawDataStruct = (ZMVideoSDKAudioRawData_struct)Marshal.PtrToStructure(audioRawData, typeof(ZMVideoSDKAudioRawData_struct));
        ZMVideoSDKAudioRawData videoSDKAudioRawData = new ZMVideoSDKAudioRawData(rawDataStruct.buffer, rawDataStruct.bufferLen, rawDataStruct.sampleRate, rawDataStruct.channelNum);
        Marshal.FreeHGlobal(audioRawData);
        callBackObject.onMixedAudioRawDataReceived(videoSDKAudioRawData);
    }

    public static void onOneWayAudioRawDataReceived(IntPtr audioRawData, IntPtr user)
    {
        if (callBackObject == null)
        {
            return;
        }
        ZMVideoSDKAudioRawData_struct rawDataStruct;
        rawDataStruct = (ZMVideoSDKAudioRawData_struct)Marshal.PtrToStructure(audioRawData, typeof(ZMVideoSDKAudioRawData_struct));
        ZMVideoSDKAudioRawData videoSDKAudioRawData = new ZMVideoSDKAudioRawData(rawDataStruct.buffer, rawDataStruct.bufferLen, rawDataStruct.sampleRate, rawDataStruct.channelNum);
        Marshal.FreeHGlobal(audioRawData);
        callBackObject.onOneWayAudioRawDataReceived(videoSDKAudioRawData, new ZMVideoSDKUser(new MacZMVideoSDKUser(user)));
    }

    public static void onUserShareStatusChanged(IntPtr shareHelper, IntPtr user, ZMVideoSDKShareStatus status)
    {
        if (callBackObject == null)
        {
            return;
        }
        ZMVideoSDKShareHelper helper = new ZMVideoSDKShareHelper(new MacZMVideoSDKShareHelper(shareHelper));
        ZMVideoSDKUser macOSUser = new ZMVideoSDKUser(new MacZMVideoSDKUser(user));
        callBackObject.onUserShareStatusChanged(helper, macOSUser, status);
    }

    public static void onChatPrivilegeChanged(IntPtr chatHelper, ZMVideoSDKChatPrivilegeType privilege)
    {
        if (callBackObject == null)
        {
            return;
        }
        ZMVideoSDKChatHelper helper = new ZMVideoSDKChatHelper(new MacZMVideoSDKChatHelper(chatHelper));
        // not implemented yet in application layer
        //callBackObject.onChatPrivilegeChanged(helper, privilege);
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

    public static void onChatNewMessageNotify(IntPtr chatHelper, IntPtr marshalledChatMessage)
    {
        if (callBackObject == null)
        {
            return;
        }
        ZMVideoSDKChatHelper helper = new ZMVideoSDKChatHelper(new MacZMVideoSDKChatHelper(chatHelper));
        ZoomVideoSDKChatMessage_struct unMarshalledChatMessage;
        unMarshalledChatMessage = (ZoomVideoSDKChatMessage_struct)Marshal.PtrToStructure(marshalledChatMessage, typeof(ZoomVideoSDKChatMessage_struct));
        ZMVideoSDKChatMessage chatMessage = new ZMVideoSDKChatMessage();
        chatMessage.msgId = unMarshalledChatMessage.messageID;
        chatMessage.content = unMarshalledChatMessage.content;
        chatMessage.timeStamp = unMarshalledChatMessage.timeStamp;
        chatMessage.isChatToAll = unMarshalledChatMessage.isChatToAll;
        chatMessage.isSelfSend = unMarshalledChatMessage.isSelfSend;
        chatMessage.senderUser = new ZMVideoSDKUser(new MacZMVideoSDKUser(unMarshalledChatMessage.senderUser));
        chatMessage.receiverUser = new ZMVideoSDKUser(new MacZMVideoSDKUser(unMarshalledChatMessage.receiverUser));
        Marshal.FreeHGlobal(marshalledChatMessage);
        callBackObject.onChatNewMessageNotify(helper, chatMessage);
    }

    public static void onChatMsgDeleteNotification(IntPtr chatHelper, string messageID, ZMVideoSDKChatMessageDeleteType deleteType)
    {
        if (callBackObject == null)
        {
            return;
        }
        ZMVideoSDKChatHelper helper = new ZMVideoSDKChatHelper(new MacZMVideoSDKChatHelper(chatHelper));
        callBackObject.onChatDeleteMessageNotify(helper, messageID, deleteType);
    }

    public static void onCloudRecordingStatusChanged(ZMVideoSDKRecordingStatus recordingStatus, IntPtr recordingConsentHandler)
    {
        if (callBackObject == null)
        {
            return;
        }
        ZMVideoSDKRecordingConsentHandler consentHandler = new ZMVideoSDKRecordingConsentHandler(new MacZMVideoSDKRecordingConsentHandler(recordingConsentHandler));
        callBackObject.onCloudRecordingStatusChanged(recordingStatus, consentHandler);
    }

    public static void onUserRecordAgreementNotification(IntPtr user)
    {
        if (callBackObject == null)
        {
            return;
        }
        callBackObject.onUserRecordingConsentChanged(new ZMVideoSDKUser(new MacZMVideoSDKUser(user)));
    }
}
#endif
