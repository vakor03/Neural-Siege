Shader "Unlit/HealthBarShader" {
    Properties {
        _Fulfill ("Fulfillness" ,Range(0,1)) = 1
        _StartColor("Start Color", Color) = (1,1,1,1)
        _EndColor("End Color", Color) = (1,1,1,1)
        _StartThreshold("Start Threshold", Float) = 0.2
        _EndThreshold("End Threshold", Float) = 0.8
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _Fulfill;
            float4 _StartColor;
            float4 _EndColor;
            float _StartThreshold;
            float _EndThreshold;
            
            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float InverseLoop(float a, float b, float x)
            {
                return (x-a)/(b-a);
            }

            Interpolators vert(MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(Interpolators i) : SV_Target
            {
                float healthBarMask = _Fulfill > i.uv.x;
                clip(healthBarMask-0.5);
                
                float tHealthColor = saturate(InverseLoop(_StartThreshold, _EndThreshold, _Fulfill));
                float4 healthBarColor = lerp(_EndColor, _StartColor,
                                             InverseLoop(_StartThreshold, _EndThreshold, tHealthColor));
                float4 backgroundColor = float4(0,0,0,1);


                return lerp(backgroundColor, healthBarColor, healthBarMask);
            }
            ENDCG
        }
    }
}