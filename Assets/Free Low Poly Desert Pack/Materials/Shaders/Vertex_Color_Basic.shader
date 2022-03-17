Shader "Custom/Vertex_Color_Basic" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard //fullforwardshadows
		
		#pragma surface surf Lambert noforwardadd

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		struct Input 
		{
			fixed3 color : COLOR;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = IN.color.rgb* _Color;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
