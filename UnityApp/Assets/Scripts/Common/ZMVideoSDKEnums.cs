public enum ZMVideoSDKRawDataMemoryMode
{
    ZMVideoSDKRawDataMemoryMode_Stack,
    ZMVideoSDKRawDataMemoryMode_Heap,
}


public enum ZMVideoSDKErrors
{
    ZMVideoSDKErrors_Default = -1,
    ZMVideoSDKErrors_Success = 0,///<Success.
    ZMVideoSDKErrors_Wrong_Usage,
    ZMVideoSDKErrors_Internal_Error,
    ZMVideoSDKErrors_Uninitialize,
    ZMVideoSDKErrors_Memory_Error,
    ZMVideoSDKErrors_Load_Module_Error,
    ZMVideoSDKErrors_UnLoad_Module_Error,
    ZMVideoSDKErrors_Invalid_Parameter,
    ZMVideoSDKErrors_Call_Too_Frequently,
    ZMVideoSDKErrors_No_Impl,
    ZMVideoSDKErrors_Dont_Support_Feature,
    ZMVideoSDKErrors_Unknown,
    ZMVideoSDKErrors_Auth_Error = 1001,
    ZMVideoSDKErrors_Auth_Empty_Key_or_Secret,
    ZMVideoSDKErrors_Auth_Wrong_Key_or_Secret,
    ZMVideoSDKErrors_Auth_DoesNot_Support_SDK,
    ZMVideoSDKErrors_Auth_Disable_SDK,
    ZMVideoSDKErrors_JoinSession_NoSessionName = 1500,
    ZMVideoSDKErrors_JoinSession_NoSessionToken,
    ZMVideoSDKErrors_JoinSession_NoUserName,
    ZMVideoSDKErrors_JoinSession_Invalid_SessionName,
    ZMVideoSDKErrors_JoinSession_Invalid_Password,
    ZMVideoSDKErrors_JoinSession_Invalid_SessionToken,
    ZMVideoSDKErrors_JoinSession_SessionName_TooLong,
    ZMVideoSDKErrors_JoinSession_Token_MismatchedSessionName,
    ZMVideoSDKErrors_JoinSession_Token_NoSessionName,
    ZMVideoSDKErrors_JoinSession_Token_RoleType_EmptyOrWrong,
    ZMVideoSDKErrors_JoinSession_Token_UserIdentity_TooLong,
    ZMVideoSDKErrors_SessionModule_Not_Found = 2001,
    ZMVideoSDKErrors_SessionService_Invaild,
    ZMVideoSDKErrors_Session_Join_Failed,
    ZMVideoSDKErrors_Session_No_Rights,
    ZMVideoSDKErrors_Session_Already_In_Progress,
    ZMVideoSDKErrors_Session_Dont_Support_SessionType,
    ZMVideoSDKErrors_Session_Reconnecting,
    ZMVideoSDKErrors_Session_Disconnecting,
    ZMVideoSDKErrors_Session_Not_Started,
    ZMVideoSDKErrors_Session_Need_Password,
    ZMVideoSDKErrors_Session_Password_Wrong,
    ZMVideoSDKErrors_Session_Remote_DB_Error,
    ZMVideoSDKErrors_Session_Invalid_Param,
    ZMVideoSDKErrors_Session_Client_Incompatible,
    ZMVideoSDKErrors_Session_Audio_Error = 3000,
    ZMVideoSDKErrors_Session_Audio_No_Microphone,
    ZMVideoSDKErrors_Session_Audio_No_Speaker,
    ZMVideoSDKErrors_Session_Video_Error = 4000,
    ZMVideoSDKErrors_Session_Video_Device_Error,
    ZMVideoSDKErrors_Session_Live_Stream_Error = 5000,
    ZMVideoSDKErrors_Session_Phone_Error = 5500,

