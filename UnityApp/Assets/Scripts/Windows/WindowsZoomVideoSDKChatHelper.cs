#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

public class WindowsZoomVideoSDKChatHelper
{
    private IntPtr chatHelper;

    public WindowsZoomVideoSDKChatHelper(IntPtr chatHelper)
	{
        this.chatHelper = chatHelper;
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool isChatDisabled_c(IntPtr helper);
    public bool IsChatDisabled()
    {
        return isChatDisabled_c(chatHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool isPrivateChatDisabled_c(IntPtr helper);
    public bool IsPrivateChatDisabled()
    {
        return isPrivateChatDisabled_c(chatHelper);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors sendChatToUser_c(IntPtr helper, IntPtr user, string msgContent);
    public void SendChatToUser(ZMVideoSDKUser user, string msgContent, Action<ZMVideoSDKErrors> callback)
    {
        callback(sendChatToUser_c(chatHelper, user.VideoSDKUser.Instance(), msgContent));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors sendChatToAll_c(IntPtr helper, string msgContent);
    public void SendChatToAll(string msgContent, Action<ZMVideoSDKErrors> callback)
    {
        callback(sendChatToAll_c(chatHelper, msgContent));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors deleteChatMessage_c(IntPtr helper, string msgID);
    public void DeleteChatMessage(string msgId, Action<ZMVideoSDKErrors> callback)
    {
        callback(deleteChatMessage_c(chatHelper, msgId));
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern bool canChatMessageBeDeleted_c(IntPtr helper, string msgID);
    public bool CanChatMessageBeDeleted(string msgId)
    {
        return canChatMessageBeDeleted_c(chatHelper, msgId);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKErrors changeChatPrivilege_c(IntPtr helper, ZMVideoSDKChatPrivilegeType privilege); 
    public ZMVideoSDKErrors ChangeChatPrivilege(ZMVideoSDKChatPrivilegeType privilege)
    {
        return changeChatPrivilege_c(chatHelper, privilege);
    }

    [DllImport("ZMWinUnityVideoSDK.dll")]
    private static extern ZMVideoSDKChatPrivilegeType getChatPrivilege_c(IntPtr helper);

    public ZMVideoSDKChatPrivilegeType GetChatPrivilege()
    {
        return getChatPrivilege_c(chatHelper);
    }
}

#endif