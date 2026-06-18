using TMPro;
using UnityEngine;

public class DamageTextUI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private float fadeSpeed = 2f;

    private TextMeshPro textMesh;
    private Color textColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        textColor.a -= fadeSpeed * Time.deltaTime;
        textMesh.color = textColor;
    }

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void Setup(float damageAmount)
    {
        textMesh.text = Mathf.RoundToInt(damageAmount).ToString();
        textColor = textMesh.color;

        transform.position += new Vector3(Random.Range(-0.05f, 0.5f), 0, Random.Range(-0.05f, 0.5f));

        Destroy(gameObject, lifeTime);
    }
}
