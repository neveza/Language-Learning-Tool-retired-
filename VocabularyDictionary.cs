using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization;



namespace VocabularyDictionary
{
    
   [DataContract]
    public class MasterDictionary
    {
        [DataMember]
        Dictionary<string, string[]> DictionaryCatalogue;

        public int getDictionaryCount
        {
            get { return DictionaryCatalogue.Count(); }
        }

        string[] WordData;
        string _keyName;
        public string getKeyName
        {
            get { return _keyName; }
        }
        [DataMember]
        int keyCount;

        DateTime Time;

        void Add(string Kanji, string Reading, string English)
        {
            _keyName = "Word" + keyCount;

            WordData = new string[5];
            WordData[0] = "0";
            WordData[1] = "0";
            WordData[2] = Kanji;
            WordData[3] = Reading;
            WordData[4] = English;

            //DictionaryCatalogue[_keyName]
            _keyName = "Word" + keyCount;
            DictionaryCatalogue.Add(_keyName, WordData);
            keyCount++;

        } // May remove

        public void Add(List<List<XElement>> CellRows)
        {

            for (int limit = 0; limit < CellRows.Count(); limit++)
            {
                WordData = new string[6];
                /*
                 * Data order
                 * 0 = Occurance
                 * 1 = Kanji
                 * 2 = Reading
                 * 3 = English
                 * 4 = Mastery
                 * 5 = TimeUpdated
                 */

                string mastery = "1";
                if (CellRows[limit].Count < 6)
                {

                    for (int i = 0; i < (CellRows[limit].Count - 1); i++)
                    {
                        WordData[i] = CellRows[limit][i].Value;
                    }
                    WordData[4] = mastery;
                    WordData[5] = Time.Day.ToString();
                    

                    _keyName = "Word" + keyCount;
                    DictionaryCatalogue.Add(_keyName, WordData);
                    keyCount++;
                }

            }

        }

        public void LoadODS(System.IO.Stream File)
        {
            ODSReader.ODSReader reader = new ODSReader.ODSReader(File);
            Add(reader.CellRows);
        }

        List<int> TopOccurances(String Mode = "Occurances")
        {
            int occA = 0;
            int occB = 0;
            int ValueSlot = 0;

            if (Mode == "Mastery")
            {
                ValueSlot = 4;
            }
            else if (Mode == "Occurances")
            {
                ValueSlot = 0;
            }

            var occured = new List<int>();

            foreach (var Word in DictionaryCatalogue.Keys)
            {

                if (DictionaryCatalogue[Word][ValueSlot] != " ")
                {
                    //occA = Convert.ToInt32(DictionaryCatalogue[Word][ValueSlot]);

                    var result = Int32.TryParse(DictionaryCatalogue[Word][ValueSlot], out occA);

                    if (result == true)
                    {
                        if (occA > occB)
                        {
                            occured.Add(occA);
                            occB = occA;
                        }
                    }
                }

            }

            return occured;

        }

        public List<string> MasteryList()
        {
            var mastery = new List<string>();

            int maxMastery = 5;

            foreach (var keys in DictionaryCatalogue.Keys)
            {
                if (mastery.Count() < 25)
                {
                    var test = DictionaryCatalogue[keys][4];
                    //Console.WriteLine("Test: " + test);
                    int master = Convert.ToInt16(DictionaryCatalogue[keys][4]);
                    if (master < maxMastery)
                    {

                        mastery.Add(keys);

                    }
                }
            }


            return mastery;
        }


        public List<string> TopOccurredWords(string Mode = "Occurances")
        {
            int valueIndex = 0;
            if (Mode == "Occurances")
            {
                valueIndex = 0;
            }
            var top = new List<string>();
            var occuredCount = TopOccurances(Mode);

           // Console.WriteLine("GettingList");

            for (int i = 0; i < occuredCount.Count(); i++)
            {
                //Console.WriteLine("For check passed");
                if (top.Count() < 25)
                {
                    //Console.WriteLine("if check passed");
                    foreach (var key in DictionaryCatalogue.Keys)
                    {
                        if (top.Count() < 25)
                        {
                            int value;
                            var result = Int32.TryParse(DictionaryCatalogue[key][valueIndex], out value);

                            if (result == true)
                            {
                                if (value == occuredCount[i])
                                {

                                    top.Add(key);
                                }
                            }
                        }
                    }
                }
            }

            return top;
        }


