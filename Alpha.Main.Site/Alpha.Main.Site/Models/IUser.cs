namespace Alpha.Main.Site.Models
{
    using Alpha.Main.Site.Models.Enumerations;
    using Dtos;
    using Sunplus.Agent.Model.Entities;

    public interface IUser
    {
        int Id { get; set; }

        int SubAccountId { get; set; }
          
        UserLevel Level { get; set; }

        string Name { get; set; }

        bool IsMainAccount { get; }

        bool IsSubAccount { get; }

        PageAccess PageAccess { get; }

        bool HasPermissionOnFunction(string function);
    }
}