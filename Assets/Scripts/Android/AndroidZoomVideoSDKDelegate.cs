#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class AndroidZoomVideoSDKDelegate : AndroidJavaProxy
{
    private AndroidJavaObject zoomSDK;
    private Dictionary<string, IZMVideoSDKRawDataPipeDelegate> rawDataPipeDic;
    private IZMVideoSDKDelegate videoSDKDelegate;

    public AndroidZoomVideoSDKDelegate(IZMVideoSDKDelegate videoSDKDelegate) : base(Config.ZOOM_VIDEO_SDK_DELEGATE_PATH)
    {
        rawDataPipeDic = new Dictionary<string, IZMVideoSDKRawDataPipeDelegate>();
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass(Config.UNITY_PLAYER_PATH))
        using (AndroidJavaClass jc = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_PATH))
        {
            zoomSDK = jc.CallStatic<AndroidJavaObject>("getInstance");
        }
        this.videoSDKDelegate = videoSDKDelegate;
    }

    //Callback from ZoomVideoSDKDelegate
    public void onSessionJoin()
    {
        using (AndroidJavaObject session = zoomSDK.Call<AndroidJavaObject>("getSession"))
        {
            AndroidJavaObject myself = session.Call<AndroidJavaObject>("getMySelf");
            AndroidZoomVideoSDKUser androidUserObject = new AndroidZoomVideoSDKUser(myself);
            ZMVideoSDKUser user = new ZMVideoSDKUser(androidUserObject);
            videoSDKDelegate.onSessionJoin(user);
        }
    }

    public void onSessionLeave()
    {
        Debug.Log("onSessionLeave");
        rawDataPipeDic.Clear();
        videoSDKDelegate.onSessionLeave();
    }

    public void onUserJoin(AndroidJavaObject userHelper, AndroidJavaObject userList)
    {
        using (userHelper)
        using (userList)
        {
            List<ZMVideoSDKUser> users = new List<ZMVideoSDKUser>();
            int listSize = userList.Call<int>("size");
            for (int i = 0; i < listSize; i++)
            {
                AndroidJavaObject userObject = userList.Call<AndroidJavaObject>("get", i);
                AndroidZoomVideoSDKUser androidUserObject = new AndroidZoomVideoSDKUser(userObject);
                ZMVideoSDKUser user = new ZMVideoSDKUser(androidUserObject);
                users.Add(user);
            }
            AndroidZoomVideoSDKUserHelper androidUserHelper = new AndroidZoomVideoSDKUserHelper(userHelper);
            ZMVideoSDKUserHelper helper = new ZMVideoSDKUserHelper(androidUserHelper);
            videoSDKDelegate.onUserJoin(helper, users);
        }
    }

    public void onError(int errorCode)
    {
        ZMVideoSDKErrors errorEnum = AndroidZoomVideoSDKErrors.GetEnum(errorCode);
        videoSDKDelegate.onError(errorEnum, errorCode);
    }

    public void onUserLeave(AndroidJavaObject userHelper, AndroidJavaObject userList)
    {
        using (userHelper)
        using (userList)
        {
            List<ZMVideoSDKUser> users = new List<ZMVideoSDKUser>();
            int listSize = userList.Call<int>("size");
            for (int i = 0; i < listSize; i++)
            {
                AndroidJavaObject userObject = userList.Call<AndroidJavaObject>("get", i);
                AndroidZoomVideoSDKUser androidUserObject = new AndroidZoomVideoSDKUser(userObject);
                ZMVideoSDKUser user = new ZMVideoSDKUser(androidUserObject);
                users.Add(user);
            }
            AndroidZoomVideoSDKUserHelper androidUserHelper = new AndroidZoomVideoSDKUserHelper(userHelper);
            ZMVideoSDKUserHelper helper = new ZMVideoSDKUserHelper(androidUserHelper);
            videoSDKDelegate.onUserLeave(helper, users);
        }
    }


    public void onUserVideoStatusChanged(AndroidJavaObject videoHelper, AndroidJavaObject userList)
    {
        using (videoHelper)
        using (userList)
        {
            List<ZMVideoSDKUser> users = new List<ZMVideoSDKUser>();
            int listSize = userList.Call<int>("size");
            for (int i = 0; i < listSize; i++)
            {
                AndroidJavaObject userObject = userList.Call<AndroidJavaObject>("get", i);
                AndroidZoomVideoSDKUser androidUserObject = new AndroidZoomVideoSDKUser(userObject);
                ZMVideoSDKUser user = new ZMVideoSDKUser(androidUserObject);
                users.Add(user);
            }
            ZMVideoSDKVideoHelper helper = new ZMVideoSDKVideoHelper(videoHelper);
            videoSDKDelegate.onUserVideoStatusChanged(helper, users);
        }
    }

    public void onUserAudioStatusChanged(AndroidJavaObject audioHelper, AndroidJavaObject userList)
    {
        using (audioHelper)
        using (userList)
        {
            List<ZMVideoSDKUser> users = new List<ZMVideoSDKUser>();
            int listSize = userList.Call<int>("size");
            for (int i = 0; i < listSize; i++)
            {
                AndroidJavaObject userObject = userList.Call<AndroidJavaObject>("get", i);
                AndroidZoomVideoSDKUser androidUserObject = new AndroidZoomVideoSDKUser(userObject);
                ZMVideoSDKUser user = new ZMVideoSDKUser(androidUserObject);
                users.Add(user);
            }
            AndroidZoomVideoSDKAudioHelper androidAudioHelper = new AndroidZoomVideoSDKAudioHelper(audioHelper);
            ZMVideoSDKAudioHelper helper = new ZMVideoSDKAudioHelper(androidAudioHelper);
            videoSDKDelegate.onUserAudioStatusChanged(helper, users);
        }
    }


    public void onUserHostChanged(AndroidJavaObject userHelper, AndroidJavaObject userInfo)
    {
        using (userHelper)
        using (userInfo)
        {
            AndroidZoomVideoSDKUser androidUserObject = new AndroidZoomVideoSDKUser(userInfo);
            ZMVideoSDKUser user = new ZMVideoSDKUser(androidUserObject);
            AndroidZoomVideoSDKUserHelper androidUserHelper = new AndroidZoomVideoSDKUserHelper(userHelper);
            ZMVideoSDKUserHelper helper = new ZMVideoSDKUserHelper(androidUserHelper);
            videoSDKDelegate.onUserHostChanged(helper, user);
        }
    }


    public void onUserManagerChanged(AndroidJavaObject userObject)
    {
        using (userObject)
        {
            AndroidZoomVideoSDKUser androidUserObject = new AndroidZoomVideoSDKUser(userObject);
            ZMVideoSDKUser user = new ZMVideoSDKUser(androidUserObject);
            videoSDKDelegate.onUserManagerChanged(user);
        }
    }


    public void onUserNameChanged(AndroidJavaObject userObject)
    {
        using (userObject)
        {
            AndroidZoomVideoSDKUser androidUserObject = new AndroidZoomVideoSDKUser(userObject);
            ZMVideoSDKUser user = new ZMVideoSDKUser(androidUserObject);
            videoSDKDelegate.onUserNameChanged(user);
        }

    }


    public void onUserActiveAudioChanged(AndroidJavaObject audioHelper, AndroidJavaObject userList)
    {
        using (audioHelper)
        using (userList)
        {
            List<ZMVideoSDKUser> users = new List<ZMVideoSDKUser>();
            int listSize = userList.Call<int>("size");
            for (int i = 0; i < listSize; i++)
            {
                using (AndroidJavaObject userObject = userList.Call<AndroidJavaObject>("get", i))
                {
                    AndroidZoomVideoSDKUser androidUser = new AndroidZoomVideoSDKUser(userObject);
                    ZMVideoSDKUser user = new ZMVideoSDKUser(androidUser);
                    users.Add(user);
                }
            }
            AndroidZoomVideoSDKAudioHelper androidHelper = new AndroidZoomVideoSDKAudioHelper(audioHelper);
            ZMVideoSDKAudioHelper helper = new ZMVideoSDKAudioHelper(androidHelper);
            videoSDKDelegate.onUserActiveAudioChanged(helper, users);
        }

    }


    public void onMixedAudioRawDataReceived(AndroidJavaObject rawData)
    {
        ZMVideoSDKAudioRawData audioRawData = processAudioData(rawData);
        videoSDKDelegate.onMixedAudioRawDataReceived(audioRawData);
    }


    private ZMVideoSDKAudioRawData processAudioData(AndroidJavaObject rawData)
    {
        int bufferLen = rawData.Call<int>("getBufferLen");
        int sampleRate = rawData.Call<int>("getSampleRate");
        int channelNum = rawData.Call<int>("getChannelNum");
        long handle = rawData.Call<long>("getNativeHandle");
        IntPtr ptr = (IntPtr)handle;
        return new ZMVideoSDKAudioRawData(ptr, bufferLen, sampleRate, channelNum);
    }


    public void onOneWayAudioRawDataReceived(AndroidJavaObject rawData, AndroidJavaObject user)
    {
        ZMVideoSDKAudioRawData audioRawData = processAudioData(rawData);
        AndroidZoomVideoSDKUser androidUserObject = new AndroidZoomVideoSDKUser(user);
        ZMVideoSDKUser userObject = new ZMVideoSDKUser(androidUserObject);
        videoSDKDelegate.onOneWayAudioRawDataReceived(audioRawData, userObject);
    }


    public void onChatNewMessageNotify(AndroidJavaObject javaChatHelper, AndroidJavaObject javaMessageItem)
    {
        using (javaChatHelper)
        using (javaMessageItem)
        {
            AndroidZoomVideoSDKChatHelper androidChatHelper = new AndroidZoomVideoSDKChatHelper(javaChatHelper);
            ZMVideoSDKChatHelper chatHelper = new ZMVideoSDKChatHelper(androidChatHelper);

            ZMVideoSDKChatMessage chatMessage = new ZMVideoSDKChatMessage();

            AndroidJavaObject javaSenderUser = javaMessageItem.Call<AndroidJavaObject>("getSenderUser");
            AndroidZoomVideoSDKUser androidSenderUser = new AndroidZoomVideoSDKUser(javaSenderUser);
            chatMessage.senderUser = new ZMVideoSDKUser(androidSenderUser);

            AndroidJavaObject javaReceiverUser = javaMessageItem.Call<AndroidJavaObject>("getReceiverUser");
            AndroidZoomVideoSDKUser androidReceiverUser = new AndroidZoomVideoSDKUser(javaReceiverUser);
            chatMessage.receiverUser = new ZMVideoSDKUser(androidReceiverUser);

            chatMessage.msgId = javaMessageItem.Call<string>("getMessageId");
            chatMessage.content = javaMessageItem.Call<string>("getContent");
            chatMessage.timeStamp = javaMessageItem.Call<long>("getTimeStamp");
            chatMessage.isChatToAll = javaMessageItem.Call<bool>("isChatToAll");
            chatMessage.isSelfSend = javaMessageItem.Call<bool>("isSelfSend");

            videoSDKDelegate.onChatNewMessageNotify(chatHelper, chatMessage);
        }
    }


    public void onChatDeleteMessageNotify(AndroidJavaObject javaChatHelper, string msgID, AndroidJavaObject deleteBy)
    {
        using (javaChatHelper)
        using (deleteBy)
        {
            AndroidZoomVideoSDKChatHelper androidChatHelper = new AndroidZoomVideoSDKChatHelper(javaChatHelper);
            ZMVideoSDKChatHelper chatHelper = new ZMVideoSDKChatHelper(androidChatHelper);
            ZMVideoSDKChatMessageDeleteType deleteType = AndroidZoomVideoSDKChatMessageDeleteType.GetEnum(deleteBy.Call<string>("name"));

            videoSDKDelegate.onChatDeleteMessageNotify(chatHelper, msgID, deleteType);
        }
    }


    public void onHostAskUnmute()
    {
        videoSDKDelegate.onHostAskUnmute();
    }


    public void onUserRecordingConsent(AndroidJavaObject javaUserObj)
    {
        AndroidZoomVideoSDKUser androidUserObj = new AndroidZoomVideoSDKUser(javaUserObj);
        ZMVideoSDKUser user = new ZMVideoSDKUser(androidUserObj);
        videoSDKDelegate.onUserRecordingConsentChanged(user);
    }


    public void onCloudRecordingStatus(AndroidJavaObject javaStatusObj, AndroidJavaObject javaHandlerObj)
    {
        AndroidZoomVideoSDKRecordingConsentHandler androidHandlerObj = new AndroidZoomVideoSDKRecordingConsentHandler(javaHandlerObj);
        ZMVideoSDKRecordingConsentHandler handler = new ZMVideoSDKRecordingConsentHandler(androidHandlerObj);

        ZMVideoSDKRecordingStatus status = AndroidZoomVideoSDKRecordingStatus.GetEnum(javaStatusObj.Call<string>("name"));
        videoSDKDelegate.onCloudRecordingStatusChanged(status, handler);
    }


    public void onUserShareStatusChanged(AndroidJavaObject javaShareHelper, AndroidJavaObject javaUser, AndroidJavaObject javaStatus)
    {
        AndroidZoomVideoSDKShareHelper androidShareHelper = new AndroidZoomVideoSDKShareHelper(javaShareHelper);
        ZMVideoSDKShareHelper shareHelper = new ZMVideoSDKShareHelper(androidShareHelper);

        AndroidZoomVideoSDKUser androidUser = new AndroidZoomVideoSDKUser(javaUser);
        ZMVideoSDKUser user = new ZMVideoSDKUser(androidUser);

        ZMVideoSDKShareStatus shareStatus = AndroidZoomVideoSDKShareStatus.GetEnum(javaStatus.Call<string>("name"));

        videoSDKDelegate.onUserShareStatusChanged(shareHelper, user, shareStatus);
    }


    public void onSessionNeedPassword(AndroidJavaObject handler)
    {

    }


    public void onSessionPasswordWrong(AndroidJavaObject handler)
    {
        using (handler)
        {

        }

    }


    public void onLiveStreamStatusChanged(AndroidJavaObject liveStreamHelper, AndroidJavaObject status)
    {
        using (liveStreamHelper)
        using (status)
        {

        }

    }


    public void onShareAudioRawDataReceived(AndroidJavaObject rawData)
    {
    }


    public void onCommandReceived(AndroidJavaObject sender, string strCmd)
    {
        using (sender)
        {

        }

    }


    public void onCommandChannelConnectResult(bool isSuccess)
    {

    }


    public void onInviteByPhoneStatus(AndroidJavaObject status, AndroidJavaObject reason)
    {
        using (status)
        using (reason)
        {

        }

    }


    public void onMultiCameraStreamStatusChanged(AndroidJavaObject status, AndroidJavaObject user, AndroidJavaObject videoPipe)
    {
        using (status)
        using (user)
        using (videoPipe)
        {

        }

    }

    public void onLiveTranscriptionStatus(AndroidJavaObject status)
    {
        using (status)
        {

        }

    }


    public void onLiveTranscriptionMsgReceived(String ltMsg, AndroidJavaObject pUser, AndroidJavaObject type)
    {
        using (pUser)
        using (type)
        {

        }

    }


    public void onLiveTranscriptionMsgInfoReceived(AndroidJavaObject messageInfo)
    {
        using (messageInfo)
        {

        }
    }


    public void onLiveTranscriptionMsgError(AndroidJavaObject spokenLanguage, AndroidJavaObject transcriptLanguage)
    {
        using (spokenLanguage)
        using (transcriptLanguage)
        {

        }
    }


    public void onProxySettingNotification(AndroidJavaObject handler)
    {
        using (handler)
        {

        }
    }


    public void onSSLCertVerifiedFailNotification(AndroidJavaObject info)
    {
        using (info)
        {

        }
    }


    public void onCameraControlRequestResult(AndroidJavaObject user, bool isApproved)
    {
        using (user)
        {

        }
    }


    public void onUserVideoNetworkStatusChanged(AndroidJavaObject status, AndroidJavaObject user)
    {
        using (status)
        using (user)
        {

        }
    }

}
#endif
