namespace App.Core.Interfaces;

public interface ILuaCommandTemplateStore
{
    string GetTemplate(string templateKey);
}
