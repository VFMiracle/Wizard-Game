using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsManager : MonoBehaviour
{
    private int currentPage = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangePage(bool foward)
    {
        if (foward)
        {
            transform.Find("Page " + currentPage).gameObject.SetActive(false);
            currentPage++;
            transform.Find("Page " + currentPage).gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Page " + currentPage).gameObject.SetActive(false);
            currentPage--;
            transform.Find("Page " + currentPage).gameObject.SetActive(true);
        }
        if (currentPage == 4) transform.Find("FowardBtn").GetComponent<Button>().interactable = false;
        else transform.Find("FowardBtn").GetComponent<Button>().interactable = true;
        if (currentPage == 1) transform.Find("BackwardsBtn").GetComponent<Button>().interactable = false;
        else transform.Find("BackwardsBtn").GetComponent<Button>().interactable = true;
    }
}
