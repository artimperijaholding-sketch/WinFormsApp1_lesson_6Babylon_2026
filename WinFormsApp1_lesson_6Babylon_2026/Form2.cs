using Galactic.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1_lesson_6Babylon_2026
{
    public partial class Form2 : Form
    {
        private readonly string path = string.Empty;
        public Form2()
        {
            InitializeComponent();

            //Galactic.Core.IAncientScript

            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Artifacts");

            //path=Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Galactic");
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            Scan();
        }
        private void buttonLoadScripts_Click(object sender, EventArgs e)
        {
            LoadAncientScripts();
        }
        private void Scan()
        {
            StringBuilder stringBuilder = new StringBuilder();

            string[] artifactsArray = Directory.GetFiles(path);

            foreach (var itemPath in artifactsArray)
            {
                string data = LibraryScanner.ScanLibrary(itemPath);
                stringBuilder.Append(data);
                stringBuilder.Append(Environment.NewLine);
                Debug.WriteLine(data);
            }
            textBoxSkanResult.Text = stringBuilder.ToString();
        }
        private void LoadAncientScripts()
        {
            List<IAncientScript> scripts = new List<IAncientScript>();
            string[] Libraries = Directory.GetFiles(path, "*.dll");
            foreach (var item in Libraries)
            {
                Assembly assembly = Assembly.LoadFrom(item);
                foreach (Type type in assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract))
                {
                    object instance = Activator.CreateInstance(type);
                    if (instance is IAncientScript knownScript)
                    {
                        scripts.Add(knownScript);
                    }
                    else if (type.FullName.Contains("MaintenanceDroid"))
                    {
                        scripts.Add(new MaintenanceDroidAdapter(instance));
                    }
                    else if (type.FullName.Contains("KlingonData"))
                    {
                        scripts.Add(new KlingonDataAdapter(instance));
                    }
                    else if (type.FullName.Contains("KreeIntelligence"))
                    {
                        scripts.Add(new KreeIntellegenceAdapter(instance));
                    }
                    else
                    {
                        Debug.WriteLine("Does not support IAncientScript:" + type.FullName);
                    }
                    //else if (type.FullName)
                }
            }
            IAncientScript ancientScript = scripts[1];
            string origin = ancientScript.GetOrigin();
            string text = ancientScript.Translate("Text to translate");
            Image image = ancientScript.GetEmblem();
            IAncientScript ancientScript1 = scripts[2];
            string origin1 = ancientScript.GetOrigin();
            string text1 = ancientScript.Translate("Text to translate");
            Image image1 = ancientScript.GetEmblem();
            IAncientScript ancientScript2 = scripts[3];
            string origin2 = ancientScript.GetOrigin();
            string text2 = ancientScript.Translate("Text to translate");
            Image image2 = ancientScript.GetEmblem();


            ShowScripts(scripts);
            ShowScripts(scripts);
            ShowScripts(scripts);
        }
        private void ShowScripts(List<IAncientScript> scripts)
        {
            listBoxScripts.Items.Clear();
            foreach (var item in scripts)
            {
                listBoxScripts.Items.Add(item);
            }
        }

        private void listBoxScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxScripts.SelectedItem is IAncientScript ancientScript)
            {
                ShowScriptData(ancientScript);
            }
        }
        private void ShowScriptData(IAncientScript script)
        {
            if (script != null)
            {
                string name = script.GetOrigin();
                objectName.Text = !string.IsNullOrEmpty(name) ? name : "Unknown name";

                Image logo = script.GetEmblem() ?? ImageHelper.GetDefaultImage();
                if (pictureBoxLogo.Image != null)
                {
                    pictureBoxLogo.Image.Dispose();
                }

                pictureBoxLogo.Image = logo;
            }
        }

        private void buttonTranslate_Click(object sender, EventArgs e)
        {
            if (listBoxScripts.SelectedItem is IAncientScript ancientScript)
            {
                Translate(ancientScript);
            }
        }
        private void Translate(IAncientScript script)
        {
            textBoxTranslationResult.Text= script.Translate(textBoxInput.Text)?? "Canot be translation";
        }
    }
    public class MaintenanceDroidAdapter : IAncientScript
    {
        //   [METHODS]:
        //-> Void GetSystemLogs()
        // Parameters:
        //    - String query
        //    - out String status
        private readonly object droid;
        private readonly MethodInfo method;

        public MaintenanceDroidAdapter(object droid)
        {
            this.droid = droid;
            method = droid.GetType().GetMethod("GetSystemLogs");
        }

        public string GetOrigin()=> "Unknown Droid Unit";                     
        public string Translate(string rawData)
        {
            object[] parameters = new object[] { rawData, null };
            method.Invoke(droid, parameters);
            string translationResult = parameters[1]?.ToString() ?? "Error";

            return translationResult;
        }
        public Image GetEmblem() => null;
        public override string ToString() => GetOrigin();
    }
    public class KlingonDataAdapter:IAncientScript
    {
        private readonly object klingon;
        private readonly MethodInfo method;
        private readonly MethodInfo methodImage;

        public KlingonDataAdapter(object klingon)
        {
            this.klingon = klingon;
            method = klingon.GetType().GetMethod("DecodeWarriorCry");
            methodImage = klingon.GetType().GetMethod("GetWarriorInsignia");
        }
        public string GetOrigin() => klingon.GetType().Name;
        public string Translate(string rawData)
        {
            if (method!=null)
            {
                object[] objects = new object[] { rawData };
                return method.Invoke(klingon, objects)?.ToString() ?? "Error";
            }
            return null;
        }
        public Image GetEmblem()
        {
            if (methodImage != null)
            {
                return (Image)methodImage.Invoke(klingon, null);
            }
            return null;
        }
        public override string ToString()
        {
            return GetOrigin() ?? "Error";
        }
    }
    public class KreeIntellegenceAdapter:IAncientScript
    {
        private readonly object kree;
        private readonly MethodInfo method;
        private readonly MethodInfo methodImage;
        public KreeIntellegenceAdapter(object kree)
        {
            this.kree = kree;
            method = kree.GetType().GetMethod("GetEncryptedMessage");
            methodImage = kree.GetType().GetMethod("GetKreeLogo");
        }
        public string GetOrigin() => kree.GetType().Name;
        public string Translate(string rawData)
        {
            if (method != null)
            {
                byte[] bytes = method.Invoke(kree, null) as byte[];
               if(bytes != null)
                {
                    return Encoding.UTF8.GetString(bytes);
                }
                
            }
            return null;
        }
        public Image GetEmblem()
        {
            if (methodImage != null)
            {
                return (Image)methodImage.Invoke(kree, null);
            }
            return null;
        }
        public override string ToString() => GetOrigin() ?? "Error";
    }
}
