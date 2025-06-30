using UnityEngine;

public class DeclinedDialog : Dialog
{ 
    public void OnOK()
    {
        this.gameObject.SetActive(false);
    }
}
