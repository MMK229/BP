using System;
using UnityEngine;

public class JavaObjectHelper
{
	public JavaObjectHelper()
	{
	}

	public static void Dispose(AndroidJavaObject javaObject)
	{
		if (javaObject != null)
		{
			javaObject.Dispose();
		}
	}
}

