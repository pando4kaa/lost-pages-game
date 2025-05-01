using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class MapController_Manual : MonoBehaviour
{
    public static MapController_Manual Instance { get; set; }
    public GameObject mapParent;
    List<Image> mapImages;
    public Color highlightColor = Color.yellow;
    public Color dimmedColor = new Color(1f, 1f, 1f, 0.5f);
    public RectTransform playerIconTransform; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        mapImages = mapParent.GetComponentsInChildren<Image>().ToList();
    }

    public void HighlightArea(string areaName)
    {
        foreach (Image area in mapImages)
        {
            area.color = dimmedColor;
        }
        Image currentArea = mapImages.Find(x => x.name == areaName);
        if (currentArea != null)
        {
            currentArea.color = highlightColor;

            playerIconTransform.position = currentArea.GetComponent<RectTransform>().position;
        }
        else
        {
            Debug.LogWarning($"Area with name {areaName} not found");
        }
    }


}
