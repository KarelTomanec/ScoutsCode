
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPointer : MonoBehaviour {
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite trapSprite;
    [SerializeField] private Sprite gateSprite;
    [SerializeField] private Sprite bearSprite;
    [SerializeField] private Sprite rockSprite;
    [SerializeField] private Camera uiCamera;
    [HideInInspector] public Transform[] targets;

    private List<Pointer> questPointerList;

    private void Awake()
    {
        questPointerList = new List<Pointer>();
    }

    private void Update()
    {
        foreach (Pointer questPointer in questPointerList)
        {
            questPointer.Update();
        }
    }


    public Pointer CreatePointer(Vector3 targetPosition, int questIndex)
    {
        GameObject pointerGameObject = Instantiate(transform.Find("PointerTemplate").gameObject);
        pointerGameObject.SetActive(true);
        pointerGameObject.transform.SetParent(transform, false);
        Pointer questPointer = null;
        switch (questIndex)
        {
            case Utils.treeQuestIndex:
                {
                    questPointer = new Pointer(targetPosition, pointerGameObject, uiCamera, fireSprite, targets);
                    break;
                }
            case Utils.twoPlayerQuestIndex:
                {
                    questPointer = new Pointer(targetPosition, pointerGameObject, uiCamera, gateSprite, targets);
                    break;
                }
            case Utils.saveAnimalQuestIndex:
                {
                    questPointer = new Pointer(targetPosition, pointerGameObject, uiCamera, trapSprite, targets);
                    break;
                }
            case Utils.rockDestroyQuestIndex:
                {
                    questPointer = new Pointer(targetPosition, pointerGameObject, uiCamera, rockSprite, targets);
                    break;
                }
            case Utils.bearFeedQuestIndex:
                {
                    questPointer = new Pointer(targetPosition, pointerGameObject, uiCamera, bearSprite, targets);
                    break;
                }
            default:
                break;
        }
        questPointerList.Add(questPointer);
        return questPointer;
    }

    public void DestroyPointer(Pointer questPointer)
    {
        questPointerList.Remove(questPointer);
        questPointer.DestroySelf();
    }

    public class Pointer
    {
        private Vector3 targetPosition;
        private GameObject pointerGameObject;
        private Sprite sprite;
        private Camera uiCamera;
        private RectTransform pointerRectTransform;
        private Image pointerImage;
        private Transform[] targets;
        private Vector3 moveVelocity;

        public Pointer(Vector3 targetPosition, GameObject pointerGameObject, Camera uiCamera, Sprite sprite, Transform[] targets)
        {
            this.targetPosition = targetPosition;
            this.targetPosition.y += 1f;
            this.pointerGameObject = pointerGameObject;
            this.sprite = sprite;
            this.uiCamera = uiCamera;
            this.targets = targets;
            /*targetPosition = new Vector3(4, 0, 4); //(6.75f, 0.05f, 22.34f);*/
            pointerRectTransform = pointerGameObject.GetComponent<RectTransform>();
            pointerImage = pointerGameObject.GetComponent<Image>();
        }

        public void Update()
        {
            Vector3 toPosition = targetPosition;
            float borderSize = 40f;
            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(toPosition);
            bool isOffScreen = targetPositionScreenPoint.x <= 0 || targetPositionScreenPoint.x >= Screen.width || targetPositionScreenPoint.y <= 0 || targetPositionScreenPoint.y >= Screen.height;
            //Debug.Log(isOffScreen + ":" + targetPositionScreenPoint);

            if (isOffScreen)
            {
               // RotatePointerTowardsTargetPosition();
                pointerImage.sprite = sprite;
                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;

                cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, borderSize, Screen.width - borderSize);
                cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, borderSize, Screen.height - borderSize);

                Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
                //pointerRectTransform.position = pointerWorldPosition;
                //pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
                pointerRectTransform.position = Vector3.SmoothDamp(pointerRectTransform.position, pointerWorldPosition, ref moveVelocity, 0.2f);
                pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
            }
            else
            {
                pointerImage.sprite = sprite;
                Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(targetPositionScreenPoint);
                pointerRectTransform.position = pointerWorldPosition;
                pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
                pointerRectTransform.localEulerAngles = Vector3.zero;
            }
        }

        public void DestroySelf()
        {
            Destroy(pointerGameObject);
        }

        private void RotatePointerTowardsTargetPosition()
        {
            Vector3 toPosition = targetPosition;
            Vector3 fromPosition = FindAveragePosition();
            fromPosition.y = 0f;
            toPosition.y = 0f;
            Vector3 dir = (toPosition - fromPosition).normalized;
            float angle = GetAngleFromVectorFloat(dir);
            pointerRectTransform.localEulerAngles = new Vector3(0, 180, angle + 90);
        }

        private static float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            n += 360;
            return n;
        }

        private Vector3 FindAveragePosition()
        {
            Vector3 averagePos = new Vector3();
            int numTargets = 0;

            for (int i = 0; i < targets.Length; i++)
            {
                if (!targets[i].gameObject.activeSelf)
                    continue;

                averagePos += targets[i].position;
                numTargets++;
            }

            if (numTargets > 0)
                averagePos /= numTargets;

            averagePos.y = 0;

            return averagePos;
        }
    }
}

