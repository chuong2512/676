Shader "Custom/TimeAndSpace" {
	Properties {
		_MainTex ("主纹理", 2D) = "white" {}
		_FloatingSpeed ("滚动速度", Float) = 1
		_RotSpeed ("旋转速度", Range(0, 2)) = 4
		_NumLayer ("星空复杂度", Range(1, 10)) = 6
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			GpuProgramID 10533
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
						float _NumLayer;
						float _FloatingSpeed;
						float _RotSpeed;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec2 u_xlat1;
					vec3 u_xlat2;
					vec4 u_xlat3;
					int u_xlati3;
					vec4 u_xlat4;
					vec3 u_xlat5;
					vec4 u_xlat6;
					bool u_xlatb6;
					vec3 u_xlat7;
					vec4 u_xlat8;
					vec4 u_xlat9;
					vec4 u_xlat10;
					vec4 u_xlat11;
					vec2 u_xlat12;
					int u_xlati16;
					bool u_xlatb16;
					float u_xlat26;
					float u_xlat27;
					vec2 u_xlat29;
					int u_xlati32;
					vec2 u_xlat34;
					vec2 u_xlat36;
					float u_xlat40;
					float u_xlat41;
					bool u_xlatb41;
					float u_xlat42;
					float u_xlat44;
					int u_xlati44;
					bool u_xlatb44;
					float u_xlat45;
					bool u_xlatb45;
					float u_xlat46;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy + vec2(-0.5, -0.560000002);
					    u_xlat26 = _FloatingSpeed * _Time.y;
					    u_xlat26 = u_xlat26 * _RotSpeed;
					    u_xlat1.x = sin(u_xlat26);
					    u_xlat2.x = cos(u_xlat26);
					    u_xlat3.x = (-u_xlat1.x);
					    u_xlat3.y = u_xlat2.x;
					    u_xlat3.z = u_xlat1.x;
					    u_xlat1.x = dot(u_xlat0.xy, u_xlat3.yz);
					    u_xlat1.y = dot(u_xlat0.xy, u_xlat3.xy);
					    u_xlat0.x = dot(u_xlat1.xy, u_xlat1.xy);
					    u_xlat0.x = sqrt(u_xlat0.x);
					    u_xlat0.xw = (-u_xlat0.xx) + vec2(0.600000024, 0.550000012);
					    u_xlat27 = float(1.0) / _NumLayer;
					    u_xlat2.x = float(0.0);
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat40 = 0.0;
					    while(true){
					        u_xlatb41 = u_xlat40>=1.0;
					        if(u_xlatb41){break;}
					        u_xlat41 = (-_Time.y) * _FloatingSpeed + u_xlat40;
					        u_xlat41 = fract(u_xlat41);
					        u_xlat3.xy = vec2(u_xlat41) * vec2(3.5, 3.0);
					        u_xlat3.xy = u_xlat0.xx * u_xlat3.xy;
					        u_xlat3.xy = u_xlat3.xy * vec2(-18.0, 8.0) + vec2(3.0, -2.0);
					        u_xlat29.x = u_xlat41 + -1.0;
					        u_xlat29.x = u_xlat29.x * -9.99999809;
					        u_xlat29.x = min(u_xlat29.x, 1.0);
					        u_xlat42 = u_xlat29.x * -2.0 + 3.0;
					        u_xlat29.x = u_xlat29.x * u_xlat29.x;
					        u_xlat29.x = u_xlat29.x * u_xlat42;
					        u_xlat41 = u_xlat41 * u_xlat29.x;
					        u_xlat29.xy = vec2(u_xlat40) * vec2(453.0, 4533.0);
					        u_xlat3 = u_xlat1.xyxy * u_xlat3.xxyy + u_xlat29.xxyy;
					        u_xlat4 = fract(u_xlat3);
					        u_xlat4 = u_xlat4 + vec4(-0.5, -0.5, -0.5, -0.5);
					        u_xlat3 = floor(u_xlat3);
					        u_xlat5.x = float(0.0);
					        u_xlat5.y = float(0.0);
					        u_xlat5.z = float(0.0);
					        for(int u_xlati_loop_1 = int(int(0xFFFFFFFFu)) ; u_xlati_loop_1<=1 ; u_xlati_loop_1++)
					        {
					            u_xlat6.y = float(u_xlati_loop_1);
					            u_xlat7.xyz = u_xlat5.xyz;
					            for(int u_xlati_loop_2 = int(0xFFFFFFFFu) ; u_xlati_loop_2<=1 ; u_xlati_loop_2++)
					            {
					                u_xlat6.x = float(u_xlati_loop_2);
					                u_xlat8.xy = u_xlat3.xy + u_xlat6.xy;
					                u_xlat8.xy = u_xlat8.xy * vec2(123.339996, 456.209991);
					                u_xlat8.xy = fract(u_xlat8.xy);
					                u_xlat34.xy = u_xlat8.xy + vec2(45.2099991, 87.5599976);
					                u_xlat45 = dot(u_xlat8.xy, u_xlat34.xy);
					                u_xlat8.xy = vec2(u_xlat45) + u_xlat8.xy;
					                u_xlat45 = u_xlat8.y * u_xlat8.x;
					                u_xlat8.x = fract(u_xlat45);
					                u_xlat9 = u_xlat8.xxxx * vec4(5435.0, 35.0, 4323.0, 6.28310013);
					                u_xlat6.xw = u_xlat4.xy + (-u_xlat6.xy);
					                u_xlat8.yzw = fract(u_xlat9.xyz);
					                u_xlat6.xw = u_xlat6.xw + (-u_xlat8.xz);
					                u_xlat6.xw = u_xlat6.xw + vec2(0.5, 0.5);
					                u_xlat46 = dot(u_xlat6.xw, u_xlat6.xw);
					                u_xlat46 = sqrt(u_xlat46);
					                u_xlat8.x = 0.0399999991 / u_xlat46;
					                u_xlat34.x = u_xlat6.w * u_xlat6.x;
					                u_xlat34.x = u_xlat34.x * 100.0;
					                u_xlat34.x = -abs(u_xlat34.x) + 0.100000001;
					                u_xlat34.x = max(u_xlat34.x, 0.0);
					                u_xlat8.x = u_xlat34.x * u_xlat8.y + u_xlat8.x;
					                u_xlat34.x = dot(u_xlat6.xw, vec2(0.70712316, 0.707090378));
					                u_xlat6.x = dot(u_xlat6.xw, vec2(-0.707090378, 0.70712316));
					                u_xlat6.x = u_xlat6.x * u_xlat34.x;
					                u_xlat6.x = u_xlat6.x * 100.0;
					                u_xlat6.x = -abs(u_xlat6.x) + 0.100000001;
					                u_xlat6.x = max(u_xlat6.x, 0.0);
					                u_xlat6.x = u_xlat6.x * 0.5;
					                u_xlat6.x = u_xlat6.x * u_xlat8.y + u_xlat8.x;
					                u_xlat45 = u_xlat46 + -1.0;
					                u_xlat45 = u_xlat45 * -1.25;
					                u_xlat45 = clamp(u_xlat45, 0.0, 1.0);
					                u_xlat46 = u_xlat45 * -2.0 + 3.0;
					                u_xlat45 = u_xlat45 * u_xlat45;
					                u_xlat45 = u_xlat45 * u_xlat46;
					                u_xlat6.x = u_xlat45 * u_xlat6.x;
					                u_xlat8.xzw = u_xlat8.www * vec3(0.656000018, 0.984000027, 2.9519999);
					                u_xlat8.xzw = sin(u_xlat8.xzw);
					                u_xlat45 = _Time.y * 3.0 + u_xlat9.w;
					                u_xlat45 = sin(u_xlat45);
					                u_xlat45 = u_xlat45 * 0.5 + 1.0;
					                u_xlat6.x = u_xlat45 * u_xlat6.x;
					                u_xlat6.x = u_xlat8.y * u_xlat6.x;
					                u_xlat7.xyz = u_xlat6.xxx * u_xlat8.xzw + u_xlat7.xyz;
					            }
					            u_xlat5.xyz = u_xlat7.xyz;
					        }
					        u_xlat6.xyz = vec3(u_xlat41) * u_xlat5.xyz;
					        u_xlat6.xyz = u_xlat6.xyz * vec3(2.0, 2.0, 2.0) + u_xlat2.xyz;
					        u_xlat8.x = float(0.0);
					        u_xlat8.y = float(0.0);
					        u_xlat8.z = float(0.0);
					        for(int u_xlati_loop_3 = int(0xFFFFFFFFu) ; u_xlati_loop_3<=1 ; u_xlati_loop_3++)
					        {
					            u_xlat4.y = float(u_xlati_loop_3);
					            u_xlat9.xyz = u_xlat8.xyz;
					            for(int u_xlati_loop_4 = int(0xFFFFFFFFu) ; u_xlati_loop_4<=1 ; u_xlati_loop_4++)
					            {
					                u_xlat4.x = float(u_xlati_loop_4);
					                u_xlat10.xy = u_xlat3.zw + u_xlat4.xy;
					                u_xlat10.xy = u_xlat10.xy * vec2(123.339996, 456.209991);
					                u_xlat10.xy = fract(u_xlat10.xy);
					                u_xlat36.xy = u_xlat10.xy + vec2(45.2099991, 87.5599976);
					                u_xlat44 = dot(u_xlat10.xy, u_xlat36.xy);
					                u_xlat10.xy = vec2(u_xlat44) + u_xlat10.xy;
					                u_xlat44 = u_xlat10.y * u_xlat10.x;
					                u_xlat10.x = fract(u_xlat44);
					                u_xlat11 = u_xlat10.xxxx * vec4(5435.0, 35.0, 4323.0, 6.28310013);
					                u_xlat12.xy = (-u_xlat4.xy) + u_xlat4.zw;
					                u_xlat10.yzw = fract(u_xlat11.xyz);
					                u_xlat10.xz = (-u_xlat10.xz) + u_xlat12.xy;
					                u_xlat10.xz = u_xlat10.xz + vec2(0.5, 0.5);
					                u_xlat4.x = dot(u_xlat10.xz, u_xlat10.xz);
					                u_xlat4.x = sqrt(u_xlat4.x);
					                u_xlat44 = 0.0399999991 / u_xlat4.x;
					                u_xlat45 = u_xlat10.z * u_xlat10.x;
					                u_xlat45 = u_xlat45 * 100.0;
					                u_xlat45 = -abs(u_xlat45) + 0.100000001;
					                u_xlat45 = max(u_xlat45, 0.0);
					                u_xlat44 = u_xlat45 * u_xlat10.y + u_xlat44;
					                u_xlat45 = dot(u_xlat10.xz, vec2(0.70712316, 0.707090378));
					                u_xlat46 = dot(u_xlat10.xz, vec2(-0.707090378, 0.70712316));
					                u_xlat45 = u_xlat45 * u_xlat46;
					                u_xlat45 = u_xlat45 * 100.0;
					                u_xlat45 = -abs(u_xlat45) + 0.100000001;
					                u_xlat45 = max(u_xlat45, 0.0);
					                u_xlat45 = u_xlat45 * 0.5;
					                u_xlat44 = u_xlat45 * u_xlat10.y + u_xlat44;
					                u_xlat4.x = u_xlat4.x + -1.0;
					                u_xlat4.x = u_xlat4.x * -1.25;
					                u_xlat4.x = clamp(u_xlat4.x, 0.0, 1.0);
					                u_xlat45 = u_xlat4.x * -2.0 + 3.0;
					                u_xlat4.x = u_xlat4.x * u_xlat4.x;
					                u_xlat4.x = u_xlat4.x * u_xlat45;
					                u_xlat4.x = u_xlat4.x * u_xlat44;
					                u_xlat10.xzw = u_xlat10.www * vec3(0.656000018, 0.984000027, 2.9519999);
					                u_xlat10.xzw = sin(u_xlat10.xzw);
					                u_xlat44 = _Time.y * 3.0 + u_xlat11.w;
					                u_xlat44 = sin(u_xlat44);
					                u_xlat44 = u_xlat44 * 0.5 + 1.0;
					                u_xlat4.x = u_xlat4.x * u_xlat44;
					                u_xlat4.x = u_xlat10.y * u_xlat4.x;
					                u_xlat9.xyz = u_xlat4.xxx * u_xlat10.xzw + u_xlat9.xyz;
					            }
					            u_xlat8.xyz = u_xlat9.xyz;
					        }
					        u_xlat3.xyz = vec3(u_xlat41) * u_xlat8.xyz;
					        u_xlat2.xyz = u_xlat3.xyz * vec3(2.0, 2.0, 2.0) + u_xlat6.xyz;
					        u_xlat40 = u_xlat27 + u_xlat40;
					    }
					    u_xlat0.xyz = u_xlat0.www * u_xlat2.xyz;
					    u_xlat0 = u_xlat0 * vec4(1.5, 1.5, 1.5, 1.5);
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