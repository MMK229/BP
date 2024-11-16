#if UNITY_STANDALONE_OSX
using System;

public class MacZMVideoSDKRawData
{
    public MacZMVideoSDKRawData(IntPtr _objC_ZMVideoSDKRawData)
    {
        objC_ZMVideoSDKRawData = _objC_ZMVideoSDKRawData;
    }

    private IntPtr objC_ZMVideoSDKRawData;
    public IntPtr ObjC_ZMVideoSDKRawData
    {
        get { return objC_ZMVideoSDKRawData; }
    }
}
#endif
