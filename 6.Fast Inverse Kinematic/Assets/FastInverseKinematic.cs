using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class FastInverseKinematic : MonoBehaviour
{
    public int ChainLength = 3;

    public Transform target;
    public Transform pole;

    protected float[] BonesLength;
    protected Transform[] Bones;
    protected Vector3[] Positions;
    protected float CompleteLength;

    [Header("Server Parameters")]
    public int Iterations = 10;
    public float Delta = 0.001f;

    [Range(0, 1)]
    public float SnapBackStrength = 1f;
    void Awake()
    {
        init();
    }

    void init()
    {
        Bones = new Transform[ChainLength + 1];
        Positions = new Vector3[ChainLength + 1];
        BonesLength = new float[ChainLength];
        CompleteLength = 0;

        var current = transform;
        for(var i=Bones.Length-1;i>=0;i--)
        {
            Bones[i] = current;
            if(i==Bones.Length-1)
            {

            }
            else
            {
                BonesLength[i] = (Bones[i + 1].position - current.position).magnitude;
                CompleteLength += BonesLength[i];
            }
            current = current.parent;
        }
    }

    void LateUpdate()
    {
        ResolveIK();
    }

    private void ResolveIK()
    {
        if (target == null)
            return;

        if (CompleteLength != ChainLength)
            init();

        for(int i=0; i<Bones.Length; i++)
        {
            Positions[i] = Bones[i].position;
        }

        if((target.position - Bones[0].position).sqrMagnitude >= CompleteLength*CompleteLength)
        {
            var direction = (target.position - Bones[0].position).normalized;
            for( int i=1; i< Bones.Length; i++)
            {
                Positions[i] = Positions[i - 1] + direction * BonesLength[i - 1];
            }

        }
        else
        {
            for(int iteration=0;iteration< Iterations; iteration++)
            {
                //back;
                for(int i=Positions.Length-1; i>=1; i--)
                {
                    if (i == Positions.Length - 1)
                        Positions[i] = target.position;
                    else
                        Positions[i] = Positions[i + 1] + (Positions[i] - Positions[i + 1]).normalized * BonesLength[i];
                }

                //forward
                for (int i = 1;i<= Positions.Length - 1;i++)
                {
                    Positions[i] = Positions[i - 1] + (Positions[i] - Positions[i - 1]).normalized * BonesLength[i - 1];
                }

                if ((Positions[Positions.Length - 1] - target.position).sqrMagnitude < Delta * Delta)
                    break;
            }
        }

        if (pole != null)
        {
            for (int i = 1; i < Positions.Length - 1; i++)
            {
                var plane = new Plane(Positions[i + 1] - Positions[i - 1], Positions[i - 1]);
                var projectedPole = plane.ClosestPointOnPlane(pole.position);
                var projectedBone = plane.ClosestPointOnPlane(Positions[i]);
                var angle = Vector3.SignedAngle(projectedBone - Positions[i - 1], projectedPole - Positions[i - 1], plane.normal);
                Positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (Positions[i] - Positions[i - 1]) + Positions[i - 1];
            }
        }

        for (int i=0; i < Bones.Length; i++)
        {
            Bones[i].position = Positions[i];
        }
    }


    void OnDrawGizmos()
    {
        var current = this.transform;
        for (int i = 0; i < ChainLength && current != null && current.parent != null; i++)
        {
            var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }
    }


}
