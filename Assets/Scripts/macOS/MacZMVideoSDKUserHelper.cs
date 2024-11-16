#if UNITY_STANDALONE_OSX
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MacZMVideoSDKUserHelper
{
    public MacZMVideoSDKUserHelper(IntPtr _objC_ZMVideoSDKUserHelper)
    {
        objC_ZMVideoSDKUserHelper = _objC_ZMVideoSDKUserHelper;
    }

    [DllImport ("ZMMacUnityVideoSDK")]
	private static extern void deallocate_ObjCWrapper(IntPtr zmVideoSDKUserHelper);
    ~MacZMVideoSDKUserHelper()
    {
        deallocate_ObjCWrapper(objC_ZMVideoSDKUserHelper);
    }

    private IntPtr objC_ZMVideoSDKUserHelper;
    public IntPtr ObjC_ZMVideoSDKUserHelper
    {
        get { return objC_ZMVideoSDKUserHelper; }
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool changeName(IntPtr zmVideoSDKUserHelper, string name, IntPtr objC_ZMVideoSDKUser);
    public bool ChangeName(string name, ZMVideoSDKUser zmVideoSDKUser)
    {
        MacZMVideoSDKUser macZMVideoSDKUser = zmVideoSDKUser.VideoSDKUser;
        return changeName(objC_ZMVideoSDKUserHelper, name, macZMVideoSDKUser.ObjC_ZMVideoSDKUser);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool makeHost(IntPtr zmVideoSDKUserHelper, IntPtr objC_ZMVideoSDKUser);
    public bool MakeHost(ZMVideoSDKUser zmVideoSDKUser)
    {
        MacZMVideoSDKUser macZMVideoSDKUser = zmVideoSDKUser.VideoSDKUser;
        return makeHost(objC_ZMVideoSDKUserHelper, macZMVideoSDKUser.ObjC_ZMVideoSDKUser);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool makeManager(IntPtr zmVideoSDKUserHelper, IntPtr objC_ZMVideoSDKUser);
    public bool MakeManager(ZMVideoSDKUser zmVideoSDKUser)
    {
        MacZMVideoSDKUser macZMVideoSDKUser = zmVideoSDKUser.VideoSDKUser;
        return makeManager(objC_ZMVideoSDKUserHelper, macZMVideoSDKUser.ObjC_ZMVideoSDKUser);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool revokeManager(IntPtr zmVideoSDKUserHelper, IntPtr objC_ZMVideoSDKUser);
    public bool RevokeManager(ZMVideoSDKUser zmVideoSDKUser)
    {
        MacZMVideoSDKUser macZMVideoSDKUser = zmVideoSDKUser.VideoSDKUser;
        return revokeManager(objC_ZMVideoSDKUserHelper, macZMVideoSDKUser.ObjC_ZMVideoSDKUser);
    }

    [DllImport ("ZMMacUnityVideoSDK")]
    private static extern bool removeUser(IntPtr zmVideoSDKUserHelper, IntPtr objC_ZMVideoSDKUser);
    public bool RemoveUser(ZMVideoSDKUser zmVideoSDKUser)
    {
        MacZMVideoSDKUser macZMVideoSDKUser = zmVideoSDKUser.VideoSDKUser;
        return removeUser(objC_ZMVideoSDKUserHelper, macZMVideoSDKUser.ObjC_ZMVideoSDKUser);
    }
}
#endif
