using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private SelectManager select;
    private RectTransform tr;
    private RectTransform canvasTr;
    private Vector2 movementVector;
    private Image sr;
    private Vector2 outPos;
    private PlayerInput pi;
    private Vector2 screenPoint;
    private PointerEventData ped;
    private GraphicRaycaster gr;

    void Awake()
    {
        transform.SetParent(FindAnyObjectByType<Canvas>().transform);
        tr = GetComponent<RectTransform>();
        canvasTr = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        sr = GetComponent<Image>();
        pi = GetComponent<PlayerInput>();
        gr = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tr.anchoredPosition = Vector2.zero;
        select = FindFirstObjectByType<SelectManager>();
    }

    // Update is called once per frame
    void Update()
    {

        // moves the pointer to the position of the mouse cursor
        if (pi.user.pairedDevices[0] is Keyboard || pi.user.pairedDevices[0] is Mouse)
        {
            //tr.anchoredPosition = Mouse.current.position.ReadValue();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTr, Mouse.current.position.ReadValue(), null, out outPos);
            tr.anchoredPosition = outPos;
        }

        // moves the pointer in the direction given by joystick movement
        else
        {
            tr.anchoredPosition += movementVector * speed * Time.deltaTime;
        }
    }

    public void onMove(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }

    public void onConfirm(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            screenPoint = RectTransformUtility.WorldToScreenPoint(null, tr.position);
            ped = new PointerEventData(EventSystem.current) { position = screenPoint };

            // makes a raycast of the objects around the pointer
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            for (int i = 0; i < results.Count; i++)
            {
                // if one of them is a button, execute the action of the button
                if (results[i].gameObject.GetComponent<Button>())
                {
                    if (SceneManager.GetActiveScene().name == "CharacterSelect")
                        select.setLastSelected(gameObject.GetComponent<PlayerInput>().devices[0]);
                    ExecuteEvents.Execute(results[i].gameObject, ped, ExecuteEvents.pointerClickHandler);
                }
            }  
        }
    }

    // change the sprite of the pointer based on the player number
    public void changeSprite(int id)
    {   
        if (id == 0)
        {
            if (sr == null)
            {
                sr = GetComponent<Image>();
            }
            sr.sprite = Resources.Load<Sprite>("Player1Pointer");
        }

        if (id == 1)
        {
            if (sr == null)
                sr = GetComponent<Image>();
            sr.sprite = Resources.Load<Sprite>("Player2Pointer");
        }
        
        if (id == 2)
        {
            if (sr == null)
            {
                sr = GetComponent<Image>();
            }
            sr.sprite = Resources.Load<Sprite>("Player3Pointer");

        }

        if (id == 3)
        {
            if (sr == null)
            {
                sr = GetComponent<Image>();
            }
            sr.sprite = Resources.Load<Sprite>("Player4Pointer");
        }
    }
}
