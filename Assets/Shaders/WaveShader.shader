﻿//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/WaveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude ("Amplitude", float) = 0.5
    }
    SubShader
    {
        Pass
        {
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; 
            float _Amplitude;

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertOut
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Implementation of the vertex shader
            vertOut vert(vertIn v)
            {
                // Displace the original vertex in model space
                //float4 displacement = float4(0.0f, 0.0f, 0.0f, 0.0f);
                //v.vertex += displacement;

                vertOut o;
                v.vertex.y += _Amplitude * sin(_Time.y + v.vertex.x);
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            // Implementation of the fragment shader
            fixed4 frag(vertOut v) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, v.uv);
                return col;
            }
            ENDCG
        }
    }
}