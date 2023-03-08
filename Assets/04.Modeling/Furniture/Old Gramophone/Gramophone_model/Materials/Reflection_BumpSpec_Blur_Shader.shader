Shader "Reflective/Reflection_BumpSpec_Blur_Shader " {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	 
	 
	_MainTex ("Base (RGB) RefStrength (A)", 2D) = "white" {} 
	_SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
	_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
	_Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
	_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
	_fallow("_Fallow", Range (0.01, 1.5)) = 1.0
	_level("_level", Range (0.01, 1.5)) = 1.0
	_mip("_Mip", Range (0.0, 5)) = 1.0
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_normal("_NormalLevel", Range (0.01, 1)) = 1.0
	 
	 
}
SubShader {
	LOD 200
	Tags { "RenderType"="Opaque" }
	
CGPROGRAM
#pragma surface surf BlinnPhong
#pragma target 3.0
#pragma glsl
#pragma exclude_renderers d3d11_9x

sampler2D _MainTex;
 
samplerCUBE _Cube;

fixed4 _Color;
fixed4 _ReflectColor;
half _mip;
 
sampler2D _BumpMap;
half _normal;
half _fallow;
half _level; 
half _Shininess;

struct Input {
	float2 uv_MainTex;
	float3 worldRefl;
	float2 uv_BumpMap;
	INTERNAL_DATA
	float3 viewDir;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	 
	fixed4 c = tex * _Color;
	o.Albedo = c.rgb  ;
	
	half _normal_invert = 1 - _normal;
	
	float3 normal_1 = UnpackNormal(tex2D(_BumpMap,IN.uv_BumpMap))  * _normal ;
	float3 normal_2 = UnpackNormal(tex2D(_BumpMap,IN.uv_BumpMap *0))  * _normal_invert  ;  
	 
	o.Gloss = tex.a*2  ;
	o.Specular = _Shininess; 
	 
	o.Normal = normal_1 + normal_2  ;
	float3 worldRefl = WorldReflectionVector (IN, o.Normal);
	 
	fixed4 Fresnel  = float4(0,0,1,1); 
	fixed4 Fresnel1=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel.xyz ) )) * _fallow  + _level  ;
     
    
	 
	
	fixed4 reflcol = texCUBElod(_Cube, half4(worldRefl, _mip)) *0.3;
	fixed4 reflcol2 = texCUBElod(_Cube, half4(worldRefl , _mip) + (_mip * 0.005))*0.3;
	fixed4 reflcol3 = texCUBElod(_Cube, half4(worldRefl , _mip) - (_mip * 0.005))*0.1;
	reflcol *= o.Gloss;
	reflcol += reflcol2 * reflcol3 + reflcol3;
	o.Emission = reflcol.rgb * _ReflectColor.rgb * Fresnel1;
	o.Alpha = reflcol.a * _ReflectColor.a;
	
	 
	 
}
ENDCG
}
	
FallBack "Reflective/VertexLit"
} 
