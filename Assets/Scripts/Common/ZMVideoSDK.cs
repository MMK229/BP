using System;
using UnityEditor;

/**
    @brief Zoom Video SDK API manager. Main singleton object that controls the video session creation, event callbacks and other main features of video SDK.
    {@link ZoomVideoSDK#initialize(Context, ZoomVideoSDKInitParams)}
 */
public class ZMVideoSDK
{
    private static readonly ZMVideoSDK instance = new ZMVideoSDK();
#if UNITY_STANDALONE_OSX
        private MacZMVideoSDK _videoSDK = null;
#elif UNITY_ANDROID
    private AndroidZoomVideoSDK _videoSDK = null;
#elif UNITY_IOS
    private IOSZMVideoSDK _videoSDK = null;
#elif UNITY_STANDALONE_WIN
    private WindowsZoomVideoSDK _videoSDK = null;
#endif

    static ZMVideoSDK()
    {
    }

    private ZMVideoSDK()
    {
#if UNITY_STANDALONE_OSX
        _videoSDK = MacZMVideoSDK.Instance;
#elif UNITY_ANDROID
        _videoSDK = AndroidZoomVideoSDK.Instance;
#elif UNITY_IOS
        _videoSDK = IOSZMVideoSDK.Instance;
#elif UNITY_STANDALONE_WIN
        _videoSDK = WindowsZoomVideoSDK.Instance;
#else
        Debug.Log("Platform not supported.");
#endif
    }
    public static ZMVideoSDK Instance
    {
        get
        {
            return instance;
        }
    }

    /**
        @brief Initialize the Zoom Video SDK with the appropriate parameters in the @link ZMVideoSDKInitParams @endlink structure.
        @param [out] zmVideoSDKInitParams Parameters for init zoom video sdk. For more details, see @link ZMVideoSDKInitParams @endlink structure.
        @param [out] callback a callback function to return the @link ZMVideoSDKErrors @endlink status code of this method.
    */
    public void Initialize(ZMVideoSDKInitParams zmVideoSDKInitParams, Action<ZMVideoSDKErrors> callback)
    {
        _videoSDK.Initialize(zmVideoSDKInitParams, callback);
    }
    /**
        @brief Clean up ZOOM Video SDK.
        @param [out] callback a callback function to return the @link ZMVideoSDKErrors @endlink status code of this method.
    */
    public void CleanUp(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDK.Cleanup(callback);
    }

    /**
        @brief Add a listener for session events
        @param  videoSDKDelegate A listener class that groups together all the callbacks related to a session.
    */
    public void AddListener(IZMVideoSDKDelegate videoSDKDelegate)
    {
        _videoSDK.AddListener(videoSDKDelegate);
    }

    /**
        @brief Remove a listener for session events
        @param videoSDKDelegate A listener class that groups together all the callbacks related to a session.
    */
    public void RemoveListener(IZMVideoSDKDelegate videoSDKDelegate)
    {
        _videoSDK.RemoveListener(videoSDKDelegate);
    }

    /**
        @brief Call this method to join a session with the appropriate @link ZMVideoSDKSessionContext @endlink parameters. 
	    When seccessful, the SDK will attempt to join a session. Use the callbacks in the listener to confirm whether the SDK actually joined.
	    @param [out] zmVideoSDKSessionContext The parameter is used to join session. For more details, see @link ZMVideoSDKSessionContext @endlink structure. 
	    @param [out] callback a callback function to return the @link ZMVideoSDKErrors @endlink status code of this method.
    */
    public void JoinSession(ZMVideoSDKSessionContext zmVideoSDKSessionContext, Action<ZMVideoSDKErrors> callback)
    {
        _videoSDK.JoinSession(zmVideoSDKSessionContext, callback);
    }

    /**
        @brief Call this method to leave a session previously joined through joinSession method call.
	    When successful, the SDK will attempt to leave a session. Use the callbacks in the listener to confirm whether the SDK actually left.
        @param [out]  end True if the host should end the entire session, or False if the host should just leave the session. 
        @param [out] callback a callback function to return the @link ZMVideoSDKErrors @endlink status code of this method. 
    */
    public void LeaveSession(bool end, Action<ZMVideoSDKErrors> callback)
    {
        _videoSDK.LeaveSession(end, callback);
    }

