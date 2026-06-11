// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/HeartShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue" = "Opaque" "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" "LightMode" = "UniversalForward"}
        LOD 100

        ZWrite On

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #pragma shader_feature _FORWARD_PLUS
            #pragma shader_feature_fragment _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma shader_feature_fragment _ADDITIONAL_LIGHT_SHADOWS

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal: NORMAL; 
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 vertexWS: TEXCOORD1;
                float4 normal: NORMAL;
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
                o.normal = v.normal;
                v.vertex = mul(rot, v.vertex);
                o.vertexWS = mul(UNITY_MATRIX_M, v.vertex);
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                // InputData lighting = (InputData)0;
                // handles lit
                // lighting.positionWS = i.vertexWS;
                // lighting.normalWS = normalize(mul(UNITY_MATRIX_M, float4(i.normal.xyz, 0)));
                // lighting.viewDirectionWS = GetWorldSpaceViewDir(i.vertexWS);
                // lighting.shadowCoord = TransformWorldToShadowCoord(i.vertexWS);

                // SurfaceData surface = (SurfaceData) 0;
                // surface.albedo = col.xyz;
                // surface.alpha = 1;
                // surface.smoothness = 0;
                // surface.specular = 0.5;
                // surface.metallic = 0;
                // return UniversalFragmentBlinnPhong(lighting, surface) + unity_AmbientSky;
                return col;
            }
            ENDHLSL
        }   
    }
}
