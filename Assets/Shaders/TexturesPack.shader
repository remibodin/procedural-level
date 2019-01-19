Shader "Nrtx/TexturesPack" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float4 color : COLOR;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float2 uv = IN.uv_MainTex;
			uv.x = (uv.x * IN.color.b) + IN.color.r;
			uv.y = (uv.y * IN.color.b) + IN.color.g;

			fixed4 c = tex2D (_MainTex, uv);
			o.Albedo = c;
			o.Emission = c.rgb * c.a;
			o.Metallic = 0.0f;
			o.Smoothness = 0.0f;
			o.Alpha = 0.0f;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
