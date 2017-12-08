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
    public class V001VillageAreasXtn : MonoBehaviour {

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
            return "Vol 1 - Village Stamps Pack";
        }

        #endregion
		
		#region Class Variables
		
		private static List<string> _packContents = new List<string>()
		{
				"Vol1Village001",
				"Vol1Village002",
				"Vol1Village003",
				"Vol1Village004",
				"Vol1Village005",
				"Vol1Village006",
				"Vol1Village007",
				"Vol1Village008",
				"Vol1Village009",
				"Vol1Village010",
		};
		
		#endregion
		
		public static void GX_About_About()
		{
			EditorUtility.DisplayDialog("About Vol 1 - Village Stamps Pack", "Village Areas is a collection of 10 stamps of areas containing a mostly flat area in the center of the stamp which is ideal for Gaia's POI spawning system.", "OK");
		}
		
		
		// Bulk Operations Foldout
		// These operations are for enabling/disabling all stamps from pack
		//
		
		public static void GX_StampBulkOperations_EnableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Villages, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V001VillageAreas/Stamps/Villages/", 1);
			}
		}
		
		public static void GX_StampBulkOperations_DisableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Villages, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V001VillageAreas/Stamps/Villages/", 0);
			}
		}
		
		
		// Enable/Disable Individual Stamps
		// These operations are for enabling/disabling stamps individually
		//
		
		public static void GX_IndividualStamps_Vol1Village001Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[0], BWGGaiaExtConst.BWGStampFeatureType.Villages, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V001VillageAreas/Stamps/Villages/");
		}
		
		public static void GX_IndividualStamps_Vol1Village002Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[1], BWGGaiaExtConst.BWGStampFeatureType.Villages, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V001VillageAreas/Stamps/Villages/");
		}
		
		public static void GX_IndividualStamps_Vol1Village003Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[2], BWGGaiaExtConst.BWGStampFeatureType.Villages, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V001VillageAreas/Stamps/Villages/");
		}
		
		public static void GX_IndividualStamps_Vol1Village004Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[3], BWGGaiaExtConst.BWGStampFeatureType.Villages, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V001VillageAreas/Stamps/Villages/");
		}
		
		public static void GX_IndividualStamps_Vol1Village005Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[4], BWGGaiaExtConst.BWGStampFeatureType.Villages, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V001VillageAreas/Stamps/Villages/");
		}
		
		public static void GX_IndividualStamps_Vol1Village006Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[5], BWGGaiaExtConst.BWGStampFeatureType.Villages, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V001VillageAreas/Stamps/Villages/");
		}
		
		public static void GX_IndividualStamps_Vol1Village007Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[6], BWGGaiaExtConst.BWGStampFeatureType.Villages, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V001VillageAreas/Stamps/Villages/");
		}
		
		public static void GX_IndividualStamps_Vol1Village008Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[7], BWGGaiaExtConst.BWGStampFeatureType.Villages, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V001VillageAreas/Stamps/Villages/");
		}
		
		public static void GX_IndividualStamps_Vol1Village009Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[8], BWGGaiaExtConst.BWGStampFeatureType.Villages, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V001VillageAreas/Stamps/Villages/");
		}
		
		public static void GX_IndividualStamps_Vol1Village010Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[9], BWGGaiaExtConst.BWGStampFeatureType.Villages, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V001VillageAreas/Stamps/Villages/");
		}
		
		
		// Playback Individual Default Sessions
		// These operations are for playing back individual area Gaia sessions
		//      using the default Gaia resources files
		//
		
		public static void GX_PlaySessions_Vol1Village001Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol1Village001", BWGGaiaExtConst.BWGStampFeatureType.Villages, "V001VillageAreas");
		}
		
		public static void GX_PlaySessions_Vol1Village002Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol1Village002", BWGGaiaExtConst.BWGStampFeatureType.Villages, "V001VillageAreas");
		}
		
		public static void GX_PlaySessions_Vol1Village003Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol1Village003", BWGGaiaExtConst.BWGStampFeatureType.Villages, "V001VillageAreas");
		}
		
		public static void GX_PlaySessions_Vol1Village004Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol1Village004", BWGGaiaExtConst.BWGStampFeatureType.Villages, "V001VillageAreas");
		}
		
		public static void GX_PlaySessions_Vol1Village005Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol1Village005", BWGGaiaExtConst.BWGStampFeatureType.Villages, "V001VillageAreas");
		}
		
		public static void GX_PlaySessions_Vol1Village006Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol1Village006", BWGGaiaExtConst.BWGStampFeatureType.Villages, "V001VillageAreas");
		}
		
		public static void GX_PlaySessions_Vol1Village007Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol1Village007", BWGGaiaExtConst.BWGStampFeatureType.Villages, "V001VillageAreas");
		}
		
		public static void GX_PlaySessions_Vol1Village008Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol1Village008", BWGGaiaExtConst.BWGStampFeatureType.Villages, "V001VillageAreas");
		}
		
		public static void GX_PlaySessions_Vol1Village009Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol1Village009", BWGGaiaExtConst.BWGStampFeatureType.Villages, "V001VillageAreas");
		}
		
		public static void GX_PlaySessions_Vol1Village010Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol1Village010", BWGGaiaExtConst.BWGStampFeatureType.Villages, "V001VillageAreas");
		}
	}
}
#endif
