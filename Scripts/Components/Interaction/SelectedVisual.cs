using System;
using UnityEngine;

public class SelectedVisual : MonoBehaviour, ISelection
{
    [SerializeField] private GameObject selected;
    [SerializeField] private GameObject normal;

    private Outline.Mode outlineMode = Outline.Mode.OutlineVisible;
    private Color outlineColor;
    private float outlineWidth = 2;

    private void Start()
    {
        ColorUtility.TryParseHtmlString("#BDFFAD", out outlineColor);

        normal.SetActive(true);
        Outline outline = selected.AddComponent<Outline>();
        outline.OutlineMode = outlineMode;
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;
        selected.SetActive(false);
        
    }

    public void SelectObject(bool isSelected)
    {
        selected.SetActive(isSelected);
        normal.SetActive(!isSelected);
    }
}
