using StudentApi.Data;
using StudentApi.Data.Entities;
using StudentApi.DTO;
using System.Data;


namespace StudentDataAccessLayer
{

    public class StudentsData(AppDbcontext _appDbContext) : IStudentsData
    {
        public AppDbcontext appDbContext = _appDbContext;

        public List<StudentDTO> GetAllStudent()
        {

            var studentList = new List<StudentDTO>();

            foreach (Student student in appDbContext.Students)
            {
                StudentDTO studentDTO = new StudentDTO(student.Id,
                         student.Name,
                         student.Grade,
                         student.Age);

                studentList.Add(studentDTO);
            }

            return studentList;

        }

        public List<StudentDTO> GetPassedStudents()
        {
            List<Student>? students = appDbContext.Students.Where(x => x.Grade >= 50).ToList();

            List<StudentDTO> StudentPassed = new List<StudentDTO>();

            foreach (var student in students)
            {
                StudentDTO studentDTO = new StudentDTO(student.Id,
                                   student.Name,
                                   student.Grade,
                                   student.Age);



                StudentPassed.Add(studentDTO);
            }



            return StudentPassed;
        }

        public double GetAverageGrade()
        {
            double AverageGrade = 0;


            double result = appDbContext.Students.Average(x => x.Grade);


            if (result != null)
            {
                AverageGrade = result;
            }
            else
            {
                AverageGrade = 0;
            }

            return AverageGrade;
        }

        public StudentDTO GetStudentByID(int studentId)
        {
            Student? Student = appDbContext.Students.FirstOrDefault(x => x.Id == studentId);

            if (Student != null)
            {
                var studentDTO = new StudentDTO(Student.Id, Student.Name, Student.Age, Student.Grade);

                return studentDTO;
            }
            else
            {
                return null;
            }
        }

        public void AddNewStudent(StudentDTO SDTO)
        {

            Student student = new Student(SDTO.Id, SDTO.Name, SDTO.Age, SDTO.Grade);
            appDbContext.Students.Add(student);

        }

        public bool UpdateStudent(StudentDTO SDTO)
        {
            try
            {
                Student student = new Student(SDTO.Id, SDTO.Name, SDTO.Age, SDTO.Grade);
                appDbContext.Students.Update(student);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public void Save()
        {
            appDbContext.SaveChanges();
        }

        public void Delete(int ID)
        {
            var Student = appDbContext.Students.FirstOrDefault(x => x.Id == ID);
            if (Student != null)
            {
                appDbContext.Remove(Student);
            }
        }
    }
}
