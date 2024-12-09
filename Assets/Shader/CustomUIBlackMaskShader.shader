Shader "Custom/UIBlackMaskShader" {
	Properties {
		_WidthBlackRate ("WidthBlackRate", Range(0, 1)) = 0
		_HeightBlackRate ("HeightBlackRate", Range(0, 1)) = 0
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" }
		Pass {
			Tags { "QUEUE" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			GpuProgramID 42223
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
					in  vec2 in_TEXCOORD0;
					out vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
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
						vec4 unused_0_0[2];
						float _WidthBlackRate;
						float _HeightBlackRate;
					};
					in  vec2 vs_TEXCOORD0;
					layout(location = 0) out vec4 SV_Target0;
					vec2 u_xlat0;
					bvec2 u_xlatb0;
					bvec2 u_xlatb2;
					void main()
					{
					    u_xlat0.xy = (-vs_TEXCOORD0.xy) + vec2(1.0, 1.0);
					    u_xlatb0.xy = lessThan(u_xlat0.xyxx, vec4(_WidthBlackRate, _HeightBlackRate, _WidthBlackRate, _WidthBlackRate)).xy;
					    u_xlatb2.xy = lessThan(vs_TEXCOORD0.xyxy, vec4(_WidthBlackRate, _HeightBlackRate, _WidthBlackRate, _HeightBlackRate)).xy;
					    {
					        bvec2 hlslcc_orTemp = u_xlatb0;
					        hlslcc_orTemp.x = u_xlatb0.x || u_xlatb2.x;
					        hlslcc_orTemp.y = u_xlatb0.y || u_xlatb2.y;
					        u_xlatb0 = hlslcc_orTemp;
					    }
					    u_xlatb0.x = u_xlatb0.y || u_xlatb0.x;
					    SV_Target0.w = u_xlatb0.x ? 1.0 : float(0.0);
					    SV_Target0.xyz = vec3(0.0, 0.0, 0.0);
					    return;
					}
				}
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}