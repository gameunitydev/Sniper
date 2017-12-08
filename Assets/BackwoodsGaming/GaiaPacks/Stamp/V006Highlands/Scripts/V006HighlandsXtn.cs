#if GAIA_PRESENT && UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using BackwoodsGaming.Framework;
using System.Reflection;
using System.Text;
using UnityEditor;

namespace Gaia.GX.BackwoodsGaming
{
    /// <summary>
    /// Gaia Item Pack
    /// </summary>
    public class V006HighlandsXtn : MonoBehaviour {

        #region Generic informational methods

        /// <summary>
        /// Returns the publisher name if provided. 
        /// This will override the publisher name in the namespace ie Gaia.GX.PublisherName
        /// </summary>
        /// <returns>Publisher name</returns>
        public static string GetPublisherName()
        {
            return "Backwoods Gaming";
        }

        /// <summary>
        /// Returns the package name if provided
        /// This will override the package name in the class name ie public class PackageName.
        /// </summary>
        /// <returns>Package name</returns>
        public static string GetPackageName()
        {
            return "Vol 6 - Highlands Stamps Pack";
        }

        #endregion
		
		#region Class Variables
		
		private static List<string> _packContents = new List<string>()
		{
				"Highlands001",
				"Highlands002",
				"Highlands003",
				"Highlands004",
				"Highlands005",
				"Highlands006",
				"Highlands007",
				"Highlands008",
				"Highlands009",
				"Highlands010",
		};
		
		#endregion
		
		public static void GX_About_About()
		{
			EditorUtility.DisplayDialog("About Vol 6 - Highlands Stamps Pack", "Highlands Terrain Stamps Pack for Gaia is a collection of 10 stamps of areas based off of scans of the Scottish Highlands.", "OK");
		}
		
		
		// Bulk Operations Foldout
		// These operations are for enabling/disabling all stamps from pack
		//
		
		public static void GX_StampBulkOperations_EnableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V006Highlands/Stamps/Valleys/", 1);
			}
		}
		
		public static void GX_StampBulkOperations_DisableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V006Highlands/Stamps/Valleys/", 0);
			}
		}
		
		
		// Enable/Disable Individual Stamps
		// These operations are for enabling/disabling stamps individually
		//
		
		public static void GX_IndividualStamps_Highlands001Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[0], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V006Highlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_Highlands002Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[1], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V006Highlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_Highlands003Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[2], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V006Highlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_Highlands004Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[3], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V006Highlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_Highlands005Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[4], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V006Highlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_Highlands006Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[5], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V006Highlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_Highlands007Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[6], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V006Highlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_Highlands008Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[7], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V006Highlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_Highlands009Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[8], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V006Highlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_Highlands010Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[9], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V006Highlands/Stamps/Valleys/");
		}
		
		
		// Playback Individual Default Sessions
		// These operations are for playing back individual area Gaia sessions
		//      using the default Gaia resources files
		//
		
		public static void GX_PlaySessions_Highlands001Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Highlands001", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V006Highlands");
		}
		
		public static void GX_PlaySessions_Highlands002Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Highlands002", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V006Highlands");
		}
		
		public static void GX_PlaySessions_Highlands003Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Highlands003", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V006Highlands");
		}
		
		public static void GX_PlaySessions_Highlands004Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Highlands004", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V006Highlands");
		}
		
		public static void GX_PlaySessions_Highlands005Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Highlands005", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V006Highlands");
		}
		
		public static void GX_PlaySessions_Highlands006Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Highlands006", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V006Highlands");
		}
		
		public static void GX_PlaySessions_Highlands007Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Highlands007", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V006Highlands");
		}
		
		public static void GX_PlaySessions_Highlands008Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Highlands008", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V006Highlands");
		}
		
		public static void GX_PlaySessions_Highlands009Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Highlands009", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V006Highlands");
		}
		
		public static void GX_PlaySessions_Highlands010Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Highlands010", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V006Highlands");
		}
	}
}
#endif
