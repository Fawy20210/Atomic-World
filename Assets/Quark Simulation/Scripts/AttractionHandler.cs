using System;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class AttractionHandler : MonoBehaviour
{
    public Mesh mesh;
    public Material material;
    public int ParticleCount;
    public float TimeFactor;
    public float CoulombConstant;
    public float scale = 1e-15f;
    public float ForceProtons = 1f;
    public float ForceNeutrons = 0.01f;
    public float ForceElectrons = -1f;
    public float a = 0.4f;
    public float o = 0.18f;

    public Vector2 BottomLeft;
    public Vector2 TopRight;


    public Vector2[] positions;
    public Vector2[] velocities;
    public float[] weights;
    public float[] sizes;
    public Color[] colors;

    float[] charges;
    float k;

    ComputeBuffer positionBuffer;
    ComputeBuffer sizeBuffer;
    ComputeBuffer colorBuffer;
    RenderParams rp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        //1fm = 1e-15
        // scale/2.5669699665e-38f = (1e-15f)^2 / 2.5669699665e-38f = 3.8956435527e22f
        k =  scale*scale/2.5669699665e-38f * CoulombConstant;//1.602176634e-19f * CoulombConstant;
        Debug.Log(k);


        positions = new Vector2[ParticleCount];
        velocities = new Vector2[ParticleCount];
        charges = new[] {ForceProtons, ForceNeutrons, ForceElectrons};

        positionBuffer = new ComputeBuffer(ParticleCount, sizeof(float) * 2);
        sizeBuffer = new ComputeBuffer(3, sizeof(float));
        colorBuffer = new ComputeBuffer(3, sizeof(float) * 4);

        for(int i=0; i<ParticleCount; i++)
        {
            float x = Random.Range(BottomLeft.x,TopRight.x);
            float y = Random.Range(BottomLeft.y,TopRight.y);
            positions[i]=new Vector2(x,y);
            velocities[i]=new Vector2(0,0);
        }

        positionBuffer.SetData(positions);
        sizeBuffer.SetData(sizes);
        colorBuffer.SetData(colors);

    }
    void OnDisable()
    {
        positionBuffer.Release();
        sizeBuffer.Release();
        colorBuffer.Release();
    }

    float calcForce(float q1, float q2, float d)
    {
        return k * q1 * q2 / d;
    }

    float CornellPotential(float distance)
    {
        // Cornell potential: V(r) = -(4/3)(a/r)+omega*r+constant || or without (4/3) and constant
        //derivative: F(r) = -(a/r^2)+omega
        // a = radius of particle?
        // omega = 0.18GeV^2
        /* float a = 0.4f; */
        /* float o = 0.18f; */
        return -(a/distance)-o;

    }

    void updatePositions()
    {

        for(int i=0; i<ParticleCount; i++)
        {
            for(int j=i+1; j<ParticleCount; j++)
            {
                Vector2 direction = (positions[i] - positions[j]).normalized;
                float distance = (positions[i] - positions[j]).SqrMagnitude();
                /* if(distance < 1)
                {
                    //float rootDist = Mathf.Sqrt(distance);
                    velocities[i] += direction * CornellPotential(distance) / weights[i % 3] * TimeFactor;
                    velocities[j] += -direction * CornellPotential(distance) / weights[i % 3] * TimeFactor;
                }
                else
                {
                } */
                //if(distance<1) Debug.Log(("close",CornellPotential(distance)));
                /* Debug.Log((CornellPotential(distance), distance)); */
                    velocities[i] += direction * (calcForce(charges[i % 3], charges[j % 3], distance) +  CornellPotential(distance)) / weights[i % 3] * TimeFactor;
                    velocities[j] += -direction * (calcForce(charges[i % 3], charges[j % 3], distance) +  CornellPotential(distance)) / weights[j % 3] * TimeFactor;
            }
        }
        for(int i=0; i<ParticleCount; i++)
        {
            positions[i] += velocities[i];
            
        }
        positionBuffer.SetData(positions);

    }

    // Update is called once per frame
    void Update()
    {
        updatePositions();
        rp = new RenderParams(material);
        rp.worldBounds = new Bounds(Vector3.zero, 100000000*Vector3.one); // use tighter bounds
        rp.matProps = new MaterialPropertyBlock();
        rp.matProps.SetMatrix("_ObjectToWorld", Matrix4x4.Translate(new Vector3(-4f, 0, 0)));
        rp.matProps.SetFloat("_NumInstances", ParticleCount);
        rp.matProps.SetBuffer("_positions", positionBuffer);
        rp.matProps.SetBuffer("_sizes", sizeBuffer);
        rp.matProps.SetBuffer("_colors", colorBuffer);
        Graphics.RenderMeshPrimitives(rp, mesh, 0, ParticleCount);
    }
}
