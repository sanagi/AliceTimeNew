﻿#ifndef UNIVERSAL_SIMPLE_LIT_PASS_INCLUDED
#define UNIVERSAL_SIMPLE_LIT_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "LLToonLighting.hlsl"
#include "LLToonOutline.hlsl"

struct Attributes
{
    half4 color: COLOR0;
    float4 positionOS    : POSITION;
    float3 normalOS      : NORMAL;
    float4 tangentOS     : TANGENT;
    float2 texcoord      : TEXCOORD0;
    float2 staticLightmapUV    : TEXCOORD1;
    float2 dynamicLightmapUV    : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 color: COLOR0;
    float2 uv                       : TEXCOORD0;

    float3 positionWS                  : TEXCOORD1;    // xyz: posWS

    #ifdef _NORMALMAP
        half4 normalWS                 : TEXCOORD2;    // xyz: normal, w: viewDir.x
        half4 tangentWS                : TEXCOORD3;    // xyz: tangent, w: viewDir.y
        half4 bitangentWS              : TEXCOORD4;    // xyz: bitangent, w: viewDir.z
    #else
        half3  normalWS                : TEXCOORD2;
    #endif

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        half4 fogFactorAndVertexLight  : TEXCOORD5; // x: fogFactor, yzw: vertex light
    #else
        half  fogFactor                 : TEXCOORD5;
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        float4 shadowCoord             : TEXCOORD6;
    #endif

    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 7);

    float4 screenPos                   : TEXCOORD8;

    float3 binormal: TEXCOORD9;

    float3 normalOS: TEXCOOR10;
    
#ifdef DYNAMICLIGHTMAP_ON
    float2  dynamicLightmapUV : TEXCOORD8; // Dynamic lightmap UVs
#endif

#ifdef ENABLE_WATER    
    float4 refData : TEXCOORD11;
#endif      
    float3 positionVS : TEXCOORD12;
    float4 positionCS                  : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

/**
 *　頭上の平面と光源からのベクトルの交点
 * \brief 
 * \param posA 
 * \param posB 
 * \param posC 
 * \param lightPos 
 * \param posW 
 * \return 
 */
float3 ComputeLightVectorInterSection(float3 posA, float3 posB, float3 posC, float3 lightPos, float3 posW) {
    // 平面の法線ベクトルを計算
    float3 normal = normalize(cross(posB-posA, posC-posA));

    // 法線ベクトルと光源ベクトルの内積
    float t = dot(float3(0,1,0), posW - posA);
    
    float3 intersection = posW + t * normalize(lightPos - posW); // 交点を計算

    return intersection;
}

// 点pから直方体の各面までの距離を計算する関数
float2 calc_distance(float pos, float center, float half_size) {
    float2 result;
    float dist_to_face = abs(pos - center) - half_size;
    result.x = max(dist_to_face, 0.0);
    result.y = step(0.0, dist_to_face);
    return result;
}

/**直方体と頂点の交点
 * \brief 
 * \param center 
 * \param size 
 * \param p_pos 
 * \return 
 */
float3 ComputeLightVectorInterSectionBox(float3 center, float3 size, float3 p_pos)
{
    // 点pから直方体の各面までの距離を計算する
    float2 x_distance = calc_distance(p_pos.x, center.x, size.x / 2.0);
    float2 y_distance = calc_distance(p_pos.y, center.y, size.y / 2.0);
    float2 z_distance = calc_distance(p_pos.z, center.z, size.z / 2.0);

    // 一番近い距離の面のインデックスを求める
    float2 min_dist_face = min(min(x_distance, y_distance), z_distance);
    int min_index = int(min_dist_face.y * 2.0 + min_dist_face.x);

    // 一番近い面と点pとの交点を求める
    float3 face_sizes[6] = {
        float3(size.x, 0.0, 0.0),  // left
        float3(size.x, 0.0, 0.0),  // right
        float3(0.0, size.y, 0.0),  // bottom
        float3(0.0, size.y, 0.0),  // top
        float3(0.0, 0.0, size.z),  // near
        float3(0.0, 0.0, size.z)   // far
    };
    
    float3 face_centers[6] = {
        float3(center.x - size.x / 2.0, center.y, center.z),  // left
        float3(center.x + size.x / 2.0, center.y, center.z),  // right
        float3(center.x, center.y - size.y / 2.0, center.z),  // bottom
        float3(center.x, center.y + size.y / 2.0, center.z),  // top
        float3(center.x, center.y, center.z - size.z / 2.0),  // near
        float3(center.x, center.y, center.z + size.z / 2.0)   // far
    };
    return p_pos + face_sizes[min_index] * (min_dist_face.x / length(face_sizes[min_index])) - face_centers[min_index];
}

