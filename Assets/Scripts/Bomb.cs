using System;
using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	private Color _color;
	private Material _material;

	public event Action<Bomb> Detonated;

    private void Awake()
    {
		_material = GetComponent<Renderer>().material;
		_color = _material.color;
    }

    private void OnEnable()
    {
		StartCoroutine(Detonate());
    }

	private void ChangeTransparency(float alpha)
	{
		_material.color = new Color(_color.r, _color.g, _color.b, alpha);
	}

	private void Explode()
	{
		Rigidbody targetInRadius;

        int bufferSize = 50;
		Collider[] buffer = new Collider[bufferSize];

		float explosionRadius = 10f;
		float explosionForce = 200f;
		Vector3 explosionCenter = transform.position;
		
		bufferSize = Physics.OverlapSphereNonAlloc(explosionCenter, explosionRadius, buffer);

		for (int i = 0; i < bufferSize; i++)
		{
			targetInRadius = buffer[i].attachedRigidbody;

			if (targetInRadius != null)
			{
				targetInRadius.AddExplosionForce(explosionForce, explosionCenter, explosionRadius);
            }
		}
    }

	private IEnumerator Detonate()
	{
		int minTime = 2;
		int maxTime = 5;
		float elapsedTime = 0;
		int detonationTime = UnityEngine.Random.Range(minTime, maxTime + 1);

		float maxAlpha = 1;
		float alphaPerSec = maxAlpha / detonationTime;
		float remainingAlpha = maxAlpha;

		while (elapsedTime < detonationTime)
		{
            yield return null;

			elapsedTime += Time.deltaTime;

			remainingAlpha = remainingAlpha - alphaPerSec * Time.deltaTime;

            ChangeTransparency(remainingAlpha);
        }

		Detonated?.Invoke(this);

        Explode();
    }
}
