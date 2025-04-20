using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour
{
    public Color paintColor;

    public float Range = 50;
    public float radius = 1;
    public float strength = 1;
    public float hardness = 1;
    public LayerMask mask;

    public bool FireForward;

    public Material paintMaterial;
    private Vector3 fireDirection;
    // Start is called before the first frame update
    void Start() 
    {
        if (FireForward) //called here to make update cleaner
        {
            fireDirection = Vector3.forward;
        }
        else
        {
            fireDirection = Vector3.down;
        }
    }

    // Update is called once per frame
    void Update()
    {


        Debug.DrawRay(this.gameObject.transform.position, fireDirection);
        Ray ray = new Ray(this.gameObject.transform.position, fireDirection);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            
            Paintable p = hit.collider.GetComponent<Paintable>();
            Debug.Log(hit.collider.gameObject.name);
            if (p != null)
            {
                PaintManager.instance.paint(p, hit.point, radius, hardness, strength, paintColor);
            }
        }
    }

}
