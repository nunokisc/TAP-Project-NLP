using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MyExtensionMethods;
using System.Data.SqlClient;
using System.IO;

namespace WcfService3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : I_GetResults
    {
        // USER DA BASE DE DADOS -----------------
        static string datasource = "KISC-PC";
        //----------------------------------------
        // CAMINHO DO OPENNLP --------------------------------------------------------------------------
        static string path = @"C:\Users\Nuno\Documents\Visual Studio 2015\Projects\WcfService3\WcfService3\MyExtensionMethods\OpenNLP\";
        //----------------------------------------------------------------------------------------------
        // CAMINHO DO STOPWORDS ------------------------------------------------------------------------
        static string stopwords = @"C:\Users\Nuno\Documents\Visual Studio 2015\Projects\WcfService3\WcfService3\stopwords-pt.txt";
        //----------------------------------------------------------------------------------------------

        static HashSet<string> setStopWords()
        {
            HashSet<string> hashSet = new HashSet<string>();
            string[] file = File.ReadLines(stopwords).ToArray();
            foreach (var result in file)
            {
                hashSet.Add(result);
            }
            return hashSet;
            //stop words https://github.com/stopwords-iso/stopwords-pt
        }

        public string queryToReturnResultsWithSequence(string input)
        {
            string result = "";
            int lines = 1;
            int som = 1;
            int aux = 0; int aux1 = 1;
            string query = "";
            HashSet<string> hashSet = setStopWords();
            string[] tokens = OpenNLP.Tokenizer(input, path);
            foreach (var results in tokens)
            {
                if (OpenNLP.checkContainsALetter(results))
                {
                    if (hashSet.Contains(results.ToLower()))
                    {
                        if (aux1 != 1)
                        {
                            aux++;
                        }
                    }
                    else
                    {
                        if (lines == 1)
                        {
                            query = "SELECT sl1.docid, d_title, d_url FROM (SELECT offs.offset, offs.docid FROM words as wd " +
                                        "inner join relation as rl ON rl.wordid = wd.id " +
                                            "inner join docs as dc ON rl.docid = dc.id " +
                                                "inner join offsetword as offs ON offs.wordid = wd.id AND offs.docid = dc.id " +
                                                    "where  word = '" + results + "' ) as sl" + aux1 +
                                                    " inner join docs as dc ON sl1.docid = dc.id	";
                            aux1++;
                            lines++;
                        }
                        else
                        {
                            query = query + " inner join (SELECT offs.offset, offs.docid FROM words as wd " +
                                        "inner join relation as rl ON rl.wordid = wd.id " +
                                            "inner join docs as dc ON rl.docid = dc.id " +
                                                "inner join offsetword as offs ON offs.wordid = wd.id AND offs.docid = dc.id " +
                                                    "where word = '" + results + "') as sl" + aux1 +
                                                        " ON sl1.docid = sl" + aux1 + ".docid AND sl" + aux1 + ".offset = sl1.offset +" + ((aux1 - 1) + aux);
                            aux1++;
                        }
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection("Data Source=" + datasource + ";Initial Catalog=snipetts;Integrated Security=SSPI"))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check is the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        result = result + "{'documents':[";
                        // Read advances to the next row.
                        while (reader.Read())
                        {
                            result = result + "{" + "'docid': '" + reader.GetInt32(0) + "', 'title':'" + reader.GetString(reader.GetOrdinal("d_title")) + "', 'url':'" + reader.GetString(reader.GetOrdinal("d_url")) 
                                + "'},";
                        }
                        result = result + "]}";
                    }
                    else
                    {
                        result = "null";
                    }
                }

                connection.Close();
            }
            return result;
        }

        public string queryToReturnResults(string input)
        {
            string result = "";
            string query = "";
            int lines = 1;
            string[] tokens = OpenNLP.Tokenizer(input, path);
            foreach (var results in tokens)
            {
                if (lines == 1)
                {
                    query = query + "word = '" + results + "' ";
                    lines++;
                }
                else
                {
                    query = query + "or word = '" + results + "' ";
                }
            }

            int som = 1;
            using (SqlConnection connection = new SqlConnection("Data Source=" + datasource + ";Initial Catalog=snipetts;Integrated Security=SSPI"))
            using (SqlCommand cmd = new SqlCommand("SELECT dc.id as docid, d_title, d_url, word FROM words as wd inner join relation as rl ON rl.wordid = wd.id inner join docs as dc ON rl.docid = dc.id where " + query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check is the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        result = result + "{'documents':[";
                        // Read advances to the next row.
                        while (reader.Read())
                        {
                            result = result + "{" + "'docid': '" + reader.GetInt32(0) + "', 'title':'" + reader.GetString(reader.GetOrdinal("d_title")) + "', 'url':'" + reader.GetString(reader.GetOrdinal("d_url")) + 
                                ", 'word': " + reader.GetString(reader.GetOrdinal("word")) + "'},";
                        }
                        result = result + "]}";
                    }
                    else
                    {
                        result = "null";
                    }
                }
                connection.Close();
            }
            return result;
        }


    }
}
