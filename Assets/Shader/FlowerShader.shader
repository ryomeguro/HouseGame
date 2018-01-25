Shader "Custom/FlowerShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
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
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		float random(float p){
			return frac(sin(p * 12.9898) * 43758.5453);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float r = random(floor(floor(IN.worldPos.x) + floor(IN.worldPos.z / 2)));
			float g = random(floor(floor(IN.worldPos.x / 2) + floor(IN.worldPos.z)));
			float b = random(floor(floor(35- IN.worldPos.x) + floor(IN.worldPos.z)));
			//o.Albedo = _Color;
			o.Albedo = fixed4(r,g,b,1);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
