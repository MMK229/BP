using System.Collections.Generic;

public class AndroidZoomVideoRotation
{
	public AndroidZoomVideoRotation()
	{
	}

    private static Dictionary<ZMVideoRotation, int> rotationDic
    = new Dictionary<ZMVideoRotation, int>()
    {
            {ZMVideoRotation.ZMVideoRotation_0, 0},
            {ZMVideoRotation.ZMVideoRotation_90, 90},
            {ZMVideoRotation.ZMVideoRotation_180, 180},
            {ZMVideoRotation.ZMVideoRotation_270, 270},
    };

    public static int GetValue(ZMVideoRotation rotationEnum)
    {
        int rotation = 0;
        try
        {
            rotation = rotationDic[rotationEnum];
        }
        catch (KeyNotFoundException)
        {
            rotation = 0;
        }
        return rotation;
    }
}