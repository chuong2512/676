Shader "Custom/CardBurn" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_SampleTex ("SampleTex", 2D) = "white" {}
		_ThreshHold ("Threshhold", Range(0, 1)) = 0
		_InsideCol ("内圈颜色", Vector) = (1,1,1,1)
		_OutsideCol ("外圈颜色", Vector) = (1,1,1,1)
		_BurnRange ("燃烧宽度", Range(0, 0.3)) = 0.05
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
			Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
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
			GpuProgramID 1351
			CGPROGRAM
			Program "vp" {
				SubProgram "d3d11 " {
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
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[2];
						vec4 _MainTex_ST;
						vec4 unused_0_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_1_1[7];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_COLOR0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
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
						vec4 unused_0_0[3];
						float _ThreshHold;
						float _BurnRange;
						vec4 _InsideCol;
						vec4 _OutsideCol;
					};
					uniform  sampler2D _SampleTex;
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					float u_xlat4;
					bool u_xlatb4;
					void main()
					{
					    u_xlat0.x = (-vs_TEXCOORD0.y) + 0.5;
					    u_xlat0.x = abs(u_xlat0.x) + abs(u_xlat0.x);
					    u_xlat4 = (-_ThreshHold) + 1.0;
					    u_xlat4 = sin(u_xlat4);
					    u_xlat1 = texture(_SampleTex, vs_TEXCOORD0.xy);
					    u_xlat4 = u_xlat4 * u_xlat1.x;
					    u_xlat0.x = u_xlat0.x * (-u_xlat4) + u_xlat4;
					    u_xlat0.y = u_xlat0.x + vs_TEXCOORD0.y;
					    u_xlat0.x = vs_TEXCOORD0.x;
					    u_xlat0 = texture(_MainTex, u_xlat0.xy);
					    u_xlat2 = u_xlat0 + (-_InsideCol);
					    u_xlat1.x = (-u_xlat1.z) + _ThreshHold;
					    u_xlat1.x = u_xlat1.x * 3.0;
					    u_xlat2 = u_xlat1.xxxx * u_xlat2 + _InsideCol;
					    u_xlat3 = u_xlat0 + (-u_xlat2);
					    u_xlat2 = vec4(_ThreshHold) * u_xlat3 + u_xlat2;
					    u_xlat0.x = u_xlat1.z + (-_ThreshHold);
					    u_xlatb4 = _ThreshHold<u_xlat1.z;
					    u_xlat0.x = (-u_xlat0.x) + _BurnRange;
					    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
					    u_xlat0.x = min(u_xlat0.x, _ThreshHold);
					    u_xlat0.x = u_xlat0.x / _BurnRange;
					    u_xlat1 = _InsideCol + (-_OutsideCol);
					    u_xlat1 = u_xlat0.xxxx * u_xlat1 + _OutsideCol;
					    u_xlat1 = u_xlat0.xxxx * u_xlat1;
					    u_xlat1 = (bool(u_xlatb4)) ? u_xlat1 : u_xlat2;
					    u_xlat1.w = u_xlat0.w * u_xlat1.w;
					    SV_Target0 = u_xlat1 * vs_COLOR0;
					    return;
					}
				}
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}