void InitializeLLToonInputData(Varyings input, half3 normalTS, out LLToonInputData inputData)
{
    inputData = (LLToonInputData)0;

    inputData.baseInputData.positionWS = input.positionWS;
    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(inputData.baseInputData.positionWS);
    
    #ifdef _NORMALMAP
        float sgn = input.tangentWS.w;      // should be either +1 or -1
        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
        inputData.baseInputData.tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
        inputData.baseInputData.normalWS = TransformTangentToWorld(normalTS, inputData.baseInputData.tangentToWorld);
    #else
        
        inputData.baseInputData.normalWS = input.normalWS;
    #endif

    inputData.baseInputData.normalWS = NormalizeNormalPerPixel(inputData.baseInputData.normalWS);
    viewDirWS = SafeNormalize(viewDirWS);

    inputData.baseInputData.viewDirectionWS = viewDirWS;
    
    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        inputData.baseInputData.shadowCoord = input.shadowCoord;
    #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
        inputData.baseInputData.shadowCoord = TransformWorldToShadowCoord(inputData.baseInputData.positionWS);
    #else
        inputData.baseInputData.shadowCoord = float4(0, 0, 0, 0);
    #endif

#ifdef _ADDITIONAL_LIGHTS_VERTEX
    inputData.baseInputData.fogCoord = InitializeInputDataFog(float4(inputData.baseInputData.positionWS, 1.0), input.fogFactorAndVertexLight.x);
    inputData.baseInputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
#else
    inputData.baseInputData.fogCoord = InitializeInputDataFog(float4(inputData.baseInputData.positionWS, 1.0), input.fogFactor);
    inputData.baseInputData.vertexLighting = half3(0, 0, 0);
#endif

#if defined(DYNAMICLIGHTMAP_ON)
    inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.dynamicLightmapUV, input.vertexSH, inputData.normalWS);
#else
    inputData.baseInputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.vertexSH, inputData.baseInputData.normalWS) * _LightMapInfluence;
#endif

    inputData.baseInputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
    inputData.baseInputData.shadowMask = SAMPLE_SHADOWMASK(input.staticLightmapUV);

    #if defined(DEBUG_DISPLAY)
    #if defined(DYNAMICLIGHTMAP_ON)
    inputData.dynamicLightmapUV = input.dynamicLightmapUV.xy;
    #endif
    #if defined(LIGHTMAP_ON)
    inputData.staticLightmapUV = input.staticLightmapUV;
    #else
    inputData.vertexSH = input.vertexSH;
    #endif
    #endif

    inputData.color = input.color;
    inputData.binormal = input.binormal;
    inputData.normalOS = input.normalOS;

    // カメラ座標系の法線を取得
    float3 normal = mul((float3x3)UNITY_MATRIX_V, input.normalWS);
    // 法線のxyを0～1に変換する
    inputData.matcapUV = normal.xy * 0.5 + 0.5;
}


// Pans the input uv in the given direction and speed.
float2 Panner(float2 uv, float2 direction, float speed)
{
    return uv + normalize(direction)*speed*_Time.y;
}

// Returns a simple waveform (to be used as a height offset) from the input wave parameters.
float SimpleWave(float2 position, float2 direction, float wavelength, float amplitude, float speed)
{
    //tex2Dlod(_WaveTex, float4(uv.xy,0,0)).g * (_Wave1Amplitude + _Wave2Amplitude) / 2.0
    float x = PI * dot(position, direction) / wavelength;
    float phase = speed * _Time.y;
    return amplitude * sin(x + phase);
}

