using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace XML.Parsing {
    public class XMLParsingV1
    {
        public string NONE_SUGGESTION = "-----------------------\n----------------------- ";
        public bool endOfConversation;
        public int nullNum = 0;

        private static TextAsset txtNPCXmlAsset = Resources.Load<TextAsset>("StreamingFiles/XML/npc_speech");
        public XDocument xNPCDoc = XDocument.Parse(txtNPCXmlAsset.text);

        private static TextAsset txtUserXmlAsset = Resources.Load<TextAsset>("StreamingFiles/XML/suggestion_speech");
        public XDocument xUserDoc = XDocument.Parse(txtUserXmlAsset.text);

        public string XMLParsingNPC(string sceneNum, string speechNum)
        {
            var conversation = from q in xNPCDoc.Descendants(sceneNum)
                               select new FindingNPCSpeech
                               {
                                   speech = q.Element(speechNum).Value,
                               };

            foreach (var speech in conversation) return speech.speech.ToString();
            
            return "null";
        }

        public void XMLParsingUser(string sceneNum, string speechNum, string[] suggestions)
        {
            string[] suggestionNum = { "Suggestion1", "Suggestion2", "Suggestion3" };

            for (int i = 0; i < suggestionNum.Length; i++)
            {
                var checkExist = xUserDoc.Descendants(sceneNum).Elements(speechNum).Elements(suggestionNum[i]).FirstOrDefault();

                if (checkExist == null)
                {
                    suggestions[i] = NONE_SUGGESTION;
                    nullNum++;
                }
                else suggestions[i] = checkExist.Value.ToString().Trim();
            }

            if (nullNum == 3)
                endOfConversation = true;
        }
    }

    public class FindingNPCSpeech
    {
        public string speech { get; set; }
    }

    /*
                    var conversation = from q in xDoc.Elements(sceneNum).Elements(speechNum).Elements(suggestionNum[i])
                                       select new FindingSuggestion
                                       {
                                           suggestion = q.Element(suggestionNum[i]).Value,
                                       };

                    foreach (var speech in conversation)
                    {
                        suggestions[i] = speech.suggestion.ToString();
                    }*/
    /*public class FindingSuggestion
    {
        public string suggestion { get; set; }
    }*/
}