using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneVsOneManager : MonoBehaviour
{
    public static OneVsOneManager Instance { get; private set; }
    [SerializeField] private GameObject winPannel;
    [SerializeField] private GameObject losePannel;
    void Start()
    {
        Instance = this;
        winPannel.SetActive(false);
        losePannel.SetActive(false);
    }
    public void WinRound()
    {
        Invoke("WinHandle", 1f);
    }
    public void LoseRound()
    {
        Invoke("LoseHandle", 1f);
    }
    private void WinHandle()
    {
        winPannel.SetActive(true);
    }
    private void LoseHandle()
    {
        losePannel.SetActive(true);
    }
    
}