    ZMVideoSDKErrors_RAWDATA_MALLOC_FAILED = 6001,
    ZMVideoSDKErrors_RAWDATA_NOT_IN_Session,
    ZMVideoSDKErrors_RAWDATA_NO_LICENSE,
    ZMVideoSDKErrors_RAWDATA_VIDEO_MODULE_NOT_READY,
    ZMVideoSDKErrors_RAWDATA_VIDEO_MODULE_ERROR,
    ZMVideoSDKErrors_RAWDATA_VIDEO_DEVICE_ERROR,
    ZMVideoSDKErrors_RAWDATA_NO_VIDEO_DATA,
    ZMVideoSDKErrors_RAWDATA_SHARE_MODULE_NOT_READY,
    ZMVideoSDKErrors_RAWDATA_SHARE_MODULE_ERROR,
    ZMVideoSDKErrors_RAWDATA_NO_SHARE_DATA,
    ZMVideoSDKErrors_RAWDATA_AUDIO_MODULE_NOT_READY,
    ZMVideoSDKErrors_RAWDATA_AUDIO_MODULE_ERROR,
    ZMVideoSDKErrors_RAWDATA_NO_AUDIO_DATA,
    ZMVideoSDKErrors_RAWDATA_PREPROCESS_RAWDATA_ERROR,
    ZMVideoSDKErrors_RAWDATA_NO_DEVICE_RUNNING,

    ZMVideoSDKErrors_RAWDATA_INIT_DEVICE,
    ZMVideoSDKErrors_RAWDATA_VIRTUAL_DEVICE,
    ZMVideoSDKErrors_RAWDATA_CANNOT_CHANGE_VIRTUAL_DEVICE_IN_PREVIEW,
    ZMVideoSDKErrors_RAWDATA_INTERNAL_ERROR,
    ZMVideoSDKErrors_RAWDATA_SEND_TOO_MUCH_DATA_IN_SINGLE_TIME,
    ZMVideoSDKErrors_RAWDATA_SEND_TOO_FREQUENTLY,
    ZMVideoSDKErrors_RAWDATA_VIRTUAL_MIC_IS_TERMINATE,

    ZMVideoSDKErrors_Session_Share_Error = 7001,
    ZMVideoSDKErrors_Session_Share_Module_Not_Ready,
    ZMVideoSDKErrors_Session_Share_You_Are_Not_Sharing,
    ZMVideoSDKErrors_Session_Share_Type_Is_Not_Support,
    ZMVideoSDKErrors_Session_Share_Internal_Error,
    ZMVideoSDKErrors_Dont_Support_Multi_Stream_Video_User,
    ZMVideoSDKErrors_Fail_Assign_User_Privilege,
    ZMVideoSDKErrors_No_Recording_In_Process,
    ZMVideoSDKErrors_Set_Virtual_Background_Fail,

    ZMVideoSDKErrors_Permission_RECORD_AUDIO = 9001,
    ZMVideoSDKErrors_Permission_READ_PHONE_STATE = 9002,
    ZMVideoSDKErrors_Permission_BLUETOOTH_CONNECT = 9003,
}

public enum ZMVideoSDKRawDataType
{
    //Raw data type is video.
    ZMVideoSDKRawDataType_Video = 0,
    //Raw data type is share.
    ZMVideoSDKRawDataType_Share,
}

public enum ZMVideoSDKResolution
{
    //The resolution is 90p.
    ZMVideoSDKResolution_90P = 0,
    //The resolution is 180p.
    ZMVideoSDKResolution_180P,
    //The resolution is 360p.
    ZMVideoSDKResolution_360P,
    //The resolution is 720p.
    ZMVideoSDKResolution_720P,
    //The resolution is 1080p.
    ZMVideoSDKResolution_1080P,
    //The resolution no used.
    ZMVideoSDKResolution_NoUse = 100,
}

