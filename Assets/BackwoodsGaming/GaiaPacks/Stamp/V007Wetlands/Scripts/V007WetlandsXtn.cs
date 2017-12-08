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
    public class V007WetlandsXtn : MonoBehaviour {

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
            return "Vol 7 - Wetlands Stamps Pack";
        }

        #endregion
		
		#region Class Variables
		
		private static List<string> _packContents = new List<string>()
		{
				"V007Wetlands001",
				"V007Wetlands002",
				"V007Wetlands003",
				"V007Wetlands004",
				"V007Wetlands005",
				"V007Wetlands006",
				"V007Wetlands007",
				"V007Wetlands008",
				"V007Wetlands009",
				"V007Wetlands010",
				"V007Wetlands011",
		};
		
		#endregion
		
		public static void GX_About_About()
		{
			EditorUtility.DisplayDialog("About Vol 7 - Wetlands Stamps Pack", "This pack contains stamps made for Gaia for marsh, swamp, and low lying wetland type areas.", "OK");
		}
		
		
		// Bulk Operations Foldout
		// These operations are for enabling/disabling all stamps from pack
		//
		
		public static void GX_StampBulkOperations_EnableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/", 1);
			}
		}
		
		public static void GX_StampBulkOperations_DisableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/", 0);
			}
		}
		
		
		// Enable/Disable Individual Stamps
		// These operations are for enabling/disabling stamps individually
		//
		
		public static void GX_IndividualStamps_V007Wetlands001Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[0], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_V007Wetlands002Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[1], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_V007Wetlands003Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[2], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_V007Wetlands004Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[3], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_V007Wetlands005Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[4], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_V007Wetlands006Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[5], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_V007Wetlands007Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[6], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_V007Wetlands008Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[7], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_V007Wetlands009Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[8], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_V007Wetlands010Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[9], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/");
		}
		
		public static void GX_IndividualStamps_V007Wetlands011Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[10], BWGGaiaExtConst.BWGStampFeatureType.Valleys, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V007Wetlands/Stamps/Valleys/");
		}
		
		
		// Playback Individual Default Sessions
		// These operations are for playing back individual area Gaia sessions
		//      using the default Gaia resources files
		//
		
		public static void GX_PlaySessions_V007Wetlands001Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V007Wetlands001", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V007Wetlands");
		}
		
		public static void GX_PlaySessions_V007Wetlands002Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V007Wetlands002", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V007Wetlands");
		}
		
		public static void GX_PlaySessions_V007Wetlands003Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V007Wetlands003", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V007Wetlands");
		}
		
		public static void GX_PlaySessions_V007Wetlands004Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V007Wetlands004", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V007Wetlands");
		}
		
		public static void GX_PlaySessions_V007Wetlands005Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V007Wetlands005", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V007Wetlands");
		}
		
		public static void GX_PlaySessions_V007Wetlands006Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V007Wetlands006", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V007Wetlands");
		}
		
		public static void GX_PlaySessions_V007Wetlands007Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V007Wetlands007", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V007Wetlands");
		}
		
		public static void GX_PlaySessions_V007Wetlands008Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V007Wetlands008", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V007Wetlands");
		}
		
		public static void GX_PlaySessions_V007Wetlands009Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V007Wetlands009", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V007Wetlands");
		}
		
		public static void GX_PlaySessions_V007Wetlands010Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V007Wetlands010", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V007Wetlands");
		}
		
		public static void GX_PlaySessions_V007Wetlands011Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V007Wetlands011", BWGGaiaExtConst.BWGStampFeatureType.Valleys, "V007Wetlands");
		}
	}
}
#endif
