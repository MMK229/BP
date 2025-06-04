
/**
    @brief User control interface.
    See {@link ZMVideoSDK#GetUserHelper()}
 */
public class ZMVideoSDKUserHelper
{
#if UNITY_STANDALONE_OSX
        private MacZMVideoSDKUserHelper _videoSDKUserHelper = null;
        public ZMVideoSDKUserHelper(MacZMVideoSDKUserHelper videoSDKUserHelper)
        {
            _videoSDKUserHelper = videoSDKUserHelper;
        }
#elif UNITY_IOS
        private IOSZMVideoSDKUserHelper _videoSDKUserHelper = null;
        public ZMVideoSDKUserHelper(IOSZMVideoSDKUserHelper videoSDKUserHelper)
        {
            _videoSDKUserHelper = videoSDKUserHelper;
        }
#elif UNITY_ANDROID
    private AndroidZoomVideoSDKUserHelper _videoSDKUserHelper = null;
    public ZMVideoSDKUserHelper(AndroidZoomVideoSDKUserHelper videoSDKUserHelper)
    {
        _videoSDKUserHelper = videoSDKUserHelper;
    }
#elif UNITY_STANDALONE_WIN
    private WindowsZoomVideoSDKUserHelper _videoSDKUserHelper = null;
    public ZMVideoSDKUserHelper(WindowsZoomVideoSDKUserHelper videoSDKUserHelper)
    {
        _videoSDKUserHelper = videoSDKUserHelper;
    }
#endif

    /**
        @brief Change a specific user's name.
	    @param name The new name of the user object.
	    @param zmVideoSDKUser User in the session
	    @return True indicates that name change is success. Otherwise, this function returns false.
    */
    public bool ChangeName(string name, ZMVideoSDKUser zmVideoSDKUser)
    {
        return _videoSDKUserHelper.ChangeName(name, zmVideoSDKUser);
    }

    /**
        @brief Assign a user as the session host.
	    @param zmVideoSDKUser User in the session
	    @return True indicates that the user is now the host. Otherwise, this function returns false.
    */
    public bool MakeHost(ZMVideoSDKUser zmVideoSDKUser)
    {
        return _videoSDKUserHelper.MakeHost(zmVideoSDKUser);
    }

    /**
        @brief Assign a user as the session manager.
	    @param zmVideoSDKUser User in the session.
	    @return True indicates that the user is now the manager. Otherwise, this function returns false.
    */
    public bool MakeManager(ZMVideoSDKUser zmVideoSDKUser)
    {
        return _videoSDKUserHelper.MakeManager(zmVideoSDKUser);
    }

    /**
        @brief Revoke manager rights from a user.
	    @param zmVideoSDKUser User in the session.
	    @return True indicates that the user no longer has manager rights. Otherwise, this function returns false.
    */
    public bool RevokeManager(ZMVideoSDKUser zmVideoSDKUser)
    {
        return _videoSDKUserHelper.RevokeManager(zmVideoSDKUser);
    }

    /**
        @brief Remove user from session.
	    @param zmVideoSDKUser User in the session.
	    @return True indicates that remove user is success. Otherwise, this function returns false.
    */
    public bool RemoveUser(ZMVideoSDKUser zmVideoSDKUser)
    {
        return _videoSDKUserHelper.RemoveUser(zmVideoSDKUser);
    }

}