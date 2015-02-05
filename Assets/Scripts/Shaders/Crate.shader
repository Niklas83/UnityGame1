Shader "Custom/Crate" {
	Properties {
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
		_Diffuse ("Diffuse", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_SpecMap ("Specular (R), Gloss (G)", 2D) = "black" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		CGPROGRAM
		#pragma surface surf BlinnPhong

		sampler2D _Diffuse;
		sampler2D _BumpMap;
		sampler2D _SpecMap;
		half _Shininess;

		struct Input {
			float2 uv_Diffuse;
			float2 uv_BumpMap;
			float2 uv_SpecMap;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_Diffuse, IN.uv_Diffuse).rgb;
			o.Specular = _Shininess * tex2D(_SpecMap, IN.uv_SpecMap).r;
			o.Gloss = tex2D(_SpecMap, IN.uv_SpecMap).g;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	}

	FallBack "Specular"
}
