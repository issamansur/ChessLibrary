namespace ChessMaster.Application.Common;

public interface ITenantFactory
{
    ITenantRepository GetRepository();
}