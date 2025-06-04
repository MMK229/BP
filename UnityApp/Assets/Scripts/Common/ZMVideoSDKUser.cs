

using System;

/**
    @brief Zoom Video SDK user information.
 */
public class ZMVideoSDKUser
{
#if UNITY_STANDALONE_OSX
        private MacZMVideoSDKUser _videoSDKUser = null;
        public ZMVideoSDKUser(MacZMVideoSDKUser videoSDKUser)
        {
            _videoSDKUser = videoSDKUser;
        }
        public MacZMVideoSDKUser VideoSDKUser
        {
            get { return _videoSDKUser; }
        }
#elif UNITY_IOS
        private IOSZMVideoSDKUser _videoSDKUser = null;
        public ZMVideoSDKUser(IOSZMVideoSDKUser videoSDKUser)
        {
            _videoSDKUser = videoSDKUser;
        }
        public IOSZMVideoSDKUser VideoSDKUser
        {
            get { return _videoSDKUser; }
        }
#elif UNITY_ANDROID
    private AndroidZoomVideoSDKUser _videoSDKUser = null;
    public ZMVideoSDKUser(AndroidZoomVideoSDKUser videoSDKUser)
    {
        _videoSDKUser = videoSDKUser;
    }
    public AndroidZoomVideoSDKUser VideoSDKUser
    {
        get { return _videoSDKUser; }
    }
#elif UNITY_STANDALONE_WIN
    private WindowsZoomVideoSDKUser _videoSDKUser = null;
    public ZMVideoSDKUser(WindowsZoomVideoSDKUser videoSDKUser)
    {
        _videoSDKUser = videoSDKUser;
    }
    public WindowsZoomVideoSDKUser VideoSDKUser
    {
        get { return _videoSDKUser; }
    }
#endif

    /**
        @brief Get the user's id.
        @return The user id of the user object.
    */
    public string GetUserID()
    {
        return _videoSDKUser.GetUserID();
    }

    /**
        @brief Get the user's custom id.
        @return The Custom id of the user object.
    */
    public string GetCustomIdentity()
    {
        return _videoSDKUser.GetCustomIdentity();
    }

    /**
        @brief Get the user's name.
        @return The name of the user object.
    */
    public string GetUserName()
    {
        return _videoSDKUser.GetUserName();
    }

    /**
        @brief Get the user's audio status.
        @return Audio status of the user object.
    */
    public ZMVideoSDKAudioStatus GetAudioStatus()
    {
        return _videoSDKUser.GetAudioStatus();
    }

    /**
         @brief Determine whether the user is the host.
        @return True indicates that the user is the host, otherwise false.
    */
    public bool IsHost()
    {
        return _videoSDKUser.IsHost();
    }

    /**
        @brief Determine whether the user is the manager(coHost)
        @return True indicates that the user is the manager(coHost), otherwise false.
    */
    public bool IsManager()
    {
        return _videoSDKUser.IsManager();
    }

    /**
        @brief Get the user's video statistic information.
        @return The video statistic information. For more information, see @link ZMVideoSDKVideoStatisticInfo @endlink
    */
    public ZMVideoSDKVideoStatisticInfo GetVideoStatisticInfo()
    {
        return _videoSDKUser.GetVideoStatisticInfo();
    }

    /**
        @brief Get the user's video raw data pipe.
        @return The video pipe. For more information, see @link ZMVideoSDKRawDataPipe @endlink
    */
    public ZMVideoSDKRawDataPipe GetVideoPipe()
    {
        ZMVideoSDKRawDataPipe pipe = _videoSDKUser.GetVideoPipe();
        if (pipe == null)
        {
            throw new NullReferenceException("GetVideoPipe returns null");
        }
        return pipe;
    }
    /**
        @brief Set the user's local volume. This does not affect how other participants hear the user.
        @param volume The value can be >= 0 and <=10. If volume is 0, you won't be able to hear the related audio.
        @param isSharingAudio If true, sets the volume of shared audio(such as shared computer audio), otherwise sets the volume of microphone.
        @return If success return true, otherwise false.
    */
    public bool SetUserVolume(float volume, bool isSharingAudio)
    {
        return _videoSDKUser.SetUserVolume(volume, isSharingAudio);
    }

    /**
        @brief Get user volume.
        @param isSharingAudio If true, gets the volume of shared audio(such as shared computer audio), otherwise gets the volume of the microphone.
        @return return the volume of the user.
    */
    public float GetUserVolume(bool isSharingAudio)
    {
        return _videoSDKUser.GetUserVolume(isSharingAudio);
    }

    /**
        @brief Determine which audio you can set, shared audio or microphone.
        @param isShareAudio If true, checks whether you can set the volume of shared audio, otherwise you can set the volume of the microphone.
        @return return true if the volume of the user can be set, otherwise false.
    */
    public bool CanSetUserVolume(bool isShareAudio)
    {
        return _videoSDKUser.CanSetUserVolume(isShareAudio);
    }

    /**
        @brief Used to determine whether I agree to individual video recording.
        @return If agreed return true, otherwise false.
    */
    public bool HasIndividualRecordingConsent()
    {
        return _videoSDKUser.HasIndividualRecordingConsent();
    }

    /**
     * Get the user's share pipe.
     *
     * @return share pipe. For more information, see {@link ZoomVideoSDKRawDataPipe}.
     */
    public ZMVideoSDKRawDataPipe GetSharePipe()
    {
        ZMVideoSDKRawDataPipe pipe = _videoSDKUser.GetSharePipe();
        if (pipe == null)
        {
            throw new NullReferenceException("GetSharePipe returns null");
        }
        return pipe;
    }
}