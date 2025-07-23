Shader "Custom/FadingLine"
{
    Properties
    {
        _Color ("Line Color", Color) = (1,1,1,1)
        _FadeLength ("Fade Length", Range(0.0, 1.0)) = 0.2
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
            float _FadeLength;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 localPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.localPos = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float fade = 1.0;

                // Calculate fade based on Y position in local space
                float y = i.localPos.y;

                // Assuming cylinder is centered at y = 0 and scaled along Y
                float halfHeight = 1.;

                // Fade out near the ends using smoothstep
                float fadeTop = smoothstep(halfHeight, halfHeight - _FadeLength, y);
                float fadeBottom = smoothstep(-halfHeight, -halfHeight + _FadeLength, y);

                fade = min(fadeTop, fadeBottom);

                return fixed4(_Color.rgb, _Color.a * fade * 0.5);
            }
            ENDCG
        }
    }
}
