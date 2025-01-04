using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    public GameObject[] objLevels;
    public GameObject[] objObstacle;
    private GameObject objLastLevels;

    public GameObject objHero;
    private GameObject preHero;
    public GameObject startHall;

    private float kecLari = 10f;
    private float faktorPercepatan = 0.05f; 

    private bool levelAwal = true;

    private int score = -160;
    public Text scoreText;

    private int kecepatanThreshold = 100; //kecepatan meningkat tiap skor 100
    private float maxKecepatan = 30f; // kecepatan maksimum

    void Start()
    {
        objLastLevels = this.gameObject;

        // Memunculkan hero
        preHero = Instantiate(objHero, this.transform.position, this.transform.rotation) as GameObject;

        // Memunculkan startHall sekali saat game dimulai
        GameObject preStartHall = Instantiate(startHall, this.transform.position, this.transform.rotation) as GameObject;
        preStartHall.transform.parent = this.transform;
        objLastLevels = preStartHall;

        // Membuat level awal
        for (int a = 0; a <= 15; a++) {
            buat_level();
        }

        UpdateScoreUI();
    }

    void Update()
    {
        if (preHero.GetComponent<Character>().statMulai == true)
        {
            // Peningkatan kecepatan bertahap berdasarkan skor
            if (score >= kecepatanThreshold && kecLari < maxKecepatan)
            {
                kecLari += faktorPercepatan * Time.deltaTime;
                kecepatanThreshold += 100; // Update untuk peningkatan kecepatan berikutnya
            }

            foreach (Transform anak in GameObject.Find("place_levels").transform)
            {
                anak.transform.Translate(new Vector3(0, 0, -kecLari) * Time.deltaTime);

                Vector3 posAnakLevel = Camera.main.WorldToViewportPoint(anak.transform.position);
                if (posAnakLevel.z < -1f)
                {
                    Destroy(anak.gameObject);
                    buat_level();
                }
            }
        }
    }

    void buat_level()
    {
        Vector3 posLevels = objLastLevels.transform.position;
        if (levelAwal == false)
        {
            posLevels.z += objLastLevels.GetComponent<Renderer>().bounds.size.z;
        }
        else
        {
            posLevels.z += startHall.GetComponent<Renderer>().bounds.size.z;
        }
        int randJenisLevel = Random.Range(0, objLevels.Length);
        GameObject preLevels = Instantiate(objLevels[randJenisLevel], posLevels, this.transform.rotation) as GameObject;
        objLastLevels = preLevels.gameObject;
        objLastLevels.transform.parent = this.transform;

        if (levelAwal == false)
        {
            int rand = Random.Range(0, 8);
            if (rand == 0)
            {
                buat_obstacle_barrier(preLevels);
            }
        }
        levelAwal = false;

        score += 10;
        UpdateScoreUI();
    }

    void buat_obstacle_barrier(GameObject tempatLevels)
    {
        int jenisBarrier = Random.Range(2, 4);
        GameObject objObs = Instantiate(objObstacle[jenisBarrier],
        tempatLevels.transform.position, tempatLevels.transform.rotation) as GameObject;
        objObs.transform.parent = tempatLevels.transform;
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;

        // Update the score in the GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateScore(score);
        }
    }

    public int GetScore()
    {
        return score;
    }


}
