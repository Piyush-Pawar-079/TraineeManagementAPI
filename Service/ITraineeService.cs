using traineeManagementAPI.Model;

namespace traineeManagementAPI.Service;

public interface ITraineeService
{
    List<Trainee> GetAllTrainees();

    Trainee? GetTraineeById(int id);

    Trainee? UpdateTrainee(int id, Trainee trainee);

    Trainee CreateTrainee(Trainee trainee);

    bool DeleteTrainee(int id);

}