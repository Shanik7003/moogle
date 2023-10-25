namespace MoogleEngine
{
    public class Operators
    {
        public static List<char> operators = new char[] { '!', '^','*'}.ToList();
        public static  Dictionary<string,int> importance = new Dictionary<string, int>();

        public static Dictionary<char,List<string>> Words_with_Operator(string query)
        {
            Dictionary<char,List<string>> words_with_operators = new Dictionary<char, List<string>>();
            int startIndex = 0;
            int finalIndex = 0;
            int op_index = 0;
            bool search_for_space = false;
            int num_rep_imp = 0;
            
            for (int i = 0; i < query.Length; i++)
            {
                if (operators.IndexOf(query[i]) != -1)
                {   
                    if(query[i] == '*')
                    {
                        while(query[i]=='*')
                        {
                            num_rep_imp ++;
                            i++;
                        }
                        int startindex = i;
                        while(query[i] != ' ' && i != query.Length-1)
                        {
                            i++;
                        }
                        if(query[i] == ' ')
                        {
                           importance.Add(query.Substring(startindex,i-startIndex+1),num_rep_imp);  
                        }
                        if(query[i] == query.Length-1)
                        {
                           importance.Add(query.Substring(startindex,i-startIndex+2),num_rep_imp);
                        }
                                           
                    }
                    search_for_space = true;
                    op_index = i;
                    startIndex = i+1;
                }
                if ((query[i] == ' ' || i == query.Length-1) &&  search_for_space)
                {   
                    search_for_space = false;
                    finalIndex = i-1;
                    if( i == query.Length-1)
                    {
                        finalIndex++; 
                    }
                
                    if(words_with_operators.ContainsKey(query[op_index]))
                    {
                        words_with_operators[query[op_index]].Add(query.Substring(startIndex,finalIndex - startIndex+1));
                    }
                    else
                    {
                        List<string > aux = new List<string>();
                        aux.Add(query.Substring(startIndex,finalIndex - startIndex+1));
                        words_with_operators.Add(query[op_index], aux);                        
                    }
                }
            }
            return words_with_operators;        
        }
    }

}