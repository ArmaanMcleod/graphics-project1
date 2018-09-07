// Original Cg/HLSL code stub copyright (c) 2010-2012 SharpDX - Alexandre Mutel
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// Adapted for COMP30019 by Jeremy Nicholson, 10 Sep 2012
// Adapted further by Chris Ewin, 23 Sep 2013
// Adapted further (again) by Alex Zable (port to Unity), 19 Aug 2016
// Adapted further (again x 3) by Armaan McLeod, Alice Lin and Alex Vosnakis

//UNITY_SHADER_NO_UPGRADE

Shader "Unlit/PhongShader"
{
	Properties
	{
        _PointLightColor("Point Light Color", Color) = (0,0,0)
        _PointLightPosition("Point Light Position", Vector) = (0.0,0.0,0.0)
        
        _DirtTex ("Dirt Texture", 2D) = "white" {}
        _GrassTex ("Grass Texture", 2D) = "white" {}
        _RockTex ("Rock Texture", 2D) = "white" {}
        _SnowTex ("Snow Texture", 2D) = "white" {}
	}
	SubShader
	{
        Tags { "RenderType"="Opaque" }
		Pass
	    {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma debug

            #include "UnityCG.cginc"

            // Point light colour and position
            uniform float3 _PointLightColor;
            uniform float3 _PointLightPosition;

            // Height sections
            uniform float _DirtHeight;
            uniform float _GrassHeight;
            uniform float _RockHeight;

            // Textures
            uniform sampler2D _DirtTex;
            uniform sampler2D _GrassTex;
            uniform sampler2D _RockTex;
            uniform sampler2D _SnowTex;

            // Input vertex
            struct vertIn
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            // Output vertex
            struct vertOut
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float4 worldVertex : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            // Implementation of the vertex shader
            vertOut vert(vertIn v)
            {
                vertOut o;

                // Convert Vertex position and corresponding normal into world coords
                float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
                float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

                // Transform vertex in world coordinates to camera coordinates, and pass colour
                o.vertex = UnityObjectToClipPos(v.vertex);
                fixed4 color;
                float height = v.vertex.y;
                float4 textureCoord = float4(v.uv,0,0);

                // Colour texture based on height
                if (height <= _DirtHeight) {
                    color = tex2Dlod(_DirtTex, textureCoord);
                } 
                else if (height <= _GrassHeight && height > _DirtHeight) {
                    color = tex2Dlod(_GrassTex, textureCoord);
                } 
                else if (height <= _RockHeight && height > _GrassHeight) {
                    color = tex2Dlod(_RockTex, textureCoord);
                } 
                else {
                    color = tex2Dlod(_SnowTex, textureCoord);
                }
                o.color = color;

                // Pass out the world vertex position and world normal to be interpolated
                // in the fragment shader (and utilised)
                o.worldVertex = worldVertex;
                o.worldNormal = worldNormal;

                return o;
            }

            // Implementation of the fragment shader
            fixed4 frag(vertOut v) : SV_Target
            {
                // Our interpolated normal might not be of length 1
                float3 interpNormal = normalize(v.worldNormal);

                // Calculate ambient RGB intensities
                float Ka = 1;
                float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

                // Calculate diffuse RBG reflections
                float fAtt = 1;
                float Kd = 0.7;
                float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);

                // If the light is hitting from underneath the terrain only return the ambient 
                // component
                if(L.y<0){
                    v.color.rgb = amb.rgb;
                    return v.color;
                }

                float LdotN = dot(L, interpNormal);
                float3 dif = fAtt * _PointLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);

                // Calculate specular reflections using Blinn-Phong approximation
                float Ks = 0.1;
                float specN = 5; // A higher specular power is needed when using Blinn-Phong
                float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
                float3 H = normalize(V + L);
                float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);

                // Combine Phong illumination model components
                v.color.rgb = amb.rgb + dif.rgb + spe.rgb;

                return v.color;
            }
            ENDCG
        }
    }
}