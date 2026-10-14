using System;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class SelectManager : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    GameManager.PlayerSlot[] array;
    [SerializeField] private ContinueActivation activation;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private PlayerSelectionIndicatorManager indicatorManager;
    [SerializeField] private TMP_Text devModeText;
    public TMP_Text ModeTextBox;
    private InputDevice lastSelected;
    private int selectedPlayers;
    private int currentPlayers;
    private bool isTransitioning;
    private bool devMode = false;

    void Start()
    {
        if (devModeText != null)
            devModeText.gameObject.SetActive(false);
    }

    void Awake()
    {
        array = GameManager.instance.getArray();
        GameManager.instance.resetArray();
        Time.timeScale = 1.0f;
        isTransitioning = false;
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame)
        {
            ToggleDevMode();
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
       // searches for the first slot number that hasn't been joined
        for (int i = 0; i < 4; i++)
        {
            if (!array[i].joined)
            {
                array[i].joined = true;

                // create pointer and change sprite
                array[i].pointerObject = playerInput.gameObject;
                array[i].pointerObject.transform.SetParent(canvasTransform);
                array[i].pointerObject.GetComponent<PointerController>().changeSprite(i);


                // assign input to controller
                array[i].controller = playerInput.devices.ToArray();

                currentPlayers += 1;
                // should start button be displayed?
                activation.check();

                break;

            }
        }
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        if (isTransitioning)
            return;
        
        // resets their array slot
        int index = Array.FindIndex(array, slot => slot.pointerObject == playerInput.gameObject);

        if (array[index].hasSelected)
            selectedPlayers -= 1;
        currentPlayers -= 1;

        array[index].joined = false;        
        array[index].hasSelected = false;
        array[index].controller = null;
        array[index].selectedId = 999;
        array[index].pointerObject = null;
        indicatorManager.deleteIndicator(index);
    }

    // destroys the player object to trigger OnPlayerLeft when leave button is pressed
    public void DestroyPlayerObject()
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].controller != null)
            {
                for (int j = 0; j < array[i].controller.Length; j++)
                {
                    if (lastSelected == array[i].controller[j])
                    {
                        if (array[i].pointerObject != null)
                            Destroy(array[i].pointerObject);
                    }
                }
            }
        }
    }

    public void setID(int id)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].controller != null)
            {
                for (int j = 0; j < array[i].controller.Length; j++)
                {
                    if (lastSelected == array[i].controller[j])
                    {
                        // sets the id of the selected character to the PlayerSlot the lastSelected input device is in
                        array[i].selectedId = id;
                        indicatorManager.spawnIndicator(id, i);
                        // tells the system that that player has selected a character to check for start button activation
                        if (!array[i].hasSelected)
                        {
                            selectedPlayers += 1;
                            array[i].hasSelected = true;
                            activation.check();
                        }
                    }
                }
            }
        }
    }

    public void setLastSelected(InputDevice device)
    {
        lastSelected = device;
    }

    public int getCurrentPlayers()
    {
        return currentPlayers;
    }

    public int getSelectedPlayers()
    {
        return selectedPlayers;
    }

    public void setTransitioning()
    {
        isTransitioning = true;
    }

    // changes the game mode based on what button the player presses
    public void setMode(int value)
    {
        if (value == 0)
        {
            GameManager.instance.setGameMode(GameManager.mode.Lives);
            ModeTextBox.text = ("Mode: Lives");
        }

        else if (value == 1)
        {
            GameManager.instance.setGameMode(GameManager.mode.Timer);
            ModeTextBox.text = ("Mode: Timer");
        }

        else if (value == 2)
        {
            GameManager.instance.setGameMode(GameManager.mode.Practice);
            ModeTextBox.text = ("Mode: Practice");
        }
    }

    private void ToggleDevMode()
    {
        devMode = !devMode;
        GameManager.instance.setDevMode(devMode);
        
        if (devModeText != null)
        {
            devModeText.gameObject.SetActive(devMode);
            if (devMode)
            {
                devModeText.text = "DEV MODE ACTIVE\nPress F1 to disable";
                devModeText.color = Color.green;
            }
        }

        activation.check();
    }

    public bool IsDevMode()
    {
        return devMode;
    }
}


