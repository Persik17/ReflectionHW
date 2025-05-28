using System.Reflection;
using System.Text;

namespace ReflectionHW.Serializers
{
    public class CSVSerializer
    {
        public static string Serialize<T>(T obj)
        {
            Type type = obj.GetType();
            FieldInfo[] fields = type.GetFields();
            PropertyInfo[] properties = type.GetProperties();

            StringBuilder sb = new();

            // Serialize fields
            foreach (FieldInfo field in fields)
            {
                sb.Append(field.GetValue(obj));
                sb.Append(',');
            }

            // Serialize properties
            foreach (PropertyInfo property in properties)
            {
                if (property.GetGetMethod() != null)
                {
                    sb.Append(property.GetValue(obj));
                    sb.Append(',');
                }
            }

            if (sb.Length > 0)
                sb.Length--;

            return sb.ToString();
        }

        public static T Deserialize<T>(string csvData) where T : new()
        {
            T obj = new();
            Type type = typeof(T);
            FieldInfo[] fields = type.GetFields();
            PropertyInfo[] properties = type.GetProperties();

            string[] values = csvData.Split(',');

            int index = 0;

            // Deserialize fields
            foreach (FieldInfo field in fields)
            {
                if (index < values.Length)
                {
                    try
                    {
                        string valueStr = values[index];

                        if (!string.IsNullOrEmpty(valueStr))
                        {
                            object value = Convert.ChangeType(valueStr, field.FieldType);
                            field.SetValue(obj, value);
                        }
                        index++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при десериализации поля {field.Name}: {ex.Message}. Пропускаем.");
                    }
                }
                else
                {
                    Console.WriteLine($"Недостаточно данных для десериализации поля {field.Name}. Пропускаем.");
                }
            }

            // Deserialize properties
            foreach (PropertyInfo property in properties)
            {
                if (index < values.Length && property.CanWrite && property.GetGetMethod() != null)
                {
                    try
                    {
                        string valueStr = values[index];

                        if (!string.IsNullOrEmpty(valueStr))
                        {
                            object value = Convert.ChangeType(valueStr, property.PropertyType);
                            property.SetValue(obj, value, null);
                        }

                        index++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при десериализации свойства {property.Name}: {ex.Message}. Пропускаем.");
                    }
                }
                else
                {
                    Console.WriteLine($"Недостаточно данных или свойство ReadOnly для десериализации свойства {property.Name}. Пропускаем.");
                }
            }

            return obj;
        }

        public static T LoadFromCsvFile<T>(string filePath) where T : new()
        {
            try
            {
                string csvData = File.ReadAllText(filePath);
                return Deserialize<T>(csvData);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Файл не найден: {filePath}");
                return new T();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке из файла {filePath}: {ex.Message}");
                return new T();
            }
        }
    }
}
