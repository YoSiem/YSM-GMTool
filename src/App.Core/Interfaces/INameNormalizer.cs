namespace App.Core.Interfaces;

public interface INameNormalizer
{
    string NormalizeForSearch(string? value, bool removeDiacritics = true);
}
