Shader "Custom/sample" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard alpha:fade
		#pragma target 3.0

		struct Input {
			float3 worldPos;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			//float dist = distance( fixed3(0,0,0), IN.worldPos);
			//float val = abs(sin(dist*3.0-_Time*100));
			//if( val > 0.98 ){
			//	o.Albedo = fixed4(1, 1, 1, 1);
			//} else {
			//	o.Albedo = fixed4(110/255.0, 87/255.0, 139/255.0, 1);
			//	o.Alpha = 0.5;
			//	//o.Albedo = _Color;
			//}

			float val = ((sin(_Time*70) + 1) / 2) * 0.2 + 0.3;
			o.Albedo = lerp(_Color, fixed4(1,1,1,1), val);
			o.Alpha = _Color[3];
		}
		ENDCG
	}
	FallBack "Diffuse"
}