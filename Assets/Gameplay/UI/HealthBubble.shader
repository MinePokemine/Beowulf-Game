Shader "Custom/HealthBubble"
{
    Properties
    {
        _HealthColor("Health Color", Color) = (1, 0, 0, 1)
        _DeathColor("Dea Color", Color) =     (0, 0, 0, 1)
        _WobbleMoveSpeed("Wobble Move Speed", Float) = 0.1
        [MainTexture] _MainTex("Ground Texture", 2D) = "white" {}
        _Health("Health", Float) = 1
        _WobbleTex("Wobble Texture", 2D) = "white"
        _WobbleAmt("Wobble Amount", Float) = 0.5
        _WobbleSpeed("Wobble Speed", Float) = 0.5
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }

        Stencil {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        ColorMask [_ColorMask]

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Tags { "LightMode"="SRPDefaultUnlit" }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_WobbleTex);
            SAMPLER(sampler_WobbleTex);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _MainTex_ST;
                float _Health;
                float4 _HealthColor;
                float4 _DeathColor;
                float4 _WobbleTex_ST;
                float _WobbleAmt;
                float _WobbleSpeed;
                float _WobbleMoveSpeed;
            CBUFFER_END

            Varyings vert(Attributes IN) {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color;
                return OUT;
            }

            float getWobble(float x) {
                //return _WobbleAmt * (SAMPLE_TEXTURE2D(_WobbleTex, sampler_WobbleTex, float2(x, (t)).r-0.25);
                return _WobbleAmt * sin(x * 2 * 3.141592653589 + _Time.y * 3.141592653589 * _WobbleMoveSpeed) * sin(((_Time.y * _WobbleSpeed) % 1) * 2 * 3.141592653589);
            }

            half4 frag(Varyings IN) : SV_Target {
                float2 cuv = IN.uv - 0.5;
                if (cuv.x * cuv.x + cuv.y * cuv.y > 0.25) {
                    return half4(0,0,0,0);
                    return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                }
                if (IN.uv.y + getWobble(IN.uv.x) <= _Health) {
                    return _HealthColor;
                }
                return half4(0,0,0,1);
            }
            ENDHLSL
        }
    }
}
