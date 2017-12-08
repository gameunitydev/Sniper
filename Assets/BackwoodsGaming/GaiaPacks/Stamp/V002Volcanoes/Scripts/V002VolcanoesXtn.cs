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
    public class V002VolcanoesXtn : MonoBehaviour {

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
            return "Vol 2 - Mountain and Volcano Stamps Pack";
        }

        #endregion
		
		#region Class Variables
		
		private static List<string> _packContents = new List<string>()
		{
				"Vol2Volcanoes001",
				"Vol2Volcanoes002",
				"Vol2Volcanoes003",
				"Vol2Volcanoes004",
				"Vol2Volcanoes005",
				"Vol2Volcanoes006",
				"Vol2Volcanoes007",
				"Vol2Volcanoes008",
				"Vol2Volcanoes009",
				"Vol2Volcanoes010",
		};
		
		#endregion
		
		public static void GX_About_About()
		{
			EditorUtility.DisplayDialog("About Vol 2 - Mountain and Volcano Stamps Pack", "Mountain and Volcanoes is a collection of 10 stamps consisting of volcanoes or dormant volcanic mountains.  The volcanoes and mountains in the pack take up most, if not all, of the stamp making them perfect for stamping as a complete scene or blended into existing terrains using either the stamp randomizer or individual masked/blended stamping", "OK");
		}
		
		
		// Bulk Operations Foldout
		// These operations are for enabling/disabling all stamps from pack
		//
		
		public static void GX_StampBulkOperations_EnableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Mountains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V002Volcanoes/Stamps/Mountains/", 1);
			}
		}
		
		public static void GX_StampBulkOperations_DisableAllStampsFromPack()
		{
			for ( int itemCnt = 0; itemCnt < _packContents.Count; itemCnt++ )
			{
				BWGGaiaExtUtils.MoveBWStamp(_packContents[itemCnt], BWGGaiaExtConst.BWGStampFeatureType.Mountains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V002Volcanoes/Stamps/Mountains/", 0);
			}
		}
		
		
		// Enable/Disable Individual Stamps
		// These operations are for enabling/disabling stamps individually
		//
		
		public static void GX_IndividualStamps_Vol2Volcanoes001Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[0], BWGGaiaExtConst.BWGStampFeatureType.Mountains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V002Volcanoes/Stamps/Mountains/");
		}
		
		public static void GX_IndividualStamps_Vol2Volcanoes002Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[1], BWGGaiaExtConst.BWGStampFeatureType.Mountains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V002Volcanoes/Stamps/Mountains/");
		}
		
		public static void GX_IndividualStamps_Vol2Volcanoes003Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[2], BWGGaiaExtConst.BWGStampFeatureType.Mountains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V002Volcanoes/Stamps/Mountains/");
		}
		
		public static void GX_IndividualStamps_Vol2Volcanoes004Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[3], BWGGaiaExtConst.BWGStampFeatureType.Mountains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V002Volcanoes/Stamps/Mountains/");
		}
		
		public static void GX_IndividualStamps_Vol2Volcanoes005Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[4], BWGGaiaExtConst.BWGStampFeatureType.Mountains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V002Volcanoes/Stamps/Mountains/");
		}
		
		public static void GX_IndividualStamps_Vol2Volcanoes006Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[5], BWGGaiaExtConst.BWGStampFeatureType.Mountains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V002Volcanoes/Stamps/Mountains/");
		}
		
		public static void GX_IndividualStamps_Vol2Volcanoes007Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[6], BWGGaiaExtConst.BWGStampFeatureType.Mountains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V002Volcanoes/Stamps/Mountains/");
		}
		
		public static void GX_IndividualStamps_Vol2Volcanoes008Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[7], BWGGaiaExtConst.BWGStampFeatureType.Mountains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V002Volcanoes/Stamps/Mountains/");
		}
		
		public static void GX_IndividualStamps_Vol2Volcanoes009Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[8], BWGGaiaExtConst.BWGStampFeatureType.Mountains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V002Volcanoes/Stamps/Mountains/");
		}
		
		public static void GX_IndividualStamps_Vol2Volcanoes010Stamp()
		{
			BWGGaiaExtUtils.MoveBWStamp(_packContents[9], BWGGaiaExtConst.BWGStampFeatureType.Mountains, "Assets/BackwoodsGaming/GaiaPacks/Stamp/V002Volcanoes/Stamps/Mountains/");
		}
		
		
		// Playback Individual Default Sessions
		// These operations are for playing back individual area Gaia sessions
		//      using the default Gaia resources files
		//
		
		public static void GX_PlaySessions_Vol2Volcanoes001Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol2Volcanoes001", BWGGaiaExtConst.BWGStampFeatureType.Mountains, "V002Volcanoes");
		}
		
		public static void GX_PlaySessions_Vol2Volcanoes002Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol2Volcanoes002", BWGGaiaExtConst.BWGStampFeatureType.Mountains, "V002Volcanoes");
		}
		
		public static void GX_PlaySessions_Vol2Volcanoes003Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol2Volcanoes003", BWGGaiaExtConst.BWGStampFeatureType.Mountains, "V002Volcanoes");
		}
		
		public static void GX_PlaySessions_Vol2Volcanoes004Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol2Volcanoes004", BWGGaiaExtConst.BWGStampFeatureType.Mountains, "V002Volcanoes");
		}
		
		public static void GX_PlaySessions_Vol2Volcanoes005Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol2Volcanoes005", BWGGaiaExtConst.BWGStampFeatureType.Mountains, "V002Volcanoes");
		}
		
		public static void GX_PlaySessions_Vol2Volcanoes006Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol2Volcanoes006", BWGGaiaExtConst.BWGStampFeatureType.Mountains, "V002Volcanoes");
		}
		
		public static void GX_PlaySessions_Vol2Volcanoes007Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol2Volcanoes007", BWGGaiaExtConst.BWGStampFeatureType.Mountains, "V002Volcanoes");
		}
		
		public static void GX_PlaySessions_Vol2Volcanoes008Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol2Volcanoes008", BWGGaiaExtConst.BWGStampFeatureType.Mountains, "V002Volcanoes");
		}
		
		public static void GX_PlaySessions_Vol2Volcanoes009Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol2Volcanoes009", BWGGaiaExtConst.BWGStampFeatureType.Mountains, "V002Volcanoes");
		}
		
		public static void GX_PlaySessions_Vol2Volcanoes010Session()
		{
			BWGGaiaExtUtils.PlayBWSession("Vol2Volcanoes010", BWGGaiaExtConst.BWGStampFeatureType.Mountains, "V002Volcanoes");
		}
	}
}
#endif
