using System;
/**
 * Chat interface.
 * <p>
 * See {@link ZoomVideoSDK#getChatHelper()}
 */
public class ZMVideoSDKChatHelper
{
#if UNITY_STANDALONE_OSX
    private MacZMVideoSDKChatHelper _videoSDKChatHelper = null;
    public ZMVideoSDKChatHelper(MacZMVideoSDKChatHelper videoSDKChatHelper)
    {
        _videoSDKChatHelper = videoSDKChatHelper;
    }
#elif UNITY_ANDROID
    private AndroidZoomVideoSDKChatHelper _videoSDKChatHelper = null;
    public ZMVideoSDKChatHelper(AndroidZoomVideoSDKChatHelper videoSDKChatHelper)
    {
        _videoSDKChatHelper = videoSDKChatHelper;
    }
#elif UNITY_STANDALONE_WIN
    private WindowsZoomVideoSDKChatHelper _videoSDKChatHelper = null;
    public ZMVideoSDKChatHelper(WindowsZoomVideoSDKChatHelper videoSDKChatHelper)
    {
        _videoSDKChatHelper = videoSDKChatHelper;
    }
#elif UNITY_IOS
    private IOSZMVideoSDKChatHelper _videoSDKChatHelper = null;
    public ZMVideoSDKChatHelper(IOSZMVideoSDKChatHelper videoSDKChatHelper)
    {
        _videoSDKChatHelper = videoSDKChatHelper;
    }
#endif
    /**
      * Determine whether chat is disabled.
      *
      * @return true if chat is disabled, otherwise false.
      */
    public bool IsChatDisabled()
    {
        return _videoSDKChatHelper.IsChatDisabled();
    }

    /**
     * Determine whether private chat is disabled.
     *
     * @return true if private chat is disabled, otherwise false.
     */
    public bool IsPrivateChatDisabled()
    {
        return _videoSDKChatHelper.IsPrivateChatDisabled();
    }

    /**
     * Call this method to send a chat message to a specific user.
     *
     * @param user the receiver
     * @param msgContent the message content
     * @return {@link ZoomVideoSDKErrors}
     */
    public void SendChatToUser(ZMVideoSDKUser user, string msgContent, Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKChatHelper.SendChatToUser(user, msgContent, callback);
    }

    /**
     * Call this method to send a chat message to all users.
     *
     * @param msgContent the message content
     * @return {@link ZoomVideoSDKErrors}
     */
    public void SendChatToAll(string msgContent, Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKChatHelper.SendChatToAll(msgContent, callback);
    }

    /**
     * Call this method to delete a specific chat message from the Zoom server. 
     * This does not delete the message in your user interface.
     *
     * @param msgId the message Id
     * @return {@link ZoomVideoSDKErrors}
     */
    public void DeleteChatMessage(string msgId, Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKChatHelper.DeleteChatMessage(msgId, callback);
    }

    /**
     * Determine if a specific message can be deleted.
     *
     * @param msgId the message Id
     * @return true if the message can be deleted, otherwise False.
     */
    public bool CanChatMessageBeDeleted(string msgId)
    {
        return _videoSDKChatHelper.CanChatMessageBeDeleted(msgId);
    }

    /**
     * Set participant Chat Privilege when in session
     * Note: Only session host/manager can run the function
     *
     * @param privilege The chat privilege of the participant
     * @return If the function succeeds, it will return Errors_Success, otherwise failed.
     */
    public ZMVideoSDKErrors ChangeChatPrivilege(ZMVideoSDKChatPrivilegeType privilege)
    {
        return _videoSDKChatHelper.ChangeChatPrivilege(privilege);
    }

    /**
     * get participant Chat Priviledge when in session
     * @return the result of participant chat priviledge.
     */
    public ZMVideoSDKChatPrivilegeType GetChatPrivilege()
    {
        return _videoSDKChatHelper.GetChatPrivilege();
    }

}