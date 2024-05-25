// Upgrade NOTE: upgraded instancing buffer 'MetaMonkeysInvisibleWall' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MetaMonkeys/InvisibleWall"
{
	Properties
	{
		[Header(Fade)]_fadelength("fade length", Float) = 0
		_depthfade("depth fade", Float) = 0
		[Header(Main)]_maintexture("main texture", 2D) = "white" {}
		[HDR]_maincol("main col", Color) = (0,0,0,0)
		_maintilling("main tilling", Float) = 0
		_maintillingXY("main tilling XY", Vector) = (0,0,0,0)
		_mainpanXY("main pan XY", Vector) = (0,0,0,0)
		[Header(Blink)]_blinkspeed("blink speed", Float) = 0
		_blinkmin("blink min", Range( 0 , 1)) = 0
		[Header(Scanline)]_scanlinespeed("scanline speed", Float) = 0
		_scanlinefrequency("scanline frequency", Float) = 0
		_scanlinemin("scanline min", Range( 0 , 1)) = 0
		[NoKeywordToggle]_uvdir("uv dir", Float) = 0
		[Header(Distortion)][NoScaleOffset]_distortiontexture("distortion texture", 2D) = "white" {}
		_distortionspeed("distortion speed", Float) = 0
		_distortionintensity("distortion intensity", Float) = 0
		_distortiontiling("distortion tiling", Float) = 0
		[Header(Additional Glitch)]_additionalglitch("additional glitch", 2D) = "white" {}
		_addglitchlerp("add glitch lerp", Range( 0 , 1)) = 0
		_addglitchspeed("add glitch speed", Float) = 0
		_addglitchdensity("add glitch density", Float) = 0
		_addglitchtiling("add glitch tiling", Vector) = (0,0,0,0)
		[Header(Add Glitch Color)]_addglitchhue("add glitch hue", Range( 0 , 1)) = 0
		_addglitchsaturation("add glitch saturation", Float) = 0
		_addglitchvalue("add glitch value", Float) = 0
		[Header(Add Glitch Pulse)]_addglitchpulsespeed("add glitch pulse speed", Float) = 2.36
		_addglitchsmstep("add glitch smstep", Range( 0 , 1)) = 0
		[Header(Additional)][Enum(UnityEngine.Rendering.CullMode)]_cullmode("cull mode", Float) = 0
		[NoKeywordToggle]_zwrite("z write", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull [_cullmode]
		ZWrite [_zwrite]
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 4.5
		#pragma multi_compile_instancing
		#pragma surface surf Unlit keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float eyeDepth;
			float4 screenPos;
		};

		uniform float _zwrite;
		uniform float _cullmode;
		uniform sampler2D _maintexture;
		uniform float2 _mainpanXY;
		uniform float2 _maintillingXY;
		uniform sampler2D _distortiontexture;
		uniform float _distortiontiling;
		uniform float _distortionspeed;
		uniform float _distortionintensity;
		uniform float _addglitchsmstep;
		uniform float _addglitchpulsespeed;
		uniform sampler2D _additionalglitch;
		uniform float2 _addglitchtiling;
		uniform float _addglitchspeed;
		uniform float _addglitchdensity;
		uniform float _addglitchhue;
		uniform float _addglitchsaturation;
		uniform float _addglitchvalue;
		uniform float _addglitchlerp;
		uniform float4 _maincol;
		uniform float _fadelength;
		uniform float _blinkspeed;
		uniform float _blinkmin;
		uniform float _uvdir;
		uniform float _scanlinefrequency;
		uniform float _scanlinespeed;
		uniform float _scanlinemin;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _depthfade;

		UNITY_INSTANCING_BUFFER_START(MetaMonkeysInvisibleWall)
			UNITY_DEFINE_INSTANCED_PROP(float, _maintilling)
#define _maintilling_arr MetaMonkeysInvisibleWall
		UNITY_INSTANCING_BUFFER_END(MetaMonkeysInvisibleWall)


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


		float3 HSVToRGB( float3 c )
		{
			float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
			float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
			return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
		}


		float3 RGBToHSV(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
			float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
			float d = q.x - min( q.w, q.y );
			float e = 1.0e-10;
			return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float _maintilling_Instance = UNITY_ACCESS_INSTANCED_PROP(_maintilling_arr, _maintilling);
			float2 uv_TexCoord9 = i.uv_texcoord * ( _maintilling_Instance * _maintillingXY );
			float2 panner10 = ( 1.0 * _Time.y * _mainpanXY + uv_TexCoord9);
			float2 temp_cast_0 = (_distortiontiling).xx;
			float2 uv_TexCoord57 = i.uv_texcoord * temp_cast_0;
			float mulTime63 = _Time.y * _distortionspeed;
			float2 appendResult89 = (float2(uv_TexCoord57.x , ( uv_TexCoord57.y + mulTime63 )));
			float scanline256 = tex2D( _distortiontexture, appendResult89 ).r;
			float temp_output_82_0 = ( scanline256 * _distortionintensity * 0.01 );
			float2 appendResult81 = (float2(temp_output_82_0 , ( temp_output_82_0 * 0.2 )));
			float mulTime211 = _Time.y * _addglitchpulsespeed;
			float2 temp_cast_1 = (mulTime211).xx;
			float simplePerlin2D212 = snoise( temp_cast_1 );
			simplePerlin2D212 = simplePerlin2D212*0.5 + 0.5;
			float smoothstepResult221 = smoothstep( _addglitchsmstep , 1.0 , simplePerlin2D212);
			float addglitchpluse229 = smoothstepResult221;
			float2 uv_TexCoord154 = i.uv_texcoord * _addglitchtiling;
			float div166=256.0/float((int)3.0);
			float4 posterize166 = ( floor( float4( uv_TexCoord154, 0.0 , 0.0 ) * div166 ) / div166 );
			float4 tex2DNode245 = tex2D( _additionalglitch, posterize166.rg );
			float mulTime174 = _Time.y * _addglitchspeed;
			float4 temp_cast_5 = (( _addglitchdensity * 0.05 )).xxxx;
			float3 hsvTorgb203 = RGBToHSV( step( frac( ( ( tex2DNode245 + mulTime174 ) * tex2DNode245 ) ) , temp_cast_5 ).rgb );
			float3 hsvTorgb204 = HSVToRGB( float3(( hsvTorgb203.x + _addglitchhue ),( hsvTorgb203.y * _addglitchsaturation ),( hsvTorgb203.z * _addglitchvalue )) );
			float3 lerpResult227 = lerp( float3( 0,0,0 ) , hsvTorgb204 , _addglitchlerp);
			float3 addglitch159 = ( lerpResult227 * addglitchpluse229 );
			float grayscale209 = Luminance(addglitch159);
			float addglitchuv219 = ( addglitchpluse229 * grayscale209 );
			float2 mainuv96 = ( panner10 + appendResult81 + addglitchuv219 );
			float4 tex2DNode1 = tex2D( _maintexture, mainuv96 );
			o.Emission = ( ( float4( (tex2DNode1).rgb , 0.0 ) * _maincol ) + float4( addglitch159 , 0.0 ) ).rgb;
			float cameraDepthFade2 = (( i.eyeDepth -_ProjectionParams.y - 0.0 ) / _fadelength);
			float mulTime18 = _Time.y * _blinkspeed;
			float2 temp_cast_10 = (mulTime18).xx;
			float simplePerlin2D15 = snoise( temp_cast_10 );
			simplePerlin2D15 = simplePerlin2D15*0.5 + 0.5;
			float lerpResult255 = lerp( i.uv_texcoord.y , i.uv_texcoord.x , _uvdir);
			float mulTime43 = _Time.y * _scanlinespeed;
			float temp_output_42_0 = ( ( lerpResult255 * _scanlinefrequency ) + mulTime43 );
			float scanline50 = (_scanlinemin + (( 1.0 - saturate( abs( ( temp_output_42_0 % 1.0 ) ) ) ) - 0.0) * (1.0 - _scanlinemin) / (1.0 - 0.0));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth247 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth247 = saturate( abs( ( screenDepth247 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _depthfade ) ) );
			o.Alpha = ( ( tex2DNode1.a + addglitch159 ) * ( 1.0 - saturate( cameraDepthFade2 ) ) * _maincol.a * (_blinkmin + (simplePerlin2D15 - 0.0) * (1.0 - _blinkmin) / (1.0 - 0.0)) * scanline50 * distanceDepth247 ).x;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
324;99;1023;596;-108.6983;862.7172;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;226;-393.7305,-1579.642;Inherit;False;3781.597;760.837;Comment;26;159;227;228;204;205;203;206;179;178;189;175;180;173;174;177;166;207;154;172;230;231;245;249;250;251;252;add glitch;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;172;-343.7306,-1391.53;Inherit;False;Property;_addglitchtiling;add glitch tiling;22;0;Create;True;0;0;0;False;0;False;0,0;0,5.46;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;207;-134.141,-1312.91;Inherit;False;Constant;_addglitchposterize;add glitch posterize;26;0;Create;True;0;0;0;False;0;False;3;2.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;154;-162.9926,-1450.401;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;177;555.8591,-1227.022;Inherit;False;Property;_addglitchspeed;add glitch speed;20;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosterizeNode;166;116.03,-1529.642;Inherit;True;1;2;1;COLOR;0,0,0,0;False;0;INT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;174;752.2726,-1224.983;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;245;538.1609,-1444.876;Inherit;True;Property;_additionalglitch;additional glitch;18;1;[Header];Create;True;1;Additional Glitch;0;0;False;0;False;-1;None;6fe68773ad25aa24ab24d56915b9cae2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;173;996.9551,-1448.339;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;233;-2750.658,50.31693;Inherit;False;1231.639;409.049;Comment;6;229;222;221;212;211;246;addglitch pulse;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;175;1121.388,-1234.777;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;180;1268.847,-1422.487;Inherit;False;Property;_addglitchdensity;add glitch density;21;0;Create;True;0;0;0;False;0;False;0;1.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;1512.754,-1395.867;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;178;1411.606,-1235.641;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;246;-2736.143,128.8023;Inherit;False;Property;_addglitchpulsespeed;add glitch pulse speed;26;1;[Header];Create;True;1;Add Glitch Pulse;0;0;False;0;False;2.36;0.89;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;179;1641.5,-1230.673;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;211;-2700.658,217.9776;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;203;1879.212,-1284.451;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;65;-987.9996,1659.262;Inherit;False;1650.59;446.0603;Comment;8;56;53;63;57;58;64;89;90;distortion ;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;252;1879.88,-1117.581;Inherit;False;Property;_addglitchvalue;add glitch value;25;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;212;-2490.002,143.3539;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;206;1903.512,-1398.051;Inherit;False;Property;_addglitchhue;add glitch hue;23;1;[Header];Create;True;1;Add Glitch Color;0;0;False;0;False;0;0.676;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;250;1857.038,-975.2136;Inherit;False;Property;_addglitchsaturation;add glitch saturation;24;0;Create;True;0;0;0;False;0;False;0;4.93;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-2328.527,361.6217;Inherit;False;Property;_addglitchsmstep;add glitch smstep;27;0;Create;True;0;0;0;False;0;False;0;0.585;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;249;2075.869,-1048.157;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-937.9996,1722.261;Inherit;False;Property;_distortiontiling;distortion tiling;17;0;Create;True;0;0;0;False;0;False;0;7.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;251;2143.202,-1203.864;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;205;2183.913,-1306.55;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;221;-1998.326,173.2218;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-929.9996,1890.262;Inherit;False;Property;_distortionspeed;distortion speed;15;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;229;-1722.32,187.1601;Inherit;False;addglitchpluse;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;228;2229.353,-1074.523;Inherit;False;Property;_addglitchlerp;add glitch lerp;19;0;Create;True;0;0;0;False;0;False;0;0.203;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;57;-703.9997,1709.261;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;63;-720.9996,1871.262;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;204;2334.712,-1261.051;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;230;2579.706,-998.8649;Inherit;False;229;addglitchpluse;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;227;2564.353,-1268.523;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;90;-453.367,1883.278;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;231;2849.891,-1228.43;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;89;-274.5928,1750.195;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;159;3088.232,-1231.08;Inherit;False;addglitch;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;53;-61.60692,1739.895;Inherit;True;Property;_distortiontexture;distortion texture;14;2;[Header];[NoScaleOffset];Create;True;1;Distortion;0;0;False;0;False;-1;None;edf1860336e21fd4eac3cfb3641af42c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;49;-1042.47,910.2811;Inherit;False;2065.16;652.8003;Comment;17;69;50;48;51;46;74;73;71;42;72;70;43;44;41;38;255;256;Scanline;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;216;-2738.009,-493.5012;Inherit;False;1166.92;483.6185;Comment;5;219;210;232;209;193;addglitch uv;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;38;-997.4707,960.2813;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;347.1795,1883.506;Inherit;False;scanline2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;95;-2769.311,-1413.083;Inherit;False;1748.534;802.5779;Comment;16;96;31;81;10;220;86;11;9;82;13;92;12;83;84;32;14;main uv;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;193;-2658.867,-313.219;Inherit;True;159;addglitch;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;256;-896.061,1108.706;Inherit;False;Property;_uvdir;uv dir;13;1;[NoKeywordToggle];Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;-2375.293,-1002.202;Inherit;False;56;scanline2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;14;-2719.311,-1235.588;Inherit;False;Property;_maintillingXY;main tilling XY;6;0;Create;True;0;0;0;False;0;False;0,0;0.25,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;84;-2308.322,-810.2939;Inherit;False;Constant;_Float3;Float 3;16;0;Create;True;0;0;0;False;0;False;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;232;-2406.8,-408.7128;Inherit;False;229;addglitchpluse;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;255;-731.2629,1045.803;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;209;-2411.551,-323.2717;Inherit;True;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-719.1053,1394.848;Inherit;False;Property;_scanlinespeed;scanline speed;10;1;[Header];Create;True;1;Scanline;0;0;False;0;False;0;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2717.524,-1363.083;Inherit;False;InstancedProperty;_maintilling;main tilling;5;0;Create;True;0;0;0;False;0;False;0;45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-2375.21,-897.6685;Inherit;False;Property;_distortionintensity;distortion intensity;16;0;Create;True;0;0;0;False;0;False;0;3.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-694.1084,1308.456;Inherit;False;Property;_scanlinefrequency;scanline frequency;11;0;Create;True;0;0;0;False;0;False;0;4.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;210;-2118.052,-370.4522;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-443.4472,1030.726;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-2143.801,-750.8931;Inherit;False;Constant;_Float1;Float 1;18;0;Create;True;0;0;0;False;0;False;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;43;-510.6928,1375.496;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-2502.19,-1286.081;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-2081.698,-1005.169;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;219;-1841.825,-333.4308;Inherit;False;addglitchuv;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-302.8811,1214.452;Inherit;False;Constant;_Float2;Float 2;19;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-2325.466,-1282.294;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;11;-2261.959,-1165.107;Inherit;False;Property;_mainpanXY;main pan XY;7;0;Create;True;0;0;0;False;0;False;0,0;0.1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-246.8031,1011.655;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-1974.035,-887.2549;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;220;-1832.177,-836.1841;Inherit;False;219;addglitchuv;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleRemainderNode;71;-119.881,1167.452;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;81;-1883.059,-1029.936;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;10;-2006.099,-1202.768;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-1637.417,-1087.616;Inherit;True;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;73;-105.881,1264.452;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;96;-1284.485,-1085.004;Inherit;False;mainuv;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;235;-1002.362,362.2848;Inherit;False;1059.523;399.5055;Comment;6;19;18;21;15;20;192;blink;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;74;-118.881,1342.452;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;244;-899.6234,69.85883;Inherit;False;890.7758;236.231;Comment;4;4;2;6;7;fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;46;156.0981,999.0402;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;105.0893,1259.609;Inherit;False;Property;_scanlinemin;scanline min;12;0;Create;True;0;0;0;False;0;False;0;0.643;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-952.3625,466.5405;Inherit;False;Property;_blinkspeed;blink speed;8;1;[Header];Create;True;1;Blink;0;0;False;0;False;0;12.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;97;-528.4897,-165.7176;Inherit;False;96;mainuv;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-849.6234,186.4349;Inherit;False;Property;_fadelength;fade length;1;1;[Header];Create;True;1;Fade;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-326.0788,-195.1089;Inherit;True;Property;_maintexture;main texture;3;1;[Header];Create;True;1;Main;0;0;False;0;False;-1;None;3ec2163f985e75048b13970dfe206565;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;18;-778.1621,446.3435;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CameraDepthFade;2;-660.5217,147.0898;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;48;429.5406,1026.175;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;171;207.4714,-54.79404;Inherit;False;159;addglitch;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;6;-411.8441,123.1058;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;23;-141.5728,-442.6684;Inherit;False;Property;_maincol;main col;4;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;2.975383,2.975383,2.975383,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-625.4211,645.7902;Inherit;False;Property;_blinkmin;blink min;9;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;212.6187,383.9498;Inherit;False;Property;_depthfade;depth fade;2;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;50;485.5295,1297.108;Inherit;False;scanline;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;75;-25.84454,-169.3161;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;15;-565.7056,412.2848;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;170;430.1543,15.48343;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;304.3904,205.8438;Inherit;False;50;scanline;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;247;382.4888,351.8483;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;173.6254,-185.8215;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;151;554.6056,-600.8096;Inherit;False;613;370.1;Comment;3;150;152;153;adds;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemapNode;20;-232.839,424.884;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;7;-187.8477,119.8588;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;192;-196.0882,645.6322;Inherit;False;blinknoise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;152;789.2018,-551.4221;Inherit;False;Property;_alphatocoverrage;alpha to coverrage;30;1;[NoKeywordToggle];Create;True;0;0;1;;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;153;613.9017,-463.1183;Inherit;False;Property;_zwrite;z write;29;1;[NoKeywordToggle];Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;150;603.6056,-550.8096;Inherit;False;Property;_cullmode;cull mode;28;2;[Header];[Enum];Create;True;1;Additional;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;258;-548.428,848.0983;Inherit;False;Property;_Float0;Float 0;31;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;234;573.9784,39.01664;Inherit;False;6;6;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FractNode;69;-74.24738,1017.726;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;169;422.4714,-142.794;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;722.4155,-151.1911;Float;False;True;-1;5;ASEMaterialInspector;0;0;Unlit;MetaMonkeys/InvisibleWall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;True;153;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;True;150;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;152;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;154;0;172;0
WireConnection;166;1;154;0
WireConnection;166;0;207;0
WireConnection;174;0;177;0
WireConnection;245;1;166;0
WireConnection;173;0;245;0
WireConnection;173;1;174;0
WireConnection;175;0;173;0
WireConnection;175;1;245;0
WireConnection;189;0;180;0
WireConnection;178;0;175;0
WireConnection;179;0;178;0
WireConnection;179;1;189;0
WireConnection;211;0;246;0
WireConnection;203;0;179;0
WireConnection;212;0;211;0
WireConnection;249;0;203;2
WireConnection;249;1;250;0
WireConnection;251;0;203;3
WireConnection;251;1;252;0
WireConnection;205;0;203;1
WireConnection;205;1;206;0
WireConnection;221;0;212;0
WireConnection;221;1;222;0
WireConnection;229;0;221;0
WireConnection;57;0;58;0
WireConnection;63;0;64;0
WireConnection;204;0;205;0
WireConnection;204;1;249;0
WireConnection;204;2;251;0
WireConnection;227;1;204;0
WireConnection;227;2;228;0
WireConnection;90;0;57;2
WireConnection;90;1;63;0
WireConnection;231;0;227;0
WireConnection;231;1;230;0
WireConnection;89;0;57;1
WireConnection;89;1;90;0
WireConnection;159;0;231;0
WireConnection;53;1;89;0
WireConnection;56;0;53;1
WireConnection;255;0;38;2
WireConnection;255;1;38;1
WireConnection;255;2;256;0
WireConnection;209;0;193;0
WireConnection;210;0;232;0
WireConnection;210;1;209;0
WireConnection;70;0;255;0
WireConnection;70;1;41;0
WireConnection;43;0;44;0
WireConnection;13;0;12;0
WireConnection;13;1;14;0
WireConnection;82;0;32;0
WireConnection;82;1;83;0
WireConnection;82;2;84;0
WireConnection;219;0;210;0
WireConnection;9;0;13;0
WireConnection;42;0;70;0
WireConnection;42;1;43;0
WireConnection;86;0;82;0
WireConnection;86;1;92;0
WireConnection;71;0;42;0
WireConnection;71;1;72;0
WireConnection;81;0;82;0
WireConnection;81;1;86;0
WireConnection;10;0;9;0
WireConnection;10;2;11;0
WireConnection;31;0;10;0
WireConnection;31;1;81;0
WireConnection;31;2;220;0
WireConnection;73;0;71;0
WireConnection;96;0;31;0
WireConnection;74;0;73;0
WireConnection;46;0;74;0
WireConnection;1;1;97;0
WireConnection;18;0;19;0
WireConnection;2;0;4;0
WireConnection;48;0;46;0
WireConnection;48;3;51;0
WireConnection;6;0;2;0
WireConnection;50;0;48;0
WireConnection;75;0;1;0
WireConnection;15;0;18;0
WireConnection;170;0;1;4
WireConnection;170;1;171;0
WireConnection;247;0;248;0
WireConnection;22;0;75;0
WireConnection;22;1;23;0
WireConnection;20;0;15;0
WireConnection;20;3;21;0
WireConnection;7;0;6;0
WireConnection;192;0;15;0
WireConnection;234;0;170;0
WireConnection;234;1;7;0
WireConnection;234;2;23;4
WireConnection;234;3;20;0
WireConnection;234;4;52;0
WireConnection;234;5;247;0
WireConnection;69;0;42;0
WireConnection;169;0;22;0
WireConnection;169;1;171;0
WireConnection;0;2;169;0
WireConnection;0;9;234;0
ASEEND*/
//CHKSM=5F16E21E0F8CE2DA9DA3A95418BCF0D9B440AE8B