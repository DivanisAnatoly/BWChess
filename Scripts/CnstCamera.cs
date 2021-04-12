using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CnstCamera : MonoBehaviour
// ����� ������������� ������������� ���������� ��� ������,
// ��� ��������� ���������� ����, ������ ����� ���������� ��� ������������ � ����������������� ����������,
// �������� ��� ����� ����������� ������ �������� UI �� ������� ������
{
    public Vector2 DefaultResolution = new Vector2(1280, 720);
    [Range(0f, 1f)] public float WidthOrHeight = 0;

    private Camera componentCamera;

    private float initSize;
    private float targetAspect;

    private float initFov;
    private float horFov = 120f;

    private void Start()
    {
        componentCamera = GetComponent<Camera>();
        initSize = componentCamera.orthographicSize;

        targetAspect = DefaultResolution.x / DefaultResolution.y; //  

        initFov = componentCamera.fieldOfView; // ����������� ��� ��� ���������� ����������� ������ ������
        horFov = CalcVerticalFov(initFov, 1 / targetAspect);
    }

    private void Update()
    {
        float constWidthSize = initSize * (targetAspect / componentCamera.aspect);
        componentCamera.orthographicSize = Mathf.Lerp(constWidthSize, initSize, WidthOrHeight); // � ������� �������� ������������ ������������� ������ ������
    }

    private float CalcVerticalFov(float hFovInDeg, float aspectRatio)
    {
        float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

        float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio); 

        return vFovInRads * Mathf.Rad2Deg;
    }

}