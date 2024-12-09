Shader "Custom/FlowLight" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_LineWhip ("Line Whip", 2D) = "white" {}
		_FloatSpeedY ("Flow Speed Y", Float) = 2
		_FloatSpeedX ("Flow Speed X", Float) = 2
		_AlphaTune ("AlphaTune", Range(0, 1)) = 0.2
		_Threshhold ("颜色阈值", Range(0, 1)) = 0.5
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ColorMask 0 -1
			ZWrite Off
			Stencil {
				ReadMask 0
				WriteMask 0
			//	Comp Disabled
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 9075
			CGPROGRAM
			Program "vp" {
				SubProgram "d3d11 " {
					Keywords { "SHADOWS_DEPTH" }
					vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_0_1[7];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_1_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_1_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD1;
					out vec4 vs_Color0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD1.xy = in_TEXCOORD0.xy;
					    vs_Color0 = in_COLOR0;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "SHADOWS_CUBE" }
					vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_0_1[7];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_1_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_1_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD1;
					out vec4 vs_Color0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD1.xy = in_TEXCOORD0.xy;
					    vs_Color0 = in_COLOR0;
					    return;
					}
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					Keywords { "SHADOWS_DEPTH" }
					ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _LineWhip_ST;
						float _FloatSpeedY;
						float _FloatSpeedX;
						float _AlphaTune;
						float _Threshhold;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _LineWhip;
					in  vec2 vs_TEXCOORD1;
					in  vec4 vs_Color0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD1.xy * _LineWhip_ST.xy + _LineWhip_ST.zw;
					    u_xlat0.xy = vec2(_FloatSpeedX, _FloatSpeedY) * _Time.yy + u_xlat0.xy;
					    u_xlat0 = texture(_LineWhip, u_xlat0.xy);
					    u_xlat1.x = u_xlat0.w;
					    u_xlat2 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat1.yzw = u_xlat2.xyz;
					    u_xlat0 = u_xlat0 * u_xlat1.yzwx;
					    u_xlat1.w = u_xlat2.w;
					    u_xlat0 = u_xlat0 * u_xlat1.xxxw;
					    u_xlat0 = u_xlat0 * vec4(3.0, 3.0, 3.0, 3.0);
					    u_xlat1.x = u_xlat1.y + (-_Threshhold);
					    u_xlat1.x = clamp(u_xlat1.x, 0.0, 1.0);
					    u_xlat0 = u_xlat0 * u_xlat1.xxxx + u_xlat2;
					    u_xlat0.w = u_xlat0.w * _AlphaTune;
					    SV_Target0 = u_xlat0 * vs_Color0;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "SHADOWS_CUBE" }
					ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _LineWhip_ST;
						float _FloatSpeedY;
						float _FloatSpeedX;
						float _AlphaTune;
						float _Threshhold;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _LineWhip;
					in  vec2 vs_TEXCOORD1;
					in  vec4 vs_Color0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD1.xy * _LineWhip_ST.xy + _LineWhip_ST.zw;
					    u_xlat0.xy = vec2(_FloatSpeedX, _FloatSpeedY) * _Time.yy + u_xlat0.xy;
					    u_xlat0 = texture(_LineWhip, u_xlat0.xy);
					    u_xlat1.x = u_xlat0.w;
					    u_xlat2 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat1.yzw = u_xlat2.xyz;
					    u_xlat0 = u_xlat0 * u_xlat1.yzwx;
					    u_xlat1.w = u_xlat2.w;
					    u_xlat0 = u_xlat0 * u_xlat1.xxxw;
					    u_xlat0 = u_xlat0 * vec4(3.0, 3.0, 3.0, 3.0);
					    u_xlat1.x = u_xlat1.y + (-_Threshhold);
					    u_xlat1.x = clamp(u_xlat1.x, 0.0, 1.0);
					    u_xlat0 = u_xlat0 * u_xlat1.xxxx + u_xlat2;
					    u_xlat0.w = u_xlat0.w * _AlphaTune;
					    SV_Target0 = u_xlat0 * vs_Color0;
					    return;
					}
				}
			}
				ENDCG
		}
	}
	FallBack "Diffuse"
}