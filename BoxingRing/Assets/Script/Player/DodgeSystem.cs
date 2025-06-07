using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeSystem : MonoBehaviour
{
    [SerializeField] private float dodgeTime = 1f; //Thời gian không nhận dps
    [SerializeField] private bool isLeftDodge = false; //Có đang né bên trái không (Né trái sẽ không nhận dps đấm phải)
    [SerializeField] private bool isRightDodge = false; //Tương tự né trái nhưng ngược lại
    void Start()
    {

    }
    public void StartDodgeLeft()
    {
        if (!isLeftDodge)
            StartCoroutine(DodgeLeftCoroutine());
    }
    public bool GetDodgeLeft()
    {
        return isLeftDodge;
    }
    public bool GetDodgeRight()
    {
        return isRightDodge;
    }
    
    public void StartDodgeRight()
    {
        if (!isRightDodge)
            StartCoroutine(DodgeRightCoroutine());
    }
    private IEnumerator DodgeLeftCoroutine()
    {
        isLeftDodge = true;
        Debug.Log("Dodging Left Immue Right Hook");
        yield return new WaitForSeconds(dodgeTime);
        isLeftDodge = false;
    }

    private IEnumerator DodgeRightCoroutine()
    {
        isRightDodge = true;
        Debug.Log("Dodging Right Immue Left Hook");
        yield return new WaitForSeconds(dodgeTime);
        isRightDodge = false;
    }
}
