#if UNITY_STANDALONE_OSX
using System;
using System.Runtime.InteropServices;

public class MacZMVideoSDKChatHelper
{
    public MacZMVideoSDKChatHelper(IntPtr _objC_ZMVideoSDKChatHelper)
    {
        objC_ZMVideoSDKChatHelper = _objC_ZMVideoSDKChatHelper;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKChatHelper);
    ~MacZMVideoSDKChatHelper()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKChatHelper);
    }

    private IntPtr objC_ZMVideoSDKChatHelper;
    public IntPtr ObjC_ZMVideoSDKChatHelper
    {
        get { return objC_ZMVideoSDKChatHelper; }
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool isChatDisabled(IntPtr zmVideoSDKChatHelper);
    public bool IsChatDisabled()
    {
        return isChatDisabled(objC_ZMVideoSDKChatHelper);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool isPrivateChatDisabled(IntPtr zmVideoSDKChatHelper);
    public bool IsPrivateChatDisabled()
    {
        return isPrivateChatDisabled(objC_ZMVideoSDKChatHelper);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors sendChatToUser(IntPtr zmVideoSDKChatHelper, IntPtr objC_ZMVideoSDKUser, string content);
    public void SendChatToUser(ZMVideoSDKUser zmVideoSDKUser, string msgContent, Action<ZMVideoSDKErrors> callback)
    {
        MacZMVideoSDKUser macOSZMVideoSDKUser = zmVideoSDKUser.VideoSDKUser;
        ZMVideoSDKErrors vsdkError = sendChatToUser(objC_ZMVideoSDKChatHelper, macOSZMVideoSDKUser.ObjC_ZMVideoSDKUser, msgContent);
        callback(vsdkError);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors sendChatToAll(IntPtr zmVideoSDKChatHelper, string content);
    public void SendChatToAll(string msgContent, Action<ZMVideoSDKErrors> callback)
    {
        ZMVideoSDKErrors vsdkError = sendChatToAll(objC_ZMVideoSDKChatHelper, msgContent);
        callback(vsdkError);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors deleteChatMessage(IntPtr zmVideoSDKChatHelper, string msgID);
    public void DeleteChatMessage(string msgId, Action<ZMVideoSDKErrors> callback)
    {
        ZMVideoSDKErrors vsdkError = deleteChatMessage(objC_ZMVideoSDKChatHelper, msgId);
        callback(vsdkError);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool canChatMessageBeDeleted(IntPtr zmVideoSDKChatHelper, string msgID);
    public bool CanChatMessageBeDeleted(string msgId)
    {
        return canChatMessageBeDeleted(objC_ZMVideoSDKChatHelper, msgId);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKErrors changeChatPrivilege(IntPtr zmVideoSDKChatHelper, ZMVideoSDKChatPrivilegeType privilege);
    public ZMVideoSDKErrors ChangeChatPrivilege(ZMVideoSDKChatPrivilegeType privilege)
    {
        return changeChatPrivilege(objC_ZMVideoSDKChatHelper, privilege);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern ZMVideoSDKChatPrivilegeType getChatPrivilege(IntPtr zmVideoSDKChatHelper);
    public ZMVideoSDKChatPrivilegeType GetChatPrivilege()
    {
        return getChatPrivilege(objC_ZMVideoSDKChatHelper);
    }
}
#endif
