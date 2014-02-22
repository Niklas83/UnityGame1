Shader "Custom/LavaCube" {
	Properties {
        _MainTex ("Diffuse (RGB)", 2D) = "white" {}
		_AshTex ("Ash (RGB)", 2D) = "white" {}
		_BlendTex ("Blend (RGB)", 2D) = "white" {}
		_Perlin2D ("Perlin2D (RGB)", 2D) = "white" {}
		_Temp ("Temp", Range (0, 1)) = 0.25
	}
	SubShader {
        Tags { "RenderType" = "Opaque" }
        
        CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _AshTex;
		sampler2D _BlendTex;
		sampler2D _Perlin2D;
		
		float _Temp;

		struct Input {
			float2 uv_MainTex;
			float2 uv_AshTex;
			float2 uv_BlendTex;
			float2 uv_Perlin2D;
		};
		
		// http://en.wikipedia.org/wiki/Blend_modes#Overlay
		float overlay(float a, float b) {
			return a<0.5 ?
			       2*a*b    :
			       1 - 2*(1-a)*(1-b);
		}

		void surf(Input IN, inout SurfaceOutput o) {
			half4 main = tex2D(_MainTex, IN.uv_MainTex);
			half4 ash = tex2D(_AshTex, IN.uv_AshTex);
			
			float2 perlin_offset = float2(IN.uv_Perlin2D.x+_Time.x/2, IN.uv_Perlin2D.y+_Time.x/4);
			half4 perlin = tex2D(_Perlin2D, perlin_offset).r;
			
			float temperature_average = 0.5f;
			float temperature_spread = 4.0f;
			float temp = temperature_average + perlin.r * (_Temp) * ((sin(perlin_offset.y)+1)/2 + (_CosTime.x+1)/2);
			float temp_sat = saturate(temp);
			float2 uv_offset = float2(IN.uv_BlendTex.x+_Time.x/2, IN.uv_BlendTex.y+_Time.x/4);
			half4 mask = tex2D(_BlendTex, uv_offset).r;
			
			float t = overlay(temp_sat, mask);
			half4 testing = lerp(main, ash, t)*2;
			
			// o.Albedo = tex2D(_Perlin2D, IN.uv_Perlin2D);
			o.Emission = testing;
		}
		ENDCG
    }
	FallBack "Diffuse"
}
