#if UNITY_ANDROID
using System;
using UnityEngine;

public class AndroidZoomVideoSDKRawDataPipe
{
    private AndroidJavaObject _rawDataPipe;

    private AndroidZoomVideoSDKRawDataPipeDelegate androidDataDelegate;

    public AndroidZoomVideoSDKRawDataPipe(AndroidJavaObject rawDataPipe)
    {
        _rawDataPipe = rawDataPipe;
    }

    public ZMVideoSDKErrors Subscribe(ZMVideoSDKResolution resolution, IZMVideoSDKRawDataPipeDelegate dataDelegate)
    {
        this.androidDataDelegate = new AndroidZoomVideoSDKRawDataPipeDelegate(dataDelegate);
        int result = _rawDataPipe.Call<int>("subscribe", AndroidZoomVideoSDKVideoResolution.GetJavaObject(resolution), androidDataDelegate);
        return AndroidZoomVideoSDKErrors.GetEnum(result);
    }

    public ZMVideoSDKErrors UnSubscribe()
    {
        int result = _rawDataPipe.Call<int>("unSubscribe", androidDataDelegate);
        return AndroidZoomVideoSDKErrors.GetEnum(result);
    }

    public ZMVideoSDKRawDataType GetRawdataType()
    {
        AndroidJavaObject rawDataTypeObject = _rawDataPipe.Call<AndroidJavaObject>("getRawdataType");
        if (rawDataTypeObject != null)
        {
            return AndroidZoomVideoSDKRawDataType.GetEnum(rawDataTypeObject.Call<string>("name"));
        }
        throw new ArgumentNullException("AndroidZoomVideoSDKRawDataPipe::GetRawdataType getRawdataType returns null enum.");
    }

    public ZMVideoSDKVideoStatus GetVideoStatus()
    {
        AndroidJavaObject videoStatusObject = _rawDataPipe.Call<AndroidJavaObject>("getVideoStatus");
        ZMVideoSDKVideoStatus zmVideoStatus = new ZMVideoSDKVideoStatus();
        zmVideoStatus.isHasVideoDevice = videoStatusObject.Call<bool>("isHasVideoDevice");
        zmVideoStatus.isOn = videoStatusObject.Call<bool>("isOn");
        return zmVideoStatus;
    }

    public ZMVideoSDKShareStatus GetShareStatus()
    {
        AndroidJavaObject shareStatusObject = _rawDataPipe.Call<AndroidJavaObject>("getShareStatus");
        if (shareStatusObject != null)
        {
            return AndroidZoomVideoSDKShareStatus.GetEnum(shareStatusObject.Call<string>("name"));
        }
        throw new ArgumentNullException("AndroidZoomVideoSDKRawDataPipe::GetShareStatus getShareStatus returns null enum.");
    }

}
#endif