// Tiles a texture in an interesting wave-like way.
// Inspired by the function of the same name in Unreal Engine.
float3 MotionFourWayChaos(sampler2D tex, float2 uv, float speed, bool unpackNormal)
{
    float2 uv1 = Panner(uv + float2(0.000, 0.000), float2( 0.1,  0.1), speed);
    float2 uv2 = Panner(uv + float2(0.418, 0.355), float2(-0.1, -0.1), speed);
    float2 uv3 = Panner(uv + float2(0.865, 0.148), float2(-0.1,  0.1), speed);
    float2 uv4 = Panner(uv + float2(0.651, 0.752), float2( 0.1, -0.1), speed);

    float3 sample1;
    float3 sample2;
    float3 sample3;
    float3 sample4;

    if (unpackNormal)
    {
        sample1 = UnpackNormal(tex2D(tex, uv1)).rgb;
        sample2 = UnpackNormal(tex2D(tex, uv2)).rgb;
        sample3 = UnpackNormal(tex2D(tex, uv3)).rgb;
        sample4 = UnpackNormal(tex2D(tex, uv4)).rgb;

        return normalize(sample1 + sample2 + sample3 + sample4);
    }
    else
    {
        sample1 = tex2D(tex, uv1).rgb;
        sample2 = tex2D(tex, uv2).rgb;
        sample3 = tex2D(tex, uv3).rgb;
        sample4 = tex2D(tex, uv4).rgb;

        return (sample1 + sample2 + sample3 + sample4) / 4.0;
    }
}

float3 MotionFourWaySparkle(sampler2D tex, float2 uv, float4 coordinateScale, float speed)
{
    float2 uv1 = Panner(uv * coordinateScale.x, float2( 0.1,  0.1), speed);
    float2 uv2 = Panner(uv * coordinateScale.y, float2(-0.1, -0.1), speed);
    float2 uv3 = Panner(uv * coordinateScale.z, float2(-0.1,  0.1), speed);
    float2 uv4 = Panner(uv * coordinateScale.w, float2( 0.1, -0.1), speed);

    float3 sample1 = UnpackNormal(tex2D(tex, uv1)).rgb;
    float3 sample2 = UnpackNormal(tex2D(tex, uv2)).rgb;
    float3 sample3 = UnpackNormal(tex2D(tex, uv3)).rgb;
    float3 sample4 = UnpackNormal(tex2D(tex, uv4)).rgb;

    float3 normalA = float3(sample1.x, sample2.y, 1);
    float3 normalB = float3(sample3.x, sample4.y, 1);
    
    return normalize(float3( (normalA+normalB).xy, (normalA*normalB).z ));
}

float GetWaveHeightVert(float2 worldPosition, float2 uv)
{
    float2 dir1 = float2(cos(PI * _Wave1Direction), sin(PI * _Wave1Direction));
    float2 dir2 = float2(cos(PI * _Wave2Direction), sin(PI * _Wave2Direction));
    float wave1 = SimpleWave(worldPosition, dir1, _Wave1Wavelength, _Wave1Amplitude, _Wave1Speed);
    float wave2 = SimpleWave(worldPosition, dir2, _Wave2Wavelength, _Wave2Amplitude, _Wave2Speed);
    
    return wave1 + wave2;// + tex2Dlod(_WaveTex, float4(uv.xy,0,0)).g;
}

float GetWaveHeight(float2 worldPosition, float2 uv)
{
    float2 dir1 = float2(cos(PI * _Wave1Direction), sin(PI * _Wave1Direction));
    float2 dir2 = float2(cos(PI * _Wave2Direction), sin(PI * _Wave2Direction));
    float wave1 = SimpleWave(worldPosition, dir1, _Wave1Wavelength, _Wave1Amplitude, _Wave1Speed);
    float wave2 = SimpleWave(worldPosition, dir2, _Wave2Wavelength, _Wave2Amplitude, _Wave2Speed);
    
    return wave1 + wave2;// + tex2D(_WaveTex, uv).g;
}

float3x3 GetWaveTBN(float2 worldPosition, float d, float2 uv)
{
    float waveHeight = GetWaveHeight(worldPosition, uv);
    float waveHeightDX = GetWaveHeight(worldPosition - float2(d, 0), uv);
    float waveHeightDZ = GetWaveHeight(worldPosition - float2(0, d), uv);
                
    // Calculate the partial derivatives in the Z and X directions, which
    // are the tangent and binormal vectors respectively.
    float3 tangent = normalize(float3(0, waveHeight - waveHeightDZ, d));
    float3 binormal = normalize(float3(d, waveHeight - waveHeightDX, 0));

    // Cross the results to get the normal vector, and return the TBN matrix.
    // Note that the TBN matrix is orthogonal, i.e. TBN^-1 = TBN^T.
    // We exploit this fact to speed up the inversion process.
    float3 normal = normalize(cross(binormal, tangent));
    return transpose(float3x3(tangent, binormal, normal));
}

