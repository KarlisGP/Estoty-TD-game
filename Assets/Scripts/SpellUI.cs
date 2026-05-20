using UnityEngine;
using UnityEngine.EventSystems;

public class SpellSlot : MonoBehaviour, IPointerDownHandler
{
    public GameObject spellPrefab;
    public int cost = 25;

    public void OnPointerDown(PointerEventData eventData)
    {
       
        {
           
            BuildingManager.instance.StartDraggingSpell(spellPrefab);
        }
    }
}