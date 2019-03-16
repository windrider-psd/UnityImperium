﻿using Imperium.Navigation;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(ShipController))]
public class ShipLineRendererController : MonoBehaviour
{
    public Vector3 endPosition;

    private static Dictionary<CommandType, LineRendererColors> commandColors = new Dictionary<CommandType, LineRendererColors>()
    {
        {CommandType.Attack, new LineRendererColors(Color.red, Color.red)},
        {CommandType.Build, new LineRendererColors(Color.yellow, Color.yellow)},
        {CommandType.Mine, new LineRendererColors(Color.yellow, Color.yellow)},
        {CommandType.Move, new LineRendererColors(Color.white, Color.white)}
    };

    private bool _active;
    private float framesPerSecond = 30f;

    private LineRenderer lineRenderer;

    private bool mouseOver = false;
    private ShipController shipController;
    private int uvAnimationTileX = -30;
    private int uvAnimationTileY = 1;

    public bool Active
    {
        get
        {
            return _active;
        }
        set
        {
            lineRenderer.enabled = value;
            _active = value;
        }
    }

    private void ActivateIfPossible()
    {
        if (shipController.fleetCommandQueue.CurrentFleetCommand == null)
        {
            Active = false;
        }
        else
        {
            endPosition = shipController.fleetCommandQueue.CurrentFleetCommand.destination;
            if (!Active)
            {
                Active = true;
            }
        }
    }

    private void AnimateLineRenderer()
    {
        int index = (int)(Time.time * framesPerSecond);
        index = index % (uvAnimationTileX * uvAnimationTileY);

        var size = new Vector2(1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY);

        var uIndex = index % uvAnimationTileX;
        var vIndex = index / uvAnimationTileX;

        var offset = new Vector2(uIndex * size.x * 2, 1.0f - size.y - vIndex * size.y);

        lineRenderer.material.SetTextureOffset("_MainTex", offset);
    }

    private void OnMouseEnter()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }

    private void SetLineRendererColor()
    {
        if (shipController.fleetCommandQueue.CurrentFleetCommand != null)
        {
            CommandType commandType = shipController.fleetCommandQueue.CurrentFleetCommand.commandType;
            LineRendererColors lineRendererColors = commandColors[commandType];

            lineRenderer.startColor = lineRendererColors.startColor;
            lineRenderer.endColor = lineRendererColors.endColor;
        }
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        shipController = GetComponent<ShipController>();
    }

    private void Update()
    {
        if (Active)
        {
            AnimateLineRenderer();
            UpdateLineRendererPositions();
        }

        if (MouseCommandsController.Instance.selectedGOs.Contains(gameObject) || mouseOver)
        {
            ActivateIfPossible();
            SetLineRendererColor();
        }
        else
        {
            if (Active)
            {
                Active = false;
            }
        }
    }

    private void UpdateLineRendererPositions()
    {
        //LineRenderer must use world space
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    public struct LineRendererColors
    {
        public Color endColor;
        public Color startColor;

        public LineRendererColors(Color startColor, Color endColor)
        {
            this.startColor = startColor;
            this.endColor = endColor;
        }
    }
}