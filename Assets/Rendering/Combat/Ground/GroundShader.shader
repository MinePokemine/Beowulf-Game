Shader "Custom/GroundShader"
{
    Properties
    {
        _Recolor("Recolor Map", 2D) = "white"
        [MainTexture] _BaseMap("Base Map", 2D) = "white"
        _TileX("Tiling X", Integer) = 1
        _TileY("Tiling Y", Integer) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_Recolor);
            SAMPLER(sampler_Recolor);

            CBUFFER_START(UnityPerMaterial)
                float4 _Recolor_ST;
                float _TileX;
                float _TileY;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _Recolor);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                float2 tiledUV = frac(IN.uv * float2(_TileX, _TileY));
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, tiledUV) 
                    * SAMPLE_TEXTURE2D(_Recolor, sampler_Recolor, IN.uv);
                return color;
            }
            ENDHLSL
        }
    }
}
