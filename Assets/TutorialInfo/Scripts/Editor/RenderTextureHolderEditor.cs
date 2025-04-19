using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;



[CustomEditor(typeof(GetRenderMask))]
public class RenderTextureHolderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GetRenderMask holder = (GetRenderMask)target;

        if (holder.maskTexture != null)
        {
            if (holder.maskTexture is RenderTexture rt)
            {
                if (GUILayout.Button("Save RenderTexture as PNG"))
                {
                    SaveRenderTextureAsPNG(rt);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Assigned texture is not a RenderTexture.", MessageType.Warning);
            }
        }
        else
        {
            Debug.Log("womp");
            EditorGUILayout.HelpBox("Assign a RenderTexture to enable saving.", MessageType.Info);
        }
    }

    private void SaveRenderTextureAsPNG(RenderTexture rt)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = currentRT;

        string path = EditorUtility.SaveFilePanel("Save RenderTexture as PNG", Application.dataPath, rt.name, "png");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllBytes(path, tex.EncodeToPNG());
            Debug.Log("RenderTexture saved to: " + path);
        }

        Object.DestroyImmediate(tex);
    }
}
