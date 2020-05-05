﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UniLink.Dependencies.Data.VO;

namespace UniLink.API.Business.Interfaces
{
	public interface IDisciplineBusiness
	{
		Task<DisciplineVO> AddTaskAsync(DisciplineVO discipline);

		Task<IList<DisciplineVO>> FindDisciplinesTaskAsync(string disciplines);

		Task<IList<DisciplineVO>> FindByCourseIdTaskAsync(Guid courseId);

		Task<DisciplineVO> UpdateTaskAync(DisciplineVO newDiscipline);

		Task DeleteTaskAsync(Guid discipline);
	}
}