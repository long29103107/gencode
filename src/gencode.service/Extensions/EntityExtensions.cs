using gencode.service.Constants;
using gencode.service.Models;

namespace gencode.service.Extensions;
public static class EntityExtensions
{
    public static Entity AddField(this Entity entity, List<string> fieldNames, Line line)
    {
        var nameAndDataType = fieldNames.FirstOrDefault();
        var options = fieldNames.LastOrDefault();

        var result = new Entity(entity);
        var field = new Field();

        field = field.AddOptions(options, line);

        field = field.AddNameAnDataType(nameAndDataType, line);

        result.Fields.Add(field);

        return result;
    }

    private static Field AddOptions(this Field field, string options, Line line)
    {
        var newField = new Field(field);

        var listOptions = options.SplitString(",").Select(x => x.Trim()).ToList();
        if(listOptions.Any())
        {
            foreach(var item in listOptions)
            {
                if(item.Contains(TableConstants.Null))
                {
                    var str = item.ToLower().ReplaceEmpty(TableConstants.Null);
                    newField.IsNull = !(str.Trim() == TableConstants.Not);
                } 
                else if(item.Contains(TableConstants.Default))
                {
                    var str = item.ToLower().ReplaceEmpty(TableConstants.Default)
                                        .ReplaceEmpty(":")
                                        .Trim();

                    if (string.IsNullOrEmpty(str))
                        throw new Exception($"Line {line.Index}: miss default value");

                    newField.DefaultValue = str;
                }
                else if (item.Contains(TableConstants.Ref))
                {
                    if (!item.Contains(":") && !item.Contains(">"))
                        throw new Exception($"Line {line.Index}: is incorrect format");

                    var str = item.ReplaceEmpty(TableConstants.Ref)
                                        .ReplaceEmpty(":")  
                                        .ReplaceEmpty(">")  
                                        .Trim();
                    if (string.IsNullOrEmpty(str))
                        throw new Exception($"Line {line.Index}: miss default value");

                    if (!str.Contains("."))
                        throw new Exception($"Line {line.Index}: is incorrect format");

                    var listForeignKey = str.Split(".").ToList();

                    if (listForeignKey.Count < 2)
                        throw new Exception($"Line {line.Index} incorrect format");

                    newField.IsForeignKey = true;
                    newField.ForeignKey = new ForeignKey(listForeignKey.ElementAt(0), listForeignKey.ElementAt(1));

                    newField.DefaultValue = str;
                }
            }    
        }    

        return newField;
    }

    private static Field AddNameAnDataType(this Field field, string nameAndDataType, Line line)
    {
        var newField = new Field(field);

        if (nameAndDataType.Contains(TableConstants.Pk))
        {
            newField.IsPrimaryKey = true;
            nameAndDataType = nameAndDataType.ReplaceEmpty(TableConstants.Pk);
        }

        List<string> listFieldName = nameAndDataType.SplitString(" ");
        if (listFieldName.Count < 2)
            throw new Exception($"Line {line.Index} incorrect format");

        newField.FieldName = listFieldName[0];
        newField.DataType = listFieldName[1];

        return newField;
    }
}
