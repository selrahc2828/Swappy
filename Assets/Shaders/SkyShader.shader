// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SkyShader"
{
	Properties
	{
		_FullOpaqueEffect("FullOpaqueEffect", 2D) = "white" {}
		_MidOpaqueEffect("MidOpaqueEffect", 2D) = "white" {}
		_TimeScaleMid("TimeScaleMid", Range( 0 , 1)) = 0.05901834
		_TimeScaleOpaque("TimeScaleOpaque", Range( 0 , 1)) = 0.05901834
		_TilingSkyEffect("TilingSkyEffect", Float) = 3
		_Float0("Float 0", Float) = 4
		_TimeScale1("TimeScale", Range( -1 , 0)) = 0.005
		[Space(10)]_OpacityTransition("OpacityTransition", Range( 0 , 1)) = 0
		_MainColor2("MainColor2", Color) = (0.3285422,0.5643939,0.6509434,0)
		_MainColor1("MainColor1", Color) = (0.6552599,0.8962264,0.8689904,0)
		_perlin3("perlin3", 2D) = "white" {}
		_Opacity("Opacity", Color) = (0,0,0,0)
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
		uniform sampler2D _FullOpaqueEffect;
		uniform float _TilingSkyEffect;
		uniform float _TimeScaleOpaque;
		uniform sampler2D _MidOpaqueEffect;
		uniform float _TimeScaleMid;
		uniform half _OpacityTransition;
		uniform float4 _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_Float0).xx;
			float mulTime15 = _Time.y * _TimeScale1;
			float2 uv_TexCoord11 = i.uv_texcoord * temp_cast_0 + ( mulTime15 * float2( 1,1 ) );
			float4 tex2DNode7 = tex2D( _perlin3, uv_TexCoord11 );
			float4 lerpResult18 = lerp( _MainColor1 , _MainColor2 , tex2DNode7);
			o.Albedo = lerpResult18.rgb;
			float2 temp_cast_2 = (_TilingSkyEffect).xx;
			float mulTime61 = _Time.y * _TimeScaleOpaque;
			float2 temp_cast_3 = (( mulTime61 / 10.0 )).xx;
			float2 uv_TexCoord5 = i.uv_texcoord * temp_cast_2 + temp_cast_3;
			float2 temp_cast_4 = (_TilingSkyEffect).xx;
			float mulTime2 = _Time.y * _TimeScaleMid;
			float2 uv_TexCoord42 = i.uv_texcoord * temp_cast_4 + ( ( mulTime2 / 10.0 ) * float2( 1,-1 ) );
			o.Alpha = ( ( ( ( tex2DNode7 * float4( 0.2924528,0.2855553,0.2855553,0 ) ) + ( tex2D( _FullOpaqueEffect, uv_TexCoord5 ) + ( tex2D( _MidOpaqueEffect, uv_TexCoord42 ) * float4( 0.1509434,0.1509434,0.1509434,0 ) ) ) ) * ( 1.0 - _OpacityTransition ) ) * _Opacity ).r;
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
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;53,-11;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SkyShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Front;0;False;;0;False;;False;0;False;;0;False;;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-2033.572,258.0003;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-1749.579,-183.7696;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-1922.899,-230.7486;Inherit;False;Property;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;4;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;15;-2176.972,-191.7465;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2449.365,-193.0379;Inherit;False;Property;_TimeScale1;TimeScale;6;0;Create;True;0;0;0;False;0;False;0.005;-0.035;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1969.516,-145.5298;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;40;-2147.516,-120.5298;Inherit;False;Constant;_Vector0;Vector 0;9;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;4;-2570.957,281.7671;Inherit;False;Property;_TilingSkyEffect;TilingSkyEffect;4;0;Create;True;0;0;0;False;0;False;3;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-2639.395,489.6923;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;-2379.408,479.5102;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1800.034,241.4419;Inherit;True;Property;_FullOpaqueEffect;FullOpaqueEffect;0;0;Create;True;0;0;0;False;0;False;-1;134b1e13ae26bf94d9569f2bf60806b8;8db5a943bd039514bbf58e35999bd325;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;41;-2050.87,463.9517;Inherit;True;Property;_MidOpaqueEffect;MidOpaqueEffect;1;0;Create;True;0;0;0;False;0;False;-1;134b1e13ae26bf94d9569f2bf60806b8;134b1e13ae26bf94d9569f2bf60806b8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-715.2103,173.6035;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1182.789,-94.4405;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.2924528,0.2855553,0.2855553,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;7;-1510.205,-243.0932;Inherit;True;Property;_perlin3;perlin3;10;0;Create;True;0;0;0;False;0;False;-1;2eee1166aed9c394b8b372f3b1cfa8b4;aec738f429fa4ab4a8b574c7c804886c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-513.601,-435.438;Inherit;False;Property;_MainColor1;MainColor1;9;0;Create;True;0;0;0;False;0;False;0.6552599,0.8962264,0.8689904,0;0.6552599,0.8962264,0.8689904,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;12;-514.0825,-261.7523;Inherit;False;Property;_MainColor2;MainColor2;8;0;Create;True;0;0;0;False;0;False;0.3285422,0.5643939,0.6509434,0;0.3285416,0.5643938,0.6509434,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;18;-299.601,-290.438;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1464.707,338.2741;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-1737.414,453.7081;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.1509434,0.1509434,0.1509434,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;48;-2838.43,570.5773;Inherit;False;Constant;_Vector1;Vector 0;9;0;Create;True;0;0;0;False;0;False;1,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleDivideOpNode;3;-2959.149,491.1617;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;2;-3163.868,486.1755;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;60;-2782.995,314.2777;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;61;-2987.714,309.2915;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-3323.11,319.0001;Inherit;False;Property;_TimeScaleOpaque;TimeScaleOpaque;3;0;Create;True;0;0;0;False;0;False;0.05901834;0.05;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-3496.264,489.8841;Inherit;False;Property;_TimeScaleMid;TimeScaleMid;2;0;Create;True;0;0;0;False;0;False;0.05901834;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-120.9585,408.4165;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.1698113,0.1690103,0.1690103,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-518.1729,169.588;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;38;-796.4692,354.491;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1186.582,350.8576;Half;True;Property;_OpacityTransition;OpacityTransition;7;0;Create;False;0;0;0;False;1;Space(10);False;0;0.7884098;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;64;-484.7308,473.7217;Inherit;False;Property;_Opacity;Opacity;11;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.5943396,0.5943396,0.5943396,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;0;0;18;0
WireConnection;0;9;63;0
WireConnection;5;0;4;0
WireConnection;5;1;60;0
WireConnection;11;0;10;0
WireConnection;11;1;39;0
WireConnection;15;0;16;0
WireConnection;39;0;15;0
WireConnection;39;1;40;0
WireConnection;47;0;3;0
WireConnection;47;1;48;0
WireConnection;42;0;4;0
WireConnection;42;1;47;0
WireConnection;1;1;5;0
WireConnection;41;1;42;0
WireConnection;59;0;58;0
WireConnection;59;1;51;0
WireConnection;58;0;7;0
WireConnection;7;1;11;0
WireConnection;18;0;17;0
WireConnection;18;1;12;0
WireConnection;18;2;7;0
WireConnection;51;0;1;0
WireConnection;51;1;57;0
WireConnection;57;0;41;0
WireConnection;3;0;2;0
WireConnection;2;0;6;0
WireConnection;60;0;61;0
WireConnection;61;0;62;0
WireConnection;63;0;37;0
WireConnection;63;1;64;0
WireConnection;37;0;59;0
WireConnection;37;1;38;0
WireConnection;38;0;20;0
ASEEND*/
//CHKSM=B9F43DB32839B0994B8EAAD6465782226447F056