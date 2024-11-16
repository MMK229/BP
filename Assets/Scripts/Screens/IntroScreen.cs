using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System;
using UnityEngine.EventSystems;
using System.Collections.Specialized;
using TMPro;

public class IntroScreen : MonoBehaviour
{
    public GameObject jwtTokenObj;
    public GameObject jwtTokenTagObj;
    public GameObject sessionNameObj;
    public GameObject sessionNameTagObj;
    public GameObject passwordObj;
    public GameObject passwordTagObj;
    public GameObject displayNameObj;
    public GameObject displayNameTagObj;

    public GameObject joinBtnObj;
    public GameObject leaveBtnObj;
    public GameObject videoBtnObj;
    public GameObject audioBtnObj;
    public GameObject shareBtnObj;
    public GameObject switchCameraBtnObj;
    public GameObject chatBtnObj;
    public GameObject videoListPrefab;
    public GameObject videoListParentObj;
    public GameObject mainVideoViewParentObj;
    public GameObject mainVideoViewPrefab;
    public GameObject toolBarObj;
    public GameObject joinSessionPageObj;
    public GameObject inSessionPageObj;
    public GameObject videoListScrollViewObj;
    public GameObject videoListObj;
    public GameObject chatBot;
    public GameObject chatInputObj;
    public GameObject alertWindowObj;
    private RawImage mainVideoView;

    private Sprite muteIcon;
    private Sprite unmuteIcon;
    private Sprite videoOffIcon;
    private Sprite videoOnIcon;
    private Sprite shareOnIcon;
    private Sprite shareOffIcon;
    private Material profileImage;
    private Texture2D profileTexture;
    private Shader yuvShader;

    private OrderedDictionary videoListDic = new OrderedDictionary();
    private Dictionary<string, Material> shaderDic = new Dictionary<string, Material>();
    private Dictionary<string, Texture> textureDic = new Dictionary<string, Texture>();

    private ZMVideoSDKUser mainVideoViewUser = null;
    private string mainVideoViewUserId = "";
    private string videoItemNamePrefix = "VideoView";
    private bool isChatBotVisible = false;
    private bool isSessionEnded = false;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        CheckPermission();
        ChangeActiveStatus(toolBarObj, false);
        ChangeActiveStatus(joinSessionPageObj, false);
        ChangeActiveStatus(inSessionPageObj, false);

        float btnPosition = CalculateButtonXPosition(videoBtnObj);

        videoBtnObj.transform.localPosition =
            new Vector2(btnPosition, videoBtnObj.transform.localPosition.y);
        audioBtnObj.transform.localPosition =
            new Vector2(btnPosition, audioBtnObj.transform.localPosition.y);
        switchCameraBtnObj.transform.localPosition =
            new Vector2(btnPosition, switchCameraBtnObj.transform.localPosition.y);
        chatBtnObj.transform.localPosition =
            new Vector2(btnPosition, chatBtnObj.transform.localPosition.y);
        shareBtnObj.transform.localPosition =
            new Vector2(btnPosition, shareBtnObj.transform.localPosition.y);

