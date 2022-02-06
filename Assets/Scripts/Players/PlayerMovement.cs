using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public int playerNumber = 1;

    public float moveSpeed;
    public float jumpForce;

    public CharacterController controller;

    public float gravityScale;

    public Animator anim;
    public Transform pivot;
    public float rotateSpeed;

    public GameObject playerModel;

    public GameObject pickaxe;

    private Vector3 moveDirection;
    private Quaternion lastRotation;

    private string movementAxisName;
    private string turnAxisName;
    private string jumpAxisName;

    private float defaultSpeed;

    private bool isDisabled;

    private bool twoPlayerQuestAnim;

    private Vector3 animRotation;


    void Start()
    {
        movementAxisName = "Vertical" + playerNumber;
        turnAxisName = "Horizontal" + playerNumber;
        jumpAxisName = "Jump" + playerNumber;
        controller = GetComponent<CharacterController>();
        defaultSpeed = moveSpeed;
        isDisabled = false;
        twoPlayerQuestAnim = false;
        if (pickaxe != null)
        {
            pickaxe.SetActive(false);
        }
    }

    public void InRiver(bool condition)
    {
        if (condition)
        {
            anim.SetBool("inRiver", condition);
            moveSpeed = 1f;
        }
        else
        {
            anim.SetBool("inRiver", condition);
            moveSpeed = defaultSpeed;
        }
    }

    public void RotateTo(Vector3 animRotation)
    {
        this.animRotation = animRotation - playerModel.transform.position;
        this.animRotation.y = 0;
    }


    void Update()
    {
        if (!isDisabled)
        {

            float yStore = moveDirection.y;
            moveDirection = (transform.forward * Input.GetAxis(movementAxisName)) + (transform.right * Input.GetAxis(turnAxisName));
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;

            if (controller.isGrounded && moveSpeed != 1f)
            {
                moveDirection.y = 0f;
                if (Input.GetButtonDown(jumpAxisName))
                {
                    moveDirection.y = jumpForce;
                }
            }

            moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
            controller.Move(moveDirection * Time.deltaTime);

            if (Input.GetAxis(turnAxisName) != 0 || Input.GetAxis(movementAxisName) != 0)
            {
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }

            anim.SetBool("isGrounded", controller.isGrounded);
            anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis(movementAxisName)) + Mathf.Abs(Input.GetAxis(turnAxisName))));
        }
        else
        {
            if (twoPlayerQuestAnim)
            {
                Quaternion newRotation = Quaternion.LookRotation(animRotation);
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed / 2 * Time.deltaTime);
            }
        }
    }

    public void PlayTwoPlayerQuestAnim()
    {
        DisableInput();
        anim.SetBool("twoPlayerQuest", true);
        twoPlayerQuestAnim = true;
    }

    public void StopTwoPlayerQuestAnim()
    {
        EnableInput();
        anim.SetBool("twoPlayerQuest", false);
        twoPlayerQuestAnim = false;
    }

    public void PlayRockQuestAnim()
    {
        DisableInput();
        pickaxe.SetActive(true);
        anim.SetBool("slashing", true);
        twoPlayerQuestAnim = true;
    }

    public void StopRockQuestAnim()
    {
        EnableInput();
        pickaxe.SetActive(false);
        anim.SetBool("slashing", false);
        twoPlayerQuestAnim = false;
    }

    public void EnableInput()
    {
        isDisabled = false;
    }

    public void DisableInput()
    {
        isDisabled = true;
    }
}

