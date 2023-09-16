using gencode.service.Constants;
using gencode.service.Extensions;
using gencode.service.Models;
using gencode.service.Services.Interfaces;

namespace gencode.service.Services;
public class DBMLService : IDBMLService
{
    public Dictionary<string, Entity> GetEntities(List<Line> lines)
    {
        var dictionary = new Dictionary<string, Entity>();
        var isNewTable = new bool();
        var newEntity = new Entity();

        foreach (Line item in lines)
        {
            var line = item.Content;
            isNewTable = line.Contains("table");
            if (item.Content.Contains("}"))
            {
                dictionary.Add(newEntity.EntityName, newEntity);
                continue;
            }

            if (isNewTable)
            {
                newEntity = new Entity();
                var checkLine = (new string(line)).ToLower();
                if (!checkLine.Contains(TableConstants.Table) || !checkLine.Contains(TableConstants.As) || !checkLine.Contains("{"))
                    throw new Exception($"Line {item.Index} incorrect format");

                var newTable = line.ReplaceEmpty(TableConstants.Table)
                                          .ReplaceEmpty("{")
                                          .Trim();

                List<string> listTableName = newTable.SplitString(TableConstants.As);
                if (listTableName.Count < 2)
                    throw new Exception($"Line {item.Index} incorrect format");

                newEntity.EntityName = listTableName.ElementAt(0);
                newEntity.TableName = listTableName.ElementAt(1);
            }
            else
            {
                //Option
                if (line.Contains("["))
                {
                    List<string> listFieldName = line.ReplaceEmpty("]").SplitString("[");

                    if (listFieldName.Count < 2)
                        throw new Exception($"Line {item.Index} incorrect format");

                    newEntity = new Entity(newEntity.AddField(listFieldName, item));
                }
                else
                {
                    var listFieldName = new List<string>();
                    listFieldName.Add(line.Trim());
                    listFieldName.Add(string.Empty);
                    newEntity = new Entity(newEntity.AddField(listFieldName, item));
                }
            }
        }

        return dictionary;
    }
}
