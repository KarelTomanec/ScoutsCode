using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questTimeSpan;
    public int numberOfQuests;

    public AiChickenController[] chickens;

    public GameObject flamableTreeQuestPrefab;
    public AudioClip flameSound;
    public AudioClip extinguishSound;

    public GameObject twoPlayerQuestObject;
    public GameObject animalSavePrefab;

    public GameObject fallingRockObject;
    public GameObject fallingRock;
    public GameObject fallingRockDestroyed;
    private Boolean moveFragments;
    private float moveDuration = 10f;
    private float elapsedDuration = 0f;

    public GameObject bearFeedQbject;
    private List<GameObject> eatenBerries;
    private List<BerriesPickupPointScript> eatenBushes;

    public GameManager gameManager;
    public RedCardManager redCardManager;
    public QuestPointer questPointer;
    public HighScoreHandler highScoreHandler;

    private int finishedEventCount = 0;
    private int lastQuestNumber = 1;
    private List<Quest> currentQuests;
    private float timeElapsed = 9f;

    private List<GameObject> flamableTrees;
    private List<GameObject> animalsToSave;
    [HideInInspector]public List<int> activeQuests;

    private bool gameOver = false;


    void Start()
    {
        flamableTrees = getAllFlamableTrees();
        AddSoundsToFlamableTrees();
        animalsToSave = getAllAnimalsToSave();
        currentQuests = new List<Quest>();
        activeQuests = new List<int>();
        eatenBushes = new List<BerriesPickupPointScript>();
        eatenBerries = new List<GameObject>();
        twoPlayerQuestObject.SetActive(false);
        for(int i = 0; i < numberOfQuests; i++)
        {
            activeQuests.Add(i);
        }
    }

     private void Update()
    {
        if(!gameOver)
        {
            if (timeElapsed > questTimeSpan)
            {
                timeElapsed = 0f;
                GenerateQuestInit();
            }

            foreach (Quest q in currentQuests.ToArray())
            {
                // check if quest was finished by user
                if (q.GetFinishedByUserStatus())
                {
                    if(q.GetReadyToDestroy())
                    {
                        q.Success();
                    }
                }
                else
                {
                    // check if quest has passed its time cap
                    if (q.GetElapsedTime() >= q.timeCap)
                    {
                        q.Fail();
                    }
                    q.AddElapsedTime(Time.deltaTime);
                }
            }
            timeElapsed += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (moveFragments)
        {
            //Debug.Log("COUNTING");
            elapsedDuration += Time.deltaTime;


            if (elapsedDuration > moveDuration / 2)
            {

                foreach  (Transform child in this.fallingRockObject.transform.Find("DestroyedRock").gameObject.transform)
                {
                    child.GetComponent<Rigidbody>().isKinematic = true;
                }

                //Debug.Log("MOVING");
                float step = 1 * Time.deltaTime;
                this.fallingRockObject.transform.Find("DestroyedRock").gameObject.transform.position = Vector3.MoveTowards(this.fallingRockObject.transform.Find("DestroyedRock").gameObject.transform.position, new Vector3(0, -100, 0), step);
            }

            if (elapsedDuration > moveDuration)
            {
                //Debug.Log("REMOVING");
                moveFragments = false;
                elapsedDuration = 0f;
                Destroy(this.fallingRockObject.transform.Find("DestroyedRock").gameObject);
            }
        }
    }

    private void GenerateQuestInit()
    {
        int questType;

        do
        {
            questType = Mathf.RoundToInt(UnityEngine.Random.Range(0, activeQuests.Count));
        } while (questType == lastQuestNumber);

        lastQuestNumber = questType;
        Debug.Log("Number of quests: " + activeQuests.Count);

        //questType = 4;

        switch (activeQuests[questType])
        {
            case Utils.treeQuestIndex:
                {
                    GenerateTreeQuest();
                    break;
                }
            case Utils.twoPlayerQuestIndex:
                {
                    GenerateTwoPlayerQuest();
                    break;
                }
            case Utils.saveAnimalQuestIndex:
                {
                    GenerateSaveAnimalQuest();
                    break;
                }
            case Utils.rockDestroyQuestIndex:
                {
                    GenerateRockDestroyQuest();
                    break;
                }
            case Utils.bearFeedQuestIndex:
                {
                    GenerateBearFeedQuest();
                    break;
                }
            default:
                break;
        }
    }

    private void GenerateBearFeedQuest()
    {
        Debug.Log("Genereating bear feed quest");

        RegrowBerries();
        ResetBushes();

        Vector3 position = bearFeedQbject.GetComponent<Transform>().position;

        // TODO: ADD ICON FOR BEAR
        QuestPointer.Pointer pointer = questPointer.CreatePointer(position, Utils.bearFeedQuestIndex);
        currentQuests.Add(new BearFeedQuest(this, bearFeedQbject, pointer));

        activeQuests.Remove(Utils.bearFeedQuestIndex);
        activeQuests.Remove(Utils.rockDestroyQuestIndex);
    }

    private void GenerateRockDestroyQuest()
    {
        Debug.Log("Genereating rock destroy quest");

        Vector3 position = new Vector3(-6.775665f, 7.74f, 28.46386f);
        Vector3 pointerPosition = GameObject.Find("FallingRockEvent").gameObject.transform.position;

        // TODO: ADD ICON FOR ROCK
        QuestPointer.Pointer pointer = questPointer.CreatePointer(pointerPosition, Utils.rockDestroyQuestIndex);
        currentQuests.Add(new RockDestroyQuest(position, fallingRock, fallingRockDestroyed, fallingRockObject, this, pointer));

        activeQuests.Remove(Utils.bearFeedQuestIndex);
        activeQuests.Remove(Utils.rockDestroyQuestIndex);

    }

    private void GenerateSaveAnimalQuest()
    {
        Debug.Log("Genereating save animal quest");

        GameObject selectedAnimal = animalsToSave[UnityEngine.Random.Range(0, animalsToSave.Count)];
        Vector3 position = selectedAnimal.GetComponent<Transform>().position;

        QuestPointer.Pointer pointer = questPointer.CreatePointer(position, Utils.saveAnimalQuestIndex);
        currentQuests.Add(new AnimalSaveQuest(position, animalSavePrefab, selectedAnimal, this, pointer));
    }

    private void GenerateTwoPlayerQuest()
    {
        Debug.Log("Genereating two player quest");
        Debug.Log(questPointer);
        QuestPointer.Pointer pointer = questPointer.CreatePointer(twoPlayerQuestObject.transform.position, Utils.twoPlayerQuestIndex);
        currentQuests.Add(new TwoPlayerQuest(this, twoPlayerQuestObject, pointer, chickens));
        twoPlayerQuestObject.SetActive(true);
    }

    private void GenerateTreeQuest()
    {
        Debug.Log("Genereating tree quest");

        GameObject selectedTree = flamableTrees[UnityEngine.Random.Range(0, flamableTrees.Count)];
        Vector3 position = selectedTree.GetComponent<Transform>().position;

        QuestPointer.Pointer pointer = questPointer.CreatePointer(position, Utils.treeQuestIndex);
        currentQuests.Add(new TreeQuest(position, flamableTreeQuestPrefab, this, selectedTree, pointer));
    }

    private List<GameObject> getAllFlamableTrees()
    {
        return new List<GameObject>(GameObject.FindGameObjectsWithTag(Utils.flamableTreeTag));
    }
     
    private List<GameObject> getAllAnimalsToSave()
    {
        return new List<GameObject>(GameObject.FindGameObjectsWithTag(Utils.animalToSaveTag));
    }

    private void AddFlamableTree(GameObject tree)
    {
        flamableTrees.Add(tree);
        if (flamableTrees.Count == 1)
        {
            activeQuests.Add(Utils.treeQuestIndex);
        }
    }

    private void RemoveFlamableTree(GameObject tree)
    {
        flamableTrees.Remove(tree);
        if(flamableTrees.Count == 0)
        {
            activeQuests.Remove(Utils.treeQuestIndex);
        }
    }

    private void AddAnimalToSave(GameObject animal)
    {
        animalsToSave.Add(animal);
        if (animalsToSave.Count == 0)
        {
            activeQuests.Add(Utils.saveAnimalQuestIndex);
        }
    }

    private void RemoveAnimalToSave(GameObject animal)
    {
        animalsToSave.Remove(animal);
        if (animalsToSave.Count == 0)
        {
            activeQuests.Remove(Utils.saveAnimalQuestIndex);
        }
    }

    private void AddPoints(int points)
    {
        gameManager.score.UpdateScore(points);
    }

    internal void AddDeletedBerries(GameObject berry)
    {
        this.eatenBerries.Add(berry);
    }

    internal void AddEatenBushes(BerriesPickupPointScript bush)
    {
        this.eatenBushes.Add(bush);
    }

    private void ResetBushes()
    {
        Debug.Log("WEEEEEEEEEEE");
        foreach (BerriesPickupPointScript bush in eatenBushes)
        {
            bush.SetEaten(false);
        }

        eatenBushes = new List<BerriesPickupPointScript>();
    }

    private void RegrowBerries()
    {
        foreach (GameObject berry in eatenBerries)
        {
            berry.SetActive(true);
        }

        eatenBerries = new List<GameObject>();
    }

    private void AddSoundsToFlamableTrees()
    {
        foreach (GameObject tree in flamableTrees)
        {
            AudioSource a = tree.AddComponent<AudioSource>() as AudioSource;
            a.clip = this.flameSound;
            a.volume = 0.1f;
            a.loop = true;

            tree.AddComponent<AudioSource>().clip = this.extinguishSound;
        }
    }


    public void Stop()
    {
        this.gameOver = true;

        highScoreHandler.ShowInputPart();
    }



    public class TreeQuest : Quest
    {
        private Vector3 position;
        private GameObject accuracySlider;
        private QuestManager questManager;
        private GameObject treeToBeBurnt;
        private List<GameObject> flames;
        private AudioSource flameSound;
        private AudioSource extinguishSound;
        private GameObject questPrefab;
        private Color firstColor;
        private GameObject flamePrefab;
        
        public TreeQuest(Vector3 position, GameObject accuracySlider, QuestManager questManager, GameObject tree, QuestPointer.Pointer pointer)
        {
            this.position = position;
            this.accuracySlider = accuracySlider;
            this.questManager = questManager;
            this.treeToBeBurnt = tree;
            this.pointer = pointer;
            this.finishedByUser = false;
            this.timeCap = 15f;
            this.points = 10;
            this.flames = new List<GameObject>();

            AudioSource[] audios = treeToBeBurnt.GetComponents<AudioSource>();
            foreach (AudioSource audio in audios)
            {
                if (audio.clip == this.questManager.flameSound)
                {
                    this.flameSound = audio;
                } else {
                    this.extinguishSound = audio;
                }
            }

            questManager.RemoveFlamableTree(treeToBeBurnt);
            InitPrefab();
        }

        private void InitPrefab()
        {
            questPrefab = Instantiate(accuracySlider, position, Quaternion.identity) as GameObject;
            questPrefab.GetComponent<AccuracySliderScript>().SetTreeQuest(this);
            SpawnParticles(treeToBeBurnt);
            flameSound.Play();
        }

        private void DestroySelf()
        {
            Debug.Log("Removing tree quest");
            questManager.currentQuests.Remove(this);
        }

        public void DestroyPrefab()
        {
            questPrefab.SetActive(false);
            Destroy(questPrefab);
        }
        

        public override void Success()
        {
            questManager.questPointer.DestroyPointer(this.pointer);
            questManager.AddPoints(points);

            StopParticles();
            flameSound.Stop();
            extinguishSound.Play();

            DestroyPrefab();
            DestroySelf();
            questManager.AddFlamableTree(treeToBeBurnt);
        }

        public override void Fail()
        {
            questManager.questPointer.DestroyPointer(this.pointer);
            treeToBeBurnt.tag = "Untagged";
            BurnTree(treeToBeBurnt);
            questManager.redCardManager.AddCard();
            DestroyPrefab();
            DestroySelf();
        }

        private void SpawnParticles(GameObject parent)
        {
            Transform transform = parent.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.tag == "Leaves")
                {
                    for (int j = 0; j < transform.GetChild(i).childCount; j++)
                    {
                        if(transform.GetChild(i).GetChild(j).childCount > 0)
                        {
                            if (transform.GetChild(i).GetChild(j).GetChild(0).tag == "Flame")
                            {
                                GameObject flame = transform.GetChild(i).GetChild(j).GetChild(0).gameObject;
                                flame.GetComponent<ParticleSystem>().Play();
                                flames.Add(flame);
                            }
                        }
                    }
                }
            }
        }

        private void StopParticles()
        {
            foreach(GameObject flame in flames)
            {
                flame.GetComponent<ParticleSystem>().Stop();
                for(int i = 0; i < flame.transform.childCount; i++)
                {
                    if (flame.transform.GetChild(i).gameObject.tag == "Light")
                    {
                        flame.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
                        Debug.Log("LIGHT");
                    }
                }
            }
        }
        
        private void BurnTree(GameObject parent)
        {
            flameSound.Stop();
            StopParticles();
            Transform transform = parent.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.tag == "Leaves")
                {
                    for(int j = 0; j < transform.GetChild(i).childCount; j++)
                    {
                        transform.GetChild(i).GetChild(j).gameObject.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
                else
                {
                    if (transform.GetChild(i).gameObject.tag == "Wood")
                    {
                        transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.black;
                    }
                    else
                    {
                        for (int j = 0; j < transform.GetChild(i).childCount; j++)
                        {
                            BurnTree(transform.GetChild(i).gameObject);
                        }
                    }
                }
            }
        }
    }

    public class TwoPlayerQuest : Quest
    {
        private QuestManager questManager;

        private GameObject questObject;

        private AiChickenController[] chickens;

        public TwoPlayerQuest(QuestManager questManager, GameObject quest, QuestPointer.Pointer pointer, AiChickenController[] chickens)
        {
            this.questManager = questManager;
            this.questObject = quest;
            this.pointer = pointer;
            this.finishedByUser = false;
            this.timeCap = 10f;
            this.points = 25;
            this.chickens = chickens;
            questManager.activeQuests.Remove(Utils.twoPlayerQuestIndex);
            questObject.SetActive(true);
            questObject.GetComponent<TwoPlayerQuestScript>().SetTwoPlayerQuest(this);
            questObject.GetComponent<TwoPlayerQuestScript>().Init();
        }

        private void DestroySelf()
        {
            Debug.Log("Removing two player quest");
            questManager.currentQuests.Remove(this);
        }

        public override void Fail()
        {
            questManager.redCardManager.AddCard();
            questManager.questPointer.DestroyPointer(this.pointer);
            questObject.SetActive(false);
            DestroySelf();
            foreach (var chicken in chickens)
            {
                chicken.EnableMovement();
            }
        }

        public override void Success()
        {
            questManager.AddPoints(points);
            questManager.questPointer.DestroyPointer(this.pointer);
            questObject.SetActive(false);
            DestroySelf();
            questManager.activeQuests.Add(Utils.twoPlayerQuestIndex);
        }
    }

    public class AnimalSaveQuest : Quest
    {
        private Vector3 position;
        private GameObject smashSlider;
        private GameObject animalToSave;

        private QuestManager questManager;
        private GameObject questPrefab;

        public AnimalSaveQuest(Vector3 position, GameObject smashSlider, GameObject animalToSave, QuestManager questManager, QuestPointer.Pointer pointer)
        {
            this.position = position;
            this.smashSlider = smashSlider;
            this.animalToSave = animalToSave;
            this.questManager = questManager;
            this.pointer = pointer;
            this.timeCap = 15f;
            this.points = 10;
            questManager.RemoveAnimalToSave(animalToSave);
            InitPrefab();
        }

        private void InitPrefab()
        {
            animalToSave.GetComponent<AiFoxController>().DisableMovement();
            animalToSave.GetComponents<AudioSource>()[1].Play();
            questPrefab = Instantiate(smashSlider, position, Quaternion.identity) as GameObject;
            questPrefab.GetComponent<SmashSliderScript>().SetAnimalSaveQuest(this);
        }

        private void DestroySelf()
        {
            Debug.Log("Removing save animal quest");
            questManager.currentQuests.Remove(this);
        }

        public void DestroyPrefab()
        {
            Destroy(questPrefab);
        }

        public override void Fail()
        {
            animalToSave.tag = "Untagged";
            animalToSave.GetComponent<AiFoxController>().Die();
            questManager.redCardManager.AddCard();
            questManager.questPointer.DestroyPointer(this.pointer);
            DestroyPrefab();
            DestroySelf();
        }

        public override void Success()
        {
            animalToSave.GetComponent<AiFoxController>().EnableMovement();
            animalToSave.GetComponents<AudioSource>()[0].Play();
            questManager.AddPoints(points);
            questManager.questPointer.DestroyPointer(this.pointer);
            DestroyPrefab();
            DestroySelf();
            questManager.AddAnimalToSave(animalToSave);
        }
    }

    public class RockDestroyQuest : Quest
    {
        private Vector3 startPosition;

        private QuestManager questManager;
        private GameObject rockObject;
        private GameObject destroyedRockObject;

        private GameObject questObject;

        public RockDestroyQuest(Vector3 startPosition, GameObject fallingRock, GameObject fallingRockDestroyed, GameObject questObject, QuestManager questManager, QuestPointer.Pointer pointer)
        {
            this.startPosition = startPosition;
            this.rockObject = fallingRock;
            this.destroyedRockObject = fallingRockDestroyed;
            this.questObject = questObject;
            this.questObject.GetComponent<FillSliderScript>().SetRunning(true);
            this.questManager = questManager;
            this.pointer = pointer;
            timeCap = float.MaxValue;
            this.points = 10;

            InitPrefab();
        }

        private void DestroySelf()
        {
            Debug.Log("Removing falling rock quest");
            DestroyPrefab();
            questManager.currentQuests.Remove(this);
            this.questObject.GetComponent<FillSliderScript>().SetRunning(false);
            this.questObject.GetComponent<FillSliderScript>().Clear();
        }

        private void InitPrefab()
        {
            GameObject rock = Instantiate(this.rockObject, startPosition, Quaternion.identity, this.questObject.transform) as GameObject;
            rock.name = "Rock";
            rock.transform.SetParent(this.questObject.transform, false);
            rock.GetComponent<AudioSource>().PlayDelayed(1.67f);
            questObject.GetComponentInChildren<FillSliderScript>().SetRockDestroyQuest(this);
        }

        private void DestroyPrefab()
        {
            Debug.Log("REMOVE FRAGMENTS");
            questManager.moveFragments = true;

            // fragments will be destroyed from update :'(
        }

        public override void Fail()
        {
            // NEVER
        }

        public override void Success()
        {       
            questManager.AddPoints(points);

            GameObject destroyedRock = Instantiate(this.destroyedRockObject, this.questObject.transform.Find("Rock").gameObject.transform.position, this.questObject.transform.Find("Rock").gameObject.transform.rotation);
            destroyedRock.name = "DestroyedRock";
            destroyedRock.transform.SetParent(this.questObject.transform);

            this.questObject.transform.Find("Rock").gameObject.SetActive(false);
            Destroy(this.questObject.transform.Find("Rock").gameObject);

            questManager.AddPoints(points);
            questManager.questPointer.DestroyPointer(this.pointer);
            DestroySelf();

            questManager.activeQuests.Add(Utils.rockDestroyQuestIndex);
            questManager.activeQuests.Add(Utils.bearFeedQuestIndex);
        }

        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSeconds(1.5f);   //Wait
            Debug.Log("End Wait() function and the time is: " + Time.time);
        }
    }

    public class BearFeedQuest : Quest
    {
        private QuestManager questManager;
        private GameObject bearEvent;
        
        public BearFeedQuest(QuestManager questManager, GameObject bearEvent, QuestPointer.Pointer pointer)
        {
            this.questManager = questManager;
            this.bearEvent = bearEvent;
            this.pointer = pointer;
            this.readyToDestroy = true;

            this.points = 20;
            this.timeCap = float.MaxValue;

            Debug.Log("SET");
            bearEvent.GetComponent<BearEventScript>().SetBearFeedQuest(this);
            bearEvent.GetComponent<BearEventScript>().Run();
        }

        private void DestroySelf()
        {
            Debug.Log("Removing bear feed quest");
            questManager.questPointer.DestroyPointer(this.pointer);
            questManager.currentQuests.Remove(this);
        }

        public override void Fail()
        {
            Debug.Log("Fail");
            DestroySelf();
        }

        public override void Success()
        {
            questManager.AddPoints(points);
            Debug.Log("Succ");
            DestroySelf();
        }

        public void SetExpiredTimeCap()
        {
            this.timeCap = 1f;
        }
    }
}

