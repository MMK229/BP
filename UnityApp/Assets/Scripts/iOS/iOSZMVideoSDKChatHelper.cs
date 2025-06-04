#if UNITY_IOS
using System;
using System.Runtime.InteropServices;

public class IOSZMVideoSDKChatHelper
{
    public IOSZMVideoSDKChatHelper(IntPtr _objC_ZMVideoSDKChatHelper)
    {
        objC_ZMVideoSDKChatHelper = _objC_ZMVideoSDKChatHelper;
    }

    [DllImport ("__Internal")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKChatHelper);
    ~IOSZMVideoSDKChatHelper()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKChatHelper);
    }

    private IntPtr objC_ZMVideoSDKChatHelper;
    public IntPtr ObjC_ZMVideoSDKChatHelper
    {
        get { return objC_ZMVideoSDKChatHelper; }
    }

    [DllImport ("__Internal")]
    private static extern bool isChatDisabled(IntPtr zmVideoSDKChatHelper);
    public bool IsChatDisabled()
    {
        return isChatDisabled(objC_ZMVideoSDKChatHelper);
    }

    [DllImport ("__Internal")]
    private static extern bool isPrivateChatDisabled(IntPtr zmVideoSDKChatHelper);
    public bool IsPrivateChatDisabled()
    {
        return isPrivateChatDisabled(objC_ZMVideoSDKChatHelper);
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors sendChatToUser(IntPtr zmVideoSDKChatHelper, IntPtr objC_ZMVideoSDKUser, string content);
    public void SendChatToUser(ZMVideoSDKUser zmVideoSDKUser, string msgContent, Action<ZMVideoSDKErrors> callback)
    {
        IOSZMVideoSDKUser iOSZMVideoSDKUser = zmVideoSDKUser.VideoSDKUser;
        ZMVideoSDKErrors vsdkError = sendChatToUser(objC_ZMVideoSDKChatHelper, iOSZMVideoSDKUser.ObjC_ZMVideoSDKUser, msgContent);
        callback(vsdkError);
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors sendChatToAll(IntPtr zmVideoSDKChatHelper, string content);
    public void SendChatToAll(string msgContent, Action<ZMVideoSDKErrors> callback)
    {
        ZMVideoSDKErrors vsdkError = sendChatToAll(objC_ZMVideoSDKChatHelper, msgContent);
        callback(vsdkError);
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors deleteChatMessage(IntPtr zmVideoSDKChatHelper, string msgID);
    public void DeleteChatMessage(string msgId, Action<ZMVideoSDKErrors> callback)
    {
        ZMVideoSDKErrors vsdkError = deleteChatMessage(objC_ZMVideoSDKChatHelper, msgId);
        callback(vsdkError);
    }

    [DllImport ("__Internal")]
    private static extern bool canChatMessageBeDeleted(IntPtr zmVideoSDKChatHelper, string msgID);
    public bool CanChatMessageBeDeleted(string msgId)
    {
        return canChatMessageBeDeleted(objC_ZMVideoSDKChatHelper, msgId);
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKErrors changeChatPrivilege(IntPtr zmVideoSDKChatHelper, ZMVideoSDKChatPrivilegeType privilege);
    public ZMVideoSDKErrors ChangeChatPrivilege(ZMVideoSDKChatPrivilegeType privilege)
    {
        return changeChatPrivilege(objC_ZMVideoSDKChatHelper, privilege);
    }

    [DllImport ("__Internal")]
    private static extern ZMVideoSDKChatPrivilegeType getChatPrivilege(IntPtr zmVideoSDKChatHelper);
    public ZMVideoSDKChatPrivilegeType GetChatPrivilege()
    {
        return getChatPrivilege(objC_ZMVideoSDKChatHelper);
    }
}
#endif
