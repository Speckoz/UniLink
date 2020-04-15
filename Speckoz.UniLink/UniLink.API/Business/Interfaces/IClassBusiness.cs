﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniLink.Dependencies.Enums;
using UniLink.Dependencies.Models;

namespace UniLink.API.Business.Interfaces
{
    public interface IClassBusiness
    {
		Task<ClassModel> FindByIdTaskAsync(Guid classId);

		Task<ClassModel> FindByURITaskAsync(string uri);

		Task<ClassModel> FindByDateTaskAsync(DateTime dateTime, ClassShiftEnum classShift);

		Task<ClassModel> FindByCourseTaskAsync(string course, byte period);

		Task<ClassModel> AddTaskAsync(ClassModel @class);

		Task<ClassModel> UpdateTaskAsync(ClassModel @class);

		Task DeleteTaskAsync(Guid classId);
	}
}
