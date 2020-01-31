use [master]
go

if  not exists (select N'{0}' from master.dbo.syslogins where name like N'{0}' and dbname like N'master')
begin
	create login [{0}] with password = '{1}', default_database=[master], check_policy=on
end