///////////////////////////////////////////////////////////////////////////////
//                  Vertex and Fragment functions                            //
///////////////////////////////////////////////////////////////////////////////

// Used in Standard (Simple Lighting) shader
Varyings VertexBase(Attributes input)
{
    Varyings output = (Varyings)0;
    output.color = input.color;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
    
#if defined(_FOG_FRAGMENT)
        half fogFactor = 0;
#else
        half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
#endif
    
    output.positionWS.xyz = vertexInput.positionWS;
    
    output.positionVS = vertexInput.positionVS;
    output.positionCS = vertexInput.positionCS;
    output.screenPos = output.positionCS;

#ifdef _NORMALMAP
    half3 viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
    output.normalWS = half4(normalInput.normalWS, viewDirWS.x);
    output.tangentWS = half4(normalInput.tangentWS, viewDirWS.y);
    output.bitangentWS = half4(normalInput.bitangentWS, viewDirWS.z);
#else
    output.normalWS = NormalizeNormalPerVertex(normalInput.normalWS);
#endif
    
    output.binormal = normalize(cross(output.normalWS.xyz, input.tangentOS.xyz) * input.tangentOS.w * unity_WorldTransformParams.w);
    output.binormal = mul(unity_ObjectToWorld, output.binormal);
    output.normalOS = input.normalOS;    
    
    OUTPUT_LIGHTMAP_UV(input.staticLightmapUV, unity_LightmapST, output.staticLightmapUV);
#ifdef DYNAMICLIGHTMAP_ON
    output.dynamicLightmapUV = input.dynamicLightmapUV.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#endif
    OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
        output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
    #else
        output.fogFactor = fogFactor;
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        output.shadowCoord = GetShadowCoord(vertexInput);
    #endif
    
    return output;
}

Varyings VertexWater(Attributes input)
{
    Varyings output = (Varyings)0;
    output.color = input.color;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
    
#if defined(_FOG_FRAGMENT)
        half fogFactor = 0;
#else
        half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
#endif

    //頂点分割してから考える
    float waveY = vertexInput.positionWS.y + GetWaveHeightVert(vertexInput.positionWS.xz, vertexInput.positionWS.xz / _WaveNormalScale + 0.5);
    float3 positionXYZ = float3(vertexInput.positionWS.x, waveY, vertexInput.positionWS.z);
    output.positionWS.xyz = positionXYZ;
    vertexInput.positionCS = TransformWorldToHClip(positionXYZ);
    
    output.positionVS = vertexInput.positionVS;
    output.positionCS = vertexInput.positionCS;
    output.screenPos = output.positionCS;

#ifdef _NORMALMAP
    half3 viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
    output.normalWS = half4(normalInput.normalWS, viewDirWS.x);
    output.tangentWS = half4(normalInput.tangentWS, viewDirWS.y);
    output.bitangentWS = half4(normalInput.bitangentWS, viewDirWS.z);
#else
    output.normalWS = NormalizeNormalPerVertex(normalInput.normalWS);
#endif
    
    output.binormal = normalize(cross(output.normalWS.xyz, input.tangentOS.xyz) * input.tangentOS.w * unity_WorldTransformParams.w);
    output.binormal = mul(unity_ObjectToWorld, output.binormal);
    output.normalOS = input.normalOS;    
    
    OUTPUT_LIGHTMAP_UV(input.staticLightmapUV, unity_LightmapST, output.staticLightmapUV);
#ifdef DYNAMICLIGHTMAP_ON
    output.dynamicLightmapUV = input.dynamicLightmapUV.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#endif
    OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
        output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
    #else
        output.fogFactor = fogFactor;
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        output.shadowCoord = GetShadowCoord(vertexInput);
    #endif

#ifdef ENABLE_WATER
    //output.refData = mul(_RefVP, mul(_RefW, vertexInput.positionWS));
#endif    
    return output;
}

// Used Chara shader
half4 LLFragmentChara(Varyings input) : SV_Target
{
    //URP基本のInputData、SurfaceDataの取得
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
    SurfaceData surfaceData;
    InitializeStandardSurfaceDataLL(input.uv, surfaceData, input.positionCS);
    LLToonInputData inputData;
    InitializeLLToonInputData(input, surfaceData.normalTS, inputData);    
    
    SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);

