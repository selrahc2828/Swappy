// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Grass"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_BaseTextureAlpha("Base Texture (Alpha)", 2D) = "white" {}
		[HDR]_TopColor("Top Color", Color) = (0,0.2178235,1,1)
		[HDR]_GroundColor("Ground Color", Color) = (1,0,0,1)
		[HDR]_Gradient("Gradient", Range( 0 , 10)) = 1.4
		_GradientPower("Gradient Power", Range( 0 , 10)) = 1
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_FluffyScale("Fluffy Scale", Float) = 1
		_Noisesize("Noise size", Float) = 0.2
		_Windtextnoise("Wind text noise", 2D) = "white" {}
		_windscroll("wind scroll", Float) = 0.1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float _FluffyScale;
		uniform sampler2D _BaseTextureAlpha;
		uniform float4 _BaseTextureAlpha_ST;
		uniform float4 _GroundColor;
		uniform float4 _TopColor;
		uniform float _Gradient;
		uniform float _GradientPower;
		uniform sampler2D _Windtextnoise;
		uniform float _windscroll;
		uniform float _Noisesize;
		uniform float _Smoothness;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 temp_cast_0 = (0.0).xx;
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float3 ase_worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
			half tangentSign = v.tangent.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
			float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * tangentSign;
			float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
			float3 tangentTobjectDir218 = mul( unity_WorldToObject, float4( mul( ase_tangentToWorldFast, float3( (temp_cast_0 + (v.texcoord.xy - float2( 0,0 )) * (float2( 1,1 ) - temp_cast_0) / (float2( 1,1 ) - float2( 0,0 ))) ,  0.0 ) ), 0 ) ).xyz;
			float3 temp_output_217_0 = ( tangentTobjectDir218 * _FluffyScale );
			float3 wind212 = temp_output_217_0;
			v.vertex.xyz = wind212;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BaseTextureAlpha = i.uv_texcoord * _BaseTextureAlpha_ST.xy + _BaseTextureAlpha_ST.zw;
			float4 tex2DNode1 = tex2D( _BaseTextureAlpha, uv_BaseTextureAlpha );
			float4 Base_Texture202 = tex2DNode1;
			float clampResult181 = clamp( pow( ( i.uv_texcoord.y * _Gradient ) , _GradientPower ) , 0.0 , 1.0 );
			float4 lerpResult173 = lerp( _GroundColor , _TopColor , clampResult181);
			float4 GRADIENT175 = ( Base_Texture202 * lerpResult173 );
			o.Albedo = GRADIENT175.rgb;
			float3 ase_worldPos = i.worldPos;
			float2 appendResult240 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner243 = ( ( _Time.y * _windscroll ) * float2( 1,1 ) + ( appendResult240 * _Noisesize ));
			float4 WindScroll249 = tex2D( _Windtextnoise, panner243 );
			o.Emission = WindScroll249.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
			clip( tex2DNode1.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19200
Node;AmplifyShaderEditor.ColorNode;38;1536,-608;Inherit;False;Property;_Color0;Color 0;7;0;Create;True;0;0;0;False;0;False;0.427451,0.8392158,0.3843138,1;0.2906469,0.567,0.2620586,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;178;357.8381,-744.5893;Inherit;False;Property;_GradientPower;Gradient Power;5;0;Create;True;0;0;0;False;0;False;1;4.22;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;172;277.838,-1400.589;Float;False;Property;_TopColor;Top Color;2;1;[HDR];Create;True;0;0;0;False;0;False;0,0.2178235,1,1;0,0.2178235,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;173;1029.838,-1336.589;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;174;277.838,-1208.589;Float;False;Property;_GroundColor;Ground Color;3;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,1;1,0,0,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;176;-10.16172,-1016.589;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;177;-26.16173,-856.5893;Float;False;Property;_Gradient;Gradient;4;1;[HDR];Create;True;0;0;0;False;0;False;1.4;1.25;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;179;277.838,-984.5892;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;180;581.8383,-952.5894;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;181;837.838,-1000.589;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;203;1034.553,-1472.239;Inherit;False;202;Base Texture;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;175;1504.839,-1375.589;Inherit;False;GRADIENT;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;1291.553,-1377.239;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;202;1482.331,-136.7796;Inherit;False;Base Texture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;194;1377,-225;Inherit;False;Property;_Smoothness;Smoothness;6;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;1192.142,-144.5803;Inherit;True;Property;_BaseTextureAlpha;Base Texture (Alpha);1;0;Create;True;0;0;0;False;0;False;-1;fa01f132a971a1a488eb1dd58ab7a1b6;fa01f132a971a1a488eb1dd58ab7a1b6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1712,-320;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Grass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Absolute;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;1499,24;Inherit;False;212;wind;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;182;1444,-397;Inherit;False;175;GRADIENT;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;236;566.9236,487.2556;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;241;1002.704,481.1281;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;240;799.7043,484.1281;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;242;800.7043,588.1276;Inherit;False;Property;_Noisesize;Noise size;11;0;Create;True;0;0;0;False;0;False;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;246;733.7043,689.1276;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;248;960.7044,723.1276;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;247;769.7043,771.1276;Inherit;False;Property;_windscroll;wind scroll;13;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;249;1739.703,486.1281;Inherit;False;WindScroll;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;223;-108.4063,483.7837;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;222;-229.1644,482.0043;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;220;-517.7341,447.6521;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NegateNode;227;-407.7742,600.585;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;221;-992.7482,607.3464;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;231;-543.4406,631.0822;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;229;-780.7742,798.5853;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;228;-1192.774,898.5855;Inherit;False;Property;_SSS_Scale;SSS_Scale;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TransformDirectionNode;232;-1214.44,734.0822;Inherit;False;Object;World;False;Fast;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;233;-1442.44,731.0822;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;226;219.562,480.4848;Inherit;False;SSS;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;235;59.67376,521.7755;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;234;-165.3262,596.7758;Inherit;False;Property;_SSSColor;SSS Color;10;0;Create;True;0;0;0;False;0;False;0.6320754,0.2057227,0.2057227,0;0.6320754,0.2057227,0.2057227,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;216;-531.2556,-339.7394;Inherit;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;0,0;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;215;-695.4841,-204.2692;Inherit;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;214;-757.2244,-337.0193;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TransformDirectionNode;218;-323.8284,-344.1369;Inherit;False;Tangent;Object;False;Fast;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;-84.12906,-343.137;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;219;-264.8286,-186.7372;Inherit;False;Property;_FluffyScale;Fluffy Scale;9;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;212;401.4178,-242.3197;Inherit;False;wind;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;251;-62.28507,-7.003256;Inherit;False;249;WindScroll;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;252;136.3123,-85.27534;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;253;-65.6879,-169.2753;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;230;1446.492,-313.4463;Inherit;False;249;WindScroll;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;250;309.615,-134.0032;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;243;1196.704,507.1281;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;245;1416.704,485.1281;Inherit;True;Property;_Windtextnoise;Wind text noise;12;0;Create;True;0;0;0;False;0;False;-1;2eee1166aed9c394b8b372f3b1cfa8b4;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;173;0;174;0
WireConnection;173;1;172;0
WireConnection;173;2;181;0
WireConnection;179;0;176;2
WireConnection;179;1;177;0
WireConnection;180;0;179;0
WireConnection;180;1;178;0
WireConnection;181;0;180;0
WireConnection;175;0;204;0
WireConnection;204;0;203;0
WireConnection;204;1;173;0
WireConnection;202;0;1;0
WireConnection;0;0;182;0
WireConnection;0;2;230;0
WireConnection;0;4;194;0
WireConnection;0;10;1;4
WireConnection;0;11;86;0
WireConnection;241;0;240;0
WireConnection;241;1;242;0
WireConnection;240;0;236;1
WireConnection;240;1;236;3
WireConnection;248;0;246;0
WireConnection;248;1;247;0
WireConnection;249;0;245;0
WireConnection;223;0;222;0
WireConnection;222;0;220;0
WireConnection;222;1;227;0
WireConnection;227;0;231;0
WireConnection;231;0;221;0
WireConnection;231;1;229;0
WireConnection;229;0;232;0
WireConnection;229;1;228;0
WireConnection;232;0;233;0
WireConnection;226;0;235;0
WireConnection;235;0;223;0
WireConnection;235;1;234;0
WireConnection;216;0;214;0
WireConnection;216;3;215;0
WireConnection;218;0;216;0
WireConnection;217;0;218;0
WireConnection;217;1;219;0
WireConnection;212;0;217;0
WireConnection;252;0;253;0
WireConnection;252;1;251;0
WireConnection;250;0;217;0
WireConnection;250;1;252;0
WireConnection;243;0;241;0
WireConnection;243;1;248;0
WireConnection;245;1;243;0
ASEEND*/
//CHKSM=E9A7FA885862133711BD58974B016917C260432C