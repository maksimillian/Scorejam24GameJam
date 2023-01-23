Shader "Custom/JellyWobble" {
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Wobble ("Wobble", Range(0, 1)) = 0.5
        _Speed ("Speed", Range(0, 10)) = 1
    }
 
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float _Wobble;
            float _Speed;
 
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag (v2f i) : COLOR {
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 uv = i.uv;
                uv -= 0.5;
                uv *= 1.0 + _Wobble * sin(_Time.y * _Speed);
                uv += 0.5;
                col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}