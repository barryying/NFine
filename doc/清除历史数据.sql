
delete from [NFineBase].[dbo].[Sys_Event];
delete from [NFineBase].[dbo].[Sys_Candidate];
delete from [NFineBase].[dbo].[Sys_Picture];
delete from [NFineBase].[dbo].[Sys_GiftList];
delete from [NFineBase].[dbo].[Sys_AutoTask];
delete from [NFineBase].[dbo].[Sys_Vote];

delete from [NFineBase].[dbo].[Sys_User] where F_IsAdministrator='0';
delete from [NFineBase].[dbo].[Sys_UserLogOn];
delete from [NFineBase].[dbo].[Sys_Log];
delete from [NFineBase].[dbo].[Sys_DbBackup];

