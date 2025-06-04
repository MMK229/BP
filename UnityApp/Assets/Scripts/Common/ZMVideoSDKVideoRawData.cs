using System;
using System.Runtime.InteropServices;

public class ZMVideoSDKVideoRawData
{
	public int height;
    public int width;
    public int rotation;
    public byte[] yBytes;
    public byte[] uBytes;
    public byte[] vBytes;

    public ZMVideoSDKVideoRawData(int height, int width, int rotation, byte[] yBytes, byte[] uBytes, byte[] vBytes)
	{
		this.height = height;
		this.width = width;
		this.rotation = rotation;
		this.yBytes = yBytes;
		this.uBytes = uBytes;
		this.vBytes = vBytes;
	}
}

public class ZMVideoSDKAudioRawData
{
    public byte[] buffer;
    public int bufferLen;
    public int sampleRate;
    public int channelNum;

    public ZMVideoSDKAudioRawData(IntPtr _rawBuffer, int _bufferLen, int _sampleRate, int _channelNum)
    {
        bufferLen = _bufferLen;
        sampleRate = _sampleRate;
        channelNum = _channelNum;
        buffer = new byte[bufferLen];
        Marshal.Copy(_rawBuffer, buffer, 0, bufferLen);
    }
}