// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ZoneAimant"
{
	Properties
	{
		_Opacity("Opacity", Float) = 0.3
		_Color0("Color 0", Color) = (0.06274509,0.4342914,0.7647059,0)
		_VertexOffsetSpeed("VertexOffsetSpeed", Float) = 0.2
		_VertexOffset("VertexOffset", Float) = 2
		_EmissionScale("Emission Scale", Float) = 2
		_perlin3("perlin3", 2D) = "white" {}
		_proutPower("proutPower", Float) = 2.45
		_grosproutMult("grosproutMult", Float) = 1.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldNormal;
			float3 viewDir;
			half ASEIsFrontFacing : VFACE;
			float2 uv_texcoord;
		};

		uniform float _VertexOffsetSpeed;
		uniform float _VertexOffset;
		uniform float4 _Color0;
		uniform float _proutPower;
		uniform float _grosproutMult;
		uniform sampler2D _perlin3;
		uniform float _EmissionScale;
		uniform float _Opacity;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float simplePerlin2D25 = snoise( ( ase_vertex3Pos + ( _Time.y * _VertexOffsetSpeed ) ).xy*2.0 );
			simplePerlin2D25 = simplePerlin2D25*0.5 + 0.5;
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float3 Offset_Vertex34 = ( ( simplePerlin2D25 * ase_worldNormal ) / _VertexOffset );
			v.vertex.xyz += Offset_Vertex34;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldNormal = i.worldNormal;
			float dotResult54 = dot( ase_worldNormal , i.viewDir );
			float switchResult56 = (((i.ASEIsFrontFacing>0)?(( 1.0 - saturate( ( pow( dotResult54 , _proutPower ) * _grosproutMult ) ) )):(0.0)));
			float2 temp_cast_0 = (2.0).xx;
			float2 temp_cast_1 = (( _Time.y / 10.0 )).xx;
			float2 uv_TexCoord7 = i.uv_texcoord * temp_cast_0 + temp_cast_1;
			float4 temp_output_49_0 = ( switchResult56 + ( tex2D( _perlin3, uv_TexCoord7 ) * float4( 0.9433962,0.9433962,0.9433962,0 ) ) );
			float4 temp_output_15_0 = ( ( _Color0 * temp_output_49_0 ) * 2.8 );
			float4 Albedo33 = temp_output_15_0;
			o.Albedo = Albedo33.rgb;
			float4 emission36 = ( temp_output_15_0 * _EmissionScale );
			o.Emission = emission36.rgb;
			o.Alpha = ( temp_output_49_0 * _Opacity ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				float3 worldNormal : TEXCOORD3;
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
				o.worldNormal = worldNormal;
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
				surfIN.worldNormal = IN.worldNormal;
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
Node;AmplifyShaderEditor.CommentaryNode;38;-1552.886,531.5236;Inherit;False;1991.896;588.7762;Comment;10;21;23;25;24;27;22;28;31;32;34;Vertex Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;37;-1405.737,-710.5494;Inherit;False;2131.511;653.1356;Comment;20;55;56;54;53;51;14;8;7;49;1;15;4;16;39;33;36;35;45;13;62;Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;314.1935,18.89886;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ZoneAimant;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1258.149,581.5236;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-965.5825,636.2147;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;24;-1224.582,696.2148;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;27;-725.8447,866.2997;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;22;-1502.886,677.0737;Inherit;False;Property;_VertexOffsetSpeed;VertexOffsetSpeed;2;0;Create;True;0;0;0;False;0;False;0.2;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-472.3375,741.2491;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;31;-47.13409,771.5426;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-272.1341,998.5426;Inherit;False;Property;_VertexOffset;VertexOffset;3;0;Create;True;0;0;0;False;0;False;2;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;34;197.0095,830.218;Inherit;False;Offset Vertex;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1888.104,64.90228;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;42;84.63702,340.3201;Inherit;False;34;Offset Vertex;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;33.78174,79.25958;Inherit;False;36;emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;40;39.78174,-2.740417;Inherit;False;33;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;25;-748.5825,638.2147;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-326.1009,340.7929;Inherit;False;Property;_Opacity;Opacity;0;0;Create;True;0;0;0;False;0;False;0.3;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-27.07629,243.6562;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-707.9941,-321.9557;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.9433962,0.9433962,0.9433962,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;45;-992.4863,-309.0689;Inherit;True;Property;_perlin3;perlin3;5;0;Create;True;0;0;0;False;0;False;-1;2eee1166aed9c394b8b372f3b1cfa8b4;3357ccef23e8b664dafba7d927f4925b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;362.5607,-253.7655;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;36;514.5634,-271.3256;Inherit;False;emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;495.9026,-414.0622;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;39;99.71381,-155.0134;Inherit;False;Property;_EmissionScale;Emission Scale;4;0;Create;True;0;0;0;False;0;False;2;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-31.47742,-267.8163;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;0;False;0;False;2.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-88.27715,-487.8263;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;138.5228,-482.8156;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.9433962;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;1;-329.6487,-563.9274;Inherit;False;Property;_Color0;Color 0;1;0;Create;True;0;0;0;False;0;False;0.06274509,0.4342914,0.7647059,0;0.6941177,0.8627451,0.9921568,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;49;-452.2538,-317.3092;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1227.86,-290.7453;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;8;-1353.532,-168.2318;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1390.544,-319.9785;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;51;-1264.464,-582.5347;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;53;-1270.215,-438.3064;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PowerNode;60;-1241.179,-870.8211;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1448.66,-807.8425;Inherit;False;Property;_proutPower;proutPower;6;0;Create;True;0;0;0;False;0;False;2.45;2.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-1183.179,-990.821;Inherit;False;Property;_grosproutMult;grosproutMult;7;0;Create;True;0;0;0;False;0;False;1.5;1.84;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;56;-495.2381,-520.2526;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-946.7468,-827.6337;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;54;-1089.299,-570.3536;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;55;-640.8427,-697.3961;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;62;-683.6695,-471.8979;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
WireConnection;0;0;40;0
WireConnection;0;2;41;0
WireConnection;0;9;50;0
WireConnection;0;11;42;0
WireConnection;21;0;6;0
WireConnection;21;1;22;0
WireConnection;23;0;24;0
WireConnection;23;1;21;0
WireConnection;28;0;25;0
WireConnection;28;1;27;0
WireConnection;31;0;28;0
WireConnection;31;1;32;0
WireConnection;34;0;31;0
WireConnection;25;0;23;0
WireConnection;50;0;49;0
WireConnection;50;1;9;0
WireConnection;13;0;45;0
WireConnection;45;1;7;0
WireConnection;35;0;15;0
WireConnection;35;1;39;0
WireConnection;36;0;35;0
WireConnection;33;0;15;0
WireConnection;4;0;1;0
WireConnection;4;1;49;0
WireConnection;15;0;4;0
WireConnection;15;1;16;0
WireConnection;49;0;56;0
WireConnection;49;1;13;0
WireConnection;7;0;14;0
WireConnection;7;1;8;0
WireConnection;8;0;6;0
WireConnection;60;0;54;0
WireConnection;60;1;61;0
WireConnection;56;0;55;0
WireConnection;58;0;60;0
WireConnection;58;1;59;0
WireConnection;54;0;51;0
WireConnection;54;1;53;0
WireConnection;55;0;62;0
WireConnection;62;0;58;0
ASEEND*/
//CHKSM=346498D70FD44630291ABFBD9B504E196F54B1CA