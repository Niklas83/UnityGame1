Shader "Custom/ShadowProxy" {
	Properties {
	}
	SubShader {
        Tags { "RenderType"="Transparent" }

        GrabPass { }

		Pass {
            SetTexture [_GrabTexture] { combine texture }
		}
    }
	FallBack "Diffuse"
}
