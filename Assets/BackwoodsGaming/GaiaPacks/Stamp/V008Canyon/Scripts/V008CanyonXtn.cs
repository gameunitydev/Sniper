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
    public class V008CanyonXtn : MonoBehaviour {

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
            return "Vol 8 - Rivers: Canyon & Gorge Stamps Pack";
        }

        #endregion
		
		#region Class Variables
		
		private static List<string> _packContents = new List<string>()
		{
				"V008Canyon001",
				"V008Canyon002",
				"V008Canyon003",
				"V008Canyon004",
				"V008Canyon005",
				"V008Canyon006",
				"V008Canyon007",
				"V008Canyon008",
				"V008Canyon009",
				"V008Canyon010",
				"V008Canyon011",
				"V008Canyon012",
				"V008Canyon013",
				"V008Canyon014",
				"V008Canyon015",
		};
		
		#endregion
		
		public static void GX_About_About()
		{
			EditorUtility.DisplayDialog("About Vol 8 - Rivers: Canyon & Gorge Stamps Pack", "Rivers: Canyon & Gorge Stamps Pack includes a set of river stamps surrounded by walled canyons or mountainous gorges.", "OK");
		}
		
		
		// Bulk Operations Foldout
		// These operations are for enabling/disabling all stamps from pack
		//
		
		public static void GX_StampBulkOperations_EnableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/", 1);
			}
		}
		
		public static void GX_StampBulkOperations_DisableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/", 0);
			}
		}
		
		
		// Enable/Disable Individual Stamps
		// These operations are for enabling/disabling stamps individually
		//
		
		public static void GX_IndividualStamps_V008Canyon001Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[0], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon002Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[1], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon003Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[2], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon004Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[3], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon005Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[4], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon006Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[5], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon007Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[6], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon008Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[7], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon009Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[8], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon010Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[9], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon011Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[10], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon012Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[11], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon013Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[12], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon014Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[13], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		public static void GX_IndividualStamps_V008Canyon015Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[14], BWGGaiaExtConst.BWGStampFeatureType.Rivers, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V008Canyon/Stamps/Rivers/");
		}
		
		
		// Playback Individual Default Sessions
		// These operations are for playing back individual area Gaia sessions
		//      using the default Gaia resources files
		//
		
		public static void GX_PlaySessions_V008Canyon001Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon001", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon002Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon002", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon003Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon003", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon004Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon004", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon005Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon005", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon006Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon006", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon007Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon007", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon008Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon008", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon009Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon009", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon010Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon010", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon011Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon011", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon012Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon012", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon013Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon013", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon014Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon014", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
		
		public static void GX_PlaySessions_V008Canyon015Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V008Canyon015", BWGGaiaExtConst.BWGStampFeatureType.Rivers, "V008Canyon");
		}
	}
}
#endif
