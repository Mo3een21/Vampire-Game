using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Animator))]
public class FieldOfView2D : MonoBehaviour
{
    [Header("View Settings")]
    [Tooltip("How far the enemy can see")]
    public float viewRadius = 5f;
    [Range(0, 360), Tooltip("Angle of the view cone")]
    public float viewAngle = 90f;

    [Header("Mesh Appearance")]
    [Tooltip("Number of segments in the filled arc (more = smoother)")]
    public int segments = 30;
    [Tooltip("Color & opacity of the cone")]
    public Color viewColor = new Color(1f, 1f, 0f, 0.2f);

    [Header("Detection Audio")]
    [Tooltip("Clip to loop while the player is inside the cone")]
    public AudioClip detectionClip;
    [Range(0f, 1f), Tooltip("Volume of the detection SFX")]
    public float detectionVolume = 1f;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    // internals
    MeshFilter mf;
    MeshRenderer mr;
    Mesh mesh;
    PolygonCollider2D poly;
    Animator animator;
    AudioSource audioSource;

    // cache for rebuild
    float _lastRadius, _lastAngle;
    int _lastSegments;

    void Awake()
    {
        animator = GetComponent<Animator>();

        // 1) Create child for mesh + collider
        var go = new GameObject("FOV Mesh");
        go.transform.SetParent(transform, false);

        // 2) Mesh components
        mf = go.AddComponent<MeshFilter>();
        mr = go.AddComponent<MeshRenderer>();
        mesh = new Mesh { name = "FOV Cone" };
        mf.mesh = mesh;
        var mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = viewColor;
        mr.material = mat;

        // 3) Collider
        poly = go.AddComponent<PolygonCollider2D>();
        poly.isTrigger = true;

        // 4) AudioSource for detection SFX
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
    }

    void Start()
    {
        BuildMesh();
    }

    void Update()
    {
        // Rotate the cone to face animator's MoveX/MoveY
        Vector2 face = GetFacingDirection();
        float ang = Mathf.Atan2(face.y, face.x) * Mathf.Rad2Deg;
        mf.transform.localEulerAngles = new Vector3(0, 0, ang);

        // Rebuild if properties changed
        if (!Mathf.Approximately(_lastRadius, viewRadius) ||
            !Mathf.Approximately(_lastAngle, viewAngle) ||
            _lastSegments != segments)
        {
            BuildMesh();
        }
    }

    void BuildMesh()
    {
        _lastRadius = viewRadius;
        _lastAngle = viewAngle;
        _lastSegments = segments;

        int seg = Mathf.Max(3, segments);
        float half = viewAngle * 0.5f;
        float r = viewRadius;

        // Vertices: center + perimeter points
        Vector3[] verts = new Vector3[seg + 2];
        Vector2[] path = new Vector2[seg + 2];
        verts[0] = Vector3.zero;
        path[0] = Vector2.zero;

        for (int i = 0; i <= seg; i++)
        {
            float a = -half + (viewAngle * i / seg);
            float rad = a * Mathf.Deg2Rad;
            Vector3 v = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * r;
            verts[i + 1] = v;
            path[i + 1] = new Vector2(v.x, v.y);
        }

        // Triangles: fan from center
        int[] tris = new int[seg * 3];
        for (int i = 0, t = 0; i < seg; i++)
        {
            tris[t++] = 0;
            tris[t++] = i + 1;
            tris[t++] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateBounds();

        // Match collider
        poly.pathCount = 1;
        poly.SetPath(0, path);
    }

    Vector2 GetFacingDirection()
    {
        float mx = animator.GetFloat("MoveX");
        float my = animator.GetFloat("MoveY");
        Vector2 dir = new Vector2(mx, my);
        return dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Start looping detection SFX if this is first entry
        if (visibleTargets.Count == 0 && detectionClip != null)
        {
            audioSource.clip = detectionClip;
            audioSource.volume = detectionVolume;
            audioSource.Play();
        }

        if (!visibleTargets.Contains(other.transform))
            visibleTargets.Add(other.transform);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        visibleTargets.Remove(other.transform);

        // Stop the SFX when no longer seeing the player
        if (visibleTargets.Count == 0 && audioSource.isPlaying)
            audioSource.Stop();
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector2 face = GetFacingDirection();
        float baseA = Mathf.Atan2(face.y, face.x) * Mathf.Rad2Deg;
        float half  = viewAngle * 0.5f;
        Vector3 dirA = DirFromAngle(baseA - half);
        Vector3 dirB = DirFromAngle(baseA + half);

        Gizmos.DrawLine(transform.position, transform.position + dirA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + dirB * viewRadius);
    }
#endif

    Vector3 DirFromAngle(float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
    }
}
