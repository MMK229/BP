using UnityEngine;
using UnityEngine.UI;

public class ShareRawDataRender : RawDataRender
{
    private bool isBindToVideoItem = false;

    public ShareRawDataRender(IntroScreen introScreen) : base(introScreen)
    {
    }

    public override void SetData(ZMVideoSDKVideoRawData rawData, string userId)
    {
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

            GameObject mainViewObj = GameObject.Find("MainVideoViewPrefab");
            Material shaderMaterial = new Material(Shader.Find("YUV420Shader"));
            image = mainViewObj.GetComponent<RawImage>();
            image.material = shaderMaterial;

            if (image == null)
            {
                return;
            }
            target = image.material;
            float[] itemSize = CalculateHeight(width, height);
            image.rectTransform.sizeDelta = new Vector2(itemSize[0], itemSize[1]);
        }

        target.SetTexture("_YTexture", yTex);
        target.SetTexture("_UTexture", uTex);
        target.SetTexture("_VTexture", vTex);
        target.SetFloat("_Rotation", rotation);
    }

    private float[] CalculateHeight(float width, float height)
    {
        float[] itemSize = new float[2];
        if (width == 0)
        {
            return itemSize;
        }
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (width / height > screenWidth / screenHeight)
        {
            itemSize[0] = screenWidth;
            itemSize[1] = height / width * screenWidth;
        }
        else
        {
            itemSize[0] = width / height * screenHeight;
            itemSize[1] = screenHeight;
        }

        if (screenHeight < screenWidth)
        {
            itemSize[0] /= 2;
            itemSize[1] /= 2;
        }

        return itemSize;
    }

}