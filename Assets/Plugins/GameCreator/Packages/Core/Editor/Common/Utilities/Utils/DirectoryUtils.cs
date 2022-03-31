using UnityEngine;
using System.IO;
using System.Text;
using GameCreator.Runtime.Common;
using UnityEditor;

namespace GameCreator.Editor.Common
{
    public static class DirectoryUtils
    {
        public static void RequirePath(string path)
        {
            string[] folders = path.Split('/');
            string previous = string.Empty;

            foreach (string folder in folders)
            {
                string trail = PathUtils.Combine(previous, folder);
                if (!AssetDatabase.IsValidFolder(trail))
                {
                    AssetDatabase.CreateFolder(previous, folder);
                }

                AssetDatabase.Refresh();
                previous = trail;
            }
        }
    }
}