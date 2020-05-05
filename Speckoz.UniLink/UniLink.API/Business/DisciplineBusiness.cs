﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UniLink.API.Business.Interfaces;
using UniLink.API.Data.Converters;
using UniLink.API.Repository.Interfaces;
using UniLink.API.Utils;
using UniLink.Dependencies.Data.VO;
using UniLink.Dependencies.Models;

namespace UniLink.API.Business
{
	public class DisciplineBusiness : IDisciplineBusiness
	{
		private readonly DisciplineConverter _disciplineConverter;
		private readonly IDisciplineRepository _disciplineRepository;

		public DisciplineBusiness(IDisciplineRepository disciplineRepository)
		{
			_disciplineRepository = disciplineRepository;
			_disciplineConverter = new DisciplineConverter();
		}

		public Task<DisciplineVO> AddTaskAsync(DisciplineVO discipline) => throw new NotImplementedException();

		public async Task<IList<DisciplineVO>> FindByCourseIdTaskAsync(Guid courseId) =>
			_disciplineConverter.ParseList(await _disciplineRepository.FindDisciplinesByCourseIdTaskAsync(courseId));

		public async Task<IList<DisciplineVO>> FindDisciplinesTaskAsync(string disciplines)
		{
			if (GuidFormat.TryParseList(disciplines, ';', out IList<Guid> result))
				if (await _disciplineRepository.FindByRangeIdTaskAsync(result) is IList<DisciplineModel> disc)
					if (!disc.Contains(null))
						return _disciplineConverter.ParseList(disc);

			return null;
		}

		public Task<DisciplineVO> UpdateTaskAync(DisciplineVO newDiscipline) => throw new NotImplementedException();

		public Task DeleteTaskAsync(Guid discipline) => throw new NotImplementedException();
	}
}