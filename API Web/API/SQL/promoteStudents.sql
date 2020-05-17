
update Student set idenrollment = 1

alter procedure promoteStudents @course nvarchar(50),@Semester int
as
begin
    begin tran
	    declare @IdStudies int=(select s.idStudy from Studies s join Enrollment e on e.IdStudy=s.IdStudy where name=@name and Semester=@Semester)
	        if @IdStudies is null
	            begin
		            print'Niema takiego kierunku'
		        rollback
		    return
	end
	
	declare @oldEnrollment int =(select IdEnrollment from Studies s, Enrollment e where e.IdStudy= s.IdStudy and Semester = @semester and @Name = Name)
	declare @Id int=(select idenrollment from Enrollment e join Studies s on s.IdStudy=e.IdStudy where Semester=@Semester+1 and name =@name)
	
	        if @Id is null
	            begin
		            set @Id= (select MAX(IdEnrollment) + 1 from Enrollment);
		            insert into Enrollment values(@id,@Semester+1,@IdStudies,GETDATE())
		            print'Dodano Enrollment'
	            end
	
	update student set IdEnrollment = @Id where IdEnrollment=@oldEnrollment
	    print'promocja studentow'
	commit
end

execute promoteStudents 'Informatyka', 1;
    go

alter procedure enrollStudent  @indexNumber nvarchar(30), @firstName nvarchar(15), @lastName nvarchar(60),@BirthDate Date,@course nvarchar(100)

as
begin
    
	begin tran
	
	declare @existingIdStudy int = (select count(1) from Studies where name=@course)
	declare @existingStudent int = (select count(1) from Student where IndexNumber=@indexNumber)
	    
	if @existingStudent != 0
	        begin
		        print 'Student o takim indeksie istnieje'
		    rollback
		return
	end
	
	if @existingIdStudy = 0
	begin
		print 'Nie ma takiego kierunku studiow'
		rollback
		return
	end
	
	declare @IdEnrollment int = (select top 1 IdEnrollment from Enrollment where Semester = 1 and IdStudy = @IdStudy order by StartDate desc)
	
	if @IdEnrollment is null
	begin
		declare @today Date = (SELECT CONVERT(char(10), GetDate(),126))
		declare @maxIdEnrollment int = (select max(IdEnrollment) from Enrollment)
		insert into Enrollment (IdEnrollment, Semester,IdStudy,StartDate)
		values ((@maxIdEnrollment + 1), 1, @IdStudy, @today)
		print 'Dodano element'
		set @IdEnrollment = @maxIdEnrollment +1
	end
	insert into Student values (@indexNumber, @firstName, @lastName, @BirthDate, @IdEnrollment)
	commit
end

execute enrollStudent 's19828' , 'Arnold', 'Boczek', '11-05-1989', 1;

go