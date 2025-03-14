Shader "UI/RoundedBorder"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _BorderColor ("Border Color", Color) = (0,0,0,1)
        _BorderThickness ("Border Thickness", Range(0,0.1)) = 0.05
        _CornerRadius ("Corner Radius", Range(0,0.5)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;
            fixed4 _BorderColor;
            float _BorderThickness;
            float _CornerRadius;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float dist = min(min(uv.x, uv.y), min(1.0 - uv.x, 1.0 - uv.y));

                if (dist < _CornerRadius)
                    return _BorderColor; // Border
                
                return _Color; // Main Panel
            }
            ENDCG
        }
    }
}