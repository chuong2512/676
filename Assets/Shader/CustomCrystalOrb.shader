Shader "Custom/CrystalOrb" {
	Properties {
		_MainTex ("主纹理", 2D) = "white" {}
		_SecTex ("烟雾纹理", 2D) = "white" {}
		_GlowTex ("高亮图层", 2D) = "white" {}
		_MaskTex ("蒙版纹理", 2D) = "white" {}
		_TintCol ("颜色调节", Vector) = (1,1,1,1)
		_SecFloatSpeedY ("次纹理Y滚动速度", Float) = 2
		_SecFloatSpeedX ("次纹理X滚动速度", Float) = 2
		_NumLayer ("星空复杂度", Range(1, 10)) = 6
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			GpuProgramID 26077
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
						vec4 unused_0_0[3];
						vec4 _SecTex_ST;
						vec4 _TintCol;
						float _SecFloatSpeedY;
						float _SecFloatSpeedX;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _SecTex;
					uniform  sampler2D _GlowTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec2 u_xlat3;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _SecTex_ST.xy;
					    u_xlat0.xy = u_xlat0.xy * vec2(0.5, 0.5) + _SecTex_ST.zw;
					    u_xlat0.xy = vec2(_SecFloatSpeedX, _SecFloatSpeedY) * _Time.yy + u_xlat0.xy;
					    u_xlat1 = u_xlat0.xyxy * _SecTex_ST.xyxy;
					    u_xlat0 = texture(_SecTex, u_xlat0.xy);
					    u_xlat1 = u_xlat1 * vec4(0.333333343, 0.333333343, 0.200000003, 0.200000003) + _SecTex_ST.zwzw;
					    u_xlat0.xy = vec2(_SecFloatSpeedX, _SecFloatSpeedY) * _Time.yy + u_xlat1.xy;
					    u_xlat2 = texture(_SecTex, u_xlat0.xy);
					    u_xlat0.x = max(u_xlat0.w, u_xlat2.w);
					    u_xlat3.xy = vec2(_SecFloatSpeedX, _SecFloatSpeedY) * _Time.yy;
					    u_xlat3.xy = u_xlat3.xy * vec2(2.0, 2.0) + u_xlat1.zw;
					    u_xlat1 = texture(_SecTex, u_xlat3.xy);
					    u_xlat0.x = max(u_xlat0.x, u_xlat1.w);
					    u_xlat3.x = (-vs_TEXCOORD0.y) + 1.0;
					    u_xlat0.x = u_xlat3.x * u_xlat0.x;
					    u_xlat0 = u_xlat0.xxxx * _TintCol;
					    u_xlat1 = texture(_MaskTex, vs_TEXCOORD0.xy);
					    u_xlat0 = u_xlat0 * u_xlat1.wwww;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat0 = u_xlat0 * vec4(1.29999995, 1.29999995, 1.29999995, 1.29999995) + u_xlat1;
					    u_xlat1.x = sin(_Time.y);
					    u_xlat1.x = u_xlat1.x * 0.200000003 + 1.20000005;
					    u_xlat2 = texture(_GlowTex, vs_TEXCOORD0.xy);
					    u_xlat1 = u_xlat1.xxxx * u_xlat2;
					    u_xlat1 = u_xlat1 * vec4(1.10000002, 1.10000002, 1.10000002, 1.10000002);
					    u_xlat1 = u_xlat2.wwww * u_xlat1;
					    u_xlat2.x = (-u_xlat2.w) + 1.0;
					    u_xlat0 = u_xlat0 * u_xlat2.xxxx + u_xlat1;
					    SV_Target0 = u_xlat0 * vs_COLOR0;
					    return;
					}
				}
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}