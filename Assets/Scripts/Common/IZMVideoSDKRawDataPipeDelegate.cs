
/**
    @brief Video raw data callback
 */
public interface IZMVideoSDKRawDataPipeDelegate
{
    /**
        @brief Video raw data callback
        @param rawData see @link ZMVideoSDKVideoRawData @endlink structure.
     */
    void onRawDataFrameReceived(ZMVideoSDKVideoRawData rawDataObject);


    /**
        @brief Subscribe source video/share status changed.
        @param status raw data status, see @link ZMVideoSDKRawDataStatus @endlink structure.
     */
    void onRawDataStatusChanged(ZMVideoSDKRawDataStatus status);


}