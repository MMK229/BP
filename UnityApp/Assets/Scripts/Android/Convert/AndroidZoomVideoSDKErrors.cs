using System;

public class AndroidZoomVideoSDKErrors
{
    public AndroidZoomVideoSDKErrors()
	{
	}

    public static ZMVideoSDKErrors GetEnum(int errorCode)
    {
        return (ZMVideoSDKErrors)Enum.ToObject(typeof(ZMVideoSDKErrors), errorCode);
    }
}

