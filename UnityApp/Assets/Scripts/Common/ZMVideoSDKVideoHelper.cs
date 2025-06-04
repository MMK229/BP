using System;
using System.Collections.Generic;
using UnityEngine;

/**
    @brief An interface to control video and manage cameras during a video session.
    See {@link ZMVideoSDK#GetVideoHelper()}
 */
public class ZMVideoSDKVideoHelper
{

#if UNITY_STANDALONE_OSX
        private MacZMVideoSDKVideoHelper _videoSDKVideoHelper = null;
        public ZMVideoSDKVideoHelper(MacZMVideoSDKVideoHelper videoSDKVideoHelper)
        {
            _videoSDKVideoHelper = videoSDKVideoHelper;
        }
#elif UNITY_IOS
        private IOSZMVideoSDKVideoHelper _videoSDKVideoHelper = null;
        public ZMVideoSDKVideoHelper(IOSZMVideoSDKVideoHelper videoSDKVideoHelper)
        {
            _videoSDKVideoHelper = videoSDKVideoHelper;
        }
#elif UNITY_ANDROID
    private AndroidZoomVideoSDKVideoHelper _videoSDKVideoHelper = null;
    public ZMVideoSDKVideoHelper(AndroidJavaObject videoSDKVideoHelper)
    {
        AndroidZoomVideoSDKVideoHelper androidVideoHelper = new AndroidZoomVideoSDKVideoHelper(videoSDKVideoHelper);
        _videoSDKVideoHelper = androidVideoHelper;
    }

    public AndroidZoomVideoSDKVideoHelper VideoSDKVideoHelper
    {
        get { return _videoSDKVideoHelper; }
    }
#elif UNITY_STANDALONE_WIN
    private WindowsZoomVideoSDKVideoHelper _videoSDKVideoHelper = null;
    public ZMVideoSDKVideoHelper(WindowsZoomVideoSDKVideoHelper videoSDKVideoHelper)
    {
        _videoSDKVideoHelper = videoSDKVideoHelper;
    }
#endif

    /**
        @brief Call this method to start sending local video data from the camera.
        @param [out] callback a callback function to return the @link ZMVideoSDKErrors @endlink status code of this method.
    */
    public void StartVideo(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKVideoHelper.StartVideo(callback);
    }

    /**
        @brief Call this method to stop sending local video data from the camera.
        @param [out] callback a callback function to return the @link ZMVideoSDKErrors @endlink status code of this method.
    */
    public void StopVideo(Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKVideoHelper.StopVideo(callback);
    }

    /**
        @brief Call this method to rotate the video when the device is rotated.
        @param rotation the rotation of the video. 0: no rotation (natural orientation); 1: rotate 90 degrees; 2: rotate 180 degrees; 3: rotate 270 degrees.
        @param [out] callback a callback function to return the @link ZMVideoSDKErrors @endlink status code of this method.
	*/
    public void RotateMyVideo(ZMVideoRotation rotation, Action<bool> callback)
    {
        _videoSDKVideoHelper.RotateMyVideo(rotation, callback);
    }

    /**
        @brief Switch to the next available camera.
        @param [out] callback a callback function to return true if the function succeeds, otherwise false.
	*/
    public void SwitchCamera(Action<bool> callback)
    {
        _videoSDKVideoHelper.SwitchCamera(callback);
    }

    /**
        @brief Returns number of cameras available to share the video.
        @return Number of cameras.
    */
    public uint GetNumberOfCameras()
    {
        return _videoSDKVideoHelper.GetNumberOfCameras();
    }

    /**
        @brief Returns a collection of camera devices available to share the video as an object of type WebCamDevice
        @return Camera devices list.
    */
    public List<ZMVideoSDKCameraDevice> GetCameraList()
    {
        return _videoSDKVideoHelper.GetCameraList();
    }

    /**
        @brief Check whether the current user has permission to control the camera.	
        @param deviceID The camera device ID to check. The default is the main camera ID.
        @return Return a tuple, if the function succeeds, return ZMVideoSDKErrors_Success,
        otherwise it fails. To get extended error information, see @link ZMVideoSDKErrors @endlink enum;
                Return true if the user's camera can be controled, otherwise false.
    */
    public (ZMVideoSDKErrors, bool) CanControlCamera(string deviceID)
    {
        return _videoSDKVideoHelper.CanControlCamera(deviceID);
    }

    /**
        @brief Pan the camera to the left.
        @param range Rotation range, 10 <= range <= 100.
        @param deviceID The camera device ID to rotate. The default is the main camera ID.
        @return If the function succeeds, the return value is ZMVideoSDKErrors_Success.
        Otherwise it fails. To get extended error information, see @link ZMVideoSDKErrors @endlink enum.
    */
    public ZMVideoSDKErrors TurnCameraLeft(uint range, string deviceID)
    {
        return _videoSDKVideoHelper.TurnCameraLeft(range, deviceID);
    }

    /**
        @brief Pan the camera to the right.
        @param range Rotation range, 10 <= range <= 100.
        @param deviceID The camera device ID to rotate.The default is the main camera ID.
        @return If the function succeeds, the return value is ZMVideoSDKErrors_Success.
        Otherwise failed. To get extended error information, see @link ZMVideoSDKErrors @endlink enum.
    */
    public ZMVideoSDKErrors TurnCameraRight(uint range, string deviceID)
    {
        return _videoSDKVideoHelper.TurnCameraRight(range, deviceID);
    }

