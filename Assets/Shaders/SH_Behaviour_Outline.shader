// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Behaviour_Split"
{
	Properties
	{
		_Color_1("Color_1", Color) = (0.8207547,0.6651516,0.1897027,0)
		_Color_2("Color_2", Color) = (0.2415005,0.4518574,0.764151,0)
		_Perlin_Dezoom2("Perlin_Dezoom2", 2D) = "white" {}
		_Perlin_Dezoom3("Perlin_Dezoom2", 2D) = "white" {}
		_speed("speed", Float) = -0.6
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
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
			float2 uv_texcoord;
		};

		uniform float4 _Color_1;
		uniform float4 _Color_2;
		uniform sampler2D _Perlin_Dezoom2;
		uniform float _speed;
		uniform sampler2D _Perlin_Dezoom3;


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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime171 = _Time.y * _speed;
			float temp_output_183_0 = ( mulTime171 / 10.0 );
			float2 temp_cast_0 = (temp_output_183_0).xx;
			float2 uv_TexCoord182 = i.uv_texcoord * float2( 0.5,0.5 ) + temp_cast_0;
			float2 temp_cast_2 = (temp_output_183_0).xx;
			float2 uv_TexCoord168 = i.uv_texcoord + temp_cast_2;
			float simplePerlin2D176 = snoise( tex2D( _Perlin_Dezoom2, uv_TexCoord182 ).rg*tex2D( _Perlin_Dezoom3, uv_TexCoord168 ).r );
			float4 lerpResult147 = lerp( _Color_1 , _Color_2 , saturate( ( simplePerlin2D176 * 3.26 ) ));
			float4 Color23 = lerpResult147;
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
Node;AmplifyShaderEditor.CommentaryNode;141;-4135.287,954.8141;Inherit;False;2530.495;885.9614;Comment;17;126;127;29;135;136;116;121;123;115;124;114;119;125;113;38;42;43;Anciens Tests;1,1,1,1;0;0
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
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;-2976.536,1608.837;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-3116.448,1724.442;Inherit;False;Property;_Scale;Scale;4;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;135;-2572.445,1025.966;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-2339.955,1004.814;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;116;-3453.764,1655.776;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;119;-3188.105,1553.555;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-2054.898,1420.182;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalizeNode;43;-1782.792,1389.444;Inherit;False;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;38;-2818.957,1570.645;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;dc5a7afc544c2564bb0f913574b6eba3;dc5a7afc544c2564bb0f913574b6eba3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;114;-3816.182,1425.97;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TransformPositionNode;113;-4055.183,1330.97;Inherit;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;125;-4085.287,1479.398;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;29;-2388.106,1341.977;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;-3419.099,1395.185;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;-3611.099,1396.185;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldToObjectMatrix;115;-3492.181,1501.97;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.ViewMatrixNode;124;-3651.099,1508.186;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.ColorNode;137;-2427.668,-93.83956;Inherit;False;Property;_Color_1;Color_1;5;0;Create;True;0;0;0;False;0;False;0.8207547,0.6651516,0.1897027,0;0.8207547,0.6651516,0.1897027,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;138;-2423.732,75.17727;Inherit;False;Property;_Color_2;Color_2;6;0;Create;True;0;0;0;False;0;False;0.2415005,0.4518574,0.764151,0;0.2415005,0.4518574,0.764151,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;147;-2048.964,113.2852;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;161;-2238.886,276.0663;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;176;-3225.84,296.9077;Inherit;True;Simplex2D;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1.32;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-1245.927,159.9707;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;162;-2518.02,275.4951;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;3.26;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;171;-4106.608,333.2898;Inherit;False;1;0;FLOAT;-2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;173;-4257.624,335.321;Inherit;False;Property;_speed;speed;9;0;Create;True;0;0;0;False;0;False;-0.6;-0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;183;-3920.011,332.0848;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;191;-3540.28,145.5945;Inherit;True;Property;_Perlin_Dezoom3;Perlin_Dezoom2;8;0;Create;True;0;0;0;False;0;False;-1;1353f9f49cf3a1c4f8486e37ebf5db93;1353f9f49cf3a1c4f8486e37ebf5db93;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;168;-3807.546,146.0868;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;181;-3538.314,466.065;Inherit;True;Property;_Perlin_Dezoom2;Perlin_Dezoom2;7;0;Create;True;0;0;0;False;0;False;-1;1353f9f49cf3a1c4f8486e37ebf5db93;1353f9f49cf3a1c4f8486e37ebf5db93;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;182;-3766.58,484.9083;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;18;0;12;4
WireConnection;20;0;13;0
WireConnection;13;0;14;0
WireConnection;13;1;16;0
WireConnection;16;0;15;0
WireConnection;10;0;21;0
WireConnection;126;0;119;0
WireConnection;126;1;127;0
WireConnection;136;1;38;0
WireConnection;119;0;121;0
WireConnection;119;1;116;0
WireConnection;42;1;29;0
WireConnection;43;0;42;0
WireConnection;38;1;126;0
WireConnection;114;0;113;0
WireConnection;114;1;125;0
WireConnection;29;2;38;0
WireConnection;121;0;123;0
WireConnection;121;1;115;0
WireConnection;123;0;114;0
WireConnection;123;1;124;0
WireConnection;147;0;137;0
WireConnection;147;1;138;0
WireConnection;147;2;161;0
WireConnection;161;0;162;0
WireConnection;176;0;181;0
WireConnection;176;1;191;0
WireConnection;23;0;147;0
WireConnection;162;0;176;0
WireConnection;171;0;173;0
WireConnection;183;0;171;0
WireConnection;191;1;168;0
WireConnection;168;1;183;0
WireConnection;181;1;182;0
WireConnection;182;1;183;0
ASEEND*/
//CHKSM=92BE4AABBCCB4D976538E6C8B841B098B40160CA