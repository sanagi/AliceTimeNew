Shader "Universal Render Pipeline/URPLLToonBGWater"
{
    Properties
    {
        [Space(5)]
        [Header(Base Color)]
        [HDR]_ShallowColor ("Shallow", Color) = (0.44, 0.95, 0.36, 1.0)
        [HDR]_DeepColor ("Deep", Color) =  (0.0, 0.05, 0.19, 1.0)
        [HDR]_FarColor ("Far", Color) = (0.04, 0.27, 0.75, 1.0)
        _WorldLightInfluence ("World Light Influence", range(0.0, 1.0)) = 1.0
        _GIInfluence ("GI Influence", range(0.0, 3.0)) = 1.0
        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}
        [Space(30)]
        
        
        [Header(BRDF)]
        [Space(5)]
        _Metallic("Metalic", Range(0.0, 1.0)) = 0.5
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        // SRP batching compatibility for Clear Coat (Not used in Lit)
        [Toggle(ENABLE_MIRROR)] _EnableMirror("Enable Mirror", Float) = 0.0
        _ReflectIntensity("Reflect Intensity", Range(0.0, 2.0)) = 0.5

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
        
        [Space(5)]
        [ToggleUI] _CastShadows("Cast Shadows", Float) = 1.0
        [Toggle] _ReceiveShadows("Receive Shadow", float) = 1.0
        
        //_SpeedX ("SpeedX", range(0.0, 1.0)) = 1.0
        //_SpeedY ("SpeedY", range(0.0, 1.0)) = 1.0
        //_Time ("Time", range(0.0, 1.0)) = 1.0
        _F0 ("F0", range(0.0, 1.0)) = 0.02
        
        _WaterNormalMap ("Normal Map", 2D) = "bump"{}
        _WaveNormalScale ("Scale", float) = 10.0
        _WaveNormalSpeed ("Speed", float) = 1.0
        
        _Wave1Direction ("Direction", Range(0, 1)) = 0
        _Wave1Amplitude ("Amplitude", float) = 1
        _Wave1Wavelength ("Wavelength", float) = 1
        _Wave1Speed ("Speed", float) = 1
        
        _Wave2Direction ("Direction", Range(0, 1)) = 0
        _Wave2Amplitude ("Amplitude", float) = 1
        _Wave2Wavelength ("Wavelength", float) = 1
        _Wave2Speed ("Speed", float) = 1     
        
        _DepthDensity ("Depth", Range(0.0, 1.0)) = 0.5
        _DistanceDensity ("Distance", Range(0.0, 1.0)) = 0.1        
        
        _ParallaxScale("Parallax Scale", Float) = 1
		_NormalScaleFactor("Normal Scale Factor", Float) = 1
        _DistanceDensity ("DistanceDensity", range(0.0, 1.0)) = 0.02
        _Distortion("Distortion", Float) = 0.2
        [HDR]_SSSColor("SSSColor", Color) = (1,1,1,1)
        
        _FoamScale ("FoamScale", float) = 1.0
        _FoamNoiseScale ("FoamNoiseScale", range(0.0, 1.0)) = 0.5
        _FoamSpeed ("FoamSpeed", float) = 1.0
        _FoamIntensity ("FoamIntensity", range(0.0, 1.0)) = 1.0
        [HideInInspector ] _FoamTexture ("FoamTexture", 2D) = "black" {}  
        
        [HDR]_SunSpecularColor ("Color", Color) = (1, 1, 1, 1)
        _SunSpecularExponent ("Exponent", float) = 1000        
        
        _SparklesNormalMap ("Normal Map", 2D) = "bump"{}
        _SparkleScale ("Scale", float) = 10
        _SparkleSpeed ("Speed", float) = 0.75
        [HDR]_SparkleColor ("Color", Color) = (1, 1, 1, 1)
        _SparkleExponent ("Exponent", float) = 10000
        
        [HDR]_EdgeFoamColor ("Color", Color) = (1, 1, 1, 1)
        _EdgeFoamDepth ("Scale", float) = 10.0
        
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
            NAME "BG_BASE_Water"
            
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
            #pragma shader_feature_local_fragment ENABLE_WATER
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT

            
            #pragma vertex VertexWater
            #pragma fragment LLFragmentBGWater
            // make fog work
            #pragma multi_compile_fog
            ENDHLSL
        }
    }
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.LLToonShaderWater"
}
