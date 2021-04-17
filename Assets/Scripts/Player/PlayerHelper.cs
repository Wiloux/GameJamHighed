using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    public static PlayerHelper instance;
    private void Awake() {instance = this;}

}
