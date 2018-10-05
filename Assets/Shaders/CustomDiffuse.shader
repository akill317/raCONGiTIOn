// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Diffuse"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
    _Color("Tint", Color) = (1,1,1,1)
        _Intensity("Intensity", Float) = 1
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
    }

        SubShader
        {
            Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

            Cull Off
            Lighting Off
            ZWrite Off
            Blend One OneMinusSrcAlpha

            CGPROGRAM
    #pragma surface surf Lambert vertex:vert nofog keepalpha
    #pragma multi_compile _ PIXELSNAP_ON
    #pragma shader_feature ETC1_EXTERNAL_ALPHA

            sampler2D _MainTex;
        fixed4 _Color;
        sampler2D _AlphaTex;
        float _Intensity;

        struct Input
        {
            float2 uv_MainTex;
            fixed4 color;
        };

        void vert(inout appdata_full v, out Input o)
        {
    #if defined(PIXELSNAP_ON)
            v.vertex = UnityPixelSnap(v.vertex);
    #endif
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color * _Color;
        }

        float hash(float n)
        {
            return frac(sin(n)*43758.5453);
        }


        fixed4 SampleSpriteTexture(float2 uv)
        {
            fixed4 color = tex2D(_MainTex, float2(uv.x + _Intensity*0.05 * sin(frac(sin(_Time[1] * 100))) * hash(uv.y * _Time[1]), uv.y + _Intensity* 0.05 * sin(frac(sin(_Time[1] * 100)))* hash(uv.x * _Time[1])));

    #if ETC1_EXTERNAL_ALPHA
            color.a = tex2D(_AlphaTex, uv).r;
    #endif //ETC1_EXTERNAL_ALPHA

            return color;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;
            o.Albedo = c.rgb * c.a;
            o.Alpha = c.a;
        }
        ENDCG
        }

            Fallback "Transparent/VertexLit"
}
