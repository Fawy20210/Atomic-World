using UnityEngine;

public class Controller3D : MonoBehaviour
{
    public Mesh mesh;
    public Material material;
    public ComputeShader compute;
    public int ParticleCount;
    public int upPart = 1;
    public int downPart = 1;

    public float TimeFactor;
    public float CoulombConstant = 8.987e-09f;
    public float scale = 1e-15f;
    public float size = 0.0043f;
    public float sizeScale = 100f;
    public float minDist = 0.0004f;
    public float maxDist = 1f;
    public float dampening = 0.9f;
    public float a = 0.4f;
    public float o = 0.18f;
    public float bounds;
    public int A,B,C;


    public Vector3[] positions;
    public Vector3[] velocities;
    public Color[] colors;
    float[] charges;

    float k;
    float minDistSqrt;
    float maxDistSqrt;

    ComputeBuffer positionsBuffer;
    ComputeBuffer velocitiesBuffer;
    ComputeBuffer chargesBuffer;
    ComputeBuffer colorBuffer;
    RenderParams rp;

    int updateVelocitiesID;
    int updatePositionsID;



    void BindAll(int k)
    {
        compute.SetBuffer(k, "_positions", positionsBuffer);
        compute.SetBuffer(k, "_velocities", velocitiesBuffer);
        compute.SetBuffer(k, "_charges", chargesBuffer);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        k =  scale*scale/2.5669699665e-38f * CoulombConstant;//1.602176634e-19f * CoulombConstant;
        Debug.Log(k);
        minDistSqrt = minDist*minDist;
        maxDistSqrt = maxDist*maxDist;


        positions = new Vector3[ParticleCount];
        velocities = new Vector3[ParticleCount];
        charges = new float[upPart + downPart];

        positionsBuffer = new ComputeBuffer(ParticleCount, sizeof(float) * 3);
        velocitiesBuffer = new ComputeBuffer(ParticleCount, sizeof(float) * 3);
        chargesBuffer = new ComputeBuffer(upPart + downPart, sizeof(float));
        colorBuffer = new ComputeBuffer(upPart + downPart, sizeof(float) * 4);

        updateVelocitiesID = compute.FindKernel("updateVelocities");
        updatePositionsID = compute.FindKernel("updatePositions");

        for(int i=0; i<ParticleCount; i++)
        {
            float x,y,z;
            x = Random.Range(-bounds,bounds);
            y = Random.Range(-bounds,bounds);
            z = Random.Range(-bounds,bounds);
                
            positions[i]=new Vector3(x,y,z);
            velocities[i]=new Vector3(0,0,0);
        }
        for(int i=0; i<upPart+downPart; i++)
        {
            if (i < upPart)
            {
                charges[i] = 2f/3f;
            }
            else
            {
                charges[i] = -1f/3f;
            }
        }


        positionsBuffer.SetData(positions);
        velocitiesBuffer.SetData(velocities);
        chargesBuffer.SetData(charges);
        colorBuffer.SetData(colors);

        
        compute.SetFloat("_ParticleCount", ParticleCount);
        compute.SetInt("_Differents", upPart + downPart);
        compute.SetFloat("_K", k);
        compute.SetFloat("_O", o);
        compute.SetFloat("_A", a);
        compute.SetFloat("_dampening", dampening);
        compute.SetFloat("_minDistSqrt", minDistSqrt);
        compute.SetFloat("_maxDistSqrt", maxDistSqrt);
        compute.SetFloat("_TimeFactor", TimeFactor);

        BindAll(updateVelocitiesID);
        BindAll(updatePositionsID);

    }
    void OnDisable()
    {
        positionsBuffer.Release();
        velocitiesBuffer.Release();
        chargesBuffer.Release();
        colorBuffer.Release();
    }

    // Update is called once per frame
    void Update()
    {
        compute.Dispatch(updateVelocitiesID, A,B,C);
        compute.Dispatch(updatePositionsID, A,B,C);
        rp = new RenderParams(material);
        rp.worldBounds = new Bounds(Vector3.zero, 100000000*Vector3.one); // use tighter bounds
        rp.matProps = new MaterialPropertyBlock();
        rp.matProps.SetMatrix("_ObjectToWorld", Matrix4x4.Translate(new Vector3(-4f, 0, 0)));
        rp.matProps.SetFloat("_NumInstances", ParticleCount);
        rp.matProps.SetBuffer("_positions", positionsBuffer);
        rp.matProps.SetFloat("_size", size*sizeScale);
        rp.matProps.SetFloat("_Differents", upPart+downPart);
        rp.matProps.SetBuffer("_colors", colorBuffer);
        Graphics.RenderMeshPrimitives(rp, mesh, 0, ParticleCount);
    }
}
