using UnityEngine;
using UnityEngine.UIElements;

public class AttractionHandler : MonoBehaviour
{
    public Mesh mesh;
    public Material material;
    public float size;
    public float TimeFactor;
    public int ParticleCount;
    public float ForceProtons = 1f;
    public float ForceNeutrons = 0.01f;
    public float ForceElectrons = -1f;


    public Vector2 BottomLeft;
    public Vector2 TopRight;


    public Vector2[] positions;
    public Vector2[] velocities;
    public Color[] colors;


    ComputeBuffer positionBuffer;
    ComputeBuffer colorBuffer;
    RenderParams rp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        positions = new Vector2[ParticleCount];
        velocities = new Vector2[ParticleCount];
        for(int i=0; i<ParticleCount; i++)
        {
            float x = Random.Range(BottomLeft.x,TopRight.x);
            float y = Random.Range(BottomLeft.y,TopRight.y);
            positions[i]=new Vector2(x,y);
        }


        positionBuffer.SetData(positions);
        colorBuffer.SetData(colors);

        rp = new RenderParams(material);
        rp.worldBounds = new Bounds(Vector3.zero, 100000000*Vector3.one); // use tighter bounds
        rp.matProps = new MaterialPropertyBlock();
        rp.matProps.SetMatrix("_ObjectToWorld", Matrix4x4.Translate(new Vector3(-4f, 0, 0)));
        rp.matProps.SetFloat("_NumInstances", ParticleCount);
        rp.matProps.SetBuffer("_positions", positionBuffer);
        rp.matProps.SetBuffer("_colors", colorBuffer);
        rp.matProps.SetFloat("_particleScale", size);
    }
    void updatePositions()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Graphics.RenderMeshPrimitives(rp, mesh, 0, ParticleCount);
    }
}