#ifdef _DBUFFER
    ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
#endif

    //ライティング
    LLLightingData lllData;
    LLToonLighting(inputData, surfaceData, input.uv, true, lllData, input.screenPos);

    // 計算した要素を足し合わせる
    half4 finalColor = lllData.BaseToonLightingColor + lllData.AdditionalLightsColor * _AddLightIntensity + (lllData.RimColor + lllData.DarkRimColor) * lllData.RimColor.a + lllData.EmissionColor.a * lllData.EmissionColor + lllData.SpecRimEmission.a * lllData.SpecRimEmission + lllData.GIColor * _GIInfluence; 
    //half4 finalColor = lllData.GIColor * _GIInfluence;
    //half4 finalColor = lllData.RimColor; 
    
    // Outline作る
    float2 screenPos = ComputeScreenPos(input.screenPos / input.screenPos.w).xy;
    half width = lerp(_OutlineWidth, _OutlineWidth * 0.5, lllData.RampOutline * _OutlineLightAffects);
    width *= SAMPLE_TEXTURE2D(_OutlineMask, sampler_OutlineMask, input.uv).r;
    half outlineFactor = SoftOutline(screenPos, width, _OutlineStrength, _OutlineSmoothness);
    half lerpValue = lllData.HalfLambert > 1.0 ? lllData.HalfLambert * _OutlineLightAffects : lllData.RampOutline * _OutlineLightAffects;
    finalColor.rgb = lerp(finalColor.rgb, shift(finalColor.rgb, half3(0.0, _OutlineSaturation, lerp(_OutlineBrightness, saturate(_OutlineBrightness * 2.0), lerpValue))), outlineFactor);

    // apply fog
    finalColor.rgb = MixFog(finalColor.rgb, inputData.baseInputData.fogCoord);
    
    //color.rgb = MixFog(color.rgb, inputData.fogCoord);
    finalColor.a = OutputAlpha(finalColor.a, _Surface);

    return finalColor;
}

half4 Cal_ReflectColor(Varyings input, float2 bumpOffset)
{
        //Reflection
    float4 reflectColor = float4(0,0,0,0);
    float2 offset = float2(0,0);

#ifdef ENABLE_WAVE
    offset = bumpOffset;
#endif    
    
#ifdef ENABLE_MIRROR
        const float blur_radius = _BlurIntensity;
        float2 blur_coords[9] = {
            float2( 0.000,  0.000),
            float2( 0.1080925165271518,  -0.9546740999616308)*blur_radius,
            float2(-0.4753686437884934,  -0.8417212473681748)*blur_radius,
            float2( 0.7242715177221273,  -0.6574584801064549)*blur_radius,
            float2(-0.023355087558461607, 0.7964400038854089)*blur_radius,
            float2(-0.8308210026544296,  -0.7015103725420933)*blur_radius,
            float2( 0.3243705688309195,   0.2577797517167695)*blur_radius,
            float2( 0.31851240326305463, -0.2220789454739755)*blur_radius,
            float2(-0.36307729185097637, -0.7307245945773899)*blur_radius
        };
        float2 coord = ComputeScreenPos(input.screenPos / input.screenPos.w).xy + offset;

        reflectColor.rgb += tex2D(_CameraReflectionTexture, coord+blur_coords[0]).rgb * _EnableMirror;
        reflectColor.rgb += tex2D(_CameraReflectionTexture, coord+blur_coords[1]).rgb * _EnableMirror;
        reflectColor.rgb += tex2D(_CameraReflectionTexture, coord+blur_coords[2]).rgb * _EnableMirror;
        reflectColor.rgb += tex2D(_CameraReflectionTexture, coord+blur_coords[3]).rgb * _EnableMirror;
        reflectColor.rgb += tex2D(_CameraReflectionTexture, coord+blur_coords[4]).rgb * _EnableMirror;
        reflectColor.rgb += tex2D(_CameraReflectionTexture, coord+blur_coords[5]).rgb * _EnableMirror;
        reflectColor.rgb += tex2D(_CameraReflectionTexture, coord+blur_coords[6]).rgb * _EnableMirror;
        reflectColor.rgb += tex2D(_CameraReflectionTexture, coord+blur_coords[7]).rgb * _EnableMirror;
        reflectColor.rgb += tex2D(_CameraReflectionTexture, coord+blur_coords[8]).rgb * _EnableMirror;
    
#endif
    
    return reflectColor * _ReflectIntensity;
}

