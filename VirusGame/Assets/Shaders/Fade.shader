﻿Shader "Custom/Fade"
{
    Properties 
    {

        _TintColor ("TintColor", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Fade ("Fade", Range(0, 1)) = 1

    }

    SubShader 
    { 
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

    
        Blend SrcAlpha OneMinusSrcAlpha 
        Zwrite On

        LOD 200

        CGPROGRAM
        #pragma surface surf Unlit alpha noforwardadd halfasview approxview noambient

        sampler2D _MainTex;
        fixed4 _TintColor;
        fixed _Fade;


        fixed4 LightingUnlit (SurfaceOutput s, half3 lightDir, half atten) 
        {

          return fixed4(s.Albedo, s.Alpha);

        }
        
        struct Input 
        {
            fixed2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) 
        {

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * _TintColor.rgb ;

            fixed4 al = tex2D(_MainTex, IN.uv_MainTex);
            o.Alpha = c.a * _Fade;
            
        }

        ENDCG
   }
}
