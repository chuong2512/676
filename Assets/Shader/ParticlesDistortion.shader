Shader "Particles/Distortion" {
	Properties {
		_TintColor ("Tint Color", Vector) = (0.5,0.5,0.5,0.5)
		_GlowPower ("GlowPower", Range(1, 10)) = 1
		_EmisCol ("Emisison Color", Vector) = (0,0,0,0)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Distortion ("Distortion", Range(0, 128)) = 10
		_BumpMap ("Normalmap", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01, 3)) = 1
	}
	SubShader {
		Tags { "QUEUE" = "Overlay" "RenderType" = "Transparent" }
		GrabPass {
		}
		Pass {
			Tags { "QUEUE" = "Overlay" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 11193
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
						vec4 _MainTex_ST;
						vec4 _BumpMap_ST;
						vec4 unused_0_3[2];
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
					in  vec4 in_COLOR0;
					in  vec2 in_TEXCOORD0;
					out vec4 vs_COLOR0;
					out vec4 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
					out vec2 vs_TEXCOORD2;
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
					    u_xlat0 = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    gl_Position = u_xlat0;
					    vs_COLOR0 = in_COLOR0;
					    u_xlat0.xy = u_xlat0.xy * vec2(1.0, -1.0) + u_xlat0.ww;
					    vs_TEXCOORD0.zw = u_xlat0.zw;
					    vs_TEXCOORD0.xy = u_xlat0.xy * vec2(0.5, 0.5);
					    vs_TEXCOORD1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
					    vs_TEXCOORD2.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "SOFTPARTICLES_ON" }
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
						vec4 _MainTex_ST;
						vec4 _BumpMap_ST;
						vec4 unused_0_3[2];
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_2_1[7];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[9];
						mat4x4 unity_MatrixV;
						vec4 unused_3_2[4];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_4[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_COLOR0;
					in  vec2 in_TEXCOORD0;
					out vec4 vs_COLOR0;
					out vec4 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
					out vec2 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec2 u_xlat2;
					float u_xlat3;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    gl_Position = u_xlat1;
					    vs_COLOR0 = in_COLOR0;
					    u_xlat2.xy = u_xlat1.xy * vec2(1.0, -1.0) + u_xlat1.ww;
					    vs_TEXCOORD0.xy = u_xlat2.xy * vec2(0.5, 0.5);
					    vs_TEXCOORD0.zw = u_xlat1.zw;
					    vs_TEXCOORD3.w = u_xlat1.w;
					    vs_TEXCOORD1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
					    vs_TEXCOORD2.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat3 = u_xlat0.y * unity_MatrixV[1].z;
					    u_xlat0.x = unity_MatrixV[0].z * u_xlat0.x + u_xlat3;
					    u_xlat0.x = unity_MatrixV[2].z * u_xlat0.z + u_xlat0.x;
					    u_xlat0.x = unity_MatrixV[3].z * u_xlat0.w + u_xlat0.x;
					    vs_TEXCOORD3.z = (-u_xlat0.x);
					    u_xlat0.x = u_xlat1.y * _ProjectionParams.x;
					    u_xlat1.xz = u_xlat1.xw * vec2(0.5, 0.5);
					    u_xlat1.w = u_xlat0.x * 0.5;
					    vs_TEXCOORD3.xy = u_xlat1.zz + u_xlat1.xw;
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
						vec4 _TintColor;
						vec4 _EmisCol;
						float _Distortion;
						vec4 unused_0_4[2];
						float _GlowPower;
						vec4 _GrabTexture_TexelSize;
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _BumpMap;
					uniform  sampler2D _GrabTexture;
					in  vec4 vs_COLOR0;
					in  vec4 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD1;
					in  vec2 vs_TEXCOORD2;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					float u_xlat6;
					float u_xlat9;
					void main()
					{
					    u_xlat0 = texture(_BumpMap, vs_TEXCOORD1.xy);
					    u_xlat0.x = u_xlat0.w * u_xlat0.x;
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
					    u_xlat6 = _Distortion * 100.0;
					    u_xlat0.xy = vec2(u_xlat6) * u_xlat0.xy;
					    u_xlat0.xy = u_xlat0.xy * _GrabTexture_TexelSize.xy;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat2 = u_xlat1.wxyz * vs_COLOR0.wxyz;
					    u_xlat0.xy = u_xlat0.xy * u_xlat2.xx;
					    u_xlat0.xy = u_xlat0.xy * vs_TEXCOORD0.zz + vs_TEXCOORD0.xy;
					    u_xlat0.xy = u_xlat0.xy / vs_TEXCOORD0.ww;
					    u_xlat0 = texture(_GrabTexture, u_xlat0.xy);
					    u_xlat9 = u_xlat0.w * vs_COLOR0.w;
					    u_xlat0.xyz = u_xlat0.xyz * vec3(_GlowPower);
					    u_xlat0.xyz = u_xlat0.xyz * vs_COLOR0.xyz + _EmisCol.xyz;
					    SV_Target0.xyz = u_xlat2.yzw * abs(u_xlat0.xyz);
					    u_xlat0.x = u_xlat1.w * u_xlat9;
					    u_xlat0.x = u_xlat0.x * _TintColor.w;
					    SV_Target0.w = abs(u_xlat0.x);
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "SOFTPARTICLES_ON" }
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
						vec4 _TintColor;
						vec4 _EmisCol;
						float _Distortion;
						vec4 unused_0_4[2];
						float _GlowPower;
						float _InvFade;
						vec4 _GrabTexture_TexelSize;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[7];
						vec4 _ZBufferParams;
						vec4 unused_1_2;
					};
					uniform  sampler2D _CameraDepthTexture;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _BumpMap;
					uniform  sampler2D _GrabTexture;
					in  vec4 vs_COLOR0;
					in  vec4 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD1;
					in  vec2 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					vec2 u_xlat6;
					float u_xlat9;
					void main()
					{
					    u_xlat0 = texture(_BumpMap, vs_TEXCOORD1.xy);
					    u_xlat0.x = u_xlat0.w * u_xlat0.x;
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
					    u_xlat6.x = _Distortion * 100.0;
					    u_xlat0.xy = u_xlat6.xx * u_xlat0.xy;
					    u_xlat0.xy = u_xlat0.xy * _GrabTexture_TexelSize.xy;
					    u_xlat6.xy = vs_TEXCOORD3.xy / vs_TEXCOORD3.ww;
					    u_xlat1 = texture(_CameraDepthTexture, u_xlat6.xy);
					    u_xlat6.x = _ZBufferParams.z * u_xlat1.x + _ZBufferParams.w;
					    u_xlat6.x = float(1.0) / u_xlat6.x;
					    u_xlat6.x = u_xlat6.x + (-vs_TEXCOORD3.z);
					    u_xlat6.x = u_xlat6.x * _InvFade;
					    u_xlat6.x = clamp(u_xlat6.x, 0.0, 1.0);
					    u_xlat6.x = u_xlat6.x * vs_COLOR0.w;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat9 = u_xlat6.x * u_xlat1.w;
					    u_xlat0.xy = vec2(u_xlat9) * u_xlat0.xy;
					    u_xlat0.xy = u_xlat0.xy * vs_TEXCOORD0.zz + vs_TEXCOORD0.xy;
					    u_xlat0.xy = u_xlat0.xy / vs_TEXCOORD0.ww;
					    u_xlat2 = texture(_GrabTexture, u_xlat0.xy);
					    u_xlat0.x = u_xlat6.x * u_xlat2.w;
					    u_xlat3.xyz = u_xlat2.xyz * vec3(_GlowPower);
					    u_xlat3.xyz = u_xlat3.xyz * vs_COLOR0.xyz + _EmisCol.xyz;
					    u_xlat0.x = u_xlat1.w * u_xlat0.x;
					    u_xlat1.xyz = u_xlat1.xyz * vs_COLOR0.xyz;
					    SV_Target0.xyz = abs(u_xlat3.xyz) * u_xlat1.xyz;
					    u_xlat0.x = u_xlat0.x * _TintColor.w;
					    SV_Target0.w = abs(u_xlat0.x);
					    return;
					}
				}
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}