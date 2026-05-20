Shader "Custom/WaterShader"
{
    Properties // properties show in inspector
    {
        //Cbuffer("name", variable type) = value
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _Intensity("Intensity",Float) = 1
        _Vector("Vector", Vector) = (1,1,1)
    }

    SubShader // shader code
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass //renders the object
        //only second pass if casting shadows 
        {
            HLSLPROGRAM

            //pragma = shaders 
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes //send in from the mesh
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings //output vertex shader
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
               //texcoord0 is for vertex -> fragment
            };

            //these are used for textures
            //TEXTURE2D(_BaseMap);
            //SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial) //declare vavriables here
                half4 _BaseColor;
                float _Intensity;
                float3 _Vector; 
            CBUFFER_END


            //vertex shader
            Varyings vert(Attributes IN)
            {
                //simplex noise = math function that puts in X and Z position of vertex, puts out value 0 to 1 wich we can inout in position.y
                Varyings OUT;
                IN.positionOS.y = sin(dot(IN.positionOS.x, IN.positionOS.z)* _Time.x);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                //OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }
            
           
            //fragment shader, always returns a half4 or float4
            half4 frag(Varyings IN) : SV_Target
            {
                //half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                half4 color = _BaseColor;
                return color;
            }

            
            ENDHLSL
        }
    }
}
