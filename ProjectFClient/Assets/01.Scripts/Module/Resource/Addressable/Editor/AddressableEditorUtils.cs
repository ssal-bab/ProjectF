using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

public static class AddressableEditorUtils
{
    public static Object GetAssetFromKey(string key)
    {
        if(string.IsNullOrEmpty(key))
            return null;

        // AddressableAssetSettings 인스턴스 가져오기
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
            return null;

        // 모든 그룹을 순회하며 key에 해당하는 엔트리 찾기
        foreach (var group in settings.groups)
        {
            if (group == null) continue;

            foreach (var entry in group.entries)
            {
                if (entry.address == key)
                {
                    // 원본 에셋 반환
                    return entry.MainAsset;
                }
            }
        }

        return null;
    }

    public static string GetKeyFromAsset(Object asset)
    {
        if (asset == null)
            return null;

        // 에셋의 GUID 가져오기
        string assetPath = AssetDatabase.GetAssetPath(asset);
        string guid = AssetDatabase.AssetPathToGUID(assetPath);

        if (string.IsNullOrEmpty(guid))
            return null;

        // AddressableAssetSettings 인스턴스 가져오기
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
            return null;

        // 모든 그룹을 순회하며 GUID에 해당하는 엔트리 찾기
        foreach (var group in settings.groups)
        {
            if (group == null) continue;

            foreach (var entry in group.entries)
            {
                if (entry.guid == guid)
                {
                    // Addressables key 반환
                    return entry.address;
                }
            }
        }

        return null;
    }
}