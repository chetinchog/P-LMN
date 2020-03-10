using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iCTG_PlayerNetwork : MonoBehaviour
{
    public static iCTG_PlayerNetwork Instance;
    public string PlayerName { get; private set; }

    private void Awake()
    {
        // Set the instance
        Instance = this;

        // Set the name
        PlayerName = "iCTG#" + Random.Range(1000, 9999);
    }
}
