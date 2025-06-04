using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKChatPrivilegeType
{

    public AndroidZoomVideoSDKChatPrivilegeType()
    {
    }

    private static AndroidJavaClass androidClass = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_CHAT_PRIVILEGE_TYPE_PATH);

    private static Dictionary<ZMVideoSDKChatPrivilegeType, AndroidJavaObject> enumDic
        = new Dictionary<ZMVideoSDKChatPrivilegeType, AndroidJavaObject>()
        {
            {ZMVideoSDKChatPrivilegeType.NoOne, androidClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKChatPrivilege_No_One")},
            {ZMVideoSDKChatPrivilegeType.Publicly, androidClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKChatPrivilege_Publicly")},
            {ZMVideoSDKChatPrivilegeType.PubliclyAndPrivately, androidClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKChatPrivilege_Publicly_And_Privately")},
            {ZMVideoSDKChatPrivilegeType.Unknown, androidClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKChatPrivilege_Unknown")},
        };

    private static Dictionary<string, ZMVideoSDKChatPrivilegeType> nameDic
    = new Dictionary<string, ZMVideoSDKChatPrivilegeType>()
    {
            {androidClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKChatPrivilege_No_One").Call<string>("name"), ZMVideoSDKChatPrivilegeType.NoOne},
            {androidClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKChatPrivilege_Publicly").Call<string>("name"), ZMVideoSDKChatPrivilegeType.Publicly},
            {androidClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKChatPrivilege_Publicly_And_Privately").Call<string>("name"), ZMVideoSDKChatPrivilegeType.PubliclyAndPrivately},
            {androidClass.GetStatic<AndroidJavaObject>("ZoomVideoSDKChatPrivilege_Unknown").Call<string>("name"), ZMVideoSDKChatPrivilegeType.Unknown},
    };

    public static ZMVideoSDKChatPrivilegeType GetEnum(string name)
    {
        ZMVideoSDKChatPrivilegeType enumType;
        try
        {
            enumType = nameDic[name];
        }
        catch (KeyNotFoundException)
        {
            enumType = ZMVideoSDKChatPrivilegeType.Unknown;
        }
        return enumType;
    }

    public static AndroidJavaObject GetJavaObject(ZMVideoSDKChatPrivilegeType name)
    {
        AndroidJavaObject javaObject;
        try
        {
            javaObject = enumDic[name];
        }
        catch (KeyNotFoundException)
        {
            javaObject = null;
        }
        return javaObject;
    }
}

