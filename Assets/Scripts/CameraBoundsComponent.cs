using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundsComponent : MonoBehaviour
{
    private Vector3 bl;
    private Vector3 tl;
    private Vector3 br;
    private Vector3 tr;

    private Rect bounds;

    public Rect Bounds 
    {
        get { return bounds; }
    }

    void Awake()
    {
        bl = new Vector3();
        tl = new Vector3();
        br = new Vector3();
        tr = new Vector3();

        bounds = new Rect();

        CalculateBounds();
    }

    void CalculateBounds()
    {
        bl = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
        tl = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -Camera.main.transform.position.z));
        br = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, -Camera.main.transform.position.z));
        tr = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, -Camera.main.transform.position.z));

        bounds.xMin = bl.x;
        bounds.yMin = bl.y;
        bounds.xMax = tr.x;
        bounds.yMax = tr.y;
    }

    void Update()
    {
        CalculateBounds();
    }

    void OnDrawGizmos()
    {
        Debug.DrawLine(bl, tl, Color.green);
        Debug.DrawLine(tl, tr, Color.green);
        Debug.DrawLine(tr, br, Color.green);
        Debug.DrawLine(br, bl, Color.green);
    }
}
