using UnityEngine;
public class Asteroids : MonoBehaviour
{
    GameObject x;
    ParticleSystem PS;
    void Start()
    {
        PS = GetComponent<ParticleSystem>();
        x = transform.parent.GetChild(1).GetChild(0).gameObject;
        var sourceMesh = x.GetComponent<MeshFilter>().mesh;
        var mesh = new Mesh();
        mesh.vertices = sourceMesh.vertices;

        //this section taken from Unity docs
        var sh = PS.shape;
        sh.enabled = true;
        sh.shapeType = ParticleSystemShapeType.Mesh; //just in case you forgot to set it up in the inspector
        sh.mesh = mesh;
    }
}