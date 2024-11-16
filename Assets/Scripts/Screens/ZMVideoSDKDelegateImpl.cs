using System;
using System.Collections.Generic;
using UnityEngine;

public class ZMVideoSDKDelegateImpl : IZMVideoSDKDelegate
{
    public static IntroScreen introScreen;
    private string myselfUserId = "";
    private static Dictionary<string, ZMVideoSDKRawDataPipe> rawDataPipeDic = new Dictionary<string, ZMVideoSDKRawDataPipe>();
    private static Dictionary<string, IZMVideoSDKRawDataPipeDelegate> rawDataPipeDelegateDic = new Dictionary<string, IZMVideoSDKRawDataPipeDelegate>();
    private static Dictionary<string, ZMVideoSDKRawDataPipe> shareDataPipeDic = new Dictionary<string, ZMVideoSDKRawDataPipe>();
    //private bool enableRawAudio = false;

    public ZMVideoSDKDelegateImpl()
    {
    }

    public static void SubscribeVideoRawData(ZMVideoSDKUser user, ZMVideoSDKResolution resolution)
    {
        string userId = user.GetUserID();
        IZMVideoSDKRawDataPipeDelegate rawDataPipeDelegate;
        if (rawDataPipeDelegateDic.ContainsKey(userId))
        {
            rawDataPipeDelegate = rawDataPipeDelegateDic[userId];
        } else
        {
            rawDataPipeDelegate = new ZMVideoSDKRawDataPipeDelegateImpl(userId, introScreen, false);
        }
        ZMVideoSDKRawDataPipe pipe = user.GetVideoPipe();
        rawDataPipeDic[userId] = pipe;
        pipe.Subscribe(resolution, rawDataPipeDelegate);
    }

    private void UnsubscribeRawData(string userId)
    {
        if (rawDataPipeDic.ContainsKey(userId))
        {
            rawDataPipeDelegateDic.Remove(userId);
            rawDataPipeDic[userId].UnSubscribe();
            rawDataPipeDic.Remove(userId);
        }
    }

    public void onSessionJoin(ZMVideoSDKUser myself)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            introScreen.CreateRawImageObject(myself.GetUserID());
            introScreen.SetVideoListActive();
        });
        myselfUserId = myself.GetUserID();
        SubscribeVideoRawData(myself, ZMVideoSDKResolution.ZMVideoSDKResolution_90P);
        introScreen.SetSessionIsEnd(false);

#if UNITY_STANDALONE_OSX
            ZMVideoSDKVideoHelper videoHelper = ZMVideoSDK.Instance.GetVideoHelper();
            List<ZMVideoSDKCameraDevice> cameras = videoHelper.GetCameraList();
            if (cameras != null && cameras.Count > 0)
            {
                videoHelper.SelectCamera(cameras[0].deviceID);
            }

            ZMVideoSDKAudioHelper audioHelper = ZMVideoSDK.Instance.GetAudioHelper();
            List<ZMVideoSDKSpeakerDevice> speakers = audioHelper.GetSpeakerList();
            if (speakers != null && speakers.Count > 0)
            {
                audioHelper.SelectSpeaker(speakers[0].deviceID, speakers[0].deviceName);
            }

            List<ZMVideoSDKMicDevice> mics = audioHelper.GetMicList();
            if (mics != null && mics.Count > 0)
            {
                audioHelper.SelectMic(mics[0].deviceID, mics[0].deviceName);
            }
