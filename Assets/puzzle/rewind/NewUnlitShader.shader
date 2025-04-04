Shader "Custom/GlitchEffectShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" { }
        _ShakePower ("Shake Power", Float) = 0.03
        _ShakeRate ("Shake Rate", Range(0, 1)) = 0.2
        _ShakeSpeed ("Shake Speed", Float) = 5.0
        _ShakeBlockSize ("Shake Block Size", Float) = 30.5
        _ShakeColorRate ("Shake Color Rate", Range(0, 1)) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Tags { "Queue" = "Overlay" }
            Blend SrcAlpha OneMinusSrcAlpha // Add this line to support transparency
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
    
            // Properties
            sampler2D _MainTex;
            float _ShakePower;
            float _ShakeRate;
            float _ShakeSpeed;
            float _ShakeBlockSize;
            float _ShakeColorRate;
    
            // Function to generate random values
            float random(float seed)
            {
                return frac(sin(dot(float2(seed, seed), float2(3525.46, -54.3415))) * 543.2543);
            }
    
            // Vertex Shader
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
    
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };
    
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
    
            // Fragment Shader
            half4 frag(v2f i) : SV_Target
            {
                // Get random value based on time
                float enable_shift = float(random(_Time.y * _ShakeSpeed) < _ShakeRate);
                float2 uv = i.uv;
    
                // Apply shake effect to the UV coordinates
                uv.x += (random((floor(i.uv.y * _ShakeBlockSize) / _ShakeBlockSize) + _Time.y) - 0.5) * _ShakePower * enable_shift;
    
                // Sample the texture at the modified UV coordinates
                half4 pixelColor = tex2D(_MainTex, uv);
    
                // Apply color shift
                pixelColor.r = lerp(pixelColor.r, tex2D(_MainTex, uv + float2(_ShakeColorRate, 0.0)).r, enable_shift);
                pixelColor.b = lerp(pixelColor.b, tex2D(_MainTex, uv + float2(-_ShakeColorRate, 0.0)).b, enable_shift);
    
                // Maintain original alpha (transparency)
                pixelColor.a = pixelColor.a;
    
                return pixelColor;
            }
            ENDCG
        }
    }
    
}
