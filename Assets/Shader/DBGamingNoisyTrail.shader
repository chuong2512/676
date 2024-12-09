Shader "DBGaming/NoisyTrail" {
	Properties {
		_MainTex ("MainTexture", 2D) = "white" {}
		_Noise ("噪声图", 2D) = "white" {}
		_ColPower ("尾端颜色占比", Range(0, 2)) = 1
		_NoiseFlowSpeed ("滚动速度", Float) = 1
		_StartColor ("首端颜色", Vector) = (1,0,0,1)
		_EndColor ("尾端颜色", Vector) = (1,1,0,1)
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			GpuProgramID 9353
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
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_COLOR0;
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
						vec4 unused_0_0[2];
						vec4 _Noise_ST;
						vec4 _StartColor;
						vec4 _EndColor;
						float _NoiseFlowSpeed;
						float _ColPower;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _Noise;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec2 u_xlat3;
					vec2 u_xlat6;
					void main()
					{
					    u_xlat0.x = _NoiseFlowSpeed * _Time.y;
					    u_xlat0.y = 0.0;
					    u_xlat0.xy = vs_TEXCOORD0.xy * _Noise_ST.xy + u_xlat0.xy;
					    u_xlat0.xy = u_xlat0.xy + _Noise_ST.zw;
					    u_xlat0 = texture(_Noise, u_xlat0.xy);
					    u_xlat3.xy = (-vs_TEXCOORD0.xy) + vec2(1.0, 0.5);
					    u_xlat3.x = log2(u_xlat3.x);
					    u_xlat6.xy = -abs(u_xlat3.yy) + vec2(0.5, 0.460000008);
					    u_xlat3.x = u_xlat3.x * 0.800000012;
					    u_xlat3.x = exp2(u_xlat3.x);
					    u_xlat3.x = max(u_xlat3.x, 0.200000003);
					    u_xlat3.x = min(u_xlat3.x, 0.600000024);
					    u_xlat0.x = u_xlat3.x + u_xlat0.x;
					    u_xlat0.x = u_xlat0.x + (-vs_TEXCOORD0.x);
					    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat3.x = u_xlat1.x * vs_COLOR0.w;
					    u_xlat0.x = u_xlat0.x * u_xlat3.x;
					    u_xlat3.x = float(1.0) / u_xlat6.x;
					    u_xlat3.x = u_xlat3.x * u_xlat6.y;
					    u_xlat3.x = clamp(u_xlat3.x, 0.0, 1.0);
					    u_xlat6.x = u_xlat3.x * -2.0 + 3.0;
					    u_xlat3.x = u_xlat3.x * u_xlat3.x;
					    u_xlat3.x = u_xlat3.x * u_xlat6.x;
					    u_xlat0.x = u_xlat3.x * u_xlat0.x;
					    u_xlat3.x = log2(vs_TEXCOORD0.x);
					    u_xlat3.x = u_xlat3.x * _ColPower;
					    u_xlat3.x = exp2(u_xlat3.x);
					    u_xlat3.x = min(u_xlat3.x, 1.0);
					    u_xlat2 = (-_StartColor) + _EndColor;
					    u_xlat2 = u_xlat3.xxxx * u_xlat2 + _StartColor;
					    u_xlat0.x = u_xlat0.x * u_xlat2.w;
					    SV_Target0.xyz = u_xlat2.xyz * vs_COLOR0.xyz;
					    SV_Target0.w = u_xlat1.w * u_xlat0.x;
					    return;
					}
				}
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}