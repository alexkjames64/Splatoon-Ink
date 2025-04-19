using UnityEngine;

public class Paintable : MonoBehaviour {
    const int TEXTURE_SIZE = 1024;
    public bool KeepMask; //makes object keep mask on startup, use for places that should start painted
    public Texture2D MaskTexture;
    public RenderTexture CurrentMask;
    public float extendsIslandOffset = 1;

    public Renderer[] renderers;
    public Material[] materialsInRend;

    RenderTexture extendIslandsRenderTexture;
    RenderTexture uvIslandsRenderTexture;
    RenderTexture uvIslands2RenderTexture;
    RenderTexture maskRenderTexture;
    RenderTexture supportTexture;
    
    Renderer rend;

    int maskTextureID = Shader.PropertyToID("_MaskTexture");

    public RenderTexture getMask() => maskRenderTexture;
    public RenderTexture getUVIslands() => uvIslandsRenderTexture;

    //debug
    public RenderTexture getUVIslands2() => uvIslands2RenderTexture;

    public RenderTexture getExtend() => extendIslandsRenderTexture;
    public RenderTexture getSupport() => supportTexture;
    public Renderer getRenderer() => rend;

    public int getTextureSize() => TEXTURE_SIZE;
    void Start() {

        //debug
        renderers = GetComponentsInChildren<Renderer>();
        materialsInRend = renderers[0].materials;

        
            //this all happens if there is no render texture given to start with
            maskRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
            maskRenderTexture.filterMode = FilterMode.Bilinear;

            extendIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
            extendIslandsRenderTexture.filterMode = FilterMode.Bilinear;

            uvIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
            uvIslandsRenderTexture.filterMode = FilterMode.Bilinear;

            uvIslands2RenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
            uvIslands2RenderTexture.filterMode = FilterMode.Bilinear;

            supportTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
            supportTexture.filterMode = FilterMode.Bilinear;


        if (MaskTexture != null)
        {
            Graphics.Blit(MaskTexture, maskRenderTexture);
            Graphics.Blit(MaskTexture, extendIslandsRenderTexture);
            Graphics.Blit(MaskTexture, uvIslandsRenderTexture);
            Graphics.Blit(MaskTexture, uvIslands2RenderTexture);
            Graphics.Blit(MaskTexture, supportTexture);
            
        }


        rend = GetComponent<Renderer>();

        //for all submeshes/materials in renderer, set the maskTexture
        for (int i = 0; i < rend.materials.Length; i++)
        {
            rend.materials[i].SetTexture(maskTextureID, extendIslandsRenderTexture);
        }
       // rend.material.SetTexture(maskTextureID, extendIslandsRenderTexture);
        
        PaintManager.instance.initTextures(this);

        Debug.Log("HI!");
       
        var mesh = GetComponent<MeshFilter>().mesh;
        Debug.Log(mesh.name + " has " + mesh.subMeshCount + " submeshes!");
    }

    void OnDisable(){
        
        maskRenderTexture.Release();
        uvIslandsRenderTexture.Release();
        extendIslandsRenderTexture.Release();
        supportTexture.Release();
    }
}