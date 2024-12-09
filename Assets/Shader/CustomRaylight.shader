Shader "Custom/Raylight" {
	Properties {
		[PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
		_LineWhip ("Line Whip", 2D) = "white" {}
		_FloatSpeedY ("Flow Speed Y", Float) = 2
		_FloatSpeedX ("Flow Speed X", Float) = 2
		_AlphaTune ("AlphaTune", Range(0, 1.5)) = 1
		_DustIntense ("烟尘强度", Range(0, 1)) = 0.25
		_SmokeColor ("烟雾颜色", Vector) = (1,1,1,1)
	}
	SubShader {
		Tags { "CanUseSpriteAtlas" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 15500
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
					out vec4 vs_COLOR0;
					out vec2 vs_TEXCOORD1;
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
					    vs_COLOR0 = in_COLOR0;
					    vs_TEXCOORD1.xy = in_TEXCOORD0.xy;
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
					out vec4 vs_COLOR0;
					out vec2 vs_TEXCOORD1;
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
					    vs_COLOR0 = in_COLOR0;
					    vs_TEXCOORD1.xy = in_TEXCOORD0.xy;
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
						vec4 _SmokeColor;
						float _FloatSpeedY;
						float _FloatSpeedX;
						float _AlphaTune;
						float _DustIntense;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _LineWhip;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec2 u_xlat6;
					void main()
					{
					    u_xlat0.xy = vec2(_FloatSpeedX, _FloatSpeedY) * _Time.yy;
					    u_xlat6.xy = vs_TEXCOORD1.xy * _LineWhip_ST.xy + _LineWhip_ST.zw;
					    u_xlat6.xy = vec2(_FloatSpeedX, _FloatSpeedY) * _Time.yy + u_xlat6.xy;
					    u_xlat1.xy = u_xlat6.xy * _LineWhip_ST.xy + _LineWhip_ST.zw;
					    u_xlat2 = texture(_LineWhip, u_xlat6.xy);
					    u_xlat0.xy = u_xlat0.xy * vec2(0.5, 1.5) + u_xlat1.xy;
					    u_xlat0.xy = u_xlat0.xy * vec2(0.5, 0.5);
					    u_xlat0 = texture(_LineWhip, u_xlat0.xy);
					    u_xlat0.x = u_xlat0.z * 0.5;
					    u_xlat0.x = u_xlat2.z * 0.5 + u_xlat0.x;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat1 = u_xlat1 * vs_COLOR0;
					    u_xlat0.x = u_xlat0.x * u_xlat1.w;
					    u_xlat1.xyz = (-u_xlat0.xxx) * vec3(vec3(_DustIntense, _DustIntense, _DustIntense)) + u_xlat1.xyz;
					    u_xlat0 = u_xlat1 * _SmokeColor;
					    SV_Target0.w = u_xlat0.w * _AlphaTune;
					    SV_Target0.xyz = u_xlat0.xyz;
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
						vec4 _SmokeColor;
						float _FloatSpeedY;
						float _FloatSpeedX;
						float _AlphaTune;
						float _DustIntense;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _LineWhip;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec2 u_xlat6;
					void main()
					{
					    u_xlat0.xy = vec2(_FloatSpeedX, _FloatSpeedY) * _Time.yy;
					    u_xlat6.xy = vs_TEXCOORD1.xy * _LineWhip_ST.xy + _LineWhip_ST.zw;
					    u_xlat6.xy = vec2(_FloatSpeedX, _FloatSpeedY) * _Time.yy + u_xlat6.xy;
					    u_xlat1.xy = u_xlat6.xy * _LineWhip_ST.xy + _LineWhip_ST.zw;
					    u_xlat2 = texture(_LineWhip, u_xlat6.xy);
					    u_xlat0.xy = u_xlat0.xy * vec2(0.5, 1.5) + u_xlat1.xy;
					    u_xlat0.xy = u_xlat0.xy * vec2(0.5, 0.5);
					    u_xlat0 = texture(_LineWhip, u_xlat0.xy);
					    u_xlat0.x = u_xlat0.z * 0.5;
					    u_xlat0.x = u_xlat2.z * 0.5 + u_xlat0.x;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat1 = u_xlat1 * vs_COLOR0;
					    u_xlat0.x = u_xlat0.x * u_xlat1.w;
					    u_xlat1.xyz = (-u_xlat0.xxx) * vec3(vec3(_DustIntense, _DustIntense, _DustIntense)) + u_xlat1.xyz;
					    u_xlat0 = u_xlat1 * _SmokeColor;
					    SV_Target0.w = u_xlat0.w * _AlphaTune;
					    SV_Target0.xyz = u_xlat0.xyz;
					    return;
					}
				}
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}