Shader "Custom/ScrollingBackgroundShader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {} // Default texture is white (fallback)
        _ScrollSpeed("Scroll Speed", Float) = 0.1
        _Tiling("Tiling", Vector) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue" = "Background" }

        Pass
        {
            // Transparency settings to ensure proper blending
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _ScrollSpeed;
            float4 _Tiling;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Scroll the texture by modifying the UV coordinates over time
                float scrollOffset = _Time.y * _ScrollSpeed;

                // Apply the scroll and tiling effect
                float2 scrollUV = i.uv + float2(scrollOffset, 0.0);

                // Wrap the UVs to prevent them from going out of bounds
                scrollUV = frac(scrollUV);

                // Sample the texture
                half4 texColor = tex2D(_MainTex, scrollUV);

                // Discard fully transparent pixels to avoid the white background
                if (texColor.a == 0)
                {
                    discard; // Discard the pixel to make it fully transparent
                }

                return texColor; // Return the texture color, respecting transparency
            }

            ENDCG
        }
    }

    Fallback "Diffuse"
}
