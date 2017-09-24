
/// <summary>
/// Singleton genérico 
/// </summary>
public class Singleton<TEntity> where TEntity : class, new()
{
  private static volatile TEntity instance = default(TEntity);
  private static object syncRoot = new object();

  public static TEntity Instance
  {
    get
    {
      if (instance == default(TEntity))
      {
        lock (syncRoot)
        {
          if (instance == default(TEntity))
          {
            instance = new TEntity();
          }
        }
      }
      return instance;
    }
  }
}