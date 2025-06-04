using System.Collections.Generic;

/**
    @brief Session instance object
    See {@link ZMVideoSDK#JoinSession(ZMVideoSDKSessionContext)}
 */
public class ZMVideoSDKSession
{
#if UNITY_STANDALONE_OSX
        private MacZMVideoSDKSession _videoSDKSession = null;
        public ZMVideoSDKSession(MacZMVideoSDKSession videoSDKSession)
        {
            _videoSDKSession = videoSDKSession;
        }
#elif UNITY_IOS
        private IOSZMVideoSDKSession _videoSDKSession = null;
        public ZMVideoSDKSession(IOSZMVideoSDKSession videoSDKSession)
        {
            _videoSDKSession = videoSDKSession;
        }
#elif UNITY_ANDROID
    private AndroidZoomVideoSDKSession _videoSDKSession = null;
    public ZMVideoSDKSession(AndroidZoomVideoSDKSession videoSDKSession)
    {
        _videoSDKSession = videoSDKSession;
    }
#elif UNITY_STANDALONE_WIN
    private WindowsZoomVideoSDKSession _videoSDKSession = null;
    public ZMVideoSDKSession(WindowsZoomVideoSDKSession videoSDKSession)
    {
        _videoSDKSession = videoSDKSession;
    }
#endif
    /**
        @brief Get the current session number.
        @return If the function succeeds, the return value is the current session number. Otherwise returns ZERO(0).
    */

    public ulong GetSessionNumber()
    {
        return _videoSDKSession.GetSessionNumber();
    }

    /**
        @brief Get the session name.
        @return If the function succeeds, the return value is session name. Otherwise returns NULL.
    */
    public string GetSessionName()
    {
        return _videoSDKSession.GetSessionName();
    }

    /**
        @brief Get the session's password.
        @return If the function succeeds, the return value is session password. Otherwise returns NULL.
    */
    public string GetSessionPassword()
    {
        return _videoSDKSession.GetSessionPassword();
    }

    /**
        @brief Get the session phone passcode.
        @return If the function succeeds, the return value is session phone passcode. Otherwise returns NULL.
    */
    public string GetSessionPhonePasscode()
    {
        return _videoSDKSession.GetSessionPhonePasscode();
    }

    /**
        @brief Get the session ID.
        @return If the function succeeds, the return value is session ID. Otherwise returns NULL.
        @remarks This interface is only valid for the host.
    */
    public string GetSessionID()
    {
        return _videoSDKSession.GetSessionID();
    }

    /**
        @brief Get the host's name.
        @return If the function succeeds, the return value is session host name. Otherwise returns NULL.
    */
    public string GetSessionHostName()
    {
        return _videoSDKSession.GetSessionHostName();
    }

    /**
        @brief Get the session's host user object.
        @return If the function succeeds, the return value is session host user object. Otherwise returns NULL.
    */
    public ZMVideoSDKUser GetSessionHost()
    {
        return _videoSDKSession.GetSessionHost();
    }

    /**
        @brief Get a list of the session's remote users.
        @return If the function succeeds, the return value is remote users list. Otherwise returns NULL.
    */
    public List<ZMVideoSDKUser> GetRemoteUsers()
    {
        return _videoSDKSession.GetRemoteUsers();
    }

    /**
        @brief Get the session's user object for myself.
        @return If the function succeeds, the return value is myself object. Otherwise returns NULL.
    */
    public ZMVideoSDKUser GetMySelf()
    {
        return _videoSDKSession.GetMySelf();
    }

    /**
        @brief Get session's audio statistic information.
        @return If the function succeeds, the return value is audio statistic info object. Otherwise returns NULL.
    */
    public ZMVideoSDKSessionAudioStatisticInfo GetSessionAudioStatisticInfo()
    {
        return _videoSDKSession.GetSessionAudioStatisticInfo();
    }

    /**
        @brief Get session's Video statistic information.
        @return If the function succeeds, the return value is session video statistic info object. Otherwise returns NULL.
    */
    public ZMVideoSDKSessionASVStatisticInfo GetSessionVideoStatisticInfo()
    {
        return _videoSDKSession.GetSessionVideoStatisticInfo();
    }

    /**
        @brief Get session's Video statistic information.
        @return If the function succeeds, the return value is session share statistic info object. Otherwise returns NULL.
    */
    public ZMVideoSDKSessionASVStatisticInfo GetSessionShareStatisticInfo()
    {
        return _videoSDKSession.GetSessionShareStatisticInfo();
    }
}