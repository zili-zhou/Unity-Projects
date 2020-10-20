﻿using UnityEngine;

public class FPSCounter : MonoBehaviour
{

	public int frameRange = 60;
	public int AverageFPS;


	int[] fpsBuffer;
	int fpsBufferIndex;


	void InitializeBuffer()
	{
		if (frameRange <= 0)
		{
			frameRange = 1;
		}
		fpsBuffer = new int[frameRange];
		fpsBufferIndex = 0;
	}

	void Update()
	{
		if(fpsBuffer==null|| fpsBuffer.Length!=frameRange)
		{
			InitializeBuffer();
		}
		UpdateBuffer();
		CalculateFPS();
	}

	void UpdateBuffer()
	{
		fpsBuffer[fpsBufferIndex++] = (int)(1.0f / Time.unscaledDeltaTime);
		if (fpsBufferIndex >= frameRange)
		{
			fpsBufferIndex = 0;
		}
	}

	void CalculateFPS()
	{
		int sum = 0;

		for(int i=0; i < frameRange; i++)
		{
			sum += fpsBuffer[i];
		}

		AverageFPS = (int)((float)sum / frameRange);

	}
}
