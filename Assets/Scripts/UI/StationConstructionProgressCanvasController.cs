﻿using UnityEngine;

[RequireComponent(typeof(StationController))]
public class StationConstructionProgressCanvasController : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private GameObject progressCanvasPrefab;

    [SerializeField]
    private float scale;

    private StationConstructionProgressCanvas stationConstructionProgressCanvas;

    private StationController stationController;

    private void Start()
    {
        stationController = GetComponent<StationController>();
        if (!stationController.constructed)
        {
            stationConstructionProgressCanvas = Instantiate(progressCanvasPrefab, transform.position, progressCanvasPrefab.transform.rotation, transform)
                .GetComponent<StationConstructionProgressCanvas>();

            Vector3 localScale = stationConstructionProgressCanvas.gameObject.transform.localScale;

            stationConstructionProgressCanvas.gameObject.transform.localScale = localScale * scale;

            stationConstructionProgressCanvas.gameObject.transform.localPosition = offset;
        }
    }

    private void Update()
    {
        if (stationController.constructed)
        {
            if (stationConstructionProgressCanvas != null)
            {
                stationConstructionProgressCanvas.gameObject.SetActive(false);
                enabled = false;
            }
        }
        else
        {
            stationConstructionProgressCanvas.progressSlider.fillAmount = stationController.constructionProgress / 100;
        }
    }
}