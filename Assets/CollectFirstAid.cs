using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectFirstAid : MonoBehaviour
{
    private int firstaid = 0;
    public Text firstaidText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "FirstAid")
        {
            firstaid++;
            firstaidText.text = "First Aid: " + firstaid.ToString();
            Debug.Log("First Aid Collected: " + firstaid);
            Destroy(other.gameObject);
        }
    }
}
