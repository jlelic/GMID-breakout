using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHandler : MonoBehaviour
{
    public BoxCrackHandler CrackBox;
    public ParticleSystem SmokeParticleSystem;
    public ParticleSystem SplinterParticleSystem;
    public AudioClip[] BoxHitSounds;
    public GameObject TextMessagePrefab;

    public int totalHealth = 2;

    private int _health;
    private GamePlayHandler _gamePlayHandler;
    private AudioSource _audioSource;

    // Use this for initialization
    void Start()
    {
        _health = totalHealth;
        _audioSource = GetComponent<AudioSource>();
        _gamePlayHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GamePlayHandler>();
        _gamePlayHandler.OnBoxAdded();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision col)
    {
        var tag = col.gameObject.tag;
        if (tag == "Ball")
        {
            _audioSource.clip = BoxHitSounds[(int) Random.Range(0, BoxHitSounds.Length - 0.001f)];
            _audioSource.Play();
            StartCoroutine(PauseOnHit(0.015f));
            SplinterParticleSystem.Play();
            CrackBox.SetCrack();
            _health--;
            if (_health <= 0)
            {
                StartCoroutine(TakeDamage(true, 3));
                _audioSource.volume = 5f;
                _audioSource.Play();
                _audioSource.Play();
                _audioSource.Play();
            }
        }
    }

    public void ShowMessage(string text)
    {
        ShowMessage(text, Color.white);
    }

    public void ShowMessage(string text, Color color)
    {
        var textMesh= GameObject.Instantiate(TextMessagePrefab, transform.localPosition, Quaternion.identity).GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.color = color;
    }

    public IEnumerator TakeDamage(bool destroy, int skip = 0)
    {
        if (GetComponent<MeshFilter>() == null || GetComponent<SkinnedMeshRenderer>() == null)
        {
            yield return null;
        }

        if (GetComponent<Collider>())
        {
            GetComponent<Collider>().enabled = !destroy;
        }

        Mesh M = new Mesh();
        if (GetComponent<MeshFilter>())
        {
            M = GetComponent<MeshFilter>().mesh;
        }
        else if (GetComponent<SkinnedMeshRenderer>())
        {
            M = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        Material[] materials = new Material[0];
        if (GetComponent<MeshRenderer>())
        {
            materials = GetComponent<MeshRenderer>().materials;
        }
        else if (GetComponent<SkinnedMeshRenderer>())
        {
            materials = GetComponent<SkinnedMeshRenderer>().materials;
        }

        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;
        for (int submesh = 0; submesh < M.subMeshCount; submesh++)
        {
            int[] indices = M.GetTriangles(submesh);

            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];
                for (int n = 0; n < 3; n++)
                {
                    int index = indices[i + n];
                    newVerts[n] = verts[index];
                    newUvs[n] = uvs[index];
                    newNormals[n] = normals[index];
                }

                Mesh mesh = new Mesh();
                mesh.vertices = newVerts;
                mesh.normals = newNormals;
                mesh.uv = newUvs;

                mesh.triangles = new int[] {0, 1, 2, 2, 1, 0};

                GameObject GO = new GameObject("Triangle " + (i / 3));
                GO.layer = LayerMask.NameToLayer("Particle");
                GO.transform.position = transform.position;
                GO.transform.rotation = transform.rotation;
                GO.transform.localScale = transform.localScale;
                GO.AddComponent<MeshRenderer>().material = materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;
                GO.AddComponent<BoxCollider>();
                Vector3 explosionPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),
                    transform.position.y + Random.Range(0f, 0.5f), transform.position.z + Random.Range(-0.5f, 0.5f));
                GO.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(300, 500), explosionPos, 5);
                Destroy(GO, 5 + Random.Range(0.0f, 3.0f));
            }
        }

        GetComponent<Renderer>().enabled = !destroy;
        if (destroy)
        {
            SmokeParticleSystem.Play();
            CrackBox.ClearCrack();
            _gamePlayHandler.OnBoxDestroyed();
            OnBoxBroken();
        }

        yield return new WaitForSeconds(5.0f);
        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnBoxBroken()
    {
        
    }

    private IEnumerator PauseOnHit(float pauseLength)
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(pauseLength);
        Time.timeScale = 1;
    }
}