using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CreatureType
{
    Player, Drone
}
public class Creature : MonoBehaviour
{
    public CreatureType CreatureType => _type;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float turnSpeed = 6f;


    [SerializeField]
    private CreatureType _type;

}
