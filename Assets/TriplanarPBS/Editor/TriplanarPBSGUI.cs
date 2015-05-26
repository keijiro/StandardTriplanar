//
// Custom material editor for the triplanar PBS
//

using UnityEngine;
using UnityEditor;
using System;

public class TriplanarPBSGUI : ShaderGUI
{
	MaterialProperty _albedoMap;
	MaterialProperty _albedoColor;
	MaterialProperty _metallic;
	MaterialProperty _smoothness;
	MaterialProperty _bumpMap;
	MaterialProperty _occlusionStrength;
	MaterialProperty _occlusionMap;
	MaterialProperty _mapScale;

	static GUIContent _albedoText     = new GUIContent("Albedo", "Albedo (RGB)");
	static GUIContent _normalMapText  = new GUIContent("Normal Map", "Normal Map");
	static GUIContent _occlusionText  = new GUIContent("Occlusion", "Occlusion (G)");

    bool _initial = true;

	void FindProperties(MaterialProperty[] props)
	{
		_albedoMap         = FindProperty("_MainTex", props);
		_albedoColor       = FindProperty("_Color", props);
		_metallic          = FindProperty("_Metallic", props, false);
		_smoothness        = FindProperty("_Glossiness", props);
		_bumpMap           = FindProperty("_BumpMap", props);
		_occlusionStrength = FindProperty("_OcclusionStrength", props);
		_occlusionMap      = FindProperty("_OcclusionMap", props);
		_mapScale          = FindProperty("_MapScale", props);
	}

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        FindProperties(properties);

        if (ShaderPropertiesGUI(materialEditor) || _initial)
            foreach (Material m in materialEditor.targets)
                SetMaterialKeywords(m);

        _initial = false;
    }

	bool ShaderPropertiesGUI(MaterialEditor materialEditor)
	{
		EditorGUI.BeginChangeCheck();

		materialEditor.TexturePropertySingleLine(_albedoText, _albedoMap, _albedoColor);
		materialEditor.ShaderProperty(_metallic, "Metallic");
		materialEditor.ShaderProperty(_smoothness, "Smoothness");

        EditorGUILayout.Space();

        materialEditor.TexturePropertySingleLine(_normalMapText, _bumpMap, null);

        EditorGUILayout.Space();

		materialEditor.TexturePropertySingleLine(_occlusionText, _occlusionMap, _occlusionMap.textureValue ? _occlusionStrength : null);

        EditorGUILayout.Space();

		materialEditor.ShaderProperty(_mapScale, "Mapping Scale");

        return EditorGUI.EndChangeCheck();
    }

    static void SetMaterialKeywords(Material material)
    {
        SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap"));
        SetKeyword(material, "_OCCLUSIONMAP", material.GetTexture("_OcclusionMap"));
    }

    static void SetKeyword(Material m, string keyword, bool state)
    {
        if (state)
            m.EnableKeyword(keyword);
        else
            m.DisableKeyword(keyword);
    }
}
