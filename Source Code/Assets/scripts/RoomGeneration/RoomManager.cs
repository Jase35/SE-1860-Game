using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour{
    //public Room TestRoom; //test
    public Room InitialRoom;

    public List<GameObject> RoomPrefabs = new List<GameObject>();

    /*void Update(){
        //testing
        if(Input.GetKeyDown("y")){
            Debug.Log(RotateVector(new Vector3(0.0f, 0.0f, 5.0f), 0.0f));
            Debug.Log(RotateVector(new Vector3(0.0f, 0.0f, 5.0f), 90.0f));
            Debug.Log(RotateVector(new Vector3(0.0f, 0.0f, 5.0f), 180.0f));
            Debug.Log(RotateVector(new Vector3(0.0f, 0.0f, 5.0f), 270.0f));
        }
        if(Input.GetKeyDown("l")){
            UpdateRooms(TestRoom);
        }
        if(Input.GetKeyDown("p")){
            GenerateAround(TestRoom);
        }
        if(Input.GetKeyDown("o")){
            DegenerateAround(TestRoom);
        }
    }*/

    void Awake(){
        CurrentRoom = InitialRoom;
        UpdateRooms(CurrentRoom);
    }

    private Room CurrentRoom;

    private List<Room> Surrounding = new List<Room>();
    private List<Room> SecondSurrounding = new List<Room>();
    public void UpdateRooms(Room current){
        GenerateAround(current);

        Surrounding.Clear();
        SecondSurrounding.Clear();

        for(int i = 0; i < current.ConnectionPoints.Count; i++){
            if(current.ConnectionPoints[i].adjacent != null){
                Surrounding.Add(current.ConnectionPoints[i].adjacent);
            }
        }

        for(int i = 0; i < Surrounding.Count; i++){
            GenerateAround(Surrounding[i]);
        }

        Room cur;
        Room adj;
        for(int i = 0; i < Surrounding.Count; i++){
            cur = Surrounding[i];
            for(int j = 0; j < cur.ConnectionPoints.Count; j++){
                adj = cur.ConnectionPoints[j].adjacent;
                if(adj != current && !Surrounding.Contains(adj)){
                    SecondSurrounding.Add(adj);
                }
            }
        }

        for(int i = 0; i < SecondSurrounding.Count; i++){
            cur = SecondSurrounding[i];
            for(int j = 0; j < cur.ConnectionPoints.Count; j++){
                adj = cur.ConnectionPoints[j].adjacent;
                if(adj != null && !Surrounding.Contains(adj)){
                    Destroy(adj.gameObject);
                    cur.ConnectionPoints[j].adjacent = null;
                }
            }
        }
    }

    private void GenerateAround(Room room){
        Room.Connection curcon;
        Room NextRoom;
        Room.Connection newcon;
        GameObject ChosenPrefab;
        float roomangle;
        for(int i = 0; i < room.ConnectionPoints.Count; i++){
            curcon = room.ConnectionPoints[i];
            if(curcon.adjacent == null || curcon.adjacent == room){
                ChosenPrefab = RoomPrefabs[Random.Range(0, RoomPrefabs.Count)];

                NextRoom = ChosenPrefab.GetComponent<Room>();
                int id = Random.Range(0, NextRoom.ConnectionPoints.Count);
                newcon = NextRoom.ConnectionPoints[id];

                roomangle = (room.transform.localEulerAngles.y + 360.0f) % 360.0f;

                float rot = NewRotation(curcon.angle + roomangle, newcon.angle);
                Vector3 pos = room.transform.position + NewPosition(curcon.position, /*curcon.angle*/roomangle, newcon.position, rot);
                GameObject NewRoom = Instantiate(ChosenPrefab, pos, Quaternion.Euler(0.0f, rot, 0.0f), transform);

                Room NewRoomScript = NewRoom.GetComponent<Room>();
                room.ConnectionPoints[i].adjacent = NewRoomScript;
                NewRoomScript.ConnectionPoints[id].adjacent = room;

                NewRoom.name = "Room" + i.ToString() + "(" + room.name + ")";
            }
        }
    }

    private void DegenerateAround(Room room){
        for(int i = 0; i < room.ConnectionPoints.Count; i++){
            Destroy(room.ConnectionPoints[i].adjacent.gameObject);
            room.ConnectionPoints[i].adjacent = null;
        }
    }

    private float NewRotation(float a, float b){
        float dif = 180.0f - (b - a);
        return (dif + 360.0f) % 360.0f;
    }

    private Vector3 NewPosition(Vector3 oldpos, float oldangle, Vector3 newpos, float angle){
        float oa = (oldangle + 360.0f) % 360.0f;
        float an = (angle + 360.0f) % 360.0f;
        return RotateVector(oldpos, oa) - RotateVector(newpos, an);
    }

    private Vector3 RotateVector(Vector3 vec, float angle){
        angle = (float)((Mathf.RoundToInt(angle) + 1440) % 360);
        if(Mathf.Abs(angle - 90.0f) < 0.001f){
            return new Vector3(vec.z, 0.0f, -vec.x);
        }
        if(Mathf.Abs(angle - 180.0f) < 0.001f){
            return new Vector3(-vec.x, 0.0f, -vec.z);
        }
        if(Mathf.Abs(angle - 270.0f) < 0.001f){
            return new Vector3(-vec.z, 0.0f, vec.x);
        }
        return vec;
    }
}
