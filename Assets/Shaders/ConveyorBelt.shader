Shader "Custom/ConveyorBelt" {
	Properties {
        _MainTex ("Diffuse (RGB)", 2D) = "white" {}
		_Speed ("Speed", Range (-1, 1)) = 0.5
	}
	SubShader {
        Tags { "RenderType" = "Opaque" }
        
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
                float2 uv_MainTex : TEXCOORD0;
                float2 uv_BlendTex : TEXCOORD1;
            };

            sampler2D _MainTex;

            float4 _MainTex_ST;
            
            float _Speed;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
            	float2 uv = i.uv_MainTex;
            	uv.x += _Speed*_Time.y;
                return tex2D(_MainTex, uv);
            }
		ENDCG
		}
    }
	FallBack "Diffuse"
}
