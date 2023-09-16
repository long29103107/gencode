namespace gencode.service.Models;
public class Field
{
    public Field()
    {
        
    }
    public Field(Field request)
    {
        FieldName = request.FieldName;
        DataType = request.DataType;
        IsForeignKey = request.IsForeignKey;
        IsPrimaryKey = request.IsPrimaryKey;
        ForeignKey = new ForeignKey(request.ForeignKey);
    }
    public string FieldName { get; set; }
    public string DataType { get; set; }
    public bool IsForeignKey { get; set; } = false;
    public bool IsPrimaryKey { get; set; } = false;
    public bool IsNull { get; set; } = false;
    public string DefaultValue { get; set; }

    public ForeignKey ForeignKey = new ForeignKey();
}

public class ForeignKey
{
    public ForeignKey()
    {
        
    }
    public ForeignKey(string entityName, string fieldName)
    {
        EntityName = entityName;
        FieldName = fieldName;
    }
    public ForeignKey(ForeignKey request)
    {
        EntityName = request.EntityName;
        FieldName = request.FieldName;
    }
    public string EntityName { get; set; }
    public string FieldName { get; set; }
}
