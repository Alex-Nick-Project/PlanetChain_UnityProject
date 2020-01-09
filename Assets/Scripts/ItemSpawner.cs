using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] itemPrefabs = null; // array of empthy GO that spawn a variable of that polygon

    //[SerializeField]
    //private star[] stars;

    [SerializeField]
    private float maxDistance; // max render distance of camera

    private void Awake()
    {
        //maxDistance = Camera.main.farClipPlane;
    }

    private void Start()
    {
        //stars = FindObjectsOfType<Star>();   
    }
    public GameObject updateRooms(int amountOfMaxPlayer)
    {
        // gives a random position in a sphere around this gameobject
        Vector3 position = Random.insideUnitSphere * maxDistance;
        Vector3 pos = position + Random.onUnitSphere * maxDistance;

        itemPrefabs[amountOfMaxPlayer-1].transform.localScale = new Vector3(1, 1, 1) /* * Random.Range(1f, maxSize) */;
        var _planet = Instantiate(itemPrefabs[amountOfMaxPlayer-1], pos, Quaternion.identity, gameObject.transform /*stars[Random.Range(0, stars.length)].transform*/);
        //x.transform.localScale = Vector3.one*Random.Range(minSize, maxSize);
        return _planet;
    }
}
