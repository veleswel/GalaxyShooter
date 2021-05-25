using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    private Vector3 bl;
    private Vector3 tl;
    private Vector3 br;
    private Vector3 tr;

    private Rect _bounds;

    public Rect Bounds 
    {
        get { return _bounds; }
    }

    void Awake()
    {
        bl = new Vector3();
        tl = new Vector3();
        br = new Vector3();
        tr = new Vector3();

        _bounds = new Rect();

        CalculateBounds();
    }

    void CalculateBounds()
    {
        bl = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
        tl = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -Camera.main.transform.position.z));
        br = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, -Camera.main.transform.position.z));
        tr = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, -Camera.main.transform.position.z));

        _bounds.xMin = bl.x;
        _bounds.yMin = bl.y;
        _bounds.xMax = tr.x;
        _bounds.yMax = tr.y;
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
