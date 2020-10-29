using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DeathMatch : MonoBehaviour
{

    [SerializeField]
    Sprite enemySprite;

    [SerializeField]
    DeathMatchPlayer pfDrone;

    [SerializeField]
    DeathMatchPlayer pfPlayer;

    [SerializeField]
    GameObject gameOverUI;

    [SerializeField]
    public Window_Pointer pointerUI;

    [SerializeField]
    public Window_HealthBar healthBarUI;

    [SerializeField]
    GameObject healthLabel;

    [SerializeField]
    GameObject killsLabel;
    public int NumOfDrones = 0;

    public List<DeathMatchPlayer> drones;
    public List<DeathMatchPlayer> players;
    private bool isVictory = false;
    private bool gameIsOver = false;
    private int playerKills = 0;

    private Settings settings;

    void Awake()
    {
        settings = TagResolver.i.settings;

        if (NumOfDrones == 0)
        {
            NumOfDrones = UnityEngine.Random.Range(settings.enemiesMinCount, settings.enemiesMaxCount);
        }
        drones = new List<DeathMatchPlayer>();
        players = new List<DeathMatchPlayer>();
        SpawnLocalPlayer();
        SpawnDrones(NumOfDrones);
        gameOverUI.SetActive(false);
    }
    void FixedUpdate()
    {
        if (GetAliveEnemiesCount() == 0)
        {
            if (!isVictory)
            {
                isVictory = true;
                ShowGameOver(1);
            }
        }
        updatePlayerHealth(players[0].GetCurrentHealth(), players[0].GetMaxHealth());
        if (players.Count == 0) return;
        if (players[0].GetWarrior() == null) return;
        if (players[0].GetWarrior().isDead)
        {
            if (!gameIsOver)
            {
                gameIsOver = true;
                ShowGameOver(0);
            }
        }
    }

    public int GetAliveEnemiesCount()
    {
        var aliveCount = 0;
        for (var i = 0; i < drones.Count; i++)
        {
            if (!drones[i].GetComponent<Warrior>().isDead)
            {
                aliveCount++;
            }
        }
        return aliveCount;
    }

    public void ShowGameOver(int condition)
    {
        gameOverUI.SetActive(true);
        switch (condition)
        {
            case 0:
                gameOverUI.GetComponentInChildren<Text>().text = "Game Over";
                break;
            case 1:
                gameOverUI.GetComponentInChildren<Text>().text = "You Win";
                break;
            default:
                gameOverUI.GetComponentInChildren<Text>().text = "Lol";
                break;
        }
    }

    public void SpawnLocalPlayer()
    {
        Vector3 pos = new Vector3(0, 3, 0);
        Quaternion rot = Quaternion.LookRotation(new Vector3(10, 0, 0));
        DeathMatchPlayer player = GameObject.Instantiate(pfPlayer, pos, rot);
        pointerUI.SetPlayer(player.transform);
        WarriorHealth health = player.gameObject.GetComponent<WarriorHealth>();
        healthBarUI.SubscribeOnWarriorHealth(health);
        health.Show();
        Camera.main.GetComponent<CameraController>().SetTarget(player.gameObject);
        players.Add(player);
    }

    public void SpawnDrones(int count)
    {
        List<PointerTarget> pointerTargets = new List<PointerTarget>();
        for (var i = 0; i < count; i++)
        {
            Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-40, 40), 0, UnityEngine.Random.Range(-40, 40));
            Quaternion randomRotation = Quaternion.LookRotation(new Vector3(UnityEngine.Random.Range(-40, 40), 4, UnityEngine.Random.Range(-40, 40)));
            DeathMatchPlayer drone = GameObject.Instantiate(pfDrone, randomPosition, randomRotation);
            drone.name = "Drone";
            drone.gameObject.SetActive(true);
            drones.Add(drone);
            pointerTargets.Add(new PointerTarget(drone.transform, enemySprite));
            WarriorHealth health = drone.gameObject.GetComponent<WarriorHealth>();
            healthBarUI.SubscribeOnWarriorHealth(health);
            health.Show();
        }
        pointerUI.SetTargets(pointerTargets);
    }

    public void RestartMatch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void updatePlayerHealth(float current, float total)
    {
        Text text = healthLabel.GetComponent<Text>();
        text.text = "Health: " + current + " / " + total;
    }

    public void IncrementPlayerKills()
    {
        playerKills += 1;
        updatePlayerKills(playerKills);
    }

    private void updatePlayerKills(int killed)
    {
        Text text = killsLabel.GetComponent<Text>();
        text.text = "kills: " + killed + " / " + NumOfDrones;
    }
}
