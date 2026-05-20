// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/HeartShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                float4x4 M = UNITY_MATRIX_M;
                //v.vertex = mul(M, v.vertex);

                v.vertex.y += 4 * sin(_Time.y);
                
                float angle = _Time.y;
                float4x4 rot = float4x4
				(
                    cos(angle), 0,sin(angle), 0,
					0, 1,  0, 0,
					-sin(angle), 0, cos(angle), 0,
                    0, 0, 0, 1
					
				);
                v.vertex = mul(rot, v.vertex);

     //            float4x4 rot4d = float4x4
     //            (
     //                cos(angle), 0,sin(angle), 0,
					// 0, 1,  0, 0,
					// 0, 0, 1, 0,
     //                sin(angle), 0, cos(angle), 0
     //            );
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
