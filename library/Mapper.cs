using System.Threading;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Globalization;

namespace library;
public static class Mapper
{
    public static T ToClone<T>(this T self)
    where T : class, new()
    {
        self.NullCecker();
        var classType = self.GetType();
        var selfProperties = classType.GetProperties();
        var cloned = new T();
        var classTypeCloned = cloned.GetType();
        foreach (var type in selfProperties)
        {
            var fieldName = type.Name;
            var fieldNameToAssign = classTypeCloned.GetProperty(fieldName);
            var valueToAssign = type.GetValue(self);
            fieldNameToAssign?.SetValue(cloned,valueToAssign);
        }
        return cloned;
    }
    /// <summary>
    /// Will try to merge two models returning one with combination of fields from both
    /// </summary>
    /// <param name="self"></param>
    /// <param name="model2"></param>
    /// <param name="stringOverwriting">Optional parameter to check fro string.empty if true will treat them as null and overwrite</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToMerged<T>(this T self, T model2, bool stringOverwriting = false)
        where T : class, new()
    {
        self.NullCecker();
        model2.NullCecker();
        var merged = new T();
        var selfProperties = self.GetType().GetProperties();

        if (self.GetType() == model2.GetType())
            foreach(var prop in selfProperties)
            {
                var selfPropName = prop.Name;
                var modelPropName = model2.GetObjectPropertyName(selfPropName);
                if (ComparePropName(selfPropName, modelPropName))
                {
                    if(prop.GetValue(self) == null)
                    {
                        var modelValue = model2.GetPropertyValue(modelPropName);
                        System.Console.WriteLine($"Eval: {modelPropName} {modelValue.ToString()}");
                        merged.GetType().GetProperty(selfPropName).SetValue(merged, modelValue);
                    }
                    else if (model2.GetPropertyValue(selfPropName) == null)
                    {
                        var selfValue = prop.GetValue(self);
                        System.Console.WriteLine($"Eval: {modelPropName} {selfValue.ToString()}");
                        merged.GetType().GetProperty(prop.Name).SetValue(merged, selfValue);
                    }
                    else
                    {
                        var modelValue = model2.GetPropertyValue(modelPropName);
                        merged.GetType().GetProperty(selfPropName).SetValue(merged, modelValue);
                    }
                }
            }

        return merged;
    }

    public static void DisplayData<T>(this T self)
    {
        self.NullCecker();
        var classType = self!.GetType();
        var fields = classType.GetProperties();
        foreach(var field in fields)
        {
            System.Console.WriteLine($"FieldName: {field.Name}");
            System.Console.WriteLine($"Value: {field.GetValue(self)}");
        }
    }

    private static void NullCecker<T>(this T self)
    {
        if (self == null)
            throw new ArgumentNullException(nameof(self));
    }

    private static string GetPropName(this System.Reflection.PropertyInfo self)
    {
        return self.Name;
    }

    private static bool ComparePropName(string propName1, string propName2)
    {
        if (propName1.Equals(propName2))
            return true;
        return false;
    }

    private static string GetObjectPropertyName<T>(this T self, string propName)
    {
        self.NullCecker();
        return self.GetType().GetProperty(propName).Name;
    }

    private static object GetPropertyValue<T>(this T self, string propName)
    {
        self.NullCecker();
        var name = self?.GetType().GetProperty(propName);
        return name.GetValue(self);
    }
}