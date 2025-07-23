Shader "Custom/PulsatingIndicator"
{
    Properties
    {
        _Color ("Ring Color", Color) = (1,1,1,1)
        _RingWidth ("Ring Width", Range(0.01, 1)) = 0.1
        _RingSpacing ("Ring Spacing", Range(0.1, 2)) = 0.5
        _Speed ("Pulse Speed", Range(1, 10)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _RingWidth;
            float _RingSpacing;
            float _Speed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2 - 1; // center at 0,0
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float dist = length(uv);
                float t = _Time * _Speed * 10.;

                // Create moving ring wave
                // float ring = abs(frac(dist / _RingSpacing - t));
                float sine = abs(sin(dist*dist*dist / _RingSpacing * 5. + t));
                float ring = smoothstep(_RingWidth, 0.0, sine);
                float maskOuter = ring * smoothstep(0.5, 0.4, dist);
                float maskInner = maskOuter * smoothstep(0.05, 0.3, dist);

                return fixed4(_Color.rgb, _Color.a * maskInner);
                // return fixed4(_Color.rgb, _Color.a * mask);
            }
            ENDCG
        }
    }
}
