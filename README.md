# OpenID_Test

## Packages EF Core
```
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
```
## Add identity scaffold your api project and create db

```
add-migration initial
update-database
```



## Packages IdentitySever4
```
Install-Package IdentityServer4
Install-Package IdentityServer4.EntityFramework
Install-Package IdentityServer4.Storage
```

## Config ConfigurationStore and OperationalStore migrations in api project program.cs
```
add-migration identityServerConf -c PersistedGrantDbContext
add-migration identityServerConf -c ConfigurationDbContext

update-database -Context ConfigurationDbContext
update-database -Context PersistedGrantDbContext 
```
## Change route identity Login.cshtml
```
@page "/Account/Login"
```

### Change identity Login post method

# Create mvc web project and config program.cs



