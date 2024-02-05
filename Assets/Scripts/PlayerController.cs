using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GeneralController general;   
    public Animator animator;
    public Rigidbody thisBody;
    public CharacterController characterController;
    public FixedJoystick variableJoystick;

    public Vector2 touchPos;
    public Vector2 swipeStartPos;

    public bool blocked;
    public bool touchbegan;
    public bool touchcontinues;

    private Vector3 moveDirection = Vector3.zero;
    public float gravity;
    public float gravityMax;
    public float gravityMin;
    public bool ground;
    public GameObject playergraphics;
    public float rotationSpeed;
    public float jumpForce;

    private float targetRotationX;
    public float lerpSpeed;

    public float speed;
    public float horizontal;
    public float vertical;

    private bool jumped;
    private bool blockdone;
    private bool groundonce;
    private bool groundonce1;

    public float holdingSensTimer;

    public float swipeSensitivity;

    public bool touchjoystic;
    private Vector2 joystickStartPosition;
    public Camera playercamera;

    private void Start()
    {
        gliderOff();
    }

    public void gliderOff()
    {
        lerpSpeed = 25;
        targetRotationX = 0;
        gravity = -9.8f;
        general.gliderisactive = false;
        variableJoystick.gameObject.SetActive(false);
        general.gliderobj.SetActive(false);
    }
    public void gliderOn()
    {
        lerpSpeed = 1;
        targetRotationX = 90;
        moveDirection.y = 0f;
        variableJoystick.gameObject.SetActive(true);
        general.gliderobj.SetActive(true);
        gravity = gravityMin;
        if (general.ui.tutorial2 == 0)
        {
            general.ui.tutorial2 = 1;
            PlayerPrefs.SetInt("tutorial2", 1);
            PlayerPrefs.Save();
            general.a2activated();
            general.ui.tipAnimator.Play("Tutor3");
            general.ui.tutorialHand.gameObject.SetActive(true);
            general.ui.tutorialHand.Play("Tutorial3");

        }
    }


    // touch input
    public void Update()
    {
        if (!general.paused)
        {
            touchInput();

            //if (!touchjoystic)
            //{
            //    Quaternion targetRotation = Quaternion.Euler(playercamera.transform.eulerAngles.x, 0f, 0);
            //    playercamera.transform.rotation = Quaternion.Slerp(playercamera.transform.rotation, targetRotation, 1 * Time.deltaTime);
            //}

            if(transform.position.y > 5f && general.ui.tutorial1 == 0)
            {
                    general.ui.tutorial1 = 1;
                    PlayerPrefs.SetInt("tutorial1", 1);
                    PlayerPrefs.Save();
                moveDirection.y = 0;
                gravity = 0;
                general.ui.tutorialHand.gameObject.SetActive(false);
                    general.ui.tipAnimator.Play("Tutor2");
                
            }

            if (variableJoystick.gameObject.activeSelf && (variableJoystick.Vertical != 0 || variableJoystick.Horizontal != 0))
            {
                vertical = variableJoystick.Vertical;
                horizontal = variableJoystick.Horizontal;
                if (vertical < 0)
                {
                    vertical = 0;
                }
               else
                { playergraphics.transform.rotation = Quaternion.Lerp(playergraphics.transform.rotation, Quaternion.Euler(targetRotationX, playergraphics.transform.rotation.eulerAngles.y, 0), lerpSpeed * Time.deltaTime); }
            }
            else if (variableJoystick.gameObject.activeSelf)
            {
                vertical = 0;
                horizontal = 0;
                playergraphics.transform.rotation = Quaternion.Lerp(playergraphics.transform.rotation, Quaternion.Euler(targetRotationX, playergraphics.transform.rotation.eulerAngles.y, 0), lerpSpeed * Time.deltaTime);
              
            }
            else
            {
                vertical = 0;
                horizontal = 0;
                Vector3 currentRotation = playergraphics.transform.rotation.eulerAngles;
                playergraphics.transform.rotation = Quaternion.Lerp(playergraphics.transform.rotation, Quaternion.Euler(targetRotationX, playergraphics.transform.rotation.eulerAngles.y, 0), lerpSpeed * Time.deltaTime);
            }


            if (ground && moveDirection.y < 0)
            {
                moveDirection = Vector3.zero;
            }
           

            Vector3 move = new Vector3(horizontal * speed, 0, vertical * speed);
            characterController.Move(move);

            if (move != Vector3.zero)
            {
                playergraphics.transform.forward = Vector3.Lerp(playergraphics.transform.forward, move, rotationSpeed * Time.deltaTime);
            }

            moveDirection.y += gravity * Time.deltaTime;
            characterController.Move(moveDirection);
                       
            animator.SetBool("ground", ground);
        }
    }

   

    public void CloudCollision(GameObject other, Vector3 position)
    {
        if (!general.shield.activeSelf && !general.ui.loseScreen.activeSelf && !general.ui.winScreen.activeSelf)
        {
            general.effects[0].gameObject.transform.position = position;
            general.effects[0].Play();
            general.ui.sounds[5].Play();
            general.ui.lose();
        }
        else
        {
            Destroy(other.gameObject);
            general.shield.SetActive(false);
            general.ui.sounds[4].Play();
            general.effects[1].gameObject.transform.position = position;
            general.effects[1].Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Koluchka")
        {
            if (!general.shield.activeSelf)
            {
                general.effects[0].gameObject.transform.position = collision.contacts[0].point;
                general.effects[0].Play();
                general.ui.sounds[5].Play();
                general.ui.lose();
            }
            else
            {
               Destroy(collision.gameObject);
                general.shield.SetActive(false);
                general.ui.sounds[4].Play();
                general.effects[1].gameObject.transform.position = collision.contacts[0].point;
                general.effects[1].Play();
            }
        }
        else if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Untagged" && collision.gameObject.tag != "Cloud")
        {
            // platform
            if (collision.gameObject.tag != "Ground")
            {
                if (!groundonce1)
                {
                    Debug.Log(collision.gameObject.tag);
                    groundonce1 = true;
                    general.effects[0].gameObject.transform.position = collision.contacts[0].point;
                    general.effects[0].Play();
                    general.colorTapped(collision.gameObject.tag, collision.gameObject);
                }
            }
            // ground
            else
            {
                if (!groundonce)
                {
                    Debug.Log(collision.gameObject.tag);
                    groundonce = true;
                    general.effects[0].gameObject.transform.position = collision.contacts[0].point;
                    general.effects[0].Play();
                    general.ui.sounds[3].Play();
                }
            }
            ground = true;
            jumped = false;
            gliderOff();
        }
        
        
    }
    public void touchInput()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchPos = Input.mousePosition;
                swipeStartPos = touchPos;
                startTouch();
            }
            if (Input.GetMouseButton(0))
            {
                touchPos = Input.mousePosition;
                continueTouch();
            }
            if (Input.GetMouseButtonUp(0))
            {
                endTouch();
            }

        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:

                    touchPos = touch.position;
                    swipeStartPos = touchPos;
                    startTouch();
                    break;
                case TouchPhase.Moved:
                    touchPos = touch.position;
                    continueTouch();
                    break;
                case TouchPhase.Ended:
                    endTouch();

                    break;

            }
        }
    }

    public void touchedJoystic()
    {
        touchjoystic = true;
        if (general.ui.tutorial3 == 0)
        {
            general.ui.tutorial3 = 1;
            PlayerPrefs.SetInt("tutorial3", 1);
            PlayerPrefs.Save();

            general.ui.tutorialHand.gameObject.SetActive(true);
            general.ui.tutorialHand.Play("Tutorial4");
            general.ui.tipAnimator.Play("Tutor4");
        }
        else if (general.ui.tutorial4 != 0 && general.ui.tutorial5 == 0)
        {
            general.ui.tutorialHand.enabled = false;
              general.ui.tutorialHand.gameObject.SetActive(false);
            general.ui.tutorial5 = 1;
            PlayerPrefs.SetInt("tutorial5", 1);
            PlayerPrefs.Save();
            general.ui.tipAnimator.enabled = false;
        }
    }

    public void startTouch()
    {
        if (!general.paused)
        {
            if (!blocked)
            {
                touchbegan = true;
                touchcontinues = false;
                if (!general.gliderobj.activeSelf && jumped && !ground)
                {
                    gliderOn();
                }
            }
            //else
            //{
            //    blocked = false;
            //}
        }

    }
    public void continueTouch()
    {
        if (!general.paused && !blocked)
        {
            touchbegan = false;
            touchcontinues = true;

            //if (touchjoystic && Input.touchCount > 2)
            //{
            //    Vector2 swipeDelta = (Vector2)touchPos - swipeStartPos;

            //    float rotationAmount = swipeDelta.x * 1 * Time.deltaTime;
            //    transform.Rotate(Vector3.up, rotationAmount);

            //    float currentRotation = Mathf.Clamp(playercamera.transform.eulerAngles.y, -30, 30);
            //    playercamera.transform.eulerAngles = new Vector3(playercamera.transform.eulerAngles.x, currentRotation, 0);

            //    joystickStartPosition = touchPos;
            //}
            
        }
        else if(blocked)
        {
            blocked = false;
            touchjoystic = true;
        }
    }


    public void endTouch()
    {
        if (!general.paused)
        {
            if (!blocked)
            {
                touchbegan = false;
                touchcontinues = false;
                float swipeDelta = touchPos.y - swipeStartPos.y;
                if (swipeDelta > 0 && !jumped && !blockdone && ground && !touchjoystic)
                {
                    general.ui.sounds[2].Play();
                    animator.Play("Jump");
                    jumped = true;
                    ground = false;
                    gravity = -2;
                    moveDirection.y += Mathf.Sqrt(jumpForce);
                    groundonce = false;
                    groundonce1 = false;

                  
                }
                else if(swipeDelta <= 0 && general.gliderisactive && !touchjoystic)
                {
                    jumped = false;
                    animator.Play("Jump2");
                    Debug.Log("swipe down");
                    gliderOff();
                    if (general.ui.tutorial4 == 0)
                    {
                        general.ui.tutorial4 = 1;
                        PlayerPrefs.SetInt("tutorial4", 1);
                        PlayerPrefs.Save();

                        general.ui.tutorialHand.gameObject.SetActive(true);
                        general.ui.tutorialHand.Play("Tutorial5");
                        general.ui.tipAnimator.Play("Tutor5");
                    }
                }
                else if(general.gliderobj.activeSelf && !general.gliderisactive)
                {
                    general.gliderisactive = true;
                }
                touchjoystic = false;
            }
        }
    }


}
