/*
	Copyright © Carl Emil Carlsen 2020
	http://cec.dk
*/

using UnityEngine;
using System;
using System.IO;

namespace RenderTextureShot
{
	public static class PathHelper
	{
		public static string GenerateUniqueFilePath( string directoryPath, string fileNameWithoutExtension, string fileExtension )
		{
			string filePath = directoryPath + "/" + fileNameWithoutExtension + fileExtension;
			int i = 2;
			while( File.Exists( filePath ) ) {
				filePath = directoryPath + "/" + fileNameWithoutExtension + " " + i + fileExtension;
				i++;
			}
			return filePath;
		}


		public static string GetDateTimeCode()
		{
			return DateTime.Now.ToString( "yyMMddhhmmss" );
		}


		public static string GetDesktopPath()
		{
			string desktopPath;
			if( Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer ) {
				desktopPath = Environment.GetEnvironmentVariable( "HOME" ) + "/Desktop";
			} else if( Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer ) {
				desktopPath = Environment.GetFolderPath( Environment.SpecialFolder.Desktop );
				desktopPath = desktopPath.Replace( "\\", "/" );
			} else {
				Debug.LogWarning( "Screen shot failed. Platform not supported." + Environment.NewLine );
				return string.Empty;
			}
			return desktopPath;
		}
	}
}
