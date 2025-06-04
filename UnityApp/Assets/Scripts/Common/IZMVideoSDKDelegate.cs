using System.Collections.Generic;

/**
    @brief A listener class that groups together the callbacks related to a session.
    See {@link ZoomVideoSDK#addListener(ZoomVideoSDKDelegate)}
 */
public interface IZMVideoSDKDelegate
{

    /**
        @brief Callback: Invoked when the current user joins the session.
    */
    void onSessionJoin(ZMVideoSDKUser myself);
    
    /**
        @brief Callback: Invoked when the current user leaves the session.
    */
    void onSessionLeave();
    
    /**
        @brief Callback: Invoked when errors occur.
        @param errorType Provides error code associated with the error. See {@link ZMVideoSDKErrors} for more information.
        @param details Detailed errorCode.
    */
    void onError(ZMVideoSDKErrors errorType, int details);
    
    /**
        @brief Callback: Invoked when a user joins the session.
        @param userHelper User help utility.
        @param userArray List of users who have just joined the session.
    */
    void onUserJoin(ZMVideoSDKUserHelper userHelper, List<ZMVideoSDKUser> userArray);
    
    /**
        @brief Triggered when other users leave session.
        @param userHelper The pointer of user helper object.
        @param userArray An array contains the users leaved.
    */
    void onUserLeave(ZMVideoSDKUserHelper userHelper, List<ZMVideoSDKUser> userArray);
    
    /**
        @brief Callback: Invoked when a user makes changes to their video, such as starting or stopping their video.
        @param videoHelper The pointer of video helper object.
        @param userArray The array contain user objoct.
    */
    void onUserVideoStatusChanged(ZMVideoSDKVideoHelper videoHelper, List<ZMVideoSDKUser> userArray);
    
    /**
        @brief Callback: Invoked when a user makes changes to their audio, such as muting or unmuting their audio.
        @param audioHelper The pointer of audio helper object.
        @param userArray The array contain user objoct.
    */
    void onUserAudioStatusChanged(ZMVideoSDKAudioHelper audioHelper, List<ZMVideoSDKUser> userArray);
    
    
    /**
        @brief Callback: Invoked when the session host changes.
        @param userHelper The pointer of user helper object.
        @param user The pointer of user object.
    */
    void onUserHostChanged(ZMVideoSDKUserHelper userHelper, ZMVideoSDKUser user);
    
    /**
        @brief Callback: Invoked when the active audio changes.
        @param audioHelper The pointer of audio helper object.
        @param userArray Active audio list.
    */
    void onUserActiveAudioChanged(ZMVideoSDKAudioHelper audioHelper, List<ZMVideoSDKUser> userArray);
    
    /**
        @brief Callback: Invoked when the manager of the session changes.
        @param user The pointer of user object.
    */
    void onUserManagerChanged(ZMVideoSDKUser user);
        
    /**
        @brief Callback: Invoked when a user changes their name.
        @param user The pointer of user object.
    */
    void onUserNameChanged(ZMVideoSDKUser user);

    /**
        @brief Callback: Invoked when a host requests you to unmute yourself.
    */
    void onHostAskUnmute();

    /**
        @brief Notify the current mic or speaker volume when testing.
        @param micVolume Specify the volume of the mic.
        @param speakerVolume Specify the volume of the speaker.
    */
    void onMicSpeakerVolumeChanged(uint micVolume, uint speakerVolume);

    /**
        @brief Notify the user that a mic/speaker device is selected when testing. Then the SDK will close the mic/speaker testing. The user shall restart the test manually if he still wants to test.
    */
    void onSelectedAudioDeviceChanged();

    /**
        @brief Callback: Invoked when mixed (all users) audio raw data received.
        @param audioRawData The Raw audio data object.
    */
    void onMixedAudioRawDataReceived(ZMVideoSDKAudioRawData audioRawData);

    /**
        @brief Callback: Invoked when individual user's audio raw data received.
        @param audioRawData The Raw audio data object.
        @param user The user object associated with the raw audio data.
    */
    void onOneWayAudioRawDataReceived(ZMVideoSDKAudioRawData audioRawData, ZMVideoSDKUser user);

    /**
        @brief Callback: Invoked when receiving a chat message.
        @param chatHelper chat helper util
        @param messageItem {@link ZMVideoSDKChatMessage}
     */
    void onChatNewMessageNotify(ZMVideoSDKChatHelper chatHelper, ZMVideoSDKChatMessage messageItem);

    /**
        @brief Callback: Invoked when a user deletes a chat message.
        @param chatHelper Chat helper utility.
        @param msgID      The deleted message's ID.
        @param deleteBy   Indicates by whom the message was deleted, which is defined in {@link ZMVideoSDKChatMessageDeleteType}.
     */
    void onChatDeleteMessageNotify(ZMVideoSDKChatHelper chatHelper, string msgID, ZMVideoSDKChatMessageDeleteType deleteBy);

    /**
     * Callback: Invoked when cloud recording status has paused, stopped, resumed, or otherwise changed.
     *
     * @param status  Cloud recording status defined in {@link ZMVideoSDKRecordingStatus}.
     * @param handler When the cloud recording starts, this object is used to let the user choose whether to accept or not.
     */
    void onCloudRecordingStatusChanged(ZMVideoSDKRecordingStatus status, ZMVideoSDKRecordingConsentHandler handler);


    /**
     * Invoked when a user consent to individual recording.
     *
     * @param user {@link ZMVideoSDKUser}
     */
    void onUserRecordingConsentChanged(ZMVideoSDKUser user);

    /**
        @brief Callback: Invoked when a user's share status changes.
        @param shareHelper Share helper utility.
        @param user      The user object.
        @param status    Indicates the current share status, which is defined in {@link ZMVideoSDKShareStatus}.
     */
    void onUserShareStatusChanged(ZMVideoSDKShareHelper shareHelper, ZMVideoSDKUser user, ZMVideoSDKShareStatus status);
}