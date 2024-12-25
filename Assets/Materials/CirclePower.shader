Shader "Custom/CirclePower"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Progress;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv - float2(0.5, 0.5); // Center UV to (0, 0)
                float angle = atan2(uv.y, uv.x) / (2 * UNITY_PI) + 0.5; // Normalize angle to [0, 1]
                float dist = length(uv);

                // If the angle is within the progress range and inside the circle, show the sprite
                if (angle <= _Progress && dist <= 0.5)
                    return tex2D(_MainTex, i.uv) * i.color;
                else
                    return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
    FallBack "Transparent"
}