        RectTransform videoListScrollViewTransform = videoListScrollViewObj.GetComponent<RectTransform>();
        videoListObj.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, videoListScrollViewTransform.sizeDelta.y);
        videoListScrollViewTransform.sizeDelta = new Vector2(Screen.width, videoListScrollViewTransform.sizeDelta.y);

        leaveBtnObj.transform.localPosition = new Vector2(CalculateButtonXPosition(leaveBtnObj), CalculateLeaveButtonYPosition(leaveBtnObj));

        videoListObj.transform.localPosition = new Vector2(10f, CalculateVideoListYPosition(videoListObj));

        if (Screen.width > Screen.height)
        {
            ChangeActiveStatus(switchCameraBtnObj, false);
        }

        this.yuvShader = Shader.Find("YUV420Shader");
        this.profileTexture = Resources.Load<Sprite>("Icons/default-avatar").texture;
        this.profileImage = new Material(Shader.Find("UI/Default"));
        this.muteIcon = Resources.Load("Icons/mute@2x", typeof(Sprite)) as Sprite;
        this.unmuteIcon = Resources.Load("Icons/unmute@2x", typeof(Sprite)) as Sprite;
        this.videoOnIcon = Resources.Load("Icons/Video-on@2x", typeof(Sprite)) as Sprite;
        this.videoOffIcon = Resources.Load("Icons/Video-off@2x", typeof(Sprite)) as Sprite;
        this.shareOnIcon = Resources.Load("Icons/icon_share", typeof(Sprite)) as Sprite;
        this.shareOffIcon = Resources.Load("Icons/icon_stop_share", typeof(Sprite)) as Sprite;

        ZMVideoSDKInitParams initParams = new ZMVideoSDKInitParams();
        initParams.domain = "https://zoom.us";
        initParams.enableLog = true;
        #if UNITY_IOS
        /**
        * Set the group identifier for the app here used for screen sharing
        * Create your own app groupId on the Apple Developer Web site, and fill the group ID in this file and in the file SampleHandler.mm
        * Create an "App Groups" Capability in the main project target and the screenshare target, and select the groupId correctly.
        * If you can't select groupId correctly in "App Groups" Capability, Please check files of ZoomVideoSample.entitlements and ZoomVideoSDKScreenShare.entitlements, need to configure the correct group id. etc:
        *
        * For details refer to https://developers.zoom.us/docs/video-sdk/ios/share/
        *
        * if you don't need screen share feature, appGroupId can fill an empty string, or delete the bottom line. And delete ZoomVideoSDKScreenShare target.
        */
            initParams.appGroupIdentifier = "YOUR_GROUP_IDENTIFIER_HERE";
        #endif
        ZMVideoSDK.Instance.Initialize(initParams, (result) => {
            if (result == ZMVideoSDKErrors.ZMVideoSDKErrors_Success)
            {
                Dispatcher.RunOnMainThread(() =>
                {
                    ShowChatBot(false);
                    ChangeActiveStatus(joinSessionPageObj, true);
                });
            }
        });
    }

    private void DestroyGameObject(UnityEngine.Object gameObject)
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        DestroyGameObject(joinBtnObj);
        DestroyGameObject(leaveBtnObj);
        DestroyGameObject(videoBtnObj);
        DestroyGameObject(audioBtnObj);
        DestroyGameObject(switchCameraBtnObj);
        DestroyGameObject(videoListPrefab);
        DestroyGameObject(videoListParentObj);
        DestroyGameObject(mainVideoViewParentObj);
        DestroyGameObject(mainVideoViewPrefab);
        DestroyGameObject(mainVideoView);
        DestroyGameObject(muteIcon);
        DestroyGameObject(unmuteIcon);
        DestroyGameObject(videoOffIcon);
        DestroyGameObject(videoOnIcon);
        DestroyGameObject(profileImage);
        DestroyGameObject(profileTexture);
        DestroyGameObject(yuvShader);
    }

    public void JoinSession()
    {
        ZMVideoSDKSessionContext sessionContext = new ZMVideoSDKSessionContext();
        sessionContext.sessionName = sessionNameObj.GetComponent<TMP_InputField>().text.ToString();
        string password = passwordObj.GetComponent<TMP_InputField>().text.ToString();
        if (!String.IsNullOrEmpty(password))
        {
            sessionContext.sessionPassword = password;
        }
        sessionContext.token = jwtTokenObj.GetComponent<TMP_InputField>().text.ToString();
        sessionContext.userName = displayNameObj.GetComponent<TMP_InputField>().text.ToString();
        sessionContext.videoOption.localVideoOn = true;
        sessionContext.audioOption.mute = false;
        sessionContext.audioOption.connect = true;
        ZMVideoSDK.Instance.JoinSession(sessionContext, (result) => {
            if (result == ZMVideoSDKErrors.ZMVideoSDKErrors_Success)
            {        
                IZMVideoSDKDelegate ZMDelegate = new ZMVideoSDKDelegateImpl();
                ZMVideoSDKDelegateImpl.introScreen = this;
                ZMVideoSDK.Instance.AddListener(ZMDelegate);
                Debug.Log("JoinSession success!");
            }
        });
    }

    public void LeaveSession()
    {
        ZMVideoSDK.Instance.LeaveSession(false, (result) =>
        {
            if (result == ZMVideoSDKErrors.ZMVideoSDKErrors_Success)
            {
                DestroyAllGameObject();
            }
        });
    }

    public void OnSwitchCamera()
    {
        ZMVideoSDKVideoHelper videoHelper = ZMVideoSDK.Instance.GetVideoHelper();
        ZMVideoSDKSession session = ZMVideoSDK.Instance.GetSessionInfo();
        if (session.GetMySelf().GetVideoPipe().GetVideoStatus().isOn)
        {
            videoHelper.SwitchCamera((result) => {
                Debug.Log("Switch Camera success.");
            });
        }
        else
        {
            Debug.Log("The video is off.");
        }
    }

    private string GetCurrentMonitorId()
    {
        List<DisplayInfo> displayLayout = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displayLayout);
        return Screen.mainWindowDisplayInfo.name;
    }

    private uint GetCurrentMonitorIdAsUInt()
    {
        List<DisplayInfo> displayLayout = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displayLayout);
        return (uint)displayLayout.IndexOf(Screen.mainWindowDisplayInfo);
    }

    public void OnShareScreenBtnClicked()
    {
        ZMVideoSDKShareHelper shareHelper = ZMVideoSDK.Instance.GetShareHelper();
        if (shareHelper.IsOtherUserSharingScreen())
        {
            ChangeActiveStatus(alertWindowObj, true);
        }
        else if (shareHelper.IsSharingOut())
        {
            shareHelper.StopShare(res => { });
        }
        else
        {
#if UNITY_ANDROID
            shareHelper.StartShareScreen(res => { });
#elif (UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX)
            ZMVideoSDKShareOption shareOption = new ZMVideoSDKShareOption();
            shareOption.isWithDeviceAudio = false;
            shareOption.isOptimizeForSharedVideo = false;
            ZMVideoSDKErrors res;
            #if UNITY_STANDALONE_WIN
                res = shareHelper.StartShareScreen(GetCurrentMonitorId(), shareOption);
            #elif UNITY_STANDALONE_OSX
                res = shareHelper.StartShareScreen(GetCurrentMonitorIdAsUInt(), shareOption);
            #endif
#endif
        }
    }

    public void OnCloseAlertWindowBtnClicked()
    {
        ChangeActiveStatus(alertWindowObj, false);
    }

    public void ReceiveChatMessage(ZMVideoSDKChatMessage msg)
    {
        BindGameObject(chatBot, "ChatBot");
        ChatBot chatBotObj = chatBot.GetComponent<ChatBot>();
        chatBotObj.SendMessageToChatPanel(msg);
    }

    public void DeleteChatMessage(string msgID, ZMVideoSDKChatMessageDeleteType deleteType)
    {
        BindGameObject(chatBot, "ChatBot");
        ChatBot chatBotObj = chatBot.GetComponent<ChatBot>();
        chatBotObj.DeleteMessageInChatPanel(msgID);
    }

    public void OnSendChatBtnClicked()
    {
        BindGameObject(chatInputObj, "ChatInput");
        BindGameObject(chatBot, "ChatBot");

        string content = chatInputObj.GetComponent<TMP_InputField>().text.ToString();
        ZMVideoSDKChatHelper chatHelper = ZMVideoSDK.Instance.GetChatHelper();
        chatHelper.SendChatToAll(content, res =>
        {
            if (res == ZMVideoSDKErrors.ZMVideoSDKErrors_Success)
            {
                Dispatcher.RunOnMainThread(() =>
                {
                    chatInputObj.GetComponent<TMP_InputField>().text = "";
                });
            }
        });
    }

    public void OnVideoClick()
    {
        ZMVideoSDKSession session = ZMVideoSDK.Instance.GetSessionInfo();
        ZMVideoSDKUser myself = session.GetMySelf();
        ZMVideoSDKVideoHelper videoHelper = ZMVideoSDK.Instance.GetVideoHelper();
        if (myself.GetVideoPipe().GetVideoStatus().isOn)
        {
            videoHelper.StopVideo(result => {
                Debug.Log("StopVideo " + result.ToString());
            });
        }
        else
        {
            videoHelper.StartVideo(result =>
            {
                Debug.Log("StartVideo " + result.ToString());
            });
        }
    }

    public void OnAudioClick()
    {
        ZMVideoSDKSession session = ZMVideoSDK.Instance.GetSessionInfo();
        ZMVideoSDKUser myself = session.GetMySelf();
        ZMVideoSDKAudioHelper audioHelper = ZMVideoSDK.Instance.GetAudioHelper();
        ZMVideoSDKAudioStatus audioStatus = myself.GetAudioStatus();
        if (audioStatus.audioType == ZMVideoSDKAudioType.ZMVideoSDKAudioType_None)
        {
            audioHelper.StartAudio(result =>
            {
                Debug.Log("StartAudio success");
            });
        }
        else if (audioStatus.isMuted)
        {
            audioHelper.UnMuteAudio(myself, result =>
            {
                Debug.Log("UnMuteAudio " + result.ToString());
            });
        }
        else
        {
            audioHelper.MuteAudio(myself, result =>
            {
                Debug.Log("MuteAudio " + result.ToString());
            });
        }
    }

    public void DestroyGameObject(string userId)
    {
        RawImage videoImage = (RawImage)videoListDic[userId];
        if (videoImage == null)
        {
            return;
        }
        if (!isSessionEnded && String.Equals(mainVideoViewUserId, userId))
        {
            ZMVideoSDKUser myself = ZMVideoSDK.Instance.GetSessionInfo()?.GetMySelf();
            mainVideoViewUser = null;
            mainVideoViewUserId = "";
            UpdateMainViewVideo(myself.GetUserID());
        }

        float position = 0;
        IDictionaryEnumerator videoListEnumerator = videoListDic.GetEnumerator();
        while (videoListEnumerator.MoveNext())
        {
            string userIdInList = (string)videoListEnumerator.Key;
            RawImage videoItem = (RawImage)videoListEnumerator.Value;
            if (videoItem != null)
            {
                videoItem.transform.localPosition = new Vector2(position, 0f);
            }
        }
        videoListDic.Remove(userId);
        shaderDic.Remove(userId);
        textureDic.Remove(userId);
        DestroyImmediate(videoImage.gameObject);
    }

    public void DestroyAllGameObject()
    {
        IDictionaryEnumerator videoListEnumerator = videoListDic.GetEnumerator();
        while (videoListEnumerator.MoveNext())
        {
            RawImage videoItem = (RawImage)videoListEnumerator.Value;
            if (videoItem != null)
            {
                DestroyImmediate(videoItem.gameObject);
            }
        }
        videoListDic.Clear();
        shaderDic.Clear();
        textureDic.Clear();
    }

    public void CreateRawImageObject(string userId)
    {
        BindGameObject(videoListParentObj, "VideoListContent");
        RawImage newVideoView = null;
        if (!videoListDic.Contains(userId))
        {
            GameObject newVideoViewObj = Instantiate(videoListPrefab, videoListParentObj.transform);
            newVideoView = newVideoViewObj.GetComponent<RawImage>();
            newVideoView.rectTransform.sizeDelta = new Vector2(0, 0);
            newVideoView.name = videoItemNamePrefix + userId;
            newVideoView.gameObject.AddComponent(typeof(EventTrigger));
            EventTrigger trigger = newVideoViewObj.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) =>
            UpdateMainViewVideo(userId));
            trigger.triggers.Add(entry);
            videoListDic.Add(userId, newVideoView);
        }
        else
        {
            newVideoView = (RawImage)videoListDic[userId];
        }

        Material shaderMaterial = new Material(yuvShader);
        shaderDic.TryAdd(userId, shaderMaterial);
        textureDic.TryAdd(userId, newVideoView.texture);

        newVideoView.material = profileImage;
        newVideoView.texture = profileTexture;
        float[] itemSize = CalculateVideoItemSize();
        newVideoView.rectTransform.sizeDelta = new Vector2(itemSize[0], itemSize[1]);
        ChangeVideo(userId, false);
    }

    public void ChangeAudioIcon(bool isMuted)
    {
        Button audioBtn = audioBtnObj.GetComponent<Button>();
        if (isMuted)
        {
            audioBtn.image.sprite = unmuteIcon;
        }
        else
        {
            audioBtn.image.sprite = muteIcon;
        }
    }

    public void ChangeVideoIcon(bool isOn)
    {
        Button videoBtn = videoBtnObj.GetComponent<Button>();
        if (isOn)
        {
            videoBtn.image.sprite = videoOffIcon;
        }
        else
        {
            videoBtn.image.sprite = videoOnIcon;
        }
    }

    public void ToggleShareIcon(bool isSharing)
    {
        Button shareBtn = shareBtnObj.GetComponent<Button>();
        if (isSharing)
        {
            shareBtn.image.sprite = shareOffIcon;
        }
        else
        {
            shareBtn.image.sprite = shareOnIcon;
        }
    }

    public void ChangeVideo(string userId, bool isOn)
    {
        RawImage videoView = (RawImage)videoListDic[userId];
        if (videoView == null)
        {
            return;
        }
        if (isOn)
        {
            Material shaderMaterial = shaderDic.GetValueOrDefault(userId, profileImage);
            videoView.material = shaderMaterial;
        }
        else
        {
            videoView.material = profileImage;
            videoView.texture = profileTexture;
        }

        if (String.Equals(userId, mainVideoViewUserId))
        {
            UpdateMainViewVideo(userId);
        }
    }

    public void SetVideoListActive()
    {
        ChangeActiveStatus(joinSessionPageObj, false);
        ChangeActiveStatus(alertWindowObj, false);
        ChangeActiveStatus(toolBarObj, true);
        ChangeActiveStatus(inSessionPageObj, true);

        Button videoBtn = videoBtnObj.GetComponent<Button>();
        Button audioBtn = audioBtnObj.GetComponent<Button>();
        Button shareBtn = shareBtnObj.GetComponent<Button>();
        ZMVideoSDKSession session = ZMVideoSDK.Instance.GetSessionInfo();
        ZMVideoSDKUser myself = session.GetMySelf();
        if (myself.GetAudioStatus().isMuted)
        {
            audioBtn.image.sprite = unmuteIcon;
        }
        else
        {
            audioBtn.image.sprite = muteIcon;
        }

        if (myself.GetVideoPipe().GetVideoStatus().isOn)
        {
            videoBtn.image.sprite = videoOnIcon;
        }
        else
        {
            videoBtn.image.sprite = videoOffIcon;
        }

        shareBtn.image.sprite = shareOnIcon;
    }

    public void ResetPage()
    {
        ChangeActiveStatus(toolBarObj, false);
        ChangeActiveStatus(joinSessionPageObj, true);
        ChangeActiveStatus(inSessionPageObj, false);
        BindGameObject(chatBot, "ChatBot");
        ShowChatBot(false);

        ChatBot chatBotObj = chatBot.GetComponent<ChatBot>();
        chatBotObj.ClearChatPanel();

        mainVideoViewUser = null;
        mainVideoViewUserId = "";
    }

    public bool IsMainViewEmpty()
    {
        return mainVideoViewUser == null;
    }

    public void OnClickChatBot()
    {
        ShowChatBot(!isChatBotVisible);
    }

    private float CalculateButtonXPosition(GameObject gameObject)
    {
        float screenWidth = Screen.safeArea.width;
        return screenWidth / 2 - gameObject.GetComponent<RectTransform>().rect.width - 15;
    }

    private float CalculateLeaveButtonYPosition(GameObject gameObject)
    {
        float screenHeight = Screen.safeArea.height;
        return screenHeight / 2 - gameObject.GetComponent<RectTransform>().rect.height * 2;
    }

    private float CalculateVideoListYPosition(GameObject gameObject)
    {
        float screenHeight = Screen.safeArea.height;
        if (Screen.width > Screen.height)
        {
            return (-6f / 14f) * screenHeight + 70f;
        }
        else
        {
            return (-3f / 8f) * screenHeight + 50f;
        }
    }

    public float[] CalculateMainVideoSize(GameObject gameObject)
    {
        RectTransform objectRect = gameObject.GetComponent<RectTransform>();
        float width = objectRect.sizeDelta.x;
        float height = objectRect.sizeDelta.y;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float[] res = new float[2];

        if (width / height > screenWidth / screenHeight)
        {
            res[0] = screenWidth;
            res[1] = height / width * screenWidth;
        } else
        {
            res[0] = width / height * screenHeight;
            res[1] = screenHeight;
        }

        if (screenHeight < screenWidth)
        {
            res[0] /= 2;
            res[1] /= 2;
        }
        return res;
    }

    private float[] CalculateVideoItemSize()
    {
        float[] itemSize = new float[2];

        if (Screen.width > Screen.height)
        {
            itemSize[0] = Screen.width / 7;
            itemSize[1] = Screen.height / 7;
        }
        else
        {
            itemSize[0] = Screen.width / 4;
            itemSize[1] = Screen.height / 4;
        }

        return itemSize;
    }

    public void UpdateMainViewVideo(string userId)
    {
        BindGameObject(mainVideoViewPrefab, "MainVideoViewPrefab");
        if (mainVideoView == null)
        {
            mainVideoView = mainVideoViewPrefab.GetComponent<RawImage>();
        }

        ZMVideoSDKShareHelper shareHelper = ZMVideoSDK.Instance.GetShareHelper();
        if (mainVideoViewUser == null || !shareHelper.IsOtherUserSharingScreen())
        {
            ZMVideoSDKSession session = ZMVideoSDK.Instance.GetSessionInfo();
            ZMVideoSDKUser myself = session.GetMySelf();
            ZMVideoSDKUser newMainViewUser = myself;
            if (!String.Equals(myself.GetUserID(), userId))
            {
                List<ZMVideoSDKUser> remoteUsers = session.GetRemoteUsers();
                foreach (ZMVideoSDKUser remoteUser in remoteUsers)
                {
                    if (String.Equals(remoteUser.GetUserID(), userId))
                    {
                        newMainViewUser = remoteUser;
                        break;
                    }
                }
            }

            ZMVideoSDKDelegateImpl.SubscribeVideoRawData(newMainViewUser, ZMVideoSDKResolution.ZMVideoSDKResolution_360P);
            if (mainVideoViewUser != null && !String.Equals(mainVideoViewUser.GetUserID(), newMainViewUser.GetUserID()))
            {
                ZMVideoSDKDelegateImpl.SubscribeVideoRawData(mainVideoViewUser, ZMVideoSDKResolution.ZMVideoSDKResolution_90P);
            }

            RawImage userVideo = (RawImage)videoListDic[userId];
            float[] videoSize = CalculateMainVideoSize(userVideo.gameObject);
            mainVideoView.rectTransform.sizeDelta = new Vector2(videoSize[0], videoSize[1]);
            mainVideoView.material = userVideo.material;
            mainVideoView.texture = userVideo.texture;
            mainVideoViewUser = newMainViewUser;
            mainVideoViewUserId = newMainViewUser.GetUserID();
        }
    }

    public void SetSessionIsEnd(bool isEnded)
    {
        isSessionEnded = isEnded;
    }

    private void ChangeActiveStatus(GameObject gameObject, bool isActive)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(isActive);
        }
    }

    private void BindGameObject(GameObject gameObject, string name)
    {
        if (gameObject == null)
        {
            gameObject = GameObject.Find(name);
        }
    }

    private void CheckPermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.Log("Camera has not been enabled");
            Permission.RequestUserPermission(Permission.Camera);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Debug.Log("Microphone has not been enabled");
            Permission.RequestUserPermission(Permission.Microphone);
        }
    }

    private void ShowChatBot(bool show)
    {
        BindGameObject(chatBot, "ChatBot");
        ChangeActiveStatus(chatBot, show);
        isChatBotVisible = show;
    }
}