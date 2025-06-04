using UnityEngine;

public class AndroidZoomVideoSDKVideoRawData
{
    private AndroidJavaObject _rawData;

    public AndroidZoomVideoSDKVideoRawData(AndroidJavaObject rawData)
	{
        _rawData = rawData;
	}

	public void AddRef()
	{
        _rawData.Call("addRef");
    }

    public void ReleaseRef()
    {
        _rawData.Call("releaseRef");
    }

    public bool CanAddRef()
    {
        return _rawData.Call<bool>("canAddRef");
    }
}