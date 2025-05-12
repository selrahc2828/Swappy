// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Behaviour_Split"
{
	Properties
	{
		_TextureSample1("Texture Sample 0", 2D) = "white" {}
		_Color_1("Color_1", Color) = (0.8207547,0.6651516,0.1897027,0)
		_Color_2("Color_2", Color) = (0.2415005,0.4518574,0.764151,0)
		_raf360x360075tfafafa_ca443f4786("raf,360x360,075,t,fafafa_ca443f4786", 2D) = "white" {}
		_Emissive_Strenght("Emissive_Strenght", Float) = 2
		_Perlin_Dezoom2("Perlin_Dezoom2", 2D) = "white" {}
		_Perlin_Dezoom3("Perlin_Dezoom2", 2D) = "white" {}
		_Speed("Speed", Float) = -0.1
		_Speed1("Speed", Float) = 0.15
		_RotateSpeed("RotateSpeed", Float) = 20
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
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
		uniform sampler2D _raf360x360075tfafafa_ca443f4786;
		uniform float4 _raf360x360075tfafafa_ca443f4786_ST;
		uniform sampler2D _Perlin_Dezoom3;
		uniform float _Speed1;
		uniform float _RotateSpeed;
		uniform sampler2D _Perlin_Dezoom2;
		uniform float _Speed;
		uniform sampler2D _TextureSample1;
		uniform float _Emissive_Strenght;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_raf360x360075tfafafa_ca443f4786 = i.uv_texcoord * _raf360x360075tfafafa_ca443f4786_ST.xy + _raf360x360075tfafafa_ca443f4786_ST.zw;
			float mulTime245 = _Time.y * _Speed1;
			float cos263 = cos( _Time.y );
			float sin263 = sin( _Time.y );
			float2 rotator263 = mul( float2( 0,0 ) - float2( 0.1,0 ) , float2x2( cos263 , -sin263 , sin263 , cos263 )) + float2( 0.1,0 );
			float2 temp_output_265_0 = ( rotator263 / ( _RotateSpeed * 1000.0 ) );
			float2 uv_TexCoord242 = i.uv_texcoord * float2( 0.8,0.8 ) + ( mulTime245 * temp_output_265_0 );
			float mulTime223 = _Time.y * _Speed;
			float2 uv_TexCoord221 = i.uv_texcoord + ( mulTime223 * temp_output_265_0 );
			float temp_output_246_0 = ( (-1.27 + (tex2D( _Perlin_Dezoom3, uv_TexCoord242 ).r - 0.0) * (3.5 - -1.27) / (1.0 - 0.0)) * (-1.27 + (tex2D( _Perlin_Dezoom2, uv_TexCoord221 ).r - 0.0) * (3.5 - -1.27) / (1.0 - 0.0)) );
			float4 temp_cast_0 = (temp_output_246_0).xxxx;
			float temp_output_233_0 = ( 1.0 - i.uv_texcoord.x );
			float4 lerpResult228 = lerp( tex2D( _raf360x360075tfafafa_ca443f4786, uv_raf360x360075tfafafa_ca443f4786 ) , temp_cast_0 , saturate( ( ( saturate( ( ( ( i.uv_texcoord.x - 0.43 ) * ( temp_output_233_0 - 0.43 ) ) * 100.0 ) ) + saturate( ( ( i.uv_texcoord.x * temp_output_233_0 ) * 3.7 ) ) ) - 0.4 ) ));
			float cos270 = cos( radians( 90.0 ) );
			float sin270 = sin( radians( 90.0 ) );
			float2 rotator270 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos270 , -sin270 , sin270 , cos270 )) + float2( 0.5,0.5 );
			float4 temp_cast_1 = (temp_output_246_0).xxxx;
			float temp_output_278_0 = ( 1.0 - i.uv_texcoord.y );
			float4 lerpResult268 = lerp( tex2D( _TextureSample1, rotator270 ) , temp_cast_1 , saturate( ( ( saturate( ( ( ( i.uv_texcoord.y - 0.43 ) * ( temp_output_278_0 - 0.43 ) ) * 100.0 ) ) + saturate( ( ( i.uv_texcoord.y * temp_output_278_0 ) * 3.7 ) ) ) - 0.4 ) ));
			float4 lerpResult293 = lerp( lerpResult228 , lerpResult268 , 0.5);
			float4 lerpResult256 = lerp( _Color_1 , _Color_2 , saturate( ( lerpResult293 * float4( 1,1,1,0 ) ) ));
			float4 Color23 = ( lerpResult256 + float4( 0,0,0,0 ) );
			float4 temp_output_21_0 = Color23;
			o.Albedo = temp_output_21_0.rgb;
			o.Emission = ( Color23 / _Emissive_Strenght ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19200
Node;AmplifyShaderEditor.CommentaryNode;298;-4861.146,-408.9599;Inherit;False;1742.01;584.4876;Comment;14;229;234;247;233;248;236;249;251;253;237;254;255;250;238;Pour pas voir la scission propre;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;297;-5625.327,212.4013;Inherit;False;2517.245;643.5378;Comment;18;226;221;224;223;240;241;243;244;245;242;260;265;266;267;246;263;220;227;Texture qui bouge;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;296;-4849.127,872.8544;Inherit;False;1742.004;584.4891;Comment;14;275;276;277;280;281;282;283;284;285;287;278;279;286;288;Pour pas voir la scission propre 2;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;295;-4255.577,1491.256;Inherit;False;1151.571;495.4832;Comment;6;273;274;272;271;270;192;2e image input rotate;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;141;-1620.734,1961.324;Inherit;False;2530.495;885.9614;Comment;17;126;127;29;135;136;116;121;123;115;124;114;119;125;113;38;42;43;Anciens Tests;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;22;-1412.795,-930.6334;Inherit;False;834.7886;371.308;Comment;5;20;13;15;14;16;Outline SIMPLE;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;12;-605.2734,-429.8865;Inherit;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;-349.7715,-317.8427;Inherit;False;alphaMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;-820.0086,-798.7275;Inherit;False;Outline SIMPLE;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OutlineNode;13;-1066.133,-812.459;Inherit;False;1;True;None;0;0;Front;True;True;True;True;0;False;;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;14;-1315.538,-880.6334;Inherit;False;Property;_Color1;Color 1;1;0;Create;True;0;0;0;False;0;False;0.6792453,0.1890353,0.1890353,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;16;-1208.796,-696.3254;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;10;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Behaviour_Split;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Masked;1;True;True;0;False;TransparentCutout;;AlphaTest;ForwardOnly;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexScale;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-187.7715,240.1573;Inherit;False;18;alphaMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;-461.9819,2615.347;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-601.8941,2730.953;Inherit;False;Property;_Scale;Scale;5;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;135;-57.89122,2032.476;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;174.5989,2011.324;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;116;-939.2106,2662.286;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;119;-673.551,2560.065;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;459.6564,2426.692;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalizeNode;43;731.7629,2395.954;Inherit;False;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;114;-1301.629,2432.48;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TransformPositionNode;113;-1540.629,2337.481;Inherit;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;125;-1570.733,2485.908;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;29;126.4482,2348.488;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;-904.5457,2401.696;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;-1096.545,2402.696;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldToObjectMatrix;115;-977.6274,2508.48;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.ViewMatrixNode;124;-1136.546,2514.696;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.SamplerNode;38;-304.403,2577.155;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;dc5a7afc544c2564bb0f913574b6eba3;dc5a7afc544c2564bb0f913574b6eba3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;21;-407.6616,-0.0369873;Inherit;False;23;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;218;-186.2205,74.58192;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;219;-419.2205,163.5819;Inherit;False;Property;_Emissive_Strenght;Emissive_Strenght;9;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;137;-2088.108,-180;Inherit;False;Property;_Color_1;Color_1;6;0;Create;True;0;0;0;False;0;False;0.8207547,0.6651516,0.1897027,0;0,1,0.3468351,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;138;-2084.172,-10.98312;Inherit;False;Property;_Color_2;Color_2;7;0;Create;True;0;0;0;False;0;False;0.2415005,0.4518574,0.764151,0;0,0.668682,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;161;-1940.326,185.9063;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;162;-2083.46,186.3351;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;256;-1766.462,-80.34167;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1381.724,-681.0296;Inherit;False;Property;_Widht;Widht;2;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;228;-2568.427,141.0311;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;268;-2610.462,432.1892;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;293;-2205.168,351.1237;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;192;-3474.005,1589.612;Inherit;True;Property;_TextureSample1;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;dc5a7afc544c2564bb0f913574b6eba3;1353f9f49cf3a1c4f8486e37ebf5db93;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;275;-4799.127,922.8545;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;276;-4420.59,1117.228;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.43;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;277;-4419.312,939.2417;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.38;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;280;-4242.658,953.1236;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;281;-3827.985,1203.344;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;282;-4033.89,1200.469;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;3.7;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;283;-4228.37,1200.381;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;284;-3634.668,949.4422;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;285;-3844.601,951.4288;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;287;-3413.94,997.2485;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;278;-4688.43,1103.793;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;279;-4578.592,1037.426;Inherit;False;Constant;_Float2;Float 0;17;0;Create;True;0;0;0;False;0;False;0.43;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;286;-4038.894,948.5233;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;288;-3285.122,1112.011;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;226;-4325.926,599.8963;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;221;-4146.577,541.8195;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;224;-4657.37,572.7789;Inherit;False;Property;_Speed;Speed;12;0;Create;True;0;0;0;False;0;False;-0.1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;223;-4506.615,556.7789;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;240;-4336.49,335.5936;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;241;-3948.14,275.5169;Inherit;True;Property;_Perlin_Dezoom3;Perlin_Dezoom2;11;0;Create;True;0;0;0;False;0;False;-1;1353f9f49cf3a1c4f8486e37ebf5db93;1353f9f49cf3a1c4f8486e37ebf5db93;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;243;-3666.658,262.4013;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1.27;False;4;FLOAT;3.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;244;-4667.935,308.4764;Inherit;False;Property;_Speed1;Speed;13;0;Create;True;0;0;0;False;0;False;0.15;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;245;-4517.18,292.4764;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;242;-4157.141,277.5168;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.8,0.8;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;260;-5575.327,615.2253;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;265;-4897.938,582.939;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;266;-5355.585,742.9392;Inherit;False;Property;_RotateSpeed;RotateSpeed;14;0;Create;True;0;0;0;False;0;False;20;25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;267;-5126.38,697.7935;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1000;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;246;-3341.081,426.016;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;263;-5274.156,563.6974;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.1,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;220;-3937.575,539.8195;Inherit;True;Property;_Perlin_Dezoom2;Perlin_Dezoom2;10;0;Create;True;0;0;0;False;0;False;-1;1353f9f49cf3a1c4f8486e37ebf5db93;1353f9f49cf3a1c4f8486e37ebf5db93;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;227;-3656.094,526.7039;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1.27;False;4;FLOAT;3.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;229;-4811.146,-358.9599;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;234;-4432.609,-164.5878;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.43;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;247;-4431.331,-342.5726;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.38;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;233;-4752.449,-237.0229;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;-4575.611,-257.3895;Inherit;False;Constant;_Float0;Float 0;17;0;Create;True;0;0;0;False;0;False;0.43;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;236;-4254.677,-328.6911;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;249;-3840,-78.47227;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;251;-4240.389,-81.4354;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;253;-3646.683,-332.3724;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;237;-3856.618,-330.3858;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;254;-3425.956,-284.5664;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;255;-3297.137,-169.8047;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;250;-4045.91,-81.34739;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;3.7;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;238;-4053.212,-335.5898;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;217;-3445.82,-649.1844;Inherit;True;Property;_raf360x360075tfafafa_ca443f4786;raf,360x360,075,t,fafafa_ca443f4786;8;0;Create;True;0;0;0;False;0;False;-1;dc5a7afc544c2564bb0f913574b6eba3;dc5a7afc544c2564bb0f913574b6eba3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;270;-3949.512,1640.996;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;271;-4187.803,1554.463;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;272;-4148.193,1676.433;Inherit;False;Constant;_Vector0;Vector 0;18;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RadiansOpNode;274;-4054.347,1803.589;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;273;-4209.347,1809.589;Inherit;False;Constant;_Float1;Float 1;18;0;Create;True;0;0;0;False;0;False;90;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-1182.529,136.3175;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;299;-1702.802,162.0314;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;303;-1464.93,77.17014;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;294;-2376.47,608.9238;Inherit;False;Constant;_Float3;Float 3;18;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
WireConnection;18;0;12;4
WireConnection;20;0;13;0
WireConnection;13;0;14;0
WireConnection;13;1;16;0
WireConnection;16;0;15;0
WireConnection;10;0;21;0
WireConnection;10;2;218;0
WireConnection;126;0;119;0
WireConnection;126;1;127;0
WireConnection;136;1;38;0
WireConnection;119;0;121;0
WireConnection;119;1;116;0
WireConnection;42;1;29;0
WireConnection;43;0;42;0
WireConnection;114;0;113;0
WireConnection;114;1;125;0
WireConnection;29;2;38;0
WireConnection;121;0;123;0
WireConnection;121;1;115;0
WireConnection;123;0;114;0
WireConnection;123;1;124;0
WireConnection;38;1;126;0
WireConnection;218;0;21;0
WireConnection;218;1;219;0
WireConnection;161;0;162;0
WireConnection;162;0;293;0
WireConnection;256;0;137;0
WireConnection;256;1;138;0
WireConnection;256;2;161;0
WireConnection;228;0;217;0
WireConnection;228;1;246;0
WireConnection;228;2;255;0
WireConnection;268;0;192;0
WireConnection;268;1;246;0
WireConnection;268;2;288;0
WireConnection;293;0;228;0
WireConnection;293;1;268;0
WireConnection;293;2;294;0
WireConnection;192;1;270;0
WireConnection;276;0;278;0
WireConnection;276;1;279;0
WireConnection;277;0;275;2
WireConnection;277;1;279;0
WireConnection;280;0;277;0
WireConnection;280;1;276;0
WireConnection;281;0;282;0
WireConnection;282;0;283;0
WireConnection;283;0;275;2
WireConnection;283;1;278;0
WireConnection;284;0;285;0
WireConnection;284;1;281;0
WireConnection;285;0;286;0
WireConnection;287;0;284;0
WireConnection;278;0;275;2
WireConnection;286;0;280;0
WireConnection;288;0;287;0
WireConnection;226;0;223;0
WireConnection;226;1;265;0
WireConnection;221;1;226;0
WireConnection;223;0;224;0
WireConnection;240;0;245;0
WireConnection;240;1;265;0
WireConnection;241;1;242;0
WireConnection;243;0;241;1
WireConnection;245;0;244;0
WireConnection;242;1;240;0
WireConnection;265;0;263;0
WireConnection;265;1;267;0
WireConnection;267;0;266;0
WireConnection;246;0;243;0
WireConnection;246;1;227;0
WireConnection;263;2;260;0
WireConnection;220;1;221;0
WireConnection;227;0;220;1
WireConnection;234;0;233;0
WireConnection;234;1;248;0
WireConnection;247;0;229;1
WireConnection;247;1;248;0
WireConnection;233;0;229;1
WireConnection;236;0;247;0
WireConnection;236;1;234;0
WireConnection;249;0;250;0
WireConnection;251;0;229;1
WireConnection;251;1;233;0
WireConnection;253;0;237;0
WireConnection;253;1;249;0
WireConnection;237;0;238;0
WireConnection;254;0;253;0
WireConnection;255;0;254;0
WireConnection;250;0;251;0
WireConnection;238;0;236;0
WireConnection;270;0;271;0
WireConnection;270;1;272;0
WireConnection;270;2;274;0
WireConnection;274;0;273;0
WireConnection;23;0;303;0
WireConnection;303;0;256;0
ASEEND*/
//CHKSM=867B6A76DA52EF1AEFEC0008473E1ABAE41FC8DB