    /**
        @brief Tilt the camera up.
        @param range Rotation range, 10 <= range <= 100.
        @param deviceID The camera device ID to operate.The default is main camera ID.
        @return If the function succeeds, the return value is ZMVideoSDKErrors_Success.
        Otherwise it fails. To get extended error information, see @link ZMVideoSDKErrors @endlink enum.
    */
    public ZMVideoSDKErrors TurnCameraUp(uint range, string deviceID)
    {
        return _videoSDKVideoHelper.TurnCameraUp(range, deviceID);
    }

    /**
        @brief Tilt the camera down.
        @param range Rotation range, 10 <= range <= 100.
        @param deviceID The camera device ID to operate.The default is the main camera ID.
        @return If the function succeeds, the return value is ZMVideoSDKErrors_Success.
        Otherwise it fails. To get extended error information, see @link ZMVideoSDKErrors @endlink enum.
    */
    public ZMVideoSDKErrors TurnCameraDown(uint range, string deviceID)
    {
        return _videoSDKVideoHelper.TurnCameraDown(range, deviceID);
    }

    /**
        @brief Zoom the camera in.
        @param range Zoom range, 10 <= range <= 100.
        @param deviceID The camera device ID to operate.The default is the main camera ID.
        @return If the function succeeds, the return value is ZMVideoSDKErrors_Success.
        Otherwise it fails. To get extended error information, see @link ZMVideoSDKErrors @endlink enum.
    */
    public ZMVideoSDKErrors ZoomCameraIn(uint range, string deviceID)
    {
        return _videoSDKVideoHelper.ZoomCameraIn(range, deviceID);
    }

    /**
        @brief Zoom the camera out.
        @param range Zoom range, 10 <= range <= 100.
        @param deviceID The camera device ID to operate.The default is the main camera ID.
        @return If the function succeeds, the return value is ZMVideoSDKErrors_Success.
        Otherwise it fails. To get extended error information, see @link ZMVideoSDKErrors @endlink enum.
    */
    public ZMVideoSDKErrors ZoomCameraOut(uint range, string deviceID)
    {
        return _videoSDKVideoHelper.ZoomCameraOut(range, deviceID);
    }

    /**
        @brief Automatically adjust user's video resolution and frame-rate
        @param [out] preferenceSetting Specifies the video quality preference.	
        when setting custom modes, the maximum and minimum frame rates are provided by the developer.
        @param [out] callback A callback function to return the @link ZMVideoSDKErrors @endlink status code.
	*/
    public void SetVideoQualityPreference(ZMVideoSDKPreferenceSetting preferenceSetting, Action<ZMVideoSDKErrors> callback)
    {
        _videoSDKVideoHelper.SetVideoQualityPreference(preferenceSetting, callback);
    }

    /**
        @brief Enable multiple stream video if you have multiple cameras 
        and other participants can see multiple videos of you.
        @param cameraDeviceID The camera ID  for the camera to enable. 
        @param customDeviceName The custom device name of the camera. If this parameter is not passed, a default name will be generated.
        @return True if success. Otherwise returns false.
    */
    public bool EnableMultiStreamVideo(string cameraDeviceID, string customDeviceName)
    {
        return _videoSDKVideoHelper.EnableMultiStreamVideo(cameraDeviceID, customDeviceName);
    }

    /**
        @brief Disable multiple stream video 
        @param cameraDeviceID The camera id which you want to disable. 
        @return True if success. Otherwise returns false.
    */
    public bool DisableMultiStreamVideo(string cameraDeviceID)
    {
        return _videoSDKVideoHelper.DisableMultiStreamVideo(cameraDeviceID);
    }

    /**
        @brief Get the device ID associated with my multi-camera pipe
        @param zmVideoSDKRawDataPipe My multi-camera pipe
        @return The video device ID if successful. Otherwise returns NULL.
    */
    public string GetDeviceIDByMyPipe(ZMVideoSDKRawDataPipe zmVideoSDKRawDataPipe)
    {
        return _videoSDKVideoHelper.GetDeviceIDByMyPipe(zmVideoSDKRawDataPipe);
    }

    /* todo
    - (ZMVideoSDKErrors)startVideoPreview:(id<IZMVideoSDKRawDataPipeDelegate>)listener deviceID:(NSString*)cameraDeviceID;
    - (ZMVideoSDKErrors)stopVideoPreview:(id<IZMVideoSDKRawDataPipeDelegate>)listener;
    */

    /**
        @brief Check if my video is mirrored
        @return True if mirror is mirrored, otherwise false.
    */
    public bool IsMyVideoMirrored()
    {
        return _videoSDKVideoHelper.IsMyVideoMirrored();
    }

    /**
        @brief Check if original aspect ratio is enabled
        @return True if mirror is enabled, False if not
    */
    public bool IsOriginalAspectRatioEnabled()
    {
        return _videoSDKVideoHelper.IsOriginalAspectRatioEnabled();
    }

    /**
        @brief This function is used to set the aspect ratio of the video sent out.
        @param bEnabled False means the aspect ratio is 16:9, true means that using the original aspect ratio of video
        @param [out] callback a callback function to return true if the function succeeds, otherwise false.
        @remarks: If session is using video source and data_mode is not VideoSourceDataMode_None, default always use original aspect ration of video.
    */
    public void EnableOriginalAspectRatio(bool bEnabled, Action<bool> callback)
    {
        _videoSDKVideoHelper.EnableOriginalAspectRatio(bEnabled, callback);
    }

    /**
    @brief This function is used to select the camera device.
    @param cameraDeviceID the device id
    @return True if successful, otherwise false
    */
    public bool SelectCamera(string cameraDeviceID)
    {
        return _videoSDKVideoHelper.SelectCamera(cameraDeviceID);
    }

}