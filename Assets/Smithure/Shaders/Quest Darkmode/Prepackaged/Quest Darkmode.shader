Shader "Smithure/Quest Darkmode"
{
    Properties
    {
		_Color ("Color", Color) = (1,1,1,0)
		_Alpha ("Alpha Multiplier", Range(0,1)) = 1//value that can be edited by udon
        _MainTex ("Texture", 2D) = "white" {}

    }
    SubShader
    {
        Tags { "Queue"="Transparent-1" }//this helps the user make materials that are not effected
        //anything with a renderqueue 2999 or below will be effected by darkmode.
		
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		ZTest Always
        Pass
        {
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
sampler2D _MainTex;//future planned quest screen filter idea.

			float4 _Color;
			float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 tex = tex2D(_MainTex, i.uv);
                float4 col = float4(_Color.rgb, _Color.a * _Alpha) * tex;
                return col;
            }
            ENDCG
        }
    }
}
