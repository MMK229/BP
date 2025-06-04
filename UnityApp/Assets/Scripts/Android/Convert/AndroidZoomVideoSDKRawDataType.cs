using System.Collections.Generic;
using UnityEngine;

public class AndroidZoomVideoSDKRawDataType
{
	public AndroidZoomVideoSDKRawDataType()
	{
	}

    private static AndroidJavaClass rawDataTypeClass = new AndroidJavaClass(Config.ZOOM_VIDEO_SDK_RAW_DATA_TYPE_PATH);

    private static Dictionary<string, ZMVideoSDKRawDataType> rawDataTypeDic
        = new Dictionary<string, ZMVideoSDKRawDataType>()
        {
            {rawDataTypeClass.GetStatic<AndroidJavaObject>("RAW_DATA_TYPE_VIDEO").Call<string>("name"), ZMVideoSDKRawDataType.ZMVideoSDKRawDataType_Video},
            {rawDataTypeClass.GetStatic<AndroidJavaObject>("RAW_DATA_TYPE_SHARE").Call<string>("name"), ZMVideoSDKRawDataType.ZMVideoSDKRawDataType_Share},
        };

    public static ZMVideoSDKRawDataType GetEnum(string rawDataType)
    {
        ZMVideoSDKRawDataType rawDataTypeEnum;
        try
        {
            rawDataTypeEnum = rawDataTypeDic[rawDataType];
        }
        catch (KeyNotFoundException)
        {
            rawDataTypeEnum = ZMVideoSDKRawDataType.ZMVideoSDKRawDataType_Video;
        }
        return rawDataTypeEnum;
    }
}

