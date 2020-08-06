using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ToggleTag
{
    public class UserManager
    {
        private string FolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Configs", "ToggleTagInfo");
        private string TagFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Configs", "ToggleTagInfo", "UserTags.txt");
        private string OwFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Configs", "ToggleTagInfo", "UserOverwatch.txt");
        private List<string> TagFileLines = new List<string>();
        private List<string> OwFileLines = new List<string>();
        private static Encoding Encoding;
        public Dictionary<string, int> TagPlayers = new Dictionary<string, int>();
        public Dictionary<string, int> OwPlayers = new Dictionary<string, int>();

        public UserManager()
        {
            Encoding = new UTF8Encoding();

            if (!Directory.Exists(FolderLocation))
                Directory.CreateDirectory(FolderLocation);
            if (!File.Exists(TagFileLocation))
                File.Create(TagFileLocation).Close();
            if (!File.Exists(OwFileLocation))
                File.Create(OwFileLocation).Close();
            TagFileLines = File.ReadAllLines(TagFileLocation, Encoding).ToList();
            OwFileLines = File.ReadAllLines(OwFileLocation, Encoding).ToList();

            CheckLinesInFile(TagPlayers, TagFileLines);
            CheckLinesInFile(OwPlayers, OwFileLines);
        }

        public void CheckLinesInFile(Dictionary<string, int> PlayerProperties, List<string> TextInFileLines)
        {
            try
            {
                if (!(TextInFileLines.Count == 0))
                {
                    for (int i = 0; i < TextInFileLines.Count; i++)
                    {
                        char NumFormat = TextInFileLines[i].ElementAt(TextInFileLines[i].LastIndexOf("(") + 1);
                        string UserId = TextInFileLines[i].Substring(0, TextInFileLines[i].LastIndexOf("(") - 1).Trim();
                        if (int.TryParse(NumFormat.ToString(), out int value))
                        {
                            if (!PlayerProperties.ContainsKey(UserId))
                                PlayerProperties.Add(UserId, value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"There was an error with trying to read the file: {e.ToString()}");
            }
        }

        public void AddPlayer(string UserId, int TagValue, ToggleTagEventHandler.PropertyType Property)
        {
            try
            {
                switch (Property)
                {
                    case ToggleTagEventHandler.PropertyType.Tag:
                        if (!TagPlayers.ContainsKey(UserId))
                        {
                            TagPlayers.Add(UserId, TagValue);
                            TagFileLines.Add(FormatValue(UserId, TagValue));
                        }
                        else
                        {
                            int index = TagPlayers.Keys.ToList().IndexOf(UserId);
                            TagPlayers[TagPlayers.ElementAt(index).Key] = TagValue;
                            TagFileLines[index] = FormatValue(TagPlayers.ElementAt(index).Key, TagValue);
                        }
                        File.WriteAllLines(TagFileLocation, TagFileLines);
                        break;
                    case ToggleTagEventHandler.PropertyType.Overwatch:
                        if (!OwPlayers.ContainsKey(UserId))
                        {
                            OwPlayers.Add(UserId, TagValue);
                            OwFileLines.Add(FormatValue(UserId, TagValue));
                        }
                        else
                        {
                            int index = OwPlayers.Keys.ToList().IndexOf(UserId);
                            OwPlayers[OwPlayers.ElementAt(index).Key] = TagValue;
                            OwFileLines[index] = FormatValue(OwPlayers.ElementAt(index).Key, TagValue);
                        }
                        File.WriteAllLines(OwFileLocation, OwFileLines);
                        break;
                }
            }
            catch (Exception e)
            {
                Log.Error($"There was an error with trying to add the value into the file: {e.ToString()}");
            }

        }

        public string FormatValue(string UserId, int TagValue)
        {
            StringBuilder ValueBuilder = new StringBuilder();
            ValueBuilder.Append(UserId);
            ValueBuilder.Append(" ");
            ValueBuilder.Append("(");
            ValueBuilder.Append(TagValue);
            ValueBuilder.Append(")");
            return ValueBuilder.ToString();
        }

        public Dictionary<string, int> GetDictionary()
        {
            return TagPlayers;
        }
    }
}
