using System;
using System.Collections.Generic;

/**
    @brief Audio control interface
    See{@link ZMVideoSDK#GetAudioHelper()}
 */
public class ZMVideoSDKAudioHelper
{

#if UNITY_STANDALONE_OSX
        private MacZMVideoSDKAudioHelper _videoSDKAudioHelper = null;
        public ZMVideoSDKAudioHelper(MacZMVideoSDKAudioHelper videoSDKAudioHelper)
        {
            _videoSDKAudioHelper = videoSDKAudioHelper;
        }
#elif UNITY_IOS
        private IOSZMVideoSDKAudioHelper _videoSDKAudioHelper = null;
        public ZMVideoSDKAudioHelper(IOSZMVideoSDKAudioHelper videoSDKAudioHelper)
        {
            _videoSDKAudioHelper = videoSDKAudioHelper;
        }
#elif UNITY_ANDROID
    private AndroidZoomVideoSDKAudioHelper _videoSDKAudioHelper = null;
    public ZMVideoSDKAudioHelper(AndroidZoomVideoSDKAudioHelper audioHelper)
    {
        _videoSDKAudioHelper = audioHelper;
    }

    public AndroidZoomVideoSDKAudioHelper VideoSDKAudioHelper
    {
        get { return _videoSDKAudioHelper; }
    }
#elif UNITY_STANDALONE_WIN
    private WindowsZoomVideoSDKAudioHelper _videoSDKAudioHelper = null;
    public ZMVideoSDKAudioHelper(WindowsZoomVideoSDKAudioHelper videoSDKAudioHelper)
    {
        _videoSDKAudioHelper = videoSDKAudioHelper;
    }
#endif

    /**
        @brief Start audio with voip.
        @param [out] callback a callback function to return the @link ZMVideoSDKErrors @endlink status code of this method.
    */
    public void StartAudio(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKAudioHelper.StartAudio(callback);
    }

    /**
        @brief Stop voip.
        @param [out] callback a callback function to return the @link ZMVideoSDKErrors @endlink status code of this method.
    */
    public void StopAudio(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKAudioHelper.StopAudio(callback);
    }

    /**
        @brief Unmute user's voip audio.
        @param user The user which you want to unMute.
        @param [out] callback a callback function to return the @link ZMVideoSDKErrors @endlink status code of this method.
    */
    public void UnMuteAudio(ZMVideoSDKUser user, Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKAudioHelper.UnMuteAudio(user, callback);
    }

    /**
        @brief Mute user's voip audio. 0 means current user (myself).
        @param user The user which you want to mute.
	    @return If the function succeeds, the return value is ZoomVideoSDKErrors_Success.
        Otherwise failed. To get extended error information, see @link ZMVideoSDKErrors @endlink enum.
    */
    public void MuteAudio(ZMVideoSDKUser user, Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKAudioHelper.MuteAudio(user, callback);
    }

    /**
        @brief Get speaker device list.
        @return If the function succeeds, the return value is @link ZMVideoSDKSpeakerDevice @endlink speaker device list, Otherwise NULL.
    */

    public List<ZMVideoSDKSpeakerDevice> GetSpeakerList()
    {
        return _videoSDKAudioHelper.GetSpeakerList();
    }

    /**
        @brief Get mic device list.
        @return If the function succeeds, the return value is @link ZMVideoSDKMicDevice @endlink  mic device list, Otherwise NULL.
    */

    public List<ZMVideoSDKMicDevice> GetMicList()
    {
        return _videoSDKAudioHelper.GetMicList();
    }

    /**
        @brief Select a speaker device as default device.
        @param deviceID Device id.
        @param deviceName Device name.
        @return If the function succeeds, the return value is ZMVideoSDKErrors_Success.
        Otherwise failed. To get extended error information, see @link ZMVideoSDKErrors @endlink enum.
    */
    public ZMVideoSDKErrors SelectSpeaker(string deviceID, string deviceName)
    {
        return _videoSDKAudioHelper.SelectSpeaker(deviceID, deviceName);
    }

    /**
        @brief Select a microphone device as default device.
        @param deviceID Device id.
        @param deviceName Device name.
        @return If the function succeeds, the return value is ZMVideoSDKErrors_Success.
        Otherwise failed. To get extended error information, see @link ZMVideoSDKErrors @endlink enum.
    */
    public ZMVideoSDKErrors SelectMic(string deviceID, string deviceName)
    {
        return _videoSDKAudioHelper.SelectMic(deviceID, deviceName);
    }

    /**
        @brief Subscribe audio raw data.
        @param [out] callback a callback function to return the @link ZMVideoSDKErrors @endlink status code of this method.
    */
    public void Subscribe(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKAudioHelper.Subscribe(callback);
    }

    /**
        @brief UnSubscribe audio raw data.
        @param [out] callback a callback function to return the @link ZMVideoSDKErrors @endlink status code of this method.
    */
    public void UnSubscribe(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKAudioHelper.UnSubscribe(callback);
    }

}