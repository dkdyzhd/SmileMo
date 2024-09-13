using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject roomPrefab;
    [SerializeField] private int maxRooms = 10;
    [SerializeField] private int minRooms = 5;

    [SerializeField] int roomWidth = 20;
    [SerializeField] int roomHeight = 12;

    [SerializeField] int gridSizeX = 10;
    [SerializeField] int gridSizeY = 10;

    private List<GameObject> roomObjects = new List<GameObject>();

    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    private int[,] roomGrid;

    private int roomCount;

    private bool gernerationComplete = false;

    private void Start()
    {
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    private void Update()
    {
        if(roomQueue.Count > 0 && roomCount < maxRooms && !gernerationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;

            TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
            TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
        }

        else if(roomCount < minRooms)
        {
            Debug.Log("RoomCount was less than minimum amount of rooms, Try again ");
            RegenerateRooms();
        }

        else if (!gernerationComplete)
        {
            Debug.Log($"Generation complete, {roomCount} rooms created");
            gernerationComplete = true;
        }
        //아직 랜덤하게 생성 x
        //to do : random 하게 만들기 > TryGenerateRoom 에서 수정 ( 20분 12초 )
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;

        var initialRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"Room-{roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom);
    }

    //To do : create more rooms
    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        // ----- To Create Random
        if(roomCount >= maxRooms)
            return false;

        if(Random.value < 0.5f && roomIndex != Vector2.zero)
            return false;
        // ------ Create Rooms like snake( 연결연결 형태)
        if(CountAdjacentRooms(roomIndex) > 1)
            return false;

        roomQueue.Enqueue(roomIndex);
        roomGrid[x, y] = 1;
        roomCount++;

        var newRoom = Instantiate(roomPrefab,GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        newRoom.GetComponent<Room>().RoomIndex = roomIndex;
        newRoom.name = $"Room-{roomCount}";
        roomObjects.Add(newRoom);

        // ----- Open Door Activate
        OpenDoors(newRoom, x, y);

        return true;
    }

    //Under MinRoom
    private void RegenerateRooms()
    {
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue.Clear();
        roomCount = 0;
        gernerationComplete = false;

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2); 
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    void OpenDoors(GameObject room, int x, int y)
    {
        Room newRoomScript = room.GetComponent<Room>();

        //Neighbours
        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x-1, y));
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room topRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1));
        Room bottomRoomScript = GetRoomScriptAt(new Vector2Int(x, y-1));

        //Determine which doors to open based on the direction
        if(x > 0 && roomGrid[ x - 1, y] != 0)
        {
            //Neighbouring room to the left
            newRoomScript.OpenDoor(Vector2Int.left);
            rightRoomScript.OpenDoor(Vector2Int.right);
        }
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0)
        {
            //Neighbouring room to the right
            newRoomScript.OpenDoor(Vector2Int.right);
            rightRoomScript.OpenDoor(Vector2Int.left);
        }
        if (y > 0 && roomGrid[x, y - 1] != 0)
        {
            //Neighbouring room to the bottom
            newRoomScript.OpenDoor(Vector2Int.down);
            rightRoomScript.OpenDoor(Vector2Int.up);
        }
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0)
        {
            //Neighbouring room to the top
            newRoomScript.OpenDoor(Vector2Int.up);
            rightRoomScript.OpenDoor(Vector2Int.down);
        }
    }

    Room GetRoomScriptAt (Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(x => x.GetComponent<Room>().RoomIndex == index);
        if(roomObject != null)
            return roomObject.GetComponent<Room>();
        return null;
    }

    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        if (x > 0 && roomGrid[x - 1, y] != 0) count++; //left neighbour
        if(x < gridSizeX -1 && roomGrid[x + 1, y] != 0) count++; //right neighbour
        if (y > 0 && roomGrid[x,y-1] != 0) count++; // bottom neighbour
        if(y < gridSizeY -1 && roomGrid[x,y+1] != 0) count++; // top neighbour 

        return count;
    }

    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)  //room이 생성될 수 있는 grid 생성
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizeX / 2),
            roomHeight * (gridY - gridSizeY / 2));
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));  //room grid 를 기즈모로 보이게함
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }

}
