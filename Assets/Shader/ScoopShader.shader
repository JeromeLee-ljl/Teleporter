Shader "MyShader/ScoopShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        // _Alpha ("Alpha", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { 
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"  // 说明subshader使用了透明度混合
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 screenPos: TEXCOORD2;
            };


            sampler2D _MainTex;
            float4 _MainTex_ST;
            // float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                i.screenPos /= i.screenPos.w;

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.screenPos);
                return col;
            }
            ENDCG
        }
    }
    Fallback "Transparent/VertexLit"
}
