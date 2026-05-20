using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSlot : MonoBehaviour, IPointerDownHandler
{
    public GameObject towerPrefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        {
            BuildingManager.instance.StartDraggingTower(towerPrefab);
        }
    }
}