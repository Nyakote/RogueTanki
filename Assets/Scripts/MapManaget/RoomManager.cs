using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    #region Variables

    [SerializeField] int minRooms = 5;
    [SerializeField] int maxRooms = 15;
    [SerializeField] GameObject StartRoom;
    [SerializeField] GameObject Corridor;
    [SerializeField] GameObject[] Room;

    private List<GameObject> roomObjects = new List <GameObject>();
    private List<GameObject> doorObjects = new List <GameObject>();
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
    private GameObject currentRoom;

    private int ammountRooms;
    private int roomCount;

    private int gridSizeX = 200;
    private int gridSizeY = 200;
    private int roomWidth = 5;
    private int roomHeight = 5;
    private int[,] roomGrid;

    Room room;
    #endregion


    private void Start()
    {  
        room = GetComponent<Room>();
        roomGrid = new int[gridSizeX, gridSizeY];
        currentRoom = StartRoom;

        ammountRooms = Random.Range(minRooms, maxRooms + 1);
        GenerateRooms();
    }

    private void GenerateRooms()
    {
        StartRoomGenerationFromRoom(new Vector2Int(gridSizeX / 2, gridSizeY / 2));

        while (roomCount < ammountRooms)
        {
            Vector2Int randomIndex = new Vector2Int(Random.Range(0, gridSizeX), Random.Range(0, gridSizeY));

            if (roomGrid[randomIndex.x, randomIndex.y] == 0)
            {
                currentRoom = Room[Random.Range(0, Room.Length)];
                StartRoomGenerationFromRoom(randomIndex);
            }
        }
    }

    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizeX / 2), 0f, roomHeight * (gridY - gridSizeY / 2));   
    }
    
    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
      roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;
        var initialRoom = Instantiate(currentRoom, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"Room {roomCount}";
        initialRoom.transform.parent = transform;
        initialRoom.GetComponent<Room>().roomIndex = roomIndex;
        roomObjects.Add(initialRoom);
        int i = 1;
        var roomDoor =  initialRoom.GetComponent<Room>();
        foreach(Transform door in roomDoor.doors)
        {
            Vector3 pos = new Vector3(door.position.x, 0f, door.position.z);
            var cord = Instantiate(Corridor, pos, initialRoom.transform.rotation);
            cord.transform.parent = initialRoom.transform;
            cord.transform.name = $"Corridor {i}";
            i++;
        }


    }

    private void OnDrawGizmos()
    {
        Color givenColor = Color.white;
        Gizmos.color = givenColor;

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x,y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, 0f ,roomHeight));
            }
        }
    }
}
