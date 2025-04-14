using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;

namespace Service;

public class ParticipantService : I_ParticipantService
{
    private I_ParticipantDBRepository participantRepository;

    public ParticipantService(I_ParticipantDBRepository participantRepository)
    {
        this.participantRepository = participantRepository;
    }

    public Participant findByID(long id)
    {
        return participantRepository.findById(id);
    }

    public IEnumerable<Participant> getAll()
    {
        return participantRepository.getAll();
    }

    public void add(Participant entity)
    { 
        participantRepository.Add(entity);
    }

    public void delete(long id)
    {
        participantRepository.Remove(id);
    }

    public void update(long id, Participant entity)
    {
        participantRepository.Update(id, entity);
    }
}