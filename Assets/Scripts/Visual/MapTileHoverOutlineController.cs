using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider), typeof(Outline))]
public class MapTileHoverOutlineController : MonoBehaviour
{
    private Outline outlineComponent;
    private void Awake()
    {
        outlineComponent = GetComponent<Outline>();
        outlineComponent.enabled = false;
    }
    private void OnMouseEnter()
    {
        outlineComponent.enabled = true;
    }
    private void OnMouseExit() {
        outlineComponent.enabled = false;
    }
}
