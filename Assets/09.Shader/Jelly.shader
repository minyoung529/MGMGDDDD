// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Jelly" 
{ // defines the name of the shader 
    Properties {
         _MainTex ("Base (RGB)", 2D) = "white" {}
    }
   SubShader { // Unity chooses the subshader that fits the GPU best
      
      Pass { // some shaders require multiple passes
         Tags { "Queue"="Transparent" "RenderType"="Transparent" }
         
         Cull Front // first pass renders only back faces 
            // (the "inside")
         ZWrite Off // don't write to depth buffer 
            // in order not to occlude other objects
         Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
         CGPROGRAM // here begins the part in Unity's Cg

         #pragma vertex vert 
            // this specifies the vert function as the vertex shader 
         #pragma fragment frag
            // this specifies the frag function as the fragment shader
         #include "UnityCG.cginc"
         //#include "UnityEngine"
         sampler2D _MainTex;
         struct v2f {
            float4 pos : SV_POSITION;
            half2 uv : TEXCOORD0;
         };


         v2f vert(appdata_base v)
            // vertex shader 
         {
            v2f o;
            // sin(_Time.w) Outputs a value between 0-1 based on time. Might be manipulated?
            // Dividing it changes the amount it gets displaced. Higher means less displacement
            if (v.vertex.y == -0.5){
               
            } else {
            v.vertex.x += (sin(_Time.w*3)*2-1)*abs(v.vertex.x*sin(_Time.w)/25); 
            v.vertex.z += (sin(_Time.w*5)*2-1)*abs(v.vertex.z*cos(_Time.w)/50); 
            v.vertex.y += v.vertex.y*sin(_Time.w)/75; 
            }
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.texcoord;
            return o;
              // this line transforms the vertex input parameter 
              // and returns it as a nameless vertex output parameter 
              // (with semantic SV_POSITION)
              // (X,Y,Z,W)
         }
         //*0.75 makes the back more transparent
         half4 frag(v2f i) : COLOR {
            half4 c = tex2D(_MainTex, i.uv)*0.75;
            return c;
            }

         ENDCG // here ends the part in Cg 
      }
      Pass {
         Tags { "Queue"="Transparent" "RenderType"="Transparent" }
         Cull Back // second pass renders only front faces 
             // (the "outside")
         ZWrite Off // don't write to depth buffer 
            // in order not to occlude other objects
         Blend SrcAlpha OneMinusSrcAlpha // use alpha blending

         CGPROGRAM 
 
         #pragma vertex vert 
            // this specifies the vert function as the vertex shader 
         #pragma fragment frag
            // this specifies the frag function as the fragment shader
         #include "UnityCG.cginc"
         // #include "UnityEngine"
 
         sampler2D _MainTex;
         struct v2f {
            float4 pos : SV_POSITION;
            half2 uv : TEXCOORD0;
         };
 
         v2f vert(appdata_base v)
            // vertex shader 
         {
            v2f o;
            // sin(_Time.w) Outputs a value between 0-1 based on time. Might be manipulated?
            // Dividing it changes the amount it gets displaced. Higher means less displacement
            if (v.vertex.y == -0.5){
               
            } else {
            v.vertex.x += (sin(_Time.w*3)*2-1)*abs(v.vertex.x*sin(_Time.w)/25); 
            v.vertex.z += (sin(_Time.w*5)*2-1)*abs(v.vertex.z*cos(_Time.w)/50); 
            v.vertex.y += v.vertex.y*sin(_Time.w)/75; 
            }
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.texcoord;
            return o;
              // this line transforms the vertex input parameter 
              // and returns it as a nameless vertex output parameter 
              // (with semantic SV_POSITION)
              // (X,Y,Z,W)
         }
         half4 frag(v2f i) : COLOR {
            half4 c = tex2D(_MainTex, i.uv);
            return c;
         }
 
         ENDCG  
      }
   }
   FallBack "Diffuse"
}