    /**
        @brief Returns the current session information.
	    @return If the function succeeds, the return value is the pointer to @link ZMVideoSDKSession @endlink object.
	    Otherwise NULL.
    */
    public ZMVideoSDKSession GetSessionInfo()
    {
        ZMVideoSDKSession info = _videoSDK.GetSessionInfo();
        if (info == null)
        {
            throw new NullReferenceException("GetSessionInfo returns null");
        }
        return info;
    }

    /**
        @brief Check if there is an active session between participants.
        @return True if there is; False if not
    */
    public bool IsInSession()
    {
        return _videoSDK.IsInSession();
    }

    /**
        @brief Returns the sdk version
        @return If the function succeeds, the return value is sdk version. Otherwise returns NULL.
    */
    public string GetSDKVersion()
    {
        return _videoSDK.GetSdkVersion();
    }

    /**
        @brief Returns an instance to manage audio controls related to the current video SDK session.
        @return If the function succeeds, the return value is the audio helper object. Otherwise returns NULL. For more details, see @link ZMVideoSDKAudioHelper @endlink.
    */
    public ZMVideoSDKAudioHelper GetAudioHelper()
    {
        ZMVideoSDKAudioHelper helper = _videoSDK.GetAudioHelper();
        if (helper == null)
        {
            throw new NullReferenceException("GetAudioHelper returns null");
        }
        return helper;
    }

    /**
        @brief Returns an instance to manage cameras and video during a video SDK session.
        @return If the function succeeds, the return value is the video helper object. Otherwise returns NULL. For more details, see @link ZMVideoSDKVideoHelper @endlink.
   */
    public ZMVideoSDKVideoHelper GetVideoHelper()
    {
        ZMVideoSDKVideoHelper helper = _videoSDK.GetVideoHelper();
        if (helper == null)
        {
            throw new NullReferenceException("GetVideoHelper returns null");
        }
        return helper;
    }

    /**
        @brief Returns an instance to manage cameras and video during a video SDK session.
	    @return If the function succeeds, the return value is the video helper object. Otherwise returns NULL. For more details, see @link ZMVideoSDKUserHelper @endlink.
    */
    public ZMVideoSDKUserHelper GetUserHelper()
    {
        ZMVideoSDKUserHelper helper = _videoSDK.GetUserHelper();
        if (helper == null)
        {
            throw new NullReferenceException("GetUserHelper returns null");
        }
        return helper;
    }

    /**
        @brief Returns an instance to send and receive chat messages within video SDK session participants.
	    @return If the function succeeds, the return value is the chat helper object. Otherwise returns NULL. For more details, see @link ZMVideoSDKChatHelper @endlink.
     */
    public ZMVideoSDKChatHelper GetChatHelper()
    {
        ZMVideoSDKChatHelper helper = _videoSDK.GetChatHelper();
        if (helper == null)
        {
            throw new NullReferenceException("GetChatHelper returns null");
        }
        return helper;
    }

    /**
     * Returns an instance to manage cloud recordings during a video SDK session.
     *
     * @return A {@link ZMVideoSDKRecordingHelper} instance.
     */
    public ZMVideoSDKRecordingHelper GetRecordingHelper()
    {
        ZMVideoSDKRecordingHelper helper = _videoSDK.GetRecordingHelper();
        if (helper == null)
        {
            throw new NullReferenceException("GetRecordingHelper returns null");
        }
        return helper;
    }

    /** Returns an instance to manage screen sharing during a video SDK session.
     *
     * @return {@link ZoomVideoSDKShareHelper }
     */
    public ZMVideoSDKShareHelper GetShareHelper()
    {
        ZMVideoSDKShareHelper helper = _videoSDK.GetShareHelper();
        if (helper == null)
        {
            throw new NullReferenceException("GetShareHelper returns null");
        }
        return helper;
    }
}