namespace JobPortal.Models
{
    public interface IPortalRepository
    {
        IQueryable<User> Users { get; }
    }
}
