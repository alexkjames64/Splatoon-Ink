using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

//this class gets the rendermask for us
public class GetRenderMask : MonoBehaviour
{
    public Texture maskTexture;
    private Paintable paint;
    private int TextureSize;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        paint = GetComponent<Paintable>();
        TextureSize = paint.getTextureSize();
        rend = paint.getRenderer();
    }
    // Update is called once per frame
    void Update()
    { 
        paint = GetComponent<Paintable>();
        rend = paint.getRenderer();
        maskTexture = rend.material.GetTexture("_MaskTexture");
    }
}
