/// <summary>
/// This script calculates the position of this gameobject based on a Lissajous Curve formula.
/// You can change the values for the various parameters of the formula to customize the curve.
/// The gameobject should have a Trail Renderer attached to it, so that the trail shows the curve.
/// Read up on Lissajous Curve on Wikipedia to understand the formula, and to also understand 
/// the various ways you can control the curve.
/// NOTE: Each time you wanna change values, exit Play Mode. If you change the values during
/// Play Mode, the design of whatever pattern is currently being drawn will get messed up.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lissajous : MonoBehaviour
{
    [Range(0,20)] public float A = 4f;
    public bool A_Random = false;
    [Range(0,20)] public float B = 4f;
    public bool B_Random = false;
    [Range(0,20)] public float C = 5f;
    public bool C_Random = false;
    [Range(0,20)] public float D = 4f;
    public bool D_Random = false;
    [Range(0,20)] public float delta = 2.3f;
    public bool delta_Random = false;
    public float t = 0f;
    [Range(1,5)] public float t_multipler = 2f;
    [Range(0,20)] public float multiplier = 1f;
    [Range(-20,20)] public float xOffset = 0f;    
    [Range(-20,20)] public float yOffset = 0f;

    public TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        // Checks if there is a Trail Renderer component attached to this gameobject.
        // If not, it will attach one, and set values for the relevant fields. But generally
        // it's best to set up the Trail Renderer component yourself from the inspector.
        CheckTrailRenderer();

        A = A_Random ? Random.Range(0f, 5f) : A;
        B = B_Random ? Random.Range(0f, 5f) : B;
        C = C_Random ? Random.Range(0f, 10f) : C;
        D = D_Random ? Random.Range(0f, 10f) : D;
        delta = delta_Random ? Random.Range(0f, 5f) : delta;
        
        Calculate();
        if(tr.enabled == false) tr.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Calculate();
    }

    void Calculate()
    {
        t = Time.time / 5 * t_multipler;
        Vector3 finalPos = transform.position;

        finalPos.x = (A * Mathf.Sin((C * t) + delta)) * multiplier;
        finalPos.y = (B * Mathf.Sin(D * t)) * multiplier;

        finalPos.x += xOffset;
        finalPos.y += yOffset;
        transform.position = finalPos;
    }

    void CheckTrailRenderer()
    {
        if(GetComponent<TrailRenderer>() == null)
        {
            tr = gameObject.AddComponent<TrailRenderer>();
            tr.time = 1000;
            tr.materials[0] = new Material(Shader.Find("Standard"));
            tr.startWidth = 0.02f;
            tr.endWidth = 0.02f;
        }
    }
}