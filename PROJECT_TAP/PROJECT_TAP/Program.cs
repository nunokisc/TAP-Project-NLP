using MyExtensionMethods;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Project_TAP
{
    class Program
    {
        // USER DA BASE DE DADOS -----------------
        static string datasource = "KISC-PC";
        //----------------------------------------
        // CAMINHO DO OPENNLP --------------------------------------------------------------------------
        static string path = @"C:\Users\Nuno\Downloads\TRABALHO DE TAP\TRABALHO DE TAP\Project_TAP_RECURSOS\MyExtensionMethods\OpenNLP\";
        //----------------------------------------------------------------------------------------------
		// CAMINHO DO STOPWORDS ------------------------------------------------------------------------
		static string stopwords = @"C:\Users\Nuno\Downloads\TRABALHO DE TAP\TRABALHO DE TAP\Project_TAP_RECURSOS\stopwords-pt.txt";
		//----------------------------------------------------------------------------------------------
        static void Main(string[] args)
        {
            bool haveData = false;
            bool opMenu = true;
            string loadData = ""; string deleteData = ""; string query = ""; string option = "";
            using (SqlConnection connection = new SqlConnection("Data Source="+datasource+";Initial Catalog=snipetts;Integrated Security=SSPI"))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM words", connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check is the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        haveData = true;
                        while (haveData == true)
                        {
                            Console.WriteLine("Base de dados encontra se carregada pretende elimina-la? (y/n)");
                            deleteData = Console.ReadLine();
                            reader.Close();
                            if (deleteData == "y" || deleteData == "Y")
                            {
								SqlCommand cmd1 = new SqlCommand("DELETE FROM relation", connection);
                                cmd1.ExecuteNonQuery();
								cmd1 = new SqlCommand("DELETE FROM offsetword ", connection);
                                cmd1.ExecuteNonQuery();
                                cmd1 = new SqlCommand("DELETE FROM words", connection);
                                cmd1.ExecuteNonQuery();
								cmd1 = new SqlCommand("DELETE FROM docs", connection);
                                cmd1.ExecuteNonQuery();
                                haveData = false;
                            }
                            else if (deleteData == "n" || deleteData == "N")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Parametro errado!");
                            }
                        }
                    }
                }
                connection.Close();
            }

                while (haveData == false) {
                    Console.WriteLine("Deseja carregar os dados por defeito? (y/n)");
                    loadData = Console.ReadLine();

                    if (loadData == "y" || loadData == "Y")
                    {
                        insertResults("‪‪Emmanuel Macron‬, ‪Angela Merkel‬, ‪União Europeia‬‬");
                        insertResults("‪‪Feyenoord Rotterdam‬, ‪Dirk Kuyt‬‬‬‬");
                        insertResults("Salvador Sobral‬, ‪Super Bock Super Rock‬‬");
                        insertResults("‪‪Apple‬, ‪Apple Worldwide Developers Conference‬, ‪iPad Pro‬‬");
                        insertResults("Paulo David‬‬");
                        insertResults("‪‪São Manços");
                        insertResults("Daniela Ruah‬, ‪Salvador Sobral");
                        insertResults("‪‪Roger Federer‬, ‪Torneio de Roland Garros‬‬");
                        insertResults("‪‪‪‪Sport Club Internacional‬, ‪Campeonato Brasileiro de Futebol - Série B‬, ‪Londrina Esporte Clube");
                        insertResults("‪‪Apple‬, ‪iOS 10‬, ‪Mac OS X v10.3‬, ‪tvOS‬, ‪WatchOS‬, ‪macOS‬‬");
                        haveData = true;
                     }
                    else if (loadData == "n" || loadData == "N")
                    {
                        string input = "";
                        Console.WriteLine("Inserir as pesquisas: (para parar inserir letra x)");
                        input = Console.ReadLine();
                            while (input.ToLower() != "x")
                            {
                                insertResults(input);
                        Console.WriteLine("Inserir nova pesquisa: (para parar inserir letra x)");
                        input = Console.ReadLine();
                                haveData = true;
                            }    
                    }
                    else
                    {
                        Console.WriteLine("Parametro errado!");
                }
                }
            while (option.ToLower() != "x")
            {
                Console.WriteLine("Qual o tipo de query que quer fazer? (1,2,3,X)");
                Console.WriteLine("1 - One word query");
                Console.WriteLine("2 - Free text query");
                Console.WriteLine("3 - Phrase query");
                option = Console.ReadLine();
                opMenu = true;
                if (option == "1")
                {
                    while (opMenu)
                    {
                        Console.WriteLine("Escreva a query: (para voltar atras X)");
                        query = Console.ReadLine();
                        if(query.ToLower() != "x")
                        {
                            queryToReturnResults(query);
                        }
                        else
                        {
                            opMenu = false;
                        }
                    }
                }
                else if(option == "2")
                {
                    while (opMenu)
                    {
                        Console.WriteLine("Escreva a query: (para voltar atras X)");
                        query = Console.ReadLine();
                        if (query.ToLower() != "x")
                        {
                            queryToReturnResults(query);
                        }
                        else
                        {
                            opMenu = false;
                        }
                    }
                }
                else if (option == "3")
                {
                    while (opMenu)
                    {
                        Console.WriteLine("Escreva a query: (para voltar atras X)");
                        query = Console.ReadLine();
                        if (query.ToLower() != "x")
                        {
                            queryToReturnResultsWithSequence(query);
                        }
                        else
                        {
                            opMenu = false;
                        }
                    }
                }
                else if(option.ToLower() == "x")
                {
                    
                }
                else
                {
                    Console.WriteLine("Parametro errado!");
                }
            }
        }

        static HashSet<string> setStopWords()
        {
            HashSet<string> hashSet = new HashSet<string>();
            string[] file = File.ReadLines(stopwords).ToArray();
            foreach(var result in file) {
                hashSet.Add(result);
            }
            return hashSet;
            //stop words https://github.com/stopwords-iso/stopwords-pt
        }

        static int lastIdDoc()
        {
            int docId = 0;
            using (SqlConnection connection = new SqlConnection("Data Source="+ datasource + ";Initial Catalog=snipetts;Integrated Security=SSPI"))
            using (SqlCommand cmd = new SqlCommand("SELECT MAX(id)+1 as id FROM docs", connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check is the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        // Read advances to the next row.
                        while (reader.Read())
                        {
                            if(reader.IsDBNull(0))
                            {
                                docId = 1;
                            }
                            else
                            {
                                docId = reader.GetInt32(0);
                            }
                        }
                    }
                }
                connection.Close();
            }
            return docId;
        }

        static int lastIdWord()
        {
            int wordId = 0;
            using (SqlConnection connection = new SqlConnection("Data Source="+ datasource + ";Initial Catalog=snipetts;Integrated Security=SSPI"))
            using (SqlCommand cmd = new SqlCommand("SELECT MAX(id)+1 as id FROM words", connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check is the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        // Read advances to the next row.
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0))
                            {
                                wordId = 1;
                            }
                            else
                            {
                                wordId = reader.GetInt32(0);
                            }
                        }
                    }
                }
                connection.Close();
            }
            return wordId;
        }

        static bool confirmIfExist(string word, int docid)
        {
            bool confirm = false;
            using (SqlConnection connection = new SqlConnection("Data Source="+ datasource + ";Initial Catalog=snipetts;Integrated Security=SSPI"))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM relation WHERE wordid =(SELECT id FROM words WHERE word='" + word + "') and docid = "+docid+" ", connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check is the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        // Read advances to the next row.
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0))
                            {
                                confirm = false;
                            }
                            else
                            {
                                confirm = true;
                            }
                        }
                    }
                }
                connection.Close();
            }
            return confirm;
        }

        static HashSet<string> setAddedWords()
        {
            HashSet<string> hashSet = new HashSet<string>();
            using (SqlConnection connection = new SqlConnection("Data Source="+ datasource + ";Initial Catalog=snipetts;Integrated Security=SSPI"))
            using (SqlCommand cmd = new SqlCommand("SELECT word FROM words", connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check is the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        // Read advances to the next row.
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                hashSet.Add(Regex.Replace(reader.GetString(reader.GetOrdinal("word")).ToLower(), @"\s+", ""));
                            }
                        }
                    }
                }
                connection.Close();
            }
            return hashSet;
            
        }

        static void queryToReturnResults(string input)
        {
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
                //Console.WriteLine(query);
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
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                Console.WriteLine(som++ + "{");
                                Console.WriteLine("docid: " + reader.GetInt32(0));
                                Console.WriteLine("title: " + reader.GetString(reader.GetOrdinal("d_title")));
                                Console.WriteLine("url: " + reader.GetString(reader.GetOrdinal("d_url")));
                                Console.WriteLine("word: " + reader.GetString(reader.GetOrdinal("word")));
                                Console.WriteLine("}");
                                Console.WriteLine("");
                            }
                        }
                    else
                    {
                        Console.WriteLine("Não existe essa palavra!");
                    }
                }               
                    connection.Close();
                }
            //Console.ReadLine();
        }

        static void queryToReturnResultsWithSequence(string input)
        {
            int lines = 1;
            int som = 1;
            int aux = 0;int aux1 = 1;
            string query = "";
            HashSet<string> hashSet = setStopWords();
            string[] tokens = OpenNLP.Tokenizer(input, path);
            foreach (var results in tokens)
            {
                if (OpenNLP.checkContainsALetter(results))
                {
                    if (hashSet.Contains(results.ToLower()))
                    {
                        if(aux1 != 1)
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
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                            Console.WriteLine(som++ + "{");
                            Console.WriteLine("docid: " + reader.GetInt32(0));
                            Console.WriteLine("title: " + reader.GetString(reader.GetOrdinal("d_title")));
                            Console.WriteLine("url: " + reader.GetString(reader.GetOrdinal("d_url")));
                            Console.WriteLine("}");
                            Console.WriteLine("");
                        }
                        }
                        else
                        {
                            Console.WriteLine("Não existe!");
                        }
                    }

                    connection.Close();
                }
            //Console.ReadLine();
        }

        static void insertResults(string query)
        {
            int idDoc = lastIdDoc(); // last id docs sql
            int idWord = lastIdWord();
            int wordPosition = 0;
            HashSet<string> hashSet = setStopWords();
            HashSet<string> addedWords = setAddedWords(); // SELECT words FROM words
            string api = "https://api.cognitive.microsoft.com/bing/v5.0/search?";
            string safesearch = "Moderate";
            string lang = "en-us";
            int offset = 0;
            int count = 10;
            string word = query;
            var client = new RestClient(api +
                "safesearch=" + safesearch +
                "&mkt=" + lang +
                "&offset=" + offset +
                "&count=" + count +
                "&q=" + word);
            var request = new RestRequest(Method.GET);
            request.AddHeader("ocp-apim-subscription-key", "eb0b8f77941d45c5bead950366586f3d");
            request.AddHeader("host", "api.cognitive.microsoft.com");
            IRestResponse response = client.Execute(request);
            JObject o = JObject.Parse(response.Content);
            SqlConnection connection = new SqlConnection("Data Source="+ datasource + ";Initial Catalog=snipetts;Integrated Security=SSPI");
            SqlCommand cmd;
            cmd = new SqlCommand("INSERT INTO docs (id,d_title,d_content,d_url) VALUES (@id,@title,@content,@url)", connection);
            connection.Open();
            cmd.Parameters.Add("@id", System.Data.SqlDbType.NVarChar);
            cmd.Parameters.Add("@title", System.Data.SqlDbType.NVarChar);
            cmd.Parameters.Add("@content", System.Data.SqlDbType.NVarChar);
            cmd.Parameters.Add("@url", System.Data.SqlDbType.NVarChar);
            foreach (var results in o["webPages"]["value"])
            {     
                //Console.WriteLine((string)results["name"]);
                string title = (string)results["name"];
                string url = (string)results["url"];
                string content =  (string)results["snippet"];
                cmd.Parameters["@id"].Value = idDoc;
                cmd.Parameters["@title"].Value = title;
                cmd.Parameters["@content"].Value = content;
                cmd.Parameters["@url"].Value = url;
                cmd.ExecuteNonQuery();
                string[] tokens = OpenNLP.Tokenizer(content, path);

                foreach (var result in tokens)
                {
                    if (OpenNLP.checkContainsALetter(result))
                    {
                        if (hashSet.Contains(result.ToLower()))
                        {
                            //Console.WriteLine("STOP WORD "+ result);
                        }
                        else
                        {
                            if (addedWords.Contains(result.ToLower()))
                            {
                                if (confirmIfExist(result, idDoc))
                                {
                                    SqlCommand updateWord = new SqlCommand("UPDATE words SET tf=tf+1 WHERE word='" + result + "'", connection);
                                    updateWord.ExecuteNonQuery();
                                    //Console.WriteLine("REPETIDA " + result);
                                    SqlCommand updateRelation = new SqlCommand("UPDATE relation SET tf=tf+1 WHERE docid='" + idDoc + "' AND wordid=(SELECT id FROM words WHERE word='" + result + "')", connection);
                                    updateRelation.ExecuteNonQuery();
                                    SqlCommand insertOffset = new SqlCommand("INSERT INTO offsetword (docid,offset,wordid) VALUES ('" + idDoc + "','" + wordPosition + "',(SELECT id FROM words WHERE word='" + result + "'))", connection);
                                    insertOffset.ExecuteNonQuery();
                                } else
                                {
                                    SqlCommand update = new SqlCommand("UPDATE words SET tf=tf+1, df=df+1 WHERE word='" + result + "'", connection);
                                    update.ExecuteNonQuery();
                                    //Console.WriteLine("REPETIDA " + result);
                                    SqlCommand insertOccurs = new SqlCommand("INSERT INTO relation (docid,wordid,tf) VALUES (" + idDoc + ",(SELECT id FROM words WHERE word='" + result + "'),1)", connection);
                                    insertOccurs.ExecuteNonQuery();
                                    SqlCommand insertOffset = new SqlCommand("INSERT INTO offsetword (docid,offset,wordid) VALUES ('" + idDoc + "','" + wordPosition + "',(SELECT id FROM words WHERE word='" + result + "'))", connection);
                                    insertOffset.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                SqlCommand insert = new SqlCommand("INSERT INTO words (id,df,tf,word) VALUES ("+idWord+",1,1,'" + result + "')", connection);
                                insert.ExecuteNonQuery();
                                addedWords.Add(result.ToLower());
                                SqlCommand insertOccurs = new SqlCommand("INSERT INTO relation (docid,wordid,tf) VALUES ("+ idDoc + "," + idWord + ",1)", connection);
                                insertOccurs.ExecuteNonQuery();
                                SqlCommand insertOffset = new SqlCommand("INSERT INTO offsetword (docid,offset,wordid) VALUES ('"+ idDoc + "','"+ wordPosition + "','"+ idWord + "')", connection);
                                insertOffset.ExecuteNonQuery();
                                idWord = idWord + 1;
                            }
                        }
                        wordPosition = wordPosition + 1;
                    }
                }
                wordPosition = 0;
                idDoc = idDoc + 1;
            }
            Console.WriteLine("Dados inseridos!");
            connection.Close();
        }
    }
}
