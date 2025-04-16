using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour{
    public List<Connection> ConnectionPoints = new List<Connection>();

    [System.Serializable]
    public class Connection{
        public Vector3 position;
        public float angle;
        public Room adjacent = null;
    }
}
