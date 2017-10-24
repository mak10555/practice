CREATE DATABASE EmployeesDb  
COLLATE Cyrillic_General_CI_AS; 

GO  
CREATE TABLE dbo.employees_payment_type(
		    id int identity(1, 1) primary key, 
			payment_type_name varchar(50)  not null)

INSERT INTO dbo.employees_payment_type (payment_type_name)
values ('Фиксированная оплата'), ('Почасовая оплата')

CREATE TABLE dbo.employees (
			id int IDENTITY(1, 1) primary key,
			first_name VARCHAR(255) not null, 
			middle_name VARCHAR(255), 
			last_name varchar(255) not null, 
			payment_type_id int  not null, 
			payment_rate money  not null
			)

alter table dbo.employees 
add constraint fk_emplouyees__employees_type_payment_type 
foreign key (payment_type_id) 
references dbo.employees_payment_type(id)

insert into dbo.employees (first_name, middle_name, last_name, payment_type_id, payment_rate)
values ('Иван', 'Иванович', 'Иванов', 1, 30000), ('Николай', 'Николаевич', 'Семенов', 1, 35000), ('Екатерина', 'Семеновна', 'Малышева', 1, 40000),
	   ('Петр', 'Петрович', 'Петров', 1, 31000), ('Александр', 'Евгеньевич', 'Удальцов', 2, 200), ('Максим', 'Алексеевич', 'Шмыгин', 2, 275),
	   ('Надежда', 'Ивановна', 'Студина', 1, 50000), ('Иван', 'Иванович', 'Иванов', 1, 33000), ('Анастасия', 'Федоровна', 'Игнатьева', 2, 250)

go

create procedure dbo.employees_get_all
as
begin
set nocount on

select id, 
       first_name, 
	   middle_name, 
	   last_name, 
	   payment_type_id, 
	   payment_rate
from dbo.employees

end

go

create procedure dbo.employees_add (@first_name varchar(255), @middle_name varchar(255), @last_name varchar(255), @payment_type_id int, @payment_rate money)
as
begin
set nocount on

insert into dbo.employees(first_name, middle_name, last_name, payment_type_id, payment_rate)
select @first_name, @middle_name, @last_name, @payment_type_id, @payment_rate

commit;

end

go

create procedure dbo.employees_get_by_name (@first_name varchar(255), @middle_name varchar(255), @last_name varchar(255))
as
begin
set nocount on

select id, 
       first_name, 
	   middle_name, 
	   last_name, 
	   payment_type_id, 
	   payment_rate
from dbo.employees
where (first_name like '%' + @first_name + '%' or @first_name is null) and
      (middle_name like '%' + @middle_name + '%' or @middle_name is null) and
	  (last_name like '%' + @last_name + '%' or @last_name is null)

end