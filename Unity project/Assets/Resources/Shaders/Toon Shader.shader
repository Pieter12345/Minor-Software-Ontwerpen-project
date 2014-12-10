Shader "Toon Shader/Textured" {
   Properties {
      _Color ("Diffuse Material Color Tint", Color) = (1,1,1,1) 
      _MainTex ("Diffuse Texture", 2D) = "white" {}
      _Treshold ("Shadow-Treshold", range(0,1)) = 0.5
      _Shadow ("Shadow-Darkness", range(0,3)) = 0.5
      _Intensity ("Intensity-Light", range(0,5)) = 0.5
      _Diffuse ("Toon-Factor", range(1,0.0001)) = 0
   }
   SubShader {
      Pass {	
         Tags { "LightMode" = "ForwardBase" } 
            // pass for ambient light and first light source
 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc"
         uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         // User-specified properties
         uniform sampler2D _MainTex;
         uniform float4 _MainTex_ST;
         uniform float4 _Color; 
		 uniform float _Treshold;
		 uniform float _Intensity;
		 uniform float _Shadow;
		 uniform float _Diffuse;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
            float4 posWorld : TEXCOORD1;
            float3 normalDir : TEXCOORD2;

         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            output.posWorld = mul(modelMatrix, input.vertex);
            output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            output.tex = input.texcoord;
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            float3 normalDirection = normalize(input.normalDir);
 
            float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(_WorldSpaceLightPos0.xyz);
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
 			// Ambient Component
            float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb;
 
 			//Diffuse component
 			float3 temp;
 
 			if (max(0.0, dot(normalDirection, lightDirection) > _Treshold)) {temp = (_Intensity,_Intensity,_Intensity);}
 			else {temp = (_Shadow,_Shadow,_Shadow);}

            float3 diffuseReflection = attenuation * _LightColor0.rgb * temp * pow(max(0.0, dot(normalDirection, lightDirection)),_Diffuse);              
 
            //texture maps
            float4 tex = tex2D(_MainTex, input.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
            float3 texLighting = tex.xyz * _LightColor0.rgb;
             
            return float4((diffuseReflection + ambientLighting)* texLighting * _Color.rgb, 1.0);
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
         uniform sampler2D _MainTex;
         uniform float4 _MainTex_ST;
         uniform float4 _Color; 
		 uniform float _Treshold;
		 uniform float _Intensity;
		 uniform float _Shadow;
		 uniform float _Diffuse;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
            float4 posWorld : TEXCOORD1;
            float3 normalDir : TEXCOORD2;

         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            output.posWorld = mul(modelMatrix, input.vertex);
            output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            output.tex = input.texcoord;
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            float3 normalDirection = normalize(input.normalDir);
 
            float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(_WorldSpaceLightPos0.xyz);
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
 			// Ambient Component
            float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb;
 
 			//Diffuse component
 			float3 temp;
 
 			if (max(0.0, dot(normalDirection, lightDirection) > _Treshold)) {temp = (_Intensity,_Intensity,_Intensity);}
 			else {temp = (_Shadow,_Shadow,_Shadow);}

            float3 diffuseReflection = attenuation * _LightColor0.rgb * temp * pow(max(0.0, normalize(dot(normalDirection, lightDirection))),_Diffuse);              
 
            //texture maps
            float4 tex = tex2D(_MainTex, input.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
            float3 texLighting = tex.xyz * _LightColor0.rgb;
             
            return float4(diffuseReflection * texLighting.xyz * _Color.xyz, 1.0);
         }
 
         ENDCG
      }
   }
   // The definition of a fallback shader should be commented out 
   // during development:
   // Fallback "Specular"
}