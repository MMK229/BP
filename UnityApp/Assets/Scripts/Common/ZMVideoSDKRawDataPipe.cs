
/**
    @brief Interface for user to subscribe@unSubscribe video raw data.
    {@link ZMVideoSDKUser#GetVideoPipe()}
 */
public class ZMVideoSDKRawDataPipe
{

#if UNITY_STANDALONE_OSX
    private MacZMVideoSDKRawDataPipe _rawDataPipe = null;
    public ZMVideoSDKRawDataPipe(MacZMVideoSDKRawDataPipe rawDataPipe)
    {
        _rawDataPipe = rawDataPipe;
    }
    public MacZMVideoSDKRawDataPipe VideoSDKRawDataPipe
    {
        get { return _rawDataPipe; }
    }
#elif UNITY_IOS
    private IOSZMVideoSDKRawDataPipe _rawDataPipe = null;
    public ZMVideoSDKRawDataPipe(IOSZMVideoSDKRawDataPipe rawDataPipe)
    {
        _rawDataPipe = rawDataPipe;
    }
    public IOSZMVideoSDKRawDataPipe VideoSDKRawDataPipe
    {
        get { return _rawDataPipe; }
    }

#elif UNITY_ANDROID
    private AndroidZoomVideoSDKRawDataPipe _rawDataPipe = null;
    public ZMVideoSDKRawDataPipe(AndroidZoomVideoSDKRawDataPipe rawDataPipe)
    {
        _rawDataPipe = rawDataPipe;
    }

    public AndroidZoomVideoSDKRawDataPipe VideoSDKRawDataPipe
    {
        get { return _rawDataPipe; }
    }
#elif UNITY_STANDALONE_WIN
    private WindowsZoomVideoSDKRawDataPipe _rawDataPipe = null;
    public ZMVideoSDKRawDataPipe(WindowsZoomVideoSDKRawDataPipe rawDataPipe)
    {
        _rawDataPipe = rawDataPipe;
    }

    public WindowsZoomVideoSDKRawDataPipe VideoSDKRawDataPipe
    {
        get { return _rawDataPipe; }
    }
#endif

    /**
        @brief Subscribe video@share.
        @param resolution Subscribe size.
        @param dataDelegate Callback sink object.
        @return If the function succeeds, the return value is ZMVideoSDKErrors_Success.
        Otherwise failed. To get extended error information, see @link ZMVideoSDKErrors @endlink enum.
    */
    public ZMVideoSDKErrors Subscribe(ZMVideoSDKResolution resolution, IZMVideoSDKRawDataPipeDelegate dataDelegate)
    {
        return _rawDataPipe.Subscribe(resolution, dataDelegate);
    }

    /**
        @brief Unsubscribe video@share.
        @return If the function succeeds, the return value is ZMVideoSDKErrors_Success.
        Otherwise failed. To get extended error information, see @link ZMVideoSDKErrors @endlink enum.
    */
    public ZMVideoSDKErrors UnSubscribe()
    {
        return _rawDataPipe.UnSubscribe();
    }

    /**
        @brief Get the raw data data type. 
        @return Share or Video data type, see @link ZMVideoSDKRawDataType @endlink enum.
    */
    public ZMVideoSDKRawDataType GetRawdataType()
    {
        return _rawDataPipe.GetRawdataType();
    }

    /**
        @brief Get video status.
        @return Video status of the user object.
    */
    public ZMVideoSDKVideoStatus GetVideoStatus()
    {
        return _rawDataPipe.GetVideoStatus();
    }

    /**
        @brief Get share status.
        @return Share status of the user object.
    */
    public ZMVideoSDKShareStatus GetShareStatus()
    {
        return _rawDataPipe.GetShareStatus();
    }
}

