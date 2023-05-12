Shader "Depth"
{
    Properties
    {
        _N("_N", float) = 0.0
        _F("_F", float) = 0.0
        _F_N("_F_N", float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _CameraDepthTexture;
            float _N;
            float _F;
            float _F_N;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,i.uv));
                float a = _N / _F_N * (_F / (_N + _F_N * depth) - 1.0f);
                return float4(a, a, a, 1);
            }
            ENDCG
        }
    }
}