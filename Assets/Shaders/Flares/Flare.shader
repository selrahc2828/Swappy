// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Flare"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0
		_Comportement_Color("Comportement_Color", Color) = (0.745283,0.3339711,0.3339711,0)
		_NoiseSpeed("NoiseSpeed", Float) = 0.5
		_VertexOffsetSpeed("VertexOffsetSpeed", Float) = 0.2
		_VertexOffset("VertexOffset", Float) = 2
		_perlin3("perlin3", 2D) = "white" {}
		_Tiling("Tiling", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		float4 _ASEOutlineColor;
		float _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float _VertexOffsetSpeed;
		uniform float _VertexOffset;
		uniform sampler2D _perlin3;
		uniform float _Tiling;
		uniform float _NoiseSpeed;
		uniform float4 _Comportement_Color;


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
			float simplePerlin2D28 = snoise( ( ase_vertex3Pos + ( _Time.y * _VertexOffsetSpeed ) ).xy*2.0 );
			simplePerlin2D28 = simplePerlin2D28*0.5 + 0.5;
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float3 Offset_Vertex26 = ( ( simplePerlin2D28 * ase_worldNormal ) / _VertexOffset );
			v.vertex.xyz += Offset_Vertex26;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV11 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode11 = ( 0.0 + 9.11 * pow( 1.0 - fresnelNdotV11, 2.38 ) );
			float2 temp_cast_0 = (_Tiling).xx;
			float2 temp_cast_1 = (( _Time.y * _NoiseSpeed )).xx;
			float2 uv_TexCoord3 = i.uv_texcoord * temp_cast_0 + temp_cast_1;
			float4 temp_output_15_0 = ( fresnelNode11 + tex2D( _perlin3, uv_TexCoord3 ) );
			float4 temp_output_7_0 = ( temp_output_15_0 * _Comportement_Color );
			o.Albedo = temp_output_7_0.rgb;
			o.Emission = temp_output_7_0.rgb;
			o.Alpha = saturate( ( temp_output_15_0 + float4( 0.5,0,0,0 ) ) ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

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
				surfIN.worldPos = worldPos;
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
Node;AmplifyShaderEditor.ColorNode;1;-735.8022,173.6091;Inherit;False;Property;_Comportement_Color;Comportement_Color;0;0;Create;True;0;0;0;False;0;False;0.745283,0.3339711,0.3339711,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-243.1307,-146.4643;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-466.2201,-216.4736;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;11;-787.173,-363.1777;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;9.11;False;3;FLOAT;2.38;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-308.6236,109.3474;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;10;-165.888,143.1409;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;23;-1900.726,376.232;Inherit;False;1991.896;588.7762;Comment;10;33;32;31;30;29;28;27;26;25;24;Vertex Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1605.989,426.2318;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1850.726,521.7814;Inherit;False;Property;_VertexOffsetSpeed;VertexOffsetSpeed;2;0;Create;True;0;0;0;False;0;False;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-151.8291,624.9259;Inherit;False;Offset Vertex;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-1313.422,480.9226;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;28;-1096.422,482.9226;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;29;-1073.685,711.0081;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-820.1765,585.9568;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;31;-394.9732,616.251;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-619.9728,843.2515;Inherit;False;Property;_VertexOffset;VertexOffset;3;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;33;-1599.981,540.9224;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;2;-1596.109,-100.255;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1601.809,-25.10987;Inherit;False;Property;_NoiseSpeed;NoiseSpeed;1;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1423.108,-109.9099;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;135,-5;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Flare;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;True;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SamplerNode;35;-789.3173,-112.3533;Inherit;True;Property;_perlin3;perlin3;4;0;Create;True;0;0;0;False;0;False;-1;2eee1166aed9c394b8b372f3b1cfa8b4;2eee1166aed9c394b8b372f3b1cfa8b4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1060.549,-180.4877;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;20,20;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;36;-1264.976,-203.6772;Inherit;False;Property;_Tiling;Tiling;5;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
WireConnection;7;0;15;0
WireConnection;7;1;1;0
WireConnection;15;0;11;0
WireConnection;15;1;35;0
WireConnection;8;0;15;0
WireConnection;10;0;8;0
WireConnection;24;0;2;0
WireConnection;24;1;25;0
WireConnection;26;0;31;0
WireConnection;27;0;33;0
WireConnection;27;1;24;0
WireConnection;28;0;27;0
WireConnection;30;0;28;0
WireConnection;30;1;29;0
WireConnection;31;0;30;0
WireConnection;31;1;32;0
WireConnection;5;0;2;0
WireConnection;5;1;6;0
WireConnection;0;0;7;0
WireConnection;0;2;7;0
WireConnection;0;9;10;0
WireConnection;0;11;26;0
WireConnection;35;1;3;0
WireConnection;3;0;36;0
WireConnection;3;1;5;0
ASEEND*/
//CHKSM=18FB4AE3070577DBD3A2B579B97B8CD6928C9AC0