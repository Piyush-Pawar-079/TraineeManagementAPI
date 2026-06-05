using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Service;

public class TraineeService : ITraineeService
{
    private readonly List<Trainee> _trainee = new();

    private static int _nextId = 1;

    public List<Trainee> GetAllTrainees()
    {
        return _trainee;
    }

    public Trainee? GetTraineeById(int id)
    {
        return _trainee.FirstOrDefault(t => t.Id == id);

    }

    public Trainee? UpdateTrainee(int id, Trainee trainee)
    {
        var desiredTrainee = GetTraineeById(id);

        if (desiredTrainee == null)
        {
            return null;
        }

        desiredTrainee.FirstName = trainee.FirstName;
        desiredTrainee.LastName = trainee.LastName;
        desiredTrainee.Email = trainee.Email;
        desiredTrainee.TechStack = trainee.TechStack;
        desiredTrainee.Status = trainee.Status;

        return desiredTrainee;

    }

    public Trainee CreateTrainee(Trainee trainee)
    {
        trainee.Id = _nextId++;

        trainee.CreatedDate = DateTime.Now;
        trainee.UpdatedDate = DateTime.Now;

        _trainee.Add(trainee);

        return trainee;

    }

    public bool DeleteTrainee(int id)
    {
        var trainee = GetTraineeById(id);

        if(trainee == null)
        {
            return false;
        }

        _trainee.Remove(trainee);

        return true;

    }

}