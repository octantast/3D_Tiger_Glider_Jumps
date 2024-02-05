using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GeneralController : MonoBehaviour
{
    public PlayerController player;
    public UI ui;

    public bool gliderisactive;
    public GameObject gliderobj;

    public GameObject shield;

    public List<int> intsforlevel;
    public List<Material> allMaterials;
    public List<MeshRenderer> allTilesOnField;
    public List<GameObject> allTilesOnFieldGo;
    public Color32 whiteActivated;
    public List<Color32> allColors; // all possible colors
    public List<Image> targetImagesOfLevel; // holders
    public List<string> allStrings;// all possible strings

    public int thisLevelTargetCount; // how many platforms to activate
    public List<string> targetStringsOfLevel; // strings on current level

    public bool cloudsAllowed;
    public List<GameObject> clouds;
    public GameObject cloudHolder;
    public float spawnInterval;
    private float timercloud;
    private float lastX = -12;

    public bool spikesAllowed;
    public GameObject spikes;
    public int howOftenSpikes;

    private Transform targetPoint;
    private GameObject closestRedObject;

    public int currentLevel;
    public string currentString;

    public int numberOfTargets;

    public bool paused;

    public GameObject arrowa2;
    public TMP_Text arrowText;

    // effects
    public List<ParticleSystem> effects;

    public void Update()
    {
        if (!paused)
        {
            if (cloudsAllowed)
            {
                timercloud += Time.deltaTime;

                if (timercloud >= spawnInterval)
                {
                    SpawnRandomPrefab();
                    timercloud = 0f;
                }
            }

            if (arrowa2.activeSelf)
            {
                if(player.transform.position.z - closestRedObject.transform.position.z > 6)
                {
                    a2activated();
                }

                Vector2 object1XZ = new Vector2(player.animator.gameObject.transform.position.x, player.animator.gameObject.transform.position.z);
                Vector2 object2XZ = new Vector2(closestRedObject.transform.position.x, closestRedObject.transform.position.z);

                //if (player.animator.gameObject.transform.position.x - closestRedObject.transform.position.x < 3 && player.animator.gameObject.transform.position.z - closestRedObject.transform.position.z < 3)

                if (Vector2.Distance(object1XZ, object2XZ) + Vector2.Distance(object1XZ, object2XZ) < 5f)
                {
                    arrowText.text = "Jump now!";
                }
                arrowa2.transform.LookAt(new Vector3(closestRedObject.transform.position.x, 0, closestRedObject.transform.position.z));
            }
            else
            {
                arrowText.text = "";
            }
        }

    }

    public void a2activated()
    {
        closestRedObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in allTilesOnFieldGo)
        {
            if (obj.CompareTag(currentString))
            {
                float distanceToTarget = Vector3.Distance(player.animator.gameObject.transform.position, obj.transform.position);
                if (distanceToTarget < closestDistance && obj.transform.position.z - player.animator.gameObject.transform.position.z > 5 && obj.transform.childCount == 0)
                {
                    closestRedObject = obj;
                    closestDistance = distanceToTarget;
                }
            }
        }
        arrowa2.transform.LookAt(new Vector3(closestRedObject.transform.position.x, 0, closestRedObject.transform.position.z));
        arrowa2.SetActive(true);
    }

    private void SpawnRandomPrefab()
    {
        lastX = lastX * -1;
        spawnInterval += 5;
        float randomZ = Random.Range(player.transform.position.z+20, 50f);
        GameObject prefabToSpawn = Instantiate(clouds[Random.Range(0, clouds.Count)], transform.position, Quaternion.identity, cloudHolder.transform);
        prefabToSpawn.transform.localPosition = new Vector3(lastX, 6, randomZ);
        prefabToSpawn.GetComponent<CloudController>().general = this;
    }

    public void a2off()
    {
        arrowText.text = "";
        ui.a2active = false;
        arrowa2.SetActive(false);
    }

    public void colorTapped(string color, GameObject platform)
    {
            ui.sounds[6].Play();
            if (color == currentString)
            {
            // good!
            a2off();
                // next
                if (currentLevel != thisLevelTargetCount - 1)
                {
                    platform.transform.localPosition += new Vector3(0, -0.05f, 0);
                    platform.tag = "Ground";
                    targetImagesOfLevel[currentLevel].gameObject.SetActive(false);
                currentLevel += 1;
                    targetImagesOfLevel[currentLevel].gameObject.SetActive(true);
                    currentString = targetStringsOfLevel[currentLevel];
                }
                else if(!ui.loseScreen.activeSelf && !ui.winScreen.activeSelf)
                {
                    ui.win();
                }
               
                platform.GetComponent<MeshRenderer>().material.color = whiteActivated;
            }
            else
            {
                // nope



            }
        }
    
}
