#region Using
using UnityEngine;
using System;
using System.Collections;
#endregion // Using

public class SkyBoxPicker : MonoBehaviour {

	#region Varibales
	public Material[] SkyboxList;
	int m_nCurrent = 0;
	
	float m_fDelay = 10;
	#endregion // Varibales
	
	#region Set-up
	void Start ()
	{
		UpdateSkyBox();
	}
	#endregion // Set-up
	
	#region functions
	void UpdateSkyBox()
	{
		try{
			RenderSettings.skybox= SkyboxList[m_nCurrent];
		}
		catch
		{
		}
	}
	#endregion functions
		
	#region GUI
	void OnGUI()
	{
	
		/*if (m_fDelay == 50 )
		{	
			m_nCurrent = 1;
			UpdateSkyBox();
		}
		if (m_fDelay == 0 )
		{
			m_nCurrent = 0;
			UpdateSkyBox();
			m_fDelay = 100;	
		}
		else
		{
			m_fDelay --;
		
		}*/
		
	
	
		RecordScreens RecScre;
		RecScre = GetComponentInChildren<RecordScreens>(); 
		if ( !RecScre.IsRecording())
		{
			for ( int i = 1 ;i < 6 ; i++ )
			{
				if ( m_nCurrent+1 != i )
				{
					if ( GUI.Button(new Rect(48 + ( i * 45 ), Screen.height-64 , 44	, 32), i.ToString() ) )
					{
						m_nCurrent = i-1;
						UpdateSkyBox();
					}	
				}
				else
				{
					GUI.Button(new Rect(48 + ( i * 45 ), Screen.height-64 , 44, 32), "["+i.ToString()+"]" );
				}
			}
		}
	}
	#endregion // GUI
}
