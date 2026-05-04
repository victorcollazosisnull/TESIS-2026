Shader "Custom/URP_ParticleStandardGlow" {
    Properties {
        [HDR] _BaseColor("Color", Color) = (1.0, 0.0, 0.0, 1.0)
        _MainTex("Glow Mask (R=Color, G=Brightness, A=Alpha)", 2D) = "white" {}
        _Brightness("Brightness Boost", Range(0.0, 5.0)) = 1.0
        
        [Header(Twinkle)]
        [Toggle(_TWINKLE_ON)] _TwinkleEnabled("Enable Twinkle", Float) = 0.0
        _TwinkleAmount("Twinkle Amount", Range(0.0, 1.0)) = 0.8
        _TwinkleSpeed("Twinkle Speed", Range(0.1, 10.0)) = 5.0

        [Header(Soft Particles)]
        _SoftFadeDistance("Soft Fade Distance", Float) = 1.0
    }

    SubShader {
        Tags { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
        }

        Pass {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha One // Aditivo para efecto Glow
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature_local _TWINKLE_ON
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float3 customData : TEXCOORD1; // Para el offset del twinkle si usas partículas
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 screenPos : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Brightness;
                float _TwinkleAmount;
                float _TwinkleSpeed;
                float _SoftFadeDistance;
            CBUFFER_END

            Varyings vert(Attributes input) {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.uv = input.uv;
                output.color = input.color;
                
                // Twinkle Logic
                #if _TWINKLE_ON
                    // Usamos el tiempo y un offset (puedes usar input.customData.x si lo mandas desde el sistema de partículas)
                    float twinkle = 1.0 - (_TwinkleAmount * (0.5 + 0.5 * cos(_Time.y * _TwinkleSpeed)));
                    output.color.a *= twinkle;
                #endif

                output.screenPos = ComputeScreenPos(output.positionCS);
                return output;
            }

            half4 frag(Varyings input) : SV_Target {
                half4 mask = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                
                // Aplicar el color y el brillo (basado en el canal R y G como el original)
                half3 finalRGB = (mask.r * input.color.rgb * _BaseColor.rgb) + (mask.g * _Brightness);
                half finalAlpha = mask.a * input.color.a;

                // Soft Particles (Simplificado para URP)
                float2 uv = input.screenPos.xy / input.screenPos.w;
                float rawDepth = SampleSceneDepth(uv);
                float sceneZ = LinearEyeDepth(rawDepth, _ZBufferParams);
                float partZ = input.screenPos.w;
                float softParticles = saturate((sceneZ - partZ) / _SoftFadeDistance);
                
                return half4(finalRGB, finalAlpha * softParticles);
            }
            ENDHLSL
        }
    }
}