using UnityEngine;
using UnityEngine.UI;
public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;
    void Start()
    {
       ActivateTab(0);
    }

    public void ActivateTab(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            // tabImages[i].color = Color.grey;
            tabImages[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        pages[index].SetActive(true);
        // tabImages[index].color = Color.white;
        tabImages[index].color = new Color(1, 1, 1, 1);
    }

}
