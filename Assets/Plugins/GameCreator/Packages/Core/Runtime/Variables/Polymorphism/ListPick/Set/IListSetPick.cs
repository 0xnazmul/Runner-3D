using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("From List")]
    
    public interface IListSetPick
    {
        int GetIndex(ListVariableRuntime list, int count);
        int GetIndex(int count);
        
        string ToString();
    }
}