half4 Cal_ReflectWater(Varyings input, float3 normal)
{
    //Reflection
    float4 coord = ComputeScreenPos(input.screenPos / input.screenPos.w);
    //float2 xy = (float2(input.positionCS.x, input.positionCS.y * 1) + input.positionCS.w) * 0.5;
    //float2 zw = input.positionCS.zw;
    coord.xy = normal.xy * _Distortion * coord.z + coord.xy;
    float4 reflectColor = tex2D(_CameraReflectionTexture, coord.xy / coord.w ) * _EnableMirror * _ReflectIntensity;
    
    return reflectColor;
}

half4 LLFragmentBG(Varyings input) : SV_Target
{
    //URP基本のInputData、SurfaceDataの取得
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
    SurfaceData surfaceData;
    InitializeStandardSurfaceDataLL(input.uv, surfaceData, input.positionCS);
    LLToonInputData inputData;
    InitializeLLToonInputData(input, surfaceData.normalTS, inputData);
    SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);

#if REFLECT   
    if(input.positionWS.y < 0) //変数でコントロールできるように
    {
        discard;
    }
#endif    

#ifdef _DBUFFER
    ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
#endif

    //ライティング
    LLLightingData lllData;
    LLToonLighting(inputData, surfaceData, input.uv, false, lllData, input.screenPos);

    //Reflection
    //ミラー反射の強さ
    half4 reflectColor = Cal_ReflectColor(input, surfaceData.normalTS.rg);
    
    // 計算した要素を足し合わせる
    half4 finalColor = lllData.BaseToonLightingColor + lllData.AdditionalLightsColor * _AddLightIntensity + (lllData.RimColor + lllData.DarkRimColor) * lllData.RimColor.a + lllData.EmissionColor.a * lllData.EmissionColor + lllData.SpecRimEmission.a * lllData.SpecRimEmission + (lllData.GIColor * _GIInfluence) + reflectColor; 
    
#ifdef ENABLE_EMISSION_ONLY
    float4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
    finalColor = baseColor + lllData.EmissionColor.a * lllData.EmissionColor;
#endif

    /*float2 screenPos = ComputeScreenPos(input.screenPos / input.screenPos.w).xy;
    float sceneDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, my_linear_clamp_sampler, screenPos);
    half depth = Linear01Depth(sceneDepth, _ZBufferParams);
    float D = max(0,1.0 - depth - 0.5);
    finalColor.rgb = 1 / (1 + exp(-500.0 * (D-0.5)));
    */
    // apply fog
    finalColor.rgb = MixFog(finalColor.rgb, inputData.baseInputData.fogCoord);
    
    finalColor.a = OutputAlpha(finalColor.a * _BaseColor.a, _Surface);
    
    return finalColor;
}

half4 LLFragmentBGWave(Varyings input) : SV_Target
{
    //URP基本のInputData、SurfaceDataの取得
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
    SurfaceData surfaceData;
    InitializeStandardSurfaceDataLL(input.uv, surfaceData, input.positionCS);
    LLToonInputData inputData;
    InitializeLLToonInputData(input, surfaceData.normalTS, inputData);
    SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);

#if REFLECT   
    if(input.positionWS.y < 0) //変数でコントロールできるように
    {
        discard;
    }
#endif    

#ifdef _DBUFFER
    ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
