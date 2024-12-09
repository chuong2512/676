Shader "Custom/EnemyRoundEffectShader" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_SampleTex ("Sample Texture", 2D) = "white" {}
		_SmokeTex1 ("Smoke Texture 1", 2D) = "white" {}
		_SmokeTex2 ("Smoke Texture 2", 2D) = "white" {}
		_SmokeTex3 ("Smoke Texture 3", 2D) = "white" {}
		_Color ("颜色", Vector) = (1,1,1,1)
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			GpuProgramID 17061
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
						vec4 unused_0_0[5];
						vec4 _Color;
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
					in  vec2 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_COLOR0;
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
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat0.xyz = _Color.www * _Color.xyz;
					    u_xlat0.w = _Color.w;
					    vs_COLOR0 = u_xlat0 * in_COLOR0;
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
						vec4 _SmokeTex2_ST;
						vec4 _SmokeTex3_ST;
						vec4 _Color;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _SampleTex;
					uniform  sampler2D _SmokeTex1;
					uniform  sampler2D _SmokeTex2;
					uniform  sampler2D _SmokeTex3;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					float u_xlat3;
					float u_xlat6;
					void main()
					{
					    u_xlat0.x = _Time.x * 5.0 + vs_TEXCOORD0.y;
					    u_xlat0.x = sin(u_xlat0.x);
					    u_xlat0.y = (-u_xlat0.x) * 0.300000012 + vs_TEXCOORD0.y;
					    u_xlat0.x = vs_TEXCOORD0.x;
					    u_xlat1.xy = u_xlat0.xy * _SmokeTex3_ST.xy + _SmokeTex3_ST.zw;
					    u_xlat0 = texture(_SmokeTex1, u_xlat0.xy);
					    u_xlat0.xy = _Time.xy * vec2(5.0, 0.333333343);
					    u_xlat6 = vs_TEXCOORD0.y * -3.0 + u_xlat0.x;
					    u_xlat6 = sin(u_xlat6);
					    u_xlat1.z = u_xlat6 * 0.200000003 + u_xlat1.y;
					    u_xlat1 = texture(_SmokeTex3, u_xlat1.xz);
					    u_xlat6 = u_xlat0.w + u_xlat1.w;
					    u_xlat0.x = vs_TEXCOORD0.y * 2.0 + u_xlat0.x;
					    u_xlat3 = fract(u_xlat0.y);
					    u_xlat3 = (-u_xlat3) + 0.5;
					    u_xlat3 = abs(u_xlat3) + 1.0;
					    u_xlat0.x = sin(u_xlat0.x);
					    u_xlat1.xy = vs_TEXCOORD0.xy * _SmokeTex2_ST.xy + _SmokeTex2_ST.zw;
					    u_xlat0.xw = (-u_xlat0.xx) * vec2(0.400000006, 0.400000006) + u_xlat1.xy;
					    u_xlat1 = texture(_SmokeTex2, u_xlat0.xw);
					    u_xlat0.x = u_xlat6 + u_xlat1.w;
					    u_xlat0.x = u_xlat0.x * 0.333333343;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat0.x = u_xlat0.x * u_xlat1.w;
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * vec3(u_xlat3) + vec3(-1.0, -1.0, -1.0);
					    u_xlat2 = texture(_SampleTex, vs_TEXCOORD0.xy);
					    u_xlat0.w = u_xlat0.x * 4.0 + u_xlat2.w;
					    u_xlat0.x = float(1.0);
					    u_xlat0.y = float(1.0);
					    u_xlat0.z = float(1.0);
					    u_xlat1.w = 0.0;
					    u_xlat0 = u_xlat0.wwww * u_xlat1 + u_xlat0;
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