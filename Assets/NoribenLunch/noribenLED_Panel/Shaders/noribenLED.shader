Shader "Noriben/noribenLED"
{
    Properties
    {
        [Header(LED Source Tex)]
        [HDR]_SourceColor ("Main Source Color", Color) = (1,1,1,1)
        _MainTex ("Main Source Tex", 2D) = "black" {}
        _Divide ("LED Divide", Int) = 1
        _Aspect ("LED Aspect Ratio", Float) = 1
        [Toggle]_IsMirror ("Flip in Mirror(For VRChat)", Float) = 0
        [Toggle]_IsAVProVideo ("AVProVideo Mode", Float) = 0
        [Toggle]_IsTopazChat ("TopazChatPlayer Mode", Float) = 0

        [Header(Color Correction)]
        _SourceContrast ("Gamma", Range(0, 4)) = 1
        _Hue ("Hue", Range(0, 1)) = 0
        _Saturation ("Saturation", Range(0, 1)) = 1

        [Header(Distance Fade)]
        [Toggle]_DistanceFadeOn ("Distance Fade Toggle", Float) = 0
        _FadeDistance ("Fade Distance", Range(0, 30)) = 1
        _FadeBlur ("Fade Blur", Range(0,20)) = 2

        [Header(Broken)]
        _BrokenTex ("Broken Tex", 2D) = "white" {}
        _BrokenPower ("Broken Power", Range(0, 1)) = 0
        _BrokenX ("Broken X Threshold", Range(0, 1)) = .8
        _BrokenY ("Broken Y Threshold", Range(0, 1)) = .2
        _BHue ("Broken Hue", Range(0, 1)) = .5
        

        [Header(Base)]
        _Color ("Color", Color) = (1,1,1,1)
        _BaseTex ("Albedo (RGB)", 2D) = "white" {}
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Culling", Float) = 0

        [Header(Metallic Smoothness)]
        _MetallicGlossMap ("MetallicSmoothness Tex", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Smoothness ("Smoothness", Range(0,1)) = 0.5

        [Header(Occlusion)]
        _OcclusionMap ("Occlusion Tex", 2D) = "white" {}
        _Occlusion ("Occlusion Intensity", Range(0, 1)) = 1

        [Header(Normal)]
        _BumpMap ("Normal Tex", 2D) = "bump" {}
        _NormalScale ("Normal scale", float) = 1

        [Header(Emission)]
        _EmissionMap ("Emission Tex", 2D) = "black" {}
        [HDR]_Emission ("Emission Intensity", float) = 1
        _EmissionClamp ("Emission Brightness Clamp", Float) = 3

        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry"}
        LOD 200
        Cull [_Cull]

        CGPROGRAM
        #include "UnityShaderVariables.cginc"
        #pragma surface surf Standard fullforwardshadows addshadow
        #pragma target 3.0       

        

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BaseTex;
            float3 worldPos;
        };

        float4 _Color;
        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _MetallicGlossMap;
        sampler2D _OcclusionMap;
        sampler2D _EmissionMap;
        float _Smoothness;
        float _Metallic;
        float _NormalScale;
        float _Occlusion;
        float _Emission;
        sampler2D _BaseTex;
        float _Divide;
        float _SourceContrast;
        float4 _SourceColor;
        float _Hue;
        float _Saturation;
        float _FadeBlur;
        float _FadeDistance;
        float _DistanceFadeOn;
        float _EmissionClamp;
        float _Aspect;
        float _IsMirror;
        float _IsAVProVideo;
        float _IsTopazChat;

        sampler2D _BrokenTex;
        float _BrokenX;
        float _BrokenY;
        float _BHue;
        float _BrokenPower;

        //1D randam
        float rand1d(float t)
        {
            return frac(sin(t) * 100000.);
        }
        
        float noise1d(float t)
        {
            float i = floor(t);
            float f = frac(t);
            return lerp(rand1d(i), rand1d(i + 1.), smoothstep(0., 1., f));
        }

        float remap(float In, float2 InMinMax, float2 OutMinMax)
        {
            return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }

        //HSV変換
        float3 hsv2rgb(float3 c)
        {
            float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
            return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
        }
        float3 rgb2hsv(float3 c)
        {
            float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
            float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
            float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
        
            float d = q.x - min(q.w, q.y);
            float e = 1.0e-10;
            return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
        }
        

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            


            // Camera distance fade
            float cameraDistance = length(_WorldSpaceCameraPos - IN.worldPos);
            cameraDistance = saturate(smoothstep(_FadeDistance, _FadeDistance + _FadeBlur, cameraDistance));
            cameraDistance = lerp(0, cameraDistance, _DistanceFadeOn);


            float divide = floor(_Divide);
            float2 uv = float2(IN.uv_BaseTex.x * divide, IN.uv_BaseTex.y * floor(divide * 1/_Aspect));
            float2 suv = IN.uv_MainTex;

            // Mirror
            if(_IsMirror && 0 < dot(cross(UNITY_MATRIX_V[0], UNITY_MATRIX_V[1]), UNITY_MATRIX_V[2]))
            {
                suv = float2(1 - suv.x, suv.y);
            }
            else {
                suv = suv;
            }

            // AVPro Video Mode UV
            if (_IsAVProVideo)
            {
                suv = float2(suv.x, 1 - suv.y);
            }

            float2 divideSUV = float2(floor(suv.x * divide) / divide, floor(suv.y * floor(divide * 1/_Aspect)) / floor(divide * 1/_Aspect));


            // Broken
            //float2 buv = suv;
            float2 buv = lerp(divideSUV, suv, pow(cameraDistance, .5));
            float4 broken = tex2D(_BrokenTex, buv);
            float brokenNoise01 = .6 * rand1d(_Time.y * .0001); 
            float brokenNoise02 = .5 * rand1d(_Time.y * .01); 
            if(broken.y > _BrokenX)
            {
                buv.y = lerp(buv.y, brokenNoise01, 1);

            }
            else if (broken.y < _BrokenY)
            {
                buv.y = lerp(buv.y, brokenNoise02, .5);
            }
            

            float brokenBlack = lerp(1, broken.x, _BrokenPower);
            float4 brokenSouceTex = tex2D(_MainTex, buv);
            float3 brokenSouceTexHSV = rgb2hsv(brokenSouceTex.rgb);

            if(broken.y > _BrokenX)
            {
                brokenSouceTexHSV.x += _BHue;
            }
            else if (broken.y < _BrokenY)
            {
                brokenSouceTexHSV.x -= _BHue;
            }

            brokenSouceTex.xyz = hsv2rgb(brokenSouceTexHSV.xyz);


            fixed4 c = tex2D (_BaseTex, uv) * _Color;
            float occlusion = tex2D(_OcclusionMap, uv) * _Occlusion;
            o.Albedo = c.rgb * occlusion;
            
            o.Metallic = tex2D(_MetallicGlossMap, uv).xyz * _Metallic;
            o.Smoothness =  (tex2D(_MetallicGlossMap, uv).w) * _Smoothness;
            o.Normal = UnpackScaleNormal(tex2D(_BumpMap, uv), _NormalScale);

            // Souce Tex
            float4 sourceDivideTex = tex2D(_MainTex, divideSUV) * 2.4;
            float4 sourceTex = tex2D(_MainTex, suv);

            sourceTex = lerp(sourceDivideTex, sourceTex, pow(cameraDistance, .5));
            sourceTex.xyz = pow(sourceTex.xyz, float3(_SourceContrast, _SourceContrast, _SourceContrast));

            // Broken Power
            sourceTex.xyz = lerp(sourceTex.xyz, brokenSouceTex.xyz, _BrokenPower);



            // Color correction
            float3 sourceTexHSV = rgb2hsv(sourceTex.xyz);
            sourceTexHSV.x += _Hue;
            sourceTexHSV.y *= _Saturation;
            sourceTex.rgb = hsv2rgb(sourceTexHSV.rgb);

            // AVPro Video Mode, TopazChat Gammma
            if (_IsAVProVideo)
            {
                sourceTex.rgb = GammaToLinearSpace(sourceTex.rgb);
            }
            else if (_IsTopazChat)
            {
                sourceTex.rgb = GammaToLinearSpace(sourceTex.rgb);
            }

            float4 emissionTex = tex2D(_EmissionMap, uv);
            emissionTex = float4(lerp(emissionTex.xyz, 1, cameraDistance), 1);

            o.Emission = sourceTex * emissionTex * _Emission * _SourceColor  * brokenBlack;
            o.Emission = clamp(o.Emission, 0, _EmissionClamp);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
