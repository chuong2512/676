Shader "HZEffect/UVAniHHRD" {
	Properties {
		_MainTex ("MainTex", 2D) = "white" {}
		[MaterialToggle] _WuAlpha ("无Alpha贴图（不要用在彩色贴图上）", Float) = 0
		_MainTex_Color ("MainTex_Color", Vector) = (1,1,1,1)
		_Tex_WeiYi ("MainTex Y方向上UV位移", Float) = 0
		[MaterialToggle] _Tex_WeiYiFangXiang ("MainTex X方向上UV位移", Float) = 0
		_HunHeTex ("混合贴图（也用于扰动）", 2D) = "white" {}
		_HunHe_Color ("混合贴图_Color", Vector) = (1,1,1,1)
		_HunHe_DuiBiDu ("混合强度", Float) = 1.5
		_XiangHuHunHe_Bi ("混合强度辅助", Float) = 1
		_HunHe_WeiYiX ("混合贴图UV位移", Float) = 0
		[MaterialToggle] _AlphaONOff ("全局Alpha", Float) = 0
		_RaoDong_QiangDu ("扰动强度", Float) = 0.03
		_RaoDong_WeiYiZ ("扰动UV位移", Float) = 0
		_XiaoRong ("XiaoRong", Range(1, 0)) = 1
		[MaterialToggle] _XRSwap ("反转（黑白哪个部分先消融）", Float) = 0.01
		_Mask ("Mask", 2D) = "white" {}
		_Alpha ("Alpha", Range(1, 0)) = 1
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Name "FORWARD"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 19437
			CGPROGRAM
			Program "vp" {
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" }
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
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" }
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
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" }
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
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "SHADOWS_SCREEN" }
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
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "VERTEXLIGHT_ON" }
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
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "VERTEXLIGHT_ON" }
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
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
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
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
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
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" }
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
						vec4 _TimeEditor;
						vec4 _MainTex_Color;
						vec4 _MainTex_ST;
						float _RaoDong_QiangDu;
						vec4 _HunHe_Color;
						vec4 unused_0_6;
						float _HunHe_DuiBiDu;
						float _HunHe_WeiYiX;
						float _RaoDong_WeiYiZ;
						float _AlphaONOff;
						float _XiaoRong;
						float _XiangHuHunHe_Bi;
						float _Tex_WeiYi;
						vec4 _Mask_ST;
						float _Tex_WeiYiFangXiang;
						float _Alpha;
						float _WuAlpha;
						float _XRSwap;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _HunHeTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _Mask;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec2 u_xlat4;
					bool u_xlatb4;
					vec2 u_xlat8;
					float u_xlat12;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _Mask_ST.xy + _Mask_ST.zw;
					    u_xlat0 = texture(_Mask, u_xlat0.xy);
					    u_xlat4.xy = _TimeEditor.xy + _Time.xy;
					    u_xlat1 = u_xlat4.xxxx * vec4(_RaoDong_WeiYiZ, _RaoDong_WeiYiZ, _HunHe_WeiYiX, _HunHe_WeiYiX);
					    u_xlat4.x = u_xlat4.y * _Tex_WeiYi;
					    u_xlat8.xy = u_xlat1.xy * vec2(0.0, 11.0) + vs_TEXCOORD0.xy;
					    u_xlat2 = texture(_HunHeTex, u_xlat8.xy);
					    u_xlat2 = vec4(_RaoDong_QiangDu) * u_xlat2.xxxx + vs_TEXCOORD0.xyxy;
					    u_xlat3 = u_xlat4.xxxx * vec4(0.0, 1.0, 1.0, 0.0) + u_xlat2;
					    u_xlat8.xy = u_xlat1.zw * vec2(1.0, 0.0) + u_xlat2.zw;
					    u_xlat1 = texture(_HunHeTex, u_xlat8.xy);
					    u_xlat4.xy = u_xlat4.xx * vec2(0.0, 0.699999988) + vs_TEXCOORD0.xy;
					    u_xlat2 = texture(_HunHeTex, u_xlat4.xy);
					    u_xlat4.xy = (-u_xlat3.xy) + u_xlat3.zw;
					    u_xlat4.xy = vec2(_Tex_WeiYiFangXiang) * u_xlat4.xy + u_xlat3.xy;
					    u_xlat4.xy = u_xlat4.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat3 = texture(_MainTex, u_xlat4.xy);
					    u_xlat4.x = (-u_xlat3.w) + u_xlat3.x;
					    u_xlat0.y = _WuAlpha * u_xlat4.x + u_xlat3.w;
					    u_xlat3.xyz = u_xlat3.xyz * _MainTex_Color.xyz;
					    u_xlat0.xz = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat4.x = _AlphaONOff * u_xlat0.z + u_xlat0.y;
					    u_xlat0.x = (-u_xlat0.x) + u_xlat4.x;
					    u_xlat4.x = max(u_xlat1.x, 0.00999999978);
					    u_xlat1.xyz = u_xlat1.xyz * vec3(_HunHe_DuiBiDu);
					    u_xlat4.x = min(u_xlat4.x, 1.0);
					    u_xlat8.x = u_xlat4.x * -2.0 + 1.0;
					    u_xlat4.x = _XRSwap * u_xlat8.x + u_xlat4.x;
					    u_xlatb4 = _XiaoRong>=u_xlat4.x;
					    u_xlat4.x = (u_xlatb4) ? -0.0 : -1.0;
					    u_xlat0.x = u_xlat4.x + u_xlat0.x;
					    u_xlat0.x = u_xlat0.x * vs_COLOR0.w;
					    u_xlat0.x = u_xlat0.x * _MainTex_Color.w;
					    SV_Target0.w = u_xlat0.x * _Alpha;
					    u_xlat0.xyz = u_xlat2.xyz * vec3(_HunHe_DuiBiDu);
					    u_xlat0.xyz = u_xlat0.xyz * _HunHe_Color.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * _HunHe_Color.xyz + (-u_xlat0.xyz);
					    u_xlat0.xyz = u_xlat2.xxx * u_xlat1.xyz + u_xlat0.xyz;
					    u_xlat12 = u_xlat2.x * _XiangHuHunHe_Bi;
					    u_xlat0.xyz = vec3(u_xlat12) * u_xlat0.xyz + u_xlat3.xyz;
					    SV_Target0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" }
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
						vec4 _TimeEditor;
						vec4 _MainTex_Color;
						vec4 _MainTex_ST;
						float _RaoDong_QiangDu;
						vec4 _HunHe_Color;
						vec4 unused_0_6;
						float _HunHe_DuiBiDu;
						float _HunHe_WeiYiX;
						float _RaoDong_WeiYiZ;
						float _AlphaONOff;
						float _XiaoRong;
						float _XiangHuHunHe_Bi;
						float _Tex_WeiYi;
						vec4 _Mask_ST;
						float _Tex_WeiYiFangXiang;
						float _Alpha;
						float _WuAlpha;
						float _XRSwap;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _HunHeTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _Mask;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec2 u_xlat4;
					bool u_xlatb4;
					vec2 u_xlat8;
					float u_xlat12;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _Mask_ST.xy + _Mask_ST.zw;
					    u_xlat0 = texture(_Mask, u_xlat0.xy);
					    u_xlat4.xy = _TimeEditor.xy + _Time.xy;
					    u_xlat1 = u_xlat4.xxxx * vec4(_RaoDong_WeiYiZ, _RaoDong_WeiYiZ, _HunHe_WeiYiX, _HunHe_WeiYiX);
					    u_xlat4.x = u_xlat4.y * _Tex_WeiYi;
					    u_xlat8.xy = u_xlat1.xy * vec2(0.0, 11.0) + vs_TEXCOORD0.xy;
					    u_xlat2 = texture(_HunHeTex, u_xlat8.xy);
					    u_xlat2 = vec4(_RaoDong_QiangDu) * u_xlat2.xxxx + vs_TEXCOORD0.xyxy;
					    u_xlat3 = u_xlat4.xxxx * vec4(0.0, 1.0, 1.0, 0.0) + u_xlat2;
					    u_xlat8.xy = u_xlat1.zw * vec2(1.0, 0.0) + u_xlat2.zw;
					    u_xlat1 = texture(_HunHeTex, u_xlat8.xy);
					    u_xlat4.xy = u_xlat4.xx * vec2(0.0, 0.699999988) + vs_TEXCOORD0.xy;
					    u_xlat2 = texture(_HunHeTex, u_xlat4.xy);
					    u_xlat4.xy = (-u_xlat3.xy) + u_xlat3.zw;
					    u_xlat4.xy = vec2(_Tex_WeiYiFangXiang) * u_xlat4.xy + u_xlat3.xy;
					    u_xlat4.xy = u_xlat4.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat3 = texture(_MainTex, u_xlat4.xy);
					    u_xlat4.x = (-u_xlat3.w) + u_xlat3.x;
					    u_xlat0.y = _WuAlpha * u_xlat4.x + u_xlat3.w;
					    u_xlat3.xyz = u_xlat3.xyz * _MainTex_Color.xyz;
					    u_xlat0.xz = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat4.x = _AlphaONOff * u_xlat0.z + u_xlat0.y;
					    u_xlat0.x = (-u_xlat0.x) + u_xlat4.x;
					    u_xlat4.x = max(u_xlat1.x, 0.00999999978);
					    u_xlat1.xyz = u_xlat1.xyz * vec3(_HunHe_DuiBiDu);
					    u_xlat4.x = min(u_xlat4.x, 1.0);
					    u_xlat8.x = u_xlat4.x * -2.0 + 1.0;
					    u_xlat4.x = _XRSwap * u_xlat8.x + u_xlat4.x;
					    u_xlatb4 = _XiaoRong>=u_xlat4.x;
					    u_xlat4.x = (u_xlatb4) ? -0.0 : -1.0;
					    u_xlat0.x = u_xlat4.x + u_xlat0.x;
					    u_xlat0.x = u_xlat0.x * vs_COLOR0.w;
					    u_xlat0.x = u_xlat0.x * _MainTex_Color.w;
					    SV_Target0.w = u_xlat0.x * _Alpha;
					    u_xlat0.xyz = u_xlat2.xyz * vec3(_HunHe_DuiBiDu);
					    u_xlat0.xyz = u_xlat0.xyz * _HunHe_Color.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * _HunHe_Color.xyz + (-u_xlat0.xyz);
					    u_xlat0.xyz = u_xlat2.xxx * u_xlat1.xyz + u_xlat0.xyz;
					    u_xlat12 = u_xlat2.x * _XiangHuHunHe_Bi;
					    u_xlat0.xyz = vec3(u_xlat12) * u_xlat0.xyz + u_xlat3.xyz;
					    SV_Target0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" }
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
						vec4 _TimeEditor;
						vec4 _MainTex_Color;
						vec4 _MainTex_ST;
						float _RaoDong_QiangDu;
						vec4 _HunHe_Color;
						vec4 unused_0_6;
						float _HunHe_DuiBiDu;
						float _HunHe_WeiYiX;
						float _RaoDong_WeiYiZ;
						float _AlphaONOff;
						float _XiaoRong;
						float _XiangHuHunHe_Bi;
						float _Tex_WeiYi;
						vec4 _Mask_ST;
						float _Tex_WeiYiFangXiang;
						float _Alpha;
						float _WuAlpha;
						float _XRSwap;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _HunHeTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _Mask;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec2 u_xlat4;
					bool u_xlatb4;
					vec2 u_xlat8;
					float u_xlat12;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _Mask_ST.xy + _Mask_ST.zw;
					    u_xlat0 = texture(_Mask, u_xlat0.xy);
					    u_xlat4.xy = _TimeEditor.xy + _Time.xy;
					    u_xlat1 = u_xlat4.xxxx * vec4(_RaoDong_WeiYiZ, _RaoDong_WeiYiZ, _HunHe_WeiYiX, _HunHe_WeiYiX);
					    u_xlat4.x = u_xlat4.y * _Tex_WeiYi;
					    u_xlat8.xy = u_xlat1.xy * vec2(0.0, 11.0) + vs_TEXCOORD0.xy;
					    u_xlat2 = texture(_HunHeTex, u_xlat8.xy);
					    u_xlat2 = vec4(_RaoDong_QiangDu) * u_xlat2.xxxx + vs_TEXCOORD0.xyxy;
					    u_xlat3 = u_xlat4.xxxx * vec4(0.0, 1.0, 1.0, 0.0) + u_xlat2;
					    u_xlat8.xy = u_xlat1.zw * vec2(1.0, 0.0) + u_xlat2.zw;
					    u_xlat1 = texture(_HunHeTex, u_xlat8.xy);
					    u_xlat4.xy = u_xlat4.xx * vec2(0.0, 0.699999988) + vs_TEXCOORD0.xy;
					    u_xlat2 = texture(_HunHeTex, u_xlat4.xy);
					    u_xlat4.xy = (-u_xlat3.xy) + u_xlat3.zw;
					    u_xlat4.xy = vec2(_Tex_WeiYiFangXiang) * u_xlat4.xy + u_xlat3.xy;
					    u_xlat4.xy = u_xlat4.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat3 = texture(_MainTex, u_xlat4.xy);
					    u_xlat4.x = (-u_xlat3.w) + u_xlat3.x;
					    u_xlat0.y = _WuAlpha * u_xlat4.x + u_xlat3.w;
					    u_xlat3.xyz = u_xlat3.xyz * _MainTex_Color.xyz;
					    u_xlat0.xz = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat4.x = _AlphaONOff * u_xlat0.z + u_xlat0.y;
					    u_xlat0.x = (-u_xlat0.x) + u_xlat4.x;
					    u_xlat4.x = max(u_xlat1.x, 0.00999999978);
					    u_xlat1.xyz = u_xlat1.xyz * vec3(_HunHe_DuiBiDu);
					    u_xlat4.x = min(u_xlat4.x, 1.0);
					    u_xlat8.x = u_xlat4.x * -2.0 + 1.0;
					    u_xlat4.x = _XRSwap * u_xlat8.x + u_xlat4.x;
					    u_xlatb4 = _XiaoRong>=u_xlat4.x;
					    u_xlat4.x = (u_xlatb4) ? -0.0 : -1.0;
					    u_xlat0.x = u_xlat4.x + u_xlat0.x;
					    u_xlat0.x = u_xlat0.x * vs_COLOR0.w;
					    u_xlat0.x = u_xlat0.x * _MainTex_Color.w;
					    SV_Target0.w = u_xlat0.x * _Alpha;
					    u_xlat0.xyz = u_xlat2.xyz * vec3(_HunHe_DuiBiDu);
					    u_xlat0.xyz = u_xlat0.xyz * _HunHe_Color.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * _HunHe_Color.xyz + (-u_xlat0.xyz);
					    u_xlat0.xyz = u_xlat2.xxx * u_xlat1.xyz + u_xlat0.xyz;
					    u_xlat12 = u_xlat2.x * _XiangHuHunHe_Bi;
					    u_xlat0.xyz = vec3(u_xlat12) * u_xlat0.xyz + u_xlat3.xyz;
					    SV_Target0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
					    return;
					}
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "SHADOWS_SCREEN" }
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
						vec4 _TimeEditor;
						vec4 _MainTex_Color;
						vec4 _MainTex_ST;
						float _RaoDong_QiangDu;
						vec4 _HunHe_Color;
						vec4 unused_0_6;
						float _HunHe_DuiBiDu;
						float _HunHe_WeiYiX;
						float _RaoDong_WeiYiZ;
						float _AlphaONOff;
						float _XiaoRong;
						float _XiangHuHunHe_Bi;
						float _Tex_WeiYi;
						vec4 _Mask_ST;
						float _Tex_WeiYiFangXiang;
						float _Alpha;
						float _WuAlpha;
						float _XRSwap;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _HunHeTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _Mask;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec2 u_xlat4;
					bool u_xlatb4;
					vec2 u_xlat8;
					float u_xlat12;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _Mask_ST.xy + _Mask_ST.zw;
					    u_xlat0 = texture(_Mask, u_xlat0.xy);
					    u_xlat4.xy = _TimeEditor.xy + _Time.xy;
					    u_xlat1 = u_xlat4.xxxx * vec4(_RaoDong_WeiYiZ, _RaoDong_WeiYiZ, _HunHe_WeiYiX, _HunHe_WeiYiX);
					    u_xlat4.x = u_xlat4.y * _Tex_WeiYi;
					    u_xlat8.xy = u_xlat1.xy * vec2(0.0, 11.0) + vs_TEXCOORD0.xy;
					    u_xlat2 = texture(_HunHeTex, u_xlat8.xy);
					    u_xlat2 = vec4(_RaoDong_QiangDu) * u_xlat2.xxxx + vs_TEXCOORD0.xyxy;
					    u_xlat3 = u_xlat4.xxxx * vec4(0.0, 1.0, 1.0, 0.0) + u_xlat2;
					    u_xlat8.xy = u_xlat1.zw * vec2(1.0, 0.0) + u_xlat2.zw;
					    u_xlat1 = texture(_HunHeTex, u_xlat8.xy);
					    u_xlat4.xy = u_xlat4.xx * vec2(0.0, 0.699999988) + vs_TEXCOORD0.xy;
					    u_xlat2 = texture(_HunHeTex, u_xlat4.xy);
					    u_xlat4.xy = (-u_xlat3.xy) + u_xlat3.zw;
					    u_xlat4.xy = vec2(_Tex_WeiYiFangXiang) * u_xlat4.xy + u_xlat3.xy;
					    u_xlat4.xy = u_xlat4.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat3 = texture(_MainTex, u_xlat4.xy);
					    u_xlat4.x = (-u_xlat3.w) + u_xlat3.x;
					    u_xlat0.y = _WuAlpha * u_xlat4.x + u_xlat3.w;
					    u_xlat3.xyz = u_xlat3.xyz * _MainTex_Color.xyz;
					    u_xlat0.xz = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat4.x = _AlphaONOff * u_xlat0.z + u_xlat0.y;
					    u_xlat0.x = (-u_xlat0.x) + u_xlat4.x;
					    u_xlat4.x = max(u_xlat1.x, 0.00999999978);
					    u_xlat1.xyz = u_xlat1.xyz * vec3(_HunHe_DuiBiDu);
					    u_xlat4.x = min(u_xlat4.x, 1.0);
					    u_xlat8.x = u_xlat4.x * -2.0 + 1.0;
					    u_xlat4.x = _XRSwap * u_xlat8.x + u_xlat4.x;
					    u_xlatb4 = _XiaoRong>=u_xlat4.x;
					    u_xlat4.x = (u_xlatb4) ? -0.0 : -1.0;
					    u_xlat0.x = u_xlat4.x + u_xlat0.x;
					    u_xlat0.x = u_xlat0.x * vs_COLOR0.w;
					    u_xlat0.x = u_xlat0.x * _MainTex_Color.w;
					    SV_Target0.w = u_xlat0.x * _Alpha;
					    u_xlat0.xyz = u_xlat2.xyz * vec3(_HunHe_DuiBiDu);
					    u_xlat0.xyz = u_xlat0.xyz * _HunHe_Color.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * _HunHe_Color.xyz + (-u_xlat0.xyz);
					    u_xlat0.xyz = u_xlat2.xxx * u_xlat1.xyz + u_xlat0.xyz;
					    u_xlat12 = u_xlat2.x * _XiangHuHunHe_Bi;
					    u_xlat0.xyz = vec3(u_xlat12) * u_xlat0.xyz + u_xlat3.xyz;
					    SV_Target0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
					    return;
					}
				}
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ShaderForgeMaterialInspector"
}