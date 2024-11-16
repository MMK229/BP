using UnityEngine;
using UnityEngine.UI;

public class RawDataRender : MonoBehaviour
{

    [SerializeField]
    protected Texture2D yTex = null;
    [SerializeField]
    protected Texture2D uTex = null;
    [SerializeField]
    protected Texture2D vTex = null;
    protected IntroScreen introScreen;
    protected Material target;
    protected RawImage image;

    protected int previousWidth = 0;
    protected int previousHeight = 0;

    public RawDataRender(IntroScreen introScreen)
	{
        this.introScreen = introScreen;
    }

    public virtual void SetData(ZMVideoSDKVideoRawData rawData, string userId)
	{
        int width = rawData.width;
        int height = rawData.height;
        int rotation = rawData.rotation;
        if (yTex == null || uTex == null || vTex == null || width != previousWidth || height != previousHeight)
        {
            yTex = new Texture2D(width, height, TextureFormat.R8, false);
            uTex = new Texture2D(width >> 1, height >> 1, TextureFormat.R8, false);
            vTex = new Texture2D(width >> 1, height >> 1, TextureFormat.R8, false);
            previousHeight = height;
            previousWidth = width;
        }

        yTex.LoadRawTextureData(rawData.yBytes);
        uTex.LoadRawTextureData(rawData.uBytes);
        vTex.LoadRawTextureData(rawData.vBytes);

        yTex.Apply();
        uTex.Apply();
        vTex.Apply();
    }

    protected void DestroyTexture()
    {
        if (yTex != null)
        {
            GameObject.Destroy(yTex);
            yTex = null;
        }
        if (uTex != null)
        {
            GameObject.Destroy(uTex);
            uTex = null;
        }
        if (vTex != null)
        {
            GameObject.Destroy(vTex);
            vTex = null;
        }
    }

    protected void OnDestroy()
    {
        DestroyTexture();
    }

}

