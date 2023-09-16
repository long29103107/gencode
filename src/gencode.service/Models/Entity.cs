using gencode.service.Models;

public class Entity
{
    public Entity()
    {
        
    }
    public Entity(Entity request)
    {
        EntityName = request.EntityName;
        TableName = request.TableName;
        Fields = new List<Field>(request.Fields);
    }
    public string EntityName { get; set; }
    public string TableName { get; set; }

    public List<Field> Fields = new List<Field>();

}