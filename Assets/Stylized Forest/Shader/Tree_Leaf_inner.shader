// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:32817,y:32663,varname:node_2865,prsc:2|diff-1306-OUT,spec-1838-OUT,gloss-1300-OUT,emission-7484-OUT,voffset-400-OUT;n:type:ShaderForge.SFN_Tex2d,id:7736,x:31921,y:32620,ptovrint:True,ptlb:Diffuse,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d3bd970d92654d1469c7340b6ff74079,ntxv:0,isnm:False|UVIN-5055-OUT;n:type:ShaderForge.SFN_TexCoord,id:7663,x:31459,y:32550,varname:node_7663,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:5055,x:31661,y:32625,varname:node_5055,prsc:2|A-7663-UVOUT,B-9913-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9913,x:31451,y:32768,ptovrint:False,ptlb:DiffuseTile,ptin:_DiffuseTile,varname:node_9913,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Fresnel,id:9987,x:32107,y:32283,varname:node_9987,prsc:2|EXP-616-OUT;n:type:ShaderForge.SFN_OneMinus,id:5014,x:32144,y:32482,varname:node_5014,prsc:2|IN-7736-R;n:type:ShaderForge.SFN_ValueProperty,id:616,x:31930,y:32283,ptovrint:False,ptlb:FresnelPower,ptin:_FresnelPower,varname:node_616,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:5226,x:32312,y:32310,varname:node_5226,prsc:2|A-9987-OUT,B-5014-OUT;n:type:ShaderForge.SFN_Lerp,id:1306,x:32378,y:32095,varname:node_1306,prsc:2|A-5341-RGB,B-2682-RGB,T-5226-OUT;n:type:ShaderForge.SFN_Color,id:5341,x:32107,y:31951,ptovrint:False,ptlb:Color1,ptin:_Color1,varname:node_5341,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.005831636,c2:0.1691176,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:2682,x:32107,y:32112,ptovrint:False,ptlb:Color2,ptin:_Color2,varname:_Color2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4919371,c2:0.8014706,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:7484,x:32546,y:32485,varname:node_7484,prsc:2|A-1306-OUT,B-8869-OUT;n:type:ShaderForge.SFN_Lerp,id:1646,x:32642,y:32304,varname:node_1646,prsc:2|A-1306-OUT,B-9900-RGB,T-7736-R;n:type:ShaderForge.SFN_Color,id:9900,x:32437,y:32271,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_9900,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.2647059,c3:0.03468563,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:8869,x:32278,y:32576,ptovrint:False,ptlb:EmissivePower,ptin:_EmissivePower,varname:node_8869,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Vector1,id:1838,x:32592,y:32737,varname:node_1838,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:1300,x:32594,y:32830,varname:node_1300,prsc:2,v1:1;n:type:ShaderForge.SFN_FragmentPosition,id:429,x:30766,y:33215,varname:node_429,prsc:2;n:type:ShaderForge.SFN_ObjectPosition,id:7191,x:30751,y:33370,varname:node_7191,prsc:2;n:type:ShaderForge.SFN_Subtract,id:3905,x:31069,y:33247,varname:node_3905,prsc:2|A-429-XYZ,B-7191-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:2749,x:31250,y:33222,varname:node_2749,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-3905-OUT;n:type:ShaderForge.SFN_Divide,id:3359,x:31451,y:33319,varname:node_3359,prsc:2|A-2749-OUT,B-5007-OUT;n:type:ShaderForge.SFN_Vector1,id:5007,x:31227,y:33440,varname:node_5007,prsc:2,v1:5000;n:type:ShaderForge.SFN_ComponentMask,id:4670,x:31632,y:33225,varname:node_4670,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-3359-OUT;n:type:ShaderForge.SFN_ComponentMask,id:9241,x:31632,y:33423,varname:node_9241,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-3359-OUT;n:type:ShaderForge.SFN_Multiply,id:1408,x:31817,y:33348,varname:node_1408,prsc:2|A-4670-OUT,B-9241-OUT;n:type:ShaderForge.SFN_Add,id:6527,x:32083,y:33343,varname:node_6527,prsc:2|A-1408-OUT,B-849-OUT;n:type:ShaderForge.SFN_Time,id:658,x:31632,y:33680,varname:node_658,prsc:2;n:type:ShaderForge.SFN_OneMinus,id:26,x:31860,y:33664,varname:node_26,prsc:2|IN-658-T;n:type:ShaderForge.SFN_Vector1,id:5288,x:31674,y:33609,varname:node_5288,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:849,x:32037,y:33580,varname:node_849,prsc:2|A-5288-OUT,B-26-OUT;n:type:ShaderForge.SFN_Sin,id:9202,x:32293,y:33296,varname:node_9202,prsc:2|IN-6527-OUT;n:type:ShaderForge.SFN_Multiply,id:312,x:32511,y:33343,varname:node_312,prsc:2|A-9202-OUT,B-6070-OUT;n:type:ShaderForge.SFN_Vector3,id:6070,x:32328,y:33490,varname:node_6070,prsc:2,v1:0.372,v2:0.372,v3:0.372;n:type:ShaderForge.SFN_Vector3,id:5022,x:30811,y:33499,cmnt:Wind direction,varname:node_5022,prsc:2,v1:1,v2:0.5,v3:0.5;n:type:ShaderForge.SFN_VertexColor,id:5822,x:32950,y:33411,varname:node_5822,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:736,x:33137,y:33295,prsc:2,pt:False;n:type:ShaderForge.SFN_Time,id:1190,x:33137,y:33650,varname:node_1190,prsc:2;n:type:ShaderForge.SFN_Sin,id:7730,x:33543,y:33612,varname:node_7730,prsc:2|IN-1452-OUT;n:type:ShaderForge.SFN_Multiply,id:400,x:33793,y:33484,cmnt:Wind animation,varname:node_400,prsc:2|A-2989-OUT,B-5822-R,C-7730-OUT,D-9159-OUT;n:type:ShaderForge.SFN_Vector1,id:847,x:33543,y:33763,varname:node_847,prsc:2,v1:0.016;n:type:ShaderForge.SFN_Add,id:1452,x:33362,y:33612,varname:node_1452,prsc:2|A-8756-OUT,B-1190-T;n:type:ShaderForge.SFN_Multiply,id:8756,x:33137,y:33521,varname:node_8756,prsc:2|A-5822-B,B-3992-OUT;n:type:ShaderForge.SFN_Pi,id:3992,x:32983,y:33558,varname:node_3992,prsc:2;n:type:ShaderForge.SFN_Add,id:1800,x:33358,y:33235,varname:node_1800,prsc:2|A-3751-XYZ,B-736-OUT;n:type:ShaderForge.SFN_Normalize,id:2989,x:33543,y:33344,varname:node_2989,prsc:2|IN-1800-OUT;n:type:ShaderForge.SFN_Vector3,id:2710,x:33228,y:33081,cmnt:Wind direction,varname:node_2710,prsc:2,v1:1,v2:0.5,v3:0.5;n:type:ShaderForge.SFN_ValueProperty,id:9159,x:33560,y:33841,ptovrint:False,ptlb:WaveStrenght,ptin:_WaveStrenght,varname:node_9159,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ObjectPosition,id:3751,x:32828,y:33293,varname:node_3751,prsc:2;proporder:7736-9913-616-5341-2682-8869-9159;pass:END;sub:END;*/

