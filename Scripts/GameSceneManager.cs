using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    private GameObject[] player = new GameObject[4];
    [SerializeField] private SpawnPoints points;
    [SerializeField] private WinScreen winScreen;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject dummyEnemyPrefab;
    GameManager instance;
    private int id;
    private int alivePlayers;
    private int lastPlayer;
    private GameObject dummyEnemy;

    
    void Awake()
    {
        alivePlayers = 0;
    }
    void Start()
    {
        spawnPlayers();
        // checks if the time game mode is selected
        if (GameManager.instance.getGameMode() == GameManager.mode.Timer)
        {
            timer.SetActive(true);
        }

        // spawns dummy enemy if dev mode is on
        if (GameManager.instance.getDevMode())
        {
            SpawnDummyEnemy();
        }
    }

    public void spawnPlayers()
    {
        instance = GameManager.instance;
        GameManager.PlayerSlot[] array = instance.getArray();

        for (int i = 0; i < 4; i++)
        {
            if (array[i].joined)
            {
                // spawns the character the player selected in the right position relative to their player number amd moves their health and ability bars
                player[i] = Instantiate(characters[array[i].selectedId], points.getPoints()[i], Quaternion.identity);
                player[i].GetComponentInChildren<HealthBar>().moveHealthBar(points.getBarPoints()[i].healthBarSpawn, i);
                player[i].GetComponentInChildren<AbilityBar>().moveAbilityBar(points.getBarPoints()[i].abilityBarSpawn, i);
                alivePlayers += 1;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (array[i].joined)
            {
                // switches their control scheme to the correct one based on their input device
                if (array[i].controller.Length > 1)
                {
                    player[i].GetComponent<PlayerInput>().enabled = true;
                    player[i].GetComponent<PlayerInput>().SwitchCurrentControlScheme("KBM", array[i].controller[0], array[i].controller[1]);
                    player[i].GetComponentInChildren<PlayerInput>().SwitchCurrentControlScheme("KBM", array[i].controller[0], array[i].controller[1]);
                }


                if (array[i].controller[0] is Gamepad)
                {
                    player[i].GetComponent<PlayerInput>().enabled = true;
                    player[i].GetComponent<PlayerInput>().SwitchCurrentControlScheme("Controller", array[i].controller[0]);
                }
            }
        }
    }

    public void losePlayer()
    {
        alivePlayers -= 1;

        // if there is one player left the game checks who it is and activates the win screen
        if (alivePlayers == 1)
        {
            for (int i = 0; i < player.Length; i++)
            {
                if (player[i] != null && player[i].activeSelf)
                {
                    lastPlayer = i + 1;
                    winScreen.livesActivateScreen(lastPlayer);
                    break;
                }
            }
        }
    }

    public void sortKills()
    {
        int max = -999;
        int playerKills = 0;
        int maxPlayerNumber = 0;

        // sorting algorithm for kills
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i] != null)
            {
                playerKills = player[i].GetComponent<HealthNDeath>().getKillsTakeDeaths();
                if (playerKills > max)
                {
                    max = playerKills;
                    maxPlayerNumber = i + 1;
                }
            }
        }

        winScreen.timerActivateScreen(maxPlayerNumber, max);
    }

    private void SpawnDummyEnemy()
    {

        Vector3 dummySpawnPos = points.getPoints()[1];
        dummyEnemy = Instantiate(dummyEnemyPrefab, dummySpawnPos, Quaternion.identity);
        
        dummyEnemy.GetComponentInChildren<HealthBar>().moveHealthBar(points.getBarPoints()[1].healthBarSpawn, 1);
        dummyEnemy.GetComponentInChildren<AbilityBar>().moveAbilityBar(points.getBarPoints()[1].abilityBarSpawn, 1);

        DummyEnemy dummyScript = dummyEnemy.AddComponent<DummyEnemy>();
        dummyScript.SetSpawnPosition(dummySpawnPos);
    }    

}
