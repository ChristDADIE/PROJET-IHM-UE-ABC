using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquide : MonoBehaviour
{
    [SerializeField]
    public float volume;

    [SerializeField]
    public Color color;

    [SerializeField]
    Mesh[] shapes;

    float previousQuantity;
    [SerializeField]
    float quantity;

    AbstractLiquid.type[] t = new AbstractLiquid.type[] { };


    public AbstractLiquid Property
    {
        get
        { 
            return new AbstractLiquid(GetComponent<Renderer>().material.color, quantity*volume, t);
        }
        set
        {
            quantity = value.quantity/volume;
            GetComponent<Renderer>().material.color = value.color;
            this.t = value.t;
        }
    }

    public float Quantity
    {
        get
        {
            return quantity;
        }

        set
        {
            quantity = value;
        }
    }

    [SerializeField]
    float mass = 1.0f;

    [SerializeField]
    float viscosity = 0.05f;

    [SerializeField]
    Rigidbody movements;

    [SerializeField]
    float delta = 0.01f;

    [SerializeField]
    float gravity = 1f;

    [SerializeField]
    Vector3 center;

    [SerializeField]
    Mesh triggerMesh;

    [SerializeField]
    bool calm;

    [SerializeField]
    public Vector3 output;
    
    public bool Calm
    {
        get
        {
            return calm;
        }

        set
        {
            calm = value;
            previousQuantity = -1;
            if (calm)
            {
                speed = new Vector3(0, 0, 0);
                oldSpeed = new Vector3(0, 0, 0);
            }
        }
    }

    [SerializeField]
    Collider input;


    Mesh[] reference;
    Mesh liquid;
    Mesh[] liquids;

    Vector3 direction;
    Vector3 speed;
    Vector3 oldSpeed;

    private float shaking;

    public float Shaking
    {
        get
        {
            return shaking;
        }
    }

    [NonSerialized]
    public Vector3 oldAcceleration = new Vector3(0,0,0);

    void UpdateReference()
    {
        for(int i = 0;i != shapes.Length;++i)
        {
            if (!shapes[i].isReadable)
                Debug.Log("Erreur, le mesh " + i + " n'est pas en mode readable");
        }
        
        
        reference = new Mesh[shapes.Length];
        for (int i = 0; i != shapes.Length; ++i)
        {
            reference[i] = new Mesh();
            Vector3[] vertices = new Vector3[shapes[i].vertexCount];
            int[] triangles = new int[shapes[i].triangles.Length];

            for (int j = 0; j != shapes[i].vertexCount; ++j)
            {
                vertices[j] = shapes[i].vertices[j]+delta*(center- shapes[i].vertices[j]).normalized;
            }
            for(int j = 0;j != shapes[i].triangles.Length;++j)
            {
                triangles[j] = shapes[i].triangles[j];
            }
            reference[i].vertices = vertices;
            reference[i].triangles = triangles;
        }
    }

    Mesh Fuse(Mesh[] meshs)
    {
        Mesh total = new Mesh();
        int totalVertices = 0;
        int totalTriangles = 0;
        for(int i = 0;i != meshs.Length;++i)
        {
            totalVertices += meshs[i].vertexCount;
            totalTriangles += meshs[i].triangles.Length;
        }

        Vector3[] vertices = new Vector3[totalVertices];
        int[] triangles = new int[totalTriangles];

        int currentVerticesIndex = 0;
        int currentVerticesDelta = 0;
        int currentTrianglesIndex = 0;
        for (int i = 0;i != meshs.Length;++i)
        {
            for(int j = 0;j != meshs[i].vertexCount;++j)
            {
                vertices[currentVerticesIndex++] = meshs[i].vertices[j];
            }
            for(int j = 0;j != meshs[i].triangles.Length;++j)
            {
                triangles[currentTrianglesIndex++] = meshs[i].triangles[j] + currentVerticesDelta;
            }
            currentVerticesDelta = currentVerticesIndex;
        }
        total.vertices = vertices;
        total.triangles = triangles;
        return total;
    }


    class Triangle
    {
        static Vector3 pointCut = new Vector3(0,0,0);
        public static void Clear()
        {
            pointCut = new Vector3(0, 0, 0);
        }
        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            this.a = a;this.b = b;this.c = c;
        }

        public List<Triangle> cutAlong(Vector3 position,Vector3 direction,bool fill = true)
        {
            a -= position;
            b -= position;
            c -= position;

            int permutation = 0;
            if (Vector3.Dot(a, direction) > 0) permutation += 1 << 0;
            if (Vector3.Dot(b, direction) > 0) permutation += 1 << 1;
            if (Vector3.Dot(c, direction) > 0) permutation += 1 << 2;
            a += position;
            b += position;
            c += position;
            int cut;

            switch(permutation)
            {
                case 0:
                    
                    return new List<Triangle>() { this };
                case 1:
                    (a, b, c) = (b, c, a); cut = 2; break;
                case 2:
                    (a, b, c) = (c, a, b); cut = 2; break;
                case 3:
                    (a, b, c) = (c, a, b); cut = 1; break;
                case 4:
                    /*(a, b, c) = (a, b, c);*/ cut = 2; break;
                case 5:
                    (a, b, c) = (b, c, a); cut = 1; break;
                case 6:
                    /*(a, b, c) = (a, b, c);*/ cut = 1; break;
                case 7:
                    return new List<Triangle>();
                default:
                    Debug.Log("Erreur, permutation non valide");
                    cut = 0;
                    break;
            }

            Plane plan = new Plane(direction, position);

            if(cut == 1)
            {
                plan.Raycast(new Ray(a, c-a), out float distanceD);
                plan.Raycast(new Ray(a, b-a), out float distanceE);
                Vector3 d = a + (c - a).normalized * distanceD;
                Vector3 e = a + (b - a).normalized * distanceE;
                if(pointCut == new Vector3(0,0,0) || !fill)
                {
                    pointCut = e;
                    return new List<Triangle>() { new Triangle(a, e, d) };
                }
                else
                {
                    return new List<Triangle>() { new Triangle(a, e, d),new Triangle(e,pointCut, d) };
                }
                
            }
            else
            {
                plan.Raycast(new Ray(a, c-a), out float distanceD);
                plan.Raycast(new Ray(b, c-b),out float distanceE);
                Vector3 d = a + (c - a).normalized * distanceD;
                Vector3 e = b + (c - b).normalized * distanceE;
                if (pointCut == new Vector3(0, 0, 0) || !fill)
                {
                    pointCut = e;
                    return new List<Triangle>() { new Triangle(a,e,d),new Triangle(a,b,e)};
                }
                else
                {
                    return new List<Triangle>() { new Triangle(a, e, d), new Triangle(a, b, e), new Triangle(e, pointCut, d) };
                }

        }

        }
        public Vector3 a,b,c;
    }
    void UpdateLiquid(Vector3 position,Vector3 direction)
    {
        liquid.Clear();
        for (int index_ref = 0; index_ref != reference.Length;++index_ref)
        {
            liquids[index_ref].Clear();
            List<Triangle> listMetaTriangles = new();
            for (int i = 0; i < reference[index_ref].triangles.Length; i += 3)
            {
                listMetaTriangles.Add(new Triangle(reference[index_ref].vertices[reference[index_ref].triangles[i]],
                    reference[index_ref].vertices[reference[index_ref].triangles[i + 1]],
                    reference[index_ref].vertices[reference[index_ref].triangles[i + 2]]));
            }

            List<Vector3> listVertices = new();
            List<int> listTriangles = new();
            Triangle.Clear();
            foreach (Triangle triangle in listMetaTriangles)
            {
                foreach (Triangle t in triangle.cutAlong(position, direction))
                {
                    listTriangles.Add(listVertices.Count);
                    listVertices.Add(t.a);
                    listTriangles.Add(listVertices.Count);
                    listVertices.Add(t.b);
                    listTriangles.Add(listVertices.Count);
                    listVertices.Add(t.c);
                }
            }

            liquids[index_ref].vertices = listVertices.ToArray();
            liquids[index_ref].triangles = listTriangles.ToArray();
        }
        Mesh fusedMesh = Fuse(liquids);
        liquid.vertices = fusedMesh.vertices;
        liquid.triangles = fusedMesh.triangles;
        liquid.RecalculateNormals();
        liquid.Optimize();

    }
    void Start()
    {
        UpdateReference();
        liquid = new Mesh();
        liquids = new Mesh[reference.Length];
        for(int i = 0;i != reference.Length;++i)
            liquids[i] = new Mesh();

        GetComponent<MeshFilter>().mesh = liquid;
        direction = (Quaternion.Inverse(transform.rotation)) * (new Vector3(0, 1, 0));
        oldSpeed = new Vector3(0, 0, 0);
        speed = new Vector3(0, 0, 0);
        shaking = 0;
        previousQuantity = -1;

        GetComponent<Renderer>().material.color = color;

    }

    private Vector3 GetDown()
    {
        return Quaternion.Inverse(transform.rotation) * new Vector3(0, 1, 0);
    }

    
    private void FixedUpdate()
    {
        if(movements != null && !calm)
        {
            Vector3 acceleration = (movements.velocity - oldSpeed)/mass + GetDown() * Time.fixedDeltaTime*gravity;
            oldAcceleration = acceleration;
            oldSpeed = movements.velocity;
            shaking = acceleration.magnitude;
            speed += acceleration;
            speed = Vector3.ProjectOnPlane(speed, direction) * Mathf.Pow(0.999f,viscosity / (Time.fixedDeltaTime*(quantity+0.2f)));
            direction += speed;
            direction = direction.normalized;
            Vector3 position = direction * scale + center;
            bool isFlowing = false;
            if (triggerMesh != null)
            {
                for (int i = 0; i != triggerMesh.vertexCount; ++i)
                {

                    if (Vector3.Dot(triggerMesh.vertices[i] - position, direction) <= 0)
                    {
                        isFlowing = true;
                        break;
                    }
                }
            }
            if (isFlowing)
            {
                flowing = Vector3.Dot(-acceleration, output);
                if (flowing < 0)
                    flowing = 0;
            }
            else
            {
                flowing = 0;
            }

        }
        else
        {
            direction = (Quaternion.Inverse(transform.rotation)) * (new Vector3(0, 1, 0));
            shaking = 0;
        }
    }


    float flowing;

    public float Flowing

    {
        get
        {
            return flowing;
        }
        set
        {
            flowing = value;
        }
    }

    private float scale = 0;


    public bool IsFilling(Collider liquidObject)
    {
        if (input == null)
            return false;
        return input.bounds.Intersects(liquidObject.bounds);

    }


    void Update()
    {
        if(!calm || previousQuantity != quantity)
        {
            previousQuantity = quantity;
            float borneMin = float.MaxValue;
            float borneMax = float.MinValue;
            for (int i = 0; i != reference.Length; ++i)
            {
                for (int j = 0; j != reference[i].vertexCount; ++j)
                {
                    float value = Vector3.Dot(direction, reference[i].vertices[j] - center);
                    if (value < borneMin) borneMin = value;
                    if (value > borneMax) borneMax = value;
                }
            }
            scale = (borneMax - borneMin) * quantity + borneMin;
            Vector3 position = direction * scale + center;
            UpdateLiquid(position, direction);
        }
    }
}
