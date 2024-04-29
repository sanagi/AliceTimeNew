using UnityEngine;
using System.Collections;

public class Ladder_UV : MonoBehaviour {
	MeshFilter mf;
	float uvScale;

    void Start() {
        mf = gameObject.GetComponent<MeshFilter>();

        uvScale = gameObject.transform.parent.gameObject.transform.lossyScale.y;

        Mesh mesh = mf.mesh;
        var uvs = new Vector2[4];
        uvs[0] = new Vector2(0.0f, uvScale);
        uvs[1] = new Vector2(1.0f, 0.0f);
        uvs[2] = new Vector2(1.0f, uvScale);
        uvs[3] = new Vector2(0.0f, 0.0f);

        if (mesh.uv.Length != uvs.Length) {
            return;
        }

        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        ;
    }
}
