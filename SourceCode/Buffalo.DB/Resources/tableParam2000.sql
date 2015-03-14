SELECT 
    tableName= d.[name],
    tableDescription=f.[value],
    tabletype=d.xtype,
    paramOrder= a.colorder,
    paramName= a.[name],
    isIdentity= COLUMNPROPERTY( a.id,a.name,'IsIdentity'),
    isPrimary=case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in (
                     SELECT name FROM sysindexes WHERE indid in(SELECT indid FROM sysindexkeys WHERE id = a.id AND 
					 colid=a.colid))) then 1 else 0 end,
    dbType= b.[name],
    dataSize= a.length,
    length= COLUMNPROPERTY(a.id,a.name,'PRECISION'),
    scale= isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),
    allowNull=a.isnullable,
    defaultValue= isnull(e.text,''),
    paramDescription=isnull(g.[value],'')
FROM syscolumns a

left join systypes b on a.xusertype=b.xusertype

inner join sysobjects d on a.id=d.id

left join syscomments e on a.cdefault=e.id

left join sysproperties g on a.id=g.id and a.colid=g.smallid

left join sysproperties f on d.id=f.id and f.smallid=0
where 
    d.xtype in ('U','V') and  d.[name] not in('dtproperties','sysdiagrams')
    <%=TableNames%>
order by 
    d.xtype,d.[name],a.colorder