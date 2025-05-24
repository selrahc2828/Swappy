// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Kelp"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Kelp_Base_color("Kelp_Base_color", 2D) = "white" {}
		_IntColor("IntColor", Color) = (0.1860092,0.4150943,0.4006006,0)
		_ExtColor1("ExtColor", Color) = (0.3794055,0.9245283,0.6184413,0)
		_KelpRadial("KelpRadial", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _IntColor;
		uniform float4 _ExtColor1;
		uniform sampler2D _KelpRadial;
		uniform float4 _KelpRadial_ST;
		uniform sampler2D _Kelp_Base_color;
		uniform float4 _Kelp_Base_color_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_KelpRadial = i.uv_texcoord * _KelpRadial_ST.xy + _KelpRadial_ST.zw;
			float4 lerpResult2 = lerp( _IntColor , _ExtColor1 , tex2D( _KelpRadial, uv_KelpRadial ));
			o.Albedo = lerpResult2.rgb;
			o.Alpha = 1;
			float2 uv_Kelp_Base_color = i.uv_texcoord * _Kelp_Base_color_ST.xy + _Kelp_Base_color_ST.zw;
			float4 tex2DNode1 = tex2D( _Kelp_Base_color, uv_Kelp_Base_color );
			clip( tex2DNode1.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19200
Node;AmplifyShaderEditor.ColorNode;3;-614.1642,-167.955;Inherit;False;Property;_IntColor;IntColor;2;0;Create;True;0;0;0;False;0;False;0.1860092,0.4150943,0.4006006,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-328.25,309.9526;Inherit;True;Property;_Kelp_Base_color;Kelp_Base_color;1;0;Create;True;0;0;0;False;0;False;-1;88724dc8d40a28142826451606dec818;88724dc8d40a28142826451606dec818;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-689.7154,257.3598;Inherit;True;Property;_KelpRadial;KelpRadial;4;0;Create;True;0;0;0;False;0;False;-1;7593c01b685ada5468b5182b0733b03d;7593c01b685ada5468b5182b0733b03d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-612.1642,2.045013;Inherit;False;Property;_ExtColor1;ExtColor;3;0;Create;True;0;0;0;False;0;False;0.3794055,0.9245283,0.6184413,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;2;-294.1642,67.04501;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;7,9;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Kelp;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;3;0
WireConnection;2;1;4;0
WireConnection;2;2;5;0
WireConnection;0;0;2;0
WireConnection;0;9;1;0
WireConnection;0;10;1;0
ASEEND*/
//CHKSM=CF6831A4F979206A12D5525A44583891DB2AFB95