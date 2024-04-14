namespace ChessMaster.Application.Common;

public interface ITenantRepositoryFactory
{
    ITenantRepository GetRepository();
}