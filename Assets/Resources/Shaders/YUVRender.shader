Shader "YUV420Shader" 
{
	Properties
	{
		_MainTex("Texture",2D) = "white" {}
		_YTexture("Texture",2D) = "white" {}
		_UTexture("Texture",2D) = "white"{}
		_VTexture("Texture",2D) = "white"{}
		_Rotation("Rotation", Range(0, 360)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Cull off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _YTexture;
			sampler2D _UTexture;
			sampler2D _VTexture;

			float4 _MainTex_ST;
			float _Rotation;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

				if (_Rotation == 90) // 90 degree rotation
				{
					o.uv = float2(1.0 - v.uv.y, 1.0 - v.uv.x);
				}
				else if (_Rotation == 180) // 180 degree rotation
				{
					o.uv = float2(v.uv.x, 1.0 - v.uv.y);
				}
				else if (_Rotation == 270) // 270 degree rotation
				{
					o.uv = float2(v.uv.y, v.uv.x);
				}
				else // 0 degree rotation
				{
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.uv = float2(v.uv.x, 1.0 - v.uv.y);
				}
                return o;
            }
			
			fixed4 frag (v2f i) : SV_Target
			{
				float y_col = tex2D(_YTexture, i.uv) - 0.5;
                float u_col = tex2D(_UTexture, i.uv) - 0.5;
                float v_col = tex2D(_VTexture, i.uv) - 0.5;

                float4 col;

                col.r = y_col + 1.140*v_col + 0.5;
                col.g = y_col - 0.395*u_col - 0.581*v_col + 0.5;
                col.b = y_col + 2.032*u_col + 0.5;
                col.a = 1.0;

                return col;
			}

			ENDCG
		}
	}
}
