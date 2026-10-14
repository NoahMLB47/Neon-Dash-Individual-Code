using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private PlayerInputManager inputManager;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private PlayerSelectionIndicatorManager indicatorManager;
    [SerializeField] private ContinueActivation activation;

    private int selectedId;
    private int currentPlayers;
    private InputDevice lastSelected;
    private int selectedPlayers;
    private mode gameMode;
    private bool devModeActive = false;

    // structure to store PlayerSlot info to be transferred to other scenes
    public struct PlayerSlot
    {
        public bool joined;
        public bool hasSelected;
        public InputDevice[] controller;
        public int selectedId;
        public GameObject pointerObject;
    }
    public PlayerSlot[] playersInfo = new PlayerSlot[4];

    // stores the game modes
    public enum mode {
        Lives,
        Timer,
        Practice
    } 

    void Awake()
    {
        // makes this class a singleton
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
        currentPlayers = 0;
        selectedPlayers = 0;
        gameMode = mode.Lives;
    }

    public int getId()
    {
        return selectedId;
    }

    // the array of PlayerSlots is reset every time the character select scene is entered
    public void resetArray()
    {
        for (int i = 0; i < 4; i++)
        {
            playersInfo[i].joined = false;
            playersInfo[i].hasSelected = false;
            playersInfo[i].controller = null;
            playersInfo[i].selectedId = 999;
            playersInfo[i].pointerObject = null;
        }
    }

    public PlayerSlot[] getArray()
    {
        return playersInfo;
    }

    public int getCurrentPlayers()
    {
        return currentPlayers;
    }

    public int getSelectedPlayers()
    {
        return selectedPlayers;
    }

    public mode getGameMode()
    {
        return gameMode;
    }

    public void setGameMode(mode mode)
    {
        gameMode = mode;
    }

    public void setDevMode(bool isDevMode)
    {
        devModeActive = isDevMode;
    }

    public bool getDevMode()
    {
        return devModeActive;
    }
}
