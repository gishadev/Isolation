using UnityEngine;

public static class MaterialsRewriter
{
    // Обновляем коллекцию материалов для динамичного изменения.
    public static void ResetMaterials(MeshRenderer mr, Material sampleMat, out Material outSampleMat)
    {
        Material[] newMats = new Material[mr.sharedMaterials.Length];
        outSampleMat = null;
        if (mr.sharedMaterials.Length > 1)
        {
            for (int i = 0; i < newMats.Length; i++)
            {
                if (mr.sharedMaterials[i] == sampleMat)
                {
                    outSampleMat = new Material(sampleMat);
                    newMats[i] = outSampleMat;
                    continue;
                }

                newMats[i] = mr.sharedMaterials[i];
            }
        }

        else
        {
            outSampleMat = new Material(sampleMat);
            newMats[0] = outSampleMat;
        }

        mr.materials = newMats;
    }

    // Обновляем коллекцию материалов для динамичного изменения.
    public static void ResetMaterials(LineRenderer lr, out Material outSampleMat)
    {
        outSampleMat = new Material(lr.sharedMaterial);
        lr.material = outSampleMat;
    }
}
