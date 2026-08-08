using System;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class AttractionHandlerQuark : MonoBehaviour
{
    public Mesh mesh;
    public Material material;
    public int ParticleCount;
    public float TimeFactor;
    public float CoulombConstant;
    public float scale = 1e-15f;
    public float size = 0.0004f;
    public float sizeScale = 1f;
    public float dampening = 0.9f;
    public float minDist = 0.0004f;
    public float maxDist = 1f;
    public float pushForce = 0.0004f;
    public float ForceUp = 2/3f;
    public float ForceDown = -1/3f;
    public float a = 0.4f;
    public float o = 0.18f;
    public float bounds;

    public Vector2 BottomLeft;
    public Vector2 TopRight;


    public Vector2[] positions;
    public Vector2[] velocities;
    public float[] weights;
    public Color[] colors;

    public float[] charges;
    float k;
    float minDistSqrt;
    float maxDistSqrt;

    ComputeBuffer positionBuffer;
    ComputeBuffer colorBuffer;
    RenderParams rp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        //1fm = 1e-15
        // scale/2.5669699665e-38f = (1e-15f)^2 / 2.5669699665e-38f = 3.8956435527e22f
        k =  scale*scale/2.5669699665e-38f * CoulombConstant;//1.602176634e-19f * CoulombConstant;
        Debug.Log(k);
        minDistSqrt = minDist*minDist;
        maxDistSqrt = maxDist*maxDist;


        positions = new Vector2[ParticleCount];
        velocities = new Vector2[ParticleCount];
        //charges = new[] {ForceUp, ForceDown};

        positionBuffer = new ComputeBuffer(ParticleCount, sizeof(float) * 2);
        colorBuffer = new ComputeBuffer(10, sizeof(float) * 4);

        for(int i=0; i<ParticleCount; i++)
        {
            float x,y;
            if (bounds != 0)
            {
                x = Random.Range(-bounds,bounds);
                y = Random.Range(-bounds,bounds);
                
            }
            else
            {
                x = Random.Range(BottomLeft.x,TopRight.x);
                y = Random.Range(BottomLeft.y,TopRight.y);
            }
            positions[i]=new Vector2(x,y);
            velocities[i]=new Vector2(0,0);
        }

        positionBuffer.SetData(positions);
        colorBuffer.SetData(colors);

    }
    void OnDisable()
    {
        positionBuffer.Release();
        colorBuffer.Release();
    }

    float calcForce(float q1, float q2, float d)
    {
        return  d<minDistSqrt ? 0 : (k * q1 * q2 / d);
    }
    float convert(float x)
    {
        //turn fm into GeV^-1
        return x*5.068f;
    }

    float CornellPotential(float distance)
    {
        // Cornell potential: V(r) = -(4/3)(a/r)+omega*r+constant || or without (4/3) and constant
        //derivative: F(r) = -(a/r^2)+omega
        // a = radius of particle?
        // omega = 0.18GeV^2
        /* float a = 0.4f; */
        /* float o = 0.18f; */
        //distance<minDistSqrt ? :(((4*a)/(3*distance))+o)
        return -((4*a)/(3*distance))-o;

    }
    float MyStrongForce(float distance)
    {
        return distance/(a*a);
    }

    void updatePositions()
    {

        for(int i=0; i<ParticleCount; i++)
        {
            velocities[i]*=dampening;
            for(int j=i+1; j<ParticleCount; j++)
            {
                Vector2 direction = (positions[i] - positions[j]).normalized;
                float distance = (positions[i] - positions[j]).sqrMagnitude/* +(size*size) */;
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

                float forceSum = 0f;
                if(distance > minDistSqrt )
                {
                    //Debug.Log((calcForce(charges[i % 10], charges[j % 10], distance), CornellPotential(convert(distance))));
                    /* 
                    velocities[i] += direction * (calcForce(charges[i % 10], charges[j % 10], distance) +  CornellPotential(convert(distance))) / weights[i % 10] * TimeFactor;
                    velocities[j] += -direction * (calcForce(charges[i % 10], charges[j % 10], distance) +  CornellPotential(convert(distance))) / weights[j % 10] * TimeFactor;
                    */
                    if (distance < maxDistSqrt) forceSum += calcForce(charges[i % 10], charges[j % 10], distance) +  CornellPotential(convert(distance));
                    else forceSum += calcForce(charges[i % 10], charges[j % 10], distance);
                }
                velocities[i] += direction * forceSum / weights[i % 10] * TimeFactor;
                velocities[j] += -direction * forceSum / weights[j % 10] * TimeFactor;
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
        rp.matProps.SetFloat("_size", size*sizeScale);
        rp.matProps.SetBuffer("_colors", colorBuffer);
        Graphics.RenderMeshPrimitives(rp, mesh, 0, ParticleCount);
    }
}
