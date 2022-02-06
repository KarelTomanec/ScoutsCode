using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float startDelay = 3f;
    public float endDelay = 3f;
    public CameraControl cameraControl;

    [SerializeField] public QuestPointer questPointer;

    public Score score;
    public Timer timer;

    public QuestManager questManager;

    public GameObject playerBobPrefab;
    public GameObject playerAlicePrefab;
    public PlayerManager[] players;

    public GameObject[] foxes;

    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    

    void Start() {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnAllPlayers();
        SetCameraTargets();
        SetQuestPointerPlayerTargets();

        timer.StartTimer();
        score.ShowScore();
    }

    private void Update()
    {
        
    }

    private void SpawnAllPlayers()
    {
        players[0].instance = Instantiate(playerBobPrefab, players[0].spawnPoint.position, players[0].spawnPoint.rotation) as GameObject;
        players[0].playerNumber = 1;
        players[0].Setup();
        players[0].instance.tag = Utils.player1Tag;
        players[0].instance.GetComponent<PlayerInventory>().SetVisualizer(GameObject.FindGameObjectWithTag(Utils.player1InventoryTag));

        players[1].instance = Instantiate(playerAlicePrefab, players[1].spawnPoint.position, players[1].spawnPoint.rotation) as GameObject;
        players[1].playerNumber = 2;
        players[1].instance.tag = Utils.player2Tag;
        players[1].instance.GetComponent<PlayerInventory>().SetVisualizer(GameObject.FindGameObjectWithTag(Utils.player2InventoryTag));
        players[1].Setup();
    }

    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[players.Length];

        for(int i = 0; i < targets.Length; i++)
        {
            targets[i] = players[i].instance.transform;
        }

        cameraControl.targets = targets;
    }

    private void SetQuestPointerPlayerTargets()
    {
        Transform[] targets = new Transform[players.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = players[i].instance.transform;
        }

        questPointer.targets = targets;
    }

    public void GameOver()
    {
        questManager.Stop();
        timer.StopTimer();

        GameObject.FindGameObjectWithTag(Utils.player1Tag).GetComponent<PlayerMovement>().DisableInput();
        GameObject.FindGameObjectWithTag(Utils.player2Tag).GetComponent<PlayerMovement>().DisableInput();
    }
}
