using UnityEngine;
using UnityEngine.Rendering;

public class PaintManager : Singleton<PaintManager>{

    public Shader texturePaint;
    public Shader extendIslands;
    public Material sampleMaterial;


    int prepareUVID = Shader.PropertyToID("_PrepareUV");
    int positionID = Shader.PropertyToID("_PainterPosition");
    int hardnessID = Shader.PropertyToID("_Hardness");
    int strengthID = Shader.PropertyToID("_Strength");
    int radiusID = Shader.PropertyToID("_Radius");
    int blendOpID = Shader.PropertyToID("_BlendOp");
    int colorID = Shader.PropertyToID("_PainterColor");
    int textureID = Shader.PropertyToID("_MainTex");
    int uvOffsetID = Shader.PropertyToID("_OffsetUV");
    int uvIslandsID = Shader.PropertyToID("_UVIslands");

    Material paintMaterial;
    Material extendMaterial;

    CommandBuffer command;

    public override void Awake(){
        base.Awake();
        
        paintMaterial = new Material(texturePaint);
        extendMaterial = new Material(extendIslands);
        command = new CommandBuffer();
        command.name = "CommmandBuffer - " + gameObject.name;
    }

    public void initTextures(Paintable paintable){
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture uvIslands2 = paintable.getUVIslands2();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        command.SetRenderTarget(mask);
        command.SetRenderTarget(extend);
        command.SetRenderTarget(support);

        paintMaterial.SetFloat(prepareUVID, 1);
        command.SetRenderTarget(uvIslands);

        for (int i = 0; i < rend.materials.Length; i++)
        {
           // command.DrawRenderer(rend, paintMaterial, i);
        }

        //find all of renderes submeshes and draw on them.
        //HERE

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }


    public void paint(Paintable paintable, Vector3 pos, float radius = 1f, float hardness = .5f, float strength = .5f, Color? color = null){
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        paintMaterial.SetFloat(prepareUVID, 0);
        paintMaterial.SetVector(positionID, pos);
        paintMaterial.SetFloat(hardnessID, hardness);
        paintMaterial.SetFloat(strengthID, strength);
        paintMaterial.SetFloat(radiusID, radius);
        paintMaterial.SetTexture(textureID, support); //this might be setting texture? Do for each??
        paintMaterial.SetColor(colorID, color ?? Color.red);
        extendMaterial.SetFloat(uvOffsetID, paintable.extendsIslandOffset);
        extendMaterial.SetTexture(uvIslandsID, uvIslands);

        command.SetRenderTarget(mask);

        //For each submesh, 
        for (int i = 0; i < rend.materials.Length; i++)
        {
            command.DrawRenderer(rend, paintMaterial, i);
        }
        
        //command.DrawRenderer(rend, paintMaterial, 0);
        //command.DrawRenderer(rend, paintMaterial, 1);

        command.SetRenderTarget(support);
        command.Blit(mask, support);

        command.SetRenderTarget(extend);
        command.Blit(mask, extend, extendMaterial);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }

    public void colHere(Paintable paintable, Vector2 pos)
    {
        RenderTexture mask = paintable.getMask();
        //Debug.Log("Before:" + pos.x);
        int x = Mathf.FloorToInt(pos.x * mask.width);
        int y = Mathf.FloorToInt(pos.y * mask.height);



        Color pixelColor = colorSample(mask, pos);

        if(pixelColor.a <= 0.2f)
        {
            Debug.Log("not in goop:");
        }
        else
        {
            Debug.Log("in goop:" + pixelColor);
        }
        //Debug.Log($"RenderTexture pixel color at hit point: {pixelColor}");
    }

    public Color colorSample(RenderTexture source, Vector2 uv)
    {
       
        RenderTexture rt = RenderTexture.GetTemporary(1, 1, 0, RenderTextureFormat.ARGB32);
        RenderTexture.active = rt;

        sampleMaterial.SetTexture("_MainTex", source);
        sampleMaterial.SetVector("_UV", new Vector4(uv.x, uv.y, 0, 0));

        // Blit to sample just 1 pixel
        Graphics.Blit(null, rt, sampleMaterial);

        Texture2D tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, 1, 1), 0, 0);
        tex.Apply();

        Color sampledColor = tex.GetPixel(0, 0);

        RenderTexture.ReleaseTemporary(rt);
        Object.Destroy(tex);
        RenderTexture.active = null;

        return sampledColor;
    }

}
