using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VocabularyDictionary;

namespace Game
{
    class GameRules
    {
        MasterDictionary Data;

        public Dictionary<string, List<int>> Modes;
        public string mode = "";

        public Word AnswerWord;
        public Word Choice1;
        public Word Choice2;
        public Word Choice3;

        static int Kanji = 1;
        static int Reading = 2;
        static int English = 3;

        List<string> wordList;

        Random random;

        int NumberOfPlays = 0;
        public int getNumberofPlays
        {
            get { return NumberOfPlays; }
        }

        public void GameMode(string GenerationType)
        {
            //Console.WriteLine("GameMode:" + mode);
            //_mode = mode;
            createWordList(GenerationType);
            Distribute();
            //GameScene();
        }

        void createWordList(string vocabType)
        {
            if (vocabType == "Occurance")
            {
                wordList = Data.TopOccurredWords();
                System.Diagnostics.Debug.WriteLine("Occurance Test:" + wordList.Count());
            }
            else if (vocabType == "Mastery")
            {
                wordList = Data.MasteryList();
                System.Diagnostics.Debug.WriteLine("Mastery Test:" + wordList.Count());
            }

        }

        void Distribute()
        {
            int listMax = wordList.Count();

            Queue<int> WordIndex = new Queue<int>();

            for (int i = 0; i < 3; i++)
            {
                int wordIndex = random.Next(0, listMax);

                if (WordIndex.Contains(wordIndex))
                {
                    wordIndex = random.Next(0, listMax);
                }

                WordIndex.Enqueue(wordIndex);

            }

            var word1 = Data.PullWord(wordList[WordIndex.Dequeue()]);
            var word2 = Data.PullWord(wordList[WordIndex.Dequeue()]);
            var word3 = Data.PullWord(wordList[WordIndex.Dequeue()]);

            Word[] words = new Word[] { word1, word2, word3 };

            Queue<Word> wordPick = new Queue<Word>();

            for (int i = 0; i < 3; i++)
            {
                int indexA = random.Next(0, words.Length);
                int indexB = random.Next(0, words.Length);

                var wordA = words[indexA];
                var wordB = words[indexB];

                words[indexA] = wordB;
                words[indexB] = wordA;
            }

            AnswerWord = word1;
            Choice1 = words[0];
            Choice2 = words[1];
            Choice3 = words[2];

        }

        public bool doesMatch(Word A, Word B)
        {
            if (A.getWordID == B.getWordID)
            {
                AnswerWord.updateMastery();
                Data.PutBack(AnswerWord);
                NumberOfPlays++;
                Distribute();
                return true;
            }
            else
            {
                NumberOfPlays++;
                Distribute();
                return false;
            }


        }

        public GameRules(string GenerationType, string PracticeType, MasterDictionary Dictionary)
        {
            Data = Dictionary;
            random = new Random();

            Modes = new Dictionary<string, List<int>>() { { "Kanji", new List<int>(){Kanji, Reading, English } },
                                                            { "Reading", new List<int>(){ Reading, English, Kanji } },
                                                            { "English", new List<int>(){ English, Reading, Kanji } } };

            mode = PracticeType;

            GameMode(GenerationType);
        }



    }
}
