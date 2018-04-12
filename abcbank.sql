create table AccountUsers
(

cAuLd char(200) constraint cauk unique,
nEmail nvarchar(200) constraint nemailpk primary key,
nPassword nvarchar(200) not null,
nPasswordSalt nvarchar(200) not null,

)

select * 
from AccountUsers

Insert into AccountUsers 
values('100002','jekii.kabiru@gmail.com','123456789','123456789')

create table StaffDetails
(
staffId char(4) constraint pkstaffid primary key,
staffName varchar(25) NOT NULL,
staffLastName varchar(25) NOT NULL,
cPhoneNo char(16) NOT NULL,
iAge int,
cSex char(7) NOT NULL
)

select * FROM StaffDetails

insert into StaffDetails (staffId,staffName,staffLastName,cPhoneNo,iAge,cSex) 
values('002','Ahmed','Kabiru','08117713143',22,'male')

update StaffDetails 
set staffLastName='Opeyemi' ,staffName='shina'
where staffId='001'

delete StaffDetails
where staffId='002'

create table CustomersAcct
(
iAcNo int IDENTITY(1000000001,1) constraint pkiAcNo primary key,
vCusName varchar(25) NOT NULL ,
vCusSurName varchar(25) NOT NULL ,
cSex char(7),
vAddress varchar(30) NOT NULL,
dDate datetime CONSTRAINT def DEFAULT getDate(),
mBal money NOT NULL constraint chkmoney CHECK(mBal >= 1000),
staffid  char(4) constraint fkidNo Foreign key
references StaffDetails(staffId) on delete set null
)

insert into CustomersAcct (vCusName,vCusSurName,cSex,vAddress,mBal,staffid)
values('sulaimon','abdul-afeez','Male','5, oyeniyi fatungase street',1500,'001')

select GETDATE();

select * FROM CustomersAcct 
order by iAcNo desc

Update CustomersAcct
Set mBal=mBal+2000
Where iAcNo=1000000001

create table Withdrawls
(
SlipNo int IDENTITY(100001,1)  CONSTRAINT pkslipNo primary key,
iAcNo int constraint fkiAcNo 
foreign key references CustomersAcct(iAcNo),
Amtwithdraw money NOT NULL,
Amtinwords char(35),
DateOfWith datetime Constraint def3 DEFAULT getDate()
)

select * FROM Withdrawls 
order by SlipNo DESC

insert into Withdrawls (iAcNo,Amtwithdraw,Amtinwords)
values ('1000000001',9000,'two thousand naira only')

create table Deposits
(
SlipNo int IDENTITY(100001,1)  CONSTRAINT pkslipNo1 primary key,
iAcNo int constraint fkiAcNo1 
foreign key references CustomersAcct(iAcNo),
AmtDep money NOT NULL,
Amtinwords char(35),
DateOfDep datetime Constraint def1 DEFAULT getDate()
)

select * FROM Deposits
order by SlipNo DESC

insert into Deposits (iAcNo,AmtDep,Amtinwords)
values ('1000000001',5000,'two thousand naira only')

Update Deposits
Set Amtinwords='five thousand naira  only'
WHERE SlipNo=100001

declare @name char(25)
select @name =staffName from StaffDetails where staffId='001' 
print @name
go

create Trigger trgDeposit
on Deposits
For  insert
as
declare @iaccno int 
declare @amt money
select @iaccno=inserted.iAcNo , @amt=amtDep
from Inserted
begin
update CustomersAcct
set mBal=mBal+@amt
where iAcNo=@iaccno
end
Go

create Trigger trgWithdraw
on Withdrawls
For  insert
as
declare @iaccno int 
declare @amt money
select @iaccno=inserted.iAcNo , @amt=Amtwithdraw
from Inserted
begin
update CustomersAcct
set mBal=mBal-@amt
where iAcNo=@iaccno
end