#endif

    //ライティング
    LLLightingData lllData;
    LLToonLighting(inputData, surfaceData, input.uv, false, lllData, input.screenPos);

    //Reflection
    //ミラー反射の強さ
    half4 reflectColor = Cal_ReflectColor(input, surfaceData.normalTS.rg);
    
    // 計算した要素を足し合わせる
    half4 finalColor = lllData.BaseToonLightingColor + lllData.AdditionalLightsColor * _AddLightIntensity + (lllData.RimColor + lllData.DarkRimColor) * lllData.RimColor.a + lllData.EmissionColor.a * lllData.EmissionColor + lllData.SpecRimEmission.a * lllData.SpecRimEmission + (lllData.GIColor * _GIInfluence) + reflectColor; 

    // テクスチャ座標を計算
    float scaleFactor = 30.0;
    float2 uv = float2((input.uv.x - 0.5) * scaleFactor + 0.5, (input.uv.y - 0.5) * scaleFactor + 0.5);
    float3 wave = float3(tex2D(_WaveTex, uv).g, tex2D(_WaveTex, uv).g, tex2D(_WaveTex, uv).g) * 5;
    //finalColor.rgb += wave;
    //float2 scroll = float2(_SpeedX, _SpeedY) * _Time;
    //finalColor.rgb += SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uv + scroll);
    //finalColor.rgb = surfaceData.normalTS;
    

    /*float2 screenPos = ComputeScreenPos(input.screenPos / input.screenPos.w).xy;
    float sceneDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, my_linear_clamp_sampler, screenPos);
    half depth = Linear01Depth(sceneDepth, _ZBufferParams);
    float D = max(0,1.0 - depth - 0.5);
    finalColor.rgb = 1 / (1 + exp(-500.0 * (D-0.5)));
    */
    // apply fog
    finalColor.rgb = MixFog(finalColor.rgb, inputData.baseInputData.fogCoord);
    
    finalColor.a = OutputAlpha(finalColor.a * _BaseColor.a, _Surface);
    
    return finalColor;
}

float Cal_Fresnel(float vdotn)
{
    half fresnel = _F0 + (1.0h - _F0) * pow(1.0h - vdotn, 5);
    return fresnel;
}

