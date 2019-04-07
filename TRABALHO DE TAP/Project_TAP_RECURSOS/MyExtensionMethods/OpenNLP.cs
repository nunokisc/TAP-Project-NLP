using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenNLP.Tools.Tokenize;
using OpenNLP.Tools.SentenceDetect;
using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.NameFind;
using System.Text.RegularExpressions;

namespace MyExtensionMethods
{
    public static class OpenNLP
    {
        //https://github.com/AlexPoint/OpenNlp
        //Necessário instalar o nuGet package OpenNLP
        //Depois é necessário acrescentar alguns usings (dependendo da funcionalidade a utilizar)

        static string Path = @"C:\Users\kisc\Source\Repos\turma_TAP1617\MyExtensionMethods\OpenNLP\";

        /// <summary>
        /// A sentence splitter splits a paragraph in sentences. Technically, the sentence detector will compute the likelihood that a specific character ('.', '?' or '!' in the case of English) marks the end of a sentence.
        /// </summary>
        /// <param name="txt">Texto a dividir</param>
        /// <returns>a set of tokens</returns>
        #region Sentence Splitter
        public static string[] SplitTextIntoSentences1(this string txt)
        {
            //É necessário dividir primeiro por \n uma vez que o SentenceDetect não faz isso internamente
            string[] sen = txt.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            //A sentence splitter splits a paragraph in sentences. Technically, the sentence detector will compute the likelihood that a specific character ('.', '?' or '!' in the case of English) marks the end of a sentence.
            //Models for English. Tive que fazer download deles
            //Não esquecer de adicionar using OpenNLP.Tools.SentenceDetect;
            var modelPath = Path + "EnglishSD.nbin";
            var sentenceDetector = new EnglishMaximumEntropySentenceDetector(modelPath);

            List<string> sentencesTemp = new List<string>(); //Temos que criar uma lista uma vez que à partida não sabemos quantas sentences vamos ter (o que invalida a criação de um array)
            foreach (string text in sen)
            {
                string[] sentences = sentenceDetector.SentenceDetect(text); //retorna através do método as respetivas sentences para o txt em questão
                sentencesTemp.AddRange(sentences.ToList<string>()); //Copia as sentences do txt em questão para a lista
            }

            return sentencesTemp.ToArray(); //Converter para array
        }
        #endregion

        #region Tokenizer (sem modelo)
        /// <summary>
        /// a specific rule-based tokenizer (based on regexes) was created and has a better precision. This tokenizer doesn't need any model.
        /// </summary>
        /// <param name="txt">Texto a tokenizar</param>
        /// <param name="hyphen">por defeito a false: significa que não vai fazer split por hyphen</param>
        /// <returns>a set of tokens</returns>
        public static string[] Tokenizer_RuleBased(this string txt, bool hyphen=false)
        {
            //For English, a specific rule-based tokenizer (based on regexes) was created and has a better precision. This tokenizer doesn't need any model.
            //Não esquecer de adicionar using OpenNLP.Tools.Tokenize;
            var tokenizer = new EnglishRuleBasedTokenizer(false);
            string[] tokens = tokenizer.Tokenize(txt);

            return tokens;
        }
        #endregion


        #region Tokenizer (com modelo)
        /// <summary>
        /// A tokenizer breaks a text into words, symbols or other meaningful elements. The historical tokenizers are based on the maxent algorithm.
        /// </summary>
        /// <param name="txt">Texto a tokenizar</param>
        /// <returns>a set of tokens</returns>
        public static string[] Tokenizer(this string txt, string path)
        {
            //Models for English. Tive que fazer download deles
            //Não esquecer de adicionar using OpenNLP.Tools.Tokenize;
            var modelPath = path + "EnglishTok.nbin";
            var tokenizer = new EnglishMaximumEntropyTokenizer(modelPath);
            string[] tokens = tokenizer.Tokenize(txt);

            return tokens;
        }

        public static bool checkContainsALetter(string token)
        {
            return Regex.IsMatch(token, "^[a-zA-Z0-9éÉáÁãÃâÂíÍúÚóÓêÊôÔ]*$");
            // https://social.msdn.microsoft.com/Forums/vstudio/en-US/8daa5f84-8535-47b0-b4bf-1669a9682911/check-if-a-string-contains-a-letter?forum=csharpgeneral
        }
        #endregion

        #region PoS
        /// <summary>
        /// A part of speech tagger assigns a part of speech (noun, verb etc.) to each token in a sentence. For the full list of part of speech abbreviations, please refer to the Penn Treebank Project (https://www.ling.upenn.edu/courses/Fall_2003/ling001/penn_treebank_pos.html)
        /// </summary>
        /// <param name="txt">Texto de input</param>
        /// <returns>a set of tokens</returns>
        public static string[] PoS(this string txt)
        {
            //Models for English. Tive que fazer download deles
            //Não esquecer de adicionar using OpenNLP.Tools.PosTagger;
            var modelPath2 = Path + "EnglishPOS.nbin";
            var posTagger = new EnglishMaximumEntropyPosTagger(modelPath2);
            string[] tokens = txt.Tokenizer_RuleBased(); //Faz uma chamada ao método anterior
            string[] pos = posTagger.Tag(tokens);

            return tokens;
        }
        #endregion

        #region NER
        /// <summary>
        /// Name entity recognition identifies specific entities in sentences. With the current models, you can detect persons, dates, locations, money, percentages and time
        /// </summary>
        /// <param name="txt">Texto de input</param>
        /// <param name="tags">string[] com a designação das tags possíveis de etiquetar: "date", "location", "money", "organization", "percentage", "person", "time"</param>
        /// <returns>retorna o texto etiquetado</returns>
        public static string NER(this string txt, string[] tags)
        {
            //Tive que fazer download da diretoria
            //Não esquecer de adicionar using OpenNLP.Tools.NameFind;
            var modelPath3 = Path + @"NameFind\";
            var nameFinder = new EnglishNameFinder(modelPath3);
            string ner = nameFinder.GetNames(tags, txt);

            return ner;
        }
        #endregion
    }
}