Shader "Stylized/Tree_Leaf_inner" {
    Properties {
        _MainTex ("Diffuse", 2D) = "white" {}
        _DiffuseTile ("DiffuseTile", Float ) = 2
        _FresnelPower ("FresnelPower", Float ) = 1
        _Color1 ("Color1", Color) = (0.005831636,0.1691176,0,1)
        _Color2 ("Color2", Color) = (0.4919371,0.8014706,0,1)
        _EmissivePower ("EmissivePower", Float ) = 0.2
        _WaveStrenght ("WaveStrenght", Float ) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _DiffuseTile;
            uniform float _FresnelPower;
            uniform float4 _Color1;
            uniform float4 _Color2;
            uniform float _EmissivePower;
            uniform float _WaveStrenght;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD10;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_1190 = _Time + _TimeEditor;
                v.vertex.xyz += (normalize((objPos.rgb+v.normal))*o.vertexColor.r*sin(((o.vertexColor.b*3.141592654)+node_1190.g))*_WaveStrenght);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = 1.0 - 1.0; // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                d.boxMax[0] = unity_SpecCube0_BoxMax;
                d.boxMin[0] = unity_SpecCube0_BoxMin;
                d.probePosition[0] = unity_SpecCube0_ProbePosition;
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.boxMax[1] = unity_SpecCube1_BoxMax;
                d.boxMin[1] = unity_SpecCube1_BoxMin;
                d.probePosition[1] = unity_SpecCube1_ProbePosition;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float2 node_5055 = (i.uv0*_DiffuseTile);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_5055, _MainTex));
                float3 node_1306 = lerp(_Color1.rgb,_Color2.rgb,(pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelPower)*(1.0 - _MainTex_var.r)));
                float3 diffuseColor = node_1306; // Need this for specular when using metallic
                float specularMonochrome;
                float3 specularColor;
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, 0.0, specularColor, specularMonochrome );
                specularMonochrome = 1-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
                float specularPBL = max(0, (NdotL*visTerm*normTerm) * (UNITY_PI / 4) );
                float3 directSpecular = 1 * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 directDiffuse = ((1 +(fd90 - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = (node_1306*_EmissivePower);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _DiffuseTile;
            uniform float _FresnelPower;
            uniform float4 _Color1;
            uniform float4 _Color2;
            uniform float _EmissivePower;
            uniform float _WaveStrenght;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_1190 = _Time + _TimeEditor;
                v.vertex.xyz += (normalize((objPos.rgb+v.normal))*o.vertexColor.r*sin(((o.vertexColor.b*3.141592654)+node_1190.g))*_WaveStrenght);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = 1.0 - 1.0; // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float2 node_5055 = (i.uv0*_DiffuseTile);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_5055, _MainTex));
                float3 node_1306 = lerp(_Color1.rgb,_Color2.rgb,(pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelPower)*(1.0 - _MainTex_var.r)));
                float3 diffuseColor = node_1306; // Need this for specular when using metallic
                float specularMonochrome;
                float3 specularColor;
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, 0.0, specularColor, specularMonochrome );
                specularMonochrome = 1-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
                float specularPBL = max(0, (NdotL*visTerm*normTerm) * (UNITY_PI / 4) );
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 directDiffuse = ((1 +(fd90 - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _WaveStrenght;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_1190 = _Time + _TimeEditor;
                v.vertex.xyz += (normalize((objPos.rgb+v.normal))*o.vertexColor.r*sin(((o.vertexColor.b*3.141592654)+node_1190.g))*_WaveStrenght);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _DiffuseTile;
            uniform float _FresnelPower;
            uniform float4 _Color1;
            uniform float4 _Color2;
            uniform float _EmissivePower;
            uniform float _WaveStrenght;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_1190 = _Time + _TimeEditor;
                v.vertex.xyz += (normalize((objPos.rgb+v.normal))*o.vertexColor.r*sin(((o.vertexColor.b*3.141592654)+node_1190.g))*_WaveStrenght);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float2 node_5055 = (i.uv0*_DiffuseTile);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_5055, _MainTex));
                float3 node_1306 = lerp(_Color1.rgb,_Color2.rgb,(pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelPower)*(1.0 - _MainTex_var.r)));
                o.Emission = (node_1306*_EmissivePower);
                
                float3 diffColor = node_1306;
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, 0.0, specColor, specularMonochrome );
                float roughness = 1.0;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
