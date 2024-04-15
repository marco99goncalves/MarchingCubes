Shader "Custom/TestGPT"
{
    Properties
    {
        _Point("Center Point", Vector) = (0,0,0,1) // Ensure w is 1 for position
        _Color("Base Color", Color) = (1,1,1,1)
        _EffectStrength("Effect Strength", Float) = 1.0
        _Alpha("Alpha Value", Range(0, 1)) = 1.0
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 position; // Including position in Input struct
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _Point; // Ensure this is recognized as a fixed4 or float4
        half _EffectStrength;
        half _Alpha;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Convert position to world space
            fixed3 worldPosition = mul(unity_ObjectToWorld, IN.position).xyz;
            fixed distance = length(worldPosition - _Point.xyz);

            // Modify the albedo according to the distance
            fixed3 colorShift = 1;// _Color.rgb * (1 - saturate(distance / _EffectStrength));
            //o.Albedo = c.rgb;//o.Albedo * colorShift;
            
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = _Alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
