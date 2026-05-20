using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    [Header("Setup")]
    public GameObject ghostTower; // The visual object that follows the mouse
    public LayerMask tileLayer;   // Set this to "Tiles" in the Inspector

    private GameObject currentPrefab;
    private BuildTile currentTile;
    private bool isDraggingSpell = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (ghostTower != null) ghostTower.SetActive(false);
    }

    // --- DRAG STARTING ---

    public void StartDraggingTower(GameObject towerPrefab)
    {
        isDraggingSpell = false;
        PrepareGhost(towerPrefab);
        Debug.Log("Dragging Tower: " + towerPrefab.name);
    }

    public void StartDraggingSpell(GameObject spellPrefab)
    {
        isDraggingSpell = true;
        PrepareGhost(spellPrefab);
        Debug.Log("Dragging Spell: " + spellPrefab.name);
    }

    private void PrepareGhost(GameObject prefab)
    {
        currentPrefab = prefab;
        ghostTower.SetActive(true);

        // Update Ghost Sprite to match the item
        SpriteRenderer ghostSR = ghostTower.GetComponent<SpriteRenderer>();
        SpriteRenderer prefabSR = prefab.GetComponentInChildren<SpriteRenderer>();
        
        if (prefabSR != null) ghostSR.sprite = prefabSR.sprite;
        
        ghostSR.color = new Color(1, 1, 1, 0.6f); // Semi-transparent
        ghostSR.sortingOrder = 500; // Front of everything
    }

    // --- UPDATE LOOP ---

    void Update()
    {
        if (currentPrefab == null || !ghostTower.activeInHierarchy) return;

        // 1. Move Ghost to Mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        ghostTower.transform.position = mousePos;

        // 2. Logic based on Mode
        if (!isDraggingSpell)
        {
            HandleTowerHover(mousePos);
        }

        // 3. Placing the Item
        if (Input.GetMouseButtonUp(0))
        {
            if (isDraggingSpell)
            {
                PlaceSpell(mousePos);
            }
            else
            {
                PlaceTower();
            }
        }
    }

    // --- TOWER LOGIC ---

    void HandleTowerHover(Vector3 worldPos)
    {
        // Look for a BuildTile on the "Tiles" layer
        Collider2D hit = Physics2D.OverlapPoint(worldPos, tileLayer);

        if (hit != null)
        {
            BuildTile tile = hit.GetComponent<BuildTile>();
            if (tile != null)
            {
                if (currentTile != tile)
                {
                    if (currentTile != null) currentTile.ResetColor();
                    currentTile = tile;
                }
                currentTile.Highlight(!currentTile.isOccupied);
            }
        }
        else
        {
            ClearCurrentTile();
        }
    }

    void PlaceTower()
    {
        if (currentTile != null && !currentTile.isOccupied)
        {
            // Spawn the tower exactly on the tile position
            GameObject newTower = Instantiate(currentPrefab, currentTile.transform.position, Quaternion.identity);
            currentTile.towerOnTile = newTower; // Occupy the tile

            // Trigger Pathfinding Update
            UpdateWorldNavigation();
            Debug.Log("Tower Built!");
        }
        else
        {
            Debug.Log("Build Cancelled: Not over a valid tile.");
        }

        CancelDragging();
    }

    // --- SPELL LOGIC ---

    void PlaceSpell(Vector3 pos)
    {
        // Spells ignore tiles and layers, just spawn at mouse
        Instantiate(currentPrefab, pos, Quaternion.identity);
        Debug.Log("Spell Cast!");
        CancelDragging();
    }

    // --- UTILITIES ---

    void ClearCurrentTile()
    {
        if (currentTile != null)
        {
            currentTile.ResetColor();
            currentTile = null;
        }
    }

    void CancelDragging()
    {
        ghostTower.SetActive(false);
        ClearCurrentTile();
        currentPrefab = null;
    }

    void UpdateWorldNavigation()
    {
        // Re-scan the grid for the A* pathfinding
        PathfindingGrid grid = Object.FindAnyObjectByType<PathfindingGrid>();
        if (grid != null) grid.CreateGrid();

        // Tell all enemies to find new paths around the new tower
        EnemyBase[] enemies = Object.FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        foreach (EnemyBase e in enemies)
        {
            e.RecalculatePath();
        }
    }
}