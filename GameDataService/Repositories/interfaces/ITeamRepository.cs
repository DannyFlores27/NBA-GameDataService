using GameDataService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameDataService.Repositories.interfaces
{
	public interface ITeamRepository
	{
		Task<IEnumerable<Team>> GetAll();
		Task<Team?> GetById(int id);
		Task<Team> Add(Team team);
		Task<Team?> Update(Team team);
		Task<bool> Delete(int id);
	}
}
