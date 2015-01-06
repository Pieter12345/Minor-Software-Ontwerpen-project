Shader "Toon Shader/transparent" {
   Properties {
      _Color ("Diffuse Material Color", Color) = (1,1,1,1) 
      _Treshold ("Shadow-Treshold", range(0,1)) = 0.5
      _Shadow ("Shadow-Darkness", range(0,3)) = 0.5
      _Intensity ("Intensity-Light", range(0,5)) = 0.5
      _Diffuse ("Toon-Factor", range(1,0.0001)) = 0
      _Alpha ("Transparancy", range(0,1)) = 1
         }
   SubShader {
   	  
   	  Tags { "Queue" = "Transparent" } //makes sure opaque geometry is rendered/drawn first
   	  
      Pass {
         ZWrite Off //Depth writing in pixels toggled off	
         Tags { "LightMode" = "ForwardBase" } 
            // pass for ambient light and first light source
 
 		 Blend SrcAlpha OneMinusSrcAlpha // alpha blending
 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc"
         uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         // User-specified properties
         uniform float4 _Color; 
		 uniform float _Treshold;
		 uniform float _Intensity;
		 uniform float _Shadow;
		 uniform float _Diffuse;
		 uniform float _Alpha;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 posWorld : TEXCOORD0;
            float3 normalDir : TEXCOORD1;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            output.posWorld = mul(modelMatrix, input.vertex);
            output.normalDir = normalize(
               mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            float3 normalDirection = normalize(input.normalDir);
 
            float3 viewDirection = normalize(
               _WorldSpaceCameraPos - input.posWorld.xyz);
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(_WorldSpaceLightPos0.xyz);
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = 
                  _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
            float3 ambientLighting = 
               UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
 
 			float3 temp;
 
 			if (max(0.0, dot(normalDirection, lightDirection) > _Treshold)) {temp = (_Intensity,_Intensity,_Intensity);}
 			else {temp = (_Shadow,_Shadow,_Shadow);}

            float3 diffuseReflection = 
               attenuation * _LightColor0.rgb * _Color.rgb * temp
               * pow(max(0.0, dot(normalDirection, lightDirection)),_Diffuse);              
 
             
            return float4(ambientLighting + diffuseReflection
               , _Alpha);
         }
 
         ENDCG
      }
 
      Pass {	
         Tags { "LightMode" = "ForwardAdd" } 
            // pass for additional light sources
         Blend One One // additive blending 
 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc"
         uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         // User-specified properties
         uniform float4 _Color; 
		 uniform float _Treshold;
		 uniform float _Intensity;
		 uniform float _Shadow;
		 uniform float _Diffuse;
		 uniform float _Alpha;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 posWorld : TEXCOORD0;
            float3 normalDir : TEXCOORD1;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            output.posWorld = mul(modelMatrix, input.vertex);
            output.normalDir = normalize(
               mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            float3 normalDirection = normalize(input.normalDir);
 
            float3 viewDirection = normalize(
               _WorldSpaceCameraPos - input.posWorld.xyz);
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(_WorldSpaceLightPos0.xyz);
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = 
                  _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
            float3 ambientLighting = 
               UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
 
 			float3 temp;
 
 			if (max(0.0, dot(normalDirection, lightDirection) > _Treshold)) {temp = (_Intensity,_Intensity,_Intensity);}
 			else {temp = (_Shadow,_Shadow,_Shadow);}
 
            float3 diffuseReflection = 
               attenuation * _LightColor0.rgb * _Color.rgb
               * temp * pow(max(0.0, dot(normalDirection, lightDirection)),_Diffuse);
          
 
             
            return float4(ambientLighting + diffuseReflection
               , _Alpha);
         }
 
         ENDCG
      }
   }
   // The definition of a fallback shader should be commented out 
   // during development:
   // Fallback "Specular"
}