Shader "Custom/GlowPulseShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (250,1,1,1)
        _GlowStrength ("Glow Strength", Range(0,1)) = 0.5
        _PulseSpeed ("Pulse Speed", Range(0,10)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert
        
        sampler2D _MainTex;
        fixed4 _Color;
        float _GlowStrength;
        float _PulseSpeed;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * (_GlowStrength + 0.5) * (sin(_Time.y * _PulseSpeed) * 0.5 + 0.5);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

