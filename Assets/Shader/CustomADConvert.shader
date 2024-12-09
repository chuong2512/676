Shader "Custom/ADConvert" {
	Properties {
		_MainTex ("攻击纹理", 2D) = "white" {}
		_DefTex ("防御纹理", 2D) = "white" {}
		_MaskTex ("遮罩纹理", 2D) = "white" {}
		_ShieldTex ("防御护盾", 2D) = "white" {}
		_NoiseTex ("噪声", 2D) = "white" {}
		_ShieldCol ("护盾颜色", Vector) = (1,1,1,1)
		_AToDPara ("防御比例(1位防御0位攻击)", Range(0, 1)) = 1
		_MinAlpha ("Min Alpha", Range(0, 1)) = 0.6
		_ShiningSpeed ("Shining Speed", Float) = 2
		_FloatSpeed ("Flow Speed", Float) = 2
	}
	SubShader {
		LOD 100
		Tags { "RenderType" = "Transparent" }
		Pass {
			LOD 100
			Tags { "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			CGPROGRAM
			Program "vp" {
				SubProgram "d3d11 " {
					
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
						float _AToDPara;
						float _ShiningSpeed;
						float _MinAlpha;
						float _FloatSpeed;
						vec4 _ShieldCol;
						vec4 _NoiseTex_ST;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DefTex;
					uniform  sampler2D _ShieldTex;
					uniform  sampler2D _NoiseTex;
					uniform  sampler2D _MaskTex;
					in  vec2 vs_TEXCOORD0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec2 u_xlat3;
					void main()
					{
					    u_xlat0.x = _ShiningSpeed * _Time.y;
					    u_xlat0.x = cos(u_xlat0.x);
					    u_xlat0.x = u_xlat0.x * 0.5 + 0.5;
					    u_xlat3.x = (-_MinAlpha) + 1.0;
					    u_xlat0.x = u_xlat0.x * u_xlat3.x + _MinAlpha;
					    u_xlat1.y = _FloatSpeed * _Time.y;
					    u_xlat3.xy = vs_TEXCOORD0.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
					    u_xlat1.x = 0.0;
					    u_xlat3.xy = u_xlat1.xy + u_xlat3.xy;
					    u_xlat1 = texture(_NoiseTex, u_xlat3.xy);
					    u_xlat2 = texture(_ShieldTex, vs_TEXCOORD0.xy);
					    u_xlat3.x = (-u_xlat2.w) * _AToDPara + 1.0;
					    u_xlat0.x = u_xlat1.x * u_xlat3.x + u_xlat0.x;
					    u_xlat3.x = u_xlat2.w * _AToDPara;
					    u_xlat2.w = u_xlat0.x * u_xlat3.x;
					    u_xlat0 = u_xlat2.wwww * u_xlat2;
					    u_xlat0 = u_xlat0 * _ShieldCol;
					    u_xlat1 = texture(_DefTex, vs_TEXCOORD0.xy);
					    u_xlat2 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat1 = u_xlat1 + (-u_xlat2);
					    u_xlat1 = vec4(vec4(_AToDPara, _AToDPara, _AToDPara, _AToDPara)) * u_xlat1 + u_xlat2;
					    u_xlat0 = u_xlat0 * vec4(vec4(_AToDPara, _AToDPara, _AToDPara, _AToDPara)) + u_xlat1;
					    u_xlat1 = texture(_MaskTex, vs_TEXCOORD0.xy);
					    SV_Target0 = u_xlat0 * u_xlat1.wwww;
					    return;
					}
				}
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}