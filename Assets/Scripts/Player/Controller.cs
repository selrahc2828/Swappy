using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour
{
    private Controls controls;

    [Header("Scripts Reference")]
    public ComponentStealer stealPasteSript;
    public GrabObject carryingScript;

    [Header("Properties")]
    public Transform orientation;
    [Tooltip("Frottement sur le rigidbody, ça le ralenti")]
    public float groundDrag;
    public float gravityForceY = 5f;

    [Header("SlowTime")]
    [Range(0,1)]
    public float slowCoeff;
    public bool slowTimerActive;
    public float slowTimeDuration;

    [Header("Walk")]
    public float moveSpeed;
    public float maxSpeed;

    [Header("Jump")]
    public float jumpForce;
    [Tooltip("Réduction de la force appliquée dans les air (0 à 1 )")]
    [Range(0,1)]
    public float airModifier;
    // cooldown ?
    // et bool pour vérif en plus

    [Header("GroundCheck")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask floorMask;

    [Header("Debug")]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI timerSlowText;

    private Rigidbody _rb;
    private Vector2 moveInputVector;   
    private Vector3 moveDir;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        controls = GameManager.controls;
        // assigne action du controleur au methode
        controls.Player.StopTime.performed += StopTime;
        controls.Player.CopySteal.performed += CopyStealComp;
        controls.Player.PasteSteal.performed += PasteComp;
        controls.Player.PasteMe.performed += PasteAtMe;
        controls.Player.Jump.performed += Jump;
        controls.Player.GrabDrop.performed += GrabAndDrop;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.Instance.slowTimerActive = slowTimerActive;
        GameManager.Instance.slowTimeDuration = slowTimeDuration;

        timerSlowText.text = slowTimeDuration.ToString();

    }

    private void OnDisable()
    {
        controls.Player.StopTime.performed -= StopTime;
        controls.Player.CopySteal.performed -= CopyStealComp;
        controls.Player.PasteSteal.performed -= PasteComp;
        controls.Player.Jump.performed -= Jump;
        controls.Player.GrabDrop.performed -= GrabAndDrop;
    }

    void Update()
    {
        speedText.text = _rb.velocity.magnitude.ToString();

        if (timerSlowText != null)
        {
            timerSlowText.text = (slowTimeDuration - GameManager.Instance.slowTimer).ToString();
        }
    }

    private void FixedUpdate()
    {
        moveInputVector = controls.Player.Movement.ReadValue<Vector2>().normalized;//input
        handleMovement();
        handleGravity();

        if (Grounded())
        {
            _rb.drag = groundDrag;//applique un "frottement" par defaut au sol
            _rb.useGravity = false;
        }
        else
        {
            _rb.drag = 0f;
            _rb.useGravity = true;
        }
    }

    private void StopTime(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // if bool == true set false et vice versa
            GameManager.Instance.slowMotion = !GameManager.Instance.slowMotion;
            GameManager.Instance.StopTime(GameManager.Instance.slowMotion, slowCoeff);
        }
    }

    private void CopyStealComp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (stealPasteSript != null)
            {
                stealPasteSript.CopyStealComp();
            }
        }
    }

    private void PasteComp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (stealPasteSript != null)
            {
                stealPasteSript.PasteComp();
            }
        }
    }

    private void PasteAtMe(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (stealPasteSript != null)
            {
                stealPasteSript.PasteAtMe();
            }
        }
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("jump");
            if (Grounded())//marche po
            {
                //reset
                _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                //isGrounded = false;
            }
        }
    }
    void GrabAndDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (carryingScript)
            {
                if (carryingScript.isCarrying)
                {
                    carryingScript.Drop();
                }
                else
                {
                    carryingScript.Carrying();

                }

            }
        }
    }

    void handleMovement()
    {

        moveDir = orientation.forward * moveInputVector.y + orientation.right * moveInputVector.x;//orientation

        if (Grounded())
        {
            _rb.AddForce(moveDir.normalized * moveSpeed, ForceMode.Force);// * 10f
        }
        else
        {
            //in air
            _rb.AddForce(moveDir.normalized * moveSpeed  * airModifier, ForceMode.Force); //* 10f
        }

        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed);

        //faire un slope / pente ?
    }

    void handleGravity()
    {
        //Debug.Log(rb.velocity.y);

        if (!Grounded())
        {
            _rb.velocity -= Vector3.down * (Physics.gravity.y * (gravityForceY) * Time.deltaTime);
        }
    }

    public bool Grounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, floorMask);
    }

    void OnDrawGizmos()
    {
        if (groundCheck)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

            Gizmos.DrawRay(transform.position, moveDir.normalized * 15);
            
            Gizmos.color = Color.green;
            if (_rb)
            {
                Gizmos.DrawRay(transform.position, _rb.velocity.normalized * 15);
            }

        }
    }
}
