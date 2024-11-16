using System;
using UnityEngine;
using UnityEngine.UI;

public class VideoRawDataRender : RawDataRender
{
    private bool isBindToVideoItem = false;

    public VideoRawDataRender(IntroScreen introScreen) : base(introScreen)
    {
    }

    public override void SetData(ZMVideoSDKVideoRawData rawData, string userId)
    {
        if (String.IsNullOrEmpty(userId))
        {
            return;
        }
        base.SetData(rawData, userId);

        int width = rawData.width;
        int height = rawData.height;
        int rotation = rawData.rotation;

        if (!isBindToVideoItem)
        {
            if (rotation == 90 || rotation == 270)
            {
                int tmp = width;
                width = height;
                height = tmp;
            }
            GameObject imageObj = GameObject.Find("VideoView" + userId);
            if (imageObj == null)
            {
                return;
            }
            image = imageObj.GetComponent<RawImage>();
            target = image.material;
            float[] itemSize = CalculateHeight(width, height);
            image.rectTransform.sizeDelta = new Vector2(itemSize[0], itemSize[1]);
        }

        target.SetTexture("_YTexture", yTex);
        target.SetTexture("_UTexture", uTex);
        target.SetTexture("_VTexture", vTex);
        target.SetFloat("_Rotation", rotation);

        if (introScreen.IsMainViewEmpty())
        {
            introScreen.UpdateMainViewVideo(userId);
        }
    }

    private float[] CalculateHeight(float width, float height)
    {
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;
        float presetItemWidth;
        float presetItemHeight;
        if (screenHeight > screenWidth)
        {
            presetItemWidth = screenWidth / 4;
            presetItemHeight = screenHeight / 5;
        }
        else
        {
            presetItemWidth = screenWidth / 7;
            presetItemHeight = screenHeight / 4;
        }
        float[] itemSize = new float[2];
        if (width == 0)
        {
            return itemSize;
        }
        float ratioHeight = (float)height / (float)width * presetItemWidth;
        float ratioWidth = (float)width / (float)height * presetItemHeight;
        if (ratioHeight > presetItemHeight)
        {
            itemSize[0] = ratioWidth;
            itemSize[1] = presetItemHeight;
        }
        else
        {
            itemSize[0] = presetItemWidth;
            itemSize[1] = ratioHeight;
        }
        return itemSize;
    }
}