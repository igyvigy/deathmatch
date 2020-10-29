using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagResolver : MonoBehaviour
{
    public static TagResolver i
    {
        get
        {
            return Singleton<TagResolver>.Instance;
        }
    }
    public GamepadInputManager inputManager;
    public DeathMatch deathMatch;
    public Settings settings;

    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("input_manager").GetComponent<GamepadInputManager>();
        deathMatch = GameObject.FindGameObjectWithTag("death_match").GetComponent<DeathMatch>();
        settings = GameObject.FindGameObjectWithTag("settings").GetComponent<Settings>();
    }
}
