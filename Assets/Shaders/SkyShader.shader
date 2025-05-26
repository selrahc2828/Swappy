// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SkyShader"
{
	Properties
	{
		_YJE1z2("YJE1z2", 2D) = "white" {}
		_TimeScale("TimeScale", Range( 0 , 1)) = 0.05901834
		_TilingSkyEffect("TilingSkyEffect", Float) = 3
		_Float0("Float 0", Float) = 4
		_TimeScale1("TimeScale", Range( 0 , 1)) = 0.005
		_MainColor2("MainColor2", Color) = (0.3285422,0.5643939,0.6509434,0)
		_MainColor1("MainColor1", Color) = (0.6552599,0.8962264,0.8689904,0)
		_perlin3("perlin3", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Front
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _MainColor1;
		uniform float4 _MainColor2;
		uniform sampler2D _perlin3;
		uniform float _Float0;
		uniform float _TimeScale1;
		uniform sampler2D _YJE1z2;
		uniform float _TilingSkyEffect;
		uniform float _TimeScale;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_Float0).xx;
			float mulTime15 = _Time.y * _TimeScale1;
			float2 temp_cast_1 = (mulTime15).xx;
			float2 uv_TexCoord11 = i.uv_texcoord * temp_cast_0 + temp_cast_1;
			float4 tex2DNode7 = tex2D( _perlin3, uv_TexCoord11 );
			float4 lerpResult18 = lerp( _MainColor1 , _MainColor2 , tex2DNode7);
			o.Albedo = lerpResult18.rgb;
			float2 temp_cast_3 = (_TilingSkyEffect).xx;
			float mulTime2 = _Time.y * _TimeScale;
			float2 temp_cast_4 = (( mulTime2 / 10.0 )).xx;
			float2 uv_TexCoord5 = i.uv_texcoord * temp_cast_3 + temp_cast_4;
			o.Alpha = ( tex2DNode7 * tex2D( _YJE1z2, uv_TexCoord5 ) ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
			sampler3D _DitherMaskLOD;
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
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
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
Node;AmplifyShaderEditor.SamplerNode;7;-528.6375,-42.63902;Inherit;True;Property;_perlin3;perlin3;7;0;Create;True;0;0;0;False;0;False;-1;2eee1166aed9c394b8b372f3b1cfa8b4;3357ccef23e8b664dafba7d927f4925b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-764.0111,-24.31541;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;15;-959.404,40.70767;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1261.798,39.41626;Inherit;False;Property;_TimeScale1;TimeScale;4;0;Create;True;0;0;0;False;0;False;0.005;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;17;-514.601,-406.438;Inherit;False;Property;_MainColor1;MainColor1;6;0;Create;True;0;0;0;False;0;False;0.6552599,0.8962264,0.8689904,0;0.5613208,1,0.9347364,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;12;-515.0825,-232.7523;Inherit;False;Property;_MainColor2;MainColor2;5;0;Create;True;0;0;0;False;0;False;0.3285422,0.5643939,0.6509434,0;0.5613208,1,0.9347364,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;18;-300.601,-261.438;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-932.3305,-51.29442;Inherit;False;Property;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;4;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-518.7092,223.7921;Inherit;True;Property;_YJE1z2;YJE1z2;0;0;Create;True;0;0;0;False;0;False;-1;134b1e13ae26bf94d9569f2bf60806b8;134b1e13ae26bf94d9569f2bf60806b8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;3;-877.9189,363.8641;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-752.2466,241.3507;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;2;-1106.637,368.8779;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1439.031,372.5865;Inherit;False;Property;_TimeScale;TimeScale;1;0;Create;True;0;0;0;False;0;False;0.05901834;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-914.9312,212.1174;Inherit;False;Property;_TilingSkyEffect;TilingSkyEffect;2;0;Create;True;0;0;0;False;0;False;3;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-209.2704,172.7184;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;53,-11;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SkyShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Front;0;False;;0;False;;False;0;False;;0;False;;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;1;11;0
WireConnection;11;0;10;0
WireConnection;11;1;15;0
WireConnection;15;0;16;0
WireConnection;18;0;17;0
WireConnection;18;1;12;0
WireConnection;18;2;7;0
WireConnection;1;1;5;0
WireConnection;3;0;2;0
WireConnection;5;0;4;0
WireConnection;5;1;3;0
WireConnection;2;0;6;0
WireConnection;19;0;7;0
WireConnection;19;1;1;0
WireConnection;0;0;18;0
WireConnection;0;9;19;0
ASEEND*/
//CHKSM=7FFCCCBEE95ABCBCEA9B937CAA0E4D83EC99063A