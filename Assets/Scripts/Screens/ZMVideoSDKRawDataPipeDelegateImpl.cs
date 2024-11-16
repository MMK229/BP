
using UnityEngine;

public class ZMVideoSDKRawDataPipeDelegateImpl : IZMVideoSDKRawDataPipeDelegate
{
    private string userId;
    private RawDataRender render;

    public ZMVideoSDKRawDataPipeDelegateImpl(string userId, IntroScreen introScreen, bool isSharingScreen)
    {
        if (isSharingScreen)
        {
            render = new ShareRawDataRender(introScreen);
        } else
        {
            render = new VideoRawDataRender(introScreen);
        }
        this.userId = userId;
    }

    public void onRawDataFrameReceived(ZMVideoSDKVideoRawData rawDataObject)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            render.SetData(rawDataObject, userId);
        });
    }

    public void onRawDataStatusChanged(ZMVideoSDKRawDataStatus status)
    {
        Debug.Log("onRawDataStatusChanged: " + status.ToString());
    }
}