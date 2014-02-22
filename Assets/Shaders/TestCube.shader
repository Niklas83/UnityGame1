Shader "Custom/TestCube" {
	Properties {
        _MainTex ("Diffuse (RGB)", 2D) = "white" {}
		_BlendTex ("Blend (RGB)", 2D) = "white" {}
		_SharpnessX ("SharpnessX", Range (0, 3)) = 2
		_SharpnessY ("SharpnessY", Range (0, 3)) = 1
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
            sampler2D _BlendTex;

            float4 _MainTex_ST;
            float4 _BlendTex_ST;
            
            float _SharpnessX;
            float _SharpnessY;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.uv_BlendTex = TRANSFORM_TEX(v.texcoord, _BlendTex);
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
            	float2 uv = i.uv_BlendTex;
                half4 original = tex2D(_MainTex, i.uv_MainTex);
                half4 blending = tex2D(_BlendTex, uv);      
             	
                half4 col = lerp(original, blending, pow(uv.x, _SharpnessX));
                return lerp(original, col, pow(uv.y, _SharpnessY));
            }
		ENDCG
		}
    }
	FallBack "Diffuse"
}
