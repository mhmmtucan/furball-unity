using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneInformation : MonoBehaviour {

    public static SceneInformation instance = null;

    public Dictionary<string, GameObject> variables = new Dictionary<string, GameObject>();

    public List<GameObject> obstacles = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> collectables = new List<GameObject>();

    public GameObject finish_line = null;
    public Vector3 starting_position = Vector3.zero;

    void Awake() {
        GameInformation.dead = false;

        variables = new Dictionary<string, GameObject>() {
                        { "wall", null },
                        { "red cloud", null },
                        { "white cloud", null }
        };

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle").ToList<GameObject>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>();
        collectables = GameObject.FindGameObjectsWithTag("Collectable").ToList<GameObject>();

        finish_line = GameObject.FindGameObjectWithTag("FinishPoint");
        starting_position = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, -4.5f, 0.21f);

        instance = this;

        Debug.Log("init done");
    }
}
