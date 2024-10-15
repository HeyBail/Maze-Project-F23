using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private MazeMaker mazeMaker;
    private UIManager uIManager;
    public Vector2Int exitLocation;
    /*private GameObject[] targets;*/

    // Start is called before the first frame update
    void Start()
    {
        mazeMaker = MazeMaker.FindFirstObjectByType<MazeMaker>();
        uIManager = UIManager.FindFirstObjectByType<UIManager>();
        /*targets = GameObject.FindGameObjectsWithTag("Target");
        Debug.Log(targets.Length);*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            if (GameObject.FindGameObjectsWithTag("Target").Length == 0)
            {
                mazeMaker.finishLevel(exitLocation);
                //regenerate maze
                Debug.Log("Regenerate Maze :D");
                uIManager.refreshTargetText();
                Destroy(gameObject);
            }
        }
    }
}