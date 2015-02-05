Shader "Custom/Heat" {
	Properties {
		_AlphaMask("AlphaMask (RGB)", 2D) = "white" {}
		_Noise("Noise (RGB)", 2D) = "white" {}
		_Magnitude("Magnitude", Float) = 0.1
	}
	SubShader {
        Tags { "RenderType"="Transparent" }

        GrabPass { }

        Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			AlphaTest Greater 0
			Cull Off Lighting On ZWrite Off
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            

    		#include "UnityCG.cginc"

    		struct appdata_t {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord: TEXCOORD0;
            };

            struct v2f {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float4 uvgrab : TEXCOORD0;
                float2 uvnoise : TEXCOORD1;
                float2 uvalpha_mask : TEXCOORD2;
            };
            sampler2D _Noise;
            sampler2D _AlphaMask;
            sampler2D _GrabTexture;
            
            float4 _Noise_ST;
            float4 _AlphaMask_ST;
            float4 _GrabTexture_TexelSize;
            float _Magnitude;

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
                o.uvnoise = TRANSFORM_TEX(v.texcoord, _Noise);
                o.uvalpha_mask = TRANSFORM_TEX(v.texcoord, _AlphaMask);
                o.color = v.color;
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
            	half4 alpha_mask = tex2D(_AlphaMask, i.uvalpha_mask);
            	
            	i.uvnoise.y -= _Time.x*2;
                half2 noise = tex2D(_Noise, i.uvnoise).rg;
                
                float2 offset = noise * i.color.a * alpha_mask.a * _Magnitude;
                i.uvgrab.xy = i.uvgrab.xy + offset;

                half4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
                return col; //  half4(1, 1, 1, alpha_mask.a * i.color.a);
            }
		ENDCG
		}
    }
	FallBack "Diffuse"
}
