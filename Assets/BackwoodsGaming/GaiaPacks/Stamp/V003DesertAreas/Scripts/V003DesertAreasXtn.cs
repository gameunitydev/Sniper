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
    public class V003DesertAreasXtn : MonoBehaviour {

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
            return "Vol 3 - Desert Area Stamps Pack";
        }

        #endregion
		
		#region Class Variables
		
		private static List<string> _packContents = new List<string>()
		{
				"V003Desert001",
				"V003Desert002",
				"V003Desert003",
				"V003Desert004",
				"V003Desert005",
				"V003Desert006",
				"V003Desert007",
				"V003Desert008",
				"V003Desert009",
				"V003Desert010",
				"V003Desert011",
		};
		
		#endregion
		
		public static void GX_About_About()
		{
			EditorUtility.DisplayDialog("About Vol 3 - Desert Area Stamps Pack", "Desert Areas is a collection of 11 stamps modeled after desert areas of the United States.  While mostly flat or slightly rolling terrain, most of the stamps feature some mountain areas along the edges.  They have been made so if you would like to stamp more mountain or mesa features into the terrain, there is plenty of room for them so you can make your deserts as hilly or flat as you like!", "OK");
		}
		
		
		// Bulk Operations Foldout
		// These operations are for enabling/disabling all stamps from pack
		//
		
		public static void GX_StampBulkOperations_EnableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/", 1);
			}
		}
		
		public static void GX_StampBulkOperations_DisableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/", 0);
			}
		}
		
		
		// Enable/Disable Individual Stamps
		// These operations are for enabling/disabling stamps individually
		//
		
		public static void GX_IndividualStamps_V003Desert001Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[0], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/");
		}
		
		public static void GX_IndividualStamps_V003Desert002Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[1], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/");
		}
		
		public static void GX_IndividualStamps_V003Desert003Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[2], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/");
		}
		
		public static void GX_IndividualStamps_V003Desert004Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[3], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/");
		}
		
		public static void GX_IndividualStamps_V003Desert005Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[4], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/");
		}
		
		public static void GX_IndividualStamps_V003Desert006Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[5], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/");
		}
		
		public static void GX_IndividualStamps_V003Desert007Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[6], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/");
		}
		
		public static void GX_IndividualStamps_V003Desert008Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[7], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/");
		}
		
		public static void GX_IndividualStamps_V003Desert009Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[8], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/");
		}
		
		public static void GX_IndividualStamps_V003Desert010Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[9], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/");
		}
		
		public static void GX_IndividualStamps_V003Desert011Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[10], BWGGaiaExtConst.BWGStampFeatureType.Plains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V003DesertAreas/Stamps/Plains/");
		}
		
		
		// Playback Individual Default Sessions
		// These operations are for playing back individual area Gaia sessions
		//      using the default Gaia resources files
		//
		
		public static void GX_PlaySessions_V003Desert001Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V003Desert001", BWGGaiaExtConst.BWGStampFeatureType.Plains, "V003DesertAreas");
		}
		
		public static void GX_PlaySessions_V003Desert002Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V003Desert002", BWGGaiaExtConst.BWGStampFeatureType.Plains, "V003DesertAreas");
		}
		
		public static void GX_PlaySessions_V003Desert003Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V003Desert003", BWGGaiaExtConst.BWGStampFeatureType.Plains, "V003DesertAreas");
		}
		
		public static void GX_PlaySessions_V003Desert004Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V003Desert004", BWGGaiaExtConst.BWGStampFeatureType.Plains, "V003DesertAreas");
		}
		
		public static void GX_PlaySessions_V003Desert005Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V003Desert005", BWGGaiaExtConst.BWGStampFeatureType.Plains, "V003DesertAreas");
		}
		
		public static void GX_PlaySessions_V003Desert006Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V003Desert006", BWGGaiaExtConst.BWGStampFeatureType.Plains, "V003DesertAreas");
		}
		
		public static void GX_PlaySessions_V003Desert007Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V003Desert007", BWGGaiaExtConst.BWGStampFeatureType.Plains, "V003DesertAreas");
		}
		
		public static void GX_PlaySessions_V003Desert008Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V003Desert008", BWGGaiaExtConst.BWGStampFeatureType.Plains, "V003DesertAreas");
		}
		
		public static void GX_PlaySessions_V003Desert009Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V003Desert009", BWGGaiaExtConst.BWGStampFeatureType.Plains, "V003DesertAreas");
		}
		
		public static void GX_PlaySessions_V003Desert010Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V003Desert010", BWGGaiaExtConst.BWGStampFeatureType.Plains, "V003DesertAreas");
		}
		
		public static void GX_PlaySessions_V003Desert011Session()
		{
			BWGGaiaExtUtils.PlayBWSession("V003Desert011", BWGGaiaExtConst.BWGStampFeatureType.Plains, "V003DesertAreas");
		}
	}
}
#endif
