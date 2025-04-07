// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Behaviour_Split"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Scale("Scale", Float) = 2
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float3 worldPos;
		};

		uniform sampler2D _TextureSample0;
		uniform float _Scale;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color137 = IsGammaSpace() ? float4(0.8207547,0.6651516,0.1897027,0) : float4(0.6396053,0.3999554,0.02998661,0);
			float4 color138 = IsGammaSpace() ? float4(0.2415005,0.4518574,0.764151,0) : float4(0.04753957,0.1721552,0.5448383,0);
			float3 objToWorld113 = mul( unity_ObjectToWorld, float4( float3( 0,0,0 ), 1 ) ).xyz;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float dotResult119 = dot( mul( float4( mul( float4( ( objToWorld113 - _WorldSpaceCameraPos ) , 0.0 ), UNITY_MATRIX_V ).xyz , 0.0 ), unity_WorldToObject ).xyz , ase_vertex3Pos );
			float2 temp_cast_4 = (( dotResult119 * _Scale )).xx;
			float4 tex2DNode38 = tex2D( _TextureSample0, temp_cast_4 );
			float4 lerpResult29 = lerp( color137 , color138 , tex2DNode38);
			float4 Color23 = lerpResult29;
			o.Albedo = Color23.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19200
Node;AmplifyShaderEditor.CommentaryNode;22;-1569.857,-583.1972;Inherit;False;834.7886;371.308;Comment;5;20;13;15;14;16;Outline SIMPLE;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;12;-605.2734,-429.8865;Inherit;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;-349.7715,-317.8427;Inherit;False;alphaMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;-977.0688,-451.2913;Inherit;False;Outline SIMPLE;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OutlineNode;13;-1223.194,-465.0228;Inherit;False;1;True;None;0;0;Front;True;True;True;True;0;False;;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1519.857,-330.8892;Inherit;False;Property;_Widht;Widht;2;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;-1472.6,-533.1972;Inherit;False;Property;_Color1;Color 1;1;0;Create;True;0;0;0;False;0;False;0.6792453,0.1890353,0.1890353,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;16;-1365.857,-348.8892;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;17;-2069.386,-332.0431;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;10;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Behaviour_Split;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Masked;1;True;True;0;False;TransparentCutout;;AlphaTest;ForwardOnly;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexScale;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-187.7715,240.1573;Inherit;False;18;alphaMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;21;-237.6616,-8.036987;Inherit;False;23;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalizeNode;43;-1168.893,701.9663;Inherit;False;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-1411.475,657.0242;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;38;-2783.53,746.9629;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;dc5a7afc544c2564bb0f913574b6eba3;dc5a7afc544c2564bb0f913574b6eba3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;-2941.109,785.1546;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-3081.021,900.7587;Inherit;False;Property;_Scale;Scale;4;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;29;-2352.679,518.2951;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;135;-2537.018,202.2837;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-2304.528,181.1322;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-1993.741,437.1578;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;116;-3418.337,832.0932;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;-3383.672,571.5032;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;-3575.672,572.5032;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldToObjectMatrix;115;-3456.754,678.2879;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.ViewMatrixNode;124;-3615.672,684.5032;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;114;-3780.755,602.288;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;119;-3152.678,729.8725;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;137;-2726.185,322.9764;Inherit;False;Constant;_Color_1;Color_1;5;0;Create;True;0;0;0;False;0;False;0.8207547,0.6651516,0.1897027,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;138;-2722.249,491.9928;Inherit;False;Constant;_Color_2;Color_2;5;0;Create;True;0;0;0;False;0;False;0.2415005,0.4518574,0.764151,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceCameraPos;125;-4049.86,655.7161;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TransformPositionNode;113;-4019.756,507.2878;Inherit;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
WireConnection;18;0;12;4
WireConnection;20;0;13;0
WireConnection;13;0;14;0
WireConnection;13;1;16;0
WireConnection;16;0;15;0
WireConnection;10;0;21;0
WireConnection;43;0;42;0
WireConnection;42;1;29;0
WireConnection;38;1;126;0
WireConnection;126;0;119;0
WireConnection;126;1;127;0
WireConnection;29;0;137;0
WireConnection;29;1;138;0
WireConnection;29;2;38;0
WireConnection;136;0;135;0
WireConnection;136;1;38;0
WireConnection;23;0;29;0
WireConnection;121;0;123;0
WireConnection;121;1;115;0
WireConnection;123;0;114;0
WireConnection;123;1;124;0
WireConnection;114;0;113;0
WireConnection;114;1;125;0
WireConnection;119;0;121;0
WireConnection;119;1;116;0
ASEEND*/
//CHKSM=D69C8E49812546940E57437F574986004BD26CDF