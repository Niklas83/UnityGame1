Shader "Custom/IceCube" {
	Properties {
        _MainTex ("Diffuse (RGB)", 2D) = "white" {}
		_BumpMap ("Normal Map (RGB)", 2D) = "bump" {}
	}
	SubShader {
        Tags { "Queue" = "Transparent" }

        GrabPass { }

        Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

    		#include "UnityCG.cginc"

    		struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord: TEXCOORD0;
            };

            struct v2f {
                float4 vertex : POSITION;
                float4 uvgrab : TEXCOORD0;
                float2 uvbump : TEXCOORD1;
                float2 uv_MainTex : TEXCOORD2;
            };

            sampler2D _MainTex;

            float4 _BumpMap_ST;
            float4 _MainTex_ST;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                #if UNITY_UV_STARTS_AT_TOP
                    float scale = -1.0;
                #else
                    float scale = 1.0;
                #endif

                o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
                o.uvgrab.zw = o.vertex.zw;
                o.uvbump = TRANSFORM_TEX(v.texcoord, _BumpMap);
                o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            sampler2D _BumpMap;

            half4 frag(v2f i) : COLOR
            {
                half2 bump = UnpackNormal(tex2D(_BumpMap, i.uvbump)).rg;

                float2 offset = bump * 100 * _GrabTexture_TexelSize.xy;
                i.uvgrab.xy = i.uvgrab.xy + offset;

                half4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
                return col + tex2D(_MainTex, i.uv_MainTex + offset) * 0.75;
            }
		ENDCG
		}
    }
	FallBack "Diffuse"
}
