using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class RenderTextureSaverEditor
{ 
     [MenuItem("Assets/Save RenderTexture As PNG")]
    static void SaveRenderTexture()
    {
        RenderTexture rt = Selection.activeObject as RenderTexture;
        if (rt == null)
        {
            Debug.LogWarning("Selected object is not a RenderTexture.");
            return;
        }

        // Temporarily render to a Texture2D
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = currentRT;

        // Save the PNG
        string path = EditorUtility.SaveFilePanel("Save RenderTexture as PNG", Application.dataPath, rt.name, "png");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllBytes(path, tex.EncodeToPNG());
            Debug.Log("RenderTexture saved as PNG to: " + path);
        }

        Object.DestroyImmediate(tex);
    }

    [MenuItem("Assets/Save RenderTexture As PNG", true)]
    static bool ValidateSaveRenderTexture()
    {
        return Selection.activeObject is RenderTexture;
    }
}
