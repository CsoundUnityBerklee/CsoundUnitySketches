using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Attractor
{
	Lorenz, FourWing, Rossler
}

//[RequireComponent(typeof(Renderer))]

public class Spatializer : MonoBehaviour
{


	[Header("Spatializers")]
	[Tooltip("Select the desired Chaos Attractor")]
	public Attractor attractor;

	[Tooltip("Select Speed")]
	[Range(0.1f, 10f)]
	[SerializeField] float _speed;

	double[] variables = new double[3];
	int n = 3;


	double[] x;
	double[] xprime;
	double[] xstore;
	double[] k1;
	double[] k2;
	double[] k3;
	double[] k4;

	[Header("Visual")]
	[Tooltip("On/Off")]
	[SerializeField] bool showVisual;
	

	// Use this for initialization
	void Start()
	{


		x = new double[n];
		x[0] = transform.position.x;
		x[1] = transform.position.y;
		x[2] = transform.position.z;
		xprime = new double[n];
		xstore = new double[n];
		k1 = new double[n];
		k2 = new double[n];
		k3 = new double[n];
		k4 = new double[n];


		if (!showVisual)
		{
			Destroy(GetComponent<TrailRenderer>());
		}

		// Determine Variables depending on Attractor

		switch (attractor)
		{
			case Attractor.Lorenz:
				// Lorenz Variables
				variables[0] = 28.0;
				variables[1] = 8.0 / 3.0;
				variables[2] = 10.0;
				break;

			case Attractor.FourWing:
				// Four Wing Variables
				variables[0] = 0.2;
				variables[1] = -0.01;
				variables[2] = -0.4;
				break;

			case Attractor.Rossler:
				// Rossler Variables
				variables[0] = 0.2;
				variables[1] = 0.2;
				variables[2] = 5.7;
				break;


			default: // Lorenz
				variables[0] = 28.0;
				variables[1] = 8.0 / 3.0;
				variables[2] = 10.0;
				break;
		}
	}

	void RatesOfChange(double[] xin)
	{

		double x = xin[0];
		double y = xin[1];
		double z = xin[2];

		switch (attractor)
		{
			case Attractor.Lorenz:
				CalculateLorenz(x, y, z);
				break;
			case Attractor.FourWing:
				CalculateFourWing(x, y, z);
				break;
			case Attractor.Rossler:
				CalculateRossler(x, y, z);
				break;
		}

	}

	void FixedUpdate()
	{
		double h = (double)Time.fixedDeltaTime * _speed;
		RatesOfChange(x); // start using current position
		for (int i = 0; i < n; i++)
		{
			k1[i] = xprime[i] * h;
			xstore[i] = x[i] + 0.5 * k1[i];
		}
		RatesOfChange(xstore);
		for (int i = 0; i < n; i++)
		{
			k2[i] = xprime[i] * h;
			xstore[i] = x[i] + 0.5 * k2[i];
		}
		RatesOfChange(xstore);
		for (int i = 0; i < n; i++)
		{
			k3[i] = xprime[i] * h;
			xstore[i] = x[i] + k3[i];
		}
		RatesOfChange(xstore);
		for (int i = 0; i < n; i++)
		{
			k4[i] = xprime[i] * h;
			x[i] = x[i] + (1.0 / 6.0) *
				(k1[i] + 2.0 * k2[i] +
				2.0 * k3[i] + k4[i]);
		}
	}

	// Update is called once per frame
	void Update()
	{
		transform.position = new Vector3(
			(float)x[0], (float)x[1], (float)x[2]
		);

	}

	void CalculateLorenz(double x, double y, double z)
	{

		// Lorenz Equations
		xprime[0] = variables[2] * (y - x);
		xprime[1] = x * (variables[0] - z) - y;
		xprime[2] = x * y - (variables[1] * z);
	}

	void CalculateFourWing(double x, double y, double z)
	{

		// Four Wing Equations
		xprime[0] = (variables[0] * x) + (y * z);
		xprime[1] = (variables[1] * x) + (variables[2] * y) - (x * z);
		xprime[2] = -z - (x * y);
	}

	void CalculateRossler(double x, double y, double z)
	{

		// Rossler Equations
		xprime[0] = -(y + z);
		xprime[1] = x + (variables[0] * y);
		xprime[2] = variables[1] + z * (x - variables[2]);
	}

	
}