﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UniRx.Async;

namespace VRMLoader
{
    public class VRMPreviewLocale : MonoBehaviour {

        [System.Serializable]
        public struct Labels
        {
            public string Headline, Title, Version, Author, Contact, Reference;

            public string PermissionAct,
                PermissionViolent,
                PermissionSexual,
                PermissionCommercial,
                PermissionOther,
                DistributionLicense,
                DistributionOther;
        }

        [System.Serializable]
        public struct Buttons
        {
            public string BtnLoad, BtnCancel;
        }

        [System.Serializable]
        public struct Selections
        {
            public string[] PermissionAct, PermissionUsage, LicenseType;
        }

        [System.Serializable]
        public class LocaleText
        {
            public Labels labels;
            public Buttons buttons;
            public Selections selections;
        }

        private LocaleText _localeText;

        public async void SetLocale(string lang = "en")
        {
            var path = Application.streamingAssetsPath + "/VRMLoaderUI/i18n/" + lang + ".json";
#if UNITY_WEBGL
            UnityWebRequest uwr = UnityWebRequest.Get(path);
            await uwr.SendWebRequest();

            if(uwr.isNetworkError || uwr.isHttpError)
            {
                throw new System.Exception("Cannot local file:" + path);
            }
            
            var json = System.Text.Encoding.UTF8.GetString(uwr.downloadHandler.data, 3, uwr.downloadHandler.data.Length - 3);

#else
            if (!File.Exists(path)) return;
            var json = File.ReadAllText(path);
#endif
            _localeText = JsonUtility.FromJson<LocaleText>(json);
            
            UpdateText(_localeText);
        }

        private void UpdateText(LocaleText localeText)
        {
            var labelsParent = transform.Find("LoadConfirmPanel/Label");
            labelsParent.Find("Headline").GetComponent<Text>().text = localeText.labels.Headline;
            labelsParent.Find("Title").GetComponent<Text>().text = localeText.labels.Title;
            labelsParent.Find("Version").GetComponent<Text>().text = localeText.labels.Version;
            labelsParent.Find("Author").GetComponent<Text>().text = localeText.labels.Author;
            labelsParent.Find("Contact").GetComponent<Text>().text = localeText.labels.Contact;
            labelsParent.Find("Reference").GetComponent<Text>().text = localeText.labels.Reference;
            labelsParent.Find("PermissionAct").GetComponent<Text>().text = localeText.labels.PermissionAct;
            labelsParent.Find("PermissionViolent").GetComponent<Text>().text = localeText.labels.PermissionViolent;
            labelsParent.Find("PermissionSexual").GetComponent<Text>().text = localeText.labels.PermissionSexual;
            labelsParent.Find("PermissionCommercial").GetComponent<Text>().text = localeText.labels.PermissionCommercial;
            labelsParent.Find("PermissionOther").GetComponent<Text>().text = localeText.labels.PermissionOther;
            labelsParent.Find("DistributionLicense").GetComponent<Text>().text = localeText.labels.DistributionLicense;
            labelsParent.Find("DistributionOther").GetComponent<Text>().text = localeText.labels.DistributionOther;

            transform.Find("LoadConfirmPanel/BtnLoad/Text").GetComponent<Text>().text = localeText.buttons.BtnLoad;
            transform.Find("LoadConfirmPanel/BtnCancel/Text").GetComponent<Text>().text = localeText.buttons.BtnCancel;

            var ui = gameObject.GetComponent<VRMPreviewUI>();
            ui.SetSelectionText(localeText.selections);
        }
    }
}