#endif
    }

    public void onSessionLeave()
    {
        Dispatcher.RunOnMainThread(() =>
        {
            introScreen.DestroyAllGameObject();
            introScreen.ResetPage();
        });
        ZMVideoSDK.Instance.RemoveListener(this);
        rawDataPipeDic.Clear();
        shareDataPipeDic.Clear();
        rawDataPipeDelegateDic.Clear();
        myselfUserId = "";
        introScreen.SetSessionIsEnd(true);
        //enableRawAudio = false;
    }

    public void onUserJoin(ZMVideoSDKUserHelper userHelper, List<ZMVideoSDKUser> userArray)
    {
        foreach (ZMVideoSDKUser user in userArray)
        {
            Dispatcher.RunOnMainThread(() =>
            {
                introScreen.CreateRawImageObject(user.GetUserID());
            });
            SubscribeVideoRawData(user, ZMVideoSDKResolution.ZMVideoSDKResolution_90P);
        }
    }

    public void onUserLeave(ZMVideoSDKUserHelper userHelper, List<ZMVideoSDKUser> userArray)
    {
        foreach (ZMVideoSDKUser user in userArray)
        {
            string userId = user.GetUserID();
            UnsubscribeRawData(userId);
            Dispatcher.RunOnMainThread(() =>
            {
                introScreen.DestroyGameObject(userId);
            });
        }
    }

    public void onUserVideoStatusChanged(ZMVideoSDKVideoHelper videoHelper, List<ZMVideoSDKUser> userArray)
    {
        foreach (ZMVideoSDKUser user in userArray)
        {
            ZMVideoSDKRawDataPipe pipe = user.GetVideoPipe();
            ZMVideoSDKVideoStatus videoStatus = pipe.GetVideoStatus();
            bool isVideoOn = videoStatus.isOn;
            Dispatcher.RunOnMainThread(() =>
            {
                if (String.Equals(user.GetUserID(), myselfUserId))
                {
                    introScreen.ChangeVideoIcon(isVideoOn);
                }
                introScreen.ChangeVideo(user.GetUserID(), isVideoOn);
            });
        }
    }

    public void onError(ZMVideoSDKErrors errorType, int details)
    {
        Debug.LogError("onError " + errorType.ToString());
    }

    public void onUserAudioStatusChanged(ZMVideoSDKAudioHelper audioHelper, List<ZMVideoSDKUser> userArray)
    {
        foreach (ZMVideoSDKUser user in userArray)
        {
            bool isMuted = user.GetAudioStatus().isMuted;
            if (String.Equals(user.GetUserID(), myselfUserId))
            {
                Dispatcher.RunOnMainThread(() =>
                {
                    introScreen.ChangeAudioIcon(isMuted);
                });
            }
            //To subscribe to audio raw data.
            //if (!enableRawAudio && !isMuted)
            //{
            //    audioHelper.Subscribe(res =>
            //    {
            //        if (res == ZMVideoSDKErrors.ZMVideoSDKErrors_Success)
            //        {
            //            enableRawAudio = true;
            //        }
            //        Debug.Log("onUserAudioStatusChanged result = " + res);
            //    });
            //}
        }
    }

    public void onChatNewMessageNotify(ZMVideoSDKChatHelper chatHelper, ZMVideoSDKChatMessage messageItem)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            introScreen.ReceiveChatMessage(messageItem);
        });
    }

    public void onChatDeleteMessageNotify(ZMVideoSDKChatHelper chatHelper, string msgID, ZMVideoSDKChatMessageDeleteType deleteBy)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            introScreen.DeleteChatMessage(msgID, deleteBy);
        });
    }

    public void onCloudRecordingStatusChanged(ZMVideoSDKRecordingStatus status, ZMVideoSDKRecordingConsentHandler handler)
    {
        Debug.Log("onCloudRecordingStatus" + status);
    }

    public void onUserRecordingConsentChanged(ZMVideoSDKUser user)
    {
        Debug.Log("onUserRecordingConsent" + user.GetUserID());
    }

    public void onUserShareStatusChanged(ZMVideoSDKShareHelper shareHelper, ZMVideoSDKUser user, ZMVideoSDKShareStatus status)
    {
        string userId = user.GetUserID();
        bool isSharing = status == ZMVideoSDKShareStatus.ZMVideoSDKShareStatus_Start || status == ZMVideoSDKShareStatus.ZMVideoSDKShareStatus_Resume;
        if (String.Equals(user.GetUserID(), myselfUserId))
        {
            Dispatcher.RunOnMainThread(() =>
            {
                introScreen.ToggleShareIcon(isSharing);
            });
        }
        else
        {
            if (isSharing)
            {
                ZMVideoSDKRawDataPipe sharePipe = user.GetSharePipe();
                shareDataPipeDic.TryAdd(userId, sharePipe);
                IZMVideoSDKRawDataPipeDelegate shareDelegate = new ZMVideoSDKRawDataPipeDelegateImpl(userId, introScreen, true);
                sharePipe.Subscribe(ZMVideoSDKResolution.ZMVideoSDKResolution_360P, shareDelegate);
            }
            else
            {
                if (shareDataPipeDic.ContainsKey(userId))
                {
                    ZMVideoSDKRawDataPipe sharePipe = shareDataPipeDic[userId];
                    sharePipe.UnSubscribe();
                    shareDataPipeDic.Remove(userId);
                    Dispatcher.RunOnMainThread(() =>
                    {
                        introScreen.UpdateMainViewVideo(myselfUserId);
                    });
                }
            }
        }
    }

    public void onUserActiveAudioChanged(ZMVideoSDKAudioHelper audioHelper, List<ZMVideoSDKUser> userArray)
    {
    }

    public void onHostAskUnmute()
    {
    }

    public void onMicSpeakerVolumeChanged(uint micVolume, uint speakerVolume)
    {
    }

    public void onSelectedAudioDeviceChanged()
    {
    }

    public void onSessionNeedPassword(ZMVideoSDKAudioHelper audioHelper, List<ZMVideoSDKUser> userArray)
    {
    }

    public void onUserHostChanged(ZMVideoSDKUserHelper userHelper, ZMVideoSDKUser user)
    {
    }

    public void onUserManagerChanged(ZMVideoSDKUser user)
    {
    }

    public void onUserNameChanged(ZMVideoSDKUser user)
    {
    }

    public void onMixedAudioRawDataReceived(ZMVideoSDKAudioRawData audioRawData)
    {
        Debug.Log("inside onMixedAudioRawDataReceived");
    }

    public void onOneWayAudioRawDataReceived(ZMVideoSDKAudioRawData audioRawData, ZMVideoSDKUser user)
    {
        Debug.Log("inside onOneWayAudioRawDataReceived");
    }
}
