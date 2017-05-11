use master
RESTORE DATABASE "aspnet-WebApplication9-20151218112142"
FROM DISK = 'D:\MyDatabase.bak'

WITH MOVE 'aspnet-WebApplication9-20151218112142.mdf' TO 'D:\Work\Конструктор\local\WebApplication9\WebApplication9\WebApplication9\App_Data\aspnet-WebApplication9-20151218112142.mdf',
MOVE 'aspnet-WebApplication9-20151218112142_log.ldf' TO 'D:\Work\Конструктор\local\WebApplication9\WebApplication9\WebApplication9\App_Data\aspnet-WebApplication9-20151218112142_log.ldf',
REPLACE;