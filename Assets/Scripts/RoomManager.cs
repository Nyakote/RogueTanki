using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject RoomPrefab;
    [SerializeField] int maxRooms = 15;
    [SerializeField] int minRooms = 7;

    int gridSizeY = 10;
    int gridSizeX = 10;

    private List<GameObject> roomObjects = new List<GameObject>();

    private int[,] roomGrid;
    private int roomCount = 0;

    private void Start()
    { 
        roomGrid = new int[gridSizeX,gridSizeY];
    }

    private Vector2Int GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector2Int(gridX, gridY);
    }

    private void OnDrawGizmos()
    {
        Color color = Color.white;
        Gizmos.color = color;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2Int position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(new Vector3(position.x, position.y), new Vector3(20f, 12f, 1));
            }
        }
    }
}
