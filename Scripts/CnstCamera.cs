using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CnstCamera : MonoBehaviour
//  ласс устанавливает фиксированное расширение дл€ камеры,
// при изменении разрешени€ окна, камера будет отдал€тьс€ или приближатьс€ к пользовательскому интерфейсу,
// устран€€ тем самым возможность выхода объектов UI за пределы экрана
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

        initFov = componentCamera.fieldOfView; // стандартный фов дл€ указанного соотношени€ сторон экрана
        horFov = CalcVerticalFov(initFov, 1 / targetAspect);
    }

    private void Update()
    {
        float constWidthSize = initSize * (targetAspect / componentCamera.aspect);
        componentCamera.orthographicSize = Mathf.Lerp(constWidthSize, initSize, WidthOrHeight); // с помощью линейной интерпол€ции расчитываетс€ размер камеры
    }

    private float CalcVerticalFov(float hFovInDeg, float aspectRatio)
    {
        float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

        float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio); 

        return vFovInRads * Mathf.Rad2Deg;
    }

}