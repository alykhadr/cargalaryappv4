namespace CarGalary.Application.ErrorCatalog
{
    public interface IErrorCatalogService
    {
        ErrorCatalogEntry? GetByCode(string errorCode);
        string? GetCodeByMessage(string message);
    }
}
