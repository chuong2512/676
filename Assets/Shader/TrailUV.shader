Shader "Trail/UV" {
	Properties {
		_noise ("noise", 2D) = "white" {}
		_head ("head", Vector) = (1,0.9496022,0,0)
		_trail ("trail", Vector) = (0.9811321,0,0.5593343,0)
		_raoluan ("raoluan", Float) = 0.3
		_liangdu ("liangdu", Float) = 2
		_Texture0 ("Texture 0", 2D) = "white" {}
		_piaodong ("piaodong", Vector) = (0,0,0,0)
		_houbai ("houbai", Vector) = (0,0,0,0)
	}
	SubShader {
		LOD 100
		Tags { "QUEUE" = "Transparent" "RenderType" = "Opaque" }
		Pass {
			Name "Unlit"
			LOD 100
			Tags { "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Opaque" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 52453
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
					in  vec4 in_COLOR0;
					in  vec4 in_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
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
					    vs_TEXCOORD1.xy = in_TEXCOORD0.xy;
					    vs_TEXCOORD1.zw = vec2(0.0, 0.0);
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
						vec4 _head;
						vec4 _trail;
						float _liangdu;
						vec2 _piaodong;
						vec2 _houbai;
						float _raoluan;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _Texture0;
					uniform  sampler2D _noise;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec2 u_xlat3;
					float u_xlat6;
					void main()
					{
					    u_xlat0.xy = _Time.yy * _houbai.xy + vs_TEXCOORD1.xy;
					    u_xlat0 = texture(_noise, u_xlat0.xy);
					    u_xlat0.x = u_xlat0.z + -0.300000012;
					    u_xlat0.x = u_xlat0.x * _raoluan;
					    u_xlat0.xy = u_xlat0.xx * vs_TEXCOORD1.xx + vs_TEXCOORD1.xy;
					    u_xlat0 = texture(_noise, u_xlat0.xy);
					    u_xlat3.xy = _Time.yy * vec2(_piaodong.x, _piaodong.y) + vs_TEXCOORD1.xy;
					    u_xlat1 = texture(_Texture0, u_xlat3.xy);
					    u_xlat3.xy = (-vs_TEXCOORD1.xx) + vec2(1.0, 0.0500000119);
					    u_xlat6 = max(u_xlat3.y, -2.43000007);
					    u_xlat6 = min(u_xlat6, 0.200000003);
					    u_xlat1 = vec4(u_xlat6) + u_xlat1;
					    u_xlat1 = u_xlat0.xxxx * u_xlat1;
					    u_xlat1 = u_xlat1 * vs_COLOR0.wwww;
					    u_xlat0.x = u_xlat3.x * vs_TEXCOORD1.y;
					    u_xlat0.x = u_xlat3.x * u_xlat0.x;
					    u_xlat0.x = u_xlat0.x * 3.0;
					    u_xlat0 = u_xlat0.xxxx * u_xlat1;
					    u_xlat1.w = u_xlat0.x;
					    u_xlat2.xyz = (-_head.xyz) + _trail.xyz;
					    u_xlat2.xyz = vs_TEXCOORD1.xxx * u_xlat2.xyz + _head.xyz;
					    u_xlat2.xyz = u_xlat2.xyz * vec3(_liangdu);
					    u_xlat1.xyz = u_xlat2.xyz * vs_COLOR0.xyz;
					    SV_Target0 = u_xlat0 * u_xlat1;
					    return;
					}
				}
			}
			ENDCG
		}
	}
	FallBack "VertexLit"
	CustomEditor "ASEMaterialInspector"
}