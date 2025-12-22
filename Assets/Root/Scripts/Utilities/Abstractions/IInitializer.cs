namespace Root.Scripts.Utilities.Abstractions
{
    public interface IInitializer<in T1>
    {
        void Initialize(T1 param1);
    }

    public interface IInitializer<in T1,in T2> 
    {
        void Initialize(T1 param1, T2 param2);
    }
    
    public interface IInitializer<in T1,in T2,in T3> 
    {
        void Initialize(T1 param1, T2 param2, T3 param3);
    }
    
    public interface IInitializer<in T1,in T2,in T3,in T4> 
    {
        void Initialize(T1 param1, T2 param2, T3 param3, T4 param4);
    }
    
    public interface IInitializer<in T1,in T2,in T3,in T4,in T5> 
    {
        void Initialize(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);
    }
    
    public interface IInitializer<in T1,in T2,in T3,in T4,in T5,in T6> 
    {
        void Initialize(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6);
    }
    
}