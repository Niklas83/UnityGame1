Shader "Custom/Gradient" {
	Properties {
		_TopColor ("Top Color", Color) = (1, 1, 1, 1)
		_BottomColor ("Bottom Color", Color) = (0, 0, 0, 1)
		_MainTex ("Main Tex", 2d) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		Pass {
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
    		#include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
    		struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : POSITION;
                float2 uvtest : TEXCOORD0;
                float4 pos : TEXCOORD1;
            };
            
            float4 _TopColor;
            float4 _BottomColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                float4 position = mul(UNITY_MATRIX_MVP, v.vertex);
               	o.vertex = position;
               	o.pos = position;
               	
				o.uvtest = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
            	float posy = i.pos.y / i.pos.w; // _ScreenParams.y;
            	float height = (posy + 1.0f) * 0.5f;
            	float testy = i.pos.y / i.pos.w;
                return lerp(_BottomColor, _TopColor, height);
                
                
            }
            ENDCG
        }
	}

	FallBack "Specular"
}
