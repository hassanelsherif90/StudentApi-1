using StudentApi.DTO;

namespace StudentDataAccessLayer
{
    public interface IStudentsData
    {

        List<StudentDTO> GetAllStudent();
        List<StudentDTO> GetPassedStudents();
        double GetAverageGrade();
        StudentDTO GetStudentByID(int studentId);
        void AddNewStudent(StudentDTO SDTO);
        bool UpdateStudent(StudentDTO SDTO);
        void Delete(int ID);
        void Save();
    }
}
