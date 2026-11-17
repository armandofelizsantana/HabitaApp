namespace Habita.Domain.Exceptions;

             
                                                                             
                                                       
              
public class EntityNotFoundException : Exception
{
    public string EntityName { get; }
    public object Key { get; }

    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} con clave '{key}' no fue encontrada.")
    {
        EntityName = entityName;
        Key = key;
    }
}