half4 LLFragmentBGWater(Varyings input) : SV_Target
{
    //URP基本のInputData、SurfaceDataの取得
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
    SurfaceData surfaceData;
    InitializeStandardSurfaceDataLL(input.uv, surfaceData, input.positionCS);
    
    LLToonInputData inputData;
    InitializeLLToonInputData(input, surfaceData.normalTS, inputData);
    SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);
    
    //surfaceData.normalTS.xyz += SAMPLE_TEXTURE2D(_WaveTex, sampler_WaveTex, input.uv);
    
    #ifdef _DBUFFER
    ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
    #endif

    //BRDFデータを計算しておく
    BRDFData brdfData;
    InitializeBRDFData(surfaceData, brdfData);
    // Clear-coat計算
    BRDFData brdfDataClearCoat = CreateClearCoatBRDFData(surfaceData, brdfData);

    //メインライトの情報取得
    half4 shadowMask = CalculateShadowMask(inputData.baseInputData);
    AmbientOcclusionFactor aoFactor = CreateAmbientOcclusionFactor(inputData.baseInputData, surfaceData);
    Light mainLight = GetMainLight(inputData.baseInputData, shadowMask, aoFactor);

    //GI計算
    half4 giColor = half4(0,0,0,0);
    MixRealtimeAndBakedGI(mainLight, inputData.baseInputData.normalWS, inputData.baseInputData.bakedGI);
    giColor.rgb = GlobalIllumination(brdfData, brdfDataClearCoat, surfaceData.clearCoatMask,
                                              inputData.baseInputData.bakedGI, aoFactor.indirectAmbientOcclusion, inputData.baseInputData.positionWS,
                                              inputData.baseInputData.normalWS, inputData.baseInputData.viewDirectionWS);
    giColor.a = 0;
    
    //GIにsurfaceData側のノーマルを使ったら後は自前のノーマル計算再取得(重い場合は↑の計算を削っておく)
    float2 uv = input.positionWS.xz / _WaveNormalScale + 0.5;
    float3x3 tangentToWorld = GetWaveTBN(input.positionWS.xz, 0.01, input.uv);

    // Sample the wave normal map and calculate the world-space normal for this fragment.
    float3 normalTS = MotionFourWayChaos(_WaterNormalMap, input.positionWS.xz/_WaveNormalScale, _WaveNormalSpeed, true);
    //float2 uv = input.positionWS.xz / _WaveNormalScale + 0.5;
    //normalTS += tex2D(_WaveTex, uv).rgb;
    float3 normalWS = mul(tangentToWorld, normalTS);

    float2 screenPos = ComputeScreenPos(input.screenPos / input.screenPos.w).xy;
    float sceneDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, my_linear_clamp_sampler, screenPos);
    float opticalDepth = abs(LinearEyeDepth(sceneDepth, _ZBufferParams) - LinearEyeDepth(input.positionCS.z, _ZBufferParams));
    float transmittance = exp(-_DepthDensity * opticalDepth);
    
    //距離に応じたマスク
    float distanceMask = exp(-_DistanceDensity * length(inputData.baseInputData.positionWS - _WorldSpaceCameraPos));

    //視線反射ベクトル
    float vdotn = dot(inputData.baseInputData.viewDirectionWS.xyz, normalWS);
    
    // フレネル反射率を計算
    float frenel = Cal_Fresnel(vdotn);
    
    //Reflection
    //ミラー反射の強さ
    half4 reflectColor = Cal_ReflectWater(input, normalWS) * frenel * mainLight.shadowAttenuation;
    reflectColor *= _ReflectIntensity;
    
    half4 baseColor = reflectColor * _ShallowColor;
    baseColor = lerp(_DeepColor, baseColor, transmittance * max(0.5, shadowMask));
    baseColor = lerp(_FarColor, baseColor, distanceMask);
    
    //sss
    // Modulate the SSS color by the wave height.
    half4 sssColor = _SSSColor * GetWaveHeight(input.positionWS.xz, uv);
    sssColor.xyz *= saturate(dot(inputData.baseInputData.viewDirectionWS.xyz, mainLight.direction.xyz));

    // form色
    float2 foamUV = (inputData.baseInputData.positionWS.xz / _FoamScale) + (_FoamNoiseScale * normalTS.xz);
    half4 foamColor = half4(MotionFourWayChaos(_FoamTexture, foamUV, _FoamSpeed, false), 0);
    foamColor = foamColor * distanceMask * mainLight.shadowAttenuation;
    foamColor = foamColor * _FoamIntensity;

    // sunSpecular
    float3 viewR = reflect(inputData.baseInputData.viewDirectionWS, normalWS);
    float sunSpecularMask = saturate(dot(viewR, mainLight.direction.xyz));
    sunSpecularMask = round(saturate(pow(sunSpecularMask, _SunSpecularExponent)));
    sunSpecularMask = sunSpecularMask * mainLight.shadowAttenuation;
    half4 sunSpecularColor = half4(lerp(0, _SunSpecularColor, sunSpecularMask), 0);

    // spark
    float3 sparkly1 = MotionFourWaySparkle(_SparklesNormalMap, inputData.baseInputData.positionWS.xz / _SparkleScale, float4(1,2,3,4), _SparkleSpeed);
    float3 sparkly2 = MotionFourWaySparkle(_SparklesNormalMap, inputData.baseInputData.positionWS.xz / _SparkleScale, float4(1,0.5,2.5,2), _SparkleSpeed);
    float sparkleMask = dot(sparkly1, sparkly2) * saturate(3.0 * sqrt(saturate(dot(sparkly1.x, sparkly2.x))));
    sparkleMask = ceil(saturate(pow(sparkleMask, _SparkleExponent))) * mainLight.shadowAttenuation * distanceMask;
    half4 sparkleColor = half4(lerp(0, _SparkleColor, sparkleMask), 0);

    // エッジ色
    float edgeFoamMask = round(exp(-opticalDepth / _EdgeFoamDepth));
    half4 edgeFoamColor = half4(lerp(0, _EdgeFoamColor, edgeFoamMask), 0);
    //edgeFoamColor = edgeFoamColor * lerp(lllData.BaseToonLightingColor, 1, light.shadowAttenuation);
    
    // 計算した要素を足し合わせる
    half4 finalColor = sssColor + foamColor + sunSpecularColor + sparkleColor + edgeFoamColor + (giColor * _GIInfluence) + baseColor; 

    /*float2 screenPos = ComputeScreenPos(input.screenPos / input.screenPos.w).xy;
    float sceneDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, my_linear_clamp_sampler, screenPos);
    half depth = Linear01Depth(sceneDepth, _ZBufferParams);
    float D = max(0,1.0 - depth - 0.5);
    finalColor.rgb = 1 / (1 + exp(-500.0 * (D-0.5)));
    */
    // apply fog
    //finalColor.rgba = float4(1,1,1,1);//normalTS;//;//MixFog(finalColor.rgb, inputData.baseInputData.fogCoord);
    
    //finalColor.a = OutputAlpha(finalColor.a, _Surface);

    // フレネル反射率を反映
    //finalColor.a *= frenel;
    
    return finalColor;
}

#endif