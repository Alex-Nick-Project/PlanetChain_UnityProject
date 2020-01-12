using UnityEngine;

public class RandomizeMesh : MonoBehaviour
{
    // manual setting

    [SerializeField]
    private GameObject[] prefabs = null;

    [SerializeField]
    private GameObject[] rings = null;

    [SerializeField]
    private GameObject[] aster = null;

    // cached references
    [SerializeField]
    int random;
    [SerializeField]
    GameObject ringInstance;
    [SerializeField]
    GameObject asteroidInstance;

    private void Awake()
    {
        // to have all planets interactable
        gameObject.tag = "Planet";
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        // stores the random prefab index
        random = Random.Range(0, prefabs.Length - 1);

        // spawns the prefab ass a child of the spawner
        Instantiate(prefabs[random], transform.position, transform.rotation, gameObject.transform);

        // avoids to pick a null ring/asteroid
        if (rings.Length == 0 && aster.Length == 0)
        {
            return;
        }

        // stores the random ring and asteroid type will be intantiated
        int ringType = Random.Range(0, rings.Length);
        int asteroidType = Random.Range(0, aster.Length);
        int playerCount = transform.GetChild(0).transform.childCount;

        // always spawn ring&asteroid if it's spherical, may appear on other 50% chance
        if (playerCount == 3 || playerCount == 5 || playerCount == 7 || playerCount == 9 || Random.Range(0f, 1f) > 0.5f) 
        {
            var ringRadius = Random.Range(1.5f, 2.5f);
            var ringThickness = Random.Range(1.5f, 2.5f);
            ringInstance = Instantiate(rings[ringType], transform.position, Quaternion.identity, transform);

            PlanetRing PR = ringInstance.GetComponent<PlanetRing>();
            PR.innerRadius = ringRadius;
            PR.thickness = ringThickness;
            PR.segments = 80;

            ringInstance.GetComponent<Rotationary>().isRotationary = false;
            //aster[a].transform.localScale = new Vector3(1, 1, 1)*ranR/0.8f;
            asteroidInstance = Instantiate(aster[asteroidType], transform.position, Quaternion.identity, transform);
            asteroidInstance.AddComponent<Asteroids>();
        }

    }
}


