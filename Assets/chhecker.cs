using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chhecker : MonoBehaviour
{
    public Color paintColor;
    public LayerMask mas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(this.gameObject.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, mas))
        {
            Debug.DrawRay(this.gameObject.transform.position, Vector3.down);
            Paintable p = hit.collider.GetComponent<Paintable>();

            if(p != null)
            {
               // Debug.Log("here!");
              // PaintManager.instance.paint(p, hit.textureCoord, 0.1f, 1, 1, paintColor);
                PaintManager.instance.colHere(p, hit.textureCoord);
            }
            /*
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null && renderer.material.HasProperty("_MaskTexture"))
            {
                Debug.Log("");

                Texture2D maskTexture = renderer.material.GetTexture("_MaskTexture") as Texture2D;

                //Debug.Log(renderer.material.GetTexture("_MaskTexture"));

                if (maskTexture != null)
                {
                 //   Debug.Log("here!");
                }
            }
            */
         }



        }
}
