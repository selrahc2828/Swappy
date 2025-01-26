// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Plants_Grass"
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
		_SSS_Scale("SSS_Scale", Float) = 1
		_SSSColor("SSS Color", Color) = (0.6320754,0.2057227,0.2057227,0)
		_Windtextnoise("Wind text noise", 2D) = "white" {}
		_Speed("Speed", Float) = 0
		_scale("scale", Float) = 50
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			float3 worldPos;
		};

		uniform float _Speed;
		uniform sampler2D _Windtextnoise;
		uniform float _scale;
		uniform sampler2D _BaseTextureAlpha;
		uniform float4 _BaseTextureAlpha_ST;
		uniform float4 _GroundColor;
		uniform float4 _TopColor;
		uniform float _Gradient;
		uniform float _GradientPower;
		uniform float _SSS_Scale;
		uniform float4 _SSSColor;
		uniform float _Smoothness;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime55 = _Time.y * _Speed;
			float2 temp_cast_0 = (mulTime55).xx;
			float2 uv_TexCoord54 = v.texcoord.xy * float2( 10,0 ) + temp_cast_0;
			float temp_output_57_0 = sin( uv_TexCoord54.x );
			float2 uv_TexCoord61 = v.texcoord.xy * float2( 0.5,0.5 );
			float temp_output_62_0 = ( ( 1.0 - uv_TexCoord61.x ) - 0.5 );
			float temp_output_60_0 = ( ( temp_output_57_0 * uv_TexCoord61.x ) * ( temp_output_57_0 * temp_output_62_0 ) );
			float3 appendResult68 = (float3(temp_output_60_0 , temp_output_60_0 , temp_output_60_0));
			float4 WindScroll22 = tex2Dlod( _Windtextnoise, float4( v.texcoord.xy, 0, 0.0) );
			float4 wind65 = ( ( float4( appendResult68 , 0.0 ) * WindScroll22 ) / _scale );
			v.vertex.xyz += wind65.rgb;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BaseTextureAlpha = i.uv_texcoord * _BaseTextureAlpha_ST.xy + _BaseTextureAlpha_ST.zw;
			float4 tex2DNode49 = tex2D( _BaseTextureAlpha, uv_BaseTextureAlpha );
			float4 Base_Texture45 = tex2DNode49;
			float clampResult10 = clamp( pow( ( i.uv_texcoord.y * _Gradient ) , _GradientPower ) , 0.0 , 1.0 );
			float4 lerpResult4 = lerp( _GroundColor , _TopColor , clampResult10);
			float4 GRADIENT12 = ( Base_Texture45 * lerpResult4 );
			o.Albedo = GRADIENT12.rgb;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 objToWorldDir33 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 0 ) ).xyz;
			float dotResult26 = dot( i.viewDir , -( ase_worldlightDir + ( objToWorldDir33 * _SSS_Scale ) ) );
			float4 SSS35 = ( saturate( dotResult26 ) * _SSSColor );
			o.Emission = SSS35.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
			clip( tex2DNode49.a - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19200
Node;AmplifyShaderEditor.RangedFloatNode;2;-1207.468,-266.2172;Inherit;False;Property;_GradientPower;Gradient Power;5;0;Create;True;0;0;0;False;0;False;1;4.22;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-1287.468,-922.2169;Float;False;Property;_TopColor;Top Color;2;1;[HDR];Create;True;0;0;0;False;0;False;0,0.2178235,1,1;0,0.2178235,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;4;-535.4678,-858.2169;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;5;-1287.468,-730.2169;Float;False;Property;_GroundColor;Ground Color;3;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,1;1,0,0,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1575.468,-538.2169;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-1591.468,-378.2172;Float;False;Property;_Gradient;Gradient;4;1;[HDR];Create;True;0;0;0;False;0;False;1.4;1.25;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1287.468,-506.2171;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;9;-983.4675,-474.2173;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;10;-727.4678,-522.2169;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;11;-530.7528,-993.8669;Inherit;False;45;Base Texture;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;12;-60.4668,-897.2169;Inherit;False;GRADIENT;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-273.7528,-898.8669;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-188.3058,253.3721;Inherit;False;Property;_Smoothness;Smoothness;6;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;15;-121.3058,81.37207;Inherit;False;12;GRADIENT;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-562.6018,959.5002;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;17;-765.6015,962.5002;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-764.6015,1066.5;Inherit;False;Property;_Noisesize;Noise size;9;0;Create;True;0;0;0;False;0;False;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;22;174.3972,964.5002;Inherit;False;WindScroll;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;23;-118.8138,164.9258;Inherit;False;35;SSS;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-66.30579,502.3721;Inherit;False;65;wind;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;25;-2095.125,-878.7539;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;26;-2215.884,-880.533;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;27;-2504.453,-914.886;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NegateNode;28;-2394.493,-761.9529;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;29;-2979.468,-755.1909;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-2530.16,-731.455;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-2767.494,-563.9529;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-3179.494,-463.9525;Inherit;False;Property;_SSS_Scale;SSS_Scale;7;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TransformDirectionNode;33;-3201.159,-628.4559;Inherit;False;Object;World;False;Fast;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;34;-3429.159,-631.4559;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;-1767.157,-882.053;Inherit;False;SSS;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-1927.045,-840.762;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;37;-2152.045,-765.762;Inherit;False;Property;_SSSColor;SSS Color;8;0;Create;True;0;0;0;False;0;False;0.6320754,0.2057227,0.2057227,0;0.6320754,0.2057227,0.2057227,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;-74.97473,334.5925;Inherit;False;Base Texture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;47;-381.2798,995.8889;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;48;-369.6018,1122.5;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;49;-373.1638,333.7917;Inherit;True;Property;_BaseTextureAlpha;Base Texture (Alpha);1;0;Create;True;0;0;0;False;0;False;-1;fa01f132a971a1a488eb1dd58ab7a1b6;fa01f132a971a1a488eb1dd58ab7a1b6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;54;-2035.812,94.23746;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;55;-2309.229,138.437;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-2501.402,139.5732;Inherit;False;Constant;_Float1;Float 0;2;0;Create;True;0;0;0;False;0;False;-2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;57;-1814.298,97.43698;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1579.996,131.5126;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-1554.707,390.2502;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-1306.254,350.3815;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;61;-2380.81,382.1352;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;62;-2010.95,570.7921;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-1753.021,623.4929;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;64;-2206.516,574.4984;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;66;-2274.298,12.43698;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;0;False;0;False;10,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;68;-1048.327,330.6772;Inherit;True;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;166.6433,99.74507;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Plants_Grass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.WorldPosInputsNode;46;-975.4871,910.6796;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;50;-147.6018,963.5002;Inherit;True;Property;_Windtextnoise;Wind text noise;10;0;Create;True;0;0;0;False;0;False;-1;d143dae5f33532b47aec5195fccebd93;2eee1166aed9c394b8b372f3b1cfa8b4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;-430.8197,562.0496;Inherit;False;wind;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;67;-565.4093,600.8896;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.6320754;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-790.4588,419.4049;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;53;-1032.006,556.2731;Inherit;False;22;WindScroll;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-767.1691,654.7449;Inherit;False;Property;_scale;scale;12;0;Create;True;0;0;0;False;0;False;50;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;19;-661.3015,1216.901;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-2475.051,24.0992;Inherit;False;Property;_Speed;Speed;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
WireConnection;4;0;5;0
WireConnection;4;1;3;0
WireConnection;4;2;10;0
WireConnection;8;0;6;2
WireConnection;8;1;7;0
WireConnection;9;0;8;0
WireConnection;9;1;2;0
WireConnection;10;0;9;0
WireConnection;12;0;13;0
WireConnection;13;0;11;0
WireConnection;13;1;4;0
WireConnection;16;0;17;0
WireConnection;16;1;18;0
WireConnection;17;0;46;1
WireConnection;17;1;46;3
WireConnection;22;0;50;0
WireConnection;25;0;26;0
WireConnection;26;0;27;0
WireConnection;26;1;28;0
WireConnection;28;0;30;0
WireConnection;30;0;29;0
WireConnection;30;1;31;0
WireConnection;31;0;33;0
WireConnection;31;1;32;0
WireConnection;33;0;34;0
WireConnection;35;0;36;0
WireConnection;36;0;25;0
WireConnection;36;1;37;0
WireConnection;45;0;49;0
WireConnection;48;1;19;0
WireConnection;54;0;66;0
WireConnection;54;1;55;0
WireConnection;55;0;71;0
WireConnection;57;0;54;1
WireConnection;58;0;57;0
WireConnection;58;1;61;1
WireConnection;59;0;57;0
WireConnection;59;1;62;0
WireConnection;60;0;58;0
WireConnection;60;1;59;0
WireConnection;62;0;64;0
WireConnection;63;0;62;0
WireConnection;64;0;61;1
WireConnection;68;0;60;0
WireConnection;68;1;60;0
WireConnection;68;2;60;0
WireConnection;0;0;15;0
WireConnection;0;2;23;0
WireConnection;0;4;14;0
WireConnection;0;10;49;4
WireConnection;0;11;24;0
WireConnection;50;1;47;0
WireConnection;65;0;67;0
WireConnection;67;0;69;0
WireConnection;67;1;70;0
WireConnection;69;0;68;0
WireConnection;69;1;53;0
ASEEND*/
//CHKSM=19303CE5EFA89A1030E1130F9CB991B0C8FAC1A4