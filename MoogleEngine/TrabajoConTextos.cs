using System.Text.RegularExpressions;
namespace MoogleEngine
{
    public class ModificarTexto
    {  
        public static Dictionary<int,string[]> Vocabulary = new Dictionary<int, string[]>();
        public static Dictionary <string,  float[]> TF = new Dictionary<string, float[]>();
        public static Dictionary<string, float> IDF = new Dictionary<string, float>();
        public static  Dictionary <string, int> Doc_Frecuency = new Dictionary<string, int >();
        static int cant_doc = 0;
        public static Dictionary <string, float[]> TFxIDF = new Dictionary<string, float[]>();
        public static float [] TEXTs_SCORE = new float[Vocabulary.Count];
        public static Dictionary<string, string[]> SNIPPET = new Dictionary<string, string[]>();
        
        public static void Normalizar()
        { 
            string[] files = Directory.GetFiles("../Content", "*.txt");
            cant_doc = files.Length;
            Dictionary<int,string[]> Content = new Dictionary<int, string[]>();
            for (int i = 0; i < files.Length; i++)
            {
                Content.Add(i,Splitear(File.ReadAllText(files[i]).ToLower()).ToArray());      
            }
            Vocabulary = Content;  
        } 
        public static void TermFrecuency()
        {
           Dictionary <string, float[]> tf = new Dictionary<string, float[]>();
           //este primer ciclo se encarga de llenar la columna de los keys de TF con todas la palabras de todos los textos
           foreach(var pair in Vocabulary)
           {
                for (int i = 0; i < pair.Value.Length; i++)
                {
                    if(!tf.ContainsKey(pair.Value[i]))
                    {
                     tf.Add(pair.Value[i], new  float[Vocabulary.Count]);
                    }
                }
           }
           foreach(var pair in tf)
           { 
                foreach(var pairVoc in Vocabulary)
                {
                    int num_rep = 0;
                    for (int j = 0; j < pairVoc.Value.Length; j++)
                    {    
                        if( pair.Key == pairVoc.Value[j]) 
                        {
                            num_rep += 1;
                        }
                    } //System.Console.WriteLine(num_rep);
                    tf[pair.Key][pairVoc.Key] = num_rep;
                    //cree un dic aux llamado doc_frecuency que continene el numero de docs en los que aparece cada palabra
                    //por ejemplo: la palabra mango aparece en 10 documentos
                    if (num_rep != 0)
                    {
                        if(!Doc_Frecuency.ContainsKey(pair.Key))
                        {
                            Doc_Frecuency.Add(pair.Key,1);
                        }
                        else 
                        {
                            Doc_Frecuency[pair.Key]++;
                        }
                    } 
                }
           }  
           TF = tf;
           /*foreach (var pair in TF)
           {
            for (int i = 0; i < TF[pair.Key].Length; i++)
            {
                 System.Console.WriteLine(TF[pair.Key][i].Item2);
            }           
           }*/
        } 
            //metodo axilia que calcula el idf dado dos parametros
        public static float Calc_IDF(int CantTotal_docs, int CantDocs_palabra)
        {
            float Idf = (float)((float)CantDocs_palabra/(float)CantTotal_docs);
            return Idf;
        }
       
        public static void InverseDocFrecuency()
        {
            Dictionary<string, float> iDF = new Dictionary<string, float>();

            foreach (var pair in TF)
            {
               iDF.Add(pair.Key , Calc_IDF(cant_doc,Doc_Frecuency[pair.Key]));
            }
            IDF = iDF;
        }

        public static void TermFrecuency_InverseDocFrecuency()
        {
            Dictionary <string, float[]> TfxIDf = new Dictionary<string, float[]>();

            foreach (var pair in TF)
            {
               
                 TfxIDf.Add(pair.Key , new float[Vocabulary.Count]);
               for (int i = 0; i < pair.Value.Length; i++)
               {
                 //System.Console.WriteLine(TF[pair.Key][i]);
                  // System.Console.WriteLine(IDF[pair.Key]);
                 TfxIDf[pair.Key][i]= IDF[pair.Key] * TF[pair.Key][i];
               }
            }
            TFxIDF = TfxIDf;
           /* foreach(var pair in TFxIDF)
            {
                    System.Console.WriteLine(pair.Key + string.Join( ',', TFxIDF[pair.Key]));
            }*/
        }

        public static void Snippet()
        {
            Dictionary<string, string[]> Snippet = new Dictionary<string, string[]>(); 
             foreach (var pair in TF)
             { 
                if(!Snippet.ContainsKey(pair.Key))
                {
                    Snippet.Add(pair.Key,new string [Vocabulary.Count]);
                }
             }
               foreach(var pair in Snippet)
                {
                    foreach (var pairVoc in Vocabulary)
                    {
                       for (int i = 0; i < pairVoc.Value.Length; i++)
                      {
                        if( pair.Key == pairVoc.Value[i])
                        {   
                            Snippet[pair.Key][pairVoc.Key] = Vocabulary[pairVoc.Key][i];  
                            for (int k = 1; k <= 20 ; k++)
                            {
                                if(i+k == pairVoc.Value.Length)
                                {
                                    break;
                                }
                                Snippet[pair.Key][pairVoc.Key] += " " + Vocabulary[pairVoc.Key][i+k];                                                           
                            }
                       }
                       }
                    }
                 }
                 SNIPPET = Snippet;
        }
        public static string[] Splitear(string contenido)
        {
            char[] separador = {' ',',', ':',';','.','\r','\n','—','_','-','\t',']','[','{','}','(',')','!','@','#','$','%','^','&','*','<','>','?','-'};
            return contenido.Split(separador,StringSplitOptions.RemoveEmptyEntries); 
        }
    }

}