        public Word PullWord(string keyName)
        {
            return new Word(
            keyName,
            DictionaryCatalogue[keyName][4],
            DictionaryCatalogue[keyName][1],
            DictionaryCatalogue[keyName][2],
            DictionaryCatalogue[keyName][3]
            );

        }

        public void PutBack(Word word)
        {
            DictionaryCatalogue[word.getWordID][4] = word.getMastery;
            DictionaryCatalogue[word.getWordID][5] = Time.Day.ToString();
        }

        public void updateOverFiveOccurrance()
        {
            foreach (var key in DictionaryCatalogue.Keys)
            {
                if (DictionaryCatalogue[key][4] == "5")
                {
                    int lastDayUpdate = Convert.ToInt16(DictionaryCatalogue[key][5]);

                    int timeDifference = Time.Day - lastDayUpdate; 

                    if (timeDifference >= 10) //over 10 days
                    {
                        DictionaryCatalogue[key][4] = "0"; //resets mastery to 0
                    }

                }
            }
        }

        public MasterDictionary()
        {
            DictionaryCatalogue = new Dictionary<string, string[]>();
            Time = DateTime.Now;


            keyCount = 0;

        }

    }

    public class DataManagement
    {

        public static async void Save(string FileName, MasterDictionary data)
        {        
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            //await storageFolder.RenameAsync("LearnerToolsFolder");
            //System.Diagnostics.Debug.WriteLine(storageFolder.Path);

            using (var Stream = await storageFolder.OpenStreamForWriteAsync(FileName, Windows.Storage.CreationCollisionOption.ReplaceExisting))
            {


                System.Runtime.Serialization.DataContractSerializer Serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(List<MasterDictionary>));

                Serializer.WriteObject(Stream, data);

            }

            

            //Windows.Storage.StorageFile File = await storageFolder.CreateFileAsync(FileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);

        }

        public static async Task<List<MasterDictionary>> Load(string FileName)
        {
            List<MasterDictionary> DataList = new List<MasterDictionary>();
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            var file = await storageFolder.TryGetItemAsync(FileName);

            if (file != null)
            {
                
                Windows.Storage.FileProperties.BasicProperties basicProperties = await file.GetBasicPropertiesAsync();
                if (basicProperties.Size > 0)
                {
                    
                    using (var Stream = await storageFolder.OpenStreamForReadAsync(FileName))
                    {

                        //starts to hang here
                        System.Xml.XmlDictionaryReader XmlReader = System.Xml.XmlDictionaryReader.CreateTextReader(Stream, new System.Xml.XmlDictionaryReaderQuotas());
                        //doesn't get further than here??
                        System.Runtime.Serialization.DataContractSerializer Serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(List<MasterDictionary>));
                        var data = (MasterDictionary)Serializer.ReadObject(XmlReader);
                        DataList.Add(data);
                    }
                    

                    return DataList;
                }
                else
                {
                    return new List<MasterDictionary>();
                }

                //return new List<MasterDictionary>();
            }
            else
            {
                //Console.WriteLine("No File Found");
                return new List<MasterDictionary>();
            }

        }

        public static async Task<List<MasterDictionary>> LoadAsync(string FileName)
        {
            return await Task.Run(() => Load(FileName)).ConfigureAwait(continueOnCapturedContext: false);
        }

    }
  

    public class Word
    {
        string _WordID;
        public string getWordID
        {
            get { return _WordID; }
        }

        string _Mastery;
        public string getMastery
        {
            get { return _Mastery; }
        }

        string _Kanji;
        public string getKanji
        {
            get { return _Kanji; }
        }

        string _Reading;
        public string getReading
        {
            get { return _Reading; }
        }

        string _English;
        public string getEnglish
        {
            get { return _English; }
        }

        public string[] getMembersAsArray;

        public void updateMastery()
        {
            int newMastery = Convert.ToInt16( _Mastery) + 1;
            _Mastery = newMastery.ToString();

        }

        public Word(string wordID, string mastery, string kanji, string reading, string english)
        {
            _WordID = wordID;
            _Mastery = mastery;
            _Kanji = kanji;
            _Reading = reading;
            _English = english;

            getMembersAsArray = new string[] { _WordID, _Kanji, _Reading, _English, _Mastery };

        }
    }
}
