using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipseOutline : Singleton<EllipseOutline>
{
    [SerializeField] float x = 30;
    [SerializeField] float z = 20;

    [SerializeField] int divisions = 100;
    private int divisionsCorrection;
    private LineRenderer line_Renderer;
    private Material material;

    private Vector3[] positions;
    private float dangle;

    private float previous_x;
    private float previous_z;
    private float previous_divisions;

    void Start()
    {
        line_Renderer = gameObject.AddComponent<LineRenderer>();
        line_Renderer.widthMultiplier = 0.1f;
        material = new Material(Shader.Find("Unlit/Color"));
        material.color = Color.white;
        line_Renderer.material = material;

        Update_Divisions();
        DrawEllipseOutLine();
    }

    void Update()
    {
        if ((previous_x != x) || (previous_z != z)) DrawEllipseOutLine();
        if (previous_divisions != divisions) Update_Divisions();
    }

    private void Update_Divisions()
    {
        divisionsCorrection = divisions + 1;
        positions = new Vector3[divisionsCorrection];
        line_Renderer.positionCount = divisionsCorrection;
        dangle = 2 * Mathf.PI / divisions;
        previous_divisions = divisions;
        DrawEllipseOutLine();
    }

    private void DrawEllipseOutLine()
    {
        float angle = 0;
        Vector3 position = new Vector3();
        for (int i = 0; i < divisionsCorrection; i++, angle += dangle)
        {
            position.x = x * Mathf.Cos(angle);
            position.z = z * Mathf.Sin(angle);
            positions[i] = position;
        }
        line_Renderer.SetPositions(positions);
        previous_x = x;
        previous_z = z;
    }

    private bool isUpperArea = false;

    public DivisionInfo GetDivisionPointPosition()
    {
        int avoidPoint1 = 0;
        int avoidPoint2 = divisions / 2;
        int targetPoint = 0;

        if (isUpperArea) targetPoint = Random.Range(avoidPoint1, avoidPoint2);
        else targetPoint = Random.Range(avoidPoint2 + 1, divisionsCorrection);

        targetPoint = 6;

        DivisionInfo divisionInfo = new DivisionInfo();
        divisionInfo.peripheryPoint = positions[targetPoint];
        divisionInfo.degreeToCenter = Angle_360(divisionInfo.peripheryPoint, Vector3.zero);




        return divisionInfo;

        float Angle_360(Vector3 from_, Vector3 to_)
        {
            float x = from_.x - to_.x;
            float z = from_.z - to_.z;

            float hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(z, 2f));

            float cos = x / hypotenuse;
            float radian = Mathf.Acos(cos);
 
            float angle = 180 / (Mathf.PI / radian);

            if (z < 0) angle = -angle;
            else if ((z == 0) && (x < 0)) angle = 180;

            return angle;
        }
    }
}

public class DivisionInfo
{
    public float degreeToCenter;
    public Vector3 peripheryPoint;
}
