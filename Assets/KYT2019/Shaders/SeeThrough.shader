// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SeeThrough" {

	Properties {
		_Radius("Radius", Float) = 1.0
		_BaseColour("Base Colour", Color) = (0.0, 0.0, 0.0, 0.0)
		_CircleColor("Circle Color", Color) = (0.5, 1.0, 1.0, 1.0)
	}

	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" }

		Pass {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			// Define the vertex and fragment shader functions
			#pragma vertex vert
			#pragma fragment frag 

			// Access Shaderlab properties
			uniform float _Radius;
			uniform float4 _BaseColour;
			uniform float4 _CircleColor;

			uniform float3 _MousePosition;

			// Input into the vertex shader
			struct vertexInput {
				float4 vertex : POSITION;
			};

			// Output from vertex shader into fragment shader
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 worldPos : TEXCOORD0;
			};

			// VERTEX SHADER
			vertexOutput vert(vertexInput input) {
				vertexOutput output;
				output.pos = UnityObjectToClipPos(input.vertex);
				// Calculate the world position coordinates to pass to the fragment shader
				output.worldPos = mul(unity_ObjectToWorld, input.vertex);
				return output;
			}

			// FRAGMENT SHADER
			float4 frag(vertexOutput input) : COLOR {

				float4 color = _BaseColour;

				if (distance(input.worldPos, _MousePosition) < _Radius)
					color = _CircleColor;
				
				return color;
			}
			ENDCG
		}
	}
}