Shader "Custom/uv" {
	Properties {
		_MainTexture ("MainTexture", 2D) = "white" {}
		_SpeedMainTexUVNoiseZW ("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
		_StartColor ("StartColor", Vector) = (1,0,0,1)
		_EndColor ("EndColor", Vector) = (1,1,0,1)
		_Colorpower ("Color power", Float) = 1
		_Colorrange ("Color range", Float) = 1
		_Noise ("Noise", 2D) = "white" {}
		[Toggle(_USEDEPTH_ON)] _Usedepth ("Use depth?", Float) = 0
		_Depthpower ("Depth power", Float) = 1
		_Emission ("Emission", Float) = 2
		[Toggle(_USEDARK_ON)] _Usedark ("Use dark", Float) = 0
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "IsEmissive" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+0" "RenderType" = "Transparent" }
		Pass {
			Name "FORWARD"
			Tags { "IGNOREPROJECTOR" = "true" "IsEmissive" = "true" "LIGHTMODE" = "FORWARDBASE" "PreviewType" = "Plane" "QUEUE" = "Transparent+0" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ColorMask RGB -1
			ZWrite Off
			Cull Off
			GpuProgramID 28830
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
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[11];
						vec4 _texcoord_ST;
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_1_2[3];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec3 in_NORMAL0;
					in  vec4 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD1;
					out vec3 vs_TEXCOORD2;
					out vec4 vs_COLOR0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					float u_xlat6;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    vs_TEXCOORD2.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat0 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat0 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat0;
					    u_xlat0 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat0;
					    gl_Position = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _texcoord_ST.xy + _texcoord_ST.zw;
					    u_xlat0.x = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat0.y = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat0.z = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat6 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat6 = inversesqrt(u_xlat6);
					    vs_TEXCOORD1.xyz = vec3(u_xlat6) * u_xlat0.xyz;
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
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[11];
						vec4 _texcoord_ST;
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_1_2[3];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec3 in_NORMAL0;
					in  vec4 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD1;
					out vec3 vs_TEXCOORD2;
					out vec4 vs_COLOR0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					float u_xlat6;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    vs_TEXCOORD2.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat0 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat0 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat0;
					    u_xlat0 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat0;
					    gl_Position = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _texcoord_ST.xy + _texcoord_ST.zw;
					    u_xlat0.x = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat0.y = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat0.z = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat6 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat6 = inversesqrt(u_xlat6);
					    vs_TEXCOORD1.xyz = vec3(u_xlat6) * u_xlat0.xyz;
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
						vec4 unused_0_0[4];
						vec4 _StartColor;
						vec4 _EndColor;
						float _Colorrange;
						float _Colorpower;
						float _Emission;
						vec4 _SpeedMainTexUVNoiseZW;
						vec4 _MainTexture_ST;
						vec4 _Noise_ST;
						vec4 unused_0_9[2];
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _MainTexture;
					uniform  sampler2D _Noise;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec2 u_xlat4;
					float u_xlat6;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _Noise_ST.xy + _Noise_ST.zw;
					    u_xlat0.xy = _SpeedMainTexUVNoiseZW.zw * _Time.yy + u_xlat0.xy;
					    u_xlat0 = texture(_Noise, u_xlat0.xy);
					    u_xlat2.xy = (-vs_TEXCOORD0.xy) + vec2(1.0, 1.0);
					    u_xlat6 = log2(u_xlat2.x);
					    u_xlat2.x = u_xlat2.y * u_xlat2.x;
					    u_xlat2.x = u_xlat2.x * vs_TEXCOORD0.y;
					    u_xlat2.x = u_xlat2.x * 6.0;
					    u_xlat2.x = clamp(u_xlat2.x, 0.0, 1.0);
					    u_xlat4.x = u_xlat6 * 0.800000012;
					    u_xlat4.x = exp2(u_xlat4.x);
					    u_xlat4.x = max(u_xlat4.x, 0.200000003);
					    u_xlat4.x = min(u_xlat4.x, 0.600000024);
					    u_xlat0.x = u_xlat4.x + u_xlat0.x;
					    u_xlat0.x = u_xlat0.x + (-vs_TEXCOORD0.x);
					    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
					    u_xlat4.xy = vs_TEXCOORD0.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
					    u_xlat4.xy = _SpeedMainTexUVNoiseZW.xy * _Time.yy + u_xlat4.xy;
					    u_xlat1 = texture(_MainTexture, u_xlat4.xy);
					    u_xlat4.x = u_xlat1.x * vs_COLOR0.w;
					    u_xlat0.x = u_xlat0.x * u_xlat4.x;
					    SV_Target0.w = u_xlat2.x * u_xlat0.x;
					    u_xlat0.x = vs_TEXCOORD0.x * _Colorrange;
					    u_xlat0.x = log2(u_xlat0.x);
					    u_xlat0.x = u_xlat0.x * _Colorpower;
					    u_xlat0.x = exp2(u_xlat0.x);
					    u_xlat0.x = min(u_xlat0.x, 1.0);
					    u_xlat2.xyz = (-_StartColor.xyz) + _EndColor.xyz;
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat2.xyz + _StartColor.xyz;
					    u_xlat0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
					    SV_Target0.xyz = u_xlat0.xyz * vec3(vec3(_Emission, _Emission, _Emission));
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
						vec4 unused_0_0[4];
						vec4 _StartColor;
						vec4 _EndColor;
						float _Colorrange;
						float _Colorpower;
						float _Emission;
						vec4 _SpeedMainTexUVNoiseZW;
						vec4 _MainTexture_ST;
						vec4 _Noise_ST;
						vec4 unused_0_9[2];
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _MainTexture;
					uniform  sampler2D _Noise;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec2 u_xlat4;
					float u_xlat6;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _Noise_ST.xy + _Noise_ST.zw;
					    u_xlat0.xy = _SpeedMainTexUVNoiseZW.zw * _Time.yy + u_xlat0.xy;
					    u_xlat0 = texture(_Noise, u_xlat0.xy);
					    u_xlat2.xy = (-vs_TEXCOORD0.xy) + vec2(1.0, 1.0);
					    u_xlat6 = log2(u_xlat2.x);
					    u_xlat2.x = u_xlat2.y * u_xlat2.x;
					    u_xlat2.x = u_xlat2.x * vs_TEXCOORD0.y;
					    u_xlat2.x = u_xlat2.x * 6.0;
					    u_xlat2.x = clamp(u_xlat2.x, 0.0, 1.0);
					    u_xlat4.x = u_xlat6 * 0.800000012;
					    u_xlat4.x = exp2(u_xlat4.x);
					    u_xlat4.x = max(u_xlat4.x, 0.200000003);
					    u_xlat4.x = min(u_xlat4.x, 0.600000024);
					    u_xlat0.x = u_xlat4.x + u_xlat0.x;
					    u_xlat0.x = u_xlat0.x + (-vs_TEXCOORD0.x);
					    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
					    u_xlat4.xy = vs_TEXCOORD0.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
					    u_xlat4.xy = _SpeedMainTexUVNoiseZW.xy * _Time.yy + u_xlat4.xy;
					    u_xlat1 = texture(_MainTexture, u_xlat4.xy);
					    u_xlat4.x = u_xlat1.x * vs_COLOR0.w;
					    u_xlat0.x = u_xlat0.x * u_xlat4.x;
					    SV_Target0.w = u_xlat2.x * u_xlat0.x;
					    u_xlat0.x = vs_TEXCOORD0.x * _Colorrange;
					    u_xlat0.x = log2(u_xlat0.x);
					    u_xlat0.x = u_xlat0.x * _Colorpower;
					    u_xlat0.x = exp2(u_xlat0.x);
					    u_xlat0.x = min(u_xlat0.x, 1.0);
					    u_xlat2.xyz = (-_StartColor.xyz) + _EndColor.xyz;
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat2.xyz + _StartColor.xyz;
					    u_xlat0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
					    SV_Target0.xyz = u_xlat0.xyz * vec3(vec3(_Emission, _Emission, _Emission));
					    return;
					}
				}
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}