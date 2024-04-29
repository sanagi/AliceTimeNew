Shader "Universal Render Pipeline/URPLLToonBGTransparent"
{
    Properties
    {
        [Header(MainTex)]
        [Space(5)]
        [HideInInspector ] _BaseMap ("Texture", 2D) = "white" {}
        _BaseColor("Color", Color) = (1,1,1,1)
        _WorldLightInfluence ("World Light Influence", range(0.0, 1.0)) = 1.0
        _GIInfluence ("GI Influence", range(0.0, 3.0)) = 1.0
        [HideInInspector ]_LightMapInfluence ("LightMap Influence", range(0.0, 30.0)) = 1.0
        [HideInInspector ]_MaskMap ("Mask Texture", 2D) = "white" { } //r.Rim g.specularMap r.emission a.secondMaterialMap
        [HideInInspector ]_CharaShadowMaskMap ("CharaShadowMask Texture", 2D) = "white" { }
        [HideInInspector ]_MatCap ("MatCap Texture", 2D) = "white" { } //MatCapで反射作る場合
        [HideInInspector ]_MatCapIntensity("MatCapIntensity", Range(0.0, 2.0)) = 0.5
        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}
        [Space(30)]
        
        
        [Header(BRDF)]
        [Space(5)]
        _Metallic("Metalic", Range(0.0, 1.0)) = 0.5
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        // SRP batching compatibility for Clear Coat (Not used in Lit)
        [HideInInspector]_BumpScale("Scale", Float) = 1.0
        [HideInInspector] _ClearCoatMask("_ClearCoatMask", Float) = 0.0
        [HideInInspector] _ClearCoatSmoothness("_ClearCoatSmoothness", Float) = 0.0
        [Toggle(ENABLE_MIRROR)] _EnableMirror("Enable Mirror", Float) = 0.0
        _ReflectIntensity("Reflect Intensity", Range(0.0, 2.0)) = 0.5
        _BlurIntensity("Blur Intensity", Range(0.0, 0.01)) = 0.005
        
        // Blending state
        _Surface("__surface", Float) = 1.0
        _Blend("__blend", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        _Cull("__cull", Float) = 2.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        
        [Space(30)]

        [Header(Bloom)]
        [Space(5)]
        _BloomFactor ("Common Bloom Factor", range(0.0, 1.0)) = 1.0
        
        [Header(Emission)]
        [Toggle]_EnableEmission ("Enable Emission", Float) = 0
        _Emission ("Emission", range(0.0, 20.0)) = 1.0
        [HDR]_EmissionColor ("Emission Color", color) = (0, 0, 0, 0)
        _EmissionBloomFactor ("Emission Bloom Factor", range(0.0, 10.0)) = 1.0
        _DarkEmissionIntensity ("Dark Emission Intensity", range(0.0, 10.0)) = 1.0
        [HideInInspector]_EmissionMapChannelMask ("_EmissionMapChannelMask", Vector) = (1, 1, 1, 0)
        [HideInInspector] [Toggle(ENABLE_INVERSE_SHADOW)]_EnableDarkInverseShadow ("Enable Inverse Dark Shadow", float) = 1
        [Space(30)]

        [Header(Shadow Setting)]
        [Space(5)]
        [HideInInspector ]_ShadowMultColor ("Shadow Color", color) = (1.0, 1.0, 1.0, 1.0)
        [HideInInspector]_SceondMaterialShadowColor("SecondMaterialShadowColor", Color) = (1,1,1,1)
        [HideInInspector]_SceondMaterialDarkShadowColor("SecondMaterialDarkShadowColor", Color) = (1,1,1,1)
        [HideInInspector ]_ShadowArea ("Shadow Area", range(0.0, 1.0)) = 0.5
        [HideInInspector ]_ShadowSmooth ("Shadow Smooth", range(0.0, 1.0)) = 0.05
        [HideInInspector ]_DarkShadowMultColor ("Dark Shadow Color", color) = (0.5, 0.5, 0.5, 1)
        [HideInInspector ]_DarkShadowArea ("Dark Shadow Area", range(0.0, 1.0)) = 0.5
        [HideInInspector ]_DarkShadowSmooth ("Shadow Smooth", range(0.0, 1.0)) = 0.05
        [HideInInspector ][Toggle]_FixDarkShadow ("Fix Dark Shadow", float) = 1
        [HideInInspector ][Toggle]_EnableDarkShadow ("Enable Dark Shadow", float) = 1
        
        //SRPの為に宣言
        [HideInInspector] [Toggle]_IgnoreLightY ("Ignore Light y", float) = 0
        [HideInInspector] _FixLightY ("Fix Light y", range(-10.0, 10.0)) = 0.0
        [HideInInspector] _FixDivideShadow ("Fix Divide", range(0, 1)) = 0.5
        
        [Space(5)]
        [HideInInspector ][ToggleUI] _CastShadows("Cast Shadows", Float) = 1.0
        [Toggle] _ReceiveShadows("Receive Shadow", int) = 1
        
        /*
        [Toggle(ENABLE_FACE_SHADOW_MAP)]_EnableFaceShadowMap ("Enable Face Shadow Map", float) = 0
        _FaceShadowMap ("Face Shadow Map", 2D) = "white" { }
        _FaceShadowMapPow ("Face Shadow Map Pow", range(0.001, 1.0)) = 0.2
        _FaceShadowOffset ("Face Shadow Offset", range(-1.0, 1.0)) = 0.0
        */
        
        /*[Header(Shadow Ramp)]
        [Space(5)]
        [Toggle(ENABLE_RAMP_SHADOW)] _EnableRampShadow ("Enable Ramp Shadow", float) = 1
        _RampMap ("Shadow Ramp Texture", 2D) = "white" { }
        [Header(Ramp Area LightMapAlpha RampLine)]
        _RampArea12 ("Ramp Area 1/2", Vector) = (-50, 1, -50, 4)
        _RampArea34 ("Ramp Area 3/4", Vector) = (-50, 0, -50, 2)
        _RampArea5 ("Ramp Area 5", Vector) = (-50, 3, -50, 0)
        _RampShadowRange ("Ramp Shadow Range", range(0.0, 1.0)) = 0.8
        [Space(30)]
        */
        /*[Header(Shadow Ramp Origin)]
        [Space(5)]
        [Toggle(ENABLE_RAMP_SHADOW_ORIGIN)] _EnableRampShadowOrigin ("Enable Ramp Shadow 3rd", float) = 1
        [Header(Ramp Color)]
        _HighColor("Color2", Color) = (1,1,1,1)
        _MedColor("Color3", Color) = (1,1,1,1)
        _LowColor("Color4", Color) = (1,1,1,1)
        [Space(30)]
        */
        [Header(Specular Setting)]
        [Space(5)]
        [Toggle] _EnableSpecular ("Enable Specular", float) = 0
        [HDR]_LightSpecColor ("Specular Color", color) = (0.8, 0.8, 0.8, 1)
        
        [HideInInspector][Toggle(ENABLE_FACE_CHEEK)] _EnableFaceCheek ("Enable FaceCheek", float) = 0
        [HideInInspector][Toggle(ENABLE_MATCAP_SPECULAR)] _EnableMatCapSpecular ("Enable MatCap Specular", float) = 0
        [HideInInspector][Toggle(ENABLE_HAIR_SPECULAR)] _EnableHairSpecular ("Enable Hair Specular", float) = 0
        [HideInInspector]_Sharpness("Sharpness", float) = 30
        _DiffuseIntensity("Diffuse Intensity", Range(0.0, 10.0)) = 1.0
        _SpecularIntensity("Specular Intensity", Range(0.0, 10.0)) = 0.5
        _SpecularIntensityHigh("Specular IntensityHigh", Range(0.0, 20.0)) = 0.5
        [HideInInspector]_SpecularIntensityShadow("Intensity", Range(0.0, 1.0)) = 0.5
        [HideInInspector][HDR]_LightSpecShadowColor ("ShadowHilight Color", color) = (0.8, 0.8, 0.8, 1)
        /*_JitterMap("JitterMap", 2D) = "black" {}
        _JitterIntensity("Jitter Intensity", Range(0.0, 1.0)) = 0.5
        _SharpnessHigh("SharpnessHigh", float) = 30
        _SpecularIntensityHigh("IntensityHigh", Range(0.0, 1.0)) = 0.5
        */
        //[Space(30)]
        //[Toggle(ENABLE_METAL_SPECULAR)] _EnableMetalSpecular ("Enable Metal Specular", float) = 1
        //_MetalMap ("Metal Map", 2D) = "white" { }

        [Header(RimLight Setting)]
        [Space(5)]
        [HideInInspector ][Toggle]_EnableLambert ("Enable Lambert", float) = 1
        [HideInInspector ][Toggle]_EnableRim ("Enable Rim", float) = 1
        [HideInInspector ][HDR]_RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        [HideInInspector ][HideInInspector][HDR]_EdgeRimColor ("Rim Color", Color) = (1, 1, 1, 1)
        [HideInInspector ][HideInInspector]_EdgeRimWidth ("Edge Rim Width", Range(0.001, 1.0)) = 0.001
        [HideInInspector ]_RimSmooth ("Rim Smooth", Range(0.001, 10.0)) = 10
        [HideInInspector ]_RimPow ("Rim Pow", Range(0.0, 10.0)) = 1.2
        [HideInInspector ][Toggle]_EnableRimDS ("Enable Dark Side Rim", int) = 1
        [HideInInspector ][HDR]_DarkSideRimColor ("DarkSide Rim Color", Color) = (1, 1, 1, 1)
        [HideInInspector ]_DarkSideRimSmooth ("DarkSide Rim Smooth", Range(0.001, 10.0)) = 10
        [HideInInspector ]_DarkSideRimPow ("DarkSide Rim Pow", Range(0.0, 10.0)) = 1.0
        [HideInInspector][Toggle(ENABLE_EDGE_RIM)] _EnableEdgeRim ("Enable EdgeRim", float) = 0
        /*[Space(5)]
        [Toggle]_EnableRimDS ("Enable Dark Side Rim", int) = 1
        [HDR]_DarkSideRimColor ("DarkSide Rim Color", Color) = (1, 1, 1, 1)
        _DarkSideRimSmooth ("DarkSide Rim Smooth", Range(0.001, 10.0)) = 10
        _DarkSideRimPow ("DarkSide Rim Pow", Range(0.0, 10.0)) = 1.0
        [HideInInspector][Toggle]_EnableRimOther ("Enable Other Rim", int) = 0
        [HideInInspector][HDR]_OtherRimColor ("Other Rim Color", Color) = (1, 1, 1, 1)
        [HideInInspector]_OtherRimSmooth ("Other Rim Smooth", Range(0.001, 1.0)) = 0.01
        [HideInInspector]_OtherRimPow ("Other Rim Pow", Range(0.001, 50.0)) = 10.0
        */
        [Space(30)]

        [Header(Outline Setting)]
        [Space(5)]
        [HideInInspector ]_OutlineMask("Outline Mask", 2D) = "white" {}
        [HideInInspector ]_OutlineWidth ("_OutlineWidth (World Space)", Range(0, 50)) = 1
        [HideInInspector ]_OutlineLightAffects("Outline Light Affects", Range(0.0, 1.0)) = 1.0
        [HideInInspector ]_OutlineSaturation("Outline Saturation", Range(0.0, 4.0)) = 3.0
        [HideInInspector ]_OutlineBrightness("Outline Brightness", Range(0.0, 1.0)) = 0.25
        [HideInInspector ]_OutlineStrength("Outline Strength", Range(0.0, 1.0)) = 0.5
        [HideInInspector ]_OutlineSmoothness("Outline Smoothness", Range(0.0, 1.0)) = 1.0
        //[HideInInspector]_OutlineZOffset ("_OutlineZOffset (View Space) (increase it if is face!)", Range(0, 1)) = 0.0001

        [Header(Alpha)]
        [HideInInspector ][Toggle(ENABLE_ALPHA_CLIPPING)]_AlphaClip ("_AlphaClip", Float) = 0
        _Cutoff ("_Cutoff (Alpha Cutoff)", Range(0.0, 1.0)) = 0.5        
    
        //[HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        //[HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" }
        /*Pass
        {
            // Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
            // no LightMode tag are also rendered by Universal Render Pipeline
            Name "GBuffer"
            Tags{"LightMode" = "UniversalGBuffer"}

            ZWrite[_ZWrite]
            ZTest LEqual
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            //#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local LIGHTMAP_ON

            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local_fragment _SPECULAR_SETUP
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            //#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            //#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _LIGHT_LAYERS
            #pragma multi_compile_fragment _ _RENDER_PASS_ENABLED

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma vertex LitGBufferPassVertex
            #pragma fragment LitGBufferPassFragment
            
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitGBufferPass.hlsl"
            ENDHLSL
        }        
        */
        //デプスに書く
        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0

            HLSLPROGRAM
            #include "LLToonInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            ENDHLSL
        }
        
        //メインのライティング
        Pass
        {
            NAME "BG_BASE_TRANSPARENT"
            
            Tags {"RenderType" = "Transparent" "LightMode" = "UniversalForward" }

            ZWrite[_ZWrite]
            Cull[_Cull]
            Blend[_SrcBlend][_DstBlend]
            
            HLSLPROGRAM
            #pragma only_renderers gles gles3 glcore d3d11
            #pragma target 2.0            
            
            #include "LLToonForwardLighting.hlsl"

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fog

            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local_fragment ENABLE_ALPHA_CLIPPING
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local_fragment ENABLE_BLOOM_MASK
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment ENABLE_MIRROR
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT

            
            #pragma vertex VertexBase
            #pragma fragment LLFragmentBG
            // make fog work
            #pragma multi_compile_fog
            ENDHLSL
        }
    }
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.LLToonShader"
}
