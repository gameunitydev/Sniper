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
    public class V005CoastalAreaXtn : MonoBehaviour {

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
            return "Vol 5 - Coastal Area Stamps Pack";
        }

        #endregion
		
		#region Class Variables
		
		private static List<string> _packContents = new List<string>()
		{
				"CoastalArea001",
				"CoastalArea002",
				"CoastalArea003",
				"CoastalArea004",
				"CoastalArea005",
				"CoastalArea006",
				"CoastalArea007",
				"CoastalArea008",
				"CoastalArea009",
				"CoastalArea010",
		};
		
		#endregion
		
		public static void GX_About_About()
		{
			EditorUtility.DisplayDialog("About Vol 5 - Coastal Area Stamps Pack", "On Asset Store: ", "OK");
		}
		
		
		// Bulk Operations Foldout
		// These operations are for enabling/disabling all stamps from pack
		//
		
		public static void GX_StampBulkOperations_EnableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V005CoastalArea/Stamps/Adhoc/", 1);
			}
		}
		
		public static void GX_StampBulkOperations_DisableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V005CoastalArea/Stamps/Adhoc/", 0);
			}
		}
		
		
		// Enable/Disable Individual Stamps
		// These operations are for enabling/disabling stamps individually
		//
		
		public static void GX_IndividualStamps_CoastalArea001Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[0], BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V005CoastalArea/Stamps/Adhoc/");
		}
		
		public static void GX_IndividualStamps_CoastalArea002Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[1], BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V005CoastalArea/Stamps/Adhoc/");
		}
		
		public static void GX_IndividualStamps_CoastalArea003Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[2], BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V005CoastalArea/Stamps/Adhoc/");
		}
		
		public static void GX_IndividualStamps_CoastalArea004Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[3], BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V005CoastalArea/Stamps/Adhoc/");
		}
		
		public static void GX_IndividualStamps_CoastalArea005Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[4], BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V005CoastalArea/Stamps/Adhoc/");
		}
		
		public static void GX_IndividualStamps_CoastalArea006Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[5], BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V005CoastalArea/Stamps/Adhoc/");
		}
		
		public static void GX_IndividualStamps_CoastalArea007Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[6], BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V005CoastalArea/Stamps/Adhoc/");
		}
		
		public static void GX_IndividualStamps_CoastalArea008Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[7], BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V005CoastalArea/Stamps/Adhoc/");
		}
		
		public static void GX_IndividualStamps_CoastalArea009Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[8], BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V005CoastalArea/Stamps/Adhoc/");
		}
		
		public static void GX_IndividualStamps_CoastalArea010Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[9], BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V005CoastalArea/Stamps/Adhoc/");
		}
		
		
		// Playback Individual Default Sessions
		// These operations are for playing back individual area Gaia sessions
		//      using the default Gaia resources files
		//
		
		public static void GX_PlaySessions_CoastalArea001Session()
		{
			BWGGaiaExtUtils.PlayBWSession("CoastalArea001", BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "V005CoastalArea");
		}
		
		public static void GX_PlaySessions_CoastalArea002Session()
		{
			BWGGaiaExtUtils.PlayBWSession("CoastalArea002", BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "V005CoastalArea");
		}
		
		public static void GX_PlaySessions_CoastalArea003Session()
		{
			BWGGaiaExtUtils.PlayBWSession("CoastalArea003", BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "V005CoastalArea");
		}
		
		public static void GX_PlaySessions_CoastalArea004Session()
		{
			BWGGaiaExtUtils.PlayBWSession("CoastalArea004", BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "V005CoastalArea");
		}
		
		public static void GX_PlaySessions_CoastalArea005Session()
		{
			BWGGaiaExtUtils.PlayBWSession("CoastalArea005", BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "V005CoastalArea");
		}
		
		public static void GX_PlaySessions_CoastalArea006Session()
		{
			BWGGaiaExtUtils.PlayBWSession("CoastalArea006", BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "V005CoastalArea");
		}
		
		public static void GX_PlaySessions_CoastalArea007Session()
		{
			BWGGaiaExtUtils.PlayBWSession("CoastalArea007", BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "V005CoastalArea");
		}
		
		public static void GX_PlaySessions_CoastalArea008Session()
		{
			BWGGaiaExtUtils.PlayBWSession("CoastalArea008", BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "V005CoastalArea");
		}
		
		public static void GX_PlaySessions_CoastalArea009Session()
		{
			BWGGaiaExtUtils.PlayBWSession("CoastalArea009", BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "V005CoastalArea");
		}
		
		public static void GX_PlaySessions_CoastalArea010Session()
		{
			BWGGaiaExtUtils.PlayBWSession("CoastalArea010", BWGGaiaExtConst.BWGStampFeatureType.Adhoc, "V005CoastalArea");
		}
	}
}
#endif
