Shader "Custom/GlowPulseShader"
{
    Properties
    {
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
        
        fixed4 _Color;
        float _GlowStrength;
        float _PulseSpeed;

        struct Input
        {
            float2 _Color;
        }; //useless but error if not here

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = _Color;
            o.Albedo = c.rgb * (_GlowStrength + 0.5) * (sin(_Time.y * _PulseSpeed) * 30 + 50); //last number is sine wave offset, second to last is wave amplitude, glow affect and timing works off sine wave goes between 1 and -1
            o.Alpha = c.a;
        }
        ENDCG
    }
}

