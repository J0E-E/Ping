using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField] private GameObject modalBackdrop;
    public void Show()
    {
        Debug.Log($"Showing {gameObject.name}");
        if (modalBackdrop != null)
        {
            modalBackdrop.SetActive(true);
        }
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log($"Hiding {gameObject.name}");
        if (modalBackdrop != null)
        {
            modalBackdrop.SetActive(false);
        };
        gameObject.SetActive(false);
    }
}
