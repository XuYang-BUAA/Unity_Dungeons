using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{

    public enum Direction { up, down, left, right };
    public Direction direction;

    [Header("房间信息")]
    public GameObject roomPrefab;
    public GameObject bossRoomPrefab;
    public int roomNumber;
    public Color starColor, endColor, midColor;
    public Room endRoom;

    [Header("位置控制")]
    public Transform generatorPoint;
    public float xOffset;
    public float yOffset;
    public LayerMask roomLayer;



    public List<Room> rooms = new List<Room>();
    public Room bossRoom;

    public WallType wallType;


    // Start is called before the first frame update
    void Start()
    {
        //Generate rooms
        for (int i = 0; i < roomNumber; i++)
        {
            rooms.Add(Instantiate(roomPrefab, generatorPoint.position,Quaternion.identity).GetComponent<Room>());

            ChangePointPos();
            rooms[i].GetComponent<SpriteRenderer>().color = midColor;
        }
        rooms[0].GetComponent<SpriteRenderer>().color = starColor;

        endRoom = rooms[0];

        //find end room
        foreach (var room in rooms)
        {
            if(ManhattanDis(room.gameObject)>ManhattanDis(endRoom.gameObject))
            {
                endRoom = room;
            }

            SetupRoom(room, room.transform.position);
        }

        GenerateBossRoom(endRoom);

        foreach (var room in rooms)
        {
            SetupWall(room, room.transform.position);
        }

        bossRoom.GetComponent<SpriteRenderer>().color = endColor;

    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.anyKeyDown)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
    }

    public void ChangePointPos()
    {
        do
        {
            direction = (Direction)UnityEngine.Random.Range(0, 4);

            switch (direction)
            {
                case Direction.up:
                    generatorPoint.position += new Vector3(0, yOffset, 0);
                    break;
                case Direction.down:
                    generatorPoint.position += new Vector3(0, -yOffset, 0);
                    break;
                case Direction.left:
                    generatorPoint.position += new Vector3(-xOffset, 0, 0);
                    break;
                case Direction.right:
                    generatorPoint.position += new Vector3(xOffset, 0, 0);
                    break;
            }
        } while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, roomLayer));
    }

    public void GenerateBossRoom(Room endRoom)
    {
        do
        {
            direction = (Direction)UnityEngine.Random.Range(0, 4);

            switch (direction)
            {
                case Direction.up:
                    generatorPoint.position = endRoom.transform.position + new Vector3(0, yOffset, 0);
                    break;
                case Direction.down:
                    generatorPoint.position = endRoom.transform.position + new Vector3(0, -yOffset, 0);
                    break;
                case Direction.left:
                    generatorPoint.position = endRoom.transform.position + new Vector3(-xOffset, 0, 0);
                    break;
                case Direction.right:
                    generatorPoint.position = endRoom.transform.position + new Vector3(xOffset, 0, 0);
                    break;
            }
        } while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, roomLayer));

        bossRoom = Instantiate(bossRoomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>();

        switch(direction)
        {
            case Direction.up:
                bossRoom.roomBottom = true;
                endRoom.roomTop = true;
                break;
            case Direction.down:
                bossRoom.roomTop = true;
                endRoom.roomBottom = true;
                break;
            case Direction.left:
                bossRoom.roomRight = true;
                endRoom.roomLeft = true;
                break;
            case Direction.right:
                bossRoom.roomLeft = true;
                endRoom.roomRight = true;
                break;
        }
        SetupWall(bossRoom, bossRoom.transform.position);
    }

    //Calculate Manhattan distance
    public float ManhattanDis(GameObject room)
    {
        float dis;
        dis = System.Math.Abs(room.transform.position.x) + System.Math.Abs(room.transform.position.y);
        return dis;
    }

    //Setup room confirming which direction there is room around
    public void SetupRoom(Room newRoom, Vector3 roomPos)
    {
        newRoom.roomTop = Physics2D.OverlapCircle(roomPos + new Vector3(0, yOffset, 0), 0.2f, roomLayer);
        newRoom.roomBottom = Physics2D.OverlapCircle(roomPos + new Vector3(0, -yOffset, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPos + new Vector3(-xOffset, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPos + new Vector3(xOffset, 0, 0), 0.2f, roomLayer);

    }

    public void SetupWall(Room room, Vector3 roomPos)
    {
        
        if (!room.roomTop && !room.roomBottom && !room.roomLeft && room.roomRight)
            Instantiate(wallType.wall_R, roomPos, Quaternion.identity);
        if (!room.roomTop && !room.roomBottom && room.roomLeft && !room.roomRight)
            Instantiate(wallType.wall_L, roomPos, Quaternion.identity);
        if (!room.roomTop && !room.roomBottom && room.roomLeft && room.roomRight)
            Instantiate(wallType.wall_LR, roomPos, Quaternion.identity);
        if (!room.roomTop && room.roomBottom && !room.roomLeft && !room.roomRight)
            Instantiate(wallType.wall_D, roomPos, Quaternion.identity);
        if (!room.roomTop && room.roomBottom && !room.roomLeft && room.roomRight)
            Instantiate(wallType.wall_DR, roomPos, Quaternion.identity);
        if (!room.roomTop && room.roomBottom && room.roomLeft && !room.roomRight)
            Instantiate(wallType.wall_DL, roomPos, Quaternion.identity);
        if (!room.roomTop && room.roomBottom && room.roomLeft && room.roomRight)
            Instantiate(wallType.wall_DLR, roomPos, Quaternion.identity);
        if (room.roomTop && !room.roomBottom && !room.roomLeft && !room.roomRight)
            Instantiate(wallType.wall_U, roomPos, Quaternion.identity);
        if (room.roomTop && !room.roomBottom && !room.roomLeft && room.roomRight)
            Instantiate(wallType.wall_UR, roomPos, Quaternion.identity);
        if (room.roomTop && !room.roomBottom && room.roomLeft && !room.roomRight)
            Instantiate(wallType.wall_UL, roomPos, Quaternion.identity);
        if (room.roomTop && !room.roomBottom && room.roomLeft && room.roomRight)
            Instantiate(wallType.wall_ULR, roomPos, Quaternion.identity);
        if (room.roomTop && room.roomBottom && !room.roomLeft && !room.roomRight)
            Instantiate(wallType.wall_UD, roomPos, Quaternion.identity);
        if (room.roomTop && room.roomBottom && !room.roomLeft && room.roomRight)
            Instantiate(wallType.wall_UDR, roomPos, Quaternion.identity);
        if (room.roomTop && room.roomBottom && room.roomLeft && !room.roomRight)
            Instantiate(wallType.wall_UDL, roomPos, Quaternion.identity);
        if (room.roomTop && room.roomBottom && room.roomLeft && room.roomRight)
            Instantiate(wallType.wall_UDLR, roomPos, Quaternion.identity);
    }
}



[System.Serializable]
public class WallType
{
    public GameObject wall_U, wall_D, wall_L, wall_R,
                      wall_UD, wall_UL, wall_UR, wall_LR, wall_DL, wall_DR,
                      wall_UDL, wall_UDR, wall_DLR, wall_ULR,
                      wall_UDLR;

}