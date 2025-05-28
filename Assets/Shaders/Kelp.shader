// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Kelp"
{
	Properties
	{
		_Kelp_Base_color("Kelp_Base_color", 2D) = "white" {}
		_IntColor("IntColor", Color) = (0.1860092,0.4150943,0.4006006,0)
		_Cutoff( "Mask Clip Value", Float ) = 0.05
		_ExtColor1("ExtColor", Color) = (0.3794055,0.9245283,0.6184413,0)
		_KelpRadial("KelpRadial", 2D) = "white" {}
		_Speed("Speed", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Speed;
		uniform float4 _IntColor;
		uniform float4 _ExtColor1;
		uniform sampler2D _KelpRadial;
		uniform float4 _KelpRadial_ST;
		uniform sampler2D _Kelp_Base_color;
		uniform float4 _Kelp_Base_color_ST;
		uniform float _Cutoff = 0.05;


		float2 voronoihash7( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi7( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash7( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			
			 		}
			 	}
			}
			return F1;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float lerpResult14 = lerp( 2.95 , 3.05 , _SinTime.w);
			float time7 = ( _Time.y * _Speed );
			float2 voronoiSmoothId7 = 0;
			float2 coords7 = v.texcoord.xy * lerpResult14;
			float2 id7 = 0;
			float2 uv7 = 0;
			float fade7 = 0.5;
			float voroi7 = 0;
			float rest7 = 0;
			for( int it7 = 0; it7 <3; it7++ ){
			voroi7 += fade7 * voronoi7( coords7, time7, id7, uv7, 0,voronoiSmoothId7 );
			rest7 += fade7;
			coords7 *= 2;
			fade7 *= 0.5;
			}//Voronoi7
			voroi7 /= rest7;
			float temp_output_12_0 = ( voroi7 - 0.12 );
			float3 temp_cast_0 = (temp_output_12_0).xxx;
			v.vertex.xyz += temp_cast_0;
			v.vertex.w = 1;
		}

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
Node;AmplifyShaderEditor.ColorNode;3;-681.2158,-381.1451;Inherit;False;Property;_IntColor;IntColor;1;0;Create;True;0;0;0;False;0;False;0.1860092,0.4150943,0.4006006,0;0.3686274,0.8,0.6862744,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-679.2158,-211.1451;Inherit;False;Property;_ExtColor1;ExtColor;3;0;Create;True;0;0;0;False;0;False;0.3794055,0.9245283,0.6184413,0;0.3794053,0.9245283,0.6184413,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;2;-361.216,-146.145;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;5;-748.1705,-45.23254;Inherit;True;Property;_KelpRadial;KelpRadial;4;0;Create;True;0;0;0;False;0;False;-1;7593c01b685ada5468b5182b0733b03d;7593c01b685ada5468b5182b0733b03d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-414.5824,74.60468;Inherit;True;Property;_Kelp_Base_color;Kelp_Base_color;0;0;Create;True;0;0;0;False;0;False;-1;88724dc8d40a28142826451606dec818;beab44c6b8f0c3643a594c7cd8703ff8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;6;-613.5413,275.1931;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;12;-414.3099,471.3044;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.12;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-207.5221,522.7776;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;-980.4011,435.2056;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-780.4011,449.2056;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-948.4011,514.2056;Inherit;False;Property;_Speed;Speed;5;0;Create;True;0;0;0;False;0;False;0;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;14;-799.9833,606.9681;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;13;-944.0802,745.1881;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;7;-607.5078,473.932;Inherit;True;0;0;1;0;3;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;-0.72;False;2;FLOAT;3;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;15;-957.9833,588.9681;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;2.95;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-962.9833,657.9681;Inherit;False;Constant;_Float1;Float 1;6;0;Create;True;0;0;0;False;0;False;3.05;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-0.8000038,20.7;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Kelp;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Masked;0.05;True;True;0;False;TransparentCutout;;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;1;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;2;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;3;0
WireConnection;2;1;4;0
WireConnection;2;2;5;0
WireConnection;12;0;7;0
WireConnection;11;0;6;0
WireConnection;11;1;12;0
WireConnection;10;0;8;0
WireConnection;10;1;9;0
WireConnection;14;0;15;0
WireConnection;14;1;16;0
WireConnection;14;2;13;4
WireConnection;7;1;10;0
WireConnection;7;2;14;0
WireConnection;0;0;2;0
WireConnection;0;9;1;0
WireConnection;0;10;1;0
WireConnection;0;11;12;0
ASEEND*/
//CHKSM=4492AE4F6F7AA57338A09553C5D772FB46063749