public enum ZMVideoSDKRawDataStatus
{
    //The raw data status is on.
    ZMVideoSDKRawData_On,
    //The raw data status if off.
    ZMVideoSDKRawData_Off,
}

public enum ZMVideoSDKNetworkStatus
{
    ZMVideoSDKNetworkStatus_None,
    ZMVideoSDKNetworkStatus_Bad,
    ZMVideoSDKNetworkStatus_Normal,
    ZMVideoSDKNetworkStatus_Good,
}

public enum ZMVideoSDKAudioType
{
    //Audio type is voip.
    ZMVideoSDKAudioType_VOIP,
    //Audio type is telephony.
    ZMVideoSDKAudioType_TELEPHONY,
    //Audio type is none.
    ZMVideoSDKAudioType_None,
}

public enum ZMVideoRotation
{
    //Video rotation is 0.
    ZMVideoRotation_0,
    //Video rotation is 90.
    ZMVideoRotation_90,
    //Video rotation is 180.
    ZMVideoRotation_180,
    //Video rotation is 270.
    ZMVideoRotation_270,
}

public enum ZMVideoSDKVideoPreferenceMode
{
    //Balance mode.
    ZMVideoSDKVideoPreferenceMode_Balance,
    //Sharpness mode.
    ZMVideoSDKVideoPreferenceMode_Sharpness,
    //Smoothness mode.
    ZMVideoSDKVideoPreferenceMode_Smoothness,
    //Custom mode.
    ZMVideoSDKVideoPreferenceMode_Custom,
}

public enum ZMVideoSDKShareStatus
{
    //No view or screen share available.
    ZMVideoSDKShareStatus_None,
    //User started sharing.
    ZMVideoSDKShareStatus_Start,
    //User paused sharing.
    ZMVideoSDKShareStatus_Pause,
    //User resumed sharing.
    ZMVideoSDKShareStatus_Resume,
    //User stopped sharing.
    ZMVideoSDKShareStatus_Stop,
}

public enum ZMVideoSDKShareType
{
    ZoomVideoSDKShareType_None,
    ZoomVideoSDKShareType_Normal,//application or desktop share
    ZoomVideoSDKShareType_PureAudio, //pure computer audio share
}

/**
 * @brief Enumerations of the type for chat privilege.
 */
public enum ZMVideoSDKChatPrivilegeType
{
    Unknown,
    PubliclyAndPrivately,
    NoOne,
    Publicly,
}

/**
 * @brief The chat message delete type are sent in the IZMVideoSDKDelegate#onChatDeleteMessageNotify callback.
 */
public enum ZMVideoSDKChatMessageDeleteType
{
    /**
     * None.
     */
    ByNone,

    /**
     * Indicates that the message was deleted by myself.
     */
    BySelf,

    /**
     * Indicates that the message was deleted by the session host.
     */
    ByHost,

    /**
     * Indicates that the message was deleted by Data Loss Prevention (dlp).
     * This happens when the message goes against the host organization's compliance policies.
     */
    ByDLP
}

/**
 * An enum representing the status of the recording status.
 */
public enum ZMVideoSDKRecordingStatus
{
    /**
     * The recording has successfully started or successfully resumed.
     */
    Start,

    /**
     * The recording has stopped.
     */
    Stop,

    /**
     * Recording is unsuccessful due to insufficient storage space.
     * Please try to:
     * <ul>
     *     <li>Free up storage space</li>
     *     <li>Purchase additional storage space</li>
     * </ul>
     */
    DiskFull,

    /**
     * The recording has paused.
     */
    Pause
}

public enum ZMVideoSDKRecordingConsentType
{
    /**
     * Invalid type
     */
    Invalid,
    /**
     * In this case, 'accept' means agree to be recorded to gallery and speaker mode, 'decline' means leave session.
     */
    Traditional,
    /**
     * In this case, 'accept' means agree to be recorded to a separate file, 'decline' means stay in session and can't be recorded.
     */
    Individual,
}