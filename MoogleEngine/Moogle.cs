namespace MoogleEngine;
using System.Text.RegularExpressions;

public static class Moogle
{
    static Dictionary<int, string[]> Vocabulary = new Dictionary<int, string[]>();
    static string[] files = Directory.GetFiles("../Content", "*.txt");
    const int num_results = 1;

    public static SearchResult Query(string query)
    {
        //normalizr el conteido del query
        string[] Query;
        Query = Splitear((query).ToLower()).ToArray();

        //calcular el score de los textos dado el query que introdujo el usuario 
        float[] Texts_Score = new float[files.Length];
        for (int i = 0; i < Query.Length; i++)
        {
            for (int k = 0; k < Texts_Score.Length; k++)
            {
                if(Operators.importance.ContainsKey(Query[i]))
                {
                    Texts_Score[k] += (float)Math.Pow(ModificarTexto.TFxIDF[Query[i]][k],Operators.importance[Query[i]]);
                }
                Texts_Score[k] += ModificarTexto.TFxIDF[Query[i]][k];
            }
        }
        //snippet de los textos dada la palabra con mas tfxidf en el query       
        string[] Texts_Snippet = new string[Texts_Score.Length];
        for (int i = 0; i < Query.Length; i++)
        {
            for (int k = 0; k < Texts_Snippet.Length; k++)
            {
                Texts_Snippet[k] = ModificarTexto.SNIPPET[Query[i]][k];
            }
        }
        //operadores 
        Dictionary<char, List<string>> words_with_operators = Operators.Words_with_Operator(query);
        foreach (var op in words_with_operators)
        {
           switch (op.Key)
           {
            case '!':
            foreach(var pair in words_with_operators)
            {
                for (int k = 0; k < pair.Value.Count; k++)
                {
                  for (int j = 0; j < files.Length; j++)
                  {
                    if(ModificarTexto.TF[pair.Value[k]][j] != 0)
                    {
                        Texts_Score[j] = 0;
                    }
                  }  
                }
            }   
            break;
            case '^':
            foreach(var pair in words_with_operators)
            {
                for (int k = 0; k < pair.Value.Count; k++)
                {
                  for (int j = 0; j < files.Length; j++)
                  {
                    if(ModificarTexto.TF[pair.Value[k]][j] == 0)
                    {
                        Texts_Score[j] = 0;
                    }
                  }  
                }
            }   
            break;
           }
            
        }
        
        //convertir el array que contiene los scores de los textos en un array de SearchItems con titulo, snipet todavia, y score respectivo
        SearchItem[] items = new SearchItem[Texts_Score.Length];

        for (int j = 0; j < Texts_Score.Length; j++)
        {
            items[j] = new SearchItem(files[j], Texts_Snippet[j], Texts_Score[j]);
        }
        //convertir el array de SearchItems en una lista de SearchItems llamada items para poder organiarlos de mayos a menor 
        

        if(items == null)
        {
           items = new SearchItem[]{new SearchItem("No existen resultados para la busqueda solicitada", "",0)};
           return new SearchResult(items, query);
        }
        return new SearchResult(items.OrderByDescending(x => x.Score).ToArray(), query);
    }



    public static string[] Splitear(string contenido)
    {
        char[] separador = { ' ', ',', ':', ';', '.', '\r', '\n', '\t', ']', '[', '{', '}', '(', ')', '!', '@', '#', '$', '%', '^', '&', '*', '<', '>', '?', '-' };
        return contenido.Split(separador, StringSplitOptions.RemoveEmptyEntries);
    }
}

